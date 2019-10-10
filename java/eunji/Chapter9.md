# Concurrency best practices
## Threads and Thread Groups
### 프로세스
* 사전적 정의 : 컴퓨터에서 연속적으로 실행되고 있는 컴퓨터 프로그램
* 운영체제로 부터 시스템 자원을 할당받는 작업의 단위
* 독립된 메모리 영역(stack, data, code, heap)을 할당받음
* 모든 프로세스는 하나 이상의 스레드를 가짐
### 스레드
* 사전적 정의 : 프로세스 내에서 실행되는 여러 흐름의 단위
* 프로세스가 할당받은 자원을 이용하는 실행의 단위
* 스레드는 프로세스 내에서 stack만 할당받고 code, data, heap 영역은 공유  
* lightweight processes
### Java Thread
* JVM안에서 존재(os thread를 반영하지 않을 수 있음)
* 자바 스레드는 Thread class의 인스턴스
* Thread class의 인스턴스를 사용해 직접 스레드를 만드는 것보다 executor나 threadpool을 이용해 만드는 방법 추천

```java
[java]
class ImplementTest implements Runnable {
	@Override
	public void run() {
		System.out.println("Thread implement");
	}
}

class ExtendTest extends Thread {
	public void run() {
		System.out.println("Thread extends");
	}
}

public static void main(String[] args) {
    ImplementTest a = new ImplementTest();
    new Thread(a).start();
    	
    ExtendTest b = new ExtendTest();
    b.start();
   
    new Thread( new Runnable() {
        @Override
        public void run() {
            System.out.println("thread");
        }
    }).start();
}

//결과
Thread implement
Thread extends

// 람다식 사용( Java 8 )
public static void main(String[] args) {
    new Thread( () -> { 
        System.out.println("thread");
     ).start();
}
```
```cs
[c#]
 static void Main() {
    static void Main(string[] args) {
        new Thread(new ThreadStart(() => {
            Console.WriteLine("thread Start");
        })).Start();

        new Thread(new ParameterizedThreadStart(ParamTest)).Start("Method");

        Thread t = new Thread(new ParameterizedThreadStart((obj) => {
                Console.WriteLine(obj);
            }));
        t.Start("Lambda");
    }

    static void ParamTest(object obj) {
        Console.WriteLine(obj);
    }
}

// 결과
thread Start
Method
Lambda
```
* 스레드는 아래 표의 상태 중 하나의 상태를 가짐

|Thread State|Description|
|---|:---|
|NEW|스레드 객체 생성, 아직 start() 메소드 호출 전|
|RUNNABLE| JVM 안에서 실행되는 스레드 상태 |
|BLOCKED|사용하고자 하는 객체의 락이 풀릴때까지 기다리는 상태|
|WAITING|다른 스레드가 통지할 때까지 기다리는 상태|
|TIMED_WAITING|주어진 시간동안 기다리는 상태|
|TERMINATED|실행을 마친 상태|
* Thread group : 스레드 세트, 다른 스레드 그룹도 포함 가능
    * 요즘은 스레드 풀이 좋은 대안
## Concurrency, Synchronization and Immutability
* `synchronized` : 동기화 키워드 제공
    * instance method, static method, 임의의 블록 만들어 사용 가능
    * 자동적으로 happens-before 관계 성립
    * happens-before : 한 사건이 다른 사건 전에 발생해야 하는 경우, 그러한 사건이 실제로 잘못 실행되더라도(일반적으로 프로그램 흐름을 최적화하기 위해) 그 결과를 반영해야 한다.
    * Constructor 에는 `synchronized` 키워드를 붙일 수 없음 : 인스턴스를 생성하는 쓰레드만 인스턴스가 생성되는 동안 접근할 수 있음
