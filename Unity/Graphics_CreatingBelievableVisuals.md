## [Understanding Post Process Features](https://unity3d.com/kr/learn/tutorials/projects/creating-believable-visuals/understanding-post-process-features?playlist=17102)

> Post Process는 기존에 렌더링 된 효과에 추가로 더해지는 렌더링 효과이다. 바로 눈에 보이고, 기존 컨텐츠를 변경하지 않아도 많이 바뀐 것 처럼 보이는 장점이 있다.

### Anti aliasing

* 3d를 rasterize 하면 계단현상이 생기는데(aliasing), 이를 수정하기 위한 방법.

* 현재 가장 많이 쓰이는 방식은 FXAA와 TAA.
  * FXAA ( Fast Approximate Anti-aliasing) : 순수 포스트 프로세스 앤티 앨리어싱. 가장자리 분석 후 기존 이미지 위에 알고리즘을 실행하여 부드럽게 만든다. 간단하고 복잡한 의존성이 없으며 빠름.
  * TAA ( Temporal Anti-aliasing) : jittering(변화량 수준 체크)과 이전 프레임을 블렌딩 할 추가 데이터로 사용함. 모션 벡터로 렌더링 픽셀을 예측함. SuperSampling의 품질과 비슷하게 성능에 큰 영향을 주지 않으면서 훨씬 더 부드러운 앤티 앨리어싱을 얻을 수 있음.

### Ambient Occlusion

* Ambient occlusion post process는 스크린 공간 데이터, 주로 깊이를 기반으로 한 앰비언트 오클루전의 근사치를 의미하며, 주로 Screen Space Ambient Occlusion (SSAO)라고 한다.

* 주변 조명을 음영 처리할 때 충실도(fidelity)를 높일 수 있지만, 지나치게 많은 폐색(오클루전)을 유발할 수 있다.
* [OpenGl 에서의 SSAO](/OpenGL/Kim/ssao.md)

### Screen Space Reflection

* SSAO와 마찬가지로, 현재 장면 뷰를 사용하여 광선 추적을 통해 reflection의 근사치를 계산한다.

* 매우 정확한 반사값을 추가하여 일반적인 큐브맵 캡쳐 반사를 보완한다.

* deferred rendering으로 렌더링을 제한하고 성능에 영향을 주며, 스크린 공간에 없는 것은 반사를 생성하지 않는다.

### Depth of Field

* 한 지점에 초점을 맞췄을 때 초점이 맞은 것으로 인식되는 범위를 뜻함.

* 영화적 느낌을 주는 경우 외에도, 틸트 쉬프트 카메라 렌즈가 미니어처 효과를 주는 방식과 같이 장면의 스케일 인식을 변경하는 데에도 사용한다.

### Motion blur

* 움직이는 물체를 흐릿하게 보여주는 기술.

* 한 프레임에서 다른 프레임으로의 전환을 혼합하는 데 큰 역할을 함.

### Bloom and emissive

* Bloom은 광선의 초점이 흐려지는 기능. 저품질 카메라 또는 특수 효과 광선 카메라 필터에서 보인다.

* 임계 값이 클 수록 블룸 효과가 엹어짐.

### ToneMapper type

* ToneMapper는 하이 다이내믹 레인지(HDR) 컬러의 큰 범위를 디스플레이가 출력할 수 있는 로우 다이내믹 레인지(LDR) 컬러의 작은 범위로 매핑시키는 방법을 의미.

* 포스트 프로세싱 도중 보통 렌더링 이후 벌어지는 포스트 프로세싱 마지막 단계.

* 유니티는 Neutral과 ACES(Academy colour Encoding System) 두 타입을 사용.

### Chromatic Aberration(색수차), Grain and Vignette

* 실제 카메라 시스템의 가공물을 시뮬레이션하는 포스트 프로세스 효과들.

* CA(Chromatic Aberration) effect
  * 카메라의 렌즈가 모든 빛을 동일한 수렴 지점에 맞추지 못하는 경우 이미지에 나타나는 색 분산을 의미.
  * 보통 보정이 잘 되지 않은 렌즈나 저품질 렌즈에서 발견됨.

* Film Grain (or Granularity)
  * 실제 사진이나 영화의 최종 이미지에서 보이는 먼지같은 효과를 말함.
  * 깨끗한 3d 렌더링 장면에 추가하여 실제적인 느낌을 주기 위해 사용.
  * 너무 많은 노이즈가 있으면 주의가 산만해지고 이미지의 대비가 영향을 받을 수 있음.

* Vignette
  * 이미지 가장자리의 밝기 또는 채도를 감소시켜 액자처럼 보이게 되는 효과.

----
## [Dynamically Lit Objects](https://unity3d.com/kr/learn/tutorials/projects/creating-believable-visuals/dynamically-lit-objects?playlist=17102)

> 동적 오브젝트의 조명 품질을 향상시키기 위해 고려해야 할 사항들을 확인한다.

### Light Probe Proxy Volume (LPPV)

* 라이트 프로브 데이터를 사용할 때, 큰 오브젝트에서 필요한 조명의 미세한 세분성을 구현할 수 있는 방법.

* 큰 동적 오브젝트에서 하나 이상의 라이트 프로브를 사용하여 조명 정확도를 높일 수 있다.

### Per object baked Ambient Occlusion Map (AO)

* 오브젝트의 폐색(오클루전)을 미리 계산해서 저장한 자료형.

* Per object AO는 다른 동적 오브젝트와 상호작용하지 않기 때문에 약간의 오차가 발생할 수 있다.

### Local Reflection

* 객체에 부착된 리플렉션 프로브를 통해 실시간으로 반사를 계산하여 환경 리플렉션 프로브로부터 오는 잘못된 반사 표현을 줄일 수 있다.

### Fake Shadows or occlusion based on assumptions

* 특정한 가정을 통해 가짜 그림자나 폐색을 만들어 넣는 기법.

* 동적 오브젝트 아래에 진짜 그림자 대신 블롭 그림자 프로젝터를 넣어 실시간 렌더링에 사용되는 비용을 아낄 수 있다.


## 참조

* [Learn OpenGL](https://learnopengl.com/Advanced-Lighting)
* [Unity](https://learn.unity.com/tutorial/creating-believable-visuals#5c7f8528edbc2a002053b54c)
* 