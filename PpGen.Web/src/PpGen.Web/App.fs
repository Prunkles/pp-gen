module PpGen.Web.App

open System
open FSharp.Control
open Browser.Types
open Fable.Import.RxJS
open Feliz
open Fable.Import.RxJS.Misc

open PpGen.Api
open PpGen.Api.Fabrics
open PpGen.Web.Api
open PpGen.Web.Utils



[<ReactComponent>]
let Map (cs: int) (chunks: IObservable<int * int * Chunk>) (onGenerateChunk: (int * int) -> unit) =
    let ref = React.useRef(None: HTMLDivElement option)
    React.useEffect(fun () ->
        let disposable, domElement = Stage.configureStage cs chunks onGenerateChunk
        ref.current.Value.appendChild(domElement) |> ignore
        disposable
    , [| |])
    
    Html.div [
        prop.ref ref
    ]


let s = 8
let cs = pown 2 s
let seed = 1UL

let heightmapGenerator: IHeightmapGenerator =
    (DiamondSquareHeightmapGeneratorFabric() :> IDiamondSquareHeightmapGeneratorFabric).CreateInterpolated(seed, byte s)


[<ReactComponent>]
let App () =
    let chunkSubject =
        React.useMemo(
            fun () -> JsTypes.Subject()
            , [| |]
        )
    
    let onGenerateChunk (cx, cy) =
        async {
            let! chunks = heightmapGenerator.GenerateChunk(cx, cy)
            
            let subscription =
                chunks.Subscribe(Observer.create
                    (fun chunk -> chunkSubject.next((cx, cy, chunk)))
                    (fun err -> chunkSubject.error(err))
                    (fun () -> printfn $"Chunk ({cx}, {cy})C completed")
                )
            ignore subscription
            printfn $"Request chunk generation at ({cx}, {cy})C"
        } |> Async.StartImmediate
    
    React.useEffectOnce(fun () ->
        onGenerateChunk (1, 1)
        onGenerateChunk (1, 2)
        onGenerateChunk (2, 1)
        onGenerateChunk (2, 2)
    )
    
    Map cs chunkSubject onGenerateChunk
