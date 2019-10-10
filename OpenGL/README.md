# OpenGL

# TODO

* 폴더 정리 및 링크
* README.md 재 작성

# Question

* over sampling, geometry shader
* gamma correction 은 꼭 해야 하는가???
* [DFG 수식, unity pbr shader 첨삭(opengl에서 사용한걸로), standard shader 분석](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/pbr_qna.md)
* https://github.com/KhronosGroup/glTF-Sample-Viewer/blob/master/src/shaders/metallic-roughness.frag [분석](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/glTF-sample-viewer.md)
* unity shader debug 테크닉, NSight 사용법, https://github.com/glfw/glfw.git Debug Message Callback 분석


# References

- [learnopengl @ github](https://github.com/JoeyDeVries/LearnOpenGL)
- [WebGL Shaders and GLSL @ webglfundamentals](https://webglfundamentals.org/webgl/lessons/webgl-shaders-and-glsl.html)
- [OpenGL Programming Guide: The Official Guide to Learning OpenGL, Version 4.3](http://www.opengl-redbook.com/)
  - opengl red book
  - [src](https://github.com/openglredbook/examples)
- [OpenGL Superbible: Comprehensive Tutorial and Reference](http://www.openglsuperbible.com)
  - opengl blue book
  - [src](https://github.com/openglsuperbible/sb7code)
- [OpenGL Shading Language](https://www.amazon.com/OpenGL-Shading-Language-Randi-Rost/dp/0321637631/ref=sr_1_1?ie=UTF8&qid=1538565859&sr=8-1&keywords=opengl+shading+language)
  - opengl orange book 
- [PBR guide](https://www.substance3d.com/pbr-guide)
  - pbr guide
- [blender tutorial](https://www.youtube.com/playlist?list=PLjEaoINr3zgHs8uzT3yqe4iHGfkCmMJ0P)
- [godot tutorial](https://www.youtube.com/playlist?list=PLda3VoSoc_TSBBOBYwcmlamF1UrjVtccZ)
- [uniform vs attribute](https://webgl2fundamentals.org/webgl/lessons/ko/webgl-shaders-and-glsl.html)

# Agenda

| Chapter | Assginee |
|:--------|:---------|
| [1.1.1.hello_window](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/hello_window.md) | ~~윤~~ |
| [1.1.2.hello_window_clear](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/hello_window_clear.md) | ~~윤~~ |
| [cmake](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/cmake.md) | 윤 |
| [glfw](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/glfw.md) | 윤 |
| [glad](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/glad.md) | 윤 |
| [1.2.1.hello_triangle](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/hello_triangle.md) | ~~윤~~ |
| [1.2.2.hello_triangle_indexed](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/hello_triangle_index.md) | ~~윤~~ |
| [1.2.3.hello_triangle_exercise1](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/hello_triangle_exercise1.md) | ~~윤~~ |
| [1.2.4.hello_triangle_exercise2](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/hello_triangle_exercise2.md) | ~~윤~~ |
| [1.2.5.hello_triangle_exercise3](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/hello_triangle_exercise3.md) | ~~윤~~ |
| [glsl](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/GLSL.md) | ~~윤~~ |
| [1.3.1.shaders_uniform](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/shaders_uniform.md) | ~~윤~~ |
| [1.3.2.shaders_interpolation](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/shaders_interpolation.md) | ~~윤~~ |
| [1.3.3.shaders_class](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/shaders_class.md) | ~~윤~~ |
| [1.4.1.textures](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/textures.md) | ~~윤~~ |
| [1.4.2.textures_combined](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/textures_combined.md) | ~~윤~~ |
| [1.4.3.textures_exercise2](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/textures_exercise2.md) | ~~윤~~ |
| [1.4.4.textures_exercise3](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/textures_exercise3.md) | ~~윤~~ |
| [1.4.5.textures_exercise4](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/textures_exercise4.md) | ~~윤~~ |
| [1.5.1.transformations](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/transformations.md) | ~~윤~~ |
| [1.5.2.transformations_exercise2](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/transformations_exercise2.md) | ~~윤~~ |
| [1.6.1.coordinate_systems](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/coordinate_systems.md) | ~~윤~~ |
| [1.6.2.coordinate_systems_depth](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/coordinate_systems_depth.md) | ~~윤~~ |
| [1.6.3.coordinate_systems_multiple](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/coordinate_systems_multiple.md) | ~~윤~~ |
| [1.7.1.camera_circle](https://gitlab.com/iamslash/train/blob/master/OpenGL/Gijung/camera_circle.md) | ~~기~~ |
| [1.7.2.camera_keyboard_dt](https://gitlab.com/iamslash/train/blob/master/OpenGL/Gijung/camera_keyboard_dt.md) | ~~기~~ |
| [1.7.3.camera_mouse_zoom](https://gitlab.com/iamslash/train/blob/master/OpenGL/Gijung/camera_mouse_zoom.md) | ~~기~~ |
| [1.7.4.camera_class](https://gitlab.com/iamslash/train/blob/master/OpenGL/Gijung/camera_class.md) | ~~기~~ |
| [2.1.colors](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/colors.md) | ~~윤~~ |
| [2.2.1.basic_lighting_diffuse](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/basic_lighting_diffuse.md) | ~~윤~~ |
| [2.2.2.basic_lighting_specular](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/basic_lighting_specular.md) | ~~윤~~ |
| [2.3.1.materials](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/materials.md) | ~~윤~~ |
| [2.3.2.materials_exercise1](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/materials_exercise1.md) | ~~윤~~ |
| [2.4.1.lighting_maps_diffuse_map](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/lighting_maps_diffuse_map.md) | ~~윤~~ |
| [2.4.2.lighting_maps_specular_map](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/lighting_maps_specular_map.md) | ~~윤~~ |
| [2.4.3.lighting_maps_exercise4](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/lighting_maps_exercise4.md) | ~~윤~~ |
| [2.5.1.light_casters_directional](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/light_casters_directional.md) | ~~윤~~ |
| [2.5.2.light_casters_point](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/light_casters_point.md) | ~~윤~~ |
| [2.5.3.light_casters_spot](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/light_casters_spot.md) | ~~윤~~ |
| [2.5.4.light_casters_spot_soft](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/light_casters_spot_soft.md) | ~~윤~~ |
| [2.6.multiple_lights](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/multiple_lights.md) | ~~윤~~ |
| [3.1.model_loading](https://gitlab.com/iamslash/train/blob/master/OpenGL/Gijung/model_loading.md) | ~~기~~ |
| [4.1.1.depth_testing](https://gitlab.com/iamslash/train/blob/master/OpenGL/Nam/depth_testing.md) | ~~남~~ |
| [4.1.2.depth_testing_view](https://gitlab.com/iamslash/train/blob/master/OpenGL/Nam/depth_testing_view.md) | ~~남~~ |
| [4.10.1.instancing_quads](https://gitlab.com/iamslash/train/blob/master/OpenGL/kanghyuk/Instancing.md) | ~~강~~ |
| [4.10.2.asteroids](https://gitlab.com/iamslash/train/blob/master/OpenGL/kanghyuk/asteroids.md) | ~~강~~ |
| [4.10.3.asteroids_instanced](https://gitlab.com/iamslash/train/blob/master/OpenGL/kanghyuk/asteroids_instance.md) | ~~강~~ |
| [4.11.anti_aliasing_offscreen](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/anti_aliasing_offscreen.md) | ~~윤~~ |
| [4.2.stencil_testing](https://gitlab.com/iamslash/train/blob/master/OpenGL/Nam/stencil_testing.md) | ~~남~~ |
| [4.3.1.blending_discard](https://gitlab.com/iamslash/train/blob/master/OpenGL/Gijung/blending_dicard.md) | ~~기~~ |
| [4.3.2.blending_sort](https://gitlab.com/iamslash/train/blob/master/OpenGL/Gijung/blending_sorted.md) | ~~기~~ |
| [4.5.1.framebuffers](Kim/framebuffer.md) | ~~김~~ |
| [4.5.2.framebuffers_exercise1](Kim/framebuffer_ex.md) | ~~김~~ |
| [4.6.1.cubemaps_skybox](https://gitlab.com/iamslash/train/blob/master/OpenGL/kanghyuk/cubemap_skybox.md) | ~~강~~ |
| [4.6.2.cubemaps_environment_mapping](https://gitlab.com/iamslash/train/blob/master/OpenGL/kanghyuk/cubemap_environment.md) | ~~강~~ |
| [4.8.advanced_glsl_ubo](https://gitlab.com/iamslash/train/blob/master/OpenGL/Gijung/glsl_ubo.md) | ~~기~~ |
| [4.9.1.geometry_shader_houses](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/geometry_shader_houses.md) | ~~윤~~ |
| [4.9.2.geometry_shader_exploding](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/geometry_shader_exploding.md) | ~~윤~~ |
| [4.9.3.geometry_shader_normals](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/geometry_shader_normals.md) | ~~윤~~ |
| [5.1.advanced_lighting](https://gitlab.com/iamslash/train/blob/master/OpenGL/Gijung/advanced_lighting.md) | ~~기~~ |
| [5.2.gamma_correction](https://gitlab.com/iamslash/train/blob/master/OpenGL/Gijung/gamma_correction.md)| ~~기~~ |
| [particle](https://gitlab.com/iamslash/train/blob/master/OpenGL/Gijung/particle.md) | ~~이기정~~ |
| [renderdoc](https://gitlab.com/iamslash/train/blob/master/OpenGL/Gijung/RenderDoc.md) 사용법 | ~~이기정~~ |
| [5.3.1.1.shadow_mapping_depth](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/shadow_mapping_depth.md) | ~~윤~~ |
| [5.3.1.2.shadow_mapping_base](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/shadow_mapping_base.md) | ~~윤~~ |
| [5.3.1.3.shadow_mapping](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/shadow_mapping.md) | ~~윤~~ |
| [5.3.2.1.point_shadows](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/point_shadows.md) | ~~윤~~ |
| [5.3.2.2.point_shadows_soft](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/point_shadows_soft.md) | ~~윤~~ |
| 5.3.3.csm | ~~남~~ |
| [5.4.normal_mapping](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/normal_mapping.md) | ~~윤~~ |
| [5.5.1.parallax_mapping](https://gitlab.com/iamslash/train/blob/master/OpenGL/kanghyuk/parallax_mapping.md) | ~~강~~ |
| [5.5.2.steep_parallax_mapping](https://gitlab.com/iamslash/train/blob/master/OpenGL/kanghyuk/steep_parallax_mapping.md) | ~~강~~ |
| [5.5.3.parallax_occlusion_mapping](https://gitlab.com/iamslash/train/blob/master/OpenGL/kanghyuk/parallax_occlusion_mapping.md) | ~~강~~ |
| [5.6.hdr](https://gitlab.com/iamslash/train/blob/master/OpenGL/Nam/hdr.md) | ~~남~~ |
| [5.7.bloom](https://gitlab.com/iamslash/train/blob/master/OpenGL/Nam/bloom.md) | ~~남~~ |
| [5.8.1.deferred_shading](https://gitlab.com/iamslash/train/blob/master/OpenGL/kanghyuk/deferred_shading.md) | ~~강~~ |
| [5.8.2.deferred_shading_volumes](https://gitlab.com/iamslash/train/blob/master/OpenGL/kanghyuk/deferred_shading_volumes.md) | ~~강~~ |
| 5.9.ssao | ~~김~~ |
| [6.1.1.lighting](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/lighting.md) | ~~윤~~ |
| [6.1.2.lighting_textured](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/lighting_textured.md) | ~~윤~~ |
| [6.2.1.1.ibl_irradiance_conversion](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/ibl_irradiance_conversion.md) | ~~윤~~ |
| [6.2.1.2.ibl_irradiance](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/ibl_irradiance.md) | ~~윤~~ |
| [6.2.2.1.ibl_specular](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/ibl_specular.md) | 윤 |
| [6.2.2.2.ibl_specular_textured](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/ibl_specular_textured.md) | 윤 |
| [pbr example](https://www.jordanstevenstechart.com/physically-based-rendering) | 윤 |
| [unity standard shader](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/standard.md) | 윤 |
| [7.1.debugging](https://gitlab.com/iamslash/train/blob/master/OpenGL/Nam/debugging.md) | ~~남~~ |
| [imgui](https://gitlab.com/iamslash/train/blob/master/OpenGL/Gijung/Imgui.md) | ~~이기정~~ |
| [nuklear](https://gitlab.com/iamslash/train/blob/master/OpenGL/kanghyuk/Nuklear.md) | ~~이강혁~~ |
| [OpenGL Window Example @ qt](https://gitlab.com/iamslash/train/blob/master/OpenGL/Eunji/qt.md) | 윤 |
| [bgfx](https://gitlab.com/iamslash/train/blob/master/OpenGL/kanghyuk/bgfx.md) 샘플을 bgfx 로 작성해서 빌드하기 | ~~이강혁~~ |
| [godot](https://github.com/godotengine/godot) sample 제작해서 windows, android, ios 빌드해보기 | ~~이기정~~ |
| [godot](https://github.com/godotengine/godot) [코드분석, editor GUI, 내보내기(android, ios)](https://gitlab.com/iamslash/train/blob/master/OpenGL/kanghyuk/Godot.md) | 이강혁 |
| [blender](https://github.com/sobotka/blender) 사용법 | 이기정 |
| [blender](https://github.com/sobotka/blender) 코드분석 | ??? |