```java
public synchronized void performAction() {
}
public static synchronized void performClassAction() {
}

public void performActionBlock() {
    synchronized (this) {
    }
}
```
```java
[java]
class SynchronizedBlockTest {
	   private static int counter = 0;
	   public void print() {
		  synchronized(this) {
			counter++;
			System.out.println("count : " + counter);
		}
	}
}

public static void main(String[] args) {
    SynchronizedBlockTest b = new SynchronizedBlockTest();
    for(int i = 0; i < 10; i++) {
        new Thread(new Runnable() {
            public void run() {
                b.print();
            }
        }).start();
    }
}

// 출력
count : 1
count : 2
count : 3
count : 4
count : 5
count : 6
count : 7
count : 8
count : 9
count : 10
```
```cs
[csharp]
class SynchronizedBlockTest {
    private int counter = 0; 
    public void print() {
		lock(this) {
			counter++;
			Console.WriteLine("count : " + counter);
	    }
	}    
}

static void Main() {
    SynchronizedBlockTest b = new SynchronizedBlockTest();
    for (int i = 0; i < 10; i++) { 
        new Thread(b.print).Start(); 
    } 
}
```
* java에서 동기화는 `monitor`로 알려진 내부 개체를 중심으로 이루어짐
    * 스레드가 동기화로 선언된 메소드를 호출할 때, 스레드는 자동으로 해당 메소드의 인스턴스(or 정적 메소드의 경우 클래스)에 대한 모니터 잠금을 획득하고 해당 메소드가 반환되면 해제한다.
* java 동기화는 `reentrant`, 즉 쓰레드가 이미 소유하고 있는 잠금을 획득할 수 있음을 의미, 쓰레드가 블록될 확률이 적기 때문에 concurrency application의 프로그래밍 모델을 단순화함
```java
[java]
class Reentrant {
    public synchronized void a() {
	    b();
	    System.out.println("here I am, in a()");
    }
    public synchronized void b() {
	    System.out.println("here I am, in b()");
    }
}

public static void main(String[] args) {
    Reentrant r = new Reentrant();
    r.start();
}

// 출력
here I am, in b()                            
here I am, in a()
```
```cs
[csharp]
class SynchronizedBlockTest {
    public void a() {
        lock(this){
	        b();
	        Console.WriteLine("here I am, in a()");
        }
    }
    public void b() {
        lock(this){
	    Console.WriteLine("here I am, in b()");
        }
    }
}
```  
* `immutability` : 둘 이상의 쓰레드에 의해 업데이트되지 않으므로 불변 객체는 동기화를 필요로 하지 않음
 
## Futures, Executors and Thread Pools

* 자바 라이브러리는 쓰레드 관리를 단순화하기 위해 executors, thread pool 형태의 추상화를 제공
* Thread pool : 쓰레드 목록을 생성, 관리
    * 응용프로그램은 매번 새 쓰레드를 생성하는 대신 풀에서 빌려온 뒤 빌린 쓰레드가 작업을 끝내면 다시 풀로 돌려보냄
### executor
* 제공된 작업(Runnable 구현체)을 실행하는 객체가 구현해야할 인터페이스
* 작업 등록과 실행을 분리하는 표준적인 방법
```java
public interface Executor {
    void execute(Runnable command);
}
``` 
* 자바 라이브러리는 쓰레드풀 설정에 공통적으로 사용되는 메소드를 구현한 `ExecutorService` 라이브러리 제공
* ExecutorService : Executor의 라이프 사이클을 관리할 수 있는 기능을 정의

`ExecutorService executor = new Executors.newFixedThreadPool(10);`


라이프사이클 관련

|Method|Description|
|---|:---|
|void shutdown()|실행중지, 이미 Executor에 제공된 작업은 실행되지만 새로운 작업은 실행 안됨|
|List<Runnable> shutdownNow()|현재 실행중인 모든 작업 중지, 대기 중인 작업 목록 리턴|
|boolean isShutdown()| 셧다운되었는지 여부 확인|
|boolean isTerminated()|모든 작업이 종료되었는지 확인|
|boolean awaitTermination(lont timeout, TimeUnit unit)|지정한 시간동안 모든 작업이 종료될 때까지 대기, 지정한 시간내에서 모든 작업이 종료되면 true, 아니면 false|

작업 수행 관련

|Method|Description|
|---|:---|
|`<T>Future<T> submit(Callable<T> task)`|결과값을 리턴하는 작업 추가|
|`Future<?> submit(Runnable task)`|결과값이 없는 작업 추가|
|`void execute(Runnable task)`|미래에 어떤 특정 시점에 작업이 실행되도록 함|

