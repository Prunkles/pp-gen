namespace PpGen.Api

type IApi =
    { GenerateDiSq: unit -> Async<float[,]> }
