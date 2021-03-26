module PpGen.Web.Api

open Fable.Remoting.Client
open PpGen.Api
open PpGen.Api.Fabrics



module HeightmapGenerator =
    let create generator = { new IHeightmapGenerator with member _.GenerateChunk(cx, cy) = generator cx cy }

module DiamondSquare =
    
    module Generator =

        open System
        open Browser
        open Browser.Types
        open Fable.Core
        open Fable.Core.JsInterop
        open Fable.Import.RxJS
        
        let receiveFromWebsocket (websocket: WebSocket) = async {
            let chunkSubject = JsTypes.Subject()
            
            websocket.binaryType <- "arraybuffer"
            websocket.onmessage <- fun event ->
                let data: JS.ArrayBuffer = !!event.data
                let values: int32[] = !!JS.Constructors.Int32Array.Create(data.slice(0, 8))
                let width = values.[0]
                let height = values.[1]
                let heights: float32[] = !!JS.Constructors.Float32Array.Create(data.slice(8))
                let chunk =
                    { Heights = heights
                      Height = height; Width = width }
                chunkSubject.next(chunk)
            
            websocket.onclose <- fun event ->
                chunkSubject.complete()
            
            websocket.onerror <- fun event ->
                chunkSubject.error(Exception(event.``type``))
            
            return chunkSubject :> IObservable<_>
        }
        
        let inline path2ws path =
            let loc = window.location
            let protocol = if loc.protocol = "https:" then "wss:" else "ws:"
            $"{protocol}//{loc.host}%s{path}"
        
        let create seed size =
            let generator cx cy = async {
                let path = $"/api/alg/disq/generate?seed={seed}&size={size}&cx={cx}&cy={cy}"
                let url = path2ws path
                let ws = WebSocket.Create(url)
                return! receiveFromWebsocket ws
            }
            HeightmapGenerator.create generator
        
        let createInterpolated seed size =
            let generator cx cy = async {
                let path = $"/api/alg/disq/generate/interp?seed={seed}&size={size}&cx={cx}&cy={cy}"
                let url = path2ws path
                let ws = WebSocket.Create(url)
                return! receiveFromWebsocket ws
            }
            HeightmapGenerator.create generator

type DiamondSquareHeightmapGeneratorFabric() =
    interface IDiamondSquareHeightmapGeneratorFabric with
        member _.Create(seed, size) = DiamondSquare.Generator.create seed size
        member _.CreateInterpolated(seed, size) = DiamondSquare.Generator.createInterpolated seed size
