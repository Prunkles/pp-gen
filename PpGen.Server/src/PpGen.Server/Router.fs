module PpGen.Server.Router

open System
open System.Net.WebSockets
open System.Threading

open System.Reactive.Linq
open FSharp.Control.Reactive

open Saturn
open Giraffe

open FSharp.Control.Tasks.V2

open PpGen.Api


type OptionBuilder() =
    member _.Return(x) = Some x
    member _.Bind(x, f) = Option.bind f x
    member _.MergeSources(x1, x2) = match x1, x2 with | Some x1, Some x2 -> Some (x1, x2) | _ -> None
[<AutoOpen>]
module OptionBuilderImpl =
    let option = OptionBuilder()


module DiamondSquare =

    open PpGen.Api.Fabrics
    
    let sendChunks (chunks: IObservable<Chunk>) : HttpHandler =
        fun next context -> task {
            let! ws = context.WebSockets.AcceptWebSocketAsync()
            
            use wh = new EventWaitHandle(false, EventResetMode.ManualReset)
            let subscription =
                chunks
                |> Observable.map ^fun chunk ->
                    let data =
                        Array.concat [
                            BitConverter.GetBytes(chunk.Width)
                            BitConverter.GetBytes(chunk.Height)
                            chunk.Heights |> Array.collect BitConverter.GetBytes
                        ]
                    
                    let buffer = ArraySegment(data)
                    let work () = task {
                        do! ws.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None)
                    }
                    Observable.FromAsync(work)
                |> Observable.concatInner
                |> Observable.subscribeWithCallbacks ignore raise
                    (fun () ->
                        ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None) |> ignore
                        wh.Set() |> ignore
                    )
            wh.WaitOne() |> ignore
            subscription.Dispose()
            
            return Some context
        }
    
    let generate: HttpHandler =
        fun next context -> task {
            let seed, size, cx, cy =
                option {
                    let! seed = context.TryGetQueryStringValue("seed")
                    and! size = context.TryGetQueryStringValue("size")
                    and! cx = context.TryGetQueryStringValue("cx")
                    and! cy = context.TryGetQueryStringValue("cy")
                    let seed = UInt64.Parse(seed)
                    let size = Byte.Parse(size)
                    let cx = Int32.Parse(cx)
                    let cy = Int32.Parse(cy)
                    return seed, size, cx, cy
                }
                |> Option.get
            
            let heightmapGenerator = context.GetService<IDiamondSquareHeightmapGeneratorFabric>().Create(seed, size)
            let! chunks = heightmapGenerator.GenerateChunk(cx, cy)
            
            return! sendChunks chunks next context
        }
    
    let generateInterpolated: HttpHandler =
        fun next context -> task {
            let seed, size, cx, cy =
                option {
                    let! seed = context.TryGetQueryStringValue("seed")
                    and! size = context.TryGetQueryStringValue("size")
                    and! cx = context.TryGetQueryStringValue("cx")
                    and! cy = context.TryGetQueryStringValue("cy")
                    let seed = UInt64.Parse(seed)
                    let size = Byte.Parse(size)
                    let cx = Int32.Parse(cx)
                    let cy = Int32.Parse(cy)
                    return seed, size, cx, cy
                }
                |> Option.get
            
            let heightmapGenerator = context.GetService<IDiamondSquareHeightmapGeneratorFabric>().CreateInterpolated(seed, size)
            let! chunks = heightmapGenerator.GenerateChunk(cx, cy)
            
            return! sendChunks chunks next context
        }
    
    let router = router {
        get "/alg/disq/generate" generate
        get "/alg/disq/generate/interp" generateInterpolated
    }

// ---------


