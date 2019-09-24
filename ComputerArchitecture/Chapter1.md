* CPU
  * ALU
    * 실제 연산 수행
  * Register
    * ALU 나 ControlUnit 으로 정보를 보내기전 임시저장소
  * Bus Interface
    * CPU와 다른 외부 장치와의 신호 전달을 보내고, 받기 위함
  * Control Unit
    * CPU전체 총괄 신호 전달
    * ALU, Register, BusInterface 와 신호 전달 함.
    * 어떻게 CPU가 일을 할것인지를 결정
* 프로그램 실행 과정
  * 전처리기 -> 컴파일러 -> 어셈블러 -> 링커
* Stored Program Concept
  * Fetch : CPU내부로 명령어 이동
  * Decode : 명령어 해석, Control Unit
  * Execution : 연산을 진행, ALU
