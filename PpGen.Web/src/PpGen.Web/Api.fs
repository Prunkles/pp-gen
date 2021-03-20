module PpGen.Web.Api

open Fable.Remoting.Client
open PpGen.Api
open PpGen.Api.DiamondSquare


let api =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder FableRemoting.Route.build
    |> Remoting.buildProxy<FableRemoting.IApi>


module Generator =

    open System
    open Browser
    open FSharp.Control
    open Fable.Core
    open Fable.Core.JsInterop
    open Fable.Import.RxJS
    
    let generateChunkStream seed s cx cy = async {
        let ws = WebSocket.Create("ws://10.0.0.7:8085/api/disq/stream")
        
        let pointSubject = JsTypes.Subject()
        
        ws.onopen <- fun event ->
            let args =
                 { Seed = seed
                   ChunkX = cx
                   ChunkY = cy
                   ChunkSizeLog2 = s }
            let json = JS.JSON.stringify(args)
            ws.send(json)
        
        ws.binaryType <- "arraybuffer"
        ws.onmessage <- fun event ->
            let data: JS.ArrayBuffer = !!event.data
            let values: int32[] = !!JS.Constructors.Int32Array.Create(data.slice(0, 8))
            let x = values.[0]
            let y = values.[1]
            let values: float[] = !!JS.Constructors.Float64Array.Create(data.slice(8, 16))
            let h = values.[0]
            let point = { Height = h; X = x; Y = y }
            pointSubject.next(point)
//            if point.X % 49 = 0 && point.Y = 0 then
//                printfn $"\"Drawn\" ({point.X}, {point.Y})"
            ()
        
        ws.onclose <- fun event ->
            printfn $"Chunk data at ({cx}, {cy})C received"
            pointSubject.complete()
        
        ws.onerror <- fun event ->
            pointSubject.error(Exception(event.``type``))
        
        return
            pointSubject
            |> fun x -> !!x: IRxObservable<Point>
            |> RxOp.bufferCount 10000 0
            |> RxOp.map seq
            :> IObservable<_>
    }
    
    let generateChunkInterpolated seed s cx cy : Async<IObservable<float[]>> = async {
        let ws = WebSocket.Create("ws://10.0.0.7:8085/api/disq/interpolated/stream")
        let cs = pown 2 (int s)
        let chunkSubject = JsTypes.Subject()
        
        ws.onopen <- fun event ->
            let args =
                 { Seed = seed
                   ChunkX = cx
                   ChunkY = cy
                   ChunkSizeLog2 = s }
            let json = JS.JSON.stringify(args)
            ws.send(json)
        
        ws.binaryType <- "arraybuffer"
        ws.onmessage <- fun event ->
            printf "Chunk shot received"
            let data: JS.ArrayBuffer = !!event.data
            let chunk: float[] = !!JS.Constructors.Float64Array.Create(data)
            chunkSubject.next(chunk)
        
        ws.onclose <- fun event ->
            printfn $"Chunk data at ({cx}, {cy})C received"
            chunkSubject.complete()
        
        ws.onerror <- fun event ->
            chunkSubject.error(Exception(event.``type``))
        
        return
            chunkSubject
            |> fun x -> !!x: IRxObservable<float[]>
            |> fun x -> x :> IObservable<_>
    }


type ServerGenerator() =
    interface IGenerator with
        member this.GenerateChunk(var0) = failwith "todo"
        member this.GenerateChunkStream(args) = Generator.generateChunkStream args.Seed args.ChunkSizeLog2 args.ChunkX args.ChunkY
        member this.GenerateChunkInterpolated(args) = Generator.generateChunkInterpolated args.Seed args.ChunkSizeLog2 args.ChunkX args.ChunkY


module MockGenerator =

    open System
    open Fable.Import.RxJS
    
    let generateChunkStream (seed: uint64) (s: uint) cx cy = async {
        let cs = pown 2 (int s)
        let points =
            Rx.of' (seq { 0 .. cs } |> Seq.toArray)
            |> RxOp.map ^fun y ->
                seq {
                    for x in 0 .. cs do
                        let h = (sin (float x / 10.) + cos (float y / 10.)) / 4. + 1.
                        let x, y = x + cx * cs, y + cy * cs
                        let point = { Height = h; X = x; Y = y }
                        yield point
                }
            :> IObservable<_>
        return points
    }


//type MockGenerator() =
//    member this.GenerateChunkStream(args) = (this :> IGenerator).GenerateChunkStream(args)
//    interface IGenerator with
//        member this.GenerateChunk(args) = failwith "todo"
//        member this.GenerateChunkStream(args) = MockGenerator.generateChunkStream args.Seed args.ChunkSizeLog2 args.ChunkX args.ChunkY



//module DirectGenerator =
//    
//    open System
//    open Fable.Core
//    open Fable.Import.RxJS
//    open PpGen.DiamondSquare
//    
//    let generateChunkStream (seed: uint64) (s: uint) cx cy = async {
//        let noise (x, y) =
//            sin (float x) * cos (float y)
//        let points = Generator.generateReactive s (cx, cy) noise
//        let points =
//            points
//            |> RxObservable.ofObservable
//            |> RxOp.map ^fun ((x, y), h) ->
//                Seq.singleton { Height = h; X = x; Y = y }
//        return points :> IObservable<_>
//    }


//type DirectGenerator() =
//    interface IGenerator with
//        member this.GenerateChunk(var0) = failwith "todo"
//        member this.GenerateChunkStream(var0) = DirectGenerator.generateChunkStream var0.Seed var0.ChunkSizeLog2 var0.ChunkX var0.ChunkY