module Router

open System
open System.Net.WebSockets
open System.Text
open System.Threading
open System.Threading
open Microsoft.Extensions.Logging
open Fable.Remoting.Giraffe
open Fable.Remoting.Server
open Giraffe.Serialization.Json
open PpGen.Api
open PpGen.DiamondSquare
open PpGen.Server
open Saturn
open Giraffe.Core
open FSharp.Control.Tasks.V2
open FSharp.Control.Reactive




let disqFableRemotingHandler: HttpHandler =
    Remoting.createApi ()
    |> Remoting.fromValue FableRemoting.api
    |> Remoting.withRouteBuilder FableRemoting.Route.build
    |> Remoting.buildHttpHandler


let generate seed cx cy s =
    
    let noise (x: int, y: int) =
        let input = uint64 x <<< 32 ||| uint64 y
        let bytes = BitConverter.GetBytes(input)
        let output = Standart.Hash.xxHash.xxHash64.ComputeHash(bytes, bytes.Length, seed)
        float output / float UInt64.MaxValue
    let cs = pown 2 (int s)
//    Generator.generate s (cx, cy) noise
//    |> Array2D.mapi (fun x y h -> (x + cx * cs, y + cy * cs), h)
//    |> Seq.cast<(int*int)*float>
//    |> Observable.ofSeq
    Generator.generateReactive s (cx, cy) noise
    
    

let handleDiSq: HttpHandler =
    fun next context -> task {
        let! ws = context.WebSockets.AcceptWebSocketAsync()
        
        let buffer = Array.zeroCreate 2048
        let! result = ws.ReceiveAsync(Memory(buffer), CancellationToken.None)
        let inputBuffer = Array.take result.Count buffer
        
//        let json = Encoding.UTF8.GetString(buffer)
        let args = context.GetService<IJsonSerializer>().Deserialize<DiamondSquare.ChunkGenerationArgs>(inputBuffer)
        
        let cx, cy = args.ChunkX, args.ChunkY
        let seed = args.Seed
        let s = args.ChunkSizeLog2

        let points = generate seed cx cy s
        
        use wh = new EventWaitHandle(false, EventResetMode.ManualReset)
        
        context.GetLogger().LogInformation("Starting")
        
        let unsub =
            points
            |> Observable.map (fun ((x, y), h) ->
                let data = [|
                    yield! BitConverter.GetBytes(x)
                    yield! BitConverter.GetBytes(y)
                    yield! BitConverter.GetBytes(h)
                |]
                let buffer = ArraySegment(data)
                let work () = task {
                    do! ws.SendAsync(buffer, WebSocketMessageType.Binary, true, CancellationToken.None)
                }
                System.Reactive.Linq.Observable.FromAsync(work)
            )
            |> Observable.concatInner
            |> Observable.subscribeWithCallbacks
                (fun () -> ())
                raise
                (fun () ->
                    ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None) |> ignore
                    wh.Set() |> ignore
                )
        
        wh.WaitOne() |> ignore
        unsub.Dispose()
        
        context.GetLogger().LogInformation("Finished")
        
        return Some context
    }

let disqRouter = router {
    get "/api/disq/stream" handleDiSq
}


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