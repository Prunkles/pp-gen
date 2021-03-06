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

open Fable.Import.ThreeJs.Types.__core_Object3D
open Fable.Import.ThreeJs.Types.__geometries_EdgesGeometry
open Fable.Import.ThreeJs.Types.__geometries_PlaneGeometry
open Fable.Import.ThreeJs.Types.__helpers_HemisphereLightHelper
open Fable.Import.ThreeJs.Types.__helpers_PlaneHelper
open Fable.Import.ThreeJs.Types.__materials_LineBasicMaterial
open Fable.Import.ThreeJs.Types.__math_Plane
open Fable.Import.ThreeJs.Types.__math_Ray
open Fable.Import.ThreeJs.Types.__objects_LineSegments
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
    abstract RenderRect: x: int * y: int * w: int * h: int * heights: float32[] -> unit


type Region =
    { CanvasContext: CanvasRenderingContext2D
      Mesh: Mesh<PlaneGeometry, MeshBasicMaterial> }

type ThreeHeightmapRenderer(scene: Scene, palette, regionSize) =
    let resources = ResizeArray<IThreeDisposable>()
    
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
            material.alphaTest <- 1.
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
            scene.add(mesh) |> ignore
            resources.AddRange([!!canvasTexture; !!material; !!planeGeometry])
            
            region
        
        match regions.TryGetValue((rx, ry)) with
        | true, region -> region
        | false, _ -> createRegion rx ry
    
    let drawRect rectX0 rectY0 rectW rectH (heights: float32[]) =
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
                        let (Rgb (r, g, b)) = Palette.pick palette (float h)
                        imageData.data.[imageIdx * 4 + 0] <- r
                        imageData.data.[imageIdx * 4 + 1] <- g
                        imageData.data.[imageIdx * 4 + 2] <- b
                        imageData.data.[imageIdx * 4 + 3] <- 0xFFuy
                region.CanvasContext.putImageData(imageData, intX0 %! rs |> float, intY0 %! rs |> float)
                region.Mesh.material.needsUpdate <- true
                region.Mesh.material.map.Value.needsUpdate <- true
    
    member _.DrawRect(rectX0, rectY0, rectW, rectH, heights: float32[]) = drawRect rectX0 rectY0 rectW rectH heights
    
    member this.Clear() =
        (this :> IDisposable).Dispose()
    
    interface IHeightmapRenderer with
        member this.RenderRect(x, y, w, h, heights) = drawRect x y w h heights
    
    interface IDisposable with
        member _.Dispose() =
            resources |> Seq.iter ^fun r -> r.dispose()
            let objects = regions.Values |> Seq.map (fun r -> r.Mesh) |> Seq.toArray
            scene.remove(objects) |> ignore


// TODO: Fix incorrect rendering
type Three3DHeightmapRenderer(scene: Scene, palette) =
    let resources = ResizeArray<IThreeDisposable>()
    
    let rects = Dictionary<int * int * int * int, {| Mesh: Mesh; Disposable: IDisposable |}>()
    
    let drawRect rectX0 rectY0 rectW rectH (heights: float32[]) =
        
        // Try remove existing mesh on the same place
        match rects.TryGetValue((rectX0, rectY0, rectW, rectH)) with
        | false, _ -> ()
        | true, r ->
            r.Disposable.Dispose()
            scene.remove(r.Mesh) |> ignore
        
        
        // transpose and convert to float32
        let heights =
            heights
            |> Array.mapi (fun i _ ->
                let x, y = i / rectW, i % rectH
                let x, y = y, rectW - x
                let h = heights.[x * rectW + y]
                let h = h |> max 0.0f |> min 1.0f
                float32 h
            )
        
        let paletteTexture =
            let paletteData =
                heights
                |> Array.collect (fun h ->
                    let (Rgb (r, g, b)) = float h |> Palette.pick palette
                    [| float32 r / 255.f; float32 g / 255.f; float32 b / 255.f |]
                )
            THREE.DataTexture.Create(paletteData, float rectW, float rectH, format=THREE.RGBFormat, ``type``=THREE.FloatType)
        resources.Add(!!paletteTexture)
        
        let planeGeometry = THREE.PlaneGeometry.Create(float rectW, float rectH, 64., 64.)
        resources.Add(!!planeGeometry)
        
        let heightTexture =
            let heightData = heights
            THREE.DataTexture.Create(heightData, float rectW, float rectH, format=THREE.RedFormat, ``type``=THREE.FloatType)
