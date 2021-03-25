module PpGen.Web.Stage

open System
open System.Collections.Generic
open FSharp.Control
open Fable.Core
open Fable.Core.JsInterop
open Browser
open Browser.Types
open Fable.Import.RxJS
open Fable.Import.ThreeJs.Examples.Jsm.Controls
open Fable.Import.ThreeJs.Exports

open Fable.Import.ThreeJs.Types.__geometries_PlaneGeometry
open Fable.Import.ThreeJs.Types.__helpers_HemisphereLightHelper
open Fable.Import.ThreeJs.Types.__helpers_PlaneHelper
open Fable.Import.ThreeJs.Types.__math_Plane
open Fable.Import.ThreeJs.Types.__math_Ray
open Fable.Import.ThreeJs.Types.__objects_Mesh
open Fable.Import.ThreeJs.Types.__renderers_WebGLRenderer
open Fable.Import.ThreeJs.Types.__helpers_GridHelper
open Fable.Import.DatGui.Exports
open Fable.Import.DatGui.Types
open PpGen
open PpGen.Api
open PpGen.Web.Utils
open PpGen.Utils
open Euclidean.Operators


type IHeightmapRenderer =
    abstract RenderRect: x: int * y: int * w: int * h: int * heights: float[] -> unit


type Stage(canvas: HTMLCanvasElement) =
    
    let scene = THREE.Scene.Create()
    
    let renderer = THREE.WebGLRenderer.Create(!!{| canvas = canvas |})
    do
        renderer.setClearColor(!^"#061639")
    
    let camera =
        let aspect = renderer.domElement.clientWidth / renderer.domElement.clientHeight
        THREE.PerspectiveCamera.Create(90., aspect, 1., 10000.)
    do camera.position.set(0., 500., 0.) |> ignore
    
    let controls = OrbitControls(camera, renderer.domElement)
    do
        controls.enableDamping <- true
        controls.dampingFactor <- 0.05
        controls.maxPolarAngle <- Math.PI / 2.1
        controls.screenSpacePanning <- false
    
    let resizeRendererToDisplaySize (renderer: Renderer) =
        let canvas = renderer.domElement
        let width, height = canvas.clientWidth, canvas.clientHeight
        let needResize = canvas.width <> width || canvas.height <> height
        if needResize then
            renderer.setSize(width, height, false)
        needResize
    
    member _.Scene = scene
    member _.Renderer = renderer
    member _.Camera = camera
    
    member _.Render() =
        if (resizeRendererToDisplaySize renderer) then
            let canvas = renderer.domElement
            camera.aspect <- canvas.clientWidth / canvas.clientHeight
            camera.updateProjectionMatrix()
        controls.update()
        renderer.render(scene, camera)
    
    member _.DomElement = renderer.domElement
    
    interface IDisposable with
        member this.Dispose() =
            controls.dispose()
            renderer.dispose()
    


type Region =
    { CanvasContext: CanvasRenderingContext2D
      Mesh: Mesh<PlaneGeometry, MeshBasicMaterial> }

type ThreeHeightmapRenderer(stage: Stage, palette, regionSize) =
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
//            material.transparent <- true
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
                        let h = (h + 1.) / 3. |> max 0. |> min 1.
                        let (Rgb (r, g, b)) = Palette.pick palette h
                        imageData.data.[imageIdx * 4 + 0] <- r
                        imageData.data.[imageIdx * 4 + 1] <- g
                        imageData.data.[imageIdx * 4 + 2] <- b
                        imageData.data.[imageIdx * 4 + 3] <- 0xFFuy
                region.CanvasContext.putImageData(imageData, intX0 %! rs |> float, intY0 %! rs |> float)
                region.Mesh.material.needsUpdate <- true
                region.Mesh.material.map.Value.needsUpdate <- true
    
    member _.DrawRect(rectX0, rectY0, rectW, rectH, heights: float[]) = drawRect rectX0 rectY0 rectW rectH heights
    
    member this.Clear() =
        (this :> IDisposable).Dispose()
    
    interface IHeightmapRenderer with
        member this.RenderRect(x, y, w, h, heights) = drawRect x y w h heights
    
    interface IDisposable with
        member _.Dispose() =
            resources |> Seq.iter ^fun r -> r?dispose()
            let objects = regions.Values |> Seq.map ^fun r -> r.Mesh
            stage.Scene.remove(!!objects) |> ignore
    


