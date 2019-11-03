- [Drawcall](#drawcall)
- [Batch](#batch)
- [Batching](#batching)
  - [Static Batching](#static-batching)
  - [Dynamic Batching](#dynamic-batching)
- [참조](#%ec%b0%b8%ec%a1%b0)

## Drawcall

* CPU가 GPU에게 `그려라` 라는 명령을 하는것
* 마지막 전달한 `RenderState`를 이용해서 그리도록 하는것

## Batch

* CPU가 GPU에게 그려야 할 것에 대한 정보를 전달 하는것
* `Render State`를 전달
  * 버텍스 정보
  * 텍스쳐
  * 쉐이더 정보 등등
  * SetPass Call 이라고 함
* 횟수가 많을 수록 CPU에 부담
  * 이전 `Render State`와 다른 정보를 전달할때 횟수가 증가

## Batching

* `SetPass Call` 을 줄이기 위한 최적화 방법
* 동일한 `Material`을 사용하는 것들만 가능
  * 각각의 텍스쳐를 사용하는 경우엔, 하나의 텍스쳐로 뭉쳐야 한다

### Static Batching

* 정적인 `GameObject`들에 대해서 `Batching`을 해준다.
* `GameObject`는 고정된 위치, 회전, 크기를 가져야 한다. 즉 아무짓도 해선 안된다.
* `GameObject`의 `Inspector`에서 `Static` 옵션을 켜야만 한다.
* Unity는 위 조건에 맞는 `GameObject`들의 정보를 하나로 뭉친다.
  * 뭉친 정보는 `Vertex Buffer`, `Index Buffer`들이고, 추가 메모리를 잡아 먹는다.

### Dynamic Batching

* 아래 조건을 만족할 경우 `Dynamic Batching`이 된다.
  * 900개 미만의 버텍스 속성과 300개 미만의 버텍스가 포함된 메시
  * `MultiPass`를 사용하지 않는 쉐이더를 사용
  * 동일한 크기여야 한다.
  * 그려지는 순서가 동일 해야한다.

## 참조

* [Manual](https://docs.unity3d.com/Manual/DrawCallBatching.html)
* [Unity3d Tips](https://www.unity3dtips.com/unity-batching-dynamic-vs-static/)
* [한글 좋아](http://rapapa.net/?p=2694)



