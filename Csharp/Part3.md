## Events

* `Windows Form`에서 UI 개발의 핵심개념
* `callback`의 특별한 형태
  * `event` 키워드를 사용
  * 할당이 불가능하고 추가, 제거만 가능
  * 제거 시 대응 함수가 없으면 예외는 발생하지 않지만, 실제로 핸들러를 제거한다고 생각해서 예기치 않은 동작이 발생할 수 있다.
* 여러 메소드를 추가할 수 있다.
  * `Delegate` 클래스의 [Combine](https://docs.microsoft.com/ko-kr/dotnet/api/system.delegate.combine?view=netframework-4.8) 메소드를 사용하여 여러 delegate를 묶는다.
  * 이 때, 컴파일러가 [CompareExchange](https://docs.microsoft.com/ko-kr/dotnet/api/system.threading.interlocked.compareexchange?view=netframework-4.8) 명령으로 `lock` 문을 삽입하여 `thread-safe`하게 만든다.

```cs
class Program {
   static void Main(string[] args) {
      Application.callback += () => Console.WriteLine("Number hit");
      Application.Run();
   }
}

static class Application {
   public static event Action callback;

   public static void Run() {
      Random r = new Random(14);

      while (true) {
            double p = r.NextDouble();

            if (p < 0.0001 && callback != null)
               callback();
            else if (p > 0.9999)
               break;
      }
   }
}
```

## The .NET standard event pattern

* `.NET`에서 제공하는 이벤트 처리 패턴
* 이벤트 송신자는 핸들러가 있는지 없는지 알 필요가 없다.
* 이벤트의 송신자를 구분하기 위해 첫 번째 인수를 이벤트 송신자로 지정한다.
  * `.NET`은 `object`를 첫 번째 파라미터로 받는다.
* 두번째 파라미터는 현재의 변수 / 상태를 전송하는 객체를 사용한다.

```cs
delegate void EventHandler(object sender, EventArgs e);
```

## Reflection

* 어셈블리와 관련된 메타 데이터 정보 집합에 접근하는 방법
  * 메타 데이터에는 정의된 유형, 메소드에 대한 정보가 포함된다.
* 사용방법은 여러가지가 있지만 3개만 본다.
  * 인스턴스의 타입 가져오기 - GetType()
  * 컴파일 타임에 `Type` 인스턴스 취득 - typeof()
  * `Assembly` 클래스를 사용하여 어셈블리를 로드 
* 아래의 팩토리 메소드 예제는 리플렉션 사용의 장점을 보여준다.
  * 코드는 길어지지만, 유지 관리가 편리하다.

```cs
using System.Reflection;

class Program {
   static void Main(string[] args) {
      var test = Document.CreateElement("img");
      Console.WriteLine("test : " + test.GetType());
      test.Print();
      var test2 = Document2.CreateElement("img");
      Console.WriteLine("test : " + test.GetType());
      test2.Print();
   }
}

class HTMLElement {
   string m_tag;

   public HTMLElement(string tag) {
      m_tag = tag;
   }

   public string Tag {
      get { return m_tag; }
   }

   public virtual void Print() {
      Console.WriteLine("HTMLElement, tag : " + m_tag);
   }
}

class HTMLImageElement : HTMLElement {
   public HTMLImageElement() : base("img") {
   }

   public override void Print() {
      Console.WriteLine("HTMLImageElement, tag : " + Tag);
   }
}

class HTMLParagraphElement : HTMLElement {
   public HTMLParagraphElement() : base("p") {
   }

   public override void Print() {
      Console.WriteLine("HTMLParagraphElement, tag : " + Tag);
   }
}

// 리플렉션 x
static class Document {
   public static HTMLElement CreateElement(string tag) {
      switch (tag) {
         case "img":
         return new HTMLImageElement();
         case "p":
         return new HTMLParagraphElement();
         default:
         return new HTMLElement(tag);
      }
   }
}

// 리플렉션 o
static class Document2 {
   static Dictionary<string, ConstructorInfo> specialized;

   public static HTMLElement CreateElement(string tag) {
      if (specialized == null) {
         specialized = new Dictionary<string, ConstructorInfo>();
         // 현재 실행중인 메소드의 어셈블리를 가져와 저장된 모든 타입을 반환 (that includes those HTMLElement types)
         Type[] types = Assembly.GetCallingAssembly().GetTypes();
         
         foreach (var type in types) {
            // HTMLElement이거나 상속 받았는지 확인
            if (type.IsSubclassOf(typeof(HTMLElement)) || type == typeof(HTMLElement)) {
               // 파라미터가 없는 생성자를 서치
               ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
               
               if (ctor != null) {
                  // 생성자 호출 후 인스턴스를 변수에 저장
                  var element = ctor.Invoke(null) as HTMLElement;

                  // 딕셔너리에 인스턴스 저장
                  if (element != null)
                     specialized.Add(element.Tag, ctor);
               }
            }
         }
      }

      // 저장된 생성자를 실행한다.
      if (specialized.ContainsKey(tag))
            return specialized[tag].Invoke(null) as HTMLElement;

      //Otherwise this is an object without a special implementation; we know how to handle this!
      return new HTMLElement(tag);
   }
}

// output
test : Facade.HTMLImageElement
HTMLImageElement, tag : img
test : Facade.HTMLImageElement
HTMLImageElement, tag : img
```

* 리플렉션을 사용하는 또다른 예시
* `anonymous object`를 사용하는 한가지 방법은 `var` 키워드로 초기화하는 것이다.
* 위의 객체의 멤버에 액세스할 수 있는 방법은 3가지 이다. 3번째를 시도한다.
  * `anonymous type`은 사용하지 않지만, 데이터 캡슐화를 하는 클래스를 만든다.
  * 다음 절의 `Dynamic Language Runtime`을 사용한다.
  * 호출자 메소드의 파라미터를 object 유형으로 변경하고 리플렉션을 사용한다.

```cs
class Program {
   static void Main(string[] args) {
      VarTest t = new VarTest();
      t.CreateObject();
   }
}

class VarTest {
   public void CreateObject() {
      var person = new { Name = "Florian", Age = 28 };
      AnalyzeObject(person);
   }

   public void AnalyzeObject(object o) {
      Type type = o.GetType();
      PropertyInfo[] properties = type.GetProperties();

      foreach (var property in properties) {
            // 프로퍼티 이름
            string propertyName = property.Name;
            // 프로퍼티 타입의 이름
            string propertyType = property.PropertyType.Name;
            // 해당 인스턴스에서 프로퍼티의 값
            object propertyValue = property.GetValue(o);
            Console.WriteLine("{0}\t{1}\t{2}", propertyName, propertyType, propertyValue);
      }
   }
}

// output 
Name    String  Florian
Age     Int32   28
```

* `static property`는 아래와 같이 인스턴스로 null을 전달하면 값을 가져올 수 있다.

```cs
var propertyInfo = typeof(MyClass).GetProperty("Instances");
var value = propertyInfo.GetValue(null);
```

* 메소드 정보를 얻는 것은 프로퍼티 정보를 얻는것과 비슷하다.
* [PropertyInfo](https://docs.microsoft.com/ko-kr/dotnet/api/system.reflection.propertyinfo?view=netframework-4.8), [MethodInfo](https://docs.microsoft.com/ko-kr/dotnet/api/system.reflection.methodinfo?view=netframework-4.8), [ConstructorInfo](https://docs.microsoft.com/ko-kr/dotnet/api/system.reflection.constructorinfo?view=netframework-4.8)는 모두 `MemberInfo`를 상속한다.
* `MethodInfo`, `ConstructorInfo`는 `MethodBase`를 상속한다. `MethodBase`는 `MemberInfo`를 상속한다.

```cs
class Program {
   static void Main(string[] args) {
      VarTest t = new VarTest();
      t.CreateObject();
   }
}

class VarTest {
   public void CreateObject() {
      var person = new { Name = "Florian", Age = 28 };
      AnalyzeObject(person);
   }

   public void AnalyzeObject(object o) {
      Type type = o.GetType();
      MethodInfo[] methods = type.GetMethods();

      //Iterate over all methods
      foreach (var method in methods) {
            string methodName = method.Name;
            string methodReturnType = method.ReturnType.Name;
            Console.WriteLine("{0}\t{1}", methodName, methodReturnType);
      }
   }
}

// output
get_Name        String
get_Age Int32
Equals  Boolean
GetHashCode     Int32
ToString        String
GetType Type
```

* 컴파일러는 프로퍼티를 메소드로 변환한다. 모든 프로퍼티는 메소드이다. (get_Name, get_Age)
* `GetProperty`, `GetProperties` 메소드는 모든 메소드를 찾지 않고 간단하게 특정 메소드에 접근할 수 있는 방법이다.

## Dynamic Types

* `anonymous object` 의 멤버에 액세스할 수 있는 방법 중 하나
* `dynamic` 타입을 사용한다.
* 컴파일러에게 오류를 무시하고 런타임에 맵핑하도록 지시한다.

```cs
class Program {
   static void Main(string[] args) {
      Test t = new Test();
      t.CreateObject();
   }
}

class Test {
   public void CreateObject() {
      var person = new { Name = "Florian", Age = 28 };
      UseObject(person);
   }
   public void UseObject(dynamic o) {
      Console.Write("The name is . . . ");
      Console.WriteLine(o.Name);
   }
}

// output
The name is . . . Florian
```

* 모든 표준 [CLR](https://docs.microsoft.com/ko-kr/dotnet/standard/clr) 객체를 `dynamic type`으로 처리할 수 있다.
* 아래의 예시에서 나오는 네 변수는 다 다르다.

```cs
int a = 1;        // type Int32
var b = 1;        // type Int32 (inferred)
dynamic c = 1;    // only known at runtime (but will be Int32)
object d = 1;     // type object, but the actual type is Int32

var a2 = a + 2;   // works, Int32 + Int32 = Int32
var b2 = b + 2;   // works, Int32 + Int32 = Int32
var c2 = c + 2;   // works, dynamic + Int32 = dynamic
var d2 = d + 2;   // 오류 CS0019  '+' 연산자는 'object' 및 'int' 형식의 피연산자에 적용할 수 없습니다.

a = "hi";         // compilation errors
b = "hi";         // compilation errors
c = "hi";         // Works!
d = "hi";         // Works!
```

* `dynamic type`은 컴파일 단계에서 에러가 나지 않기 때문에 사용에 주의가 필요하다.

```cs
dynamic a = "32";
var b = a * 5;

// output
처리되지 않은 예외: Microsoft.CSharp.RuntimeBinder.RuntimeBinderException: '*' 연산자는 'string' 및 'int' 형식의 피연산 자에 적용할 수 없습니다.
```

* `dynamic type`은 매핑 기능에서 정적 타이핑보다 더 정확하게 동작한다.

```cs
var a = (object)2;//This will be inferred to be Object
dynamic b = (object)2;//This is dynamic and the actual type is Int32
Take(a); //Received an object
Take(b); //Received an integer

void Take(object o) {
   Console.WriteLine("Received an object");
}

void Take(int i) {
   Console.WriteLine("Received an integer");
}

// output
Received an object
Received an integer
```

* [Dynamic Language Runtine](https://docs.microsoft.com/ko-kr/dotnet/framework/reflection-and-codedom/dynamic-language-runtime-overview) : 동적 객체를 바인딩 하는 계층
* `DLR`은 특별한 종류의 인터페이스를 구현하여 유형을 정의한다.
* [ExpandoObject](https://docs.microsoft.com/ko-kr/dotnet/api/system.dynamic.expandoobject?view=netframework-4.8), [DynamicObject](https://docs.microsoft.com/ko-kr/dotnet/api/system.dynamic.dynamicobject?view=netframework-4.8)는 해당 인터페이스를 구현한 클래스이다.
  * `ExpandoObject`는 런타임에 멤버를 추가 및 제거하지만 특정 작업을 정의할 필요가 없고, 정적 멤버가 없을 때 사용.
  * 상효운용성 프로토콜에 참여하는 방법을 정의하거나, 동적 디스패치 캐싱을 관리하는 것과 같은 고급 시나리오가 있는 경우 `IDynamicMetaObjectProvider` 인터페이스를 구현하여 사용.
* 객체를 매우 쉽게 확장할 수 있다.

```cs
using System.Dynamic;

class Program {
   static void Main(string[] args) {
      dynamic person = new Person();
      person.Name = "Florian";
      person.Age = 28;
      Console.WriteLine(person);
      person.Country = "Germany";
      Console.WriteLine(person);
   }
}

class Person : DynamicObject {
   //This will be responsible for storing the properties
   Dictionary<string, object> properties = new Dictionary<string, object>();

   public override bool TryGetMember(GetMemberBinder binder, out object result) {
      //This will get the corresponding value from the properties
      return properties.TryGetValue(binder.Name, out result);
   }

   public override bool TrySetMember(SetMemberBinder binder, object value) {
      //binder.Name contains the name of the variable
      properties[binder.Name] = value;
      return true;
   }

   public Dictionary<string, object> GetProperties() {
      return properties;
   }

   public override string ToString() {
      //Our object also has a specialized string output
      StringBuilder sb = new StringBuilder();
      sb.AppendLine("--- Person attributes ---");

      foreach (var key in properties.Keys) {
            //We use the chaining property of the StringBuilder methods
            sb.Append(key).Append(": ").AppendLine(properties[key].ToString());
      }

      return sb.ToString();
   }
}

// output
--- Person attributes ---
Name: Florian
Age: 28

--- Person attributes ---
Name: Florian
Age: 28
Country: Germany
```

## Accessing the file system

* 디렉토리, 파일, 드라이브의 정보를 가져올 수 있다.
* `DirectoryInfo`, `FileInfo`, `DriveInfo`

```cs
using System.IO;

class Program {
   static void Main(string[] args) {
      Test t = new Test();
      t.Print();
   }
}

class Test {
   public void Print() {
      string path = @"C:\Windows";
      string[] files = Directory.GetFiles(path);
      DriveInfo[] drives = DriveInfo.GetDrives();
      
      foreach (var file in files) {
            FileInfo fi = new FileInfo(file);
            Console.WriteLine(fi.FullName);
      }
      
      foreach (var drive in drives) {
            Console.WriteLine(drive.Name);
      }
   }
}

// output
C:\Windows\mib.bin
C:\Windows\notepad.exe
..
C:\
D:\
E:\
..
```

## Streams

* 입력 및 출력 동작은 스트림으로 관리된다.
* 메모리, 파일, 입출력 등의 스트림이 존재한다.

```cs
using System.IO;

class Program {
   static void Main(string[] args) {
      Test t = new Test();
      t.Print();
   }
}

class Test {
   public void Print() {
      FileStream fs = new FileStream("test.txt", FileMode.Create);
      byte[] arr = new byte[] { (byte)'a', (byte)'b', (byte)'c'};
      fs.Write(arr, 0, arr.Length);
      fs.Close();

      fs = new FileStream("test.txt", FileMode.Open);
      int val;
      while ((val = fs.ReadByte()) != -1) {
            Console.WriteLine("Could read some more byte! : " + val);
      }

      fs.Position = 0;
      byte[] firstTenBytes = new byte[5];
      while (fs.Read(firstTenBytes, 0, firstTenBytes.Length) != 0) {
            foreach (byte b in firstTenBytes)
               Console.WriteLine("Could read some more bytes! : " + b);
      }
      fs.Close();
   }
}

// output 
Could read some more byte! : 97
Could read some more byte! : 98
Could read some more byte! : 99
Could read some more bytes! : 97
Could read some more bytes! : 98
Could read some more bytes! : 99
Could read some more bytes! : 0
Could read some more bytes! : 0
```

* `TextReader`, `TextWriter` 클래스는 `Stream`을 상속받지 않고 따로 구현되어 있다. - 인코딩 때문에
* `StreamReader`, `StreamWriter` 클래스는 `TextReader`, `TextWriter`를 상속받는다. 인코딩 지정이 가능하다.

```cs
using System.IO;

class Program {
   static void Main(string[] args) {
      Test t = new Test();
      t.Print();
   }
}

class Test {
   public void Print() {
      StreamWriter sw = new StreamWriter("myasciifile.txt", false, Encoding.ASCII);
      sw.WriteLine("My First ASCII Line!");
      sw.WriteLine("How does ASCII handle umlauts äöü?");
      sw.Close();
      StreamReader sr = new StreamReader("myasciifile.txt", Encoding.ASCII);
      string line;
      while ((line = sr.ReadLine()) != null) {
            Console.WriteLine("read line : " + line);
      }
      sr.Close();
      
   }
}

// output
read line : My First ASCII Line!
read line : How does ASCII handle umlauts ????
```

## Threads

* UI스레드에서 많은 작업을 수행하여 앱이 응답하지 않는 현상을 방지하기 위한 모델
* 모든 응용 프로그램은 이미 스레드가 1개 있다 - 응용프로그램 / GUI 스레드
* `Thread` 클래스를 실행하려면 실행할 메소드가 1개 필요하다.
* 여러 스레드를 생성하면 오버헤드가 발생하기 때문에 `ThreadPool` 클래스를 사용해야 한다.
* Window Form에서 UI를 worker thread에서 변경하는 것은 불가능하다.
* `race condition`을 피해야 한다.

```cs
using System.Threading;

class Program {
   static void Main(string[] args) {
      Thread t = new Thread(DoALotOfWork);
      t.Start("a");
      Thread t2 = new Thread(DoALotOfWork);
      t2.Start("b");
   }

   static void DoALotOfWork(object t) {
      Console.WriteLine("A lot of work has been started!");

      int i = 0;
      while (i < 100) {
            Console.WriteLine(t.ToString() + " thread i : " + ++i);
      }

      Console.WriteLine("The thread has been stopped!");
   }
}

// output
A lot of work has been started!
A lot of work has been started!
b thread i : 1
b thread i : 2
...
```

## Thread-communication

* 스레드를 동기화하기 위해 `lock` 키워드를 사용한다.
* 키워드의 포인터는 임의의 `Object`와 같은 참조 유형으로 사용이 가능하다.

```cs
using System.Threading;

class Program {
   static Object myLock = new Object();
   static void Main(string[] args) {
      Thread t1 = new Thread(FirstWorker);
      Thread t2 = new Thread(SecondWorker);

      t1.Start();
      t2.Start();
   }

   static void FirstWorker() {
      Console.WriteLine("First worker started!");
      
      lock (myLock) {
            Console.WriteLine("First worker entered the critical block!");
            Thread.Sleep(1000);
            Console.WriteLine("First worker left the critical block!");
      }
      
      Console.WriteLine("First worker completed!");
   }

   static void SecondWorker() {
      Console.WriteLine("Second worker started!");

      lock (myLock) {
            Console.WriteLine("Second worker entered the critical block!");
            Thread.Sleep(5000);
            Console.WriteLine("Second worker left the critical block!");
      }
      
      Console.WriteLine("Second worker completed!");
   }
}

// output
First worker started!
First worker entered the critical block!
Second worker started!
First worker left the critical block!
Second worker entered the critical block!
First worker completed!
Second worker left the critical block!
Second worker completed!
```

## The Task Parallel Library

* .NET Framework 4.0 부터 [Task Parallel Library](https://docs.microsoft.com/ko-kr/dotnet/standard/parallel-programming/task-parallel-library-tpl) 가 도입되었다.
* `TPL`은 `System.Threading` 및 `System.Threading.Tasks` 네임스페이스에 포함된 공용 형식 및 API의 집합이다.
  * `PLINQ` 형식의 LINQ에 대한 작업 및 확장을 위한 클래스와 메소드의 집합
* `TPL`은 단기간에 여러 스레드를 생성, 결합하는데 특화된 스레드풀을 처리한다.
* 작업의 양이 많지 않거나 실행되는 반복횟수가 적으면 병렬화에 따른 오버헤드로 코드 실행이 오히려 느려질 수 있다.
* `Parallel.For()`를 사용하면 루프를 여러 스레드가 계산하도록 작업을 분리한다.

```cs
using System.Threading.Tasks;

class Program {
   static void Main(string[] args) {
      Test t = new Test();
      Console.WriteLine(t.Print());
   }
}

class Test {
   public double Print() {
      // No Parallel
      int N = 10000000;
      double sum = 0.0;
      double step = 1.0 / N;

      for (var i = 0; i < N; i++) {
         double x = (i + 0.5) * step;
         sum += 4.0 / (1.0 + x * x);
      }

      return sum * step;

      // Parallel Loop
      object _ = new object();
      int N = 10000000;
      double sum = 0.0;
      double step = 1.0 / N;

      Parallel.For(0, N, i => {
         double x = (i + 0.5) * step;
         double y = 4.0 / (1.0 + x * x);
         lock (_) {
            sum += y;
         }
      });

      return sum * step;
   }
}

// output
3.14159265358945
```

* 위의 코드보다 효율적인 코드는 `Parallel.For<TLocal>()` 함수를 사용하는 것이다.
* `TLocal`은 return값의 타입이다.
* 세번째 파라미터는 각 스레드에서 사용할 로컬 변수를 리턴하는 액션이다. 초기화는 아님.
* `state` 변수는 `ParallelLoopState` 클래스이며 루프 실행을 중단, 중지하는 등의 작업에 액세스할 수 있게 한다.
* `local` 변수는 현재 스레드의 로컬 변수값이다.

```cs
using System.Threading.Tasks;

class Program {
   static void Main(string[] args) {
      Test t = new Test();
      Console.WriteLine(t.Print());
   }
}

class Test {
   public double Print() {
      // Parallel Loop + Local
      object _ = new object();
      int N = 10000000;
      double sum = 0.0;
      double step = 1.0 / N;

      Parallel.For<double>(0, N, () => 0.0, (i, state, local) => {
         double x = (i + 0.5) * step;
         return local + 4.0 / (1.0 + x * x);
      }, local => {
         lock (_) {
            sum += local;
            Console.WriteLine("sum ? " + sum);
         }
      });

      return sum * step;
   }
}

// output
sum ? 8513904.30711955
sum ? 15487647.58378
sum ? 25774959.3024131
sum ? 31415926.535897
3.1415926535897
```

----
## Tasks and threads

* `Task`와 `Thread`는 차이가 있다.
* `Thread`는 OS에서 나온 일종의 자원이다.
* `Task`가 `Thread`의 특수화라고 할 수 있지만, 실행중인 `Task`인스턴스가 모두 `Thread`를 기반으로 하는 것은 아니다.
* `Task<T>`를 반환하는 모든 IO 바인딩 비동기 메소드는 스레드를 사용하지 않고 콜백을 기반으로 한다.
  * IO 바인딩은 스레드를 사용하는 것보다 콜백을 사용하는 것이 훨씬 적합하다. (`asnyc`)
* `Task` 를 사용하면 스레드 생성과 콜백 사용 두 가지 방법을 하나의 코드로 구현할 수 있다.

```cs
using System.Threading.Tasks;

class Program {
   static Object myLock = new Object();
   static void Main(string[] args) {
      Test t = new Test();
      t.Print();
   }

}

class Test {
   public void Print() {
      var tasks = new List<Task<long>>();
      for (int ctr = 1; ctr <= 10; ctr++) {
         tasks.Add(SimulationAsync(ctr));
      }
      var continuation = Task.WhenAll(tasks);
      try {
         //continuation.Wait();
      } catch (AggregateException) { }

      if (continuation.Status == TaskStatus.RanToCompletion) {
         long grandTotal = 0;
         foreach (var result in continuation.Result) {
            grandTotal += result;
            Console.WriteLine("Mean: {0:N2}, n = 1,000", result / 1000.0);
         }

         Console.WriteLine("\nMean of Means: {0:N2}, n = 10,000", grandTotal / 10000);
      } else {
         foreach (var t in tasks) {
            Console.WriteLine("Task {0}: {1}", t.Id, t.Status);
         }
      }
   }

   Task<long> SimulationAsync(int seed) {
      var task = new Task<long>(() => {
         long total = 0;
         var rnd = new Random(seed);
         // Generate 1,000 random numbers.
         for (int n = 1; n <= 1000; n++)
            total += rnd.Next(0, 1000);
         return total;
      });
      
      task.Start();
      return task;
   }
}

// output
Mean: 503.09, n = 1,000
Mean: 519.38, n = 1,000
Mean: 494.67, n = 1,000
Mean: 495.93, n = 1,000
Mean: 495.24, n = 1,000
Mean: 508.53, n = 1,000
Mean: 477.83, n = 1,000
Mean: 495.10, n = 1,000
Mean: 505.39, n = 1,000
Mean: 491.69, n = 1,000

Mean of Means: 498.00, n = 10,000
```

## [Awaiting async methods](https://msdn.microsoft.com/en-us/magazine/jj991977.aspx)

* C# 5부터 `await`, `async` 키워드 도입
* 메소드를 비동기식으로 표시한다.
* 반환값이 없는 경우 `Task`를, 반환값이 있는경우 `Task<T>`를 반환값으로 사용한다.
  * `Task`를 반환하지 않는것은 `anti-pattern`이다. async void 함수는 이벤트 핸들러와 함께 사용해야 하지만 쓰지 않는게 좋다.
  * void 반환 비동기 메소드는 `Task` 객체가 없기 때문에 exception을 자연스럽게 잡을 수 없다. (SynchronizationContext 에서 직접 발생)

```cs
private async void ThrowExceptionAsync() {
  throw new InvalidOperationException();
}
public void AsyncVoidExceptions_CannotBeCaughtByCatch() {
   try {
      ThrowExceptionAsync();
   } catch (Exception) {
      // The exception is never caught here!
      Console.WriteLine(e.ToString());
      throw;
   }
}

// 아래와 같이 수정
private async Task AsyncTask() {
   throw new InvalidOperationException();
}

public async Task CallAsyncTask() {
   try {
      await AsyncTask();
   } catch (Exception e) {
      Console.WriteLine(e.ToString());
      throw;
   }
}
```

* `ASP.NET context`에서 아래와 같은 코드는 `deadlock`이 발생한다.
* 콘솔 응용 프로그램에서는 발생하지 않는다. `thread pool SynchronizationContext` 대신 `one-chunk-at-a-time SynchronizationContext`을 가지고 있기 때문에.
* 해결방법은 `ConfigureAwait(false)` 를 await 구문에 사용하는 것.

```cs
public static class DeadlockDemo {
  private static async Task DelayAsync() {
      //await Task.Delay(1000);
      await Task.Delay(1000).ConfigureAwait(false);
  }
  // This method causes a deadlock when called in a GUI or ASP.NET context.
  public static void Test() {
      // Start the delay.
      var delayTask = DelayAsync();
      // Wait for the delay to complete.
      delayTask.Wait();
  }
}
```

![](async.png)

```cs
using System.Threading.Tasks;
using System.Net.Http;

class Program {
   static void Main(string[] args) {
      Test t = new Test();
      t.Print();
   }
}

class Test {
   public void Print() {
      Console.WriteLine("task start");
      Task<int> task = AccessTheWebAsync();
      //task.Start();
      Console.WriteLine("result : " + task.Result);
   }

   async Task<int> AccessTheWebAsync() {
      using (HttpClient client = new HttpClient()) {
            Task<string> getStringTask = client.GetStringAsync("https://docs.microsoft.com");

            // GetStringAsync로부터 얻는 string 값과 관계 없는 작업을 여기서 진행한다. 
            Console.WriteLine("DoIndependentWork");

            string urlContents = await getStringTask;

            return urlContents.Length;
      }
   }
}
```