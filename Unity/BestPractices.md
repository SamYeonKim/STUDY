- [Optimizing Graphics in Unity](#optimizing-graphics-in-unity)
  - [3. Auto Generate and Generate Lighting](#3-auto-generate-and-generate-lighting)
  - [4. Lighting Settings](#4-lighting-settings)
  - [5. Multi-Scene editing](#5-multi-scene-editing)
  - [6. Camera](#6-camera)
    - [Clear (CameraClearFlags)](#clear-cameraclearflags)
    - [Culling](#culling)
    - [Fillrate](#fillrate)
    - [Draw Call Batching](#draw-call-batching)
    - [Geometry](#geometry)
    - [Level of Detail (LOD)](#level-of-detail-lod)
  - [7. Textures](#7-textures)
  - [8. Multithreaded Rendering & Graphics Jobs](#8-multithreaded-rendering--graphics-jobs)
    - [Singlethreaded Rendering (single client, no worker thread)](#singlethreaded-rendering-single-client-no-worker-thread)
    - [Multithreaded Rendering (single client, single worker thread)](#multithreaded-rendering-single-client-single-worker-thread)
    - [Jobified Rendering (multiple clients, single worker thread)](#jobified-rendering-multiple-clients-single-worker-thread)
    - [Graphics Jobs (multiple clients, no worker thread)](#graphics-jobs-multiple-clients-no-worker-thread)
  - [9. Framebuffer](#9-framebuffer)
    - [Double & Triple Buffering](#double--triple-buffering)
    - [Color Buffer](#color-buffer)
    - [Stencil & Depth Buffer](#stencil--depth-buffer)
    - [Final Blit](#final-blit)
  - [10. Shaders](#10-shaders)
- [Memory Management in Unity](#memory-management-in-unity)
  - [2. Managed Memory](#2-managed-memory)
  - [3. IL2CPP & Mono](#3-il2cpp--mono)
    - [IL2CPP](#il2cpp)
    - [Mono](#mono)
    - [Code stripping in Unity](#code-stripping-in-unity)
    - [Unity Module Stripping](#unity-module-stripping)
    - [C# Code Stripping](#c-code-stripping)
    - [Generic Sharing](#generic-sharing)
    - [Assembly Definition Files](#assembly-definition-files)
    - [Build Report](#build-report)
  - [4. Native Memory](#4-native-memory)
    - [Native Buffer](#native-buffer)
    - [Assets](#assets)
  - [5. Android Memory Management](#5-android-memory-management)
    - [Paging on Android](#paging-on-android)
    - [Memory Consumption Limitations](#memory-consumption-limitations)
    - [Dumpsys](#dumpsys)
    - [dumpsys meminfo](#dumpsys-meminfo)
    - [procrank](#procrank)
    - [meminfo](#meminfo)
    - [Android Studio](#android-studio)
    - [Plugins](#plugins)

# Optimizing Graphics in Unity

## 3. [Auto Generate and Generate Lighting](https://unity3d.com/kr/learn/tutorials/topics/best-practices/auto-generate-and-generate-lighting?playlist=30089)
* Auto Generate 옵션을 켜면 씬에 변경점이 있을 때마다 자동으로 라이트맵을 구워준다. 옵션은 Lighting Settines 에서 끌 수 있다. 
* Lighting Data Asset은 Lightmap Snapshot의 다른 이름인데, Generate Lighting을 하면 생성되는 에셋이다. 씬에서 라이팅 정보를 만들 때 필요한 GI 데이터와 기타 서포트 파일들이 포함되어 있다.
* Auto Generate를 할 때는 Lighting Data Asset를 만들지 않고 해당 정보를 씬의 메모리에다 저장한다. 이는 빠른 반복 작업을 위한 속도 때문이다. 파일보다 메모리를 읽는 속도가 빠르고, Lighting Data Asset 을 변경하면 유니티는 씬을 다시 serialize하기 때문이다.

* Play Mode 에서 Auto-Generated Lighting Data의 문제점
  * 씬을 언로드 하면 auto-generated lighting data 정보를 잃는다. 유니티가 Play Mode로 들어갈 때 오토 라이팅 정보를 게임 오브젝트에 붙이기 때문에 씬을 언로드 하면 게임오브젝트와 같이 파괴된다. Lighting Data Asset이 없으면 Edit Mode로 가야 GI를 볼 수 있다.

  * 단일 씬일 때는 Play Mode로 들어가면 유니티가 GI 데이터를 메모리에 잘 올려놓지만, 사용자가 스크립트로 추가 씬을 불러올 때는 이펙트가 미묘하게 달라 보일 수 있다.

  * Auto Generate 옵션을 켠 여러 개의 씬을 로드하면, 유니티는 라이팅 데이터를 specific combination of Scenes 에다가 저장한다. 로드된 씬들을 가지고 뭔가를 하면 GI 데이터가 메모리에 있기 때문에 빠르게 테스트가 가능한데, 다른 씬을 테스트 하거나 기존의 씬을 추가 로드하는 형식의 테스트를 하면 GI 데이터는 사라진다.

  * 결론 : Edit Mode -> Play Mode 진입 시 로드되어 있는 씬에 한해서만 Auto Generate가 만든 GI 데이터를 메모리에 저장한다. 이외에 로드 되는 씬들의 데이터는 없다.

----
## 4. [Lighting Settings](https://unity3d.com/kr/learn/tutorials/topics/best-practices/lighting-settings?playlist=30089)

* lighting setting은 전역 세팅(Global Settings), 개별 씬 세팅(Scene-dependent Settings)으로 나뉜다. 여러 씬의 조명 설정을 변경하면 전역 설정도 같이 변경해야 한다. merge 되거나 덮어 씌여지기 때문에.

* 여러 개의 씬을 로드할 때 유니티는 Global Settings 옵션을 merge 한다. 추가로 씬을 로드할 때는 기존의 옵션을 쓰되 기존 옵션이 비어있으면 추가로 로드된 씬의 정보를 덧붙이는 식으로 merge 된다.

* Lightmap Resolution, Lightmap Padding, Lightmap Size, Compress Lightmap, Ambient Occlusion, Final Gather(최종 빛 텍스쳐를 baked lightmap과 동일한 해상도로 변경시켜주는 옵션??)를 제외한 모든 옵션은 전부 Global Settings이다.

----
## 5. [Multi-Scene editing](https://unity3d.com/kr/learn/tutorials/topics/best-practices/multi-scene-editing?playlist=30089)

* 에디터에서 여러 씬을 로드하는 경우, 유니티는 하나의 Lighting Data Asset를 만들고 그 안에 각 씬의 Lighting Data Asset를 넣는다. 여러 개의 씬을 동시에 편집할 때, 편집기가 전체 씬의 설정을 merge하여 보여준다. 개별 씬의 설정은 유지된다.

----
## 6. [Camera](https://unity3d.com/kr/learn/tutorials/topics/best-practices/camera?playlist=30089)
> 중요한 컴포넌트.

### Clear (CameraClearFlags)

* 모바일 대상일 때 해당 플래그를 Don't clear 로 설정하면 안된다. 필요에 따라 유니티가 이전의 내용들을 지우거나 무시할 수 있기 때문에.(?)

* 유니티 기본 skybox 는 계산을 많이 한다. 렌더링을 비활성화 하려면 플래그를 SolidColor로 설정한다.

* Discard and Restore buffer
  * OpenGLES를 사용하는 경우 유니티는 프레임버퍼를 버린다.
  * 타일기반 GPU에서 논리 버퍼에 데이터를 로드하거나 저장하는 것은 상당한 자원이 드는 활동이다. (공유메모리 <-> 프레임 버퍼)

* 타일 기반 렌더링(Tile-based rendering)
  * 뷰포트를 32x32px 과 같은 작은 타일들로 나눠서 GPU에 가까운 빠른 메모리에 저장.
  * 빠른 메모리와 프레임버퍼 간의 복사 작업은 산술연산보다 시간이 오래 걸린다.
  * glClear 명령을 이용하여 이전 버퍼 내용이 필요없다는 것을 하드웨어에 알리면 복사 작업을 생락할 수 있다.
  * 타일 기반 렌더링을 사용하는 GPU : Adreno, PowerVR, Apple A

* RenderTexture Switching
  * 그래픽 드라이버는 렌더링 대상을 변경할 때 프레임버퍼에 로드 및 저장 작업을 한다.
  * 연속 프레임으로 렌더링 하면 시스템은 공유 메모리와 GPU간에 반복적으로 텍스쳐의 내용을 전송한다.

* Framebuffer Compression
  * 전체 버퍼를 지우면 드라이버가 GPU와 메모리 사이에 전송해야 하는 데이터 양을 줄여 압축할 수 있으므로 처리량이 향상되어 프레임 속도가 높아진다.

### Culling

* 시스템 성능에 심각한 영향을 미칠 수 있다. 특히 여러 카메라를 사용하는 경우

* Frustum Culling : 카메라 시야에 보이는 부분만 렌더링. 카메라에 설정된 레이어만 컬링

* Occlusion Culling : 다른 오브젝트에 가려지지 않는 부분만 렌더링.

### Fillrate
* Overdraw : 같은 픽셀을 여러번 그리는 것. 씬 뷰의 Overdraw view로 여러번 그리는 부분을 확인할 수 있다.
* 투명한 물체, 알파블렌딩은 픽셀 드로우를 늘린다.
* Z-Test는 드로잉보다 빠르기 때문에 물체가 앞에있는데 보이지 않으면 Draw Order를 바꿔야 한다.

### Draw Call Batching

* 모바일에서 드로우콜 배칭을 사용하여 최적화를 할 수 있다.
  * 텍스쳐 적게 사용, 아틀라스 사용
  * 움직이지 않는 오브젝트는 전부 스태틱, StaticBatchingUtility로 런타임에 생성도 가능
  * 가장 큰 아틀라스 크기로 라이트맵을 굽는다. 작은 물체는 라이트프로브가 더 나을수도 있다.
  * Renderer.material에 접근하면 자동으로 인스턴스가 만들어진다. Renderer.sharedMaterial을 사용하자.
  * 다중 패스 쉐이더를 조심. 하나 이사의 directional이 적용되어 배칭이 부서지는 것을 막기 위해 noforwardadd 옵션을 셰이더에 추가한다.(??)

* instancing은 기본 하드웨어에 따라 50~100개의 메쉬를 사용할 때 유용하다.

### Geometry

* 오브젝트의 복잡성을 최소로 유지하는 것이 중요하다.
* 유니티에서는 낮은 폴리카운트를 가진 오브젝트를 많이 그리는 것보다 높은 폴리카운트를 가진 오브젝트를 적게 그리는 것이 더 빠르다.
* 볼 수 없는 면은 안그리는 것이 최고다.
* 메쉬를 간소화하고 텍스쳐로 세부정보를 표현하는 것이 낫다.
* 베이킹을 최대한 많이 하는것이 좋다.

### Level of Detail (LOD)

* 카메라와 물체의 거리에 따라 메쉬에 렌더링되는 삼각형 수를 줄이는 방법.
* 카메라가 움직이지 않는 경우엔 LOD보다 각 거리별 메쉬를 따로 만드는 것이 낫다.
* 고품질의 LOD를 만들 수 없으면 장면이 변경되는 동안 실행할 수 있는 런타임 메쉬 조합을 사용할 수 있다. 그러나 조합을 실행하는 동안 프레임 속도가 더 낮을 수 있기 때문에 모바일에는 안맞다.
* 애니메이션에 LOD를 사용하려면 2개의 레이어를 설정하면 된다. 번거롭지만 사용은 가능.

----
## 7. [Textures](https://unity3d.com/kr/learn/tutorials/topics/best-practices/textures?playlist=30089)
> 텍스쳐 압축 방법에는 ASTC, ETC2(Android), PVRTC(iOS) 가 있다.

* ASTC 
  * 빌드 시간에 상당한 이득. 유니티에서 해당 압축방식이 가장 빠르다. ios에서는 A8칩(아이폰 6) 이상부터, 안드로이드는 Mali-T760 MP8 (갤럭시 s6) 이상부터 지원.
  * 텍스쳐 메모리 50% 절감, OpenGL ES 3.0부터 지원 가능

* PVRTC
  * ios의 메인 텍스쳐 압축 방식. android에서는 ETC2로 변경해야된다.
  * 정사각형 텍스쳐(2^x 해상도)를 사용해야 한다. 아니면 유니티는 텍스쳐를 압축하지 않는다.

* ETC2
  * Ericsson Texture Compression. 에릭슨 사에서 만든 모바일용 텍스쳐 파일 포맷. android의 표준 압축 방식.
  * OpenGL ES 3.0부터 지원
  * OpenGL ES 2.0에서 ETC2를 사용하면 보이긴 하지만 내부적으로 RGBA32 포맷을 사용하는 텍스쳐가 생성된다. -> 메모리 낭비가 심하다.

* 텍스쳐를 로드할 때 유니티는 load, awake 두 가지 단계를 거친다.

* 애셋들을 로드하면, 유니티는 preloading thread(disk I/O)에서 graphics thread(GPU)로 이동한다.??

* 유니티는 모든 씬 오브젝트들을 awake한 후 메인스레드에서 애셋을 awake한다. AssetBundle.LoadAsset은 기본 스레드를 차단한 후 awake, AssetBundle.LoadAssetAsync는 time slicing을 사용.

* GPU 메모리에 과부화가 걸리면 GPU는 가장 최근에 사용되지 않은 텍스쳐를 언로드하고 다음에 카메라에 보이면 다시 업로드 한다.

----
## 8. [Multithreaded Rendering & Graphics Jobs](https://unity3d.com/kr/learn/tutorials/topics/best-practices/multithreaded-rendering-graphics-jobs?playlist=30089)
> 유니티는 플랫폼과 그래픽 API에 의존하는 몇몇 렌더링 모드를 제공한다.

### Singlethreaded Rendering (single client, no worker thread)
* 디폴트 옵션. 
* client는 메인 스레드에서 모든 렌더링 명령을 수행(RCMD)
* client는 실제 그래픽 장치를 소유하고 그래픽 API를 통해 실제 렌더링을 수행한다.(GCMD)

### Multithreaded Rendering (single client, single worker thread)
* 그래픽 API가 지원한다면 디폴트 옵션. 
* 하나의 worker thread가 client로부터 오는 렌더링 명령을 수행한다.
* worker thread는 실제 그래픽 장치를 소유하고 그래픽 API가 렌더링을 수행하도록 한다.
* 가능하면 사용하는 것을 권장. 그래픽 API가 지원하는지 확인해야 함.

### Jobified Rendering (multiple clients, single worker thread)
* 현재 Graphics Jobs로 대체됨
* 여러 개의 job들이 각 스레드에서 실행되고, 중간 그래픽 명령을 만든다(IGCMD). 이다음은 Multithreaded Rendering과 비슷하게 worker thread에 중간 명령들이 저장되고 실제 그래픽 장치에 명령을 보낸다(GCMD).

### Graphics Jobs (multiple clients, no worker thread)
* 디폴트로 꺼져 있음. 
* 여러 client가 각각의 스레드에서 직접 그래픽 장치에 명령을 보냄(GCMD). -> 중간 명령을 쓰고 읽는 비용이 제거됨
* 현재 Vulkan API에서만 지원. (ios는 x)
* 해당 모드는 스케쥴링을 하는 렌더링 스레드가 없기 때문에 메인스레드에 약간의 오버헤드가 생긴다.

----
## 9. [Framebuffer](https://unity3d.com/kr/learn/tutorials/topics/best-practices/framebuffer?playlist=30089)
> Framebuffer는 depth, stencil, color buffer를 포함한다. color buffer는 필수이고 나머지 버퍼는 그래픽 특징에 따라 사용되지 않을 수 있다.

### Double & Triple Buffering
* 장치가 지원하면 그래픽 드라이버는 두개 또는 세개의 프레임버퍼를 필요로 한다.
* 더블버퍼링은 백 버퍼에 다음 프레임을 미리 그려놓고 화면 갱신 시 프론트 버퍼와 백 버퍼를 바꿔치기하여 자연스럽게 화면 전환을 이루게 해준다.
* 화면전환이 이루어질 때 백 버퍼의 화면과 현재 화면이 섞이는 경우 테어링(tearing)이 발생한다. 이를 방지하는 기술이 수직동기화이다.
* 트리플버퍼링은 백 버퍼를 2개 사용하여 화면전환이 이루어질 때 다른 백 버퍼에 다음 화면을 그려놓는다. 프레임 속도가 더블버퍼링보다 높다.

### Color Buffer
* 프레임버퍼의 수는 그래픽 드라이버에 달려있으며, 프레임 버퍼당 하나의 색상 버퍼가 들어간다.
* 유니티는 트리플버퍼링을 사용하지만 장치가 지원하지 않으면 더블버퍼링을 사용한다.

### Stencil & Depth Buffer
* 해당 버퍼들은 사용하는 경우에만 프레임버퍼에 들어간다. 프레임버퍼가 많은 비용이 들기 때문.
* depth 버퍼는 24비트, stencil 버퍼는 8비트이다. 데스크탑 플랫폼은 32비트 버퍼 하나로 결합되지만 모바일은 개별 버퍼로 취급된다.

### Final Blit
* ?????

----
## 10. [Shaders](https://unity3d.com/kr/learn/tutorials/topics/best-practices/shaders?playlist=30089)

* 내장쉐이더를 사용하는 경우 모바일버전 또는 Unlit버전을 사용해야 한다.
* 라이트맵 쉐이더는 기본을 쓰는게 좋다.
* 그래픽 설정에서 항상 로드되는 쉐이더를 제거할 수 있다. 로드 시간을 제어하려면 쉐이더 변형 컬렉션을 사용하면 된다.

* 쉐이더는 여러 버전으로 변형되는데, 필요한 쉐이더만 압축 해제되어있고 나머지 쉐이더는 압축한다.
* 프로파일러를 사용하여 Shaderlab 루트 아래에 있는 버퍼, 소스코드 및 쉐이더 컴파일과 관련된 다른 할당들을 확인할 수 있다.
* 쉐이더를 작성할 때 밑줄을 사용하여 전역 키워드를 해제/활성화 할 수 있다. (#pragma shader_feature ~~) 잘 사용하면 불필요한 쉐이더 컴파일을 막을 수 있다.
* 쉐이더에는 빌드 크기를 늘리는 불필요한 variants를 포함하는 경우가 많다. #define을 쉐이더 코드에서 사용하면 안된다.
* 쉐이더를 미리 로드해놓으면 씬 로드 시간이 줄어든다.
* 내장된 쉐이더는 특정 유스케이스에 적합하게 만들어졌기 때문에 사용하지 않는다면 제거해야 한다.


# Memory Management in Unity

----
## 2. [Managed Memory](https://unity3d.com/kr/learn/tutorials/topics/best-practices/managed-memory?playlist=30089)
> 유니티는 사용자가 응용 프로그램을 종료할때까지 관리 메모리를 운영체제로 반환하지 않는다. 힙은 사용 가능한 메모리가 부족할때마다 커진다. 애셋이 관리 메모리를 차지하는 방식을 파악하는 것이 중요하다.

* Destroy()를 사용하여 객체를 파괴하고 메모리를 해제해야한다. null로 설정해도 참조는 파괴되지 않는다.
* 오래 사용할 오브젝트는 클래스로, 잠깐 쓸 오브젝트는 구조체로 설정한다.(GC때문에)
* 임시 작업 버퍼를 재사용하여 할당횟수를 낮추자.
* enum은 프로그램이 끝날때까지 메모리를 정리하지 않는다.
* 코루틴 내부에서 대용량의 관리 메모리를 할당하는 일을 할 때는 코루틴을 빨리 끝내야 한다. 힙 메모리에서 스택 할당을 계속 유지하기 때문.


----
## 3. [IL2CPP & Mono](https://unity3d.com/kr/learn/tutorials/topics/best-practices/il2cpp-mono?playlist=30089)
> IL2CPP와 Mono는 각 장단점이 있다.

### IL2CPP
* C# -> IL -> c++ code로 변경
* Mono를 사용할 때보다 빠르다.
* c++ 스크립트 코드를 디버깅할 수 있다.
* 엔진 코드 스트리핑을 이용하여 코드 크기를 줄일 수 있다.
* 빌드시간이 길고 AOT(Ahead of TIme) 컴파일만 지원한다.

### Mono
* 빠른 빌드 시간
* JIT컴파일 지원. 런타임 코드 실행 지원
* managed assemblies를 제공해야 한다. (mono- 또는 .net-의 dll파일)

### Code stripping in Unity
* 코드 크기는 런타임 메모리에 직접적인 영향을 주기 때문에 사용하지 않는 코드 경로를 제거하는 것이 중요하다. 유니티는 두 가지 단계를 제공한다.
* Managed code stripping : 메소드 레벨에서 관리 코드를 제거한다. UnityLinker는 중간언어에서 사용되지 않는 클래스나 구조체 등을 제거한다. IL2CPP는 필수사항이다.
* Native code stripping : 디폴트옵션. 스트립 엔진 코드를 사용하여 유니티 엔진 코드에서 사용하지 않는 모듈과 클래스를 제거한다.

### Unity Module Stripping
* 현재 webGL만 지원함.
* 사용되지 않는 모든 유니티 모듈을 제거.
* 카메라, 애셋번들 등의 코어모듈은 제거하지 않음.

### C# Code Stripping
* UnityLinker는 GC와 비슷한 기본 마크 및 스윕 원리로 작동(?)
* 각 어셈블리에 포함된 메소드 맵을 작성한다. 타입과 메소드를 '루트'로 표시하고, 타입과 메소드 간의 의존성 그래프를 만들어 처리한다.
* UnityLinker는 내부 클래스를 씬이나 리소스에서 사용한 적이 있다면 루트로 표시한다.
* 유니티는 대부분의 c# 코드를 Assembly-CSharp.dll에 저장한다. Standard Assets와 Plugins의 코드는 Assembly-CSharp-firstpass.dll에 저장한다.

### Generic Sharing
* reference 타입을 generic 형식에 사용하면 공유할 수 있는 코드를 생성하지만, value 타입을 generic 형식에 사용하면 유형별로 코드를 생성한다. -> 코드 크기가 증가한다
* 성능차이가 눈에띄지는 않음.

### Assembly Definition Files
* Assembly-CSharp.dll 대신 사용자 스크립트를 폴더 별로 할당할 수 있다.
* 스크립트가 변경되면 영향을 받는 어셈블리만 만들기 때문에 반복 시간이 빨라진다.
* 모듈화 하면 바이너리 크기와 런타임 메모리도 같이 늘어난다.

### Build Report
* 유니티에 포함되어 있지만 UI가 없는 API.
* 빌드를 하면 생성됨. 제거된 파일과 최종 실행 파일에서 제거된 파일을 찾을 수 있다.


----
## 4. Native Memory
> 네이티브 메모리는 대부분의 엔진 코드가 resident memory에 있으므로 최적화의 핵심 요소이다. 해당 섹션에서는 유니티 내부 시스템과 네이티브 프로파일러에서 자주 보이는 메모리 데이터에 대해 설명한다.

### Native Buffer
* Scratchpad : 상수를 저장하는 4MB의 버퍼 풀. GPU에 바인딩 되어 XCode 또는 Snapdragon과 같은 프레임 캡쳐 도구에 나타난다.
* Block Allocator : 일부 내부 시스템에서 사용. 새 메모리 블록을 할당할 때마다 메모리와 CPU 오버헤드가 있다.(??)
* AssetBundles : 첫 페이지 블록 메모리에 할당. 유니티는 애셋 번들 시스템이 할당한 페이지를 재사용하지만 많은 번들을 한번에 로드하는 경우 추가 블록을 할당할 수 있다. 이 모든 것은 프로그램이 종료될 때까지 할당된다.
* Resources : 다른 시스템과 공유된 블록 할당자를 사용하기 때문에, 처음 애셋을 로드할 때 CPU 또는 메모리 오버헤드가 없다.
* Ringbuffer : 유니티가 텍스쳐를 GPU에 푸시할 때 사용하는 버퍼. 한번 할당된 후에는 시스템에 메모리를 반환할 수 없다.

### Assets
> 애셋은 런타임 중에 네이티브, 관리 메모리 관련 사항을 발생시킨다. 기본 런타임 메모리를 줄이는 방법을 사용하여 메모리를 줄일 수 있다.

* 메쉬에서 사용하지 않는 채널을 제거한다.

* 애니메이션에서 중복 키 프레임을 제거한다.

* 품질 설정에서 maxLOD를 사용하여 세부적인 메쉬를 제거한다.

* 빌드 후 Editor.log에서 디스크의 애셋 크기가 런타임 메모리 사용량에 비례하는지 확인한다.

* 밉맵을 통해 텍스쳐 해상도를 낮춰 GPU 메모리에 업로드된 메모리를 줄인다.

* 노멀 맵은 디퓨즈 맵과 같은 크기일 필요가 없으므로 노멀 맵에 작은 해상도를 사용하여 메모리 및 디스크 공간을 절약할 수 있다.

* 관리 메모리의 경우 힙의 단편화로 인해 메모리 문제를 일으킬 수 있다.

* Cloned Materials : 렌더링 도구의 material 속성을 보면 아무것도 지정되지 않아도 material이 복제된다.(?) 이 자료는 GC로 수집되지 않으면 장면을 변경하거나 Resources.UnloadUnusedAssets()을 호출해야 지워진다. 읽기 전용 자료를 보려면 customRenderer.sharedMaterial을 사용할 수 있다.
* Unloading Scenes : 씬을 언로드하여 관련된 게임오브젝트를 파괴하거나 언로드 해야한다. 연결된 애셋은 언로드 되지 않기 때문에 Resources.UnloadUnusedAssets()를 호출해야 한다.
* Audio Import Settings : 올바른 설정을 사용하여 런타임 메모리 및 CPU 성능을 절약할 수 있다. 
* 스테레오 사운드가 필요하지 않은 경우 강제로 모노 옵션을 활성화
* 큰 오디오 클립은 스트리밍으로 설정. 200KB보다 작은 오디오 파일을 Compressed into Memory로 설정
* Decompress On Load 옵션은 메모리가 여유로울 때만 사용
* 긴 클립은 압축 옵션 사용. mp3 또는 ogg. ogg가 루핑을 더 잘 처리하기 때문에 긴 클립에 적합함.


----
## 5. Android Memory Management
### Paging on Android
* 페이징 : 메인 메모리에서 보조 메모리로 또는 그 반대로 메모리를 이동시키는 방법
* 

### Memory Consumption Limitations
* 

### Dumpsys
* 

### dumpsys meminfo
* 

### procrank
* 

### meminfo
* 

### Android Studio
* 

### Plugins
* 