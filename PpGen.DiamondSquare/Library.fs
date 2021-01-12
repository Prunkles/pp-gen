namespace PpGen.DiamondSquare

open System
open System.Collections.Generic
open PpGen.Utils
open GenericNumber


type PointPosition = (struct (int * int))
type ChunkPosition = (struct (int * int))
type AreaPosition = (struct (int * int))


type CompleteChunk<'a> =
    { Data: 'a[,] }

type PartialChunk<'a> =
    { Data: IDictionary<PointPosition, 'a> }
    member this.IsEmpty = this.Data.Count = 0

type Chunk<'a> =
    | Partial of PartialChunk<'a>
    | Complete of CompleteChunk<'a>


module Chunk =
    let getPoint chunk (x, y) =
        match chunk with
        | Complete chunk -> chunk.Data.[x, y]
        | Partial chunk -> chunk.Data.[(x, y)]
    
    let setPoint chunk (pxc, pyc) value =
        match chunk with
        | Complete chunk -> chunk.Data.[pxc, pyc] <- value
        | Partial chunk -> chunk.Data.[(pxc, pyc)] <- value
    
//    let hasPoint chunk (pxc, pyc) =
//        match chunk with
//        | Partial chunk -> chunk.Data.ContainsKey((pxc, pyc))
//        | Complete chunk ->
//            chunk.Data.[pxc, pyc] |> System.Double.IsNaN //Unchecked.defaultof<_>


type DiSqMap<'a> =
    { Chunks: IDictionary<ChunkPosition, Chunk<'a>>
      ChunkSizeLog2: uint }

module DiSqMap =
    let create chunkSizeLog2 =
        { ChunkSizeLog2 = chunkSizeLog2
          Chunks = Dictionary() }

    
    let chunks (map: DiSqMap<'a>) = seq {
        for KeyValue (pos, chunk) in map.Chunks -> struct(pos, chunk)
    }
    let points (map: DiSqMap<'a>) : struct(PointPosition * 'a) seq = seq {
        let cs = pown 2 (int map.ChunkSizeLog2)
        let inline pc2pg cx px = cx * cs + px
        for (cxg, cyg), chunk in chunks map do
            match chunk with
            | Complete chunk ->
                let points = chunk.Data
                for pxc in 0..(cs - 1) do
                    for pyc in 0..(cs - 1) do
                        let value = points.[pxc, pyc]
                        yield (pc2pg cxg pxc, pc2pg cyg pyc), value
            | Partial chunk ->
                let points = chunk.Data
                for (KeyValue ((pxc, pyc), value)) in points do
                    yield (pc2pg cxg pxc, pc2pg cyg pyc), value
    }


type Noise = int * int -> float //PointPosition -> float

type IDiSqMapGenerator<'a> =
    abstract GenerateChunk: DiSqMap<'a> -> ChunkPosition -> unit



module Generator =

    open System.Collections.ObjectModel
    
    type GenArea<'a> =
        { CN1N1: Chunk<'a>; CZ0N1: Chunk<'a>; CP1N1: Chunk<'a>
          CN1Z0: Chunk<'a>; CZ0Z0: Chunk<'a>; CP1Z0: Chunk<'a>
          CN1P1: Chunk<'a>; CZ0P1: Chunk<'a>; CP1P1: Chunk<'a> }
        member this.Item(x, y) =
            match x, y with
            | -1, -1 -> this.CN1N1 | 0, -1 -> this.CZ0N1 | 1, -1 -> this.CP1N1
            | -1,  0 -> this.CN1Z0 | 0,  0 -> this.CZ0Z0 | 1,  0 -> this.CP1Z0
            | -1,  1 -> this.CN1P1 | 0,  1 -> this.CZ0P1 | 1,  1 -> this.CP1P1
            | _ -> invalidOp "" //$"Index {(x, y)}"
    
    module GenArea =
        
        let ofDict (vals: IReadOnlyDictionary<struct(_ * _), _>) =
            { CN1N1 = vals.[-1, -1]; CZ0N1 = vals.[0, -1]; CP1N1 = vals.[1, -1]
              CN1Z0 = vals.[-1,  0]; CZ0Z0 = vals.[0,  0]; CP1Z0 = vals.[1,  0]
              CN1P1 = vals.[-1,  1]; CZ0P1 = vals.[0,  1]; CP1P1 = vals.[1,  1] }
        
        let setPoint (area: GenArea<_>) cs (pxa, pya) value =
            assert (pxa > -cs && pxa < cs*2 &&
                    pya > -cs && pya < cs*2)
            let cxa, cya =
                (pxa + cs) / cs - 1, // pxa / cs
                (pya + cs) / cs - 1  // pya / cs
            let pxc, pyc =
                (pxa + cs) % cs, // pxa % cs
                (pya + cs) % cs  // pya % cs
            match (pxc, pyc) with
            // Intersection points
            | 0, 0 ->
                Chunk.setPoint area.[cxa - 0, cya - 0] ( 0,  0) value
                Chunk.setPoint area.[cxa - 0, cya - 1] ( 0, cs) value
                Chunk.setPoint area.[cxa - 1, cya - 0] (cs,  0) value
                Chunk.setPoint area.[cxa - 1, cya - 1] (cs, cs) value
            // Vertical edge
            | 0, pyc ->
                Chunk.setPoint area.[cxa - 0, cya - 0] ( 0, pyc) value
                Chunk.setPoint area.[cxa - 1, cya - 0] (cs, pyc) value
            // Horizontal edge
            | pxc, 0 ->
                Chunk.setPoint area.[cxa - 0, cya - 0] (pxc,  0) value
                Chunk.setPoint area.[cxa - 0, cya - 1] (pxc, cs) value
            // Point inside the chunk
            | pxc, pyc ->
                Chunk.setPoint area.[cxa, cya] (pxc, pyc) value
        
        let getPoint (area: GenArea<_>) cs (pxa, pya) =
            assert (pxa >= -cs && pxa <= cs*2 &&
                    pya >= -cs && pya <= cs*2)
            let cxa, cya =
                (pxa + cs) / cs - 1, // pxa / cs
                (pya + cs) / cs - 1  // pya / cs
            let pxc, pyc =
                (pxa + cs) % cs, // pxa % cs
                (pya + cs) % cs  // pya % cs
            match cxa, cya with
            | 2, 2 -> Chunk.getPoint area.[1, 1] (cs, cs)
            | 2, cya -> Chunk.getPoint area.[1, cya] (cs, pyc)
            | cxa, 2 -> Chunk.getPoint area.[cxa, 1] (pxc, cs)
            | cxa, cya -> Chunk.getPoint area.[cxa, cya] (pxc, pyc)
        
    
    
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
    
    
    let inline chunkSizeBySizeLog2 (sl2: uint) = pown (1G+1G) (int sl2) + 1G
    
    
    let stepSquare (area: GenArea<float>) csLog2 (iter: uint) (noise: Noise) ((axg, ayg)) =
        let cs1 = chunkSizeBySizeLog2 csLog2
        let cs = cs1 - 1
        let iter' = csLog2 - iter
        let r = pown 2 (iter' |> int) / 2
        
        let points = seq {
            for x in -r .. r*2 .. cs + r do
                for y in -r .. r*2 .. cs + r ->
                    struct (x, y)
        }
        for pxa, pya in points do
            let sum =
                GenArea.getPoint area cs (pxa - r, pya - r) +
                GenArea.getPoint area cs (pxa - r, pya + r) +
                GenArea.getPoint area cs (pxa + r, pya - r) +
                GenArea.getPoint area cs (pxa + r, pya + r)
            let avg = sum / 4.
            let pxg, pyg = (axg * cs) + pxa, (ayg * cs) + pya
            let value = applyNoise avg (noise (pxg, pyg)) r iter
            GenArea.setPoint area cs (pxa, pya) value
    
    
    let stepDiamond (area: GenArea<float>) csLog2 (iter: uint) (noise: Noise) ((axg, ayg)) =
        let cs1 = chunkSizeBySizeLog2 csLog2
        let cs = cs1 - 1
        let iter' = csLog2 - iter
        let r = pown 2 (iter' |> int) / 2
        
        let points = seq {
            yield! seq { for x in 0 .. r*2 .. cs -> struct(x, -r) }
            yield! seq { for x in 0 .. r*2 .. cs -> struct(x, cs + r) }
            
            yield! seq { for y in 0 .. r*2 .. cs -> struct(-r, y) }
            yield! seq { for y in 0 .. r*2 .. cs -> struct(cs + r, y) }
            
            yield! seq {
                for y in 0 .. r .. cs do
                    let xStart = if (y / r) % 2 = 0 then r else 0
                    for x in xStart .. r*2 .. cs do
                        struct(x, y)
            }
        }
        
        for pxa, pya in points do
            let sum =
                GenArea.getPoint area cs (pxa - r, pya) +
                GenArea.getPoint area cs (pxa + r, pya) +
                GenArea.getPoint area cs (pxa, pya - r) +
                GenArea.getPoint area cs (pxa, pya + r)
            let avg = sum / 4.
            let pxg, pyg = (axg * cs) + pxa, (ayg * cs) + pya
            let value = applyNoise avg (noise (pxg, pyg)) r iter
            GenArea.setPoint area cs (pxa, pya) value
    
    
    let stepInit (area: GenArea<float>) csLog2 (noise: Noise) ((axg, ayg)) =
        let cs1 = chunkSizeBySizeLog2 csLog2
        let cs = cs1 - 1
        for cxa in -1..1 do
            for cya in -1..1 do
                let chunk = area.[cxa, cya]
                let cxg, cyg = axg + cxa, ayg + cya
                let pxg, pyg = cxg * cs, cyg * cs
                Chunk.setPoint chunk ( 0,  0) (noise (pxg +  0, pyg +  0)) //(noise (pxg, pyg))
                Chunk.setPoint chunk (cs,  0) (noise (pxg + cs, pyg +  0)) //(noise (pxg + cs, pyg))
                Chunk.setPoint chunk ( 0, cs) (noise (pxg +  0, pyg + cs)) //(noise (pxg, pyg + cs))
                Chunk.setPoint chunk (cs, cs) (noise (pxg + cs, pyg + cs)) //(noise (pxg + cs, pyg + cs))
    
    let generateChunk csLog2 (map: DiSqMap<float>) ((cxg, cyg)) (noise: Noise) =
        let cs1 = chunkSizeBySizeLog2 csLog2
        
        let getChunkOrCreate (cxg, cyg) =
            match map.Chunks.TryGetValue((cxg, cyg)) with
            | false, _ -> Partial { Data = Dictionary() }
            | true, chunk -> chunk
        
        let area: GenArea<_> =
            dict [
                for cxa in -1..1 do
                    for cya in -1..1 do
                        struct (cxa, cya),
                        if cxa = 0 && cya = 0
                        then Complete { Data = Array2D.create cs1 cs1 Double.NaN }
                        else getChunkOrCreate (cxg + cxa, cyg + cya)
            ]
            |> ReadOnlyDictionary
            |> GenArea.ofDict
        
        let axg, ayg = cxg, cyg
        
        stepInit area csLog2 noise (axg, ayg)
        for iter in 0 .. int csLog2 - 1 do
            stepSquare area csLog2 (uint iter) noise (axg, ayg)
            stepDiamond area csLog2 (uint iter) noise (axg, ayg)
        
        for cxa in -1..1 do
            for cya in -1..1 do
                let chunk = area.[cxa, cya]
                map.Chunks.[(cxg + cxa, cyg + cya)] <- chunk
