module PpGen.Web.App

open System
open System.Collections.Generic
open FSharp.Control
open Fable.Core
open Fable.Core.JsInterop
open Browser
open Browser.Types
open Fable.Import.PixiJs
open Feliz

open PpGen
open PpGen.Api.DiamondSquare
open PpGen.Web.Api
open PpGen.Web.Utils
open PpGen.Utils


let palette = Palettes.lefebrve2

let regionSize = 2048


module Euclidean =
    
    let inline divEuclid x y =
        let q = x / y
        if x % y < LanguagePrimitives.GenericZero
        then
            if y > LanguagePrimitives.GenericZero
            then q - LanguagePrimitives.GenericOne
            else q + LanguagePrimitives.GenericOne
        else
            q
    
    let inline remEuclid x y =
        let r = x % y
        if r < LanguagePrimitives.GenericZero
        then
            if y < LanguagePrimitives.GenericZero
            then r - y
            else r + y
        else
            r
    
    module Operators =
        let inline ( /! ) lhs rhs = divEuclid lhs rhs
        let inline ( %! ) lhs rhs = remEuclid lhs rhs

open Euclidean.Operators


[<ReactComponent>]
let Map (cs: int) (points: IObservable<Point seq>) (onGenerateChunk: (int * int) -> unit) =
    let ref = React.useRef(None: HTMLDivElement option)
    React.useEffect(fun () ->
        
        let app = PIXI.Constructors.Application.Create(width=800., height=600.)
        ref.current.Value.appendChild(app.view) |> ignore
        
        let renderer: PIXI.WebGLRenderer = !!app.renderer
        renderer.backgroundColor <- 0x061639 |> float
        
        let regionContainer = PIXI.Constructors.Container.Create()
        app.stage.addChild(regionContainer) |> ignore
        
        regionContainer.interactive <- true
        regionContainer.on(!^"pointerdown", fun (event: PIXI.Interaction.InteractionEvent) ->
            let p = event.data.getLocalPosition(regionContainer)
            let cx = int p.x /! cs
            let cy = int p.y /! cs
            onGenerateChunk (cx, cy)
        ) |> ignore
        
        document.addEventListener("keypress", fun event ->
            let event = event :?> KeyboardEvent
            match event.code with
            | "KeyA" -> regionContainer.x <- regionContainer.x + 16.
            | "KeyD" -> regionContainer.x <- regionContainer.x - 16.
            | "KeyW" -> regionContainer.y <- regionContainer.y + 16.
            | "KeyS" -> regionContainer.y <- regionContainer.y - 16.
            | _ -> ()
        )
        
        let regions: IDictionary<int * int, CanvasRenderingContext2D * PIXI.Texture> = upcast Dictionary()
        
        let drawPoint (x: int) (y: int) (h: float) : unit =
            let rx, ry = x /! regionSize, y /! regionSize
            let ctx, texture =
                match regions.TryGetValue((rx, ry)) with
                | true, ctx -> ctx
                | false, _ ->
                    printfn $"Create new region at ({rx}, {ry})R"
                    let canvas = document.createElement("canvas") :?> HTMLCanvasElement
                    canvas.width <- float regionSize
                    canvas.height <- float regionSize
                    let texture = PIXI.Constructors.Texture.fromCanvas(canvas)
                    let sprite = PIXI.Constructors.Sprite.Create(texture)
                    sprite.x <- rx * regionSize |> float
                    sprite.y <- ry * regionSize |> float
                    regionContainer.addChild(sprite) |> ignore
                    
                    let ctx = canvas.getContext_2d()
                    ctx?webkitImageSmoothingEnabled <- false
                    ctx?imageSmoothingEnabled <- false
                    regions.[(rx, ry)] <- (ctx, texture)
                    ctx, texture
            
            let pxr, pyr = x %! regionSize, y %! regionSize
            
            let h = (h + 1.) / 3. |> max 0. |> min 1.
            let color = Palette.pick palette h
            
            let imageData = ctx.getImageData(float pxr, float pyr, 1., 1.)
            let (Rgb (r, g, b)) = color
            imageData.data.[0] <- r
            imageData.data.[1] <- g
            imageData.data.[2] <- b
            imageData.data.[3] <- 0xFFuy
            ctx.putImageData(imageData, float pxr, float pyr)
//            texture.update()
            if x %! 50 = 0 && y %! 50 = 0 then
                printfn $"Drawn point at ({x}, {y})P with ({r}, {g}, {b}) on ({rx}, {ry})R"
            ()
        
        let subscription =
            points
            |> Observable.subscribe (fun pointBuffer ->
                for point in pointBuffer do
//                    printfn $"{point.X}"
                    drawPoint point.X point.Y point.Height
                regions.Values |> Seq.iter (fun (_, tex) -> tex.update())
                printfn "Updated all textures"
            )
        
        subscription
    , [| |])
    
    Html.div [
        prop.ref ref
    ]

let generator: IGenerator =
//    upcast MockGenerator()
    upcast Generator()


let s = 7
let cs = pown 2 s
let cx = 4
let cy = 1
let scale = 1
let seed = 1UL

let args = { ChunkX = cx; ChunkY = cy; Seed = 1UL; ChunkSizeLog2 = uint s }

[<ReactComponent>]
let App () =
    let pointObv, points =
        React.useMemo(
            fun () ->
                let pointObv, points = AsyncRx.subject ()
                pointObv.ToObserver(), AsyncRx.toObservable points
            , [| |]
        )
    
    let onGenerateChunk (cx, cy) =
        async {
            let! points = generator.GenerateChunkStream({ args with ChunkX = cx; ChunkY = cy })
            let subscription =
                points.Subscribe(function
                    | OnNext x -> pointObv.OnNext(x)
                    | OnError e -> pointObv.OnError(e)
                    | OnCompleted -> printfn $"Chunk ({cx}, {cy})C completed"
                )
            ignore subscription
            printfn $"Request chunk generation at ({cx}, {cy})C"
        } |> Async.StartImmediate
    
    React.useEffectOnce(fun () ->
        onGenerateChunk (2, 2)
    )
    
    Map cs points onGenerateChunk
