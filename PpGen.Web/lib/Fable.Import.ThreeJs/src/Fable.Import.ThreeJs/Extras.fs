namespace Fable.Import.ThreeJs

namespace Fable.Import.ThreeJs.Examples.Jsm.Controls

open Browser.Types
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.ThreeJs.Types.__cameras_Camera
open Fable.Import.ThreeJs.Types.__materials_ShaderMaterial
open Fable.Import.ThreeJs.Types.__objects_Mesh

[<Import("OrbitControls", from="three/examples/jsm/controls/OrbitControls")>]
type OrbitControls(object: Camera, domElement: Element) =
    member _.update(): unit = jsNative
    member _.dispose(): unit = jsNative
    
    member _.dampingFactor with get() = jsNative: float and set(_: float) = jsNative
    member _.enableDamping with get() = jsNative: bool and set(_: bool) = jsNative
    member _.maxPolarAngle  with get() = jsNative: float and set(_: float) = jsNative
    member _.screenSpacePanning with get() = jsNative: bool and set(_: bool) = jsNative


type Sky =
    inherit Mesh<obj, ShaderMaterial>

type SkyStatic =
    [<EmitConstructor>] abstract Create: unit -> Sky

module SkyExports =
    
    type IExports =
        abstract Sky: SkyStatic
    
    let Sky: IExports = importAll "three/examples/jsm/objects/Sky"
