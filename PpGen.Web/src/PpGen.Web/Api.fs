module PpGen.Web.Api

open Fable.Remoting.Client
open PpGen.Api
open PpGen.Api.DiamondSquare


let api =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder FableRemoting.Route.build
    |> Remoting.buildProxy<FableRemoting.IApi>

module Observable =

    open System
    open System.Collections.Generic
    open FSharp.Control
    
    let subject () =
        let mutable obvMut: IObserver<'a> = null
        let obv =
            { new IObserver<'a> with
                member _.OnNext(x) =
                    if obvMut <> null then obvMut.OnNext(x)
                member _.OnCompleted() =
                    if obvMut <> null then obvMut.OnCompleted()
                member _.OnError(e) =
                    if obvMut <> null then obvMut.OnError(e)
            }
        let obs =
            { new IObservable<'a> with
                member _.Subscribe(obv') =
                    if obvMut = null
                    then obvMut <- obv'
                    else invalidOp ""
                    Disposable.Empty
            }
        obv, obs
    
    /// A mailbox subject is a subscribable mailbox. Each message is broadcasted to all subscribed observers.
    let mbSubject<'TSource> () : MailboxProcessor<Notification<'TSource>> * IObservable<'TSource> =
        let obvs = ResizeArray<IObserver<'TSource>>()
        let cts = new System.Threading.CancellationTokenSource()

        let mb = MailboxProcessor.Start(fun inbox ->
            let rec messageLoop _ = async {
                let! n = inbox.Receive ()
                for aobv in obvs do
                    match n with
                    | OnNext x ->
                        try
                            aobv.OnNext x
                        with ex ->
                            aobv.OnError ex
                            cts.Cancel ()
                    | OnError err ->
                        aobv.OnError err
                        cts.Cancel ()
                    | OnCompleted ->
                        aobv.OnCompleted ()
                        cts.Cancel ()

                return! messageLoop ()
            }
            messageLoop ()
        , cts.Token)

        let subscribe (aobv: IObserver<'TSource>) : IDisposable =
            let sobv = aobv //safeObserver aobv Disposable.Empty
            obvs.Add sobv

            let cancel () =
                obvs.Remove sobv |> ignore
            
            Disposable.Create cancel

        mb, { new IObservable<'TSource> with member __.Subscribe o = subscribe o }


    /// A stream is both an observable sequence as well as an observer.
    /// Each notification is broadcasted to all subscribed observers.
//    let subject<'TSource> () : IObserver<'TSource> * IObservable<'TSource> =
//        let mb, obs = mbSubject<'TSource> ()
//
//        let obv = { new IObserver<'TSource> with
//            member this.OnNext x =
//                OnNext x |> mb.Post
//            member this.OnError err =
//                OnError err |> mb.Post
//            member this.OnCompleted () =
//                OnCompleted |> mb.Post
//        }
//
//        obv, obs
    
module AsyncRx =

    open System.Collections.Generic
    open FSharp.Control

    let bufferCount (count: int) (source: IAsyncObservable<'a>) : IAsyncObservable<IList<'a>> =
        { new IAsyncObservable<IList<'a>> with
            member _.SubscribeAsync(obv) = async {
                let mutable buffer = null
                let mutable index = 0
                let obv' =
                    { new IAsyncObserver<'a> with
                        member _.OnNextAsync(x) = async {
                            if isNull buffer then
                                buffer <- ResizeArray()
                            buffer.Add(x)
                            let idx = index + 1
                            if idx = count then
                                index <- 0
                                do! obv.OnNextAsync(buffer)
                                buffer <- null
                            else
                                index <- idx
                        }

                        member _.OnErrorAsync(e) = async {
                            buffer <- null
                            do! obv.OnErrorAsync(e)
                        }

                        member _.OnCompletedAsync() = async {
                            let buffer' = buffer
                            buffer <- null
                            if buffer' <> null then
                                do! obv.OnNextAsync(buffer')
                            do! obv.OnCompletedAsync()
                        }
                    }
                return! source.SubscribeAsync(obv')
            }
        }



module Generator =

    open Browser
    open FSharp.Control
    open Fable.Core
    open Fable.Core.JsInterop
    
    let generateChunkStream seed s cx cy = async {
        let ws = WebSocket.Create("ws://localhost:8085/api/disq/stream")
        
//        let obv, points = Observable.subject ()
        let obv, points = AsyncRx.subject ()
        let obv = obv.ToObserver()
        
        ws.onopen <- fun event ->
            let args =
                 { Seed = seed
                   ChunkX = cx
                   ChunkY = cy
                   ChunkSizeLog2 = s }
            let json = JS.JSON.stringify(args)
            ws.send(json)
        
        ws.binaryType <- "arraybuffer"
        ws.onmessage <- fun event ->
            let data: int32[] = !!JS.Constructors.Int32Array.Create(event.data?slice(0, 8) : obj)
            let x = data.[0]
            let y = data.[1]
            let data: float[] = !!JS.Constructors.Float64Array.Create(event.data?slice(8, 16) : obj)
            let h = data.[0]
            let point = { Height = h; X = x; Y = y }
            obv.OnNext(point)
//            printfn "OnNext'ed"
        ws.onclose <- fun event ->
            printfn $"Chunk data at ({cx}, {cy})C received"
            obv.OnCompleted()
        
        ws.onerror <- fun event ->
            obv.OnError(System.Exception(event.``type``))
        
        return
            points //.ToAsyncObservable()
            |> AsyncRx.bufferCount (pown 2 (int s * 3))
            |> AsyncRx.map seq //Seq.singleton
            |> AsyncRx.toObservable
    }


type Generator() =
    interface IGenerator with
        member this.GenerateChunk(var0) = failwith "todo"
        member this.GenerateChunkStream(args) = Generator.generateChunkStream args.Seed args.ChunkSizeLog2 args.ChunkX args.ChunkY


module MockGenerator =

    open System
    open FSharp.Control
    
    let generateChunkStream (seed: uint64) (s: uint) cx cy = async {
        let cs = pown 2 (int s)
        let points =
            AsyncRx.ofSeq (seq { 0 .. cs })
            |> AsyncRx.map ^fun y ->
                seq {
                    for x in 0 .. cs do
                        let h = (sin (float x / 10.) + cos (float y / 10.)) / 4. + 1.
                        let x, y = x + cx * cs, y + cy * cs
                        let point = { Height = h; X = x; Y = y }
                        yield point
                }
            |> AsyncRx.toObservable
        return points
    }


type MockGenerator() =
    member this.GenerateChunkStream(args) = (this :> IGenerator).GenerateChunkStream(args)
    interface IGenerator with
        member this.GenerateChunk(args) = failwith "todo"
        member this.GenerateChunkStream(args) = MockGenerator.generateChunkStream args.Seed args.ChunkSizeLog2 args.ChunkX args.ChunkY



