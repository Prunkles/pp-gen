[<AutoOpen>]
module FuncStuff

open GenericNumber

let inline ( ^ ) f x = f x

[<AutoOpen>]
module Math =
    let inline log2 x = log x / log (1G + 1G)
    let inline normalize a0 a1 x = (x - a0) / (a1 - a0)
    let inline lerp a0 a1 t = a0 + t * (a1 - a0)