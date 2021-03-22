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
    
    let create seed size =
        let cs = pown 2 (int size)
        let generator cx cy = async {
            let heights = PpGen.DiamondSquare.Generator.generate size (cx, cy) (noise seed)
            let heights = Seq.cast<float> heights.[0..cs-1, 0..cs-1] |> Seq.toArray
            let chunk =
                { Heights = heights
                  Width = cs; Height = cs }
            return Observable.single chunk
        }
        HeightmapGenerator.create generator
    
    let createInterpolated seed size =
        let cs = pown 2 (int size)
        let generator cx cy = async {
            let chunks = PpGen.DiamondSquare.Generator.generateInterpolated size (cx, cy) (noise seed)
            return
                chunks
                |> Observable.map (fun chunk ->
                    let heights = Seq.cast<float> chunk.Data.[0..cs-1, 0..cs-1] |> Seq.toArray
                    { Heights = heights
                      Width = cs; Height = cs }
                )
        }
        HeightmapGenerator.create generator

type DiamondSquareHeightmapGeneratorFabric() =
    interface IDiamondSquareHeightmapGeneratorFabric with
        member this.Create(seed, size) = Generators.create seed (uint size)
        member this.CreateInterpolated(seed, size) = Generators.createInterpolated seed (uint size)