//type ThreeHeightmap3DRenderer(stage: Stage) =
//    let resources = ResizeArray<obj>()
//    
//    let regions: IDictionary<int * int, Region> = upcast Dictionary()
//    
//    let getRegion rx ry =
//        let createRegion rx ry =
//            printfn $"Create new region at ({rx}, {ry})R"
//            let canvas = document.createElement("canvas") :?> HTMLCanvasElement
//            canvas.width <- float regionSize
//            canvas.height <- float regionSize
//            let canvasTexture = THREE.CanvasTexture.Create(!^canvas)
//            canvasTexture.magFilter <- THREE.NearestFilter
//            let material = THREE.MeshBasicMaterial.Create()
//            material.map <- Some !!canvasTexture
//            let shaderMaterial = THREE.ShaderMaterial.Create()
//            
//            let planeGeometry = THREE.PlaneGeometry.Create(float regionSize, float regionSize, float regionSize / 32., float regionSize / 32.)
//            let mesh = THREE.Mesh.Create(planeGeometry, material)
//            mesh.rotation.set(deg2rad -90., 0., 0.) |> ignore
//            mesh.position.set(
//                float rx * float regionSize + float regionSize / 2.,
//                0.0,
//                float ry * float regionSize + float regionSize / 2.
//            ) |> ignore
//            
//            let ctx = canvas.getContext_2d()
//            ctx?webkitImageSmoothingEnabled <- false
//            ctx?imageSmoothingEnabled <- false
//            
//            let region = { CanvasContext = ctx; Mesh = mesh }
//            regions.[(rx, ry)] <- region
//            stage.Scene.add(!![| mesh |]) |> ignore
//            resources.AddRange([canvasTexture; material; planeGeometry])
//            
//            region
//        
//        match regions.TryGetValue((rx, ry)) with
//        | true, region -> region
//        | false, _ -> createRegion rx ry
//    
//    let drawRect rectX0 rectY0 rectW rectH (heights: float[]) =
//        let rs = regionSize
//        let rectX1, rectY1 = rectX0 + rectW - 1, rectY0 + rectH - 1
//        let regionX0, regionY0 = rectX0 /! rs, rectY0 /! rs
//        let regionX1, regionY1 = rectX1 /! rs, rectY1 /! rs
//        for rx in regionX0 .. regionX1 do
//            for ry in regionY0 .. regionY1 do
//                let region = getRegion rx ry
//                let intX0, intY0 = max (rx * rs) rectX0, max (ry * rs) rectY0
//                let intX1, intY1 = min ((rx + 1) * rs - 1) rectX1, min ((ry + 1) * rs - 1) rectY1
//                let intW, intH = intX1 - intX0 + 1, intY1 - intY0 + 1
//                let imageData = region.CanvasContext.createImageData(intW |> float, intH |> float)
//                for x in intX0 .. intX1 do
//                    for y in intY0 .. intY1 do
//                        let imageIdx = (y - intY0) * intW + (x - intX0) // transposed
//                        let heightIdx = (x - rectX0) * rectH + (y - rectY0)
//                        let h = heights.[heightIdx]
//                        let h = (h + 1.) / 3. |> max 0. |> min 1.
//                        let (Rgb (r, g, b)) = Palette.pick palette h
//                        imageData.data.[imageIdx * 4 + 0] <- r
//                        imageData.data.[imageIdx * 4 + 1] <- g
//                        imageData.data.[imageIdx * 4 + 2] <- b
//                        imageData.data.[imageIdx * 4 + 3] <- 0xFFuy
//                region.CanvasContext.putImageData(imageData, intX0 %! rs |> float, intY0 %! rs |> float)
//                region.Mesh.material.needsUpdate <- true
//                region.Mesh.material.map.Value.needsUpdate <- true
//    
//    interface IHeightmapRenderer with
//        member this.RenderRect(x, y, w, h, heights) = drawRect x y w h heights
//    
//    interface IDisposable with
//        member _.Dispose() = resources |> Seq.iter ^fun r -> r?dispose()



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
    
    let mousePos = THREE.Vector2.Create()

    let onMouseMove (event: MouseEvent) =
        let rect = stage.Renderer.domElement.getBoundingClientRect()
        let mxs, mys =
            (event.clientX - rect?x) / rect.width * 2. - 1.,
            (event.clientY - rect?y) / rect.height * 2. - 1. |> (~-)
        mousePos.set(mxs, mys) |> ignore
    do
        emitJsExpr (onMouseMove) """
            const $onMouseMove = $0;
            this._onMouseMove = $onMouseMove;
            window.addEventListener("mousemove", this._onMouseMove);
        """

    let onKeyDown (event: KeyboardEvent) =
        if event.key = "g" then
            onGenerateChunk chunkSelectionCoords
    do
        emitJsExpr (onKeyDown) """
            const $onKeyDown = $0;
            this._onKeyDown = $onKeyDown;
            document.addEventListener("keydown", this._onKeyDown);
        """
    
    let raycaster = THREE.Raycaster.Create()
    
    let planeX0Z = THREE.Plane.Create(THREE.Vector3.Create(0., 1., 0.), 0.)
    
    member _.Move(cx, cy) =
        chunkSelectionCoords <- (cx, cy)
        let x, z =
            float cx * float cs + float cs / 2. + 0.5,
            float cy * float cs + float cs / 2. + 0.5
        chunkSelection.position.set(x, 0., z) |> ignore
    
    member this.Render(camera) =
        raycaster.setFromCamera(!!mousePos, camera)
        let ray = raycaster.ray
        let intersection = ray.intersectPlane(planeX0Z, THREE.Vector3.Create())
        match intersection with
        | Some point ->
            let x, z = point.x, point.z
            let cx, cy = x /! float cs |> int, z /! float cs |> int
            this.Move(cx, cy)
        | None -> ()
    
    member _.Visible
        with get() = chunkSelection.visible
        and set(value) = chunkSelection.visible <- value
    
    interface IDisposable with
        member _.Dispose() =
            resources |> Seq.iter ^fun r -> r?dispose()
            emitJsExpr () """
                document.removeEventListener("keydown", this._onKeyDown)
                window.removeEventListener("mousemove", this._onMouseMove)
            """
            printfn "ChunkSelection disposed"


let configureStage cs (chunks: IObservable<int * int * Chunk>) palette onGenerateChunk =
    let canvas = document.createElement("canvas") :?> HTMLCanvasElement
    canvas.classList.add("stage-canvas")
    
    let stage = new Stage(canvas)
    let heightmapRenderer = new ThreeHeightmapRenderer(stage, palette, 1000)
    let subscription =
        chunks
        |> fun x -> !!x : IRxObservable<int * int * Chunk>
        |> Rx.subscribe(fun (cx, cy, chunk) ->
            let x, y = cx * chunk.Width, cy * chunk.Height
            (heightmapRenderer :> IHeightmapRenderer).RenderRect(x, y, chunk.Width, chunk.Height, chunk.Heights)
        )
    
    let chunkSelection = new ChunkSelection(stage, cs, onGenerateChunk)
    
    let render time =
        chunkSelection.Render(stage.Camera)
        stage.Render()
    
    let rec animate time =
        render time
        window.requestAnimationFrame(animate) |> ignore
    window.requestAnimationFrame(animate) |> ignore
    
    Disposable.concat [
        subscription
        heightmapRenderer
        chunkSelection
        stage
        Disposable.create ^fun () -> printfn "Stage disposed"
    ], stage.DomElement
