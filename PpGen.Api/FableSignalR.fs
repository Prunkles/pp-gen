module PpGen.Api.FableSignalR

module DiSq =
    
    type Action = Action
    
    type Response = Response
    
    module StreamFrom =
        
        type GenerateChunkAction =
            { ChunkX: int
              ChunkY: int
              Seed: uint64
              ChunkSizeLog2: int }
        
        type Action =
            | GenerateChunk of GenerateChunkAction
        
        
        type PointReceivedResponse =
            { X: int; Y: int
              Value: float }
        
        type Response =
            | PointReceived of PointReceivedResponse
        
        
    let endpoint = "/api/disq/signalr"