module PpGen.Server.FableSignalR

open System

open FSharp.Control.Tasks.V2
open Fable.SignalR

open PpGen.DiamondSquare
open PpGen.Api.FableSignalR.DiSq


module Hub =

    open FSharp.Control.Reactive
    open PpGen.Api

    let generate seed cx cy s =
        
        let noise (x: int, y: int) =
            let input = uint64 x <<< 32 ||| uint64 y
            let bytes = BitConverter.GetBytes(input)
            let output = Standart.Hash.xxHash.xxHash64.ComputeHash(bytes, bytes.Length, seed)
            float output / float UInt64.MaxValue
        
        let chunk = Generator.generate s (cx, cy) noise
        
        chunk
        |> Array2D.mapi (fun x y h -> (x, y), h)
        |> Seq.cast<(int*int)*float>
        |> Observable.ofSeq
        
//        let points = Generator.generateReactive s (cx, cy) noise
//        points
    
//    let update (msg: Action) = task {
//    }
    
//    let send (msg: Action) (hub: FableHub<Action, Response>) = task {
//        match msg with
//        | Action.GenerateChunk msg ->
//            let { Seed = seed; ChunkX = cx; ChunkY = cy; ChunkSizeLog2 = s } = msg
//            let points = generate seed cx cy (uint s)
//            let unsub =
//                points
//                |> Observable.bufferCount 100
//                |> Observable.subscribe (fun points ->
//                    let points =
//                        points
//                        |> Seq.map (fun ((x, y), h) -> { X = x; Y = y }, h)
//                        |> Seq.toArray
//                    let response = { Points = points }
//                    let response = Response.PointsReceived response
//                    hub.Clients.Caller.Send(response) |> ignore
//                )
//            ()
//    }
    
    let invoke (msg: Action) (hub: FableHub) = task {
        return Response
    }
    
    let send (msg: Action) (hub: FableHub<Action, Response>) =
        task {
            ()
        } :> System.Threading.Tasks.Task
    
    [<RequireQualifiedAccess>]
    module Stream =

        open FSharp.Control
        
        let streamFrom (msg: StreamFrom.Action) (hub: FableHub<Action, Response>) =
            match msg with
            | StreamFrom.Action.GenerateChunk msg ->
                let { StreamFrom.Seed = seed; StreamFrom.ChunkX = cx; StreamFrom.ChunkY = cy; StreamFrom.ChunkSizeLog2 = s } = msg
//                let points = generate seed cx cy (uint s)
//                points
                Observable.generate 0 ((>) 100) ((+) 1) (fun i -> (i % 128, i / 128), float i / 100.)
                |> Observable.map ^fun ((x, y), h) ->
                    { StreamFrom.X = x; StreamFrom.Y = y; StreamFrom.Value = h }
                    |> StreamFrom.Response.PointReceived
//                |> Observable.bufferCount 64
                |> AsyncSeq.ofObservableBuffered
//                |> AsyncSeq.collect AsyncSeq.ofSeq
                |> AsyncSeq.toAsyncEnum

