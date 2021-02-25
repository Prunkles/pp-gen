namespace Fable.Import.ThreeJs

namespace Fable.Import.ThreeJs.Examples.Jsm.Controls

open Browser.Types
open Fable.Core

[<Import("OrbitControls", from="three/examples/jsm/controls/OrbitControls")>]
type OrbitControls (object: obj, domElement: Element) =
    abstract update: unit -> bool
    default _.update() = jsNative
    
    abstract dampingFactor: float with get, set
    default _.dampingFactor with get() = jsNative and set(_) = jsNative