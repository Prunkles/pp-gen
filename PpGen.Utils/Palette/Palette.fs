namespace PpGen.Utils


type Palette = Palette of (float -> Rgb)

module Palette =
    
    let simple (colors: (float * (int * int * int)) seq) =
        let colors =
            colors
            |> Seq.map (fun (h, (r, g, b)) ->
                let c = Rgb.create r g b
                h, c
            )
            |> Seq.rev
            |> Seq.toArray
        let paletteF h =
            let _, c = colors |> Array.find (fun (h', _) -> h' <= h)
            c
        Palette paletteF
    
    let gradient (colors: (float * (int * int * int)) seq) =
        let colors =
            colors
            |> Seq.rev
            |> Seq.toArray
        let paletteF h =
            let i = colors |> Array.findIndex (fun (h', _) -> h' <= h)
            let h0, (r0, g0, b0) = colors.[i]
            let h1, (r1, g1, b1) = colors.[i - 1 |> max 0]
            let hd = if h0 = h1 then h0 else normalize h0 h1 h
            let c =
                Rgb.create
                    (lerp (float r0) (float r1) hd)
                    (lerp (float g0) (float g1) hd)
                    (lerp (float b0) (float b1) hd)
            c
        Palette paletteF
    
    let pick (Palette palette) (h: float) : Rgb = palette h

