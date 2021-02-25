// ts2fable 0.8.0
namespace rec Fable.Import.PixiJs

open System
open Fable.Core
open Fable.Core.JS
open Browser.Types

type Array<'T> = System.Collections.Generic.IList<'T>
type Error = System.Exception
type Function = System.Action
type RegExp = System.Text.RegularExpressions.Regex
type Symbol = obj


module PIXI =
    /// <summary>
    /// User"s customizable globals for overriding the default PIXI settings, such
    /// as a renderer"s default resolution, framerate, float percision, etc.
    /// </summary>
    /// <example>
    /// // Use the native window resolution as the default resolution
    /// // will support high-density displays when rendering
    /// PIXI.settings.RESOLUTION = window.devicePixelRatio.
    /// 
    /// // Disable interpolation when scaling, will make texture be pixelated
    /// PIXI.settings.SCALE_MODE = PIXI.SCALE_MODES.NEAREST;
    /// </example>
    let [<Import("settings","module/PIXI")>] settings: Settings.IExports = jsNative
    let [<Import("accessibility","module/PIXI")>] accessibility: Accessibility.IExports = jsNative
    let [<Import("CONST","module/PIXI")>] ``const``: CONST.IExports = jsNative
    /// <summary>
    /// Implements Dihedral Group D_8, see [group D4]<see href="http://mathworld.wolfram.com/DihedralGroupD4.html" />,
    /// D8 is the same but with diagonals. Used for texture rotations.
    /// 
    /// Vector xX(i), xY(i) is U-axis of sprite with rotation i
    /// Vector yY(i), yY(i) is V-axis of sprite with rotation i
    /// Rotations: 0 grad (0), 90 grad (2), 180 grad (4), 270 grad (6)
    /// Mirrors: vertical (8), main diagonal (10), horizontal (12), reverse diagonal (14)
    /// This is the small part of gameofbombs.com portal system. It works.
    /// </summary>
    let [<Import("GroupD8","module/PIXI")>] groupD8: GroupD8.IExports = jsNative
    let [<Import("CanvasTinter","module/PIXI")>] canvasTinter: CanvasTinter.IExports = jsNative
    let [<Import("ticker","module/PIXI")>] ticker: Ticker.IExports = jsNative
    let [<Import("extract","module/PIXI")>] extract: Extract.IExports = jsNative
    let [<Import("extras","module/PIXI")>] extras: Extras.IExports = jsNative
    let [<Import("filters","module/PIXI")>] filters: Filters.IExports = jsNative
    let [<Import("interaction","module/PIXI")>] interaction: Interaction.IExports = jsNative
    let [<Import("loaders","module/PIXI")>] loaders: Loaders.IExports = jsNative
    let [<Import("mesh","module/PIXI")>] mesh: Mesh.IExports = jsNative
    let [<Import("particles","module/PIXI")>] particles: Particles.IExports = jsNative
    let [<Import("prepare","module/PIXI")>] prepare: Prepare.IExports = jsNative
    let [<Import("glCore","module/PIXI")>] glCore: GlCore.IExports = jsNative
    let [<Import("utils","module/PIXI")>] utils: Utils.IExports = jsNative

    type [<AllowNullLiteral>] IExports =
        abstract VERSION: obj
        abstract PI_2: obj
        abstract RAD_TO_DEG: obj
        abstract DEG_TO_RAD: obj
        abstract RENDERER_TYPE: obj
        abstract BLEND_MODES: obj
        abstract DRAW_MODES: obj
        abstract SCALE_MODES: obj
        abstract WRAP_MODES: obj
        abstract TRANSFORM_MODE: obj
        abstract PRECISION: obj
        abstract GC_MODES: obj
        abstract SHAPES: obj
        abstract TEXT_GRADIENT: obj
        abstract UPDATE_PRIORITY: obj
        abstract autoDetectRenderer: width: float * height: float * ?options: PIXI.RendererOptions * ?forceCanvas: bool -> U2<PIXI.WebGLRenderer, PIXI.CanvasRenderer>
        /// <summary>
        /// This helper function will automatically detect which renderer you should be using.
        /// WebGL is the preferred renderer as it is a lot faster. If webGL is not supported by
        /// the browser then this function will return a canvas renderer
        /// </summary>
        /// <param name="options">The optional renderer parameters</param>
        /// <param name="options.width">the width of the renderers view</param>
        /// <param name="options.height">the height of the renderers view</param>
        /// <param name="options.view">the canvas to use as a view, optional</param>
        /// <param name="options.transparent">If the render view is transparent, default false</param>
        /// <param name="options.antialias">sets antialias (only applicable in chrome at the moment)</param>
        /// <param name="options.preserveDrawingBuffer">
        /// enables drawing buffer preservation, enable this if you
        /// need to call toDataUrl on the webgl context
        /// </param>
        /// <param name="options.backgroundColor">
        /// The background color of the rendered area
        /// (shown if not transparent).
        /// </param>
        /// <param name="options.clearBeforeRender">
        /// This sets if the renderer will clear the canvas or
        /// not before the new render pass.
        /// </param>
        /// <param name="options.resolution">The resolution / device pixel ratio of the renderer, retina would be 2</param>
        /// <param name="options.forceCanvas">prevents selection of WebGL renderer, even if such is present</param>
        /// <param name="options.roundPixels">
        /// If true PixiJS will Math.floor() x/y values when rendering,
        /// stopping pixel interpolation.
        /// </param>
        /// <param name="options.forceFXAA">
        /// forces FXAA antialiasing to be used over native.
        /// FXAA is faster, but may not always look as great **webgl only**
        /// </param>
        /// <param name="options.legacy">
        /// <c>true</c> to ensure compatibility with older / less advanced devices.
        /// If you experience unexplained flickering try setting this to true. **webgl only**
        /// </param>
        /// <param name="options.powerPreference">
        /// Parameter passed to webgl context, set to "high-performance"
        /// for devices with dual graphics card **webgl only**
        /// </param>
        /// <returns>Returns WebGL renderer if available, otherwise CanvasRenderer</returns>
        abstract autoDetectRenderer: ?options: PIXI.RendererOptions -> U2<PIXI.WebGLRenderer, PIXI.CanvasRenderer>
        abstract loader: PIXI.Loaders.Loader
        /// <summary>
        /// Convenience class to create a new PIXI application.
        /// This class automatically creates the renderer, ticker
        /// and root container.
        /// </summary>
        /// <example>
        /// // Create the application
        /// const app = new PIXI.Application();
        /// 
        /// // Add the view to the DOM
        /// document.body.appendChild(app.view);
        /// 
        /// // ex, add display objects
        /// app.stage.addChild(PIXI.Sprite.fromImage("something.png"));
        /// </example>
        abstract Application: ApplicationStatic
        /// "Builder" pattern for bounds rectangles
        /// Axis-Aligned Bounding Box
        /// It is not a shape! Its mutable thing, no "EMPTY" or that kind of problems
        abstract Bounds: BoundsStatic
        /// A Container represents a collection of display objects.
        /// 
        /// It is the base class of all display objects that act as a container for other objects.
        /// 
        /// <code language="js">
        ///   let container = new PIXI.Container();
        ///   container.addChild(sprite);
        /// </code>
        abstract Container: ContainerStatic
        /// The base class for all objects that are rendered on the screen.
        /// This is an abstract class and should not be used on its own rather it should be extended.
        abstract DisplayObject: DisplayObjectStatic
        /// Generic class to deal with traditional 2D matrix transforms
        abstract TransformBase: TransformBaseStatic
        /// Transform that takes care about its versions
//        abstract Transform: TransformStaticStatic
        /// Generic class to deal with traditional 2D matrix transforms
        /// local transformation is calculated from position,scale,skew and rotation
        abstract Transform: TransformStatic
        /// A GraphicsData object.
        abstract GraphicsData: GraphicsDataStatic
        /// The Graphics class contains methods used to draw primitive shapes such as lines, circles and
        /// rectangles to the display, and to color and fill them.
        abstract Graphics: GraphicsStatic
        abstract CanvasGraphicsRenderer: CanvasGraphicsRendererStatic
        /// Renders the graphics object.
        abstract GraphicsRenderer: GraphicsRendererStatic
        abstract WebGLGraphicsData: WebGLGraphicsDataStatic
        /// <summary>This shader is used to draw simple primitive shapes for <see cref="PIXI.Graphics" />.</summary>
        abstract PrimitiveShader: PrimitiveShaderStatic
        /// The PixiJS Matrix class as an object, which makes it a lot faster,
        /// here is a representation of it :
        /// | a | c | tx|
        /// | b | d | ty|
        /// | 0 | 0 | 1 |
        abstract Matrix: MatrixStatic
        abstract PointLike: PointLikeStatic
        /// The Point object represents a location in a two-dimensional coordinate system, where x represents
        /// the horizontal axis and y represents the vertical axis.
        /// An observable point is a point that triggers a callback when the point"s position is changed.
        abstract ObservablePoint: ObservablePointStatic
        /// The Point object represents a location in a two-dimensional coordinate system, where x represents
        /// the horizontal axis and y represents the vertical axis.
        abstract Point: PointStatic
        /// The Circle object can be used to specify a hit area for displayObjects
        abstract Circle: CircleStatic
        /// The Ellipse object can be used to specify a hit area for displayObjects
        abstract Ellipse: EllipseStatic
        abstract Polygon: PolygonStatic
        /// Rectangle object is an area defined by its position, as indicated by its top-left corner
        /// point (x, y) and by its width and its height.
        abstract Rectangle: RectangleStatic
        abstract RoundedRectangle: RoundedRectangleStatic
        /// <summary>
        /// The SystemRenderer is the base for a PixiJS Renderer. It is extended by the <see cref="PIXI.CanvasRenderer" />
        /// and <see cref="PIXI.WebGLRenderer" /> which can be used for rendering a PixiJS scene.
        /// </summary>
        abstract SystemRenderer: SystemRendererStatic
        /// The CanvasRenderer draws the scene and all its content onto a 2d canvas. This renderer should
        /// be used for browsers that do not support WebGL. Don"t forget to add the CanvasRenderer.view to
        /// your DOM or you will not see anything :)
        abstract CanvasRenderer: CanvasRendererStatic
        /// A set of functions used to handle masking.
        abstract CanvasMaskManager: CanvasMaskManagerStatic
        /// Creates a Canvas element of the given size.
        abstract CanvasRenderTarget: CanvasRenderTargetStatic
        /// The WebGLRenderer draws the scene and all its content onto a webGL enabled canvas. This renderer
        /// should be used for browsers that support webGL. This Render works by automatically managing webGLBatchs.
        /// So no need for Sprite Batches or Sprite Clouds.
        /// Don"t forget to add the view to your DOM or you will not see anything :)
        abstract WebGLRenderer: WebGLRendererStatic
        /// A WebGL state machines
        abstract WebGLState: WebGLStateStatic
        /// Helper class to create a webGL Texture
        abstract TextureManager: TextureManagerStatic
        /// TextureGarbageCollector. This class manages the GPU and ensures that it does not get clogged
        /// up with textures that are no longer being used.
        abstract TextureGarbageCollector: TextureGarbageCollectorStatic
        /// Base for a common object renderer that can be used as a system renderer plugin.
        abstract ObjectRenderer: ObjectRendererStatic
        /// Helper class to create a quad
        abstract Quad: QuadStatic
        abstract RenderTarget: RenderTargetStatic
        abstract BlendModeManager: BlendModeManagerStatic
        abstract FilterManager: FilterManagerStatic
        abstract StencilMaskStack: StencilMaskStackStatic
        abstract MaskManager: MaskManagerStatic
        abstract StencilManager: StencilManagerStatic
        abstract WebGLManager: WebGLManagerStatic
        abstract Filter: FilterStatic
        /// The SpriteMaskFilter class
        abstract SpriteMaskFilter: SpriteMaskFilterStatic
        /// <summary>
        /// The Sprite object is the base for all textured objects that are rendered to the screen
        /// 
        /// A sprite can be created directly from an image like this:
        /// 
        /// <code language="js">
        /// let sprite = new PIXI.Sprite.fromImage("assets/image.png");
        /// </code>
        /// 
        /// The more efficient way to create sprites is using a <see cref="PIXI.Spritesheet" />:
        /// 
        /// <code language="js">
        /// PIXI.loader.add("assets/spritesheet.json").load(setup);
        /// 
        /// function setup() {
        ///    let sheet = PIXI.loader.resources["assets/spritesheet.json"].spritesheet;
        ///    let sprite = new PIXI.Sprite(sheet.textures["image.png"]);
        ///    ...
        /// }
        /// </code>
        /// </summary>
        abstract Sprite: SpriteStatic
        abstract BatchBuffer: BatchBufferStatic
        abstract SpriteRenderer: SpriteRendererStatic
        abstract CanvasSpriteRenderer: CanvasSpriteRendererStatic
        abstract TextStyle: TextStyleStatic
        abstract TextMetrics: TextMetricsStatic
        abstract Text: TextStatic
        abstract BaseRenderTexture: BaseRenderTextureStatic
        abstract BaseTexture: BaseTextureStatic
        abstract RenderTexture: RenderTextureStatic
        abstract Texture: TextureStatic
        abstract TextureMatrix: TextureMatrixStatic
        abstract TextureUvs: TextureUvsStatic
        abstract Spritesheet: SpritesheetStatic
        abstract VideoBaseTexture: VideoBaseTextureStatic
        /// Wrapper class, webGL Shader for Pixi.
        /// Adds precision string if vertexSrc or fragmentSrc have no mention of it.
        abstract Shader: ShaderStatic
        abstract MiniSignalBinding: MiniSignalBindingStatic
        abstract MiniSignal: MiniSignalStatic

    /// <summary>
    /// User"s customizable globals for overriding the default PIXI settings, such
    /// as a renderer"s default resolution, framerate, float percision, etc.
    /// </summary>
    /// <example>
    /// // Use the native window resolution as the default resolution
    /// // will support high-density displays when rendering
    /// PIXI.settings.RESOLUTION = window.devicePixelRatio.
    /// 
    /// // Disable interpolation when scaling, will make texture be pixelated
    /// PIXI.settings.SCALE_MODE = PIXI.SCALE_MODES.NEAREST;
    /// </example>
    module Settings =

        type [<AllowNullLiteral>] IExports =
            abstract TARGET_FPMS: float
            abstract MIPMAP_TEXTURES: bool
            abstract RESOLUTION: float
            abstract FILTER_RESOLUTION: float
            abstract SPRITE_MAX_TEXTURES: float
            abstract SPRITE_BATCH_SIZE: float
            abstract RETINA_PREFIX: RegExp
            abstract RENDER_OPTIONS: IExportsRENDER_OPTIONS
            abstract TRANSFORM_MODE: float
            abstract GC_MODE: float
            abstract GC_MAX_IDLE: float
            abstract GC_MAX_CHECK_COUNT: float
            abstract WRAP_MODE: float
            abstract SCALE_MODE: float
            abstract PRECISION_VERTEX: string
            abstract PRECISION_FRAGMENT: string
            abstract PRECISION: string
            abstract UPLOADS_PER_FRAME: float
            abstract CAN_UPLOAD_SAME_BUFFER: bool
            abstract MESH_CANVAS_PADDING: float

        /// <seealso cref="PIXI.PRECISION" />
        [<Obsolete("since version 4.4.0")>]
        type PRECISION =
            float

        type [<AllowNullLiteral>] IExportsRENDER_OPTIONS =
            abstract view: HTMLCanvasElement option with get, set
            abstract antialias: bool with get, set
            abstract forceFXAA: bool with get, set
            abstract autoResize: bool with get, set
            abstract transparent: bool with get, set
            abstract backgroundColor: float with get, set
            abstract clearBeforeRender: bool with get, set
            abstract preserveDrawingBuffer: bool with get, set
            abstract roundPixels: bool with get, set
            abstract width: float with get, set
            abstract height: float with get, set
            abstract legacy: bool with get, set

    module Accessibility =

        type [<AllowNullLiteral>] IExports =
            abstract AccessibilityManager: AccessibilityManagerStatic

        type [<AllowNullLiteral>] AccessibilityManager =
            abstract activate: unit -> unit
            abstract deactivate: unit -> unit
            abstract div: HTMLElement with get, set
            abstract pool: ResizeArray<HTMLElement> with get, set
            abstract renderId: float with get, set
            abstract debug: bool with get, set
            abstract renderer: SystemRenderer with get, set
            abstract children: ResizeArray<AccessibleTarget> with get, set
            abstract isActive: bool with get, set
            abstract updateAccessibleObjects: displayObject: DisplayObject -> unit
            abstract update: unit -> unit
            abstract capHitArea: hitArea: HitArea -> unit
            abstract addChild: displayObject: DisplayObject -> unit
            abstract _onClick: e: Interaction.InteractionEvent -> unit
            abstract _onFocus: e: Interaction.InteractionEvent -> unit
            abstract _onFocusOut: e: Interaction.InteractionEvent -> unit
            abstract _onKeyDown: e: Interaction.InteractionEvent -> unit
            abstract _onMouseMove: e: MouseEvent -> unit
            abstract destroy: unit -> unit

        type [<AllowNullLiteral>] AccessibilityManagerStatic =
            [<EmitConstructor>] abstract Create: renderer: U2<CanvasRenderer, WebGLRenderer> -> AccessibilityManager

        type [<AllowNullLiteral>] AccessibleTarget =
            abstract accessible: bool with get, set
            abstract accessibleTitle: string option with get, set
            abstract accessibleHint: string option with get, set
            abstract tabIndex: float with get, set

    module CONST =

        type [<AllowNullLiteral>] IExports =
            abstract VERSION: string
            abstract PI_2: float
            abstract RAD_TO_DEG: float
            abstract DEG_TO_RAD: float
            abstract TARGET_FPMS: float
            abstract RENDERER_TYPE: IExportsRENDERER_TYPE
            abstract BLEND_MODES: IExportsBLEND_MODES
            abstract DRAW_MODES: IExportsDRAW_MODES
            abstract SCALE_MODES: IExportsSCALE_MODES
            abstract GC_MODES: IExportsGC_MODES
            abstract WRAP_MODES: IExportsWRAP_MODES
            abstract TRANSFORM_MODE: IExportsTRANSFORM_MODE
            abstract URL_FILE_EXTENSION: U2<RegExp, string>
            abstract DATA_URI: U2<RegExp, string>
            abstract SVG_SIZE: U2<RegExp, string>
            abstract SHAPES: IExportsSHAPES
            abstract PRECISION: IExportsPRECISION
            abstract TEXT_GRADIENT: IExportsTEXT_GRADIENT
            abstract UPDATE_PRIORITY: IExportsUPDATE_PRIORITY

        type [<AllowNullLiteral>] IExportsRENDERER_TYPE =
            /// Unknown render type.
            abstract UNKNOWN: float with get, set
            /// WebGL render type.
            abstract WEBGL: float with get, set
            /// Canvas render type.
            abstract CANVAS: float with get, set

        type [<AllowNullLiteral>] IExportsBLEND_MODES =
            abstract NORMAL: float with get, set
            abstract ADD: float with get, set
            abstract MULTIPLY: float with get, set
            abstract SCREEN: float with get, set
            abstract OVERLAY: float with get, set
            abstract DARKEN: float with get, set
            abstract LIGHTEN: float with get, set
            abstract COLOR_DODGE: float with get, set
            abstract COLOR_BURN: float with get, set
            abstract HARD_LIGHT: float with get, set
            abstract SOFT_LIGHT: float with get, set
            abstract DIFFERENCE: float with get, set
            abstract EXCLUSION: float with get, set
            abstract HUE: float with get, set
            abstract SATURATION: float with get, set
            abstract COLOR: float with get, set
            abstract LUMINOSITY: float with get, set
            abstract NORMAL_NPM: float with get, set
            abstract ADD_NPM: float with get, set
            abstract SCREEN_NPM: float with get, set

        type [<AllowNullLiteral>] IExportsDRAW_MODES =
            abstract POINTS: float with get, set
            abstract LINES: float with get, set
            abstract LINE_LOOP: float with get, set
            abstract LINE_STRIP: float with get, set
            abstract TRIANGLES: float with get, set
            abstract TRIANGLE_STRIP: float with get, set
            abstract TRIANGLE_FAN: float with get, set

        type [<AllowNullLiteral>] IExportsSCALE_MODES =
            abstract LINEAR: float with get, set
            abstract NEAREST: float with get, set

        type [<AllowNullLiteral>] IExportsGC_MODES =
            /// Garbage collection will happen periodically automatically
            abstract AUTO: float with get, set
            /// Garbage collection will need to be called manually
            abstract MANUAL: float with get, set

        type [<AllowNullLiteral>] IExportsWRAP_MODES =
            /// The textures uvs are clamped
            abstract CLAMP: float with get, set
            /// The texture uvs tile and repeat
            abstract MIRRORED_REPEAT: float with get, set
            /// The texture uvs tile and repeat with mirroring
            abstract REPEAT: float with get, set

        type [<AllowNullLiteral>] IExportsTRANSFORM_MODE =
            abstract DEFAULT: float with get, set
            abstract DYNAMIC: float with get, set
            abstract STATIC: float with get, set

        type [<AllowNullLiteral>] IExportsSHAPES =
            /// Polygon
            abstract POLY: float with get, set
            /// Rectangle
            abstract RECT: float with get, set
            /// Circle
            abstract CIRC: float with get, set
            /// Ellipse
            abstract ELIP: float with get, set
            /// Rounded Rectangle
            abstract RREC: float with get, set

        type [<AllowNullLiteral>] IExportsPRECISION =
            /// "lowp"
            abstract LOW: string with get, set
            /// "mediump"
            abstract MEDIUM: string with get, set
            /// "highp"
            abstract HIGH: string with get, set

        type [<AllowNullLiteral>] IExportsTEXT_GRADIENT =
            /// Vertical gradient
            abstract LINEAR_VERTICAL: float with get, set
            /// Linear gradient
            abstract LINEAR_HORIZONTAL: float with get, set

        type [<AllowNullLiteral>] IExportsUPDATE_PRIORITY =
            /// <summary>INTERACTION=50 Highest priority, used for <see cref="PIXI.interaction.InteractionManager" /></summary>
            abstract INTERACTION: float with get, set
            /// <summary>HIGH=25 High priority updating, <see cref="PIXI.VideoBaseTexture" /> and <see cref="PIXI.extras.AnimatedSprite" /></summary>
            abstract HIGH: float with get, set
            /// <summary>NORMAL=0 Default priority for ticker events, see <see cref="PIXI.ticker.Ticker.add" />.</summary>
            abstract NORMAL: float with get, set
            /// <summary>LOW=-25 Low priority used for <see cref="PIXI.Application" /> rendering.</summary>
            abstract LOW: float with get, set
            /// <summary>UTILITY=-50 Lowest priority used for <see cref="PIXI.prepare.BasePrepare" /> utility.</summary>
            abstract UTILITY: float with get, set

    type [<AllowNullLiteral>] StageOptions =
        abstract children: bool option with get, set
        abstract texture: bool option with get, set
        abstract baseTexture: bool option with get, set

    /// <summary>
    /// Convenience class to create a new PIXI application.
    /// This class automatically creates the renderer, ticker
    /// and root container.
    /// </summary>
    /// <example>
    /// // Create the application
    /// const app = new PIXI.Application();
    /// 
    /// // Add the view to the DOM
    /// document.body.appendChild(app.view);
    /// 
    /// // ex, add display objects
    /// app.stage.addChild(PIXI.Sprite.fromImage("something.png"));
    /// </example>
    type [<AllowNullLiteral>] Application =
        abstract renderer: U2<PIXI.WebGLRenderer, PIXI.CanvasRenderer> with get, set
        abstract stage: Container with get, set
        abstract ticker: Ticker.Ticker with get, set
        /// Loader instance to help with asset loading.
        abstract loader: Loaders.Loader with get, set
        abstract screen: Rectangle
        /// Convenience method for stopping the render.
        abstract stop: unit -> unit
        /// Convenience method for starting the render.
        abstract start: unit -> unit
        /// Render the current stage.
        abstract render: unit -> unit
        /// <summary>Destroy and don"t use after this.</summary>
        /// <param name="removeView">Automatically remove canvas from DOM.</param>
        /// <param name="stageOptions">
        /// Options parameter. A boolean will act as if all options
        /// have been set to that value
        /// </param>
        abstract destroy: ?removeView: bool * ?stageOptions: U2<StageOptions, bool> -> unit
        abstract view: HTMLCanvasElement

    /// <summary>
    /// Convenience class to create a new PIXI application.
    /// This class automatically creates the renderer, ticker
    /// and root container.
    /// </summary>
    /// <example>
    /// // Create the application
    /// const app = new PIXI.Application();
    /// 
    /// // Add the view to the DOM
    /// document.body.appendChild(app.view);
    /// 
    /// // ex, add display objects
    /// app.stage.addChild(PIXI.Sprite.fromImage("something.png"));
    /// </example>
    type [<AllowNullLiteral>] ApplicationStatic =
        [<EmitConstructor>] abstract Create: ?options: ApplicationOptions -> Application
        [<EmitConstructor>] abstract Create: ?width: float * ?height: float * ?options: ApplicationOptions * ?noWebGL: bool * ?sharedTicker: bool * ?sharedLoader: bool -> Application

    type [<AllowNullLiteral>] DestroyOptions =
        /// if set to true, all the children will have their destroy method called as well. "options" will be passed on to those calls.
        abstract children: bool option with get, set
        /// It Should it destroy the current texture of the sprite as well
        /// 
        /// Only used for child Sprites if options.children is set to true
        abstract texture: bool option with get, set
        /// Should it destroy the base texture of the sprite as well
        /// 
        /// Only used for child Sprites if options.children is set to true
        abstract baseTexture: bool option with get, set

    /// "Builder" pattern for bounds rectangles
    /// Axis-Aligned Bounding Box
    /// It is not a shape! Its mutable thing, no "EMPTY" or that kind of problems
    type [<AllowNullLiteral>] Bounds =
        abstract minX: float with get, set
        abstract minY: float with get, set
        abstract maxX: float with get, set
        abstract maxY: float with get, set
        abstract rect: Rectangle with get, set
        /// <summary>Checks if bounds are empty.</summary>
        /// <returns>True if empty.</returns>
        abstract isEmpty: unit -> bool
        /// Clears the bounds and resets.
        abstract clear: unit -> unit
        /// <summary>
        /// Can return Rectangle.EMPTY constant, either construct new rectangle, either use your rectangle
        /// It is not guaranteed that it will return tempRect
        /// </summary>
        /// <param name="rect">temporary object will be used if AABB is not empty</param>
        /// <returns>A rectangle of the bounds</returns>
        abstract getRectangle: ?rect: Rectangle -> Rectangle
        /// <summary>This function should be inlined when its possible.</summary>
        /// <param name="point">The point to add.</param>
        abstract addPoint: point: Point -> unit
        /// <summary>Adds a quad, not transformed</summary>
        /// <param name="vertices">The verts to add.</param>
