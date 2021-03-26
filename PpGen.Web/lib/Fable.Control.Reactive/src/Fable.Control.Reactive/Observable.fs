namespace Fable.Control.Reactive

open System


[<AutoOpen>]
module private Helpers =
    
    open Fable.Core.JsInterop
    open Fable.Import.RxJS
    open Fable.Control.Reactive.TypeTransforms
    
    let callOp (operatorName: string) (args: obj seq) (source: Observable<'a>) : Observable<'b> =
        emitJsExpr (Exports.rxjs_operators, operatorName, args, source) "$0[$1](...$2)($3)"
    
    let callIndex (name: string) (args: obj seq) =
        emitJsExpr (Exports.rxjs, name, args) "$0[$1](...$2)"
    
    let inline transformOp (jsOp: Observable<'a> -> Observable<'b>) : (IObservable<'a> -> IObservable<'b>) =
        let sysOp (sysObsA: IObservable<'a>) : IObservable<'b> =
            let sysjsObsA = TT.Observable.sysAsSysjs sysObsA
            let jsObsB = jsOp sysjsObsA
            let sysjsObsB = TT.Observable.jsToSysjs jsObsB
            upcast sysjsObsB
        sysOp


[<RequireQualifiedAccess>]
module Observable =
    
    open Fable.Control.Reactive.TypeTransforms
    
    let single (value: 'a) : IObservable<'a> =
        upcast TT.Observable.jsToSysjs (callIndex "of" [value])
    
    let map (f: 'a -> 'b) (source: IObservable<'a>) : IObservable<'b> =
        transformOp (callOp "map" [f]) source
    
    let subscribe (onNext: 'a -> unit) (observable: IObservable<'a>) =
        observable.Subscribe(onNext)
