module Fable.Import.RxJS.Playground.Program

open System
open Fable.Import.RxJS



let source: IRxObservable<string> =
    Rx.of' (Seq.init 50 string |> Seq.toArray)
//    |> Rx.bufferCount 10 5
//    |> RxOp.mergeMap ((fun x -> Rx.of' [|"a"+x; "b"+x; "c"+x|]))
//    |> RxOp.first(fun x i -> x = "10" || i = 3)
//    |> RxOp.map(fun x i -> i)


let obv =
    { new IObserver<_> with
        member _.OnNext(value) = printfn $"{value}"
        member _.OnError(err) = printfn $"<Error>: {err}"
        member _.OnCompleted() = printfn "<Completed>" }

let subscription = source |> Rx.subscribe obv

//subscription.Dispose()