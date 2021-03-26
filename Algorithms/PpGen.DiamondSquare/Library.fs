namespace PpGen.DiamondSquare

open System
open System.Collections.Generic
open GenericNumber


type PointPosition = (struct (int * int))
type ChunkPosition = (struct (int * int))
type AreaPosition = (struct (int * int))

type Point = (struct (PointPosition * float))

type Chunk<'a> =
    { Data: 'a[,] }


module Point =
    
    let inline create x y h : Point = struct(struct(x, y), h)


type Noise = int * int -> float //PointPosition -> float


module Step =
    
    let inline private s2cs (s: uint) = pown (1G+1G) (int s)
    let inline private s2cs1 (s: uint) = s2cs s + 1G
    
    let square getPoint setPoint s (iter: uint) =
        let cs = s2cs s
        let r = pown 2 (s - iter |> int) / 2
        
        let points = seq {
            for x in -r .. 2*r .. cs + r do
                for y in -r .. 2*r .. cs + r ->
                    struct(x, y)
        }
        
        for x, y in points do
            let sum =
                getPoint (x - r, y - r) +
                getPoint (x - r, y + r) +
                getPoint (x + r, y - r) +
                getPoint (x + r, y + r)
            let avg = sum / 4.
            let value = avg
            setPoint (x, y) value
    
    
    let diamond getPoint setPoint s (iter: uint) =
        let cs = s2cs s
        let r = pown 2 (s - iter |> int) / 2
        
        let points: struct(_*_) seq = seq {
            for x in 0 .. r*2 .. cs -> x,  0 - r
            for x in 0 .. r*2 .. cs -> x, cs + r
            
            for y in 0 .. r*2 .. cs ->  0 - r, y
            for y in 0 .. r*2 .. cs -> cs + r, y
            
            for y in 0 .. r .. cs do
                let xStart = if (y / r) % 2 = 0 then r else 0
                for x in xStart .. r*2 .. cs do
                    yield x, y
        }
        
        for x, y in points do
            let sum =
                getPoint (x - r, y) +
                getPoint (x + r, y) +
                getPoint (x, y - r) +
                getPoint (x, y + r)
            let avg = sum / 4.
            let value = avg
            setPoint (x, y) value
    
    
    let init setPoint s =
        let cs = s2cs s
        let points = seq {
            for x in -cs .. cs .. 2*cs do
                for y in -cs .. cs .. 2*cs do
                    struct(x, y)
        }
        for x, y in points do
            setPoint (x, y)


module Generator =

    open System.Threading
    
    type GenArea<'a> =
        { Chunk: Chunk<'a>
          Outside: IDictionary<PointPosition, 'a>
          ChunkSize: int }
    
    module GenArea =
        
        let createEmpty (s: uint) defaultValue =
            let cs = pown 2 (int s)
            let chunk = { Data = Array2D.create (cs + 1) (cs + 1) defaultValue }
            { Chunk = chunk
              Outside = Dictionary()
              ChunkSize = cs }
        
        let setPoint (area: GenArea<_>) (x, y) value =
            let cs = area.ChunkSize
            assert (x >= -cs && x <= 2*cs &&
                    y >= -cs && y <= 2*cs)
            if (x >= 0 && x <= cs) && (y >= 0 && y <= cs) then
                area.Chunk.Data.[x, y] <- value
            else
                area.Outside.[(x, y)] <- value
        
        let getPoint (area: GenArea<_>) (x, y) =
            let cs = area.ChunkSize
            assert (x >= -cs && x <= 2*cs &&
                    y >= -cs && y <= 2*cs)
            // If the point is inside the chunk
            if (x >= 0 && x <= cs) && (y >= 0 && y <= cs) then
                area.Chunk.Data.[x, y]
            else
                area.Outside.[(x, y)]

    
    
    let applyNoise h q (r: int) (i: uint) =
        let i, r = int i, int r
        let q' = q * 2. - 1.
        
        let t = 0.5
        let a = 0.5
        
        let factor = (1. - t) ** (float i - a)
        let d = q' * factor |> min 1. |> max -1.
//        sqrt (h * 2. - 1. |> abs) * (sign h |> float) + d
        h + d
//        q
    
    // TODO: Normalize values
    let pseudoNormalize h = (h + 1.5) / 4. |> min 1. |> max 0.
    
    let generate (s: uint) (cx, cy) (noise: Noise) =
        let area = GenArea.createEmpty s nan
        let cs = area.ChunkSize
        
        let getPoint (x, y) =
            GenArea.getPoint area (x, y)
        
        let setPoint i (x, y) value =
            let gx, gy = cx * cs + x, cy * cs + y
            let r = pown 2 (s - i |> int) / 2
            let q = noise (gx, gy)
            let value = applyNoise value q r i
            GenArea.setPoint area (x, y) value
        
        let setInitPoint (x, y) =
            let value = noise (x, y)
            GenArea.setPoint area (x, y) value
        
        Step.init setInitPoint s
        for i in 0u .. s - 1u do
            Step.square  getPoint (setPoint i) s i
            Step.diamond getPoint (setPoint i) s i
        
        area.Chunk.Data
        |> Array2D.map pseudoNormalize
    
    let generateReactive (s: uint) (cx, cy) (noise: Noise) =
        
        let sub (obv: IObserver<PointPosition * float>) =
        
            let area = GenArea.createEmpty s nan
            let cs = area.ChunkSize
            
            let dispatchPoint (x, y) h =
                let gx, gy = cx * cs + x, cy * cs + y
                let h = pseudoNormalize h
                obv.OnNext((gx, gy), h)
            
            let getPoint (x, y) =
                GenArea.getPoint area (x, y)
            
            let setPoint i (x, y) value =
                let gx, gy = cx * cs + x, cy * cs + y
                let r = pown 2 (s - i |> int) / 2
                let q = noise (gx, gy)
                let value = applyNoise value q r i
                dispatchPoint (x, y) value
                GenArea.setPoint area (x, y) value
            
            let setInitPoint (x, y) =
                let gx, gy = x + cx * cs, y + cy * cs
                let value = noise (gx, gy)
                dispatchPoint (x, y) value
                GenArea.setPoint area (x, y) value
            
            let cts = new CancellationTokenSource()
            
            let work = async {
                Step.init setInitPoint s
                for i in 0u .. s - 1u do
                    Step.square  getPoint (setPoint i) s i
                    Step.diamond getPoint (setPoint i) s i
                obv.OnCompleted()
            }
            
            Async.Start(work, cts.Token)
            
            { new IDisposable with
                member _.Dispose() =
                    cts.Cancel()
                    cts.Dispose()
            }
        
        { new IObservable<PointPosition * float> with member _.Subscribe(obv) = sub obv }
    
    
    let generateInterpolated (s: uint) (cx, cy) (noise: Noise) : IObservable<Chunk<float>> =
        
        let sub (obv: IObserver<Chunk<float>>) =
            
            let dispatchChunk (chunk: Chunk<float>) =
                let chunk =
                    { chunk with
                        Data =
                            chunk.Data
                            |> Array2D.map pseudoNormalize
                    }
                obv.OnNext(chunk)
            
            let interpolateArea (iter: uint) (sourceArea: GenArea<float>) : Chunk<float> =
                let destArea =
                    { Chunk = { Data = Array2D.copy sourceArea.Chunk.Data }
                      Outside = Dictionary(sourceArea.Outside)
                      ChunkSize = sourceArea.ChunkSize }
                let s = log2 (float sourceArea.ChunkSize) |> uint
                
                let getPoint (x, y) =
                    GenArea.getPoint destArea (x, y)
                
                let setPoint (x, y) h =
                    GenArea.setPoint destArea (x, y) h
                
                for i in iter .. s-1u do
                    Step.square  getPoint setPoint s i
                    Step.diamond getPoint setPoint s i
                
                destArea.Chunk
            
            let area = GenArea.createEmpty s nan
            let cs = area.ChunkSize
            
            let getPoint (x, y) =
                GenArea.getPoint area (x, y)
            
            let setPoint i (x, y) h =
                let gx, gy = cx * cs + x, cy * cs + y
                let r = pown 2 (s - i |> int) / 2
                let q = noise (gx, gy)
                let h = applyNoise h q r i
                GenArea.setPoint area (x, y) h
            
            let setInitPoint (x, y) =
                let gx, gy = cx * cs + x, cy * cs + y
                let value = noise (gx, gy)
                GenArea.setPoint area (x, y) value
            
            let cts = new CancellationTokenSource()
            
            let work = async {
                Step.init setInitPoint s
                for i in 0u .. s - 1u do
                    Step.square  getPoint (setPoint i) s i
                    area |> interpolateArea i |> dispatchChunk
                    Step.diamond getPoint (setPoint i) s i
                    area |> interpolateArea i |> dispatchChunk
                obv.OnCompleted()
            }
            
            Async.Start(work, cts.Token)
            
            { new IDisposable with
                member _.Dispose() =
                    cts.Cancel()
                    cts.Dispose()
            }
        
        { new IObservable<Chunk<float>> with member _.Subscribe(obv) = sub obv }
