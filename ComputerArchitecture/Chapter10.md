* StackFrame
  * 특정함수에서 선언한 로컬 변수 및 함수의 인자 저장
* Stack Pointer Register가 다음에 쌓을 StackFrame의 주소를 기억
* Frame Pointer Register가 함수 호출 이후, 다시 함수 호출 이전 상태로 돌아갈 현재 StackPointer의 주소를 기억
  * 하나의 주소만 기억
  * 재귀함수 처럼 반복된 함수호출이 될 경우, 기본에 기억 하고 있던 주소를 덮어 쓰게 되기 때문에, 기존에 기억하고 있던 주소 값을 StackFame에 추가 하고,
    StackPointer를 하나 증가 시킨다.
