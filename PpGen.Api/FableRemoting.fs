module PpGen.Api.FableRemoting



type DiSqParams =
    { ChunkSizeLog2: uint
      Seed: uint64
      ChunkPosition: Position2 }

type HeightmapDto = float[][]

type IApi =
    { GenerateDiSqImage: DiSqParams -> Async<HeightmapDto>
      Foo: string -> Async<string> }

module Route =
    let build typeName methodName =
        $"/api/%s{typeName}/%s{methodName}"
