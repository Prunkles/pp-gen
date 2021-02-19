namespace Fable.Import.PixiJs

open Fable.Core

[<AutoOpen>]
module PixiConstructors =
    module PIXI =
        [<ImportAll "pixi.js">]
        module Constructors =
            let Application = jsNative<PIXI.ApplicationStatic>
            let Sprite = jsNative<PIXI.SpriteStatic>
            let Texture = jsNative<PIXI.TextureStatic>
            let Container = jsNative<PIXI.ContainerStatic>
