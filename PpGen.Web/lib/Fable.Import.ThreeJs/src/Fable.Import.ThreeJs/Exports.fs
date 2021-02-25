module Fable.Import.ThreeJs.Exports

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.ThreeJs

module Three =
    type IExports =
        //
        // SRC
        //
        inherit Types.Polyfills.IExports
        inherit Types.Constants.IExports
//        inherit Types.__Three.Legacy.IExports
        //
        // Animation
        //
        inherit Types.__animation_tracks_VectorKeyframeTrack.IExports
        inherit Types.__animation_tracks_StringKeyframeTrack.IExports
        inherit Types.__animation_tracks_QuaternionKeyframeTrack.IExports
        inherit Types.__animation_tracks_NumberKeyframeTrack.IExports
        inherit Types.__animation_tracks_ColorKeyframeTrack.IExports
        inherit Types.__animation_tracks_BooleanKeyframeTrack.IExports
        inherit Types.__animation_PropertyMixer.IExports
        inherit Types.__animation_PropertyBinding.IExports
        inherit Types.__animation_KeyframeTrack.IExports
//        inherit Types.__animation_AnimationUtils.IExports
        inherit Types.__animation_AnimationObjectGroup.IExports
        inherit Types.__animation_AnimationMixer.IExports
        inherit Types.__animation_AnimationClip.IExports
        inherit Types.__animation_AnimationAction.IExports
        //
        // Audio
        //
        inherit Types.__audio_AudioListener.IExports
        inherit Types.__audio_PositionalAudio.IExports
//        inherit Types.__audio_AudioContext.IExports
        inherit Types.__audio_AudioAnalyser.IExports
        inherit Types.__audio_Audio.IExports
        //
        // Cameras
        //
        inherit Types.__cameras_StereoCamera.IExports
        inherit Types.__cameras_PerspectiveCamera.IExports
        inherit Types.__cameras_OrthographicCamera.IExports
        inherit Types.__cameras_CubeCamera.IExports
        inherit Types.__cameras_ArrayCamera.IExports
        inherit Types.__cameras_Camera.IExports
        //
        // Core
        //
        inherit Types.__core_Uniform.IExports
        inherit Types.__core_InstancedBufferGeometry.IExports
        inherit Types.__core_BufferGeometry.IExports
        inherit Types.__core_InterleavedBufferAttribute.IExports
        inherit Types.__core_InstancedInterleavedBuffer.IExports
        inherit Types.__core_InterleavedBuffer.IExports
        inherit Types.__core_InstancedBufferAttribute.IExports
        inherit Types.__core_GLBufferAttribute.IExports
        inherit Types.__core_BufferAttribute.IExports
        inherit Types.__core_Object3D.IExports
        inherit Types.__core_Raycaster.IExports
        inherit Types.__core_Layers.IExports
        inherit Types.__core_EventDispatcher.IExports
        inherit Types.__core_Clock.IExports
        //
        // Extras
        //
        inherit Types.__extras_objects_ImmediateRenderObject.IExports
//        inherit Types.__extras_curves_Curves.IExports
        inherit Types.__extras_core_Shape.IExports
        inherit Types.__extras_core_Path.IExports
        inherit Types.__extras_core_ShapePath.IExports
        inherit Types.__extras_core_Font.IExports
        inherit Types.__extras_core_CurvePath.IExports
        inherit Types.__extras_core_Curve.IExports
//        inherit Types.__extras_DataUtils.IExports
//        inherit Types.__extras_ImageUtils.IExports
//        inherit Types.__extras_ShapeUtils.IExports
        inherit Types.__extras_PMREMGenerator.IExports
        //
        // Geometries
        //
        inherit Types.__geometries_Geometries.IExports
        //
        // Helpers
        //
        inherit Types.__helpers_SpotLightHelper.IExports
        inherit Types.__helpers_SkeletonHelper.IExports
        inherit Types.__helpers_PointLightHelper.IExports
        inherit Types.__helpers_HemisphereLightHelper.IExports
        inherit Types.__helpers_GridHelper.IExports
        inherit Types.__helpers_PolarGridHelper.IExports
        inherit Types.__helpers_DirectionalLightHelper.IExports
        inherit Types.__helpers_CameraHelper.IExports
        inherit Types.__helpers_BoxHelper.IExports
        inherit Types.__helpers_Box3Helper.IExports
        inherit Types.__helpers_PlaneHelper.IExports
        inherit Types.__helpers_ArrowHelper.IExports
        inherit Types.__helpers_AxesHelper.IExports
        //
        // Lights
        //
        inherit Types.__lights_SpotLightShadow.IExports
        inherit Types.__lights_SpotLight.IExports
        inherit Types.__lights_PointLight.IExports
        inherit Types.__lights_PointLightShadow.IExports
        inherit Types.__lights_RectAreaLight.IExports
        inherit Types.__lights_HemisphereLight.IExports
        inherit Types.__lights_DirectionalLightShadow.IExports
        inherit Types.__lights_DirectionalLight.IExports
        inherit Types.__lights_AmbientLight.IExports
        inherit Types.__lights_LightShadow.IExports
        inherit Types.__lights_Light.IExports
        inherit Types.__lights_AmbientLightProbe.IExports
        inherit Types.__lights_HemisphereLightProbe.IExports
        inherit Types.__lights_LightProbe.IExports
        //
        // Loaders
        //
        inherit Types.__loaders_AnimationLoader.IExports
        inherit Types.__loaders_CompressedTextureLoader.IExports
        inherit Types.__loaders_DataTextureLoader.IExports
        inherit Types.__loaders_CubeTextureLoader.IExports
        inherit Types.__loaders_TextureLoader.IExports
        inherit Types.__loaders_ObjectLoader.IExports
        inherit Types.__loaders_MaterialLoader.IExports
        inherit Types.__loaders_BufferGeometryLoader.IExports
        inherit Types.__loaders_LoadingManager.IExports
        inherit Types.__loaders_ImageLoader.IExports
        inherit Types.__loaders_ImageBitmapLoader.IExports
        inherit Types.__loaders_FontLoader.IExports
        inherit Types.__loaders_FileLoader.IExports
        inherit Types.__loaders_Loader.IExports
        inherit Types.__loaders_LoaderUtils.IExports
