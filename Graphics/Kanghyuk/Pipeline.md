# Rendering Pipeline
> Mesh의 Life Cycle

-----
## Load
* app 또는 게임이 mesh를 렌더링 하기 위해 로드할 것을 요청
* 유니티 또는 앱이 디스크에서 RAM으로 mesh를 로드. .obj 또는 FBX 포맷으로 존재하던 파일은 mesh information의 형태로 RAM에 로드.

-----
## Interaction between CPU and GPU
* CPU 는 직접적으로 GPU에 명령하지 못한다.
* command buffer(= ring buffer, ring)라는 큐를 이용해서 커뮤니케이션을 함.
* CPU가 GPU에게 명령을 요청할 때, 두 가지를 버퍼에 저장한다. 1. render state를 세팅 2. mesh를 drawing
* Render State에는 vertex shader, pixel shader, texture, lighting settings 가 들어간다.
* state가 세팅되면, drawing command를 수행한다. 여기서 texture와 mesh가 VRAM으로 전송된다.
* GPU는 시스템보다 VRAM에 더 빨리 접근할 수 있기 때문에 information을 VRAM으로 전송한다.
* state가 완성되면 이 state를 사용하는 모든 mesh를 drawing하는데, 이는 state setting이 drawing보다 무거운 작업이기 때문이다. (= batching)

-----
## Actual Drawing
> 실제 그리기는 다음 순서로 수행된다.

* vertex shader를 실행한다. 이 shader 는 world position, normal, tangent와 같은 mesh의 속성을 읽는다.
* object space 속성은 world space -> view space -> projection space 순으로 변경된다. 
* projection space matrix는 culling을 할 수 있다. (카메라의 frustum에 기초하는 좌표이기 때문)
* Rasterization process를 수행한다. rasterizer는 어떤 픽셀이 화면에 그려질 지 결정하고 pixel shader에게 보내기 전, vertex shader에서 나온 정보를 보간(interpolate)한다.
* pixel 또는 pixel location이 결정되면 pixel shader(= fragment shader)가 실행된다.
* pixel shader는 한 픽셀의 컬러, alpha, z-depth 데이터를 결정한다. 그리고 z-test를 수행한다. (depth에 따라 해당 pixel을 그릴지 말지 결정하는 테스트)
* 스크린은 2개의 버퍼를 갖는다. color buffer(= frame buffer), z-buffer
* 모든 스크린에 보이는 컬러값 픽셀은 전부 color buffer이다.
* z-test 이후 픽셀은 blending step으로 들어간다. 이는 옵션이다.
* blending 이후 픽셀은 stencil test를 수행한다. 이 또한 옵션이며 z-test와 비슷하다. stencil은 해당 픽셀을 화면에 표시할 지를 결정한다.
* stencil test를 pass한 뒤, color masking 작업이 발생한다. 이 또한 옵션이다. color mask는 RGBA채널 중 표시하고 싶은 채널만을 screen에 투영해준다.
* color masking이 끝난 뒤 screen에 보여줄 final color가 그려지며, color buffer에 저장된다.