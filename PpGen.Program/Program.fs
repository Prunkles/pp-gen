﻿module PpGen.Program

open System
open System
open System.Collections.Generic
open System.Diagnostics
open System.Drawing
open System.Drawing.Imaging

open System.Numerics
open PpGen.DiamondSquare
open PpGen.Utils

let ( |>! ) (x: 'a) (f: 'a -> unit) = f x; x


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



[<EntryPoint>]
let main argv =
    
    let csLog2 = 8u
    let cs = pown 2 (int csLog2)
    
    let map = DiSqMap.create csLog2
    
    let noise (x, y) = noise (x, y)
    
    let cxc = 10
    let cyc = 10
    
    let stopwatch = Stopwatch()
    
    printfn "Generating..."
    stopwatch.Restart()
    
    for cxg in 1..cxc do
        for cyg in 1..cyc do
            Generator.generateChunk csLog2 map (cxg, cyg) noise
    
    printfn $"Generated by {stopwatch.Elapsed}"
    
    printfn "Converting..."
    stopwatch.Restart()
    
    let bitmap = new Bitmap(cs * (cxc + 2) + 1, cs * (cyc + 2) + 1)
    let gfx = Graphics.FromImage(bitmap)
    
    for cxg in 0..cxc+1 do
        for cyg in 0..cyc+1 do
            let color =
                if (cyg + cxg) % 2 = 0
                then Color.FromArgb(0, 60, 40)
                else Color.FromArgb(0, 80, 60)
            use brush = new SolidBrush(color)
            gfx.FillRectangle(brush, cxg * cs, cyg * cs, cs, cs)

    let minh, maxh =
        DiSqMap.points map
        |> Seq.fold
            (fun (minh, maxh) struct(_, value) -> min minh value, max maxh value)
            (Double.PositiveInfinity, Double.NegativeInfinity)

    let computeColor h =
        let h = normalize minh maxh h
        Palette.pick Palettes.lefebrve2 h
    for (pxg, pyg), h in map |> DiSqMap.points do
        let color = computeColor h
        bitmap.SetPixel(pxg, pyg, color)
    
    printfn $"Converted in {stopwatch.Elapsed}"
    
    printfn $"Saving image (png)..."
    stopwatch.Restart()
    bitmap.Save("out.png", ImageFormat.Png)
    printfn $"Saved in ${stopwatch.Elapsed}"
    
    stopwatch.Stop()
    0
