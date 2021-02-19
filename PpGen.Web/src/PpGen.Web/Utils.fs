module PpGen.Web.Utils


module Array2DJagged =
    
    let iteri (action: int -> int -> 'a -> unit) (src: 'a[][]) : unit =
        src
        |> Array.iteri (fun x -> Array.iteri (action x))


module Rgb =

    open PpGen.Utils
    open Fable.Core.JsInterop

//    #if FABLE_COMPILER
    let inline private hex c = c?toString(16)?padStart(2, "0")
    let inline toHex (Rgb (r, g, b)) =
        "#" + (hex r) + (hex g) + (hex b)
    let inline toInt32 (Rgb (r, g, b)) =
        int r <<< 16 ||| int g <<< 8 ||| int b
//    #endif
    ()


module Observer =

    open System
    open Fable.SignalR
    
    let toSignalR (obv: IObserver<'a>) : StreamSubscriber<'a> =
        { next = obv.OnNext
          error = Option.get >> obv.OnError
          complete = obv.OnCompleted }


module Async =
    
    let bind f x = async.Bind(x, f)
    
    let run (work: Async<'a>) : 'a =
        let mutable result = Unchecked.defaultof<_>
        async {
            let! r = work
            result <- r
        } |> Async.StartImmediate
        result
