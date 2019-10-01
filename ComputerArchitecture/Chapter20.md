* 가상 메모리 Control
  * 가상 메모리는 3개의 상태를 갖는다.
    * Commit : 물리 메모리와 연결된 상태 ( 메모리 할당 된 상태 )
    * Reserve : 물리 메모리와 연결은 안되어 있고, 나중에 사용할 것으로 체크해 둔 상태
    * Free : 물리 메모리와 연결되지 않은 상태 ( 메모리 할당이 안된 상태)
  * 메모리 할당은 Page단위로 이루어 진다. 이때 할당될 시작점은 Allocation Granularity Boundary 기준으로 설정
    * 메모리 할당을 할 땐 필요한 양의 Page보다 약간 더 크게 할당 한다
      * 단편화를 막기 위해서
* Heap 메모리 Control
  * Default Heap : OS에 의해서 Process에게 기본적으로 할당되는 Heap
  * Dynamic Heap : 개발자에 의해서 추가 적으로 할당되는 Heap

    