//        heightTexture.wrapS <- THREE.RepeatWrapping
//        heightTexture.wrapT <- THREE.RepeatWrapping
        resources.Add(!!heightTexture)
        
        let material = THREE.ShaderMaterial.Create()
        material.side <- THREE.DoubleSide
        material.vertexShader <- """
            uniform sampler2D heightTexture;
            uniform float heightScale;
            
            varying float vAmount;
            varying vec2 vUV;
            
            void main() {
                vec4 heightData = texture2D(heightTexture, uv);
                vAmount = heightData.r;
                vUV = uv;
                
                vec3 newPosition = position + normal * heightScale * vAmount;
                // vec3 newPosition = position + vec3(0.0, 0.0, 1.0) * heightScale * vAmount;
                gl_Position = projectionMatrix * modelViewMatrix * vec4(newPosition, 1.0);
            }
        """
        material.fragmentShader <- """
            uniform sampler2D paletteTexture;
            
            varying vec2 vUV;
            varying float vAmount;
            
            void main() {
                vec4 paletted = texture(paletteTexture, vUV);
                gl_FragColor = paletted;
            }
        """
        
        let scale = 100.0
        
        let customUniforms =
            {| heightTexture = {| ``type`` = "t"; value = heightTexture |}
               heightScale = {| ``type`` = "f"; value = scale |}
               paletteTexture = {| ``type`` = "t"; value = paletteTexture |} |}
        
        material.uniforms <- !!customUniforms
        
        resources.Add(!!material)

        let mesh = THREE.Mesh.Create(planeGeometry, material)
        mesh.position.set(
            float rectX0 + float rectW / 2.,
            0.0,
            float rectY0 + float rectH / 2.
        ) |> ignore
        mesh.rotation.set(deg2rad -90., 0., 0.) |> ignore
        
        scene.add(mesh) |> ignore
        rects.[(rectX0, rectY0, rectW, rectH)] <-
            {| Mesh = !!mesh
               Disposable = Disposable.concat [
                   Disposable.ofThreeDisposable !!material
                   Disposable.ofThreeDisposable !!heightTexture
                   Disposable.ofThreeDisposable !!planeGeometry
                   Disposable.ofThreeDisposable !!paletteTexture
               ] |}
    
    member _.DrawRect(rectX0, rectY0, rectW, rectH, heights: float32[]) = drawRect rectX0 rectY0 rectW rectH heights
    
    member this.Clear() =
        (this :> IDisposable).Dispose()
    
    interface IHeightmapRenderer with
        member this.RenderRect(x, y, w, h, heights) = drawRect x y w h heights
    
    interface IDisposable with
        member _.Dispose() =
            resources |> Seq.iter ^fun r -> r.dispose()



type ChunkSelection(scene: Scene, renderer: Renderer, cs, onGenerateChunk) =
    let resources = ResizeArray<IThreeDisposable>()
    
    let chunkSelection =
        let material = THREE.LineBasicMaterial.Create()
        material.color <- THREE.Color.Create(!^"#00F")
        let planeGeometry = THREE.PlaneGeometry.Create(float cs + 1., float cs + 1.)
        let edgeGeometry = THREE.EdgesGeometry.Create(planeGeometry)
        let mesh = THREE.LineSegments.Create(edgeGeometry, material)
        mesh.renderOrder <- 1000.
        mesh.onBeforeRender <- fun renderer _ _ _ _ _ -> renderer.clearDepth()
        mesh.rotation.set(deg2rad -90., 0., 0.) |> ignore
        
        resources.AddRange([!!planeGeometry; !!edgeGeometry; !!material])
        mesh
    
    do scene.add(chunkSelection) |> ignore

    let mutable chunkSelectionCoords = Unchecked.defaultof<_>
    
    let mousePos = THREE.Vector2.Create()

    let onMouseMove (event: MouseEvent) =
        let rect = renderer.domElement.getBoundingClientRect()
        let mxs, mys =
            (event.clientX - rect?x) / rect.width * 2. - 1.,
            (event.clientY - rect?y) / rect.height * 2. - 1. |> (~-)
        mousePos.set(mxs, mys) |> ignore
    let onMouseMove = onMouseMove
    do window.addEventListener("mousemove", !!onMouseMove)

    let onKeyDown (event: KeyboardEvent) =
        if event.key = "g" then
            onGenerateChunk chunkSelectionCoords
    let onKeyDown = onKeyDown
    do document.addEventListener("keydown", !!onKeyDown)
    
    let raycaster = THREE.Raycaster.Create()
    
    let planeX0Z = THREE.Plane.Create(THREE.Vector3.Create(0., 1., 0.), 0.)
    
    member _.Move(cx, cy) =
        chunkSelectionCoords <- (cx, cy)
        let x, z =
            float cx * float cs + float cs / 2. + 0.5,
            float cy * float cs + float cs / 2. + 0.5
        chunkSelection.position.set(x, 0., z) |> ignore
    
    member this.Update(camera) =
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
            resources |> Seq.iter ^fun r -> r.dispose()
            document.removeEventListener("keydown", !!onKeyDown)
            document.removeEventListener("mousemove", !!onMouseMove)
            printfn "ChunkSelection disposed"


