[<AutoOpen>]
module Fable.Import.RxJS.Misc

open System


module Observer =
    
    let create onNext onError onCompleted =
        { new IObserver<_> with
            member _.OnNext(x) = onNext x
            member _.OnError(err) = onError err
            member _.OnCompleted() = onCompleted () }
    
    let createNext onNext = create onNext ignore ignore
    