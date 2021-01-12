module PpGen.Palettes

open System.Drawing
open PpGen.Utils

let lefebrve2 =
    Palette.gradient ([
        0, (0, 53, 83)
        90, (5, 70, 107)
        105, (17, 85, 124)
        125, (104, 176, 196)
        130, (179, 214, 224)

        131, (8, 68, 34)
        160, (50, 101, 50)
        190, (118, 141, 69)
        210, (165, 184, 105)
        235, (205, 207, 162)
        250, (235, 243, 248)
        255, (255, 255, 255)
    ] |> Seq.map (fun (h, c) -> float h / 255., c))

let greyscale =
    Palette (fun h ->
        let gray = h * 255. |> int
        Color.FromArgb(gray, gray, gray)
    )