//        abstract addQuad: vertices: ArrayLike<float> -> Bounds option
        /// <summary>Adds sprite frame, transformed.</summary>
        /// <param name="transform">TODO</param>
        /// <param name="x0">TODO</param>
        /// <param name="y0">TODO</param>
        /// <param name="x1">TODO</param>
        /// <param name="y1">TODO</param>
        abstract addFrame: transform: Transform * x0: float * y0: float * x1: float * y1: float -> unit
        /// <summary>Add an array of vertices</summary>
        /// <param name="transform">TODO</param>
        /// <param name="vertices">TODO</param>
        /// <param name="beginOffset">TODO</param>
        /// <param name="endOffset">TODO</param>
//        abstract addVertices: transform: Transform * vertices: ArrayLike<float> * beginOffset: float * endOffset: float -> unit
        /// <summary>Adds other Bounds</summary>
        /// <param name="bounds">TODO</param>
        abstract addBounds: bounds: Bounds -> unit
        /// <summary>Adds other Bounds, masked with Bounds</summary>
        /// <param name="bounds">TODO</param>
        /// <param name="mask">TODO</param>
        abstract addBoundsMask: bounds: Bounds * mask: Bounds -> unit
        /// <summary>Adds other Bounds, masked with Rectangle</summary>
        /// <param name="bounds">TODO</param>
        /// <param name="area">TODO</param>
        abstract addBoundsArea: bounds: Bounds * area: Rectangle -> unit

    /// "Builder" pattern for bounds rectangles
    /// Axis-Aligned Bounding Box
    /// It is not a shape! Its mutable thing, no "EMPTY" or that kind of problems
    type [<AllowNullLiteral>] BoundsStatic =
        [<EmitConstructor>] abstract Create: unit -> Bounds

    /// A Container represents a collection of display objects.
    /// 
    /// It is the base class of all display objects that act as a container for other objects.
    /// 
    /// <code language="js">
    ///   let container = new PIXI.Container();
    ///   container.addChild(sprite);
    /// </code>
    type [<AllowNullLiteral>] Container =
        inherit DisplayObject
        /// <summary>Returns the display object in the container</summary>
        /// <param name="name">instance name</param>
        /// <returns>The child with the specified name.</returns>
        abstract getChildByName: name: string -> 'T when 'T :> DisplayObject
        abstract children: ResizeArray<DisplayObject> with get, set
        abstract width: float with get, set
        abstract height: float with get, set
        abstract onChildrenChange: (ResizeArray<obj option> -> unit) with get, set
        abstract addChild: [<ParamArray>] children: 'T[] -> 'T when 'T :> DisplayObject
        /// <summary>Adds a child to the container at a specified index. If the index is out of bounds an error will be thrown</summary>
        /// <param name="child">The child to add</param>
        /// <param name="index">The index to place the child in</param>
        /// <returns>The child that was added.</returns>
        abstract addChildAt: child: 'T * index: float -> 'T when 'T :> DisplayObject
        /// <summary>Swaps the position of 2 Display Objects within this container.</summary>
        /// <param name="child">First display object to swap</param>
        /// <param name="child2">Second display object to swap</param>
        abstract swapChildren: child: DisplayObject * child2: DisplayObject -> unit
        /// <summary>Returns the index position of a child DisplayObject instance</summary>
        /// <param name="child">The DisplayObject instance to identify</param>
        /// <returns>The index position of the child display object to identify</returns>
        abstract getChildIndex: child: DisplayObject -> float
        /// <summary>Changes the position of an existing child in the display object container</summary>
        /// <param name="child">The child DisplayObject instance for which you want to change the index number</param>
        /// <param name="index">The resulting index number for the child display object</param>
        abstract setChildIndex: child: DisplayObject * index: float -> unit
        /// <summary>Returns the child at the specified index</summary>
        /// <param name="index">The index to get the child at</param>
        /// <returns>The child at the given index, if any.</returns>
        abstract getChildAt: index: float -> 'T when 'T :> DisplayObject
        abstract removeChild: child: DisplayObject -> 'T when 'T :> DisplayObject
        /// <summary>Removes a child from the specified index position.</summary>
        /// <param name="index">The index to get the child from</param>
        /// <returns>The child that was removed.</returns>
        abstract removeChildAt: index: float -> 'T when 'T :> DisplayObject
        /// <summary>Removes all children from this container that are within the begin and end indexes.</summary>
        /// <param name="beginIndex">The beginning position.</param>
        /// <param name="endIndex">The ending position. Default value is size of the container.</param>
        /// <returns>List of removed children</returns>
        abstract removeChildren: ?beginIndex: float * ?endIndex: float -> ResizeArray<'T> when 'T :> DisplayObject
        /// Updates the transform on all children of this container for rendering
        abstract updateTransform: unit -> unit
        /// Recalculates the bounds of the container.
        abstract calculateBounds: unit -> unit
        /// Recalculates the bounds of the object. Override this to
        /// calculate the bounds of the specific object (not including children).
        abstract _calculateBounds: unit -> unit
        abstract containerUpdateTransform: unit -> unit
        /// Renders the object using the WebGL renderer
        abstract renderWebGL: renderer: WebGLRenderer -> unit
        abstract renderAdvancedWebGL: renderer: WebGLRenderer -> unit
        abstract _renderWebGL: renderer: WebGLRenderer -> unit
        abstract _renderCanvas: renderer: CanvasRenderer -> unit
        abstract renderCanvas: renderer: CanvasRenderer -> unit
        /// <summary>
        /// Removes all internal references and listeners as well as removes children from the display list.
        /// Do not use a Container after calling <c>destroy</c>.
        /// </summary>
        /// <param name="options">Options parameter. A boolean will act as if all options have been set to that value</param>
        abstract destroy: ?options: U2<DestroyOptions, bool> -> unit
        /// Add a one-time listener for a given event.
        /// Add a one-time listener for a given event.
        abstract once: ``event``: U2<Interaction.InteractionEventTypes, string> * fn: (Interaction.InteractionEvent -> unit) * ?context: obj -> Container
        /// Add a one-time listener for a given event.
        /// Add a one-time listener for a given event.
        abstract once: ``event``: U2<string, Symbol> * fn: (ResizeArray<obj option> -> obj option) * ?context: obj -> Container
        /// Add a listener for a given event.
        /// Add a listener for a given event.
        abstract on: ``event``: U2<Interaction.InteractionEventTypes, string> * fn: (Interaction.InteractionEvent -> unit) * ?context: obj -> Container
        /// Add a listener for a given event.
        /// Add a listener for a given event.
        abstract on: ``event``: U2<string, Symbol> * fn: (ResizeArray<obj option> -> obj option) * ?context: obj -> Container
        /// <summary>Alias method for <c>removeListener</c></summary>
        abstract off: ``event``: U3<string, Symbol, string> * ?fn: (ResizeArray<obj option> -> obj option) * ?context: obj -> Container

    /// A Container represents a collection of display objects.
    /// 
    /// It is the base class of all display objects that act as a container for other objects.
    /// 
    /// <code language="js">
    ///   let container = new PIXI.Container();
    ///   container.addChild(sprite);
    /// </code>
    type [<AllowNullLiteral>] ContainerStatic =
        [<EmitConstructor>] abstract Create: unit -> Container

    /// The base class for all objects that are rendered on the screen.
    /// This is an abstract class and should not be used on its own rather it should be extended.
    type [<AllowNullLiteral>] DisplayObject =
        inherit Utils.EventEmitter
        inherit Interaction.InteractiveTarget
        inherit Accessibility.AccessibleTarget
        abstract _cacheAsBitmap: bool with get, set
        abstract _cacheData: bool with get, set
        /// Set this to true if you want this display object to be cached as a bitmap.
        /// This basically takes a snap shot of the display object as it is at that moment. It can
        /// provide a performance benefit for complex static displayObjects.
        /// To remove simply set this property to "false"
        /// 
        /// IMPORTANT GOTCHA - make sure that all your textures are preloaded BEFORE setting this property to true
        /// as it will take a snapshot of what is currently there. If the textures have not loaded then they will not appear.
        abstract cacheAsBitmap: bool with get, set
        abstract _renderCachedWebGL: renderer: WebGLRenderer -> unit
        abstract _initCachedDisplayObject: renderer: WebGLRenderer -> unit
        abstract _renderCachedCanvas: renderer: CanvasRenderer -> unit
        abstract _initCachedDisplayObjectCanvas: renderer: CanvasRenderer -> unit
        abstract _calculateCachedBounds: unit -> Rectangle
        abstract _getCachedLocalBounds: unit -> Rectangle
        abstract _destroyCachedDisplayObject: unit -> unit
        abstract _cacheAsBitmapDestroy: options: U2<bool, obj option> -> unit
        /// The instance name of the object.
        abstract name: string option with get, set
        /// <summary>Returns the global position of the displayObject. Does not depend on object scale, rotation and pivot.</summary>
        /// <param name="point">the point to write the global value to. If null a new point will be returned</param>
        /// <param name="skipUpdate">
        /// setting to true will stop the transforms of the scene graph from
        /// being updated. This means the calculation returned MAY be out of date BUT will give you a
        /// nice performance boost
        /// </param>
        /// <returns>The updated point</returns>
        abstract getGlobalPosition: ?point: Point * ?skipUpdate: bool -> Point
        abstract accessible: bool with get, set
        abstract accessibleTitle: string option with get, set
        abstract accessibleHint: string option with get, set
        abstract tabIndex: float with get, set
        /// <summary>
        /// Enable interaction events for the DisplayObject. Touch, pointer and mouse
        /// events will not be emitted unless <c>interactive</c> is set to <c>true</c>.
        /// </summary>
        abstract interactive: bool with get, set
        /// <summary>
        /// Determines if the children to the displayObject can be clicked/touched
        /// Setting this to false allows PixiJS to bypass a recursive <c>hitTest</c> function
        /// </summary>
        abstract interactiveChildren: bool with get, set
        /// Interaction shape. Children will be hit first, then this shape will be checked.
        /// Setting this will cause this shape to be checked in hit tests rather than the displayObject"s bounds.
        abstract hitArea: U6<PIXI.Rectangle, PIXI.Circle, PIXI.Ellipse, PIXI.Polygon, PIXI.RoundedRectangle, PIXI.HitArea> with get, set
        /// <summary>
        /// If enabled, the mouse cursor use the pointer behavior when hovered over the displayObject if it is interactive
        /// Setting this changes the "cursor" property to <c>"pointer"</c>.
        /// </summary>
        abstract buttonMode: bool with get, set
        /// This defines what cursor mode is used when the mouse cursor
        /// is hovered over the displayObject.
        abstract cursor: string with get, set
        abstract trackedPointers: DisplayObjectTrackedPointers with get, set
        [<Obsolete("")>]
        abstract defaultCursor: string with get, set
        abstract transform: TransformBase with get, set
        abstract alpha: float with get, set
        abstract visible: bool with get, set
        abstract renderable: bool with get, set
        abstract parent: Container with get, set
        abstract worldAlpha: float with get, set
        abstract filterArea: Rectangle option with get, set
//        abstract _filters: Array<Filter<obj option>> option with get, set
//        abstract _enabledFilters: Array<Filter<obj option>> option with get, set
        abstract _bounds: Bounds with get, set
        abstract _boundsID: float with get, set
        abstract _lastBoundsID: float with get, set
        abstract _boundsRect: Rectangle with get, set
        abstract _localBoundsRect: Rectangle with get, set
        abstract _mask: U2<PIXI.Graphics, PIXI.Sprite> option with get, set
        abstract _destroyed: bool
        abstract x: float with get, set
        abstract y: float with get, set
        abstract worldTransform: Matrix with get, set
        abstract localTransform: Matrix with get, set
        abstract position: U2<Point, ObservablePoint> with get, set
        abstract scale: U2<Point, ObservablePoint> with get, set
        abstract pivot: U2<Point, ObservablePoint> with get, set
        abstract skew: ObservablePoint with get, set
        abstract rotation: float with get, set
        abstract worldVisible: bool with get, set
        abstract mask: U2<PIXI.Graphics, PIXI.Sprite> option with get, set
//        abstract filters: Array<Filter<obj option>> option with get, set
        /// Updates the object transform for rendering
        /// 
        /// TODO - Optimization pass!
        abstract updateTransform: unit -> unit
        abstract displayObjectUpdateTransform: unit -> unit
        /// recursively updates transform of all objects from the root to this one
        /// internal function for toLocal()
        abstract _recursivePostUpdateTransform: unit -> unit
        /// <summary>Retrieves the bounds of the displayObject as a rectangle object.</summary>
        /// <param name="skipUpdate">
        /// setting to true will stop the transforms of the scene graph from
        /// being updated. This means the calculation returned MAY be out of date BUT will give you a
        /// nice performance boost
        /// </param>
        /// <param name="rect">Optional rectangle to store the result of the bounds calculation</param>
        /// <returns>the rectangular bounding area</returns>
        abstract getBounds: ?skipUpdate: bool * ?rect: Rectangle -> Rectangle
        /// <summary>Retrieves the local bounds of the displayObject as a rectangle object</summary>
        /// <param name="rect">Optional rectangle to store the result of the bounds calculation</param>
        /// <returns>the rectangular bounding area</returns>
        abstract getLocalBounds: ?rect: Rectangle -> Rectangle
        /// <summary>Calculates the global position of the display object</summary>
        /// <param name="position">The world origin to calculate from</param>
        /// <param name="point">
        /// A Point object in which to store the value, optional
        /// (otherwise will create a new Point)
        /// </param>
        /// <param name="skipUpdate">Should we skip the update transform.</param>
        /// <returns>A point object representing the position of this object</returns>
        abstract toGlobal: position: PointLike -> Point
        /// <summary>Calculates the global position of the display object</summary>
        /// <param name="position">The world origin to calculate from</param>
        /// <param name="point">
        /// A Point object in which to store the value, optional
        /// (otherwise will create a new Point)
        /// </param>
        /// <param name="skipUpdate">Should we skip the update transform.</param>
        /// <returns>A point object representing the position of this object</returns>
        abstract toGlobal: position: PointLike * ?point: 'T * ?skipUpdate: bool -> 'T when 'T :> PointLike
        abstract toLocal: position: PointLike * ?from: DisplayObject -> Point
        /// <summary>Calculates the local position of the display object relative to another point</summary>
        /// <param name="position">The world origin to calculate from</param>
        /// <param name="from">The DisplayObject to calculate the global position from</param>
        /// <param name="point">
        /// A Point object in which to store the value, optional
        /// (otherwise will create a new Point)
        /// </param>
        /// <param name="skipUpdate">Should we skip the update transform</param>
        /// <returns>A point object representing the position of this object</returns>
        abstract toLocal: position: PointLike * ?from: DisplayObject * ?point: 'T * ?skipUpdate: bool -> 'T when 'T :> PointLike
        /// <summary>Renders the object using the WebGL renderer</summary>
        /// <param name="renderer">The renderer</param>
        abstract renderWebGL: renderer: WebGLRenderer -> unit
        abstract renderCanvas: renderer: CanvasRenderer -> unit
        abstract setParent: container: Container -> Container
        /// <summary>Convenience function to set the position, scale, skew and pivot at once.</summary>
        /// <param name="x">The X position</param>
        /// <param name="y">The Y position</param>
        /// <param name="scaleX">The X scale value</param>
        /// <param name="scaleY">The Y scale value</param>
        /// <param name="rotation">The rotation</param>
        /// <param name="skewX">The X skew value</param>
        /// <param name="skewY">The Y skew value</param>
        /// <param name="pivotX">The X pivot value</param>
        /// <param name="pivotY">The Y pivot value</param>
        /// <returns>The DisplayObject instance</returns>
        abstract setTransform: ?x: float * ?y: float * ?scaleX: float * ?scaleY: float * ?rotation: float * ?skewX: float * ?skewY: float * ?pivotX: float * ?pivotY: float -> DisplayObject
        /// <summary>
        /// Base destroy method for generic display objects. This will automatically
        /// remove the display object from its parent Container as well as remove
        /// all current event listeners and internal references. Do not use a DisplayObject
        /// after calling <c>destroy</c>.
        /// </summary>
        abstract destroy: unit -> unit
        /// Add a listener for a given event.
        abstract on: ``event``: Interaction.InteractionEventTypes * fn: (Interaction.InteractionEvent -> unit) * ?context: obj -> DisplayObject
        /// Add a listener for a given event.
        abstract on: ``event``: U2<string, Symbol> * fn: (ResizeArray<obj option> -> obj option) * ?context: obj -> DisplayObject
        /// Add a one-time listener for a given event.
        abstract once: ``event``: Interaction.InteractionEventTypes * fn: (Interaction.InteractionEvent -> unit) * ?context: obj -> DisplayObject
        /// Add a one-time listener for a given event.
        abstract once: ``event``: U2<string, Symbol> * fn: (ResizeArray<obj option> -> obj option) * ?context: obj -> DisplayObject
        /// Remove the listeners of a given event.
        abstract removeListener: ``event``: Interaction.InteractionEventTypes * ?fn: (Interaction.InteractionEvent -> unit) * ?context: obj -> DisplayObject
        /// Remove all listeners, or those of the specified event.
        abstract removeAllListeners: ?``event``: Interaction.InteractionEventTypes -> DisplayObject
        /// <summary>Alias method for <c>removeListener</c></summary>
        abstract off: ``event``: Interaction.InteractionEventTypes * ?fn: (Interaction.InteractionEvent -> unit) * ?context: obj -> DisplayObject
        /// <summary>Alias method for <c>on</c></summary>
        abstract addListener: ``event``: Interaction.InteractionEventTypes * fn: (Interaction.InteractionEvent -> unit) * ?context: obj -> DisplayObject

    /// The base class for all objects that are rendered on the screen.
    /// This is an abstract class and should not be used on its own rather it should be extended.
    type [<AllowNullLiteral>] DisplayObjectStatic =
        [<EmitConstructor>] abstract Create: unit -> DisplayObject

    /// Generic class to deal with traditional 2D matrix transforms
    type [<AllowNullLiteral>] TransformBase =
        abstract worldTransform: Matrix with get, set
        abstract localTransform: Matrix with get, set
        abstract _worldID: float with get, set
        abstract _parentID: float with get, set
        /// Updates only local matrix
        abstract updateLocalTransform: unit -> unit
        /// <summary>Updates the values of the object and applies the parent"s transform.</summary>
        /// <param name="parentTransform">The transform of the parent of this object</param>
        abstract updateTransform: parentTransform: TransformBase -> unit
        abstract updateWorldTransform: parentTransform: TransformBase -> unit

    /// Generic class to deal with traditional 2D matrix transforms
    type [<AllowNullLiteral>] TransformBaseStatic =
        [<EmitConstructor>] abstract Create: unit -> TransformBase
        abstract IDENTITY: TransformBase with get, set

    /// Transform that takes care about its versions
    type [<AllowNullLiteral>] TransformStatic =
        inherit TransformBase
        abstract position: ObservablePoint with get, set
        abstract scale: ObservablePoint with get, set
        abstract pivot: ObservablePoint with get, set
        abstract skew: ObservablePoint with get, set
        abstract _rotation: float with get, set
        abstract _sr: float option with get, set
        abstract _cr: float option with get, set
        abstract _cy: float option with get, set
        abstract _sy: float option with get, set
        abstract _sx: float option with get, set
        abstract _cx: float option with get, set
        abstract _localID: float with get, set
        abstract _currentLocalID: float with get, set
        abstract onChange: unit -> unit
        abstract updateSkew: unit -> unit
        /// <summary>Decomposes a matrix and sets the transforms properties based on it.</summary>
        /// <param name="matrix">The matrix to decompose</param>
        abstract setFromMatrix: matrix: Matrix -> unit
        abstract rotation: float with get, set

    /// Transform that takes care about its versions
    type [<AllowNullLiteral>] TransformStaticStatic =
        [<EmitConstructor>] abstract Create: unit -> TransformStatic

    /// Generic class to deal with traditional 2D matrix transforms
    /// local transformation is calculated from position,scale,skew and rotation
    type [<AllowNullLiteral>] Transform =
        inherit TransformBase
        abstract position: Point with get, set
        abstract scale: Point with get, set
        abstract skew: ObservablePoint with get, set
        abstract pivot: Point with get, set
        abstract _rotation: float with get, set
        abstract _sr: float option with get, set
        abstract _cr: float option with get, set
        abstract _cy: float option with get, set
        abstract _sy: float option with get, set
        abstract _sx: float option with get, set
        abstract _cx: float option with get, set
        abstract updateSkew: unit -> unit
        /// <summary>Decomposes a matrix and sets the transforms properties based on it.</summary>
        /// <param name="matrix">The matrix to decompose</param>
        abstract setFromMatrix: matrix: Matrix -> unit
        abstract rotation: float with get, set

    /// Generic class to deal with traditional 2D matrix transforms
    /// local transformation is calculated from position,scale,skew and rotation
//    type [<AllowNullLiteral>] TransformStatic =
//        [<EmitConstructor>] abstract Create: unit -> Transform

    /// A GraphicsData object.
    type [<AllowNullLiteral>] GraphicsData =
        abstract lineWidth: float with get, set
        abstract lineAlignment: float with get, set
        abstract nativeLines: bool with get, set
        abstract lineColor: float with get, set
        abstract lineAlpha: float with get, set
        abstract _lineTint: float with get, set
        abstract fillColor: float with get, set
        abstract fillAlpha: float with get, set
        abstract _fillTint: float with get, set
        abstract fill: bool with get, set
        abstract holes: U6<ResizeArray<Circle>, ResizeArray<Rectangle>, ResizeArray<Ellipse>, ResizeArray<Polygon>, ResizeArray<RoundedRectangle>, ResizeArray<obj option>> with get, set
        abstract shape: U6<Circle, Rectangle, Ellipse, Polygon, RoundedRectangle, obj option> with get, set
        abstract ``type``: float option with get, set
        /// <summary>Creates a new GraphicsData object with the same values as this one.</summary>
        /// <returns>Cloned GraphicsData object</returns>
        abstract clone: unit -> GraphicsData
        /// <summary>Adds a hole to the shape.</summary>
        /// <param name="shape">The shape of the hole.</param>
        abstract addHole: shape: U6<Circle, Rectangle, Ellipse, Polygon, RoundedRectangle, obj option> -> unit
        /// Destroys the Graphics data.
        abstract destroy: ?options: U2<DestroyOptions, bool> -> unit

    /// A GraphicsData object.
    type [<AllowNullLiteral>] GraphicsDataStatic =
        [<EmitConstructor>] abstract Create: lineWidth: float * lineColor: float * lineAlpha: float * fillColor: float * fillAlpha: float * fill: bool * nativeLines: bool * shape: U6<Circle, Rectangle, Ellipse, Polygon, RoundedRectangle, obj option> * ?lineAlignment: float -> GraphicsData

    /// The Graphics class contains methods used to draw primitive shapes such as lines, circles and
    /// rectangles to the display, and to color and fill them.
    type [<AllowNullLiteral>] Graphics =
        inherit Container
        /// <summary>
        /// When cacheAsBitmap is set to true the graphics object will be rendered as if it was a sprite.
        /// This is useful if your graphics element does not change often, as it will speed up the rendering
        /// of the object in exchange for taking up texture memory. It is also useful if you need the graphics
        /// object to be anti-aliased, because it will be rendered using canvas. This is not recommended if
        /// you are constantly redrawing the graphics element.
        /// </summary>
        /// <default>false</default>
        abstract cacheAsBitmap: bool with get, set
        abstract fillAlpha: float with get, set
        abstract lineWidth: float with get, set
        abstract nativeLines: bool with get, set
        abstract lineColor: float with get, set
        abstract lineAlignment: float with get, set
        abstract graphicsData: ResizeArray<GraphicsData> with get, set
        abstract tint: float with get, set
        abstract _prevTint: float with get, set
        abstract blendMode: float with get, set
        abstract currentPath: GraphicsData with get, set
        abstract _webGL: obj option with get, set
        abstract isMask: bool with get, set
        abstract boundsPadding: float with get, set
        abstract _localBounds: Bounds with get, set
        abstract dirty: float with get, set
        abstract canvasTintDirty: float with get, set
        abstract fastRectDirty: float with get, set
        abstract clearDirty: float with get, set
        abstract boundsDirty: float with get, set
        abstract cachedSpriteDirty: bool with get, set
        abstract _spriteRect: Rectangle with get, set
        abstract _fastRect: bool with get, set
        abstract clone: unit -> Graphics
        abstract _quadraticCurveLength: fromX: float * fromY: float * cpX: float * cpY: float * toX: float * toY: float -> float
        abstract _bezierCurveLength: fromX: float * fromY: float * cpX: float * cpY: float * cpX2: float * cpY2: float * toX: float * toY: float -> float
        abstract _segmentsCount: length: float -> float
        abstract lineStyle: ?lineWidth: float * ?color: float * ?alpha: float * ?alignment: float -> Graphics
        abstract moveTo: x: float * y: float -> Graphics
        abstract lineTo: x: float * y: float -> Graphics
        abstract quadraticCurveTo: cpX: float * cpY: float * toX: float * toY: float -> Graphics
        abstract bezierCurveTo: cpX: float * cpY: float * cpX2: float * cpY2: float * toX: float * toY: float -> Graphics
        abstract arcTo: x1: float * y1: float * x2: float * y2: float * radius: float -> Graphics
        abstract arc: cx: float * cy: float * radius: float * startAngle: float * endAngle: float * ?anticlockwise: bool -> Graphics
        abstract beginFill: color: float * ?alpha: float -> Graphics
        abstract endFill: unit -> Graphics
        abstract drawRect: x: float * y: float * width: float * height: float -> Graphics
        abstract drawRoundedRect: x: float * y: float * width: float * height: float * radius: float -> Graphics
        abstract drawCircle: x: float * y: float * radius: float -> Graphics
        abstract drawEllipse: x: float * y: float * width: float * height: float -> Graphics
        abstract drawPolygon: path: U3<ResizeArray<float>, ResizeArray<Point>, Polygon> -> Graphics
        abstract drawStar: x: float * y: float * points: float * radius: float * innerRadius: float * ?rotation: float -> Graphics
        abstract clear: unit -> Graphics
        abstract isFastRect: unit -> bool
        abstract _renderCanvas: renderer: CanvasRenderer -> unit
        /// Recalculates the bounds of the object. Override this to
        /// calculate the bounds of the specific object (not including children).
        abstract _calculateBounds: unit -> Rectangle
        abstract _renderSpriteRect: renderer: PIXI.SystemRenderer -> unit
        abstract containsPoint: point: Point -> bool
        abstract updateLocalBounds: unit -> unit
        abstract drawShape: shape: U6<Circle, Rectangle, Ellipse, Polygon, RoundedRectangle, obj option> -> GraphicsData
        abstract generateCanvasTexture: ?scaleMode: float * ?resolution: float -> Texture
        abstract closePath: unit -> Graphics
        abstract addHole: unit -> Graphics
        /// <summary>
        /// Removes all internal references and listeners as well as removes children from the display list.
        /// Do not use a Container after calling <c>destroy</c>.
        /// </summary>
        abstract destroy: ?options: U2<DestroyOptions, bool> -> unit

    /// The Graphics class contains methods used to draw primitive shapes such as lines, circles and
    /// rectangles to the display, and to color and fill them.
    type [<AllowNullLiteral>] GraphicsStatic =
        /// <summary>
        /// Graphics curves resolution settings. If <c>adaptive</c> flag is set to <c>true</c>,
        /// the resolution is calculated based on the curve"s length to ensure better visual quality.
        /// Adaptive draw works with <c>bezierCurveTo</c> and <c>quadraticCurveTo</c>.
        /// </summary>
        abstract CURVES: GraphicsStaticCURVES with get, set
        [<EmitConstructor>] abstract Create: ?nativeLines: bool -> Graphics
        abstract _SPRITE_TEXTURE: Texture with get, set

    type [<AllowNullLiteral>] CanvasGraphicsRenderer =
        abstract render: graphics: Graphics -> unit
        abstract updateGraphicsTint: graphics: Graphics -> unit
        abstract renderPolygon: points: ResizeArray<Point> * close: bool * context: CanvasRenderingContext2D -> unit
        abstract destroy: unit -> unit

    type [<AllowNullLiteral>] CanvasGraphicsRendererStatic =
        [<EmitConstructor>] abstract Create: renderer: SystemRenderer -> CanvasGraphicsRenderer

    /// Renders the graphics object.
    type [<AllowNullLiteral>] GraphicsRenderer =
        inherit ObjectRenderer
        abstract graphicsDataPool: ResizeArray<GraphicsData> with get, set
        abstract primitiveShader: PrimitiveShader with get, set
