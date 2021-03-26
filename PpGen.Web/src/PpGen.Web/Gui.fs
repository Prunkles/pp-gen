module PpGen.Web.Gui

open System
open Feliz
open Feliz.Bulma
open PpGen
open PpGen.Utils


type DiamondSquareProps =
    { Size: int }
    static member Default = { Size = 8 }

type PerlinNoiseProps =
    { Size: int }
    static member Default = { Size = 256 }

[<RequireQualifiedAccess>]
type AlgorithmProps =
    | DiamondSquare of DiamondSquareProps
    | PerlinNoise of PerlinNoiseProps

type GuiProps =
    { Seed: uint64
      Palette: Palette
      Use3D: bool
      Algorithm: AlgorithmProps }
    static member Default =
        { Seed = 9853101159151UL; Palette = Palettes.lefebrve2; Use3D = false
          Algorithm = AlgorithmProps.DiamondSquare DiamondSquareProps.Default }

[<ReactComponent>]
let Gui initial onChanged =
    let properties, setProperties = React.useState(fun () -> initial)
    let seed, setSeed = properties.Seed, fun seed -> setProperties { properties with Seed = seed }
    let setUse3D = fun x -> setProperties { properties with Use3D = x }
    let palette, setPalette = properties.Palette, fun palette -> setProperties { properties with Palette = palette }
    let algorithm, setAlgorithm = properties.Algorithm, fun alg -> setProperties { properties with Algorithm = alg }
    
    Html.div [
        Bulma.field.div [
            Bulma.label [ prop.text "Seed" ]
            Bulma.control.div [
                Bulma.input.number [
                    prop.value (string seed)
                    prop.onChange (fun (value: string) -> UInt64.Parse(value) |> setSeed)
                ]
            ]
        ]
        Bulma.field.div [
            Bulma.label [ prop.text "Palette" ]
            Bulma.control.div [
                Bulma.select [
                    prop.onChange (fun (value: string) ->
                        match value with
                        | "lefebrve2" -> setPalette Palettes.lefebrve2
                        | "grayscale" -> setPalette Palettes.grayscale
                        | _ -> invalidOp ""
                    )
                    prop.children [
                        Html.option [
                            prop.value "lefebrve2"
                            prop.text "lefebrve2"
                        ]
                        Html.option [
                            prop.value "grayscale"
                            prop.text "grayscale"
                        ]
                    ]
                ]
            ]
        ]
        
        Bulma.field.div [
            Checkradio.checkbox [
                prop.id "param-use3d"
                prop.name "param-use3d"
                color.isDanger
                prop.onChange (fun (value: bool) -> setUse3D value)
            ]
            Html.label [
                prop.htmlFor "param-use3d"
                prop.text "3D"
            ]
        ]
        
        Html.hr []
        
        Bulma.field.div [
            Bulma.label [ prop.text "Algorithm" ]
            Bulma.control.div [
                Bulma.select [
                    prop.onChange (fun (value: string) ->
                        match value with
                        | "diamond-square" -> setAlgorithm (AlgorithmProps.DiamondSquare DiamondSquareProps.Default)
                        | "perlin-noise" -> setAlgorithm (AlgorithmProps.PerlinNoise PerlinNoiseProps.Default)
                        | _ -> invalidOp ""
                    )
                    prop.children [
                        Html.option [
                            prop.value "diamond-square"
                            prop.text "Diamond Square"
                        ]
                        Html.option [
                            prop.value "perlin-noise"
                            prop.text "Perlin Noise"
                        ]
                    ]
                ]
            ]
        ]
        match algorithm with
        | AlgorithmProps.DiamondSquare algorithm ->
            Bulma.field.div [
                Bulma.label [ prop.text "Size (2^n+1)" ]
                Bulma.control.div [
                    Bulma.input.number [
                        prop.value (string algorithm.Size)
                        prop.onChange (fun (value: string) ->
                            setAlgorithm (AlgorithmProps.DiamondSquare { algorithm with Size = Int32.Parse(value) })
                        )
                    ]
                ]
            ]
        | AlgorithmProps.PerlinNoise algorithm ->
            Bulma.field.div [
                Bulma.label [ prop.text "Size" ]
                Bulma.control.div [
                    Bulma.input.number [
                        prop.value (string algorithm.Size)
                        prop.onChange (fun (value: string) ->
                            setAlgorithm (AlgorithmProps.PerlinNoise { algorithm with Size = Int32.Parse(value) })
                        )
                    ]
                ]
            ]
        
        Html.hr []
        
        Bulma.field.div [
            Bulma.button.button [
                color.isPrimary
                prop.text "Generate"
                prop.onClick (fun _ -> onChanged properties)
            ]
        ]
    ]
