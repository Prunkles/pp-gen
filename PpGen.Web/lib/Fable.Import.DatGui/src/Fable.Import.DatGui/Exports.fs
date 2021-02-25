module Fable.Import.DatGui.Exports

open Fable.Core
open Fable.Core.JsInterop

let DatGui: Types.IExports = importAll "dat.gui"
