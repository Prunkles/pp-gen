namespace PpGen.Utils

[<Struct>]
type Rgb = Rgb of struct(byte * byte * byte)

module Rgb =
    
    let inline create r g b = Rgb (byte r, byte g, byte b)
    
    let inline toHex (Rgb (r, g, b)) = sprintf "#%02X%02X%02X" r g b

//module Rgb =
//    let ofSystemColor (color: System.Drawing.Color) = color.R, color.G, color.B
//    let toSystemColor ((r, g, b): Rgb) = System.Drawing.Color.FromArgb(int r, int g, int b)
