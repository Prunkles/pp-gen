namespace PpGen.Server.Generators

open PpGen.Api

module HeightmapGenerator =
    let create generator = { new IHeightmapGenerator with member _.GenerateChunk(x, y) = generator x y }


namespace PpGen.Server.Generators.DiamondSquare

open FSharp.Control.Reactive
open PpGen.Api
open PpGen.Api.Fabrics
open PpGen.Server.Generators

module Generators =

    let noise seed (x: int, y: int) =
        let input = uint64 x <<< 32 ||| uint64 y
        let bytes = System.BitConverter.GetBytes(input)
        let output = Standart.Hash.xxHash.xxHash64.ComputeHash(bytes, bytes.Length, seed)
        float output / float System.UInt64.MaxValue
    
    let generate seed size =
        let generator cx cy = async {
            let heights = PpGen.DiamondSquare.Generator.generate size (cx, cy) (noise seed)
            let chunk =
                { Heights = Seq.cast<float> heights |> Seq.toArray
                  Width = pown 2 (int size); Height = pown 2 (int size) }
            return Observable.single chunk
        }
        HeightmapGenerator.create generator
    
    let generateInterpolated seed size =
        let generator cx cy = async {
            let chunks = PpGen.DiamondSquare.Generator.generateInterpolated size (cx, cy) (noise seed)
            return
                chunks
                |> Observable.map (fun chunk ->
                    { Heights = Seq.cast<float> chunk.Data |> Seq.toArray
                      Width = pown 2 (int size); Height = pown 2 (int size) }
                )
        }
        HeightmapGenerator.create generator

type DiamondSquareHeightmapGeneratorFabric() =
    interface IDiamondSquareHeightmapGeneratorFabric with
        member this.Create(seed, size) = Generators.generate seed (uint size)
        member this.CreateInterpolated(seed, size) = Generators.generateInterpolated seed (uint size)