* Executors 클래스 : Executor 인스턴스를 구할 수 있도록 메소드 제공

|Method|Description|
|---|:---|
|ExecutorService newFixedThreadPool(int nThreads)|최대 지정한 개수만큼의 쓰레드를 가질 수 있는 쓰레드풀 생성|
|ExecutorService newSingleThreadExecutor()|하나의 쓰레드만 사용하는 ExecutorService 생성|
|ExecutorService newCachedThreadPool()|필요할때마다 쓰레드를 생성하는 쓰레드풀 생성|

* Callable : Runnable과 비슷하지만 결과값을 리턴할 수 있음
```java
public interface Callable<V> {
    V call() throws Exception
}
```
* Future : executor(Callable)의 결과값을 저장
    * Callable.call()의 리턴값은 미래의 어느 시점에 구할 수 있다
```java
[java]
class MyThreadTask implements Runnable {	
	private static int count = 0;
	private int id;
	@Override
	public void run() {
		for(int i = 0; i < 5; i++) {
			System.out.println("<" + id + ">TICK TICK " + i);
			try {
				TimeUnit.MICROSECONDS.sleep((long)Math.random()*1000);
			} catch (InterruptedException e) {
				e.printStackTrace();
			}
		}
	}

	public MyThreadTask() {
		this.id = ++count;
	}
}

class CallTest implements Callable<Integer> {
    private static int count = 1;
    
    public Integer call() throws Exception {
        count++;
        return count;
    }
}

public static void main(String[] args) {
    System.out.println("Main thread starts here...");
        
    ExecutorService execService = Executors.newFixedThreadPool(2); 
    		
    execService.execute(new MyThreadTask());
    execService.execute(new MyThreadTask());

    Future<Integer> future1 = execService.submit(new CallTest());
    Future<Integer> future2 = execService.submit(new CallTest());

    Integer result1 = null;
    Integer result2 = null;
    try {
        result1 = future1.get();
        result2 = future2.get();
    } catch (InterruptedException e) {
        e.printStackTrace();
    } catch (ExecutionException e) {
        e.printStackTrace();
    }

    System.out.println("result1 : " + result1 + ", result2 : "+result2);
    
    execService.shutdown(); // 쓰레드 중지
        
    System.out.println("Main thread ends here...");
}

// 출력
Main thread starts here...
<1>TICK TICK 0
<2>TICK TICK 0
<1>TICK TICK 1
<2>TICK TICK 1
<2>TICK TICK 2
<2>TICK TICK 3
<1>TICK TICK 2
<2>TICK TICK 4
<1>TICK TICK 3
<1>TICK TICK 4
result1 : 2, result2 : 3
Main thread ends here...
```
```cs
[c#]
class MyThreadTask {
    private static int count = 0;
    private int id;

    public void run() {
        for (int i = 0; i < 5; i++) {
            Console.WriteLine("<"+id+">TICK TICK " +i);
            Random r = new Random();
            Thread.Sleep(r.Next(1000));
        }
    }

    public MyThreadTask() {
        id = ++count;
    }

    public int CallTest() {
        return id;
    }
}

static void Main(string[] args) {
    Console.WriteLine("Main thread starts here...");

    Task[] t = new Task[2];
    MyThreadTask[] my_thread = new MyThreadTask[2];

    for (int i = 0; i < 2; i++) {
        my_thread[i] = new MyThreadTask();
        t[i] = new Task(my_thread[i].run);
        t[i].Start();
    }

    Task<int> task1 = Task.Factory.StartNew(my_thread[0].CallTest);
    Task<int> task2 = Task.Factory.StartNew(my_thread[1].CallTest);

    int result1 = task1.Result;
    int result2 = task2.Result;

    Task.WaitAll(t);

    Console.WriteLine("result1 : " + result1 + ", result2 : " + result2);

    Console.WriteLine("Main thread ends here...");
}

// 결과
Main thread starts here...
<2>TICK TICK 0
<1>TICK TICK 0
<2>TICK TICK 1
<1>TICK TICK 1
<2>TICK TICK 2
<1>TICK TICK 2
<2>TICK TICK 3
<1>TICK TICK 3
<2>TICK TICK 4
<1>TICK TICK 4
result1 : 1, result2 : 2
Main thread ends here...
```
### Locks

