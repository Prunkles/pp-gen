module PpGen.Server.DiSq

open System
open System.Collections.Generic
open PpGen
open PpGen.DiamondSquare
open PpGen.Utils


let noise =
    let c = 0xD7E4E6CDEE0C0FL |> int
    let rng = Random(c)
    let cache = Dictionary<struct(_ * _), float>()
    fun (x, y) ->
        match cache.TryGetValue((x, y)) with
        | true, value -> value
        | false, _ ->
            let value = rng.NextDouble()
            cache.[(x, y)] <- value
            value


let generateDiSq () = async {
    
    let csLog2 = 8u
    let cs = pown 2 (int csLog2)
    let noise (x, y) = noise (x, y)
    let cxc = 10
    let cyc = 10
    
    let map = DiSqMap.create csLog2
    
    for cxg in 1..cxc do
        for cyg in 1..cyc do
            Generator.generateChunk csLog2 map (cxg, cyg) noise
    
    let minh, maxh =
        DiSqMap.points map
        |> Seq.fold
            (fun (minh, maxh) struct(_, value) -> min minh value, max maxh value)
            (Double.PositiveInfinity, Double.NegativeInfinity)
    
//    let computeColor h =
//        let h = normalize minh maxh h
//        Palette.pick Palettes.lefebrve2 h
    
    let heightmap = Array2D.create (cs * (cxc + 2) + 1) (cs * (cyc + 2) + 1) Unchecked.defaultof<_>
    
    for (pxg, pyg), h in map |> DiSqMap.points do
        let color = normalize minh maxh h
        heightmap.[pxg, pyg] <- color
    
    return heightmap
}
