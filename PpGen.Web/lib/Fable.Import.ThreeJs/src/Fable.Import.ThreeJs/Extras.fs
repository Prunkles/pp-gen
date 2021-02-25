namespace Fable.Import.ThreeJs

namespace Fable.Import.ThreeJs.Examples.Jsm.Controls

open Browser.Types
open Fable.Core
open Fable.Import.ThreeJs.Types.__cameras_Camera

[<Import("OrbitControls", from="three/examples/jsm/controls/OrbitControls")>]
type OrbitControls (object: Camera, domElement: Element) =
    member _.update(): unit = jsNative
    
    member _.dampingFactor with get() = jsNative: float and set(_: float) = jsNative
    member _.enableDamping with get() = jsNative: bool and set(_: bool) = jsNative
    member _.maxPolarAngle  with get() = jsNative: float and set(_: float) = jsNative
    member _.screenSpacePanning with get() = jsNative: bool and set(_: bool) = jsNative
