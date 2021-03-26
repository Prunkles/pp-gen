namespace Fable.System.Reactive

open System

type ISubject<'TSource, 'TResult> =
    inherit IObserver<'TSource>
    inherit IObservable<'TResult>

type ISubject<'T> = ISubject<'T, 'T> // TODO? : Make interface

[<AbstractClass>]
type SubjectBase<'T>() =
//    abstract HasObservers: bool
//    abstract IsDisposed: bool
    
    abstract Dispose: unit -> unit
    interface IDisposable with
        member this.Dispose() = this.Dispose()
    
    abstract OnCompleted: unit -> unit
    abstract OnError: error: Exception -> unit
    abstract OnNext: value: 'T -> unit
    abstract Subscribe: observer: IObserver<'T> -> IDisposable
    interface ISubject<'T> with
        member this.OnCompleted() = this.OnCompleted()
        member this.OnError(error) = this.OnError(error)
        member this.OnNext(value) = this.OnNext(value)
        member this.Subscribe(observer) = this.Subscribe(observer)

//[<Sealed>]
[<AbstractClass>]
type AsyncSubject<'T>() =
    inherit SubjectBase<'T>()

//[<Sealed>]
[<AbstractClass>]
type ReplaySubject<'T>() =
    inherit SubjectBase<'T>()

// --------

type Disposable =
    static member Create(dispose) = { new IDisposable with member _.Dispose() = dispose () }
    static member Empty with get() = Disposable.Create(ignore)