* 자바는 reentrant mutual exclusion lock을 제공(monitor lock과 의미와 하는 일은 같지만 더 많은 기능 제공)
* java.util.concurrent.locks 패키지 안의 ReentrantLock 클래스를 이용
* lock을 걸면 unlock으로 항상 풀어줘야함
* trylock : 락을 선점한 스레드가 없을때만 락을 얻으려고 시도하는 메서드
```java
[java]
public void performActionWithTimeout() throws InterruptedException { 
    if( lock.tryLock( 1, TimeUnit.SECONDS ) ) { 
        try { 
            // Some implementation here 
        } finally { 
            lock.unlock(); 
        } 
    } 
}
```
```cs
[csharp]
public void performAction {
    Monitor.Enter(Object);
    try {

    } finally {
        Monitor.Exit(Object);
    }
}
```
* lock() 사용 -> WAITING 상태
* tryLock() 사용 -> TIMED_WAITING 상태
* 모니터, synchronized 사용 -> BLOCKED 상태

* 교착상태(두 개 이상의 작업이 서로 끝나기만을 기다리고 있어서 결과적으로 아무것도 완료되지 못하는 상태) 발생 가능

```java
private final ReentrantLock lock1 = new ReentrantLock();
private final ReentrantLock lock2 = new ReentrantLock();
	
public void performAction() {
	lock1.lock();
	try {
		// Some implementation here
		try {
			lock2.lock();
			// Some implementation here
		} finally {
			lock2.unlock();
		}			
        // Some implementation here
	} finally {
		lock1.unlock();	
	}
}
	
public void performAnotherAction() {
    lock2.lock();
    try {
        // Some implementation here
            try {
                lock1.lock();
                // Some implementation here
            } finally {
                lock1.unlock();
            }
            // Some implementation here
    } finally {
        lock2.unlock();
    }
}
```

### Thread Schedulers

* 스레드 스케줄러가 실행할 스레드와 실행 시간을 결정
* 자바 어플리케이션에 의해 생성된 모든 스레드는 기본적으로 스레드 스케줄링에 영향을 미치는 우선순위를 가진다
* Thread 클래스는 yield()를 사용해 스레드 스케줄링에 개입하는 방법 제공
    * 실행중인 스레드가 동일하거나 우선순위가 더 높은 스레드에게 순서 양보 
* java 스레드 스케줄러 구현 세부 사항에 의존하는 것은 좋은 방법이 아니기 때문에 java 표준 라이브러리는 세부사항을 개발자에게 노출시키지 않으려고 노력

### Atomic Operations

* compare-and-swap : 주어진 값과 메인 메모리의 값이 같다면 메인 메모리의 값을 새로운 값으로 변경, 새로운 값은 항상 최신의 정보임을 보장함
    * 값 비교 중 다른 스레드에 의해 그 값이 업데이트 된다면 쓰기는 실패
    * 연산의 결과는 쓰기가 제대로 이루어졌는지를 나타냄

|Class|Description|
|---|:---|
|AtomicBoolean|원자적으로 업데이트 될 수 있는 boolean 값|
|AtomicInteger|원자적으로 업데이트 될 수 있는 int 값|
|AtomicIntegerArray|요소가 원자적으로 업데이트 될 수 있는 int 배열|
|AtomicLongArray|요소가 원자적으로 업데이트 될 수 있는 long 배열|
|AtomicReference<V>|원자적으로 업데이트 될 수 있는 오브젝트 참조|
|AtomicReferenceArray<E>|요소가 원자적으로 업데이트 될 수 있는 오브젝트 참조 배열|
|DoubleAccumulator|제공된 함수를 사용하여 업데이트된 실행중인 double 값을 유지하는 하나 이상의 값|
|DoubleAdder|처음에 0을 더해 값을 유지하는 하나 이상의 값|
|LongAccumulator|제공된 함수를 사용하여 업데이트된 실행중인 long 값을 유지하는 하나 이상의 값|
|LongAdder|처음에 0을 더해 값을 유지하는 하나 이상의 값|

