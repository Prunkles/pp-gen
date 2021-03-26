namespace PpGen.Api

open System


type Chunk =
    { Heights: float32[]
      Width: int
      Height: int }

type IHeightmapGenerator =
    abstract GenerateChunk: x: int * y: int -> Async<IObservable<Chunk>>


module Fabrics =
    
    type IDiamondSquareHeightmapGeneratorFabric =
        abstract Create: seed: uint64 * size: byte -> IHeightmapGenerator
        abstract CreateInterpolated: seed: uint64 * size: byte -> IHeightmapGenerator
    
    type IPerlinNoiseHeightmapGeneratorFabric =
        abstract Create: seed: uint64 * chunkSize: int -> IHeightmapGenerator