//        abstract gl: WebGLRenderingContext with get, set
        abstract CONTEXT_UID: float with get, set
        /// Destroys this renderer.
        abstract destroy: unit -> unit
        /// <summary>Renders a graphics object.</summary>
        /// <param name="graphics">The graphics object to render.</param>
        abstract render: graphics: Graphics -> unit
        abstract updateGraphics: graphics: PIXI.Graphics -> unit
//        abstract getWebGLData: webGL: WebGLRenderingContext * ``type``: float * nativeLines: float -> WebGLGraphicsData

    /// Renders the graphics object.
    type [<AllowNullLiteral>] GraphicsRendererStatic =
        [<EmitConstructor>] abstract Create: renderer: PIXI.CanvasRenderer -> GraphicsRenderer

    type [<AllowNullLiteral>] WebGLGraphicsData =
//        abstract gl: WebGLRenderingContext with get, set
        abstract color: ResizeArray<float> with get, set
        abstract points: ResizeArray<Point> with get, set
        abstract indices: ResizeArray<float> with get, set
//        abstract buffer: WebGLBuffer with get, set
//        abstract indexBuffer: WebGLBuffer with get, set
        abstract dirty: bool with get, set
        abstract glPoints: ResizeArray<float> with get, set
        abstract glIndices: ResizeArray<float> with get, set
        abstract shader: GlCore.GLShader with get, set
        abstract vao: GlCore.VertexArrayObject with get, set
        abstract nativeLines: bool with get, set
        abstract reset: unit -> unit
        abstract upload: unit -> unit
        abstract destroy: unit -> unit

    type [<AllowNullLiteral>] WebGLGraphicsDataStatic =
        interface end
//        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * shader: GlCore.GLShader * attribsState: GlCore.AttribState -> WebGLGraphicsData

    /// <summary>This shader is used to draw simple primitive shapes for <see cref="PIXI.Graphics" />.</summary>
    type [<AllowNullLiteral>] PrimitiveShader =
        inherit GlCore.GLShader

    /// <summary>This shader is used to draw simple primitive shapes for <see cref="PIXI.Graphics" />.</summary>
    type [<AllowNullLiteral>] PrimitiveShaderStatic =
        [<EmitConstructor>] abstract Create: unit -> PrimitiveShader

    /// <summary>
    /// Implements Dihedral Group D_8, see [group D4]<see href="http://mathworld.wolfram.com/DihedralGroupD4.html" />,
    /// D8 is the same but with diagonals. Used for texture rotations.
    /// 
    /// Vector xX(i), xY(i) is U-axis of sprite with rotation i
    /// Vector yY(i), yY(i) is V-axis of sprite with rotation i
    /// Rotations: 0 grad (0), 90 grad (2), 180 grad (4), 270 grad (6)
    /// Mirrors: vertical (8), main diagonal (10), horizontal (12), reverse diagonal (14)
    /// This is the small part of gameofbombs.com portal system. It works.
    /// </summary>
    module GroupD8 =

        type [<AllowNullLiteral>] IExports =
            abstract E: float
            abstract SE: float
            abstract S: float
            abstract SW: float
            abstract W: float
            abstract NW: float
            abstract N: float
            abstract NE: float
            abstract MIRROR_HORIZONTAL: float
            abstract MIRROR_VERTICAL: float
            abstract uX: ind: float -> float
            abstract uY: ind: float -> float
            abstract vX: ind: float -> float
            abstract vY: ind: float -> float
            abstract inv: rotation: float -> float
            abstract add: rotationSecond: float * rotationFirst: float -> float
            abstract sub: rotationSecond: float * rotationFirst: float -> float
            /// <summary>Adds 180 degrees to rotation. Commutative operation.</summary>
            /// <param name="rotation">The number to rotate.</param>
            /// <returns>rotated number</returns>
            abstract rotate180: rotation: float -> float
            /// <summary>
            /// Direction of main vector can be horizontal, vertical or diagonal.
            /// Some objects work with vertical directions different.
            /// </summary>
            /// <param name="rotation">The number to check.</param>
            /// <returns>Whether or not the direction is vertical</returns>
            abstract isVertical: rotation: float -> bool
            /// <param name="dx">TODO</param>
            /// <param name="dy">TODO</param>
            /// <returns>TODO</returns>
            abstract byDirection: dx: float * dy: float -> float
            /// <summary>Helps sprite to compensate texture packer rotation.</summary>
            /// <param name="matrix">sprite world matrix</param>
            /// <param name="rotation">The rotation factor to use.</param>
            /// <param name="tx">sprite anchoring</param>
            /// <param name="ty">sprite anchoring</param>
            abstract matrixAppendRotationInv: matrix: Matrix * rotation: float * tx: float * ty: float -> unit
            /// <seealso cref="PIXI.GroupD8.isVertical" />
            /// <param name="rotation">The number to check.</param>
            /// <returns>Whether or not the direction is vertical</returns>
            [<Obsolete("since version 4.6.0")>]
            abstract isSwapWidthHeight: rotation: float -> bool

    /// The PixiJS Matrix class as an object, which makes it a lot faster,
    /// here is a representation of it :
    /// | a | c | tx|
    /// | b | d | ty|
    /// | 0 | 0 | 1 |
    type [<AllowNullLiteral>] Matrix =
        abstract a: float with get, set
        abstract b: float with get, set
        abstract c: float with get, set
        abstract d: float with get, set
        abstract tx: float with get, set
        abstract ty: float with get, set
        /// <summary>
        /// Creates a Matrix object based on the given array. The Element to Matrix mapping order is as follows:
        /// 
        /// a = array[0]
        /// b = array[1]
        /// c = array[3]
        /// d = array[4]
        /// tx = array[2]
        /// ty = array[5]
        /// </summary>
        /// <param name="array">The array that the matrix will be populated from.</param>
        abstract fromArray: array: ResizeArray<float> -> unit
        /// <summary>sets the matrix properties</summary>
        /// <param name="a">Matrix component</param>
        /// <param name="b">Matrix component</param>
        /// <param name="c">Matrix component</param>
        /// <param name="d">Matrix component</param>
        /// <param name="tx">Matrix component</param>
        /// <param name="ty">Matrix component</param>
        /// <returns>This matrix. Good for chaining method calls.</returns>
        abstract set: a: float * b: float * c: float * d: float * tx: float * ty: float -> Matrix
        /// <summary>Creates an array from the current Matrix object.</summary>
        /// <param name="transpose">Whether we need to transpose the matrix or not</param>
        /// <param name="out">If provided the array will be assigned to out</param>
        /// <returns>the newly created array which contains the matrix</returns>
        abstract toArray: ?transpose: bool * ?out: ResizeArray<float> -> ResizeArray<float>
        /// <summary>
        /// Get a new position with the current transformation applied.
        /// Can be used to go from a child"s coordinate space to the world coordinate space. (e.g. rendering)
        /// </summary>
        /// <param name="pos">The origin</param>
        /// <param name="newPos">The point that the new position is assigned to (allowed to be same as input)</param>
        /// <returns>The new point, transformed through this matrix</returns>
        abstract apply: pos: Point * ?newPos: Point -> Point
        abstract applyInverse: pos: Point * ?newPos: Point -> Point
        abstract translate: x: float * y: float -> Matrix
        abstract scale: x: float * y: float -> Matrix
        abstract rotate: angle: float -> Matrix
        abstract append: matrix: Matrix -> Matrix
        abstract setTransform: x: float * y: float * pivotX: float * pivotY: float * scaleX: float * scaleY: float * rotation: float * skewX: float * skewY: float -> PIXI.Matrix
        abstract prepend: matrix: Matrix -> Matrix
        abstract invert: unit -> Matrix
        abstract identity: unit -> Matrix
        abstract decompose: transform: TransformBase -> TransformBase
        abstract clone: unit -> Matrix
        abstract copy: matrix: Matrix -> Matrix

    /// The PixiJS Matrix class as an object, which makes it a lot faster,
    /// here is a representation of it :
    /// | a | c | tx|
    /// | b | d | ty|
    /// | 0 | 0 | 1 |
    type [<AllowNullLiteral>] MatrixStatic =
        [<EmitConstructor>] abstract Create: ?a: float * ?b: float * ?c: float * ?d: float * ?tx: float * ?ty: float -> Matrix
        abstract IDENTITY: Matrix with get, set
        abstract TEMP_MATRIX: Matrix with get, set

    type [<AllowNullLiteral>] PointLike =
        abstract x: float with get, set
        abstract y: float with get, set
        abstract set: ?x: float * ?y: float -> unit
        abstract copy: point: PointLike -> unit

    type [<AllowNullLiteral>] PointLikeStatic =
        [<EmitConstructor>] abstract Create: unit -> PointLike

    /// The Point object represents a location in a two-dimensional coordinate system, where x represents
    /// the horizontal axis and y represents the vertical axis.
    /// An observable point is a point that triggers a callback when the point"s position is changed.
    type [<AllowNullLiteral>] ObservablePoint =
        inherit PointLike
        abstract clone: ?cb: Function * ?scope: obj -> ObservablePoint
        abstract equals: p: U3<Point, ObservablePoint, PointLike> -> bool
        abstract cb: (unit -> obj option) with get, set
        abstract scope: obj option with get, set

    /// The Point object represents a location in a two-dimensional coordinate system, where x represents
    /// the horizontal axis and y represents the vertical axis.
    /// An observable point is a point that triggers a callback when the point"s position is changed.
    type [<AllowNullLiteral>] ObservablePointStatic =
        [<EmitConstructor>] abstract Create: cb: (unit -> obj option) * ?scope: obj * ?x: float * ?y: float -> ObservablePoint

    /// The Point object represents a location in a two-dimensional coordinate system, where x represents
    /// the horizontal axis and y represents the vertical axis.
    type [<AllowNullLiteral>] Point =
        inherit PointLike
        abstract clone: unit -> Point
        abstract equals: p: PointLike -> bool

    /// The Point object represents a location in a two-dimensional coordinate system, where x represents
    /// the horizontal axis and y represents the vertical axis.
    type [<AllowNullLiteral>] PointStatic =
        [<EmitConstructor>] abstract Create: ?x: float * ?y: float -> Point

    type [<AllowNullLiteral>] HitArea =
        abstract contains: x: float * y: float -> bool

    /// The Circle object can be used to specify a hit area for displayObjects
    type [<AllowNullLiteral>] Circle =
        inherit HitArea
        abstract x: float with get, set
        abstract y: float with get, set
        abstract radius: float with get, set
        abstract ``type``: float with get, set
        abstract clone: unit -> Circle
        abstract contains: x: float * y: float -> bool
        abstract getBounds: unit -> Rectangle

    /// The Circle object can be used to specify a hit area for displayObjects
    type [<AllowNullLiteral>] CircleStatic =
        [<EmitConstructor>] abstract Create: ?x: float * ?y: float * ?radius: float -> Circle

    /// The Ellipse object can be used to specify a hit area for displayObjects
    type [<AllowNullLiteral>] Ellipse =
        inherit HitArea
        abstract x: float with get, set
        abstract y: float with get, set
        abstract width: float with get, set
        abstract height: float with get, set
        abstract ``type``: float with get, set
        abstract clone: unit -> Ellipse
        abstract contains: x: float * y: float -> bool
        abstract getBounds: unit -> Rectangle

    /// The Ellipse object can be used to specify a hit area for displayObjects
    type [<AllowNullLiteral>] EllipseStatic =
        [<EmitConstructor>] abstract Create: ?x: float * ?y: float * ?halfWidth: float * ?halfHeight: float -> Ellipse

    type [<AllowNullLiteral>] Polygon =
        inherit HitArea
        abstract closed: bool with get, set
        abstract points: ResizeArray<float> with get, set
        abstract ``type``: float with get, set
        abstract clone: unit -> Polygon
        abstract contains: x: float * y: float -> bool
        abstract close: unit -> unit

    type [<AllowNullLiteral>] PolygonStatic =
        [<EmitConstructor>] abstract Create: points: U2<ResizeArray<Point>, ResizeArray<float>> -> Polygon
        [<EmitConstructor>] abstract Create: [<ParamArray>] points: Point[] -> Polygon
        [<EmitConstructor>] abstract Create: [<ParamArray>] points: float[] -> Polygon

    /// Rectangle object is an area defined by its position, as indicated by its top-left corner
    /// point (x, y) and by its width and its height.
    type [<AllowNullLiteral>] Rectangle =
        inherit HitArea
        abstract x: float with get, set
        abstract y: float with get, set
        abstract width: float with get, set
        abstract height: float with get, set
        abstract ``type``: float with get, set
        abstract left: float with get, set
        abstract right: float with get, set
        abstract top: float with get, set
        abstract bottom: float with get, set
        /// <summary>Creates a clone of this Rectangle</summary>
        /// <returns>a copy of the rectangle</returns>
        abstract clone: unit -> Rectangle
        /// <summary>Enlarges rectangle that way its corners lie on grid</summary>
        /// <param name="resolution">resolution</param>
        /// <param name="eps">precision</param>
        abstract ceil: ?resolution: float * ?eps: float -> unit
        /// <summary>Copies another rectangle to this one.</summary>
        /// <param name="rectangle">The rectangle to copy.</param>
        /// <returns>Returns itself.</returns>
        abstract copy: rectangle: Rectangle -> Rectangle
        /// <summary>Checks whether the x and y coordinates given are contained within this Rectangle</summary>
        /// <param name="x">The X coordinate of the point to test</param>
        /// <param name="y">The Y coordinate of the point to test</param>
        /// <returns>Whether the x/y coordinates are within this Rectangle</returns>
        abstract contains: x: float * y: float -> bool
        /// <summary>Pads the rectangle making it grow in all directions.</summary>
        /// <param name="paddingX">The horizontal padding amount.</param>
        /// <param name="paddingY">The vertical padding amount.</param>
        abstract pad: paddingX: float * paddingY: float -> unit
        /// <summary>Fits this rectangle around the passed one.</summary>
        /// <param name="rectangle">The rectangle to fit.</param>
        abstract fit: rectangle: Rectangle -> unit
        /// <summary>Enlarges this rectangle to include the passed rectangle.</summary>
        /// <param name="rectangle">The rectangle to include.</param>
        abstract enlarge: rectangle: Rectangle -> unit

    /// Rectangle object is an area defined by its position, as indicated by its top-left corner
    /// point (x, y) and by its width and its height.
    type [<AllowNullLiteral>] RectangleStatic =
        [<EmitConstructor>] abstract Create: ?x: float * ?y: float * ?width: float * ?height: float -> Rectangle
        abstract EMPTY: Rectangle with get, set

    type [<AllowNullLiteral>] RoundedRectangle =
        inherit HitArea
        abstract x: float with get, set
        abstract y: float with get, set
        abstract width: float with get, set
        abstract height: float with get, set
        abstract radius: float with get, set
        abstract ``type``: float with get, set
        abstract clone: unit -> RoundedRectangle
        abstract contains: x: float * y: float -> bool

    type [<AllowNullLiteral>] RoundedRectangleStatic =
        [<EmitConstructor>] abstract Create: ?x: float * ?y: float * ?width: float * ?height: float * ?radius: float -> RoundedRectangle

    type [<AllowNullLiteral>] RendererOptions =
        /// the width of the renderers view [default=800]
        abstract width: float option with get, set
        /// the height of the renderers view [default=600]
        abstract height: float option with get, set
        /// the canvas to use as a view, optional
        abstract view: HTMLCanvasElement option with get, set
        /// If the render view is transparent, [default=false]
        abstract transparent: bool option with get, set
        /// sets antialias (only applicable in chrome at the moment) [default=false]
        abstract antialias: bool option with get, set
        /// enables drawing buffer preservation, enable this if you need to call toDataUrl on the webgl context [default=false]
        abstract preserveDrawingBuffer: bool option with get, set
        /// The resolution / device pixel ratio of the renderer, retina would be 2 [default=1]
        abstract resolution: float option with get, set
        /// prevents selection of WebGL renderer, even if such is present [default=false]
        abstract forceCanvas: bool option with get, set
        /// The background color of the rendered area (shown if not transparent) [default=0x000000]
        abstract backgroundColor: float option with get, set
        /// This sets if the renderer will clear the canvas or not before the new render pass. [default=true]
        abstract clearBeforeRender: bool option with get, set
        /// If true Pixi will Math.floor() x/ y values when rendering, stopping pixel interpolation. [default=false]
        abstract roundPixels: bool option with get, set
        /// forces FXAA antialiasing to be used over native FXAA is faster, but may not always look as great ** webgl only** [default=false]
        abstract forceFXAA: bool option with get, set
        /// <summary><c>true</c> to ensure compatibility with older / less advanced devices. If you experience unexplained flickering try setting this to true. **webgl only** [default=false]</summary>
        abstract legacy: bool option with get, set
        /// Deprecated
//        abstract context: WebGLRenderingContext option with get, set
        [<Obsolete("")>]
        abstract autoResize: bool option with get, set
        /// Parameter passed to webgl context, set to "high-performance" for devices with dual graphics card
        abstract powerPreference: string option with get, set

    type [<AllowNullLiteral>] ApplicationOptions =
        inherit RendererOptions
        /// <summary><c>true</c> to use PIXI.ticker.shared, <c>false</c> to create new ticker. [default=false]</summary>
        abstract sharedTicker: bool option with get, set
        /// <summary><c>true</c> to use PIXI.loaders.shared, <c>false</c> to create new Loader.</summary>
        abstract sharedLoader: bool option with get, set
        /// automatically starts the rendering after the construction.
        /// Note that setting this parameter to false does NOT stop the shared ticker even if you set
        /// options.sharedTicker to true in case that it is already started. Stop it by your own.
        abstract autoStart: bool option with get, set

    type [<AllowNullLiteral>] DefaultRendererPlugins =
        abstract accessibility: Accessibility.AccessibilityManager with get, set
        abstract interaction: Interaction.InteractionManager with get, set

    type [<AllowNullLiteral>] RendererPlugins =
        inherit DefaultRendererPlugins

    /// <summary>
    /// The SystemRenderer is the base for a PixiJS Renderer. It is extended by the <see cref="PIXI.CanvasRenderer" />
    /// and <see cref="PIXI.WebGLRenderer" /> which can be used for rendering a PixiJS scene.
    /// </summary>
    type [<AllowNullLiteral>] SystemRenderer =
        inherit Utils.EventEmitter
        abstract ``type``: float with get, set
        abstract options: RendererOptions with get, set
        abstract screen: Rectangle with get, set
        abstract width: float
        abstract height: float
        abstract view: HTMLCanvasElement with get, set
        abstract resolution: float with get, set
        abstract transparent: bool with get, set
        abstract autoResize: bool with get, set
        abstract blendModes: obj option with get, set
        abstract preserveDrawingBuffer: bool with get, set
        abstract clearBeforeRender: bool with get, set
        abstract roundPixels: bool with get, set
        abstract backgroundColor: float with get, set
        abstract _backgroundColor: float with get, set
        abstract _backgroundColorRgba: ResizeArray<float> with get, set
        abstract _backgroundColorString: string with get, set
        abstract _tempDisplayObjectParent: Container with get, set
        abstract _lastObjectRendered: DisplayObject with get, set
        abstract resize: screenWidth: float * screenHeight: float -> unit
        abstract generateTexture: displayObject: DisplayObject * ?scaleMode: float * ?resolution: float * ?region: Rectangle -> RenderTexture
        abstract render: [<ParamArray>] args: obj option[] -> unit
        abstract destroy: ?removeView: bool -> unit

    /// <summary>
    /// The SystemRenderer is the base for a PixiJS Renderer. It is extended by the <see cref="PIXI.CanvasRenderer" />
    /// and <see cref="PIXI.WebGLRenderer" /> which can be used for rendering a PixiJS scene.
    /// </summary>
    type [<AllowNullLiteral>] SystemRendererStatic =
        [<EmitConstructor>] abstract Create: system: string * ?options: RendererOptions -> SystemRenderer
        [<EmitConstructor>] abstract Create: system: string * ?screenWidth: float * ?screenHeight: float * ?options: RendererOptions -> SystemRenderer

    type [<AllowNullLiteral>] DefaultCanvasRendererPlugins =
        abstract extract: Extract.CanvasExtract with get, set
        abstract prepare: Prepare.CanvasPrepare with get, set

    type [<AllowNullLiteral>] CanvasRendererPlugins =
        inherit DefaultCanvasRendererPlugins
        inherit RendererPlugins

    /// The CanvasRenderer draws the scene and all its content onto a 2d canvas. This renderer should
    /// be used for browsers that do not support WebGL. Don"t forget to add the CanvasRenderer.view to
    /// your DOM or you will not see anything :)
    type [<AllowNullLiteral>] CanvasRenderer =
        inherit SystemRenderer
        /// <summary>
        /// Collection of installed plugins. These are included by default in PIXI, but can be excluded
        /// by creating a custom build. Consult the README for more information about creating custom
        /// builds and excluding plugins.
        /// </summary>
        abstract plugins: obj option with get, set
        abstract initPlugins: unit -> unit
        abstract destroyPlugins: unit -> unit
        abstract _activeBlendMode: float with get, set
        abstract rootContext: CanvasRenderingContext2D with get, set
        abstract rootResolution: float option with get, set
        abstract refresh: bool with get, set
        abstract maskManager: CanvasMaskManager with get, set
        abstract smoothProperty: string with get, set
        /// Collection of methods for extracting data (image, pixels, etc.) from a display object or render texture
        abstract extract: Extract.CanvasExtract with get, set
        abstract context: CanvasRenderingContext2D option with get, set
        abstract render: displayObject: PIXI.DisplayObject * ?renderTexture: PIXI.RenderTexture * ?clear: bool * ?transform: PIXI.Matrix * ?skipUpdateTransform: bool -> unit
        abstract setBlendMode: blendMode: float -> unit
        abstract destroy: ?removeView: bool -> unit
        abstract clear: ?clearColor: string -> unit
        abstract invalidateBlendMode: unit -> unit
        /// Add a listener for a given event.
        abstract on: ``event``: CanvasRendererOnEvent * fn: (unit -> unit) * ?context: obj -> CanvasRenderer
        /// Add a one-time listener for a given event.
        abstract once: ``event``: CanvasRendererOnceEvent * fn: (unit -> unit) * ?context: obj -> CanvasRenderer
        /// Remove the listeners of a given event.
        abstract removeListener: ``event``: CanvasRendererRemoveListenerEvent * ?fn: (unit -> unit) * ?context: obj -> CanvasRenderer
        /// Remove all listeners, or those of the specified event.
        abstract removeAllListeners: ?``event``: CanvasRendererRemoveAllListenersEvent -> CanvasRenderer
        /// <summary>Alias method for <c>removeListener</c></summary>
        abstract off: ``event``: CanvasRendererOffEvent * ?fn: (unit -> unit) * ?context: obj -> CanvasRenderer
        /// <summary>Alias method for <c>on</c></summary>
        abstract addListener: ``event``: CanvasRendererAddListenerEvent * fn: (unit -> unit) * ?context: obj -> CanvasRenderer

    type [<StringEnum>] [<RequireQualifiedAccess>] CanvasRendererOnEvent =
        | Prerender
        | Postrender

    type [<StringEnum>] [<RequireQualifiedAccess>] CanvasRendererOnceEvent =
        | Prerender
        | Postrender

    type [<StringEnum>] [<RequireQualifiedAccess>] CanvasRendererRemoveListenerEvent =
        | Prerender
        | Postrender

    type [<StringEnum>] [<RequireQualifiedAccess>] CanvasRendererRemoveAllListenersEvent =
        | Prerender
        | Postrender

    type [<StringEnum>] [<RequireQualifiedAccess>] CanvasRendererOffEvent =
        | Prerender
        | Postrender

    type [<StringEnum>] [<RequireQualifiedAccess>] CanvasRendererAddListenerEvent =
        | Prerender
        | Postrender

    /// The CanvasRenderer draws the scene and all its content onto a 2d canvas. This renderer should
    /// be used for browsers that do not support WebGL. Don"t forget to add the CanvasRenderer.view to
    /// your DOM or you will not see anything :)
    type [<AllowNullLiteral>] CanvasRendererStatic =
        abstract __plugins: CanvasRendererStatic__plugins with get, set
        /// <summary>Adds a plugin to the renderer.</summary>
        /// <param name="pluginName">The name of the plugin.</param>
        /// <param name="ctor">The constructor function or class for the plugin.</param>
        abstract registerPlugin: pluginName: string * ctor: CanvasRendererStaticRegisterPluginCtor -> unit
        [<EmitConstructor>] abstract Create: ?options: RendererOptions -> CanvasRenderer
        [<EmitConstructor>] abstract Create: ?screenWidth: float * ?screenHeight: float * ?options: RendererOptions -> CanvasRenderer

    type [<AllowNullLiteral>] CanvasRendererStaticRegisterPluginCtor =
        [<EmitConstructor>] abstract Create: renderer: CanvasRenderer -> CanvasRenderer

    /// A set of functions used to handle masking.
    type [<AllowNullLiteral>] CanvasMaskManager =
        abstract pushMask: maskData: obj option -> unit
        abstract renderGraphicsShape: graphics: Graphics -> unit
        abstract popMask: renderer: U2<WebGLRenderer, CanvasRenderer> -> unit
        abstract destroy: unit -> unit

    /// A set of functions used to handle masking.
    type [<AllowNullLiteral>] CanvasMaskManagerStatic =
        [<EmitConstructor>] abstract Create: renderer: CanvasRenderer -> CanvasMaskManager

    /// Creates a Canvas element of the given size.
    type [<AllowNullLiteral>] CanvasRenderTarget =
        abstract canvas: HTMLCanvasElement with get, set
        abstract context: CanvasRenderingContext2D with get, set
        abstract resolution: float with get, set
        abstract width: float with get, set
        abstract height: float with get, set
        abstract clear: unit -> unit
        abstract resize: width: float * height: float -> unit
        abstract destroy: unit -> unit

    /// Creates a Canvas element of the given size.
    type [<AllowNullLiteral>] CanvasRenderTargetStatic =
        [<EmitConstructor>] abstract Create: width: float * height: float * resolution: float -> CanvasRenderTarget

    type [<AllowNullLiteral>] WebGLRendererOptions =
        inherit RendererOptions

    type [<AllowNullLiteral>] DefaultWebGLRendererPlugins =
        abstract extract: Extract.WebGLExtract with get, set
        abstract prepare: Prepare.WebGLPrepare with get, set

    type [<AllowNullLiteral>] WebGLRendererPlugins =
        inherit DefaultWebGLRendererPlugins
        inherit RendererPlugins

    /// The WebGLRenderer draws the scene and all its content onto a webGL enabled canvas. This renderer
    /// should be used for browsers that support webGL. This Render works by automatically managing webGLBatchs.
    /// So no need for Sprite Batches or Sprite Clouds.
    /// Don"t forget to add the view to your DOM or you will not see anything :)
    type [<AllowNullLiteral>] WebGLRenderer =
        inherit SystemRenderer
        /// <summary>
        /// Collection of installed plugins. These are included by default in PIXI, but can be excluded
        /// by creating a custom build. Consult the README for more information about creating custom
        /// builds and excluding plugins.
        /// </summary>
        abstract plugins: obj option with get, set
        abstract initPlugins: unit -> unit
        abstract destroyPlugins: unit -> unit
        abstract _contextOptions: WebGLRenderer_contextOptions with get, set
        abstract _backgroundColorRgba: ResizeArray<float> with get, set
        abstract maskManager: MaskManager with get, set
        abstract stencilManager: StencilManager option with get, set
        abstract emptyRenderer: ObjectRenderer with get, set
        abstract currentRenderer: ObjectRenderer with get, set