```java
public static void main(String[] args) {
    AtomicInteger atomicInt = new AtomicInteger(0);
    AtomicIntegerArray atomicArray = new AtomicIntegerArray(1000);
    
    ExecutorService executor = Executors.newFixedThreadPool(1);

    IntStream.range(0, 1000)
    .forEach(i -> {
        Runnable task = () ->
            atomicInt.updateAndGet(n -> n + 2);
            atomicArray.addAndGet(i, 1);
        executor.submit(task);
    });
    
    
    executor.shutdown();
    try {
        executor.awaitTermination(60, TimeUnit.SECONDS);
    } catch (InterruptedException e) {
        e.printStackTrace();
    }
    
    System.out.println(atomicInt.get());
    System.out.println(atomicArray.length());
}
}

// 출력
2000
1000
```

## Concurrent Collections

* 자바 라이브러리는 collections 클래스에 thread-safe를 가능하게 하는 static method 함수 제공
```java
final Set<String> strings = Collections.synchronizedSet(new HashSet<String>());
final Map<String, String> keys = Collections.synchronizedMap(new HashMap<String, String>());
```
```java
private static AtomicInteger atomicInteger = new AtomicInteger();
public static void main(String[] args) throws InterruptedException {
    	
    Set<Integer> s = new HashSet<>();
    Set<Integer> set = Collections.synchronizedSet(s);
    System.out.println("initial set size: " + set.size());

    final ExecutorService e = Executors.newFixedThreadPool(10);
    for (int i = 0; i < 10; i++) {
    e.execute(() -> set.add(atomicInteger.incrementAndGet()));
    }
    e.shutdown();
    e.awaitTermination(1000, TimeUnit.SECONDS);
    
    Iterator<Integer> i = set.iterator();
    
    while(i.hasNext()) {
        System.out.println(i.next());
    }
}

// 출력
initial set size: 0
1
2
3
4
5
6
7
8
9
10
```
* Concurrency를 위해 튜닝된 collection 클래스

