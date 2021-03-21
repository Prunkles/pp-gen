module PpGen.Web.App

open System
open System.Collections.Generic
open FSharp.Control
open Fable.Core
open Fable.Core.JsInterop
open Browser
open Browser.Types
open Fable.Import.ThreeJs.Examples.Jsm.Controls
open Fable.Import.ThreeJs.Exports
open Feliz

open PpGen
open PpGen.Api.DiamondSquare
open PpGen.Web.Api
open PpGen.Web.Utils
open PpGen.Utils
open Euclidean.Operators

let palette = Palettes.lefebrve2

let regionSize = 1000

type IHeightmapRenderer =
    abstract RenderRect: x: int * y: int * w: int * h: int * heights: float[] -> unit


type Stage() =
    
    let width, height = 1024., 720.
    
    let scene = THREE.Scene.Create()
    
    let renderer = THREE.WebGLRenderer.Create()
    do
        renderer.setSize(width, height)
        renderer.setClearColor(!^"#061639")
    
    let camera = THREE.PerspectiveCamera.Create(90., width / height, 1., 10000.)
    do camera.position.set(0., 500., 0.) |> ignore
    
    let controls = OrbitControls(camera, renderer.domElement)
    do
        controls.enableDamping <- true
        controls.dampingFactor <- 0.05
        controls.maxPolarAngle <- Math.PI / 2.1
        controls.screenSpacePanning <- false
    
    member _.Scene = scene
    member _.Renderer = renderer
    member _.Camera = camera
    
    member _.Render() =
        controls.update()
        renderer.render(scene, camera)
    
    member _.DomElement = renderer.domElement
    


module Scene =
    
    type Region =
        { CanvasContext: CanvasRenderingContext2D
          Mesh: Fable.Import.ThreeJs.Types.__objects_Mesh.Mesh<Fable.Import.ThreeJs.Types.__geometries_PlaneGeometry.PlaneGeometry,Fable.Import.ThreeJs.Types.__materials_MeshBasicMaterial.MeshBasicMaterial> }
    
    type ThreeHeightmapRenderer(stage: Stage) =
        let resources = ResizeArray<obj>()
        
        let regions: IDictionary<int * int, Region> = upcast Dictionary()
        
        let getRegion rx ry =
            let createRegion rx ry =
                printfn $"Create new region at ({rx}, {ry})R"
                let canvas = document.createElement("canvas") :?> HTMLCanvasElement
                canvas.width <- float regionSize
                canvas.height <- float regionSize
                let canvasTexture = THREE.CanvasTexture.Create(!^canvas)
                canvasTexture.magFilter <- THREE.NearestFilter
                let material = THREE.MeshBasicMaterial.Create()
                material.map <- Some !!canvasTexture
                material.transparent <- true
                let planeGeometry = THREE.PlaneGeometry.Create(float regionSize, float regionSize)
                let mesh = THREE.Mesh.Create(planeGeometry, material)
                mesh.rotation.set(deg2rad -90., 0., 0.) |> ignore
                mesh.position.set(
                    float rx * float regionSize + float regionSize / 2.,
                    0.0,
                    float ry * float regionSize + float regionSize / 2.
                ) |> ignore
                
                let ctx = canvas.getContext_2d()
                ctx?webkitImageSmoothingEnabled <- false
                ctx?imageSmoothingEnabled <- false
                
                let region = { CanvasContext = ctx; Mesh = mesh }
                regions.[(rx, ry)] <- region
                stage.Scene.add(!![| mesh |]) |> ignore
                resources.AddRange([canvasTexture; material; planeGeometry])
                
                region
            
            match regions.TryGetValue((rx, ry)) with
            | true, region -> region
            | false, _ -> createRegion rx ry
        
        let drawRect rectX0 rectY0 rectW rectH (heights: float[]) =
            let rs = regionSize
            let rectX1, rectY1 = rectX0 + rectW - 1, rectY0 + rectH - 1
            let regionX0, regionY0 = rectX0 /! rs, rectY0 /! rs
            let regionX1, regionY1 = rectX1 /! rs, rectY1 /! rs
            for rx in regionX0 .. regionX1 do
                for ry in regionY0 .. regionY1 do
                    let region = getRegion rx ry
                    let intX0, intY0 = max (rx * rs) rectX0, max (ry * rs) rectY0
                    let intX1, intY1 = min ((rx + 1) * rs - 1) rectX1, min ((ry + 1) * rs - 1) rectY1
                    let intW, intH = intX1 - intX0 + 1, intY1 - intY0 + 1
                    let imageData = region.CanvasContext.createImageData(intW |> float, intH |> float)
                    for x in intX0 .. intX1 do
                        for y in intY0 .. intY1 do
                            let imageIdx = (y - intY0) * intW + (x - intX0) // transposed
                            let heightIdx = (x - rectX0) * rectH + (y - rectY0)
                            let h = heights.[heightIdx]
                            let h = (h / 4.) + 0.5
                            let (Rgb (r, g, b)) = Palette.pick palette h
                            imageData.data.[imageIdx * 4 + 0] <- r
                            imageData.data.[imageIdx * 4 + 1] <- g
                            imageData.data.[imageIdx * 4 + 2] <- b
                            imageData.data.[imageIdx * 4 + 3] <- 0xFFuy
                    region.CanvasContext.putImageData(imageData, intX0 %! rs |> float, intY0 %! rs |> float)
                    region.Mesh.material.needsUpdate <- true
                    region.Mesh.material.map.Value.needsUpdate <- true
        
        interface IHeightmapRenderer with
            member this.RenderRect(x, y, w, h, heights) = drawRect x y w h heights
        
        interface IDisposable with
            member _.Dispose() = resources |> Seq.iter ^fun r -> r?dispose()
        

