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

let inline deg2rad a = Math.PI / 180. * a

[<ReactComponent>]
let Map (cs: int) (points: IObservable<Point seq>) (onGenerateChunk: (int * int) -> unit) =
    let ref = React.useRef(None: HTMLDivElement option)
    React.useEffect(fun () ->
        
        let width, height = 1024., 720.
        
        let scene = THREE.Scene.Create()
        
        let renderer = THREE.WebGLRenderer.Create()
        renderer.setSize(width, height)
        renderer.setClearColor(!^"#061639")
        ref.current.Value.appendChild(renderer.domElement) |> ignore
        
        let camera = THREE.PerspectiveCamera.Create(90., width / height, 1., 10000.)
        camera.position.set(0., 500., 0.) |> ignore
        
        let controls = OrbitControls(camera, renderer.domElement)
        controls.enableDamping <- true
        controls.dampingFactor <- 0.05
        controls.maxPolarAngle <- Math.PI / 2.1
        controls.screenSpacePanning <- false
        
        let regions: IDictionary<int * int, _> = upcast Dictionary()
        
        let drawPoint (x: int) (y: int) (h: float) : unit =
            let rx, ry = x /! regionSize, y /! regionSize
            let ctx, mesh =
                match regions.TryGetValue((rx, ry)) with
                | true, ctx -> ctx
                | false, _ ->
                    printfn $"Create new region at ({rx}, {ry})R"
                    let canvas = document.createElement("canvas") :?> HTMLCanvasElement
                    canvas.width <- float regionSize
                    canvas.height <- float regionSize
                    let canvasMap = THREE.CanvasTexture.Create(!^canvas)
                    canvasMap.magFilter <- THREE.NearestFilter
                    let material = THREE.MeshBasicMaterial.Create()
                    material.map <- Some !!canvasMap
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
                    regions.[(rx, ry)] <- (ctx, mesh)
                    scene.add(!![| mesh |]) |> ignore
                    ctx, mesh
            
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

            mesh.material.needsUpdate <- true
            mesh.material.map.Value.needsUpdate <- true

            if x %! 50 = 0 && y %! 50 = 0 then
                printfn $"Drawn point at ({x}, {y})P with ({r}, {g}, {b}) on ({rx}, {ry})R"
            ()
        
        let chunkSelection =
            let material = THREE.LineBasicMaterial.Create()
            material.color <- THREE.Color.Create(!^"#00F")
            material.depthTest <- false
            let planeGeometry = THREE.PlaneGeometry.Create(float cs + 1., float cs + 1.)
            let edgeGeometry = THREE.EdgesGeometry.Create(planeGeometry)
            let mesh = THREE.LineSegments.Create(edgeGeometry, material)
            mesh.renderOrder <- 1000.
            mesh.rotation.set(deg2rad -90., 0., 0.) |> ignore
            mesh
        scene.add(!![| chunkSelection |]) |> ignore
        let mutable chunkSelectionCoords = Unchecked.defaultof<_>
        
        let setChunkSelectionCoords (cx: int) (cy: int) =
            chunkSelectionCoords <- (cx, cy)
            let x, z = float cx * float cs + float cs / 2. + 0.5, float cy * float cs + float cs / 2. + 0.5
            chunkSelection.position.set(x, 0., z) |> ignore
        
        let mouse = THREE.Vector2.Create()
        window.addEventListener(
            "mousemove",
            !!(fun (event: MouseEvent) ->
                let rect = renderer.domElement.getBoundingClientRect()
                let mxs, mys =
                    (event.clientX - rect?x) / rect.width * 2. - 1.,
                    -(event.clientY - rect?y) / rect.height * 2. + 1.
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
        
        let rec render time =
            controls.update()
            
            raycaster.setFromCamera(!!mouse, camera)
            raycaster.intersectObjects(scene.children)
            |> Seq.iter ^fun intersection ->
                let x, z = intersection.point.x, intersection.point.z
                let cx, cy = x /! float cs |> int, z /! float cs |> int
                setChunkSelectionCoords cx cy
            
            renderer.render(scene, camera)
            window.requestAnimationFrame(render) |> ignore
        render 0.
        
        let subscription =
            points
            |> Observable.subscribe (fun pointBuffer ->
                for point in pointBuffer do
                    drawPoint point.X point.Y point.Height
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

open Fable.Import.RxJS.Misc

[<ReactComponent>]
let App () =
    let pointSubject =
        React.useMemo(
            fun () ->
                Fable.Import.RxJS.JsTypes.Subject()
            , [| |]
        )
    
    let onGenerateChunk (cx, cy) =
        async {
            let! points = generator.GenerateChunkStream({ args with ChunkX = cx; ChunkY = cy })
            let subscription =
                points.Subscribe(Observer.create
                    (fun x -> pointSubject.next(x))
                    (fun err -> pointSubject.error(err))
                    (fun () -> printfn $"Chunk ({cx}, {cy})C completed")
                )
            ignore subscription
            printfn $"Request chunk generation at ({cx}, {cy})C"
        } |> Async.StartImmediate
    
    React.useEffectOnce(fun () ->
        onGenerateChunk (2, 2)
    )
    
    Map cs pointSubject onGenerateChunk