//let handleDiSq: HttpHandler =
//    fun next context -> task {
//        let! ws = context.WebSockets.AcceptWebSocketAsync()
//        
//        let buffer = Array.zeroCreate 2048
//        let! result = ws.ReceiveAsync(Memory(buffer), CancellationToken.None)
//        let inputBuffer = Array.take result.Count buffer
//        
////        let json = Encoding.UTF8.GetString(buffer)
//        let args = context.GetService<IJsonSerializer>().Deserialize<DiamondSquare.ChunkGenerationArgs>(inputBuffer)
//        
//        let cx, cy = args.ChunkX, args.ChunkY
//        let seed = args.Seed
//        let s = args.ChunkSizeLog2
//
//        let points = Generator.generateReactive s (cx, cy) (noise seed)
//        
//        use wh = new EventWaitHandle(false, EventResetMode.ManualReset)
//        
//        context.GetLogger().LogInformation("Starting")
//        
//        let unsub =
//            points
//            |> Observable.map (fun ((x, y), h) ->
//                let data =
//                    let span = ReadOnlySpan<byte>()
//                    let memory = Memory<byte>()
//                    span.CopyTo(memory.Span)
//                    
//                    [|
//                    yield! BitConverter.GetBytes(x)
//                    yield! BitConverter.GetBytes(y)
//                    yield! BitConverter.GetBytes(h)
//                |]
//                
//                let buffer = ArraySegment(data)
//                let work () = task {
//                    do! ws.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None)
//                }
//                System.Reactive.Linq.Observable.FromAsync(work)
//            )
//            |> Observable.concatInner
//            |> Observable.subscribeWithCallbacks
//                (fun () -> ())
//                raise
//                (fun () ->
//                    ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None) |> ignore
//                    wh.Set() |> ignore
//                )
//        
//        wh.WaitOne() |> ignore
//        unsub.Dispose()
//        
//        context.GetLogger().LogInformation("Finished")
//        
//        return Some context
//    }

// ---------------------


//let handleDiSqInterpolate: HttpHandler =
//    fun next context -> task {
//        let! ws = context.WebSockets.AcceptWebSocketAsync()
//        
//        let buffer = Array.zeroCreate 2048
//        let! result = ws.ReceiveAsync(Memory(buffer), CancellationToken.None)
//        let inputBuffer = Array.take result.Count buffer
//        
//        let args = context.GetService<IJsonSerializer>().Deserialize<DiamondSquare.ChunkGenerationArgs>(inputBuffer)
//        
//        let cx, cy = args.ChunkX, args.ChunkY
//        let seed = args.Seed
//        let s = args.ChunkSizeLog2
//        let cs = pown 2 (int s)
//
//        let points = Generator.generateInterpolated s (cx, cy) (noise seed)
//        
//        use wh = new EventWaitHandle(false, EventResetMode.ManualReset)
//        
//        context.GetLogger().LogInformation(sprintf "Starting generate chunk (%i, %i)C" cx cy)
//        
//        let subscription =
//            points
//            |> Observable.map (fun chunk ->
//                
//                let points =
//                    chunk.Data
//                    |> fun x -> x.[0 .. cs-1, 0 .. cs-1]
//                    |> Seq.cast<float>
//                    |> Seq.map BitConverter.GetBytes
//                    |> Seq.concat
//                    |> Seq.toArray
//                let buffer = ArraySegment(points)
//                let work () = task {
//                    do! ws.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None)
//                }
//                System.Reactive.Linq.Observable.FromAsync(work)
//            )
//            |> Observable.concatInner
//            |> Observable.subscribeWithCallbacks
//                (fun () -> ())
//                raise
//                (fun () ->
//                    ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None) |> ignore
//                    wh.Set() |> ignore
//                )
//        
//        wh.WaitOne() |> ignore
//        subscription.Dispose()
//        
//        context.GetLogger().LogInformation("Finished")
//        
//        return Some context
//    }


//let api = pipeline {
//    plug acceptJson
//    set_header "x-pipeline-type" "Api"
//}
//
//let apiRouter = router {
//    not_found_handler (text "Api 404")
//    pipe_through api
//
//    pipe_through remotingApiHandler
//}

//let appRouter = router {
//    pipe_through remotingApiHandler
////    forward "/api" apiRouter
//}