//        abstract gl: WebGLRenderingContext with get, set
        abstract CONTEXT_UID: float with get, set
        abstract state: WebGLState option with get, set
        abstract renderingToScreen: bool with get, set
        abstract boundTextures: ResizeArray<BaseTexture> with get, set
        abstract emptyTextures: ResizeArray<BaseTexture> with get, set
        abstract _unknownBoundTextures: ResizeArray<BaseTexture> with get, set
        abstract filterManager: FilterManager with get, set
        abstract textureManager: TextureManager option with get, set
        abstract textureGC: TextureGarbageCollector option with get, set
        /// Collection of methods for extracting data (image, pixels, etc.) from a display object or render texture
        abstract extract: Extract.WebGLExtract with get, set
        abstract drawModes: obj option with get, set
        abstract _activeShader: Shader with get, set
        abstract _activeRenderTarget: RenderTarget with get, set
        abstract _initContext: unit -> unit
        abstract render: displayObject: PIXI.DisplayObject * ?renderTexture: PIXI.RenderTexture * ?clear: bool * ?transform: PIXI.Matrix * ?skipUpdateTransform: bool -> unit
        abstract setObjectRenderer: objectRenderer: ObjectRenderer -> unit
        abstract flush: unit -> unit
        abstract setBlendMode: blendMode: float -> unit
        abstract clear: ?clearColor: float -> unit
        abstract setTransform: matrix: Matrix -> unit
        abstract clearRenderTexture: renderTexture: RenderTexture * ?clearColor: float -> WebGLRenderer
        abstract bindRenderTexture: renderTexture: RenderTexture * transform: Matrix -> WebGLRenderer
        abstract bindRenderTarget: renderTarget: RenderTarget -> WebGLRenderer
        abstract bindShader: shader: Shader * ?autoProject: bool -> WebGLRenderer
        abstract bindTexture: texture: U2<Texture, BaseTexture> * ?location: float * ?forceLocation: bool -> float
        abstract unbindTexture: texture: U2<Texture, BaseTexture> -> WebGLRenderer option
        abstract createVao: unit -> GlCore.VertexArrayObject
        abstract bindVao: vao: GlCore.VertexArrayObject -> WebGLRenderer
        abstract reset: unit -> WebGLRenderer
//        abstract handleContextLost: (WebGLContextEvent -> unit) with get, set
        abstract handleContextRestored: (unit -> unit) with get, set
        abstract destroy: ?removeView: bool -> unit
        /// Add a listener for a given event.
        abstract on: ``event``: WebGLRendererOnEvent * fn: (unit -> unit) * ?context: obj -> WebGLRenderer
        /// Add a listener for a given event.
//        [<Emit "$0.on('context',$1,$2)">] abstract on_context: fn: (WebGLRenderingContext -> unit) * ?context: obj -> WebGLRenderer
        /// Add a one-time listener for a given event.
        abstract once: ``event``: WebGLRendererOnceEvent * fn: (unit -> unit) * ?context: obj -> WebGLRenderer
        /// Add a one-time listener for a given event.
//        [<Emit "$0.once('context',$1,$2)">] abstract once_context: fn: (WebGLRenderingContext -> unit) * ?context: obj -> WebGLRenderer
        /// Remove the listeners of a given event.
        abstract removeListener: ``event``: WebGLRendererRemoveListenerEvent * ?fn: (unit -> unit) * ?context: obj -> WebGLRenderer
        /// Remove the listeners of a given event.
//        [<Emit "$0.removeListener('context',$1,$2)">] abstract removeListener_context: ?fn: (WebGLRenderingContext -> unit) * ?context: obj -> WebGLRenderer
        /// Remove all listeners, or those of the specified event.
        abstract removeAllListeners: ?``event``: WebGLRendererRemoveAllListenersEvent -> WebGLRenderer
        /// <summary>Alias method for <c>removeListener</c></summary>
        abstract off: ``event``: WebGLRendererOffEvent * ?fn: (unit -> unit) * ?context: obj -> WebGLRenderer
        /// <summary>Alias method for <c>removeListener</c></summary>
//        [<Emit "$0.off('context',$1,$2)">] abstract off_context: ?fn: (WebGLRenderingContext -> unit) * ?context: obj -> WebGLRenderer
        /// <summary>Alias method for <c>on</c></summary>
        abstract addListener: ``event``: WebGLRendererAddListenerEvent * fn: (unit -> unit) * ?context: obj -> WebGLRenderer
        /// <summary>Alias method for <c>on</c></summary>
