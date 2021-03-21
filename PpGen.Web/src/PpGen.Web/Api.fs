module PpGen.Web.Api

open Fable.Remoting.Client
open PpGen.Api
open PpGen.Api.Fabrics



//module Generator =
//
//    open System
//    open Browser
//    open FSharp.Control
//    open Fable.Core
//    open Fable.Core.JsInterop
//    open Fable.Import.RxJS
//    
//    let generateChunkStream seed s cx cy = async {
//        let ws = WebSocket.Create("ws://10.0.0.7:8085/api/disq/stream")
//        
//        let pointSubject = JsTypes.Subject()
//        
//        ws.onopen <- fun event ->
//            let args =
//                 { Seed = seed
//                   ChunkX = cx
//                   ChunkY = cy
//                   ChunkSizeLog2 = s }
//            let json = JS.JSON.stringify(args)
//            ws.send(json)
//        
//        ws.binaryType <- "arraybuffer"
//        ws.onmessage <- fun event ->
//            let data: JS.ArrayBuffer = !!event.data
//            let values: int32[] = !!JS.Constructors.Int32Array.Create(data.slice(0, 8))
//            let x = values.[0]
//            let y = values.[1]
//            let values: float[] = !!JS.Constructors.Float64Array.Create(data.slice(8, 16))
//            let h = values.[0]
//            let point = { Height = h; X = x; Y = y }
//            pointSubject.next(point)
////            if point.X % 49 = 0 && point.Y = 0 then
////                printfn $"\"Drawn\" ({point.X}, {point.Y})"
//            ()
//        
//        ws.onclose <- fun event ->
//            printfn $"Chunk data at ({cx}, {cy})C received"
//            pointSubject.complete()
//        
//        ws.onerror <- fun event ->
//            pointSubject.error(Exception(event.``type``))
//        
//        return
//            pointSubject
//            |> fun x -> !!x: IRxObservable<Point>
//            |> RxOp.bufferCount 10000 0
//            |> RxOp.map seq
//            :> IObservable<_>
//    }
//    
//    let generateChunkInterpolated seed s cx cy : Async<IObservable<float[]>> = async {
//        let ws = WebSocket.Create("ws://10.0.0.7:8085/api/disq/interpolated/stream")
//        let cs = pown 2 (int s)
//        let chunkSubject = JsTypes.Subject()
//        
//        ws.onopen <- fun event ->
//            let args =
//                 { Seed = seed
//                   ChunkX = cx
//                   ChunkY = cy
//                   ChunkSizeLog2 = s }
//            let json = JS.JSON.stringify(args)
//            ws.send(json)
//        
//        ws.binaryType <- "arraybuffer"
//        ws.onmessage <- fun event ->
//            printf "Chunk shot received"
//            let data: JS.ArrayBuffer = !!event.data
//            let chunk: float[] = !!JS.Constructors.Float64Array.Create(data)
//            chunkSubject.next(chunk)
//        
//        ws.onclose <- fun event ->
//            printfn $"Chunk data at ({cx}, {cy})C received"
//            chunkSubject.complete()
//        
//        ws.onerror <- fun event ->
//            chunkSubject.error(Exception(event.``type``))
//        
//        return
//            chunkSubject
//            |> fun x -> !!x: IRxObservable<float[]>
//            |> fun x -> x :> IObservable<_>
//    }

module HeightmapGenerator =
    let create generator = { new IHeightmapGenerator with member _.GenerateChunk(cx, cy) = generator cx cy }

module DiamondSquare =
    
    module Generator =

        open System
        open Browser
        open Fable.Core
        open Fable.Core.JsInterop
        open Fable.Import.RxJS
        
        let create seed size =
            let generator cx cy = async {
                let ws = WebSocket.Create($"ws://10.0.0.7:8085/alg/disq/generate?seed={seed}&size={size}&cx={cx}&cy={cy}")
                
                let chunkSubject = JsTypes.Subject()
                
                ws.binaryType <- "arraybuffer"
                ws.onmessage <- fun event ->
                    let data: JS.ArrayBuffer = !!event.data
                    let values: int32[] = !!JS.Constructors.Int32Array.Create(data.slice(0, 8))
                    let width = values.[0]
                    let height = values.[1]
                    let heights: float[] = !!JS.Constructors.Float64Array.Create(data.slice(8))
                    let chunk =
                        { Heights = heights
                          Height = height; Width = width }
                    chunkSubject.next(chunk)
                
                ws.onclose <- fun event ->
                    printfn $"Chunk data at ({cx}, {cy})C received"
                    chunkSubject.complete()
                
                ws.onerror <- fun event ->
                    chunkSubject.error(Exception(event.``type``))
                
                return chunkSubject :> IObservable<_>
            }
            HeightmapGenerator.create generator
        
        let createInterpolated seed size =
            let generator cx cy = async {
                let ws = WebSocket.Create($"ws://10.0.0.7:8085/alg/disq/generate/interp?seed={seed}&size={size}&cx={cx}&cy={cy}")
                
                let chunkSubject = JsTypes.Subject()
                
                ws.binaryType <- "arraybuffer"
                ws.onmessage <- fun event ->
                    let data: JS.ArrayBuffer = !!event.data
                    let values: int32[] = !!JS.Constructors.Int32Array.Create(data.slice(0, 8))
                    let width = values.[0]
                    let height = values.[1]
                    let heights: float[] = !!JS.Constructors.Float64Array.Create(data.slice(8))
                    let chunk =
                        { Heights = heights
                          Height = height; Width = width }
                    chunkSubject.next(chunk)
                
                ws.onclose <- fun event ->
                    printfn $"Chunk data at ({cx}, {cy})C received"
                    chunkSubject.complete()
                
                ws.onerror <- fun event ->
                    chunkSubject.error(Exception(event.``type``))
                
                return chunkSubject :> IObservable<_>
            }
            HeightmapGenerator.create generator

type DiamondSquareHeightmapGeneratorFabric() =
    interface IDiamondSquareHeightmapGeneratorFabric with
        member _.Create(seed, size) = DiamondSquare.Generator.create seed size
        member _.CreateInterpolated(seed, size) = DiamondSquare.Generator.createInterpolated seed size
