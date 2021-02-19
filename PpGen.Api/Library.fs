namespace PpGen.Api

open System


module DiamondSquare =
    
    type ChunkGenerationArgs =
        { ChunkX: int
          ChunkY: int
          Seed: uint64
          ChunkSizeLog2: uint32 }
    
    type Point =
        { Height: float
          X: int; Y: int }
    
    type IGenerator =
        abstract GenerateChunk: ChunkGenerationArgs -> Async<float[,]>
        abstract GenerateChunkStream: ChunkGenerationArgs -> Async<IObservable<Point seq>>
