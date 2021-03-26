module PpGen.Web.App

open System
open Browser
open FSharp.Control
open Browser.Types
open Fable.Import.RxJS
open Feliz
open Feliz.Bulma

open PpGen.Api
open PpGen.Api.Fabrics
open PpGen.Web.Api
open PpGen.Web.Utils
open PpGen.Web.Gui


[<ReactComponent>]
let Stage (cs: int) (chunks: IObservable<int * int * Chunk>) palette (onGenerateChunk: (int * int) -> unit) =
    let ref = React.useRef(None: HTMLElement option)
    React.useEffect(fun () ->
        let disposable, domElement = Stage.configureStage cs chunks palette onGenerateChunk
        ref.current.Value.appendChild(domElement) |> ignore
        Disposable.concat [
            disposable
            Disposable.create ^fun () -> ref.current.Value.removeChild(domElement) |> ignore
        ]
    , [| box cs; box palette; box onGenerateChunk; box chunks |])
    
    Html.div [
        prop.id "stage"
        prop.ref ref
    ]


[<ReactComponent>]
let App () =
    let guiProps, setGuiProps = React.useState(fun () -> GuiProps.Default)
    let seed = guiProps.Seed
    let palette = guiProps.Palette
    let algorithm = guiProps.Algorithm
    
    let heightmapGenerator =
        React.useMemo(
            fun () ->
                match algorithm with
                | AlgorithmProps.DiamondSquare algorithm ->
                    (DiamondSquareHeightmapGeneratorFabric() :> IDiamondSquareHeightmapGeneratorFabric)
                        .CreateInterpolated(seed, byte algorithm.Size)
                | AlgorithmProps.PerlinNoise props ->
                    let generator cx cy = async {
                        let path =
                            $"/api/alg/perlin/generate?seed={seed}&width={props.Size}&height={props.Size}&gx={cx*props.Size}&gy={cy*props.Size}"
                        let url = DiamondSquare.Generator.path2ws path
                        let ws = WebSocket.Create(url)
                        return! DiamondSquare.Generator.receiveFromWebsocket ws
                    }
                    HeightmapGenerator.create generator
            , [| box seed; box algorithm |]
        )
    
    let chunkSubject = React.useMemo((fun () -> printfn "Create chunk subject"; JsTypes.Subject()), [| box guiProps |])
    
    let generateChunk =
        React.useCallback(
            fun (cx, cy) ->
                async {
                    let! chunks = heightmapGenerator.GenerateChunk(cx, cy)
                    // Используем костыль автоотписки максимально на скорую руку
                    let mutable sub = Unchecked.defaultof<_>
                    sub <- chunks.Subscribe(Observer.create
                            (fun chunk -> chunkSubject.next((cx, cy, chunk)))
                            (fun err -> chunkSubject.error(err))
                            (fun () -> printfn $"Chunk ({cx}, {cy})C completed"; (sub :> IDisposable).Dispose())
                        )
                    printfn $"Request chunk generation at ({cx}, {cy})C"
                } |> Async.StartImmediate
            , [| box heightmapGenerator; box chunkSubject |]
        )
    
    React.useEffect(fun () ->
        generateChunk (1, 1)
    , [| box guiProps |])
    
    
    Html.div [
        Bulma.columns [
            Bulma.column [
                column.is9
                prop.style [
                    style.padding 0
                ]
                prop.children [
                    match algorithm with
                    | AlgorithmProps.DiamondSquare algorithm ->
                        let cs = pown 2 algorithm.Size
                        let stage = Stage cs chunkSubject palette generateChunk
                        stage
                    | AlgorithmProps.PerlinNoise props ->
                        let cs = props.Size
                        let stage = Stage cs chunkSubject palette generateChunk
                        stage
                ]
            ]
            Bulma.column [
                column.is3
                prop.children [
                    Gui guiProps setGuiProps
                ]
            ]
        ]
    ]