//        inherit Types.__loaders_Cache.IExports
        inherit Types.__loaders_AudioLoader.IExports
        //
        // Materials
        //
        inherit Types.__materials_Materials.IExports
        //
        // Math
        //
        inherit Types.__math_interpolants_QuaternionLinearInterpolant.IExports
        inherit Types.__math_interpolants_LinearInterpolant.IExports
        inherit Types.__math_interpolants_DiscreteInterpolant.IExports
        inherit Types.__math_interpolants_CubicInterpolant.IExports
        inherit Types.__math_Interpolant.IExports
        inherit Types.__math_Triangle.IExports
//        inherit Types.__math_MathUtils.IExports
        inherit Types.__math_Spherical.IExports
        inherit Types.__math_Cylindrical.IExports
        inherit Types.__math_Plane.IExports
        inherit Types.__math_Frustum.IExports
        inherit Types.__math_Sphere.IExports
        inherit Types.__math_Ray.IExports
        inherit Types.__math_Matrix4.IExports
        inherit Types.__math_Matrix3.IExports
        inherit Types.__math_Box3.IExports
        inherit Types.__math_Box2.IExports
        inherit Types.__math_Line3.IExports
        inherit Types.__math_Euler.IExports
        inherit Types.__math_Vector4.IExports
        inherit Types.__math_Vector3.IExports
        inherit Types.__math_Vector2.IExports
        inherit Types.__math_Quaternion.IExports
        inherit Types.__math_Color.IExports
        inherit Types.__math_SphericalHarmonics3.IExports
        //
        // Objects
        //
        inherit Types.__objects_Sprite.IExports
        inherit Types.__objects_LOD.IExports
        inherit Types.__objects_InstancedMesh.IExports
        inherit Types.__objects_SkinnedMesh.IExports
        inherit Types.__objects_Skeleton.IExports
        inherit Types.__objects_Bone.IExports
        inherit Types.__objects_Mesh.IExports
        inherit Types.__objects_LineSegments.IExports
        inherit Types.__objects_LineLoop.IExports
        inherit Types.__objects_Line.IExports
        inherit Types.__objects_Points.IExports
        inherit Types.__objects_Group.IExports
        //
        // Renderers
        //
        inherit Types.__renderers_WebGLMultisampleRenderTarget.IExports
        inherit Types.__renderers_WebGLCubeRenderTarget.IExports
        inherit Types.__renderers_WebGLRenderTarget.IExports
        inherit Types.__renderers_WebGLRenderer.IExports
        inherit Types.__renderers_WebGL1Renderer.IExports
        inherit Types.__renderers_shaders_ShaderLib.IExports
        inherit Types.__renderers_shaders_UniformsLib.IExports
        inherit Types.__renderers_shaders_UniformsUtils.IExports
        inherit Types.__renderers_shaders_ShaderChunk.IExports
        inherit Types.__renderers_webgl_WebGLBufferRenderer.IExports
        inherit Types.__renderers_webgl_WebGLCapabilities.IExports
        inherit Types.__renderers_webgl_WebGLClipping.IExports
        inherit Types.__renderers_webgl_WebGLExtensions.IExports
        inherit Types.__renderers_webgl_WebGLGeometries.IExports
        inherit Types.__renderers_webgl_WebGLIndexedBufferRenderer.IExports
        inherit Types.__renderers_webgl_WebGLInfo.IExports
        inherit Types.__renderers_webgl_WebGLLights.IExports
        inherit Types.__renderers_webgl_WebGLObjects.IExports
        inherit Types.__renderers_webgl_WebGLProgram.IExports
        inherit Types.__renderers_webgl_WebGLPrograms.IExports
        inherit Types.__renderers_webgl_WebGLProperties.IExports
        inherit Types.__renderers_webgl_WebGLRenderLists.IExports
        inherit Types.__renderers_webgl_WebGLShader.IExports
        inherit Types.__renderers_webgl_WebGLShadowMap.IExports
        inherit Types.__renderers_webgl_WebGLState.IExports
        inherit Types.__renderers_webgl_WebGLTextures.IExports
        inherit Types.__renderers_webgl_WebGLUniforms.IExports
        inherit Types.__renderers_webxr_WebXR.IExports
        inherit Types.__renderers_webxr_WebXRController.IExports
        inherit Types.__renderers_webxr_WebXRManager.IExports
        //
        // Scenes
        //
        inherit Types.__scenes_FogExp2.IExports
        inherit Types.__scenes_Fog.IExports
        inherit Types.__scenes_Scene.IExports
        //
        // Textures
        //
        inherit Types.__textures_VideoTexture.IExports
        inherit Types.__textures_DataTexture.IExports
        inherit Types.__textures_DataTexture2DArray.IExports
        inherit Types.__textures_DataTexture3D.IExports
        inherit Types.__textures_CompressedTexture.IExports
        inherit Types.__textures_CubeTexture.IExports
        inherit Types.__textures_CanvasTexture.IExports
        inherit Types.__textures_DepthTexture.IExports
        inherit Types.__textures_Texture.IExports


let THREE: Three.IExports = importAll "three"
