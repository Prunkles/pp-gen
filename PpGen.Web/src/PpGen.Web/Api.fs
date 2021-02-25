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
        let ws = WebSocket.Create("ws://localhost:8085/api/disq/stream")
        
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
            let data: int32[] = !!JS.Constructors.Int32Array.Create(event.data?slice(0, 8) : obj)
            let x = data.[0]
            let y = data.[1]
            let data: float[] = !!JS.Constructors.Float64Array.Create(event.data?slice(8, 16) : obj)
            let h = data.[0]
            let point = { Height = h; X = x; Y = y }
            pointSubject.next(point)
        
        ws.onclose <- fun event ->
            printfn $"Chunk data at ({cx}, {cy})C received"
            pointSubject.complete()
        
        ws.onerror <- fun event ->
            pointSubject.error(Exception(event.``type``))
        
        return
            pointSubject
            |> fun x -> !!x: IRxObservable<Point>
            |> RxOp.bufferCount 1500 0
            |> RxOp.map seq //Seq.singleton
            :> IObservable<_>
    }


type Generator() =
    interface IGenerator with
        member this.GenerateChunk(var0) = failwith "todo"
        member this.GenerateChunkStream(args) = Generator.generateChunkStream args.Seed args.ChunkSizeLog2 args.ChunkX args.ChunkY


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


type MockGenerator() =
    member this.GenerateChunkStream(args) = (this :> IGenerator).GenerateChunkStream(args)
    interface IGenerator with
        member this.GenerateChunk(args) = failwith "todo"
        member this.GenerateChunkStream(args) = MockGenerator.generateChunkStream args.Seed args.ChunkSizeLog2 args.ChunkX args.ChunkY
