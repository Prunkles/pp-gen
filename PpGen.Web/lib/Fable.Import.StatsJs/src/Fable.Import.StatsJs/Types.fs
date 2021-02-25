// ts2fable 0.8.0, modified
namespace rec Fable.Import.StatsJs

open System
open Fable.Core
open Fable.Core.JS
open Browser.Types


[<Import("Stats", "stats.js")>]
type [<AllowNullLiteral>] Stats() =
    
    abstract REVISION: float with get, set
    default _.REVISION with get() = jsNative and set _ = jsNative
    
    abstract dom: HTMLDivElement with get, set
    default _.dom with get() = jsNative and set _ = jsNative
    
    /// <param name="value">0:fps, 1: ms, 2: mb, 3+: custom</param>
    abstract showPanel: value: float -> unit
    default _.showPanel _ = jsNative

    abstract ``begin``: unit -> unit
    default _.``begin``() = jsNative
    
    abstract ``end``: unit -> float
    default _.``end``() = jsNative
    
    abstract update: unit -> unit
    default _.update() = jsNative