type ChunkSelection(stage: Stage, cs, onGenerateChunk) =
    let resources = ResizeArray<obj>()
    
    let chunkSelection =
        let material = THREE.LineBasicMaterial.Create()
        material.color <- THREE.Color.Create(!^"#00F")
        material.depthTest <- false
        let planeGeometry = THREE.PlaneGeometry.Create(float cs + 1., float cs + 1.)
        let edgeGeometry = THREE.EdgesGeometry.Create(planeGeometry)
        let mesh = THREE.LineSegments.Create(edgeGeometry, material)
        mesh.renderOrder <- 1000.
        mesh.rotation.set(deg2rad -90., 0., 0.) |> ignore
        resources.AddRange([planeGeometry; edgeGeometry; material])
        mesh
    
    do stage.Scene.add(!![|chunkSelection|]) |> ignore

    let mutable chunkSelectionCoords = Unchecked.defaultof<_>
    
    let mouse = THREE.Vector2.Create()
    do 
        window.addEventListener(
            "mousemove",
            !!(fun (event: MouseEvent) ->
                let rect = stage.Renderer.domElement.getBoundingClientRect()
                let mxs, mys =
                    (event.clientX - rect?x) / rect.width * 2. - 1.,
                    (event.clientY - rect?y) / rect.height * 2. - 1. |> (~-)
                mouse.set(mxs, mys) |> ignore
            )
        )
        
        document.addEventListener(
            "keydown",
            !!(fun (event: KeyboardEvent) ->
                if event.key = "g" then onGenerateChunk chunkSelectionCoords
            )
        )
    
    let raycaster = THREE.Raycaster.Create()
    
    member _.Move(cx, cy) =
        chunkSelectionCoords <- (cx, cy)
        let x, z =
            float cx * float cs + float cs / 2. + 0.5,
            float cy * float cs + float cs / 2. + 0.5
        chunkSelection.position.set(x, 0., z) |> ignore
    
    member this.Render(camera) =
        raycaster.setFromCamera(!!mouse, camera)
        let intersections = raycaster.intersectObjects(stage.Scene.children)
        intersections
        |> Seq.iter ^fun intersection ->
            let x, z = intersection.point.x, intersection.point.z
            let cx, cy = x /! float cs |> int, z /! float cs |> int
            this.Move(cx, cy)
        ()
    
    member _.Visible
        with get() = chunkSelection.visible
        and set(value) = chunkSelection.visible <- value
    
    interface IDisposable with
        member _.Dispose() = resources |> Seq.iter ^fun r -> r?dispose()


let configureStage cs points onGenerateChunk =
    let stage = Stage()
    let heightmapRenderer = new Scene.ThreeHeightmapRenderer(stage)
    let subscription =
        points
        |> Observable.subscribe ^fun (cx, cy, heights) ->
            let x, y = cx * cs, cy * cs
            (heightmapRenderer :> IHeightmapRenderer).RenderRect(x, y, cs, cs, heights)
    
    let chunkSelection = new ChunkSelection(stage, cs, onGenerateChunk)
    
    let rec render time =
        stage.Render()
        chunkSelection.Render(stage.Camera)
        window.requestAnimationFrame(render) |> ignore
    do render 0.
    
    { new IDisposable with
        member _.Dispose() =
            subscription.Dispose()
            (heightmapRenderer :> IDisposable).Dispose()
            (chunkSelection :> IDisposable).Dispose()
    }, stage.DomElement

[<ReactComponent>]
let Map (cs: int) (points: IObservable<int * int * float[]>) (onGenerateChunk: (int * int) -> unit) =
    let ref = React.useRef(None: HTMLDivElement option)
    React.useEffect(fun () ->
        let disposable, domElement = configureStage cs points onGenerateChunk
        ref.current.Value.appendChild(domElement) |> ignore
        disposable
    , [| |])
    
    Html.div [
        prop.ref ref
    ]

let generator: IGenerator =
//    upcast MockGenerator()
    upcast ServerGenerator()
//    upcast DirectGenerator()


let s = 8
let cs = pown 2 s
let cx = 4
let cy = 1
let scale = 1
let seed = 1UL

let args = { ChunkX = cx; ChunkY = cy; Seed = 1UL; ChunkSizeLog2 = uint s }

open Fable.Import.RxJS.Misc

[<ReactComponent>]
let App () =
    let chunkSubject =
        React.useMemo(
            fun () ->
                Fable.Import.RxJS.JsTypes.Subject()
            , [| |]
        )
    
    let onGenerateChunk (cx, cy) =
        async {
            let! chunk = generator.GenerateChunkInterpolated({ args with ChunkX = cx; ChunkY = cy })
            
            let subscription =
                chunk.Subscribe(Observer.create
                    (fun chunk -> chunkSubject.next((cx, cy, chunk)))
                    (fun err -> chunkSubject.error(err))
                    (fun () -> printfn $"Chunk ({cx}, {cy})C completed")
                )
            ignore subscription
            printfn $"Request chunk generation at ({cx}, {cy})C"
        } |> Async.StartImmediate
    
    React.useEffectOnce(fun () ->
        onGenerateChunk (1, 3)
//        onGenerateChunk (7, 7)
//        onGenerateChunk (1, 1)
//        onGenerateChunk (2, 2)
//        onGenerateChunk (1, 2)
    )
    
    Map cs chunkSubject onGenerateChunk
