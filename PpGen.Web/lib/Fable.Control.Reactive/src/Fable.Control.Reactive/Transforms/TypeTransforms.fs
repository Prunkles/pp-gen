module internal Fable.Control.Reactive.TypeTransforms

open System
open Fable.Core

[<RequireQualifiedAccess>]
module TT =
    
    open Fable.Core.JsInterop
    open Fable.System.Reactive
    open Fable.Import.RxJS
    
    module Observer =

        let js2sys (jsObv: Observer<'a>) : IObserver<'a> =
            { new IObserver<_> with
                member _.OnNext(x) = jsObv.next(x)
                member _.OnError(err) = jsObv.error(err)
                member _.OnCompleted() = jsObv.complete()
            }
        
        let sys2js (obv: IObserver<'a>) : Observer<'a> =
            { new Observer<'a> with
                member _.next(x) = obv.OnNext(x)
                member _.error(err) = obv.OnError(err)
                member _.complete() = obv.OnCompleted()
            }
        
        type ISysJsObserver<'a> =
            inherit IObserver<'a>
            inherit Observer<'a>
        
        let sys2sysjs (sysObv: IObserver<'a>) : ISysJsObserver<'a> =
            { new ISysJsObserver<'a> with
                member _.OnNext(x) = sysObv.OnNext(x)
                member _.OnError(e) = sysObv.OnError(e)
                member _.OnCompleted() = sysObv.OnCompleted()
                member _.next(x) = sysObv.OnNext(x)
                member _.error(e) = sysObv.OnError(e)
                member _.complete() = sysObv.OnCompleted()
            }


    module Disposable =
        
        let js2sys (jsDisp: Unsubscribable) : IDisposable =
            { new IDisposable with
                member _.Dispose() = jsDisp.unsubscribe() }
        
        let sys2js (sysDisp: IDisposable) : Subscription =
            Subscription(sysDisp.Dispose)
       
        type SysJsDisposable() =
            inherit Subscription()
            interface IDisposable with
                member this.Dispose() = this.unsubscribe()


    module Observable =
        
        let sysToJs (sysObs: IObservable<'a>) : Observable<'a> =
            let subscribe (subscriber: Subscriber<'a>) : TeardownLogic =
                let sysObv = Observer.js2sys subscriber
                let sysDisp = sysObs.Subscribe(sysObv)
                let jsDisp = Disposable.sys2js sysDisp
                TeardownLogic.Unsubscribable jsDisp
            Observable(subscribe)
        
        let jsToSys (jsObs: Subscribable<'a>) : IObservable<'a> =
            let subscribe (sysObv: IObserver<'a>) : IDisposable =
                let jsObv = Observer.sys2js sysObv
                let jsDisp = jsObs.subscribe(jsObv)
                let sysDisp = Disposable.js2sys jsDisp
                sysDisp
            { new IObservable<'a> with member _.Subscribe(sysObv) = subscribe sysObv }
        
        [<AbstractClass>]
        type SysJsObservable<'a>(jsSubscribe) =
            inherit Observable<'a>(jsSubscribe)
            abstract Subscribe: IObserver<'a> -> IDisposable
            interface IObservable<'a> with
                member this.Subscribe(sysObv) = this.Subscribe(sysObv)
        
        let sysToSysjs (sysObs: IObservable<'a>) : SysJsObservable<'a> =
            let jsSubscribe (jsSubscriber: Subscriber<'a>) : TeardownLogic =
                let jsObs = sysToJs sysObs
                TeardownLogic.Unsubscribable (jsObs.subscribe(jsSubscriber))
            let sysSubscribe (sysObv: IObserver<'a>) : IDisposable =
                sysObs.Subscribe(sysObv)
            { new SysJsObservable<'a>(jsSubscribe) with member _.Subscribe(sysObv) = sysSubscribe sysObv }
        
        let jsToSysjs (jsObs: Observable<'a>) : SysJsObservable<'a> =
            let jsSubscribe (jsSubscriber: Subscriber<'a>) : TeardownLogic =
                TeardownLogic.Unsubscribable (jsObs.subscribe(jsSubscriber))
            let sysSubscribe (sysObv: IObserver<'a>) : IDisposable =
                let sysObs = jsToSys jsObs
                sysObs.Subscribe(sysObv)
            { new SysJsObservable<'a>(jsSubscribe) with member _.Subscribe(sysObv) = sysSubscribe sysObv }
        
        
        let sysIsJs (sysObs: IObservable<'a>) : bool =
            jsInstanceof sysObs (Exports.rxjs?Observable)
        
        /// Check if sysObs is already Observable, if is then return without changes else call sys2sysjs and return
        let sysAsSysjs (sysObs: IObservable<'a>) : SysJsObservable<'a> =
            if sysIsJs sysObs then !!sysObs else sysToSysjs sysObs
        
        let jsIsSys (jsObs: Observable<'a>) : bool =
            isIn "Subscribe" jsObs
        
        /// Check if jsObs is already IObservable, if is then return without changes else call js2sysjs and return
        let jsAsSysjs (jsObs: Observable<'a>) : SysJsObservable<'a> =
            if jsIsSys jsObs then !!jsObs else jsToSysjs jsObs