//        [<Emit "$0.addListener('context',$1,$2)">] abstract addListener_context: fn: (WebGLRenderingContext -> unit) * ?context: obj -> WebGLRenderer

    type [<StringEnum>] [<RequireQualifiedAccess>] WebGLRendererOnEvent =
        | Prerender
        | Postrender

    type [<StringEnum>] [<RequireQualifiedAccess>] WebGLRendererOnceEvent =
        | Prerender
        | Postrender

    type [<StringEnum>] [<RequireQualifiedAccess>] WebGLRendererRemoveListenerEvent =
        | Prerender
        | Postrender

    type [<StringEnum>] [<RequireQualifiedAccess>] WebGLRendererRemoveAllListenersEvent =
        | Prerender
        | Postrender
        | Context

    type [<StringEnum>] [<RequireQualifiedAccess>] WebGLRendererOffEvent =
        | Prerender
        | Postrender

    type [<StringEnum>] [<RequireQualifiedAccess>] WebGLRendererAddListenerEvent =
        | Prerender
        | Postrender

    /// The WebGLRenderer draws the scene and all its content onto a webGL enabled canvas. This renderer
    /// should be used for browsers that support webGL. This Render works by automatically managing webGLBatchs.
    /// So no need for Sprite Batches or Sprite Clouds.
    /// Don"t forget to add the view to your DOM or you will not see anything :)
    type [<AllowNullLiteral>] WebGLRendererStatic =
        abstract __plugins: WebGLRendererStatic__plugins with get, set
        /// Adds a plugin to the renderer.
        abstract registerPlugin: pluginName: string * ctor: WebGLRendererStaticRegisterPluginCtor -> unit
        [<EmitConstructor>] abstract Create: ?options: WebGLRendererOptions -> WebGLRenderer
        [<EmitConstructor>] abstract Create: ?screenWidth: float * ?screenHeight: float * ?options: WebGLRendererOptions -> WebGLRenderer

    type [<AllowNullLiteral>] WebGLRendererStaticRegisterPluginCtor =
        [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> WebGLRenderer

    /// A WebGL state machines
    type [<AllowNullLiteral>] WebGLState =
        abstract activeState: ResizeArray<float> with get, set
        abstract defaultState: ResizeArray<float> with get, set
        abstract stackIndex: float with get, set
        abstract stack: ResizeArray<float> with get, set
//        abstract gl: WebGLRenderingContext with get, set
        abstract maxAttribs: float with get, set
        abstract attribState: GlCore.AttribState with get, set
        abstract nativeVaoExtension: obj option with get, set
        abstract push: unit -> unit
        abstract pop: unit -> unit
        abstract setState: state: ResizeArray<float> -> unit
        abstract setBlend: value: float -> unit
        abstract setBlendMode: value: float -> unit
        abstract setDepthTest: value: float -> unit
        abstract setCullFace: value: float -> unit
        abstract setFrontFace: value: float -> unit
        abstract resetAttributes: unit -> unit
        abstract resetToDefault: unit -> unit

    /// A WebGL state machines
    type [<AllowNullLiteral>] WebGLStateStatic =
        interface end
//        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext -> WebGLState

    /// Helper class to create a webGL Texture
    type [<AllowNullLiteral>] TextureManager =
        abstract renderer: WebGLRenderer with get, set
//        abstract gl: WebGLRenderingContext with get, set
        abstract _managedTextures: ResizeArray<Texture> with get, set
        abstract bindTexture: unit -> unit
//        abstract getTexture: unit -> WebGLTexture
//        abstract updateTexture: texture: U2<BaseTexture, Texture> -> WebGLTexture
        abstract destroyTexture: texture: BaseTexture * ?_skipRemove: bool -> unit
        abstract removeAll: unit -> unit
        abstract destroy: unit -> unit

    /// Helper class to create a webGL Texture
    type [<AllowNullLiteral>] TextureManagerStatic =
        [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> TextureManager

    /// TextureGarbageCollector. This class manages the GPU and ensures that it does not get clogged
    /// up with textures that are no longer being used.
    type [<AllowNullLiteral>] TextureGarbageCollector =
        abstract renderer: WebGLRenderer with get, set
        abstract count: float with get, set
        abstract checkCount: float with get, set
        abstract maxIdle: float with get, set
        abstract checkCountMax: float with get, set
        abstract mode: float with get, set
        abstract update: unit -> unit
        abstract run: unit -> unit
        abstract unload: displayObject: DisplayObject -> unit

    /// TextureGarbageCollector. This class manages the GPU and ensures that it does not get clogged
    /// up with textures that are no longer being used.
    type [<AllowNullLiteral>] TextureGarbageCollectorStatic =
        [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> TextureGarbageCollector

    /// Base for a common object renderer that can be used as a system renderer plugin.
    type [<AllowNullLiteral>] ObjectRenderer =
        inherit WebGLManager
        /// Starts the renderer and sets the shader
        abstract start: unit -> unit
        abstract stop: unit -> unit
        /// Stub method for rendering content and emptying the current batch.
        abstract flush: unit -> unit
        abstract render: [<ParamArray>] args: obj option[] -> unit

    /// Base for a common object renderer that can be used as a system renderer plugin.
    type [<AllowNullLiteral>] ObjectRendererStatic =
        [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> ObjectRenderer

    /// Helper class to create a quad
    type [<AllowNullLiteral>] Quad =
//        abstract gl: WebGLRenderingContext with get, set
        abstract vertices: ResizeArray<float> with get, set
        abstract uvs: ResizeArray<float> with get, set
        abstract interleaved: ResizeArray<float> with get, set
        abstract indices: ResizeArray<float> with get, set
//        abstract vertexBuffer: WebGLBuffer with get, set
        abstract vao: GlCore.VertexArrayObject with get, set
        abstract initVao: shader: GlCore.GLShader -> unit
        abstract map: targetTextureFrame: Rectangle * destinationFrame: Rectangle -> Quad
        abstract upload: unit -> Quad
        abstract destroy: unit -> unit

    /// Helper class to create a quad
    type [<AllowNullLiteral>] QuadStatic =
        interface end
//        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext -> Quad

    type [<AllowNullLiteral>] FilterDataStackItem =
        abstract renderTarget: RenderTarget with get, set
        abstract filter: ResizeArray<obj option> with get, set
        abstract bounds: Rectangle with get, set

    type [<AllowNullLiteral>] RenderTarget =
        abstract filterPoolKey: string with get, set
//        abstract gl: WebGLRenderingContext with get, set
        abstract frameBuffer: GlCore.GLFramebuffer with get, set
        abstract texture: Texture with get, set
        abstract clearColor: ResizeArray<float> with get, set
        abstract size: Rectangle with get, set
        abstract resolution: float with get, set
        abstract projectionMatrix: Matrix with get, set
        abstract transform: Matrix with get, set
        abstract frame: Rectangle with get, set
        abstract defaultFrame: Rectangle with get, set
        abstract destinationFrame: Rectangle with get, set
        abstract sourceFrame: Rectangle option with get, set
        abstract stencilBuffer: GlCore.GLFramebuffer with get, set
        abstract stencilMaskStack: ResizeArray<Graphics> with get, set
        abstract filterData: RenderTargetFilterData with get, set
        abstract scaleMode: float with get, set
        abstract root: bool with get, set
        abstract clear: ?clearColor: ResizeArray<float> -> unit
        abstract attachStencilBuffer: unit -> unit
        abstract setFrame: destinationFrame: Rectangle * sourceFrame: Rectangle -> unit
        abstract activate: unit -> unit
        abstract calculateProjection: destinationFrame: Rectangle * sourceFrame: Rectangle -> unit
        abstract resize: width: float * height: float -> unit
        abstract destroy: unit -> unit

    type [<AllowNullLiteral>] RenderTargetStatic =
        interface end
//        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * width: float * height: float * scaleMode: float * resolution: float * ?root: bool -> RenderTarget

    type [<AllowNullLiteral>] BlendModeManager =
        inherit WebGLManager
        abstract currentBlendMode: float with get, set
        abstract setBlendMode: blendMode: float -> bool

    type [<AllowNullLiteral>] BlendModeManagerStatic =
        [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> BlendModeManager

    type [<AllowNullLiteral>] FilterManagerStackItem =
        abstract renderTarget: RenderTarget with get, set
        abstract sourceFrame: Rectangle with get, set
        abstract destinationFrame: Rectangle with get, set
//        abstract filters: Array<Filter<obj option>> with get, set
        abstract target: obj option with get, set
        abstract resolution: float with get, set

    type [<AllowNullLiteral>] FilterManager =
        inherit WebGLManager
        abstract _screenWidth: float with get, set
        abstract _screenHeight: float with get, set
//        abstract gl: WebGLRenderingContext with get, set
        abstract quad: Quad with get, set
        abstract stack: ResizeArray<FilterManagerStackItem> with get, set
        abstract stackIndex: float with get, set
        abstract shaderCache: obj option with get, set
        abstract filterData: obj option with get, set
        abstract onPrerender: unit -> unit
//        abstract pushFilter: target: RenderTarget * filters: Array<Filter<obj option>> -> unit
        abstract popFilter: unit -> unit
//        abstract applyFilter: shader: U2<GlCore.GLShader, Filter<obj option>> * inputTarget: RenderTarget * outputTarget: RenderTarget * ?clear: bool -> unit
//        abstract syncUniforms: shader: GlCore.GLShader * filter: Filter<obj option> -> unit
        abstract getRenderTarget: ?clear: bool * ?resolution: float -> RenderTarget
        abstract returnRenderTarget: renderTarget: RenderTarget -> RenderTarget
        abstract calculateScreenSpaceMatrix: outputMatrix: Matrix -> Matrix
        abstract calculateNormalizedScreenSpaceMatrix: outputMatrix: Matrix -> Matrix
        abstract calculateSpriteMatrix: outputMatrix: Matrix * sprite: Sprite -> Matrix
        abstract destroy: ?contextLost: bool -> unit
        abstract emptyPool: unit -> unit
//        abstract getPotRenderTarget: gl: WebGLRenderingContext * minWidth: float * minHeight: float * resolution: float -> RenderTarget
        abstract freePotRenderTarget: renderTarget: RenderTarget -> unit

    type [<AllowNullLiteral>] FilterManagerStatic =
        [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> FilterManager

    type [<AllowNullLiteral>] StencilMaskStack =
        abstract stencilStack: ResizeArray<obj option> with get, set
        abstract reverse: bool with get, set
        abstract count: float with get, set

    type [<AllowNullLiteral>] StencilMaskStackStatic =
        [<EmitConstructor>] abstract Create: unit -> StencilMaskStack

    type [<AllowNullLiteral>] MaskManager =
        inherit WebGLManager
        abstract scissor: bool with get, set
        abstract scissorData: obj option with get, set
        abstract scissorRenderTarget: RenderTarget with get, set
        abstract enableScissor: bool with get, set
        abstract alphaMaskPool: ResizeArray<float> with get, set
        abstract alphaMaskIndex: float with get, set
        abstract pushMask: target: RenderTarget * maskData: U2<Sprite, Graphics> -> unit
        abstract popMask: target: RenderTarget * maskData: U2<Sprite, Graphics> -> unit
        abstract pushSpriteMask: target: RenderTarget * maskData: U2<Sprite, Graphics> -> unit
        abstract popSpriteMask: unit -> unit
        abstract pushStencilMask: maskData: U2<Sprite, Graphics> -> unit
        abstract popStencilMask: unit -> unit
        abstract pushScissorMask: target: RenderTarget * maskData: U2<Sprite, Graphics> -> unit
        abstract popScissorMask: unit -> unit

    type [<AllowNullLiteral>] MaskManagerStatic =
        [<EmitConstructor>] abstract Create: unit -> MaskManager

    type [<AllowNullLiteral>] StencilManager =
        inherit WebGLManager
        abstract stencilMaskStack: ResizeArray<Graphics> with get, set
        abstract _useCurrent: unit -> unit
        abstract _getBitwiseMask: unit -> float
        abstract setMaskStack: stencilMasStack: ResizeArray<Graphics> -> unit
        abstract pushStencil: graphics: Graphics -> unit
        abstract popStencil: unit -> unit
        abstract destroy: unit -> unit

    type [<AllowNullLiteral>] StencilManagerStatic =
        [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> StencilManager

    type [<AllowNullLiteral>] WebGLManager =
        abstract renderer: WebGLRenderer with get, set
        abstract onContextChange: unit -> unit
        abstract destroy: unit -> unit

    type [<AllowNullLiteral>] WebGLManagerStatic =
        [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> WebGLManager

    type [<AllowNullLiteral>] UniformData<'V> =
        abstract ``type``: string with get, set
        abstract value: 'V with get, set
        abstract name: string option with get, set

    type [<AllowNullLiteral>] UniformDataMap<'U> =
        interface end

    type [<AllowNullLiteral>] Filter<'U when 'U :> Object> =
        abstract _blendMode: float with get, set
        abstract vertexSrc: string option with get, set
        abstract fragmentSrc: string with get, set
        abstract blendMode: float with get, set
        abstract uniformData: UniformDataMap<'U> with get, set
        abstract uniforms: 'U with get, set
        abstract glShaders: obj option with get, set
        abstract glShaderKey: float option with get, set
        abstract padding: float with get, set
        abstract resolution: float with get, set
        abstract enabled: bool with get, set
        abstract autoFit: bool with get, set
        abstract apply: filterManager: FilterManager * input: RenderTarget * output: RenderTarget * ?clear: bool * ?currentState: obj -> unit

    type [<AllowNullLiteral>] FilterStatic =
        [<EmitConstructor>] abstract Create: ?vertexSrc: string * ?fragmentSrc: string * ?uniformData: UniformDataMap<'U> -> Filter<'U>
        abstract defaultVertexSrc: string with get, set
        abstract defaultFragmentSrc: string with get, set

    type [<AllowNullLiteral>] SpriteMaskFilterUniforms =
        abstract mask: Texture with get, set
        abstract otherMatrix: Matrix with get, set
        abstract alpha: float with get, set

    /// The SpriteMaskFilter class
    type [<AllowNullLiteral>] SpriteMaskFilter =
//        inherit Filter<SpriteMaskFilterUniforms>
        abstract maskSprite: Sprite with get, set
        abstract maskMatrix: Matrix with get, set
        abstract apply: filterManager: FilterManager * input: RenderTarget * output: RenderTarget * ?clear: bool -> unit

    /// The SpriteMaskFilter class
    type [<AllowNullLiteral>] SpriteMaskFilterStatic =
        [<EmitConstructor>] abstract Create: sprite: Sprite -> SpriteMaskFilter

    /// <summary>
    /// The Sprite object is the base for all textured objects that are rendered to the screen
    /// 
    /// A sprite can be created directly from an image like this:
    /// 
    /// <code language="js">
    /// let sprite = new PIXI.Sprite.fromImage("assets/image.png");
    /// </code>
    /// 
    /// The more efficient way to create sprites is using a <see cref="PIXI.Spritesheet" />:
    /// 
    /// <code language="js">
    /// PIXI.loader.add("assets/spritesheet.json").load(setup);
    /// 
    /// function setup() {
    ///    let sheet = PIXI.loader.resources["assets/spritesheet.json"].spritesheet;
    ///    let sprite = new PIXI.Sprite(sheet.textures["image.png"]);
    ///    ...
    /// }
    /// </code>
    /// </summary>
    type [<AllowNullLiteral>] Sprite =
        inherit Container
        abstract _anchor: ObservablePoint with get, set
        abstract anchor: ObservablePoint with get, set
        abstract _texture: Texture with get, set
        abstract _transformTrimmedID: float with get, set
        abstract _textureTrimmedID: float with get, set
        abstract _width: float with get, set
        abstract _height: float with get, set
        abstract tint: float with get, set
        abstract _tint: float with get, set
        abstract _tintRGB: float with get, set
        abstract blendMode: float with get, set
        abstract pluginName: string with get, set
        abstract cachedTint: float with get, set
        abstract texture: Texture with get, set
        abstract textureDirty: bool with get, set
        abstract _textureID: float with get, set
        abstract _transformID: float with get, set
        abstract vertexTrimmedData: Float32Array with get, set
        abstract vertexData: Float32Array with get, set
        abstract width: float with get, set
        abstract height: float with get, set
        abstract _onTextureUpdate: unit -> unit
        /// calculates worldTransform * vertices, store it in vertexData
        abstract calculateVertices: unit -> unit
        /// Recalculates the bounds of the object. Override this to
        /// calculate the bounds of the specific object (not including children).
        abstract _calculateBounds: unit -> unit
        /// calculates worldTransform * vertices for a non texture with a trim. store it in vertexTrimmedData
        /// This is used to ensure that the true width and height of a trimmed texture is respected
        abstract calculateTrimmedVertices: unit -> unit
        abstract onAnchorUpdate: unit -> unit
        abstract _renderWebGL: renderer: WebGLRenderer -> unit
        abstract _renderCanvas: renderer: CanvasRenderer -> unit
        /// <summary>Gets the local bounds of the sprite object.</summary>
        /// <param name="rect">The output rectangle.</param>
        /// <returns>The bounds.</returns>
        abstract getLocalBounds: unit -> Rectangle
        /// <summary>Tests if a point is inside this sprite</summary>
        /// <param name="point">the point to test</param>
        /// <returns>the result of the test</returns>
        abstract containsPoint: point: Point -> bool
        /// <summary>Destroys this sprite and optionally its texture and children</summary>
        /// <param name="options">Options parameter. A boolean will act as if all options have been set to that value</param>
        abstract destroy: ?options: U2<DestroyOptions, bool> -> unit

    /// <summary>
    /// The Sprite object is the base for all textured objects that are rendered to the screen
    /// 
    /// A sprite can be created directly from an image like this:
    /// 
    /// <code language="js">
    /// let sprite = new PIXI.Sprite.fromImage("assets/image.png");
    /// </code>
    /// 
    /// The more efficient way to create sprites is using a <see cref="PIXI.Spritesheet" />:
    /// 
    /// <code language="js">
    /// PIXI.loader.add("assets/spritesheet.json").load(setup);
    /// 
    /// function setup() {
    ///    let sheet = PIXI.loader.resources["assets/spritesheet.json"].spritesheet;
    ///    let sprite = new PIXI.Sprite(sheet.textures["image.png"]);
    ///    ...
    /// }
    /// </code>
    /// </summary>
    type [<AllowNullLiteral>] SpriteStatic =
        [<EmitConstructor>] abstract Create: ?texture: Texture -> Sprite
        abstract from: source: U6<float, string, BaseTexture, HTMLImageElement, HTMLCanvasElement, HTMLVideoElement> -> Sprite
        abstract fromFrame: frameId: string -> Sprite
        abstract fromImage: imageId: string * ?crossorigin: bool * ?scaleMode: float -> Sprite

    type [<AllowNullLiteral>] BatchBuffer =
        abstract vertices: ArrayBuffer with get, set
        abstract float32View: ResizeArray<float> with get, set
        abstract uint32View: ResizeArray<float> with get, set
        abstract destroy: unit -> unit

    type [<AllowNullLiteral>] BatchBufferStatic =
        [<EmitConstructor>] abstract Create: unit -> BatchBuffer

    type [<AllowNullLiteral>] SpriteRenderer =
        inherit ObjectRenderer
        abstract vertSize: float with get, set
        abstract vertByteSize: float with get, set
        abstract size: float with get, set
        abstract buffers: ResizeArray<BatchBuffer> with get, set
        abstract indices: ResizeArray<float> with get, set
        abstract shaders: ResizeArray<Shader> with get, set
        abstract currentIndex: float with get, set
        abstract tick: float with get, set
        abstract groups: ResizeArray<obj option> with get, set
        abstract sprites: ResizeArray<Sprite> with get, set
        abstract vertexBuffers: ResizeArray<float> with get, set
        abstract vaos: ResizeArray<GlCore.VertexArrayObject> with get, set
        abstract vaoMax: float with get, set
        abstract vertexCount: float with get, set
        abstract onContextChanged: (unit -> unit) with get, set
        abstract onPrerender: (unit -> unit) with get, set
        abstract render: sprite: Sprite -> unit
        /// Stub method for rendering content and emptying the current batch.
        abstract flush: unit -> unit
        /// Starts the renderer and sets the shader
        abstract start: unit -> unit
        abstract stop: unit -> unit
        abstract destroy: unit -> unit

    type [<AllowNullLiteral>] SpriteRendererStatic =
        [<EmitConstructor>] abstract Create: renderer: PIXI.WebGLRenderer -> SpriteRenderer

    type [<AllowNullLiteral>] CanvasSpriteRenderer =
        inherit ObjectRenderer
        abstract render: sprite: Sprite -> unit
        abstract destroy: unit -> unit

    type [<AllowNullLiteral>] CanvasSpriteRendererStatic =
        [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> CanvasSpriteRenderer

    module CanvasTinter =

        type [<AllowNullLiteral>] IExports =
            abstract getTintedTexture: sprite: Sprite * color: float -> HTMLCanvasElement
            abstract tintWithMultiply: texture: Texture * color: float * canvas: HTMLCanvasElement -> unit
            abstract tintWithOverlay: texture: Texture * color: float * canvas: HTMLCanvasElement -> unit
            abstract tintWithPerPixel: texture: Texture * color: float * canvas: HTMLCanvasElement -> unit
            abstract roundColor: color: float -> float
            abstract cacheStepsPerColorChannel: float
            abstract convertTintToImage: bool
            abstract canUseMultiply: bool
            abstract tintMethod: float

    type [<AllowNullLiteral>] TextStyleOptions =
        abstract align: string option with get, set
        abstract breakWords: bool option with get, set
        abstract dropShadow: bool option with get, set
        abstract dropShadowAlpha: float option with get, set
        abstract dropShadowAngle: float option with get, set
        abstract dropShadowBlur: float option with get, set
        abstract dropShadowColor: U2<string, float> option with get, set
        abstract dropShadowDistance: float option with get, set
        abstract fill: U6<string, ResizeArray<string>, float, ResizeArray<float>, CanvasGradient, CanvasPattern> option with get, set
        abstract fillGradientType: float option with get, set
        abstract fillGradientStops: ResizeArray<float> option with get, set
        abstract fontFamily: U2<string, ResizeArray<string>> option with get, set
        abstract fontSize: U2<float, string> option with get, set
        abstract fontStyle: string option with get, set
        abstract fontVariant: string option with get, set
        abstract fontWeight: string option with get, set
        abstract letterSpacing: float option with get, set
        abstract lineHeight: float option with get, set
        abstract lineJoin: string option with get, set
        abstract miterLimit: float option with get, set
        abstract padding: float option with get, set
        abstract stroke: U2<string, float> option with get, set
        abstract strokeThickness: float option with get, set
        abstract textBaseline: string option with get, set
        abstract trim: bool option with get, set
        abstract whiteSpace: string option with get, set
        abstract wordWrap: bool option with get, set
        abstract wordWrapWidth: float option with get, set
        abstract leading: float option with get, set

    type [<AllowNullLiteral>] TextStyle =
        inherit TextStyleOptions
        abstract styleID: float with get, set
        abstract clone: unit -> TextStyle
        abstract reset: unit -> unit
        abstract _align: string with get, set
        abstract align: string with get, set
        abstract _breakWords: bool with get, set
        abstract breakWords: bool with get, set
        abstract _dropShadow: bool with get, set
        abstract dropShadow: bool with get, set
        abstract _dropShadowAlpha: float with get, set
        abstract dropShadowAlpha: float with get, set
        abstract _dropShadowAngle: float with get, set
        abstract dropShadowAngle: float with get, set
        abstract _dropShadowBlur: float with get, set
        abstract dropShadowBlur: float with get, set
        abstract _dropShadowColor: U2<string, float> with get, set
        abstract dropShadowColor: U2<string, float> with get, set
        abstract _dropShadowDistance: float with get, set
        abstract dropShadowDistance: float with get, set
        abstract _fill: U6<string, ResizeArray<string>, float, ResizeArray<float>, CanvasGradient, CanvasPattern> with get, set
        abstract fill: U6<string, ResizeArray<string>, float, ResizeArray<float>, CanvasGradient, CanvasPattern> with get, set
        abstract _fillGradientType: float with get, set
        abstract fillGradientType: float with get, set
        abstract _fillGradientStops: ResizeArray<float> with get, set
        abstract fillGradientStops: ResizeArray<float> with get, set
        abstract _fontFamily: U2<string, ResizeArray<string>> with get, set
        abstract fontFamily: U2<string, ResizeArray<string>> with get, set
        abstract _fontSize: U2<float, string> with get, set
        abstract fontSize: U2<float, string> with get, set
        abstract _fontStyle: string with get, set
        abstract fontStyle: string with get, set
        abstract _fontVariant: string with get, set
        abstract fontVariant: string with get, set
        abstract _fontWeight: string with get, set
        abstract fontWeight: string with get, set
        abstract _leading: float with get, set
        abstract leading: float with get, set
        abstract _letterSpacing: float with get, set
        abstract letterSpacing: float with get, set
        abstract _lineHeight: float with get, set
        abstract lineHeight: float with get, set
        abstract _lineJoin: string with get, set
        abstract lineJoin: string with get, set
        abstract _miterLimit: float with get, set
        abstract miterLimit: float with get, set
        abstract _padding: float with get, set
        abstract padding: float with get, set
        abstract _stroke: U2<string, float> with get, set
        abstract stroke: U2<string, float> with get, set
        abstract _strokeThickness: float with get, set
        abstract strokeThickness: float with get, set
        abstract _textBaseline: string with get, set
        abstract textBaseline: string with get, set
        abstract _trim: bool with get, set
        abstract trim: bool with get, set
        abstract _whiteSpace: string with get, set
        abstract whiteSpace: string with get, set
        abstract _wordWrap: bool with get, set
        abstract wordWrap: bool with get, set
        abstract _wordWrapWidth: float with get, set
        abstract wordWrapWidth: float with get, set
        abstract toFontString: unit -> string

    type [<AllowNullLiteral>] TextStyleStatic =
        [<EmitConstructor>] abstract Create: style: TextStyleOptions -> TextStyle

    type [<AllowNullLiteral>] TextMetrics =
        abstract text: string with get, set
        abstract style: TextStyle with get, set
        abstract width: float with get, set
        abstract height: float with get, set
        abstract lines: ResizeArray<float> with get, set
        abstract lineWidths: ResizeArray<float> with get, set
        abstract lineHeight: float with get, set
        abstract maxLineWidth: float with get, set
        abstract fontProperties: obj option with get, set

    type [<AllowNullLiteral>] TextMetricsStatic =
        abstract METRICS_STRING: string with get, set
        abstract BASELINE_SYMBOL: string with get, set
        abstract BASELINE_MULTIPLIER: float with get, set
        abstract _canvas: HTMLCanvasElement with get, set
        abstract _context: CanvasRenderingContext2D with get, set
        abstract _fonts: FontMetrics with get, set
        abstract _newLines: ResizeArray<float> with get, set
        abstract _breakingSpaces: ResizeArray<float> with get, set
        [<EmitConstructor>] abstract Create: text: string * style: TextStyle * width: float * height: float * lines: ResizeArray<float> * lineWidths: ResizeArray<float> * lineHeight: float * maxLineWidth: float * fontProperties: obj option -> TextMetrics
        abstract measureText: text: string * style: TextStyle * ?wordWrap: bool * ?canvas: HTMLCanvasElement -> TextMetrics
        abstract wordWrap: text: string * style: TextStyle * ?canvas: HTMLCanvasElement -> string
        abstract addLine: line: string * ?newLine: bool -> string
        abstract getFromCache: key: string * letterSpacing: float * cache: obj option * context: CanvasRenderingContext2D -> float
        abstract collapseSpaces: ?whiteSpace: string -> bool
        abstract collapseNewlines: ?whiteSpace: string -> bool
        abstract trimRight: ?text: string -> string
        abstract isNewline: ?char: string -> bool
        abstract isBreakingSpace: ?char: string -> bool
        abstract tokenize: ?text: string -> ResizeArray<string>
        abstract canBreakWords: ?token: string * ?breakWords: bool -> bool
        abstract canBreakChars: char: string * nextChar: string * token: string * index: float * ?breakWords: bool -> bool
        abstract measureFont: font: string -> FontMetrics
        abstract clearMetrics: font: string -> unit

    type [<AllowNullLiteral>] FontMetrics =
        abstract ascent: float with get, set
        abstract descent: float with get, set
        abstract fontSize: float with get, set

    type [<AllowNullLiteral>] Text =
        inherit Sprite
        abstract canvas: HTMLCanvasElement with get, set
        abstract context: CanvasRenderingContext2D with get, set
        abstract resolution: float with get, set
        abstract _text: string with get, set
        abstract _style: TextStyle with get, set
        abstract _styleListener: (ResizeArray<obj option> -> obj option) with get, set
        abstract _font: string with get, set
        abstract localStyleID: float with get, set
        abstract width: float with get, set
        abstract height: float with get, set
        abstract style: TextStyle with get, set
        abstract text: string with get, set
        abstract updateText: ?respectDirty: bool -> unit
        abstract drawLetterSpacing: text: string * x: float * y: float * ?isStroke: bool -> unit
        abstract updateTexture: unit -> unit
        /// Renders the object using the WebGL renderer
        abstract renderWebGL: renderer: WebGLRenderer -> unit
        abstract _renderCanvas: renderer: CanvasRenderer -> unit
        /// Gets the local bounds of the sprite object.
        abstract getLocalBounds: ?rect: Rectangle -> Rectangle
        /// Recalculates the bounds of the object. Override this to
        /// calculate the bounds of the specific object (not including children).
        abstract _calculateBounds: unit -> unit
        abstract _onStyleChange: (unit -> unit) with get, set
        abstract _generateFillStyle: style: TextStyle * lines: ResizeArray<string> -> U3<string, float, CanvasGradient>
        /// Destroys this sprite and optionally its texture and children
        abstract destroy: ?options: U2<DestroyOptions, bool> -> unit
        abstract dirty: bool with get, set

    type [<AllowNullLiteral>] TextStatic =
        [<EmitConstructor>] abstract Create: ?text: string * ?style: TextStyleOptions * ?canvas: HTMLCanvasElement -> Text

    type [<AllowNullLiteral>] BaseRenderTexture =
        inherit BaseTexture
        abstract height: float with get, set
        abstract width: float with get, set
        abstract realHeight: float with get, set
        abstract realWidth: float with get, set
        abstract resolution: float with get, set
        abstract scaleMode: float with get, set
        abstract hasLoaded: bool with get, set
        abstract _glRenderTargets: BaseRenderTexture_glRenderTargets with get, set
        abstract _canvasRenderTarget: BaseRenderTexture_glRenderTargets with get, set
        abstract valid: bool with get, set
        abstract resize: width: float * height: float -> unit
        abstract destroy: unit -> unit
        /// Add a listener for a given event.
        [<Emit "$0.on('update',$1,$2)">] abstract on_update: fn: (BaseRenderTexture -> unit) * ?context: obj -> BaseRenderTexture
        /// Add a one-time listener for a given event.
        [<Emit "$0.once('update',$1,$2)">] abstract once_update: fn: (BaseRenderTexture -> unit) * ?context: obj -> BaseRenderTexture
        /// Remove the listeners of a given event.
        [<Emit "$0.removeListener('update',$1,$2)">] abstract removeListener_update: ?fn: (BaseRenderTexture -> unit) * ?context: obj -> BaseRenderTexture
        /// Remove all listeners, or those of the specified event.
        [<Emit "$0.removeAllListeners('update')">] abstract removeAllListeners_update: unit -> BaseRenderTexture
        /// <summary>Alias method for <c>removeListener</c></summary>
        [<Emit "$0.off('update',$1,$2)">] abstract off_update: ?fn: (BaseRenderTexture -> unit) * ?context: obj -> BaseRenderTexture
        /// <summary>Alias method for <c>on</c></summary>
        [<Emit "$0.addListener('update',$1,$2)">] abstract addListener_update: fn: (BaseRenderTexture -> unit) * ?context: obj -> BaseRenderTexture

    type [<AllowNullLiteral>] BaseRenderTextureStatic =
        [<EmitConstructor>] abstract Create: ?width: float * ?height: float * ?scaleMode: float * ?resolution: float -> BaseRenderTexture

    type [<AllowNullLiteral>] BaseTexture =
        inherit Utils.EventEmitter
        abstract uuid: float option with get, set
        abstract touched: float with get, set
        abstract resolution: float with get, set
        abstract width: float with get, set
        abstract height: float with get, set
        abstract realWidth: float with get, set
        abstract realHeight: float with get, set
        abstract scaleMode: float with get, set
        abstract hasLoaded: bool with get, set
        abstract isLoading: bool with get, set
        abstract wrapMode: float with get, set
        abstract source: U3<HTMLImageElement, HTMLCanvasElement, HTMLVideoElement> option with get, set
        abstract origSource: HTMLImageElement option with get, set
        abstract imageType: string option with get, set
        abstract sourceScale: float with get, set
        abstract premultipliedAlpha: bool with get, set
        abstract imageUrl: string option with get, set
        abstract isPowerOfTwo: bool with get, set
        abstract mipmap: bool with get, set
        abstract wrap: bool option with get, set
        abstract _glTextures: obj option with get, set
        abstract _enabled: float with get, set
        abstract _id: float option with get, set
        abstract _virtualBoundId: float with get, set
        abstract _destroyed: bool
        abstract textureCacheIds: ResizeArray<string> with get, set
        abstract update: unit -> unit
        abstract _updateDimensions: unit -> unit
        abstract _updateImageType: unit -> unit
        abstract _loadSvgSource: unit -> unit
        abstract _loadSvgSourceUsingDataUri: dataUri: string -> unit
        abstract _loadSvgSourceUsingXhr: unit -> unit
        abstract _loadSvgSourceUsingString: svgString: string -> unit
        abstract loadSource: source: U3<HTMLImageElement, HTMLCanvasElement, HTMLVideoElement> -> unit
        abstract _sourceLoaded: unit -> unit
        abstract destroy: unit -> unit
        abstract dispose: unit -> unit
        abstract updateSourceImage: newSrc: string -> unit
        /// Add a listener for a given event.
        abstract on: ``event``: BaseTextureOnEvent * fn: (BaseTexture -> unit) * ?context: obj -> BaseTexture
        /// Add a one-time listener for a given event.
        abstract once: ``event``: BaseTextureOnceEvent * fn: (BaseTexture -> unit) * ?context: obj -> BaseTexture
        /// Remove the listeners of a given event.
        abstract removeListener: ``event``: BaseTextureRemoveListenerEvent * ?fn: (BaseTexture -> unit) * ?context: obj -> BaseTexture
        /// Remove all listeners, or those of the specified event.
        abstract removeAllListeners: ?``event``: BaseTextureRemoveAllListenersEvent -> BaseTexture
        /// <summary>Alias method for <c>removeListener</c></summary>
        abstract off: ``event``: BaseTextureOffEvent * ?fn: (BaseTexture -> unit) * ?context: obj -> BaseTexture
        /// <summary>Alias method for <c>on</c></summary>
        abstract addListener: ``event``: BaseTextureAddListenerEvent * fn: (BaseTexture -> unit) * ?context: obj -> BaseTexture

    type [<StringEnum>] [<RequireQualifiedAccess>] BaseTextureOnEvent =
        | Update
        | Loaded
        | Error
        | Dispose

    type [<StringEnum>] [<RequireQualifiedAccess>] BaseTextureOnceEvent =
        | Update
        | Loaded
        | Error
        | Dispose

    type [<StringEnum>] [<RequireQualifiedAccess>] BaseTextureRemoveListenerEvent =
        | Update
        | Loaded
        | Error
        | Dispose

    type [<StringEnum>] [<RequireQualifiedAccess>] BaseTextureRemoveAllListenersEvent =
        | Update
        | Loaded
        | Error
        | Dispose

    type [<StringEnum>] [<RequireQualifiedAccess>] BaseTextureOffEvent =
        | Update
        | Loaded
        | Error
        | Dispose

    type [<StringEnum>] [<RequireQualifiedAccess>] BaseTextureAddListenerEvent =
        | Update
        | Loaded
        | Error
        | Dispose

    type [<AllowNullLiteral>] BaseTextureStatic =
        abstract from: source: U3<string, HTMLImageElement, HTMLCanvasElement> * ?scaleMode: float * ?sourceScale: float -> BaseTexture
        [<EmitConstructor>] abstract Create: ?source: U3<HTMLImageElement, HTMLCanvasElement, HTMLVideoElement> * ?scaleMode: float * ?resolution: float -> BaseTexture
        abstract fromImage: imageUrl: string * ?crossorigin: bool * ?scaleMode: float * ?sourceScale: float -> BaseTexture
        abstract fromCanvas: canvas: HTMLCanvasElement * ?scaleMode: float * ?origin: string -> BaseTexture
        abstract addToCache: baseTexture: BaseTexture * id: string -> unit
        abstract removeFromCache: baseTexture: U2<string, BaseTexture> -> BaseTexture

    type [<AllowNullLiteral>] RenderTexture =
        inherit Texture
        abstract legacyRenderer: obj option with get, set
        abstract valid: bool with get, set
        abstract resize: width: float * height: float * ?doNotResizeBaseTexture: bool -> unit

    type [<AllowNullLiteral>] RenderTextureStatic =
        [<EmitConstructor>] abstract Create: baseRenderTexture: BaseRenderTexture * ?frame: Rectangle -> RenderTexture
        abstract create: ?width: float * ?height: float * ?scaleMode: float * ?resolution: float -> RenderTexture

    type [<AllowNullLiteral>] Texture =
        inherit Utils.EventEmitter
        abstract noFrame: bool with get, set
        abstract baseTexture: BaseTexture with get, set
        abstract _frame: Rectangle with get, set
        abstract trim: Rectangle option with get, set
        abstract valid: bool with get, set
        abstract requiresUpdate: bool with get, set
        abstract _uvs: TextureUvs with get, set
        abstract orig: Rectangle with get, set
        abstract defaultAnchor: Point with get, set
        abstract _updateID: float with get, set
        abstract transform: TextureMatrix with get, set
        abstract textureCacheIds: ResizeArray<string> with get, set
        abstract update: unit -> unit
        abstract onBaseTextureLoaded: baseTexture: BaseTexture -> unit
        abstract onBaseTextureUpdated: baseTexture: BaseTexture -> unit
        abstract destroy: ?destroyBase: bool -> unit
        abstract clone: unit -> Texture
        abstract _updateUvs: unit -> unit
        abstract frame: Rectangle with get, set
        abstract _rotate: U2<bool, float> with get, set
        abstract rotate: float with get, set
        abstract width: float with get, set
        abstract height: float with get, set
        /// Add a listener for a given event.
        [<Emit "$0.on('update',$1,$2)">] abstract on_update: fn: (Texture -> unit) * ?context: obj -> Texture
        /// Add a one-time listener for a given event.
        [<Emit "$0.once('update',$1,$2)">] abstract once_update: fn: (Texture -> unit) * ?context: obj -> Texture
        /// Remove the listeners of a given event.
        [<Emit "$0.removeListener('update',$1,$2)">] abstract removeListener_update: ?fn: (Texture -> unit) * ?context: obj -> Texture
        /// Remove all listeners, or those of the specified event.
        [<Emit "$0.removeAllListeners('update')">] abstract removeAllListeners_update: unit -> Texture
        /// <summary>Alias method for <c>removeListener</c></summary>
        [<Emit "$0.off('update',$1,$2)">] abstract off_update: ?fn: (Texture -> unit) * ?context: obj -> Texture
        /// <summary>Alias method for <c>on</c></summary>
        [<Emit "$0.addListener('update',$1,$2)">] abstract addListener_update: fn: (Texture -> unit) * ?context: obj -> Texture

    type [<AllowNullLiteral>] TextureStatic =
        [<EmitConstructor>] abstract Create: baseTexture: BaseTexture * ?frame: Rectangle * ?orig: Rectangle * ?trim: Rectangle * ?rotate: float * ?anchor: Point -> Texture
        abstract fromImage: imageUrl: string * ?crossOrigin: bool * ?scaleMode: float * ?sourceScale: float -> Texture
        abstract fromFrame: frameId: string -> Texture
        abstract fromCanvas: canvas: HTMLCanvasElement * ?scaleMode: float * ?origin: string -> Texture
        abstract fromVideo: video: U2<HTMLVideoElement, string> * ?scaleMode: float * ?crossorigin: bool * ?autoPlay: bool -> Texture
        abstract fromVideoUrl: videoUrl: string * ?scaleMode: float * ?crossorigin: bool * ?autoPlay: bool -> Texture
        abstract from: source: U6<float, string, HTMLImageElement, HTMLCanvasElement, HTMLVideoElement, BaseTexture> -> Texture
        abstract fromLoader: source: U2<HTMLImageElement, HTMLCanvasElement> * imageUrl: string * ?name: string -> Texture
        abstract addToCache: texture: Texture * id: string -> unit
        abstract removeFromCache: texture: U2<string, Texture> -> Texture
        abstract addTextureToCache: texture: Texture * id: string -> unit
        abstract removeTextureFromCache: id: string -> Texture
        abstract EMPTY: Texture with get, set
        abstract WHITE: Texture with get, set

    type [<AllowNullLiteral>] TextureMatrix =
        abstract _texture: Texture with get, set
        abstract mapCoord: Matrix with get, set
        abstract uClampFrame: Float32Array with get, set
        abstract uClampOffset: Float32Array with get, set
        abstract _lastTextureID: float with get, set
        abstract clampOffset: float with get, set
        abstract clampMargin: float with get, set
        abstract texture: Texture with get, set
        abstract update: ?forceUpdate: bool -> bool
        abstract multiplyUvs: uvs: Float32Array * ?out: Float32Array -> Float32Array

    type [<AllowNullLiteral>] TextureMatrixStatic =
        [<EmitConstructor>] abstract Create: texture: Texture * ?clampMargin: float -> TextureMatrix

    type [<AllowNullLiteral>] TextureUvs =
        abstract x0: float with get, set
        abstract y0: float with get, set
        abstract x1: float with get, set
        abstract y1: float with get, set
        abstract x2: float with get, set
        abstract y2: float with get, set
        abstract x3: float with get, set
        abstract y3: float with get, set
        abstract uvsUint32: Uint32Array with get, set
        abstract set: frame: Rectangle * baseFrame: Rectangle * rotate: float -> unit

    type [<AllowNullLiteral>] TextureUvsStatic =
        [<EmitConstructor>] abstract Create: unit -> TextureUvs

    type [<AllowNullLiteral>] Spritesheet =
        abstract baseTexture: BaseTexture with get, set
        abstract animations: SpritesheetAnimations with get, set
        abstract textures: SpritesheetTextures with get, set
        abstract data: obj option with get, set
        abstract resolution: float with get, set
        abstract _frames: obj option with get, set
        abstract _frameKeys: string with get, set
        abstract _batchIndex: float with get, set
        abstract _callback: (Spritesheet -> SpritesheetTextures -> unit) with get, set
        abstract _updateResolution: resolutionFilename: string -> float
        abstract parse: callback: (Spritesheet -> SpritesheetTextures -> unit) -> unit
        abstract _processFrames: initialFrameIndex: float -> unit
        abstract _parseComplete: unit -> unit
        abstract _processAnimations: unit -> unit
        abstract _nextBatch: unit -> unit
        abstract destroy: ?destroyBase: bool -> unit

    type [<AllowNullLiteral>] SpritesheetStatic =
        abstract BATCH_SIZE: float with get, set
        [<EmitConstructor>] abstract Create: baseTexture: BaseTexture * data: obj option * ?resolutionFilename: string -> Spritesheet

    type [<AllowNullLiteral>] VideoBaseTexture =
        inherit BaseTexture
        abstract autoUpdate: bool with get, set
        abstract autoPlay: bool with get, set
        abstract _isAutoUpdating: bool with get, set
        abstract update: unit -> unit
        abstract _onCanPlay: unit -> unit
        abstract _onPlayStart: unit -> unit
        abstract _onPlayStop: unit -> unit
        abstract destroy: unit -> unit
        abstract _isSourcePlaying: unit -> bool
        abstract _isSourceReady: unit -> bool
        abstract source: HTMLVideoElement with get, set
        abstract loadSource: source: HTMLVideoElement -> unit

    type [<AllowNullLiteral>] VideoBaseTextureStatic =
        [<EmitConstructor>] abstract Create: source: HTMLVideoElement * ?scaleMode: float * ?autoPlay: bool -> VideoBaseTexture
        abstract fromVideo: video: HTMLVideoElement * ?scaleMode: float * ?autoPlay: bool -> VideoBaseTexture
        abstract fromUrl: videoSrc: U4<string, obj option, ResizeArray<string>, ResizeArray<obj option>> * ?crossorigin: bool * ?autoPlay: bool -> VideoBaseTexture
        abstract fromUrls: videoSrc: U4<string, obj option, ResizeArray<string>, ResizeArray<obj option>> -> VideoBaseTexture

    module Ticker =

        type [<AllowNullLiteral>] IExports =
            abstract shared: Ticker
            abstract TickerListener: TickerListenerStatic
            abstract Ticker: TickerStatic

        type [<AllowNullLiteral>] TickerListener =
            abstract fn: (float -> unit) with get, set
            abstract context: obj option with get, set
            abstract priority: float with get, set
            abstract once: bool with get, set
            abstract next: TickerListener with get, set
            abstract previous: TickerListener with get, set
            abstract _destroyed: bool with get, set
            abstract ``match``: fn: (float -> unit) * ?context: obj -> bool
            abstract emit: deltaTime: float -> TickerListener
            abstract connect: previous: TickerListener -> unit
            abstract destroy: ?hard: bool -> unit

        type [<AllowNullLiteral>] TickerListenerStatic =
            [<EmitConstructor>] abstract Create: fn: (float -> unit) * ?context: obj * ?priority: float * ?once: bool -> TickerListener

        type [<AllowNullLiteral>] Ticker =
            abstract _tick: (float -> unit) with get, set
            abstract _head: TickerListener with get, set
            abstract _requestId: float option with get, set
            abstract _maxElapsedMS: float with get, set
            abstract autoStart: bool with get, set
            abstract deltaTime: float with get, set
            abstract elapsedMS: float with get, set
            abstract lastTime: float with get, set
            abstract speed: float with get, set
            abstract started: bool with get, set
            abstract _requestIfNeeded: unit -> unit
            abstract _cancelIfNeeded: unit -> unit
            abstract _startIfPossible: unit -> unit
            abstract add: fn: (float -> unit) * ?context: obj * ?priority: float -> Ticker
            abstract addOnce: fn: (float -> unit) * ?context: obj * ?priority: float -> Ticker
            abstract remove: fn: (ResizeArray<obj option> -> obj option) * ?context: obj * ?priority: float -> Ticker
            abstract _addListener: listener: TickerListener -> Ticker
            abstract FPS: float
            abstract minFPS: float with get, set
            abstract start: unit -> unit
            abstract stop: unit -> unit
            abstract destroy: unit -> unit
            abstract update: ?currentTime: float -> unit

        type [<AllowNullLiteral>] TickerStatic =
            [<EmitConstructor>] abstract Create: unit -> Ticker

    /// Wrapper class, webGL Shader for Pixi.
    /// Adds precision string if vertexSrc or fragmentSrc have no mention of it.
    type [<AllowNullLiteral>] Shader =
        inherit GlCore.GLShader

    /// Wrapper class, webGL Shader for Pixi.
    /// Adds precision string if vertexSrc or fragmentSrc have no mention of it.
    type [<AllowNullLiteral>] ShaderStatic =
        interface end
//        [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * vertexSrc: U2<string, ResizeArray<string>> * fragmentSrc: U2<string, ResizeArray<string>> * ?attributeLocations: ShaderStaticAttributeLocations * ?precision: string -> Shader

    type [<AllowNullLiteral>] ShaderStaticAttributeLocations =
        [<EmitIndexer>] abstract Item: key: string -> float with get, set

    module Extract =

        type [<AllowNullLiteral>] IExports =
            abstract CanvasExtract: CanvasExtractStatic
            abstract WebGLExtract: WebGLExtractStatic

        type [<AllowNullLiteral>] CanvasExtract =
            abstract renderer: CanvasRenderer with get, set
            abstract image: ?target: U2<DisplayObject, RenderTexture> -> HTMLImageElement
            abstract base64: ?target: U2<DisplayObject, RenderTexture> -> string
            abstract canvas: ?target: U2<DisplayObject, RenderTexture> -> HTMLCanvasElement
            abstract pixels: ?renderTexture: U2<DisplayObject, RenderTexture> -> Uint8ClampedArray
            abstract destroy: unit -> unit

        type [<AllowNullLiteral>] CanvasExtractStatic =
            [<EmitConstructor>] abstract Create: renderer: CanvasRenderer -> CanvasExtract

        type [<AllowNullLiteral>] WebGLExtract =
            abstract renderer: WebGLRenderer with get, set
            abstract image: ?target: U2<DisplayObject, RenderTexture> -> HTMLImageElement
            abstract base64: ?target: U2<DisplayObject, RenderTexture> -> string
            abstract canvas: ?target: U2<DisplayObject, RenderTexture> -> HTMLCanvasElement
            abstract pixels: ?renderTexture: U2<DisplayObject, RenderTexture> -> Uint8Array
            abstract destroy: unit -> unit

        type [<AllowNullLiteral>] WebGLExtractStatic =
            [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> WebGLExtract

    module Extras =

        type [<AllowNullLiteral>] IExports =
            abstract BitmapText: BitmapTextStatic
            abstract AnimatedSprite: AnimatedSpriteStatic
            abstract TextureMatrix: TextureMatrixStatic
            abstract TilingSprite: TilingSpriteStatic
            abstract TilingSpriteRenderer: TilingSpriteRendererStatic

        type [<AllowNullLiteral>] BitmapTextStyle =
            abstract font: U2<string, BitmapTextStyleFont> option with get, set
            abstract align: string option with get, set
            abstract tint: float option with get, set

        type [<AllowNullLiteral>] BitmapText =
            inherit Container
            abstract letterSpacing: float with get, set
            abstract _letterSpacing: float with get, set
            abstract _textWidth: float with get, set
            abstract _textHeight: float with get, set
            abstract textWidth: float with get, set
            abstract textHeight: float with get, set
            abstract _glyphs: ResizeArray<Sprite> with get, set
            abstract _font: U2<string, BitmapTextStyleFont> with get, set
            abstract font: U2<string, BitmapTextStyleFont> with get, set
            abstract _text: string with get, set
            abstract _maxWidth: float with get, set
            abstract maxWidth: float with get, set
            abstract _maxLineHeight: float with get, set
            abstract maxLineHeight: float with get, set
            abstract _anchor: ObservablePoint with get, set
            abstract dirty: bool with get, set
            abstract tint: float with get, set
            abstract align: string with get, set
            abstract text: string with get, set
            abstract anchor: U2<PIXI.Point, float> with get, set
            abstract updateText: unit -> unit
            /// Updates the transform on all children of this container for rendering
            abstract updateTransform: unit -> unit
            /// Retrieves the local bounds of the displayObject as a rectangle object
            abstract getLocalBounds: unit -> Rectangle
            abstract validate: unit -> unit

        type [<AllowNullLiteral>] BitmapTextStatic =
            abstract registerFont: xml: XMLDocument * textures: U3<Texture, ResizeArray<Texture>, BitmapTextStaticRegisterFont> -> obj option
            [<EmitConstructor>] abstract Create: text: string * ?style: BitmapTextStyle -> BitmapText
            abstract fonts: obj option with get, set

        type [<AllowNullLiteral>] AnimatedSpriteTextureTimeObject =
            abstract texture: Texture with get, set
            abstract time: float option with get, set

        type [<AllowNullLiteral>] AnimatedSprite =
            inherit Sprite
            abstract _autoUpdate: bool with get, set
            abstract _textures: ResizeArray<Texture> with get, set
            abstract _durations: ResizeArray<float> with get, set
            abstract textures: U2<ResizeArray<Texture>, ResizeArray<AnimatedSpriteTextureTimeObject>> with get, set
            abstract animationSpeed: float with get, set
            abstract loop: bool with get, set
            abstract updateAnchor: bool with get, set
            abstract onComplete: (unit -> unit) with get, set
            abstract onFrameChange: (float -> unit) with get, set
            abstract onLoop: (unit -> unit) with get, set
            abstract _currentTime: float with get, set
            abstract playing: bool with get, set
            abstract totalFrames: float with get, set
            abstract currentFrame: float with get, set
            abstract stop: unit -> unit
            abstract play: unit -> unit
            abstract gotoAndStop: frameNumber: float -> unit
            abstract gotoAndPlay: frameNumber: float -> unit
            abstract update: deltaTime: float -> unit
            /// Destroys this sprite and optionally its texture and children
            abstract destroy: ?options: U2<DestroyOptions, bool> -> unit

        type [<AllowNullLiteral>] AnimatedSpriteStatic =
            [<EmitConstructor>] abstract Create: textures: U2<ResizeArray<Texture>, ResizeArray<AnimatedSpriteTextureTimeObject>> * ?autoUpdate: bool -> AnimatedSprite
            abstract fromFrames: frame: ResizeArray<string> -> AnimatedSprite
            abstract fromImages: images: ResizeArray<string> -> AnimatedSprite

        type [<AllowNullLiteral>] TextureMatrix =
            abstract _texture: Texture with get, set
            abstract mapCoord: Matrix with get, set
            abstract uClampFrame: Float32Array with get, set
            abstract uClampOffset: Float32Array with get, set
            abstract _lastTextureID: float with get, set
            abstract clampOffset: float with get, set
            abstract clampMargin: float with get, set
            abstract texture: Texture with get, set
            abstract update: ?forceUpdate: bool -> bool
            abstract multiplyUvs: uvs: Float32Array * ?out: Float32Array -> Float32Array

        type [<AllowNullLiteral>] TextureMatrixStatic =
            [<EmitConstructor>] abstract Create: texture: Texture * ?clampMargin: float -> TextureMatrix

        type [<AllowNullLiteral>] TilingSprite =
            inherit Sprite
            abstract tileTransform: TransformStatic with get, set
            abstract _width: float with get, set
            abstract _height: float with get, set
            abstract _canvasPattern: CanvasPattern with get, set
            abstract uvTransform: TextureMatrix with get, set
            abstract uvRespectAnchor: bool with get, set
            abstract clampMargin: float with get, set
            abstract tileScale: U2<Point, ObservablePoint> with get, set
            abstract tilePosition: U2<Point, ObservablePoint> with get, set
            abstract multiplyUvs: uvs: Float32Array * out: Float32Array -> Float32Array
            abstract _onTextureUpdate: unit -> unit
            abstract _renderWebGL: renderer: WebGLRenderer -> unit
            abstract _renderCanvas: renderer: CanvasRenderer -> unit
            /// Recalculates the bounds of the object. Override this to
            /// calculate the bounds of the specific object (not including children).
            abstract _calculateBounds: unit -> unit
            /// Gets the local bounds of the sprite object.
            abstract getLocalBounds: ?rect: Rectangle -> Rectangle
            /// Tests if a point is inside this sprite
            abstract containsPoint: point: Point -> bool
            /// Destroys this sprite and optionally its texture and children
            abstract destroy: ?options: U2<DestroyOptions, bool> -> unit
            abstract width: float with get, set
            abstract height: float with get, set

        type [<AllowNullLiteral>] TilingSpriteStatic =
            [<EmitConstructor>] abstract Create: texture: Texture * ?width: float * ?height: float -> TilingSprite
            abstract from: source: U5<float, string, BaseTexture, HTMLCanvasElement, HTMLVideoElement> * ?width: float * ?height: float -> TilingSprite
            abstract fromFrame: frameId: string * ?width: float * ?height: float -> TilingSprite
            abstract fromImage: imageId: string * ?crossorigin: bool * ?scaleMode: float -> Sprite
            abstract fromImage: imageId: string * ?width: float * ?height: float * ?crossorigin: bool * ?scaleMode: float -> TilingSprite

        type [<AllowNullLiteral>] TilingSpriteRenderer =
            inherit ObjectRenderer
            abstract render: ts: TilingSprite -> unit

        type [<AllowNullLiteral>] TilingSpriteRendererStatic =
            [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> TilingSpriteRenderer

        /// <seealso cref="PIXI.extras.AnimatedSprite" />
        [<Obsolete("since version 4.2.0")>]
        type MovieClip =
            Extras.AnimatedSprite

        /// <seealso cref="PIXI.TextureMatrix" />
        [<Obsolete("since version 4.6.0")>]
        type TextureTranform =
            TextureMatrix

        type [<AllowNullLiteral>] BitmapTextStyleFont =
            abstract name: string option with get, set
            abstract size: float option with get, set

        type [<AllowNullLiteral>] BitmapTextStaticRegisterFont =
            [<EmitIndexer>] abstract Item: key: string -> Texture with get, set

    module Filters =

        type [<AllowNullLiteral>] IExports =
            abstract FXAAFilter: FXAAFilterStatic
            abstract BlurFilter: BlurFilterStatic
            abstract BlurXFilter: BlurXFilterStatic
            abstract BlurYFilter: BlurYFilterStatic
            abstract ColorMatrixFilter: ColorMatrixFilterStatic
            abstract DisplacementFilter: DisplacementFilterStatic
            abstract AlphaFilter: AlphaFilterStatic
            abstract NoiseFilter: NoiseFilterStatic

        type [<AllowNullLiteral>] FXAAFilter =
            interface end
//            inherit Filter<FXAAFilterFilter>

        type [<AllowNullLiteral>] FXAAFilterStatic =
            [<EmitConstructor>] abstract Create: unit -> FXAAFilter

        type [<AllowNullLiteral>] BlurFilter =
//            inherit Filter<FXAAFilterFilter>
            abstract blurXFilter: BlurXFilter with get, set
            abstract blurYFilter: BlurYFilter with get, set
            abstract resolution: float with get, set
            abstract padding: float with get, set
            abstract passes: float with get, set
            abstract blur: float with get, set
            abstract blurX: float with get, set
            abstract blurY: float with get, set
            abstract quality: float with get, set
            abstract blendMode: float with get, set

        type [<AllowNullLiteral>] BlurFilterStatic =
            [<EmitConstructor>] abstract Create: ?strength: float * ?quality: float * ?resolution: float * ?kernelSize: float -> BlurFilter

        type [<AllowNullLiteral>] BlurXFilterUniforms =
            abstract strength: float with get, set

        type [<AllowNullLiteral>] BlurXFilter =
//            inherit Filter<BlurXFilterUniforms>
            abstract _quality: float with get, set
            abstract quality: float with get, set
            abstract passes: float with get, set
            abstract resolution: float with get, set
            abstract strength: float with get, set
            abstract firstRun: bool with get, set
            abstract blur: float with get, set

        type [<AllowNullLiteral>] BlurXFilterStatic =
            [<EmitConstructor>] abstract Create: ?strength: float * ?quality: float * ?resolution: float * ?kernelSize: float -> BlurXFilter

        type [<AllowNullLiteral>] BlurYFilterUniforms =
            abstract strength: float with get, set

        type [<AllowNullLiteral>] BlurYFilter =
//            inherit Filter<BlurYFilterUniforms>
            abstract _quality: float with get, set
            abstract quality: float with get, set
            abstract passes: float with get, set
            abstract resolution: float with get, set
            abstract strength: float with get, set
            abstract firstRun: bool with get, set
            abstract blur: float with get, set

        type [<AllowNullLiteral>] BlurYFilterStatic =
            [<EmitConstructor>] abstract Create: ?strength: float * ?quality: float * ?resolution: float * ?kernelSize: float -> BlurYFilter

        type [<AllowNullLiteral>] ColorMatrixFilterUniforms =
            abstract m: Matrix with get, set
            abstract uAlpha: float with get, set

        type [<AllowNullLiteral>] ColorMatrixFilter =
//            inherit Filter<ColorMatrixFilterUniforms>
            abstract _loadMatrix: matrix: ResizeArray<float> * ?multiply: bool -> unit
            abstract _multiply: out: ResizeArray<float> * a: ResizeArray<float> * b: ResizeArray<float> -> unit
            abstract _colorMatrix: matrix: ResizeArray<float> -> unit
            abstract matrix: ResizeArray<float> with get, set
            abstract alpha: float with get, set
            abstract brightness: b: float * ?multiply: bool -> unit
            abstract greyscale: scale: float * ?multiply: bool -> unit
            abstract blackAndWhite: ?multiply: bool -> unit
            abstract hue: rotation: float * ?multiply: bool -> unit
            abstract contrast: amount: float * ?multiply: bool -> unit
            abstract saturate: amount: float * ?multiply: bool -> unit
            abstract desaturate: ?multiply: bool -> unit
            abstract negative: ?multiply: bool -> unit
            abstract sepia: ?multiply: bool -> unit
            abstract technicolor: ?multiply: bool -> unit
            abstract polaroid: ?multiply: bool -> unit
            abstract toBGR: ?multiply: bool -> unit
            abstract kodachrome: ?multiply: bool -> unit
            abstract browni: ?multiply: bool -> unit
            abstract vintage: ?multiply: bool -> unit
            abstract colorTone: desaturation: float * toned: float * lightColor: string * darkColor: string * ?multiply: bool -> unit
            abstract night: intensity: float * ?multiply: bool -> unit
            abstract predator: amount: float * ?multiply: bool -> unit
            abstract lsd: ?multiply: bool -> unit
            abstract reset: unit -> unit

        type [<AllowNullLiteral>] ColorMatrixFilterStatic =
            [<EmitConstructor>] abstract Create: unit -> ColorMatrixFilter

        type [<AllowNullLiteral>] DisplacementFilterUniforms =
            abstract mapSampler: Texture with get, set
            abstract filterMatrix: Matrix with get, set
            abstract scale: Point with get, set

        type [<AllowNullLiteral>] DisplacementFilter =
//            inherit Filter<DisplacementFilterUniforms>
            abstract scale: Point with get, set
            abstract map: Texture with get, set

        type [<AllowNullLiteral>] DisplacementFilterStatic =
            [<EmitConstructor>] abstract Create: sprite: Sprite * ?scale: float -> DisplacementFilter

        type [<AllowNullLiteral>] AlphaFilter =
//            inherit Filter<FXAAFilterFilter>
            abstract alpha: float with get, set
            abstract glShaderKey: float with get, set

        type [<AllowNullLiteral>] AlphaFilterStatic =
            [<EmitConstructor>] abstract Create: ?alpha: float -> AlphaFilter

        type [<AllowNullLiteral>] NoiseFilterUniforms =
            abstract uNoise: float with get, set
            abstract uSeed: float with get, set

        type [<AllowNullLiteral>] NoiseFilter =
//            inherit Filter<NoiseFilterUniforms>
            abstract noise: float with get, set
            abstract seed: float with get, set

        type [<AllowNullLiteral>] NoiseFilterStatic =
            [<EmitConstructor>] abstract Create: ?noise: float * ?seed: float -> NoiseFilter

        /// <seealso cref="PIXI.filters.AlphaFilter" />
        [<Obsolete("since version 4.5.7")>]
        type VoidFilter =
            Filters.AlphaFilter

        type [<AllowNullLiteral>] FXAAFilterFilter =
            interface end

    module Interaction =

        type [<AllowNullLiteral>] IExports =
            abstract InteractionData: InteractionDataStatic
            abstract InteractionManager: InteractionManagerStatic

        type [<AllowNullLiteral>] InteractiveTarget =
            /// <summary>
            /// Enable interaction events for the DisplayObject. Touch, pointer and mouse
            /// events will not be emitted unless <c>interactive</c> is set to <c>true</c>.
            /// </summary>
            /// <example>
            /// const sprite = new PIXI.Sprite(texture);
            /// sprite.interactive = true;
            /// sprite.on("tap", (event) => {
            ///    //handle event
            /// });
            /// </example>
            abstract interactive: bool with get, set
            /// <summary>
            /// Determines if the children to the displayObject can be clicked/touched
            /// Setting this to false allows PixiJS to bypass a recursive <c>hitTest</c> function
            /// </summary>
            abstract interactiveChildren: bool with get, set
            /// <summary>
            /// Interaction shape. Children will be hit first, then this shape will be checked.
            /// Setting this will cause this shape to be checked in hit tests rather than the displayObject"s bounds.
            /// </summary>
            /// <example>
            /// const sprite = new PIXI.Sprite(texture);
            /// sprite.interactive = true;
            /// sprite.hitArea = new PIXI.Rectangle(0, 0, 100, 100);
            /// </example>
            abstract hitArea: U6<PIXI.Rectangle, PIXI.Circle, PIXI.Ellipse, PIXI.Polygon, PIXI.RoundedRectangle, PIXI.HitArea> with get, set
            /// <summary>
            /// If enabled, the mouse cursor use the pointer behavior when hovered over the displayObject if it is interactive
            /// Setting this changes the "cursor" property to <c>"pointer"</c>.
            /// </summary>
            /// <example>
            /// const sprite = new PIXI.Sprite(texture);
            /// sprite.interactive = true;
            /// sprite.buttonMode = true;
            /// </example>
            abstract buttonMode: bool with get, set
            /// <summary>
            /// This defines what cursor mode is used when the mouse cursor
            /// is hovered over the displayObject.
            /// </summary>
            /// <example>
            /// const sprite = new PIXI.Sprite(texture);
            /// sprite.interactive = true;
            /// sprite.cursor = "wait";
            /// </example>
            /// <seealso href="https://developer.mozilla.org/en/docs/Web/CSS/cursor" />
            abstract cursor: string with get, set
            abstract trackedPointers: InteractiveTargetTrackedPointers with get, set
            [<Obsolete("")>]
            abstract defaultCursor: string with get, set

        type [<AllowNullLiteral>] InteractionTrackingData =
            abstract pointerId: float
            abstract flags: float with get, set
            abstract none: float with get, set
            abstract over: bool with get, set
            abstract rightDown: bool with get, set
            abstract leftDown: bool with get, set

        type [<AllowNullLiteral>] InteractionEvent =
            abstract stopped: bool with get, set
            abstract target: DisplayObject with get, set
            abstract currentTarget: DisplayObject with get, set
            abstract ``type``: string with get, set
            abstract data: InteractionData with get, set
            abstract stopPropagation: unit -> unit
            abstract reset: unit -> unit

        type [<AllowNullLiteral>] InteractionData =
            abstract ``global``: Point with get, set
            abstract target: DisplayObject with get, set
            abstract originalEvent: U3<MouseEvent, TouchEvent, PointerEvent> with get, set
            abstract identifier: float with get, set
            abstract isPrimary: bool with get, set
            abstract button: float with get, set
            abstract buttons: float with get, set
            abstract width: float with get, set
            abstract height: float with get, set
            abstract tiltX: float with get, set
            abstract tiltY: float with get, set
            abstract pointerType: string with get, set
            abstract pressure: float with get, set
            abstract rotationAngle: float with get, set
            abstract twist: float with get, set
            abstract tangentialPressure: float with get, set
            abstract pointerID: float
            abstract copyEvent: ``event``: U3<Touch, MouseEvent, PointerEvent> -> unit
            abstract reset: unit -> unit
            abstract getLocalPosition: displayObject: DisplayObject * ?point: Point * ?globalPos: Point -> Point

        type [<AllowNullLiteral>] InteractionDataStatic =
            [<EmitConstructor>] abstract Create: unit -> InteractionData

        type [<StringEnum>] [<RequireQualifiedAccess>] InteractionPointerEvents =
            | Pointerdown
            | Pointercancel
            | Pointerup
            | Pointertap
            | Pointerupoutside
            | Pointermove
            | Pointerover
            | Pointerout

        type [<StringEnum>] [<RequireQualifiedAccess>] InteractionTouchEvents =
            | Touchstart
            | Touchcancel
            | Touchend
            | Touchendoutside
            | Touchmove
            | Tap

        type [<StringEnum>] [<RequireQualifiedAccess>] InteractionMouseEvents =
            | Rightdown
            | Mousedown
            | Rightup
            | Mouseup
            | Rightclick
            | Click
            | Rightupoutside
            | Mouseupoutside
            | Mousemove
            | Mouseover
            | Mouseout

        type [<StringEnum>] [<RequireQualifiedAccess>] InteractionPixiEvents =
            | Added
            | Removed

        type InteractionEventTypes =
            U4<InteractionPointerEvents, InteractionTouchEvents, InteractionMouseEvents, InteractionPixiEvents>

        type [<AllowNullLiteral>] InteractionManagerOptions =
            abstract autoPreventDefault: bool option with get, set
            abstract interactionFrequency: float option with get, set

        type [<AllowNullLiteral>] InteractionManager =
            inherit Utils.EventEmitter
            abstract renderer: SystemRenderer with get, set
            abstract autoPreventDefault: bool with get, set
            abstract interactionFrequency: float with get, set
            abstract mouse: InteractionData with get, set
            abstract activeInteractionData: InteractionManagerActiveInteractionData with get, set
            abstract interactionDataPool: ResizeArray<InteractionData> with get, set
            abstract eventData: InteractionEvent with get, set
            abstract interactionDOMElement: HTMLElement with get, set
            abstract moveWhenInside: bool with get, set
            abstract eventsAdded: bool with get, set
            abstract mouseOverRenderer: bool with get, set
            abstract supportsTouchEvents: bool
            abstract supportsPointerEvents: bool
            abstract onPointerUp: (PointerEvent -> unit) with get, set
            abstract processPointerUp: (InteractionEvent -> U3<Container, PIXI.Sprite, PIXI.Extras.TilingSprite> -> bool -> unit) with get, set
            abstract onPointerCancel: (PointerEvent -> unit) with get, set
            abstract processPointerCancel: (InteractionEvent -> U3<PIXI.Container, PIXI.Sprite, PIXI.Extras.TilingSprite> -> unit) with get, set
            abstract onPointerDown: (PointerEvent -> unit) with get, set
            abstract processPointerDown: (InteractionEvent -> U3<PIXI.Container, PIXI.Sprite, PIXI.Extras.TilingSprite> -> bool -> unit) with get, set
            abstract onPointerMove: (PointerEvent -> unit) with get, set
            abstract processPointerMove: (InteractionEvent -> U3<PIXI.Container, PIXI.Sprite, PIXI.Extras.TilingSprite> -> bool -> unit) with get, set
            abstract onPointerOut: (PointerEvent -> unit) with get, set
            abstract processPointerOverOut: (InteractionEvent -> U3<PIXI.Container, PIXI.Sprite, PIXI.Extras.TilingSprite> -> bool -> unit) with get, set
            abstract onPointerOver: (PointerEvent -> unit) with get, set
            abstract cursorStyles: InteractionManagerCursorStyles with get, set
            abstract currentCursorMode: string with get, set
            abstract cursor: string with get, set
            abstract _tempPoint: Point with get, set
            abstract resolution: float with get, set
            abstract hitTest: globalPoint: Point * ?root: Container -> DisplayObject
            abstract setTargetElement: element: HTMLCanvasElement * ?resolution: float -> unit
            abstract addEvents: unit -> unit
            abstract removeEvents: unit -> unit
            abstract update: ?deltaTime: float -> unit
            abstract setCursorMode: mode: string -> unit
            abstract dispatchEvent: displayObject: U3<Container, Sprite, Extras.TilingSprite> * eventString: string * eventData: obj option -> unit
            abstract mapPositionToPoint: point: Point * x: float * y: float -> unit
            abstract processInteractive: interactionEvent: InteractionEvent * displayObject: U3<PIXI.Container, PIXI.Sprite, PIXI.Extras.TilingSprite> * ?func: (ResizeArray<obj option> -> obj option) * ?hitTest: bool * ?interactive: bool -> bool
            abstract onPointerComplete: originalEvent: PointerEvent * cancelled: bool * func: (ResizeArray<obj option> -> obj option) -> unit
            abstract getInteractionDataForPointerId: pointerId: float -> InteractionData
            abstract releaseInteractionDataForPointerId: ``event``: PointerEvent -> unit
            abstract configureInteractionEventForDOMEvent: interactionEvent: InteractionEvent * pointerEvent: PointerEvent * interactionData: InteractionData -> InteractionEvent
            abstract normalizeToPointerData: ``event``: U3<TouchEvent, MouseEvent, PointerEvent> -> ResizeArray<PointerEvent>
            abstract destroy: unit -> unit
            abstract defaultCursorStyle: string with get, set
            abstract currentCursorStyle: string with get, set

        type [<AllowNullLiteral>] InteractionManagerStatic =
            [<EmitConstructor>] abstract Create: renderer: U3<CanvasRenderer, WebGLRenderer, SystemRenderer> * ?options: InteractionManagerOptions -> InteractionManager

        type [<AllowNullLiteral>] InteractiveTargetTrackedPointers =
            [<EmitIndexer>] abstract Item: key: float -> InteractionTrackingData with get, set

        type [<AllowNullLiteral>] InteractionManagerActiveInteractionData =
            [<EmitIndexer>] abstract Item: key: float -> InteractionData with get, set

        type [<AllowNullLiteral>] InteractionManagerCursorStyles =
            abstract ``default``: string with get, set
            abstract pointer: string with get, set

    type [<AllowNullLiteral>] MiniSignalBinding =
        abstract detach: unit -> bool

    type [<AllowNullLiteral>] MiniSignalBindingStatic =
        [<EmitConstructor>] abstract Create: unit -> MiniSignalBinding

    type [<AllowNullLiteral>] MiniSignal<'CbType> =
        abstract handlers: exists: obj -> bool
        abstract handlers: ?exists: obj -> ResizeArray<MiniSignalBinding>
        abstract has: node: MiniSignalBinding -> bool
        abstract add: fn: (ResizeArray<obj option> -> obj option) * ?thisArg: obj -> obj option
        abstract add: fn: 'CbType * ?thisArg: obj -> MiniSignalBinding
        abstract once: fn: (ResizeArray<obj option> -> obj option) * ?thisArg: obj -> obj option
        abstract once: fn: 'CbType * ?thisArg: obj -> MiniSignalBinding
        abstract detach: node: MiniSignalBinding -> MiniSignal<'CbType>
        abstract detachAll: unit -> MiniSignal<'CbType>
        abstract dispatch: 'CbType with get, set

    type [<AllowNullLiteral>] MiniSignalStatic =
        [<EmitConstructor>] abstract Create: unit -> MiniSignal<'CbType>

    module Loaders =
        let [<Import("Resource","module/PIXI/loaders")>] resource: Resource.IExports = jsNative

        type [<AllowNullLiteral>] IExports =
            abstract encodeBinary: input: string -> string
            abstract Loader: LoaderStatic
            abstract Resource: ResourceStatic
            abstract shared: Loader

        /// <summary>Options for a call to <c>.add()</c>.</summary>
        type [<AllowNullLiteral>] LoaderOptions =
            abstract crossOrigin: U2<string, bool> option with get, set
            abstract timeout: float option with get, set
            abstract loadType: Resource.LOAD_TYPE option with get, set
            abstract xhrType: Resource.XHR_RESPONSE_TYPE option with get, set
            abstract onComplete: OnCompleteSignal option with get, set
            abstract callback: OnCompleteSignal option with get, set
            abstract metadata: IMetadata option with get, set

        type [<AllowNullLiteral>] ResourceDictionary =
            [<EmitIndexer>] abstract Item: key: string -> PIXI.Loaders.Resource with get, set

        type [<AllowNullLiteral>] OnProgressSignal =
            [<Emit "$0($1...)">] abstract Invoke: loader: Loader * resource: Resource -> unit

        type [<AllowNullLiteral>] OnErrorSignal =
            [<Emit "$0($1...)">] abstract Invoke: loader: Loader * resource: Resource -> unit

        type [<AllowNullLiteral>] OnLoadSignal =
            [<Emit "$0($1...)">] abstract Invoke: loader: Loader * resource: Resource -> unit

        type [<AllowNullLiteral>] OnStartSignal =
            [<Emit "$0($1...)">] abstract Invoke: loader: Loader -> unit

        type [<AllowNullLiteral>] OnCompleteSignal =
            [<Emit "$0($1...)">] abstract Invoke: loader: Loader -> unit

        type [<AllowNullLiteral>] Loader =
            inherit Utils.EventEmitter
            abstract baseUrl: string with get, set
            abstract progress: float with get, set
            abstract loading: bool with get, set
            abstract defaultQueryString: string with get, set
            abstract resources: ResourceDictionary with get, set
            abstract onProgress: MiniSignal<OnProgressSignal> with get, set
            abstract onError: MiniSignal<OnErrorSignal> with get, set
            abstract onLoad: MiniSignal<OnLoadSignal> with get, set
            abstract onStart: MiniSignal<OnStartSignal> with get, set
            abstract onComplete: MiniSignal<OnCompleteSignal> with get, set
            abstract add: name: string * url: string * ?callback: OnCompleteSignal -> Loader
            abstract add: name: string * url: string * ?options: LoaderOptions * ?callback: OnCompleteSignal -> Loader
            abstract add: url: string * ?callback: OnCompleteSignal -> Loader
            abstract add: url: string * ?options: LoaderOptions * ?callback: OnCompleteSignal -> Loader
            abstract add: options: LoaderOptions * ?callback: OnCompleteSignal -> Loader
            abstract add: resources: Array<U2<LoaderOptions, string>> * ?callback: OnCompleteSignal -> Loader
            abstract add: [<ParamArray>] ``params``: obj option[] -> Loader
            abstract pre: fn: (ResizeArray<obj option> -> obj option) -> Loader
            abstract ``use``: fn: (ResizeArray<obj option> -> obj option) -> Loader
            abstract reset: unit -> Loader
            abstract load: ?cb: (ResizeArray<obj option> -> obj option) -> Loader
            abstract concurrency: float with get, set
            abstract destroy: unit -> unit
            /// Add a listener for a given event.
            [<Emit "$0.on('complete',$1,$2)">] abstract on_complete: fn: (Loaders.Loader -> obj option -> unit) * ?context: obj -> Loader
            /// Add a listener for a given event.
            [<Emit "$0.on('error',$1,$2)">] abstract on_error: fn: (Error -> Loaders.Loader -> Resource -> unit) * ?context: obj -> Loader
            /// Add a listener for a given event.
            abstract on: ``event``: LoaderOnEvent * fn: (Loaders.Loader -> Resource -> unit) * ?context: obj -> Loader
            /// Add a listener for a given event.
            [<Emit "$0.on('start',$1,$2)">] abstract on_start: fn: (Loaders.Loader -> unit) * ?context: obj -> Loader
            /// Add a one-time listener for a given event.
            [<Emit "$0.once('complete',$1,$2)">] abstract once_complete: fn: (Loaders.Loader -> obj option -> unit) * ?context: obj -> Loader
            /// Add a one-time listener for a given event.
            [<Emit "$0.once('error',$1,$2)">] abstract once_error: fn: (Error -> Loaders.Loader -> Resource -> unit) * ?context: obj -> Loader
            /// Add a one-time listener for a given event.
            abstract once: ``event``: LoaderOnceEvent * fn: (Loaders.Loader -> Resource -> unit) * ?context: obj -> Loader
            /// Add a one-time listener for a given event.
            [<Emit "$0.once('start',$1,$2)">] abstract once_start: fn: (Loaders.Loader -> unit) * ?context: obj -> Loader
            /// <summary>Alias method for <c>removeListener</c></summary>
            abstract off: ``event``: U2<string, string> * ?fn: (ResizeArray<obj option> -> obj option) * ?context: obj -> Loader

        type [<StringEnum>] [<RequireQualifiedAccess>] LoaderOnEvent =
            | Load
            | Progress

        type [<StringEnum>] [<RequireQualifiedAccess>] LoaderOnceEvent =
            | Load
            | Progress

        type [<AllowNullLiteral>] LoaderStatic =
            abstract addPixiMiddleware: fn: (ResizeArray<obj option> -> obj option) -> unit
            [<EmitConstructor>] abstract Create: ?baseUrl: string * ?concurrency: float -> Loader
            abstract Resource: obj with get, set
            abstract async: obj with get, set
            abstract encodeBinary: obj with get, set
            abstract base64: obj with get, set
            abstract pre: fn: (ResizeArray<obj option> -> obj option) -> Loader
            abstract ``use``: fn: (ResizeArray<obj option> -> obj option) -> Loader

        type [<AllowNullLiteral>] TextureDictionary =
            [<EmitIndexer>] abstract Item: index: string -> PIXI.Texture with get, set

        type [<AllowNullLiteral>] IMetadata =
            abstract loadElement: U3<HTMLImageElement, HTMLAudioElement, HTMLVideoElement> option with get, set
            abstract skipSource: bool option with get, set
            abstract mimeType: U2<string, ResizeArray<string>> option with get, set

        type [<AllowNullLiteral>] Resource =
            abstract name: string
            abstract url: string
            abstract extension: string
            abstract data: obj option with get, set
            abstract crossOrigin: string with get, set
            abstract timeout: float with get, set
            abstract loadType: Resource.LOAD_TYPE with get, set
            abstract xhrType: string with get, set
            abstract metadata: IMetadata with get, set
            abstract error: Error
//            abstract xhr: XMLHttpRequest
            abstract children: ResizeArray<Resource>
            abstract ``type``: Resource.LOAD_TYPE
            abstract progressChunk: float
            abstract onStart: MiniSignal<OnStartSignal> with get, set
            abstract onProgress: MiniSignal<OnProgressSignal> with get, set
            abstract onComplete: MiniSignal<OnCompleteSignal> with get, set
            abstract onAfterMiddleware: MiniSignal<OnCompleteSignal> with get, set
            abstract isDataUrl: bool
            abstract isComplete: bool
            abstract isLoading: bool
            abstract complete: unit -> unit
            abstract abort: message: string -> unit
            abstract load: ?cb: OnCompleteSignal -> unit
            abstract texture: Texture with get, set
            abstract sound: obj option with get, set
            abstract spineAtlas: obj option with get, set
            abstract spineData: obj option with get, set
            abstract textures: TextureDictionary option with get, set
            abstract spritesheet: Spritesheet option with get, set

        type [<AllowNullLiteral>] ResourceStatic =
            [<EmitConstructor>] abstract Create: name: string * url: U2<string, ResizeArray<string>> * ?options: LoaderOptions -> Resource

        module Resource =

            type [<AllowNullLiteral>] IExports =
                abstract setExtensionLoadType: extname: string * loadType: Resource.LOAD_TYPE -> unit
                abstract setExtensionXhrType: extname: string * xhrType: Resource.XHR_RESPONSE_TYPE -> unit
                abstract EMPTY_GIF: string

            type STATUS_FLAGS =
                obj

            type TYPE =
                obj

            type LOAD_TYPE =
                obj

            type XHR_RESPONSE_TYPE =
                obj

    module Mesh =

        type [<AllowNullLiteral>] IExports =
            abstract Mesh: MeshStatic
            abstract CanvasMeshRenderer: CanvasMeshRendererStatic
            abstract MeshRenderer: MeshRendererStatic
            abstract Plane: PlaneStatic
            abstract NineSlicePlane: NineSlicePlaneStatic
            abstract Rope: RopeStatic

        type [<AllowNullLiteral>] Mesh =
            inherit Container
            abstract _texture: Texture with get, set
            abstract uvs: Float32Array with get, set
            abstract vertices: Float32Array with get, set
            abstract indices: Uint16Array with get, set
            abstract dirty: float with get, set
            abstract indexDirty: float with get, set
            abstract vertexDirty: float with get, set
            abstract autoUpdate: bool with get, set
            abstract dirtyVertex: bool with get, set
            abstract _geometryVersion: float with get, set
            abstract blendMode: float with get, set
            abstract pluginName: string with get, set
            abstract canvasPadding: float with get, set
            abstract drawMode: float with get, set
            abstract texture: Texture with get, set
            abstract tintRgb: Float32Array with get, set
            abstract _glDatas: Mesh_glDatas with get, set
            abstract _uvTransform: Extras.TextureMatrix with get, set
            abstract uploadUvTransform: bool with get, set
            abstract multiplyUvs: unit -> unit
            abstract refresh: ?forceUpdate: bool -> unit
            abstract _refresh: unit -> unit
            abstract _renderWebGL: renderer: WebGLRenderer -> unit
            abstract _renderCanvas: renderer: CanvasRenderer -> unit
            abstract _onTextureUpdate: unit -> unit
            /// Recalculates the bounds of the object. Override this to
            /// calculate the bounds of the specific object (not including children).
            abstract _calculateBounds: unit -> unit
            abstract containsPoint: point: Point -> bool
            abstract tint: float with get, set

        type [<AllowNullLiteral>] MeshStatic =
            [<EmitConstructor>] abstract Create: texture: Texture * ?vertices: Float32Array * ?uvs: Float32Array * ?indices: Uint16Array * ?drawMode: float -> Mesh
            abstract DRAW_MODES: MeshStaticDRAW_MODES with get, set

        type [<AllowNullLiteral>] CanvasMeshRenderer =
            abstract renderer: CanvasRenderer with get, set
            abstract render: mesh: Mesh -> unit
            abstract _renderTriangleMesh: mesh: Mesh -> unit
            abstract _renderTriangles: mesh: Mesh -> unit
            abstract _renderDrawTriangle: mesh: Mesh * index0: float * index1: float * index2: float -> unit
            abstract renderMeshFlat: mesh: Mesh -> unit
            abstract destroy: unit -> unit

        type [<AllowNullLiteral>] CanvasMeshRendererStatic =
            [<EmitConstructor>] abstract Create: renderer: CanvasRenderer -> CanvasMeshRenderer

        type [<AllowNullLiteral>] MeshRenderer =
            inherit ObjectRenderer
            abstract shader: Shader with get, set
            abstract render: mesh: Mesh -> unit

        type [<AllowNullLiteral>] MeshRendererStatic =
            [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> MeshRenderer

        type [<AllowNullLiteral>] Plane =
            inherit Mesh
            abstract _ready: bool with get, set
            abstract verticesX: float with get, set
            abstract verticesY: float with get, set
            abstract drawMode: float with get, set
            abstract refresh: unit -> unit
            abstract _onTexureUpdate: unit -> unit

        type [<AllowNullLiteral>] PlaneStatic =
            [<EmitConstructor>] abstract Create: texture: Texture * ?verticesX: float * ?verticesY: float -> Plane

        type [<AllowNullLiteral>] NineSlicePlane =
            inherit Plane
            abstract width: float with get, set
            abstract height: float with get, set
            abstract leftWidth: float with get, set
            abstract rightWidth: float with get, set
            abstract topHeight: float with get, set
            abstract bottomHeight: float with get, set
            abstract _leftWidth: float with get, set
            abstract _rightWidth: float with get, set
            abstract _topHeight: float with get, set
            abstract _bottomHeight: float with get, set
            abstract _height: float with get, set
            abstract _width: float with get, set
            abstract _origHeight: float with get, set
            abstract _origWidth: float with get, set
            abstract _uvh: float with get, set
            abstract _uvw: float with get, set
            abstract updateHorizontalVertices: unit -> unit
            abstract updateVerticalVertices: unit -> unit
//            abstract drawSegment: context: U2<CanvasRenderingContext2D, WebGLRenderingContext> * textureSource: obj option * w: float * h: float * x1: float * y1: float * x2: float * y2: float -> unit
            abstract _renderCanvas: renderer: CanvasRenderer -> unit
            abstract _refresh: unit -> unit

        type [<AllowNullLiteral>] NineSlicePlaneStatic =
            [<EmitConstructor>] abstract Create: texture: Texture * ?leftWidth: float * ?topHeight: float * ?rightWidth: float * ?bottomHeight: float -> NineSlicePlane

        type [<AllowNullLiteral>] Rope =
            inherit Mesh
            abstract points: ResizeArray<Point> with get, set
            abstract colors: ResizeArray<float> with get, set
            abstract autoUpdate: bool with get, set
            abstract _refresh: unit -> unit
            abstract refreshVertices: unit -> unit

        type [<AllowNullLiteral>] RopeStatic =
            [<EmitConstructor>] abstract Create: texture: Texture * points: ResizeArray<Point> -> Rope

        type [<AllowNullLiteral>] Mesh_glDatas =
            [<EmitIndexer>] abstract Item: n: float -> obj option with get, set

        type [<AllowNullLiteral>] MeshStaticDRAW_MODES =
            abstract TRIANGLE_MESH: float with get, set
            abstract TRIANGLES: float with get, set

    module Particles =

        type [<AllowNullLiteral>] IExports =
            abstract ParticleContainer: ParticleContainerStatic
            abstract ParticleBuffer: ParticleBufferStatic
            abstract ParticleRenderer: ParticleRendererStatic

        type [<AllowNullLiteral>] ParticleContainerProperties =
            /// <summary>DEPRECIATED - Use <c>vertices</c></summary>
            abstract scale: bool option with get, set
            abstract vertices: bool option with get, set
            abstract position: bool option with get, set
            abstract rotation: bool option with get, set
            abstract uvs: bool option with get, set
            abstract tint: bool option with get, set
            abstract alpha: bool option with get, set

        type [<AllowNullLiteral>] ParticleContainer =
            inherit Container
            abstract _tint: float with get, set
            abstract tintRgb: U2<float, ResizeArray<obj option>> with get, set
            abstract tint: float with get, set
            abstract _properties: ResizeArray<bool> with get, set
            abstract _maxSize: float with get, set
            abstract _batchSize: float with get, set
            abstract _glBuffers: ParticleContainer_glBuffers with get, set
            abstract _bufferUpdateIDs: ResizeArray<float> with get, set
            abstract _updateID: float with get, set
            /// <summary>
            /// Determines if the children to the displayObject can be clicked/touched
            /// Setting this to false allows PixiJS to bypass a recursive <c>hitTest</c> function
            /// </summary>
            abstract interactiveChildren: bool with get, set
            abstract blendMode: float with get, set
            abstract autoSize: bool with get, set
            abstract roundPixels: bool with get, set
            abstract baseTexture: BaseTexture with get, set
            abstract setProperties: properties: ParticleContainerProperties -> unit
            abstract onChildrenChange: (float -> unit) with get, set
            /// <summary>
            /// Removes all internal references and listeners as well as removes children from the display list.
            /// Do not use a Container after calling <c>destroy</c>.
            /// </summary>
            abstract destroy: ?options: U2<DestroyOptions, bool> -> unit

        type [<AllowNullLiteral>] ParticleContainerStatic =
            [<EmitConstructor>] abstract Create: ?maxSize: float * ?properties: ParticleContainerProperties * ?batchSize: float * ?autoResize: bool -> ParticleContainer

        type [<AllowNullLiteral>] ParticleBuffer =
//            abstract gl: WebGLRenderingContext with get, set
            abstract size: float with get, set
            abstract dynamicProperties: ResizeArray<obj option> with get, set
            abstract staticProperties: ResizeArray<obj option> with get, set
            abstract staticStride: float with get, set
            abstract staticBuffer: obj option with get, set
            abstract staticData: obj option with get, set
            abstract staticDataUint32: obj option with get, set
            abstract dynamicStride: float with get, set
            abstract dynamicBuffer: obj option with get, set
            abstract dynamicData: obj option with get, set
            abstract dynamicDataUint32: obj option with get, set
            abstract _updateID: float with get, set
            abstract destroy: unit -> unit

        type [<AllowNullLiteral>] ParticleBufferStatic =
            interface end
//            [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * properties: obj option * dynamicPropertyFlags: ResizeArray<obj option> * size: float -> ParticleBuffer

        type [<AllowNullLiteral>] ParticleRendererProperty =
            abstract attribute: float with get, set
            abstract size: float with get, set
            abstract uploadFunction: children: ResizeArray<PIXI.DisplayObject> * startIndex: float * amount: float * array: ResizeArray<float> * stride: float * offset: float -> unit
            abstract unsignedByte: obj option with get, set
            abstract offset: float with get, set

        type [<AllowNullLiteral>] ParticleRenderer =
            inherit ObjectRenderer
            abstract shader: GlCore.GLShader with get, set
//            abstract indexBuffer: WebGLBuffer with get, set
            abstract properties: ResizeArray<ParticleRendererProperty> with get, set
            abstract tempMatrix: Matrix with get, set
            /// Starts the renderer and sets the shader
            abstract start: unit -> unit
            abstract generateBuffers: container: ParticleContainer -> ResizeArray<ParticleBuffer>
            abstract _generateOneMoreBuffer: container: ParticleContainer -> ParticleBuffer
            abstract uploadVertices: children: ResizeArray<DisplayObject> * startIndex: float * amount: float * array: ResizeArray<float> * stride: float * offset: float -> unit
            abstract uploadPosition: children: ResizeArray<DisplayObject> * startIndex: float * amount: float * array: ResizeArray<float> * stride: float * offset: float -> unit
            abstract uploadRotation: children: ResizeArray<DisplayObject> * startIndex: float * amount: float * array: ResizeArray<float> * stride: float * offset: float -> unit
            abstract uploadUvs: children: ResizeArray<DisplayObject> * startIndex: float * amount: float * array: ResizeArray<float> * stride: float * offset: float -> unit
            abstract uploadTint: children: ResizeArray<DisplayObject> * startIndex: float * amount: float * array: ResizeArray<float> * stride: float * offset: float -> unit
            abstract uploadAlpha: children: ResizeArray<DisplayObject> * startIndex: float * amount: float * array: ResizeArray<float> * stride: float * offset: float -> unit
            abstract destroy: unit -> unit
            abstract indices: Uint16Array with get, set

        type [<AllowNullLiteral>] ParticleRendererStatic =
            [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> ParticleRenderer

        type [<AllowNullLiteral>] ParticleContainer_glBuffers =
            interface end
//            [<EmitIndexer>] abstract Item: n: float -> WebGLBuffer with get, set

    module Prepare =

        type [<AllowNullLiteral>] IExports =
            abstract BasePrepare: BasePrepareStatic
            abstract CanvasPrepare: CanvasPrepareStatic
            abstract WebGLPrepare: WebGLPrepareStatic
            abstract CountLimiter: CountLimiterStatic
            abstract TimeLimiter: TimeLimiterStatic

        type [<AllowNullLiteral>] AddHook =
            [<Emit "$0($1...)">] abstract Invoke: item: obj option * queue: ResizeArray<obj option> -> bool

        type [<AllowNullLiteral>] UploadHook<'UploadHookSource> =
            [<Emit "$0($1...)">] abstract Invoke: prepare: 'UploadHookSource * item: obj option -> bool

        type [<AllowNullLiteral>] BasePrepare<'UploadHookSource> =
            abstract limiter: U2<CountLimiter, TimeLimiter> with get, set
            abstract renderer: SystemRenderer with get, set
            abstract uploadHookHelper: 'UploadHookSource with get, set
            abstract queue: ResizeArray<obj option> with get, set
            abstract addHooks: ResizeArray<AddHook> with get, set
            abstract uploadHooks: Array<UploadHook<'UploadHookSource>> with get, set
            abstract completes: (ResizeArray<obj option> -> ResizeArray<obj option>) with get, set
            abstract ticking: bool with get, set
            abstract delayedTick: (unit -> unit) with get, set
            abstract upload: item: U8<Function, DisplayObject, Container, BaseTexture, Texture, Graphics, Text, obj option> * ?``done``: (unit -> unit) -> unit
            abstract tick: unit -> unit
            abstract prepareItems: unit -> unit
            abstract registerFindHook: addHook: AddHook -> BasePrepare<'UploadHookSource>
            abstract registerUploadHook: uploadHook: UploadHook<'UploadHookSource> -> BasePrepare<'UploadHookSource>
            abstract findMultipleBaseTextures: item: PIXI.DisplayObject * queue: ResizeArray<obj option> -> bool
            abstract findBaseTexture: item: PIXI.DisplayObject * queue: ResizeArray<obj option> -> bool
            abstract findTexture: item: PIXI.DisplayObject * queue: ResizeArray<obj option> -> bool
            abstract add: item: U7<PIXI.DisplayObject, PIXI.Container, PIXI.BaseTexture, PIXI.Texture, PIXI.Graphics, PIXI.Text, obj option> -> BasePrepare<'UploadHookSource>
            abstract destroy: unit -> unit

        type [<AllowNullLiteral>] BasePrepareStatic =
            [<EmitConstructor>] abstract Create: renderer: SystemRenderer -> BasePrepare<'UploadHookSource>

        type [<AllowNullLiteral>] CanvasPrepare =
            inherit BasePrepare<CanvasPrepare>
            abstract canvas: HTMLCanvasElement with get, set
            abstract ctx: CanvasRenderingContext2D with get, set

        type [<AllowNullLiteral>] CanvasPrepareStatic =
            [<EmitConstructor>] abstract Create: renderer: CanvasRenderer -> CanvasPrepare

        type [<AllowNullLiteral>] WebGLPrepare =
            inherit BasePrepare<WebGLRenderer>

        type [<AllowNullLiteral>] WebGLPrepareStatic =
            [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> WebGLPrepare

        type [<AllowNullLiteral>] CountLimiter =
            abstract maxItemsPerFrame: float with get, set
            abstract itemsLeft: float with get, set
            abstract beginFrame: unit -> unit
            abstract allowedToUpload: unit -> bool

        type [<AllowNullLiteral>] CountLimiterStatic =
            [<EmitConstructor>] abstract Create: maxItemsPerFrame: float -> CountLimiter

        type [<AllowNullLiteral>] TimeLimiter =
            abstract maxMilliseconds: float with get, set
            abstract frameStart: float with get, set
            abstract beginFrame: unit -> unit
            abstract allowedToUpload: unit -> bool

        type [<AllowNullLiteral>] TimeLimiterStatic =
            [<EmitConstructor>] abstract Create: maxMilliseconds: float -> TimeLimiter

    module GlCore =

        type [<AllowNullLiteral>] IExports =
//            abstract createContext: view: HTMLCanvasElement * ?options: ContextOptions -> WebGLRenderingContext
//            abstract setVertexAttribArrays: gl: WebGLRenderingContext * attribs: ResizeArray<Attrib> * ?state: WebGLState -> WebGLRenderingContext option
            abstract GLBuffer: GLBufferStatic
            abstract GLFramebuffer: GLFramebufferStatic
            abstract GLShader: GLShaderStatic
            abstract GLTexture: GLTextureStatic
            abstract VertexArrayObject: VertexArrayObjectStatic

        type [<AllowNullLiteral>] ContextOptions =
            /// Boolean that indicates if the canvas contains an alpha buffer.
            abstract alpha: bool option with get, set
            /// Boolean that indicates that the drawing buffer has a depth buffer of at least 16 bits.
            abstract depth: bool option with get, set
            /// Boolean that indicates that the drawing buffer has a stencil buffer of at least 8 bits.
            abstract stencil: bool option with get, set
            /// Boolean that indicates whether or not to perform anti-aliasing.
            abstract antialias: bool option with get, set
            /// Boolean that indicates that the page compositor will assume the drawing buffer contains colors with pre-multiplied alpha.
            abstract premultipliedAlpha: bool option with get, set
            /// If the value is true the buffers will not be cleared and will preserve their values until cleared or overwritten by the author.
            abstract preserveDrawingBuffer: bool option with get, set
            /// Boolean that indicates if a context will be created if the system performance is low.
            abstract failIfMajorPerformanceCaveat: bool option with get, set

        type [<AllowNullLiteral>] GLBuffer =
            abstract _updateID: float option with get, set
//            abstract gl: WebGLRenderingContext with get, set
//            abstract buffer: WebGLBuffer with get, set
            abstract ``type``: float with get, set
            abstract drawType: float with get, set
            abstract data: U3<ArrayBuffer, ArrayBufferView, obj option> with get, set
            abstract upload: ?data: U3<ArrayBuffer, ArrayBufferView, obj option> * ?offset: float * ?dontBind: bool -> unit
            abstract bind: unit -> unit
            abstract destroy: unit -> unit

        type [<AllowNullLiteral>] GLBufferStatic =
            interface end
//            [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * ``type``: float * data: U3<ArrayBuffer, ArrayBufferView, obj option> * drawType: float -> GLBuffer
//            abstract createVertexBuffer: gl: WebGLRenderingContext * data: U3<ArrayBuffer, ArrayBufferView, obj option> * drawType: float -> GLBuffer
//            abstract createIndexBuffer: gl: WebGLRenderingContext * data: U3<ArrayBuffer, ArrayBufferView, obj option> * drawType: float -> GLBuffer
//            abstract create: gl: WebGLRenderingContext * ``type``: float * data: U3<ArrayBuffer, ArrayBufferView, obj option> * drawType: float -> GLBuffer

        type [<AllowNullLiteral>] GLFramebuffer =
//            abstract gl: WebGLRenderingContext with get, set
//            abstract frameBuffer: WebGLFramebuffer with get, set
//            abstract stencil: WebGLRenderbuffer with get, set
            abstract texture: GLTexture with get, set
            abstract width: float with get, set
            abstract height: float with get, set
            abstract enableTexture: texture: GLTexture -> unit
            abstract enableStencil: unit -> unit
            abstract clear: r: float * g: float * b: float * a: float -> unit
            abstract bind: unit -> unit
            abstract unbind: unit -> unit
            abstract resize: width: float * height: float -> unit
            abstract destroy: unit -> unit

        type [<AllowNullLiteral>] GLFramebufferStatic =
            interface end
//            [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * width: float * height: float -> GLFramebuffer
//            abstract createRGBA: gl: WebGLRenderingContext * width: float * height: float * data: U3<ArrayBuffer, ArrayBufferView, obj option> -> GLFramebuffer
//            abstract createFloat32: gl: WebGLRenderingContext * width: float * height: float * data: U3<ArrayBuffer, ArrayBufferView, obj option> -> GLFramebuffer

        type [<AllowNullLiteral>] GLShader =
//            abstract gl: WebGLRenderingContext with get, set
//            abstract program: WebGLProgram option with get, set
            abstract uniformData: obj option with get, set
            abstract uniforms: obj option with get, set
            abstract attributes: obj option with get, set
            abstract bind: unit -> GLShader
            abstract destroy: unit -> unit

        type [<AllowNullLiteral>] GLShaderStatic =
            interface end
//            [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * vertexSrc: U2<string, ResizeArray<string>> * fragmentSrc: U2<string, ResizeArray<string>> * ?precision: string * ?attributeLocations: GLShaderStaticAttributeLocations -> GLShader

        type [<AllowNullLiteral>] GLShaderStaticAttributeLocations =
            [<EmitIndexer>] abstract Item: key: string -> float with get, set

        type [<AllowNullLiteral>] GLTexture =
//            abstract gl: WebGLRenderingContext with get, set
//            abstract texture: WebGLTexture with get, set
            abstract mipmap: bool with get, set
            abstract premultiplyAlpha: bool with get, set
            abstract width: float with get, set
            abstract height: float with get, set
            abstract format: float with get, set
            abstract ``type``: float with get, set
            abstract upload: source: U4<HTMLImageElement, ImageData, HTMLVideoElement, HTMLCanvasElement> -> unit
            abstract uploadData: data: U2<ArrayBuffer, ArrayBufferView> * width: float * height: float -> unit
            abstract bind: ?location: float -> unit
            abstract unbind: unit -> unit
            abstract minFilter: linear: bool -> unit
            abstract magFilter: linear: bool -> unit
            abstract enableMipmap: unit -> unit
            abstract enableLinearScaling: unit -> unit
            abstract enableNearestScaling: unit -> unit
            abstract enableWrapClamp: unit -> unit
            abstract enableWrapRepeat: unit -> unit
            abstract enableWrapMirrorRepeat: unit -> unit
            abstract destroy: unit -> unit

        type [<AllowNullLiteral>] GLTextureStatic =
            interface end
//            [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * ?width: float * ?height: float * ?format: float * ?``type``: float -> GLTexture
//            abstract fromSource: gl: WebGLRenderingContext * source: U4<HTMLImageElement, ImageData, HTMLVideoElement, HTMLCanvasElement> * ?premultipleAlpha: bool -> GLTexture
//            abstract fromData: gl: WebGLRenderingContext * data: ResizeArray<float> * width: float * height: float -> GLTexture

        type [<AllowNullLiteral>] Attrib =
            abstract attribute: AttribAttribute with get, set
            abstract normalized: bool with get, set
            abstract stride: float with get, set
            abstract start: float with get, set
            abstract buffer: ArrayBuffer with get, set

        type [<AllowNullLiteral>] WebGLRenderingContextAttribute =
//            abstract buffer: WebGLBuffer with get, set
            abstract attribute: obj option with get, set
            abstract ``type``: float with get, set
            abstract normalized: bool with get, set
            abstract stride: float with get, set
            abstract start: float with get, set

        type [<AllowNullLiteral>] AttribState =
            abstract tempAttribState: ResizeArray<Attrib> with get, set
            abstract attribState: ResizeArray<Attrib> with get, set

        type [<AllowNullLiteral>] VertexArrayObject =
            abstract nativeVaoExtension: obj option with get, set
            abstract nativeState: AttribState with get, set
            abstract nativeVao: VertexArrayObject with get, set
//            abstract gl: WebGLRenderingContext with get, set
            abstract attributes: ResizeArray<Attrib> with get, set
            abstract indexBuffer: GLBuffer with get, set
            abstract dirty: bool with get, set
            abstract bind: unit -> VertexArrayObject
            abstract unbind: unit -> VertexArrayObject
            abstract activate: unit -> VertexArrayObject
            abstract addAttribute: buffer: GLBuffer * attribute: Attrib * ?``type``: float * ?normalized: bool * ?stride: float * ?start: float -> VertexArrayObject
            abstract addIndex: buffer: GLBuffer * ?options: obj -> VertexArrayObject
            abstract clear: unit -> VertexArrayObject
            abstract draw: ``type``: float * size: float * start: float -> VertexArrayObject
            abstract destroy: unit -> unit

        type [<AllowNullLiteral>] VertexArrayObjectStatic =
            abstract FORCE_NATIVE: bool with get, set
//            [<EmitConstructor>] abstract Create: gl: WebGLRenderingContext * ?state: WebGLState -> VertexArrayObject

        type [<AllowNullLiteral>] AttribAttribute =
            abstract location: float with get, set
            abstract size: float with get, set

    type [<AllowNullLiteral>] DecomposedDataUri =
        abstract mediaType: string with get, set
        abstract subType: string with get, set
        abstract encoding: string with get, set
        abstract data: obj option with get, set

    module Utils =
        let [<Import("isMobile","module/PIXI/utils")>] isMobile: IsMobile.IExports = jsNative

        type [<AllowNullLiteral>] IExports =
            abstract uid: unit -> float
            abstract hex2rgb: hex: float * ?out: ResizeArray<float> -> ResizeArray<float>
            abstract hex2string: hex: float -> string
            abstract rgb2hex: rgb: ResizeArray<float> -> float
            abstract canUseNewCanvasBlendModes: unit -> bool
            abstract getResolutionOfUrl: url: string * ?defaultValue: float -> float
            abstract getSvgSize: svgString: string -> obj option
            abstract decomposeDataUri: dataUri: string -> U2<DecomposedDataUri, unit>
            abstract getUrlFileExtension: url: string -> U2<string, unit>
            abstract sayHello: ``type``: string -> unit
            abstract skipHello: unit -> unit
            abstract isWebGLSupported: unit -> bool
            abstract sign: n: float -> float
            /// <summary>Remove a range of items from an array</summary>
            /// <param name="arr">The target array</param>
            /// <param name="startIdx">The index to begin removing from (inclusive)</param>
            /// <param name="removeCount">How many items to remove</param>
            abstract removeItems: arr: ResizeArray<'T> * startIdx: float * removeCount: float -> unit
            abstract correctBlendMode: blendMode: float * premultiplied: bool -> float
            abstract clearTextureCache: unit -> unit
            abstract destroyTextureCache: unit -> unit
            abstract premultiplyTint: tint: float * alpha: float -> float
            abstract premultiplyRgba: rgb: U2<Float32Array, ResizeArray<float>> * alpha: float * ?out: Float32Array * ?premultiply: bool -> Float32Array
            abstract premultiplyTintToRgba: tint: float * alpha: float * ?out: Float32Array * ?premultiply: bool -> Float32Array
            abstract premultiplyBlendMode: ResizeArray<ResizeArray<float>>
            abstract TextureCache: obj option
            abstract BaseTextureCache: obj option
            abstract EventEmitter: EventEmitterStatic

        module IsMobile =

            type [<AllowNullLiteral>] IExports =
                abstract apple: IExportsApple
                abstract android: IExportsAndroid
                abstract amazon: IExportsAndroid
                abstract windows: IExportsAndroid
                abstract seven_inch: bool
                abstract other: IExportsOther
                abstract any: bool
                abstract phone: bool
                abstract tablet: bool

            type [<AllowNullLiteral>] IExportsApple =
                abstract phone: bool with get, set
                abstract ipod: bool with get, set
                abstract tablet: bool with get, set
                abstract device: bool with get, set

            type [<AllowNullLiteral>] IExportsAndroid =
                abstract phone: bool with get, set
                abstract tablet: bool with get, set
                abstract device: bool with get, set

            type [<AllowNullLiteral>] IExportsOther =
                abstract blackberry10: bool with get, set
                abstract blackberry: bool with get, set
                abstract opera: bool with get, set
                abstract firefox: bool with get, set
                abstract chrome: bool with get, set
                abstract device: bool with get, set

        type [<AllowNullLiteral>] EventEmitter =
            /// <summary>Return an array listing the events for which the emitter has registered listeners.</summary>
            /// <returns />
            abstract eventNames: unit -> Array<U2<string, Symbol>>
            /// <summary>Return the listeners registered for a given event.</summary>
            /// <param name="event">The event name.</param>
            /// <returns />
            abstract listeners: ``event``: U2<string, Symbol> -> ResizeArray<Function>
            /// <summary>
            /// Check if there listeners for a given event.
            /// If <c>exists</c> argument is not <c>true</c> lists listeners.
            /// </summary>
            /// <param name="event">The event name.</param>
            /// <param name="exists">Only check if there are listeners.</param>
            /// <returns />
            abstract listeners: ``event``: U2<string, Symbol> * exists: bool -> bool
            /// <summary>Calls each of the listeners registered for a given event.</summary>
            /// <param name="event">The event name.</param>
            /// <param name="args">Arguments that are passed to registered listeners</param>
            /// <returns><c>true</c> if the event had listeners, else <c>false</c>.</returns>
            abstract emit: ``event``: U2<string, Symbol> * [<ParamArray>] args: obj option[] -> bool
            /// <summary>Add a listener for a given event.</summary>
            /// <param name="event">The event name.</param>
            /// <param name="fn">The listener function.</param>
            /// <param name="context">The context to invoke the listener with.</param>
            /// <returns><c>this</c></returns>
            abstract on: ``event``: U2<string, Symbol> * fn: (ResizeArray<obj option> -> obj option) * ?context: obj -> EventEmitter
            /// <summary>Add a one-time listener for a given event.</summary>
            /// <param name="event">The event name.</param>
            /// <param name="fn">The listener function.</param>
            /// <param name="context">The context to invoke the listener with.</param>
            /// <returns><c>this</c></returns>
            abstract once: ``event``: U2<string, Symbol> * fn: (ResizeArray<obj option> -> obj option) * ?context: obj -> EventEmitter
            /// <summary>Remove the listeners of a given event.</summary>
            /// <param name="event">The event name.</param>
            /// <param name="fn">Only remove the listeners that match this function.</param>
            /// <param name="context">Only remove the listeners that have this context.</param>
            /// <param name="once">Only remove one-time listeners.</param>
            /// <returns><c>this</c></returns>
            abstract removeListener: ``event``: U2<string, Symbol> * ?fn: (ResizeArray<obj option> -> obj option) * ?context: obj * ?once: bool -> EventEmitter
            /// <summary>Remove all listeners, or those of the specified event.</summary>
            /// <param name="event">The event name.</param>
            /// <returns><c>this</c>.</returns>
            abstract removeAllListeners: ?``event``: U2<string, Symbol> -> EventEmitter
            /// <summary>Alias method for <c>removeListener</c></summary>
            abstract off: ``event``: U2<string, Symbol> * ?fn: (ResizeArray<obj option> -> obj option) * ?context: obj * ?once: bool -> EventEmitter
            /// <summary>Alias method for <c>on</c></summary>
            abstract addListener: ``event``: U2<string, Symbol> * fn: (ResizeArray<obj option> -> obj option) * ?context: obj -> EventEmitter
            /// This function doesn"t apply anymore.
            [<Obsolete("")>]
            abstract setMaxListeners: unit -> EventEmitter

        type [<AllowNullLiteral>] EventEmitterStatic =
            abstract prefixed: U2<string, bool> with get, set
            abstract EventEmitter: EventEmitterStaticEventEmitter with get, set
            /// <summary>
            /// Minimal EventEmitter interface that is molded against the Node.js
            /// EventEmitter interface.
            /// </summary>
            [<EmitConstructor>] abstract Create: unit -> EventEmitter

        type [<AllowNullLiteral>] EventEmitterStaticEventEmitter =
            [<EmitConstructor>] abstract Create: unit -> EventEmitter
            abstract prefixed: U2<string, bool> with get, set

    module Core =

        /// <seealso cref="PIXI.ParticleContainer" />
        /// <exception cref="ReferenceError"> SpriteBatch does not exist any more, please use the new ParticleContainer instead.</exception>
        [<Obsolete("since version 3.0.0")>]
        type SpriteBatch =
            ParticleContainer

        /// <seealso cref="PIXI.loaders.Loader" />
        /// <exception cref="ReferenceError"> The loader system was overhauled in pixi v3, please see the new PIXI.loaders.Loader class.</exception>
        [<Obsolete("since version 3.0.0")>]
        type AssetLoader =
            Loaders.Loader

        /// <seealso cref="PIXI.Container" />
        [<Obsolete("since version 3.0.0")>]
        type Stage =
            Container

        /// <seealso cref="PIXI.Container" />
        [<Obsolete("since version 3.0.0")>]
        type DisplayObjectContainer =
            Container

        /// <seealso cref="PIXI.mesh.Mesh" />
        [<Obsolete("since version 3.0.0")>]
        type Strip =
            Mesh.Mesh

        /// <seealso cref="PIXI.mesh.Rope" />
        [<Obsolete("since version 3.0.0")>]
        type Rope =
            Mesh.Rope

        /// <seealso cref="PIXI.particles.ParticleContainer" />
        [<Obsolete("since version 4.0.0")>]
        type ParticleContainer =
            Particles.ParticleContainer

        /// <seealso cref="PIXI.extras.MovieClip" />
        [<Obsolete("since version 3.0.0")>]
        type MovieClip =
            Extras.AnimatedSprite

        /// <seealso cref="PIXI.extras.TilingSprite" />
        [<Obsolete("since version 3.0.0")>]
        type TilingSprite =
            Extras.TilingSprite

        /// <seealso cref="PIXI.utils.BaseTextureCache" />
        [<Obsolete("since version 3.0.0")>]
        type BaseTextureCache =
            obj option

        /// <seealso cref="PIXI.extras.BitmapText" />
        [<Obsolete("since version 3.0.0")>]
        type BitmapText =
            Extras.BitmapText

        /// <seealso cref="PIXI" />
        [<Obsolete("since version 3.0.6")>]
        type math =
            obj option

        /// <seealso cref="PIXI.Filter" />
        [<Obsolete("since version 3.0.6")>]
        type AbstractFilter<'U when 'U :> Object> =
            Filter<'U>

        /// <seealso cref="PIXI.TransformBase" />
        [<Obsolete("since version 4.0.0")>]
        type TransformManual =
            TransformBase

        /// <seealso cref="PIXI.settings.TARGET_FPMS" />
        [<Obsolete("since version 4.2.0")>]
        type TARGET_FPMS =
            float

        /// <seealso cref="PIXI.settings.FILTER_RESOLUTION" />
        [<Obsolete("since version 4.2.0")>]
        type FILTER_RESOLUTION =
            float

        /// <seealso cref="PIXI.settings.RESOLUTION" />
        [<Obsolete("since version 4.2.0")>]
        type RESOLUTION =
            float

        /// <seealso cref="PIXI.settings.MIPMAP_TEXTURES" />
        [<Obsolete("since version 4.2.0")>]
        type MIPMAP_TEXTURES =
            obj option

        /// <seealso cref="PIXI.settings.SPRITE_BATCH_SIZE" />
        [<Obsolete("since version 4.2.0")>]
        type SPRITE_BATCH_SIZE =
            float

        /// <seealso cref="PIXI.settings.SPRITE_MAX_TEXTURES" />
        [<Obsolete("since version 4.2.0")>]
        type SPRITE_MAX_TEXTURES =
            float

        /// <seealso cref="PIXI.settings.RETINA_PREFIX" />
        [<Obsolete("since version 4.2.0")>]
        type RETINA_PREFIX =
            U2<RegExp, string>

        /// <seealso cref="PIXI.settings.RENDER_OPTIONS" />
        [<Obsolete("since version 4.2.0")>]
        type DEFAULT_RENDER_OPTIONS =
            float

        /// <seealso cref="PIXI.PRECISION" />
        [<Obsolete("since version 4.4.0")>]
        type PRECISION =
            string

    type [<AllowNullLiteral>] DisplayObjectTrackedPointers =
        [<EmitIndexer>] abstract Item: key: float -> Interaction.InteractionTrackingData with get, set

    type [<AllowNullLiteral>] GraphicsStaticCURVES =
        abstract adaptive: bool with get, set
        abstract maxLength: float with get, set
        abstract minSegments: float with get, set
        abstract maxSegments: float with get, set

    type [<AllowNullLiteral>] CanvasRendererStatic__pluginsItem =
        [<EmitConstructor>] abstract Create: renderer: CanvasRenderer -> CanvasRenderer

    type [<AllowNullLiteral>] CanvasRendererStatic__plugins =
        [<EmitIndexer>] abstract Item: pluginName: string -> CanvasRendererStatic__pluginsItem with get, set

    type [<AllowNullLiteral>] WebGLRenderer_contextOptions =
        abstract alpha: bool with get, set
        abstract antiAlias: bool option with get, set
        abstract premultipliedAlpha: bool with get, set
        abstract stencil: bool with get, set
        abstract preseveDrawingBuffer: bool option with get, set

    type [<AllowNullLiteral>] WebGLRendererStatic__pluginsItem =
        [<EmitConstructor>] abstract Create: renderer: WebGLRenderer -> WebGLRenderer

    type [<AllowNullLiteral>] WebGLRendererStatic__plugins =
        [<EmitIndexer>] abstract Item: pluginName: string -> WebGLRendererStatic__pluginsItem with get, set

    type [<AllowNullLiteral>] RenderTargetFilterData =
        abstract index: float with get, set
        abstract stack: ResizeArray<FilterDataStackItem> with get, set

    type [<AllowNullLiteral>] BaseRenderTexture_glRenderTargets =
        interface end
//        [<EmitIndexer>] abstract Item: n: float -> WebGLTexture with get, set

    type [<AllowNullLiteral>] SpritesheetAnimations =
        [<EmitIndexer>] abstract Item: key: string -> ResizeArray<Texture> with get, set

    type [<AllowNullLiteral>] SpritesheetTextures =
        [<EmitIndexer>] abstract Item: key: string -> Texture with get, set

module Pixi =

    type [<AllowNullLiteral>] IExports =
        abstract gl: obj