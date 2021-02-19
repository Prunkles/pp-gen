module PpGen.Server.FableRemoting

open System
open PpGen.Api.FableRemoting
open PpGen.DiamondSquare


let generateDiSq (ps: DiSqParams) = async {
    let s = ps.ChunkSizeLog2
    let cs = pown 2 (int s)
    let seed = ps.Seed
    
    let cxg, cyg = ps.ChunkPosition.X, ps.ChunkPosition.Y
    
    let noise (x: int, y: int) =
        let input = uint64 x <<< 32 ||| uint64 y
        let bytes = BitConverter.GetBytes(input)
        let output = Standart.Hash.xxHash.xxHash64.ComputeHash(bytes, bytes.Length, seed)
        float output / float UInt64.MaxValue
    
    let chunk = Generator.generate s (cxg, cyg) noise
    let chunk = chunk.[0 .. cs, 0 .. cs]
    
    let data =
        chunk
        |> Seq.cast<float>
        |> Seq.splitInto (cs+1)
        |> Seq.toArray
    
    return data
}

let foo s = async { return $"foo {s}" }

let api: IApi =
    { GenerateDiSqImage = generateDiSq
      Foo = foo }