module Fable.Import.RxJS.JsTypes

#nowarn "0044"

open Browser.Types
open Fable.Core
open Fable.Core.JS
open Fable.Import.Browser


[<Interface>]
type Observer<'T> =
    abstract next: value: 'T -> unit
    abstract error: err: exn -> unit
    abstract complete: unit -> unit
    // abstract closed: bool option


[<Interface>]
type NextObserver<'T> =
    abstract next: value: 'T -> unit
    abstract error: (exn -> unit) option
    abstract complete: (unit -> unit) option


[<Interface>]
type Unsubscribable =
    abstract unsubscribe: unit -> unit


[<Interface>]
type SubscriptionLike =
    inherit Unsubscribable
    abstract closed: bool with get


[<Interface>]
type Subscribable<'T> =
    abstract subscribe: Observer<'T> -> Unsubscribable


[<Erase>]
type TeardownLogic = Unsubscribable of Unsubscribable | Function of (obj -> obj) | Void of unit


[<Class>]
[<Import("Subscription", from="rxjs")>]
type Subscription(?unsubscribe: unit -> unit) =
    
    interface SubscriptionLike with
        member this.unsubscribe() = jsNative
        member this.closed = jsNative
    
    abstract unsubscribe: unit -> unit
    default _.unsubscribe() = jsNative
    
    abstract closed: bool with get
    default _.closed = jsNative
    
    abstract add: teardown: TeardownLogic -> Subscription
    default _.add(_) = jsNative
    
    abstract remove: subscription: Subscription -> unit
    default _.remove(_) = jsNative
    
    // System
    interface System.IDisposable with
        member this.Dispose() = jsNative


[<Class>]
[<Import("Subscriber", from="rxjs")>]
type Subscriber<'T> =
    inherit Subscription
    interface Observer<'T> with
        member this.next(_) = jsNative
        member this.error(_) = jsNative
        member this.complete() = jsNative
    
    abstract next: value: 'T -> unit
    default _.next(_) = jsNative
    
    abstract error: err: exn -> unit
    default _.error(_) = jsNative
    
    abstract complete: unit -> unit
    default _.complete() = jsNative
    
    // System
    interface System.IObserver<'T> with
        member this.OnNext(_) = jsNative
        member this.OnError(_) = jsNative
        member this.OnCompleted() = jsNative


[<Interface>]
type Operator<'T, 'R> =
    abstract call: subscriber: Subscriber<'R> * source: obj -> TeardownLogic


[<Class>]
[<Import("Observable", from="rxjs")>]
type Observable<'T>(?subscribe: (Subscriber<'T> -> TeardownLogic)) =
    
    interface Subscribable<'T> with
        member this.subscribe(_) = jsNative
    
    abstract subscribe: Observer<'T> -> Subscription
    default _.subscribe(_) = jsNative
    
    abstract lift<'R> : operator: Operator<'T, 'R> -> Observable<'T>
    default _.lift(_) = jsNative
    
    abstract source: Observable<obj> with get
    default _.source = jsNative
    
    abstract operator: Operator<obj, 'T> with get
    default _.operator = jsNative
    
    // System
    interface System.IObservable<'T> with
        member this.Subscribe(_) = jsNative


[<Class>]
[<Import("Subject", from="rxjs")>]
type Subject<'T>() =
    inherit Observable<'T>()
    
    interface SubscriptionLike with
        member this.closed = jsNative
        member this.unsubscribe() = jsNative
    
    abstract asObservable: Observable<'T>
    default _.asObservable = jsNative
    
    abstract next: value: 'T -> unit
    default _.next(_) = jsNative
    
    abstract error: err: exn -> unit
    default _.error(_) = jsNative
    
    abstract complete: unit -> unit
    default _.complete() = jsNative


[<Class>]
[<Import("AnonymousSubject", from="rxjs")>]
type AnonymousSubject<'T>() =
    inherit Subject<'T>()


[<StringEnum>]
type BinaryType = Blob | [<CompiledName "arraybuffer">] ArrayBuffer


type WebSocketMessage = U4<string, ArrayBuffer, Blob, ArrayBufferView>


[<Interface>]
type WebSocketSubjectConfig<'T> =
    abstract url: string
    abstract protocol: U2<string, string[]> option
    abstract resultSelector: (MessageEvent -> 'T) option
    abstract serializer: ('T -> WebSocketMessage) option
    abstract deserializer: (MessageEvent -> 'T) option
    abstract openObserver: NextObserver<Event> option
    abstract closeObserver: NextObserver<CloseEvent> option
    abstract closingObserver: NextObserver<unit> option
    abstract binaryType: BinaryType option


[<Class>]
[<Import("WebSocketSubject", from="rxjs/webSocket")>]
type WebSocketSubject<'T>(urlConfigIrSource: U3<string, WebSocketSubjectConfig<'T>, Observable<'T>>, ?destination: Observer<'T>) =
    inherit AnonymousSubject<'T>()
    
    abstract lift<'R> : operator: Operator<'T, 'R> -> WebSocketSubject<'R>
    default _.lift(_: Operator<'T, 'R>): WebSocketSubject<'R> = jsNative
    
    abstract multiplex: subMsg: (unit -> obj) * unsubMsg: (unit -> obj) * messageFilter: ('T -> bool) -> unit
    default _.multiplex(_, _, _) = jsNative



type [<AllowNullLiteral>] SchedulerAction<'T> =
//    inherit Subscription
    abstract schedule: ?state: 'T * ?delay: float -> Subscription
//    abstract ``constructor``: ?unsubscribe: (unit -> unit) -> unit
//    abstract closed: obj * Object with get, set
//    abstract unsubscribe: unit -> unit
//    abstract add: teardown: TeardownLogic -> Subscription
//    abstract remove: subscription: Subscription -> unit

[<Import("SchedulerAction", "rxjs")>]
type [<AllowNullLiteral>] SchedulerActionStatic =
    static member EMPTY with get() = jsNative: Subscription and set (_: Subscription) = jsNative


type [<AllowNullLiteral>] SchedulerLike =
    abstract now: unit -> float
    abstract schedule: work: (SchedulerAction<'T> -> 'T -> unit) * ?delay: float * ?state: 'T -> Subscription


type AnimationFrameScheduler = SchedulerLike // TODO

