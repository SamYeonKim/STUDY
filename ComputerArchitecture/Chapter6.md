* Kernel Object : 커널에 의해 관리되는 리소스 정보를 담고 있는 데이터 블록
  * Kernel : OS의 핵심 부분
  * Resource : OS에 의해 생성 및 소멸되는 것들 ( Pipe, Process, Thread ... )
    * ex) Process가 생성되면 Process를 위한 Kernel Object가 만들어 진다.
  * Handle : Kernel Object에 직접적 접근을 할 수 없기 때문에, Kernel Object의 특정 값을 변경 하기 위해, OS에게 요청할 수 있는 KernelObject의 구분자
    * Handle 값은 각 Process 별로 종속적이다.
    * 각 Process 별로 Handle Table을 가지고 있다.
  * Usage Count를 가지고 있다.
    * UC는 Object를 생성한 본 프로세스, 그리고 현재 프로세스를 만든 부모 프로세스에 의해서 기본 2로 할당된다, 예외적으로 파일과 같은건 파일을 만든 프로세스에 의해 1이 할당
    * 프로세스 A가 프로세스 B를 생성한다면, 프로세스 A의 Handle Table에 B의 Kernel Object의 참조 데이터가 추가 된다. 이때 B의 Kernel Object의 UC는 2    
      * 만약 프로세스 B가 소멸 된다면, B의 Kernel Object의 UC는 1이 된다.
      * B의 Kernel Object는 A가 OS에게 더이상 B의 Kernel Object를 사용하지 않겠다는 언급이있을때 B의 KernelObject의 UC를 0으로 만들고 소멸 시킨다.     
* Process의 우선순위에 따라서 Scheduler에 의해 Running or Ready 상태가 되는데, 우선순위가 낮은 Process는 언제든 Ready 상태로 될 수 있다.
  * Code상 함수 기준이 아닌 명령어에 따라 흐름이 달라진다.
    * ex) A proc이 println ("Hello World") 이고, B proc이 A보다 우선순위가 높고, println은 30개의 명령어로 구성됬다면, 다음과같은 흐름이 될수 있다.
      * B cmd 0 -> B cmd 1 -> B cmd 2 -> A cmd -> 0 ( print ( "H" ) ) -> B cmd 3 -> B cmd 4 -> A cmd -> 1 (print("e"))
* Program은 하나 이상의 Process로 구성 될 수 있다.
* Process 간 통신 : 메모리 공유
* 각 Process는 안정성을 위해 할당 받은 메모리를 제외한 다른 메모리에 접근을 하지 못하도록 OS에 의해 통제된다.


      
     


  
  
  
