namespace Fable.Import.RxJS

open System
open Fable.Core
open Fable.Core.JsInterop

open TypeTransforms


[<AutoOpen>]
module private Imports =
    let rxjs: obj = importAll "rxjs"
    let rxjs_operators: obj = importAll "rxjs/operators/index.js" // TODO: Import directory
    let rxjs_webSocket: obj = importAll "rxjs/webSocket"
    
    Prototypes.Subscription ()
    Prototypes.Observable ()
    Prototypes.Subscriber ()


type IRxObservable<'a> = inherit IObservable<'a>

module RxObservable =
    
    let internal toClass (rxObs: IRxObservable<'a>) : JsTypes.Observable<'a> = !!rxObs
    let internal ofClass (jsObs: JsTypes.Observable<'a>) : IRxObservable<'a> = !!jsObs
    
    let ofObservable (source: IObservable<'a>) : IRxObservable<'a> =
        let subscribe (subscriber: JsTypes.Subscriber<'a>) : JsTypes.TeardownLogic =
            let obv = Obv.js2sys subscriber
            let disposable = source.Subscribe(obv)
            let unsubscribable = { new JsTypes.Unsubscribable with member _.unsubscribe() = disposable.Dispose() }
            JsTypes.TeardownLogic.Unsubscribable unsubscribable
        JsTypes.Observable(subscribe) |> ofClass


[<AutoOpen>]
module private Helpers =
    
    let callOp (operatorName: string) (args: obj seq) (source: IRxObservable<'a>) : IRxObservable<'b> =
        emitJsExpr (rxjs_operators, operatorName, args, source) "$0[$1](...$2)($3)"
    
    let callIndex (name: string) (args: obj seq) =
        emitJsExpr (rxjs, name, args) "$0[$1](...$2)"

type Operator<'a, 'b> = IRxObservable<'a> -> IRxObservable<'b>
type MonoTypeOperator<'a> = Operator<'a, 'a>

type Rx =
    
    static member of'(args: 'a[]) : IRxObservable<'a> =
        emitJsExpr (rxjs, args) "$0.of(...$1)"
    
    static member from(input: 'any) : IRxObservable<'a> =
        callIndex "from" [input]
    
    static member merge(observables: 'any[]) : IRxObservable<'a> =
        callIndex "merge" [observables]
    
    
    static member subscribe(observer: IObserver<'a>) : IRxObservable<'a> -> IDisposable =
        fun source -> source.Subscribe(observer)
    
    static member subscribe(onNext: 'a -> unit) : IRxObservable<'a> -> IDisposable =
        fun source -> source?subscribe(onNext)
    
    static member subscribe(onNext: 'a -> unit, onError: exn -> unit, onCompleted: unit -> unit) : IRxObservable<'a> -> unit =
        fun source -> source?subscribe(onNext, onError, onCompleted)

type Rx with

    static member ofSeq (xs: 'a seq) : IRxObservable<'a> = Rx.from xs


type RxWebSocket =

    static member webSocket(url: string): JsTypes.WebSocketSubject<'T> =
        rxjs_webSocket?webSocket(url)
    static member webSocket(config: JsTypes.WebSocketSubjectConfig<'T>): JsTypes.WebSocketSubject<'T> =
        rxjs_webSocket?webSocket(config)


type RxOp =
    
    static member mergeMap (project: 'a -> IRxObservable<'b>, ?concurrent: int) : IRxObservable<'a> -> IRxObservable<'b> =
        callOp "mergeMap" [project; concurrent]
    static member mergeMap (project: 'a -> int -> IRxObservable<'b>, ?concurrent: int) : IRxObservable<'a> -> IRxObservable<'b> =
        callOp "mergeMap" [project; concurrent]
    
    static member map(project: 'a -> 'b): Operator<'a, 'b> =
        callOp "map" [project]
    static member mapi(project: 'a -> int -> 'b): Operator<'a, 'b> =
        callOp "map" [project]
    
    static member bufferCount (bufferSize: int) (startBufferEvery: int) : IRxObservable<'a> -> IRxObservable<'a[]> =
        callOp "bufferCount" [bufferSize; startBufferEvery]
    
    static member find (predicate: 'a -> bool) : IRxObservable<'a> -> IRxObservable<'a option> =
        callOp "find" [predicate]
    
    static member flatMap (project: 'a -> #IRxObservable<'b>) : IRxObservable<'a> -> IRxObservable<'b> =
        callOp "flatMap" [project]
    
    static member window (windowBoundaries: IRxObservable<'a> -> IRxObservable<'any>) : IRxObservable<'a> -> IRxObservable<IRxObservable<'a>> =
        callOp "window" [windowBoundaries]
    
    static member single (predicate: 'a -> bool) : IRxObservable<'a> -> IRxObservable<'a> =
        callOp "single" [predicate]
    
    static member retry(?count: int): MonoTypeOperator<'a> =
        callOp "retry" [count]
    
    static member take (count: int) : IRxObservable<'a> -> IRxObservable<'a> =
        callOp "take" [count]
    
    static member defer (observableFactory: unit -> #IRxObservable<'a>) : IRxObservable<'a> -> IRxObservable<'a> =
        callIndex "defer" [observableFactory]
    
    static member filter (predicate: 'a -> bool) : IRxObservable<'a> -> IRxObservable<'a> =
        callOp "filter" [predicate]
    
    static member first(?predicate: 'a -> bool): Operator<'a, 'a> =
        callOp "first" [predicate]
    static member first(predicate: 'a -> int -> bool): Operator<'a, 'a> =
        callOp "first" [predicate]
    
    static member observeOn(scheduler: JsTypes.SchedulerLike, ?delay: float) : MonoTypeOperator<'T> =
        callOp "observeOn" [scheduler; delay]