|Class|Description|
|---|:---|
|CountDownLatch|하나 이상의 스레드가 다른 스레드에서 수행되는 작업이 완료 될 때까지 대기하도록 허용하는 동기화 지원 도구|
|CyclicBarrier|스레드 세트가 서로를 기다려 공통의 포인트에 도달할 수 있게 해주는 동기화 지원 도구|
|Exchanger<V>|스레드가 쌍을 이루고 요소를 교환할 수 있게 하는 동기화 지점|
|Phaser|재사용 가능한 동기화 barrier로써, CountDownLatch나 CyclicBarrier보다 유연한 사용법 지원|
|Semaphore|카운팅 세마포어|
|ThreadLocalRandom|현재 스레드와 격리된 난수 생성기|
|ReentrantReadWriteLock|읽기/쓰기 잠금 구현|
#### CountDownLatch 예제
```java
private final static int THREADS = 3;
private static CountDownLatch latch = new CountDownLatch(THREADS);
    
public static class RandomSleepRunnable implements Runnable {
    private int id = 0;
    private static Random random = new Random(System.currentTimeMillis());
    	
    public RandomSleepRunnable(int id) {
        this.id = id;
    }
    	
    @Override
    public void run() {
        System.out.println("Thread("+id+") : Start.");
        int delay = random.nextInt(1001) + 1000;	
        try {
            System.out.println("Thread("+id+") : Sleep "+delay+"ms");
            Thread.sleep(delay);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
        System.out.println("Thread(" + id + ") : End.");
        latch.countDown();
    }
}

public static void main(String[] args) {
    for(int i = 0; i < THREADS; i++) {
        new Thread(new RandomSleepRunnable(i)).start();
    }
    try {
    latch.await();
} catch (InterruptedException e) {
    e.printStackTrace();
}
    
    System.out.println("All threads terminated.");
}

// 출력
Thread(1) : Start.
Thread(1) : Sleep 1945ms
Thread(2) : Start.
Thread(2) : Sleep 1910ms
Thread(0) : Start.
Thread(0) : Sleep 1680ms
Thread(0) : End.
Thread(2) : End.
Thread(1) : End.
All threads terminated.
```
#### 세마포어 예제
```java
[java]
class SemaphoreDemo {
    Semaphore semaphore = new Semaphore(3);
    public void printLock() {
        try {
           semaphore.acquire();
            System.out.println("Locks acquired");
            System.out.println("Locks remaining >> "+semaphore.availablePermits());
		} catch (InterruptedException e) {
			e.printStackTrace();
		} finally {
			semaphore.release();
			System.out.println("Locks Released");
		}
    }
}

public static void main(String[] args) {
    SemaphoreDemo d = new SemaphoreDemo();
    for(int i = 0; i < 5; i++) {
        new Thread(new Runnable() {
            public void run() {
                d.printLock();
            }
        }).start();
    }
}

// 출력
Locks acquired
Locks remaining >> 0
Locks Released
Locks acquired
Locks remaining >> 0
Locks Released
Locks acquired
Locks remaining >> 0
Locks Released
Locks acquired
Locks remaining >> 1
Locks Released
Locks acquired
Locks remaining >> 2
Locks Released
```
```cs
[csharp]
class SemaphoreDemo {	
	private Semaphore sema;
	
	public SemaphoreDemo(){
	    sema = new Semaphore(3, 3);
	}
	
	public void Print(){
	    sema.WaitOne();
	    
	    Console.WriteLine("Locks acquired");
	    Console.WriteLine("before count >> {0}", sema.Release());
	    Console.WriteLine("Locks Released");
	}
}

static void Main() {
    SemaphoreDemo d = new SemaphoreDemo();
    for (int i = 0; i < 5; i++) {
            new Thread(d.Print).Start(i);
    }
}

// 출력
Locks acquired  
before count >> 0 
Locks Released   
Locks acquired  
before count >> 0 
Locks Released  
Locks acquired  
before count >> 1 
Locks Released    
before count >> 0  
Locks Released    
Locks acquired    
before count >> 2  
Locks Released     
```
### Using Synchronization Wisely
* synchronization을 현명하게 하지 않으면 응용 프로그램의 성능을 저하시킬 수 있고, 데이터 손상을 초래할 수 있으므로 균형이 중요함
* lock, synchronized는 실제로 필요할때만 사용(최소한 실행 블록을 작게 만듦)
* 최근에는 많은 lock-free로 불리는 알고리즘과 data structure(ex) atomic operations)이 나오고 있음
* JVM에는 필요하지 않을 때 잠금을 제거하기 위한 몇가지 런타임 최적화가 있음
    * biased locking : 동일 thread가 연속적으로 critical section에 접근하는 경우 atomic operation을 수행하지 않는 lock을 제공하여 수행 속도를 향상시키는 기법

### Wait/Notify

* wait() : 현재 스레드가 보유하고 있는 모니터 잠금 해제, 같은 모니터에서 대기 중인 다른 스레드가 실행될 수 있음
    * wait()은 루프 내에서 호출되어야함
* notify() : 스레드가 완료되면 모니터 잠금을 기다리는 스레드를 깨우는 함수
    * notifyAll() : 모니터 잠금을 기다리는 스레드 모두 깨움
* Wait, Notify, NotifyAll은 모두 Synchronized 블록 안에서 실행되어야함
* wait() / notify() 는 복잡할뿐만 아니라 필수 규칙을 따라야 하기 때문에 좋지 않음

```java
private Object lock = new Object();
public void performAction() { 
    synchronized( lock ) { 
        while( <condition> ) { 
            // Causes the current thread to wait until 
            // another thread invokes the notify() or notifyAll() methods. 
            lock.wait(); 
        }
        // Some implementation here
    }
}

public void performAnotherAction() { 
    synchronized( lock ) { 
        // Some implementation here
        
        // Wakes up a single thread that is waiting on this object’s monitor. 
        lock.notify();
    }
}

```
### Troubleshooting Concurrency Issues

* JDK에는 응용 프로그램 스레드 및 해당 상태에 대한 세부 정보를 제공하고 deadlock 조건을 진단할 수 있는 몇 가지 도구가 포함되어 있음
* JVisualVM
* Java Mission Control
* jstack