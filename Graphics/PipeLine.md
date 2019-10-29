# Rendering Pipeline

## Load
* 프로그램이 디스크에서 Mesh파일을 읽어서 Ram에 Mesh Information의 형태로 로드
* CPU가 GPU에게 바로 명력하지 못하기 때문에, 큐 형태의 CommandBuffer를 사용해서 필요한 명령을 저장하고 GPU가 CommandBuffer에서 Command를 가져가서 수행한다
* CPU는 GPU에게 요청을 2가지의 단계로 하게 되는데, 첫번째가 RenderState 설정이고, 두번째가 Draw 단계이다.
* RenderState는 Vertex Shader, Pixel Shader, Texture, Lighting Setting의 정보를 가지고 있다.
* Draw단계에서는 VRAM에 RenderState에서 넘긴 정보를 기반으로 Mesh와 텍스쳐를 그린다
* RenderState를 설정하는것이 그리는 행위보다 비싸기 때문에, Batching이 일어난다. 동일한 쉐이더에 동일한 텍스쳐, 동일한 Mesh를 사용할때, 중복된 객체를 여러개 그릴때 매번 RenderState를 거치는 것이 아니라, RenderState는 한번만 이뤄지고 그리는 행위를 여러번 한다.

# VertexShader
+ 여기서는 Vertex Position, Normal, Tangents등이 사용된다. 그리고, 물체를 화면에 배치 하기 위해서, 좌표 변환을 수행하는데, 

1. 월드 좌표
2. 뷰 좌표
3. 프로젝션 좌표

여기까지 수행하면, 래스터라이저를 할 준비가 되었지만, 여기서 한번더 카메라에 보이는 영역만을 그리기 위해서 Culling을 수행한다.

# Rasterization
+ Mesh를 어떤 픽셀에 그릴것인가를 결정하는 단계이다.

## Fragment Shader

1. 픽셀의 색상
2. 픽셀의 알파
3. 픽셀의 Z-Depth
를 결정한다.

## Alpha Blening
+ 알파값을 이용해서, 어떻게 섞을 것인가 결정된다.

## Stencil Test
+ 화면상에 어떤 픽셀을 그리는데 사용할 것인가 결정하게 되는데, 1이면 그리고, 0이면 그리지 않는다.

## Depth Test
+ 화면상에 깊이 값을 이용해서 기존 값과의 연산을 통해서 Depth Buffer에 값을 갱신 한다/

## Color Mask
+ ColorBuffer에서 어떤 Color Channel을 남겨놓을 것인가를 결정한다.





