# Threading

* Objective-c 는 멀티쓰레딩을 지원함 -> 두 개 이상의 쓰레드가 동시에 동일한 객체를 수정하려고 시도할 수 있음(critical section)
* @synchronized() 제공 : 한번에 둘 이상의 스레드에서 코드 섹션이 실행되지 않도록 함

### 프로세스
* 사전적 정의 : 컴퓨터에서 연속적으로 실행되고 있는 컴퓨터 프로그램
* 운영체제로 부터 시스템 자원을 할당받는 작업의 단위
* 독립된 메모리 영역(stack, data, code, heap)을 할당받음
### 스레드
* 사전적 정의 : 프로세스 내에서 실행되는 여러 흐름의 단위
* 프로세스가 할당받은 자원을 이용하는 실행의 단위
* 스레드는 프로세스 내에서 stack만 할당받고 code, data, heap 영역은 공유  

## @synchronized

* 한 코드가 해당 섹션(critical section)을 수행하고 있을 경우 섹션의 코드 수행을 종료할 때 까지 다른 스레드는 블로킹됨
* synchronized의 argument는 self or objective-c object(mutex or shmaphore).

    * mutex : 하나의 스레드만 critical section에 진입 가능
    * semaphore : 하나 이상의 스레드가 critical section에 진입 가능

* * *

### Locking a method using self
#### Objective-C
```objc
- (void) CreticalMethod{
    @synchronized(self){
        // Critical code.
    }
}
```
#### C#
```cs
 public void CreticalMethod() {
     lock(this){
         //Critical code.
     }
}
```
* * * 
### Locking a method using a Mutex

#### cs
```cs
class MyClass
    {
        private Mutex mutex = new Mutex(false);

        public void Run(object seq)
        {
            Console.WriteLine(seq);

            // 최대 1개 스레드만 아래 문장 실행
            mutex.WaitOne();

            Console.WriteLine("Running#" + seq);
            Thread.Sleep(500);

            // Mutex 해제. 
            // 이후 다음 스레드 WaitOne()에서 진입 가능
            mutex.ReleaseMutex();

        }
    }
```

### Locking a method using a custom semaphore
```objc
Account * account = [Account accountFromString:[accountFiled stringValue]];

//Get the semaphore
id accountSemaphore = [Account semaphore];

@synchronized(accountSemaphore){
    // Critical code
}
```
#### C#
```cs
 class MyClass
    {
        private Semaphore sema;

        public MyClass()
        {
            // 5개의 스레드만 허용
            sema = new Semaphore(5, 5);
        }

        public void Run(object seq)
        {
            Console.WriteLine(seq);

            // 최대 5개 스레드만 아래 문장 실행
            sema.WaitOne();

            Console.WriteLine("Running#" + seq);
            Thread.Sleep(500);

            // Semaphore 1개 해제. 
            // 이후 다음 스레드 WaitOne()에서 진입 가능
            sema.Release();

        }
    }
```
* @synchronized() block 에서 예외 발생시, Objective-c runtime은 예외를 캐치하고 다른 스레드가 protect code에 접근해야 하므로 semaphore release를 실행, 다음 exception handler에게 예외 throw.

# Q

* ~~프로세스와 스레드의 차이는?~~
* c# 과 비교
* 뮤텍스, 세마포어 예제
* 세마포어 사용코드, 여러쓰레드가 사용하는 코드