type Stage(cs, chunks: IObservable<int * int * Chunk>, palette, onGenerateChunk, use3D) =
    
    let canvas = document.createElement("canvas") :?> HTMLCanvasElement
    do canvas.classList.add("stage-canvas")
    
    let scene = THREE.Scene.Create()
    do
        scene.add(THREE.AmbientLight.Create(!^(float 0x222222), 1.)) |> ignore
        scene.add(THREE.DirectionalLight.Create(!^(float 0xffffff), 1.)) |> ignore
    
    let renderer = THREE.WebGLRenderer.Create(!!{| canvas = Some (U2.Case1 canvas) |})
    do
        renderer.toneMapping <- THREE.ACESFilmicToneMapping
        renderer.toneMappingExposure <- 1.0
    
    let sky = SkyExports.Sky.Sky.Create()
    do
        sky.scale.setScalar(450000.) |> ignore
        let sun = THREE.Vector3.Create(1., 0.5, 0.)
        let effectController =
            {|
                turbidity = 10.
                rayleigh = 2.
                mieCoefficient = 0.005
                mieDirectionalG = 0.8
                inclination = 0.49 // elevation / inclination
                azimuth = 0.25 // Facing front
            |}
        let uniforms = sky.material.uniforms
        let inline setUniformValue name value = uniforms.[name].value <- Some !!value
        setUniformValue "sunPosition" sun
        setUniformValue "turbidity" effectController.turbidity
        setUniformValue "rayleigh" effectController.rayleigh
        setUniformValue "mieCoefficient" effectController.mieCoefficient
        setUniformValue "mieDirectionalG" effectController.mieDirectionalG
        setUniformValue "sunPosition" (THREE.Vector3.Create(400000., 400000., 400000.))
        
        scene.add(sky) |> ignore
    
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
    
    // ---
    
    let axesHelper = THREE.AxesHelper.Create(100.)
    do scene.add(axesHelper) |> ignore
    
    let heightmapRenderer: IHeightmapRenderer =
        if use3D
        then upcast new Three3DHeightmapRenderer(scene, palette)
        else upcast new ThreeHeightmapRenderer(scene, palette, 1000)
    
    let subscription =
        chunks
        |> fun x -> !!x : IRxObservable<int * int * Chunk>
        |> Rx.subscribe(fun (cx, cy, chunk) ->
            let x, y = cx * chunk.Width, cy * chunk.Height
            (heightmapRenderer).RenderRect(x, y, chunk.Width, chunk.Height, chunk.Heights)
        )
    
    let chunkSelection = new ChunkSelection(scene, renderer, cs, onGenerateChunk)
    
    // ---
    
    member _.Render() =
        chunkSelection.Update(camera)
        if (resizeRendererToDisplaySize renderer) then
            let canvas = renderer.domElement
            camera.aspect <- canvas.clientWidth / canvas.clientHeight
            camera.updateProjectionMatrix()
        controls.update()
        renderer.render(scene, camera)
    
    member this.Animate(time) =
        this.Render()
        window.requestAnimationFrame(this.Animate) |> ignore
    
    member this.StartAnimate() =
        window.requestAnimationFrame(this.Animate) |> ignore
    
    member _.DomElement = renderer.domElement
    
    interface IDisposable with
        member this.Dispose() =
            (Disposable.concat [
                subscription
                !!heightmapRenderer
                chunkSelection
            ]).Dispose()
            controls.dispose()
            renderer.dispose()
            printfn "Stage disposed"
