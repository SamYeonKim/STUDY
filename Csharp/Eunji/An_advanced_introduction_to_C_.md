# An advanced introduction to C#

## Basic concepts

* 메모리를 Garbage Collector가 관리하므로 신경쓰지 않아도 된다.
  * 가비지 콜렉터는 참조되지 않은 오브젝트를 감지하고 수집하여 해당 메모리를 확보함
  * 가비지 콜렉터는 메모리 최적화를 수행하지만 메모리를 확보한 직후에 수행하지는 않음
  * C#에서는 세그멘트 오류가 발생하지 않음
* 가비지 콜렉터가 어떻게 작동하는가 
  
![](https://www.codeproject.com/KB/cs/1094079/gc.png)

* 컴파일러는 assembly code가 아닌 MSIL(Microsoft Intermediate Language)를 만든다
* C#의 경우 CLR(Common Language Runtime) 어셈블리를 얻는다.
  * 이 어셈블리는 런타임 중에 JIT으로 컴파일 된다.
* 런타임 중에 최적화가 수행된다.
  * 메서드 호출은 인라인되며 필요없는 명령문은 자동으로 생략됨.
* 객체 지향의 기능이 Java의 영향을 받았다.
  * Java의 키워드 집합과는 다르다. 
  * Java > `extends` -> C# > `:`
```cs
[C#]
//Those are the namespaces that are (by default) included
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

//By default Visual Studio creates already a namespace
namespace HelloWorld
{
    //Remember: Everything (variables, functions) has to be encapsulated
    class Program
    {
        //The entry point is called Main (capital M) and has to be static
        static void Main(string[] args)
        {
            //Writing something to the console is easily possible using the
            //class Console with the static method Write or WriteLine
            Console.WriteLine("Hello Tech.Pro!");
        }
    }
}
```
```java
[java]
public class Main
{
	public static void main(String[] args) {
		System.out.println("Hello Tech.Pro!");
	}
}
```
```objc
[objective-c]
int main (int argc, const char * argv[])
{
    NSLog (@"Hello Tech.Pro!");
    return 0;
}
```
* Main이 대문자라는 것을 제외하고 Java와 유사함
* 주석은 //와 /* */ 사용 가능
* 모든 메소드와 변수는 캡슐화해야함
* C#은 콘솔 기반 상호작용을 위해 Console 클래스를 사용함
* 클래스의 함수를 메소드라고 함
* Main이라는 메인 시작 지점은 한번만 존재할 수 있기 때문에 정적이어야 한다.
* C의 기본 Main메서드는 2개의 매개변수(갯수, 배열)을 전달하지만 C#은 1개의 매개변수를 전달함(모든 배열 유형에 Length라는 필드가 있음)

## Namespaces
* 타입을 구별하기 위해 컴파일러에서 사용할 문자열
* 네임스페이스를 쓰는 이유
  * 동일한 이름과 매개변수를 갖는 메서드는 리턴형식이 다르더라도 구별할 수 없음
  * 두개의 내부 라이브러리를 사용하는 경우에도 동일한 이름을 가진 유형을 정의할 수는 있지만 하나의 유형만 사용할 수 있음
* 사용된 여러 네임스페이스에 정의된 유형을 사용할 경우 명시적으로 지정해주어야함

## Data types and operators

|type|size(bytes)|description|
|---|:---|:---|
|bool|1|true/false|
|char|2|single character|
|short|2|integers|
|int|4|integers|
|long|8|integers|
|float|4|floating point numbers|
|double|8|floating point numbers|
|decimal|16|floating point numbers|
|unit|4|부호 없는 정수|
|ulong|8|부호 없는 long|

* string : 문자열을 사용하기 위해 정의된 클래스

* C와 동일한 연산자 집합

* logical operators : `==`, `!=`, `<`, `<=`, `>`, `>=`, `!`, `&&`, `||`
* Bit operators : `^`,`&`,`|`,`<<`, `>>`
* Arithmetic operators : `+`,`-`,`*`,`/`,`%`,`++`,`--`
* assignment operator : `=`
* ternary operator : `condition ? consequent : alternative`
* Brackets : `()`

* inbuilt-methods : `typeof`, `sizeof`

|operator|description|
|---|:---|
|`()`|기본 캐스트 연산자|
|`as`|참조 캐스트 연산자|
|`is`|형식 준수 검사|
|`??`|null 병합 연산자|
|`:`|상속 연산자|

## Reference and value types

* reference와 value 타입은 다름
  * class : reference
  * struct : struct
 * value : 객체를 복사, 값을 변경해도 원래 값이 변경되지 않음
* reference : 객체의 주소를 복사, 다른 곳에서 객체의 값을 변경하면 원래 값이 변경됨

* `ref`
  * 구조체를 전달하면 원래 값에 접근 가능
  * 클래스를 전달하면 포인터를 재설정할 수 있음
```cs
using System;

class Program
{
    static void Main()
    {
        //A string is a class, which can be instantiated like this
        string s = "Hi there";
        Console.WriteLine(s);//Hi there
        ChangeString(s);
        Console.WriteLine(s);//Hi there
        ChangeString(ref s);
        Console.WriteLine(s);//s is now a null reference
    }

    static void ChangeString(string str)
    {
        str = null;
    }

    static void ChangeString(ref string str)
    {
        str = null;
    }
}
```
* `out`
  * `ref` 키워드와 비슷하지만 다름
  * `out` 변수가 매개변수로 있는 메소드에서 할당되어야함
  * `out` 변수를 매개변수로 전달하는 메소드에서는 할당할 필요 없음
```cs
using System;

class Program
{
    static void Main(string[] args)
    {
        //Usually C# forbids us to leave variables uninitialized
        SampleClass sampleClass;
        SampleStruct sampleStruct;

        //However, C# thinks that an out-Function will do the initialization
        HaveALook(out sampleClass);
        HaveALook(out sampleStruct);
    }

    static void HaveALook(out SampleClass c)
    {
        //Insert a breakpoint here to see the
        //value of c before the assignment:
        //It will be null...
        c = new SampleClass();
    }

    static void HaveALook(out SampleStruct s)
    {
        //Insert a breakpoint here to see the
        //value of s before the assignment:
        //It will NOT be null...
        s = new SampleStruct();
    }

    //In C# you can created nested classes
    class SampleClass
    {
    }

    //A structure always inherits ONLY from object,
    //we cannot specify other classes (more on that later)
    //However, an arbitrary number of interfaces can
    //be implemented (more on that later as well)
    struct SampleStruct
    {
    }
}
```
* `new`
  * 적절한 위치에 메모리 할당
    * reference type은 heap, value type은 stack
  * 일치하는 constructor 호출
  
## Control flow

* C와 유사함

* `if`,`switch` : 조건문
* `for`,`while`,`do-while` : 반복문
* `break` : 반복문 탈출
* `condition` : 다음 반복문으로 이동
* `goto` : 점프문
* `foreach` : iterator 반복문
    * C에서는 사용할 수 없음
    * 모든 배열은 iterator를 정의하고 있음
```cs
[C#]
//Creating an array is possible by just appending [] to any data type
int[] myints = new int[4];
//This is now a fixed array with 4 elements. Arrays in C# are 0-based
//hence the first element has index 0.
myints[0] = 2;
myints[1] = 3;
myints[2] = 17;
myints[3] = 24;

//This foreach construct is new in C# (compared to C++):
foreach(int myint in myints)
{
    //Write will not start a new line after the string.
    Console.Write("The element is given by ");
    //WriteLine will do that.
    Console.WriteLine(myint);
    myint = 1; // compile error
}
```
* `foreach`문에서는 현재 요소를 변경할 수 없음

```java
[java]
int[] myints = new int[4];
myints[0] = 2;
myints[1] = 3;
myints[2] = 17;
myints[3] = 24;

for(int myint : myints)
{
    myint = 1;          // 가능
    System.out.println("The element is given by ");
    System.out.println(myint);  
}
```
```objc
[objective-c]
int main (int argc, const char * argv[])
{
    NSArray *myints = [NSArray arrayWithObjects:@"2", @"3", @"17", @"24", nil];
    for (NSString *myint in myints) {
        myint = 1;          // 불가능
        NSLog(@"The element is given by %@", myint);
    }

    return 0;
}
```
## Object-oriented programming

* 객체 지향 프로그래밍은 함수 대신 객체에 초점을 맞추는 방법
* 인스턴스에 종속성이 없는 정적일지라도 클래스 안에 구현되어야 함
  * sin(), cos(), sign() 대신 클래스 이름도 기입해야함 -> Math.Sin(), Math.Cos(), Math.Sign()  

* 객체 지향 프로그래밍의 주요 측면
  * Data encapsulation
  * Inheritance
  * Relations between types
  * Declaring dependencies
  * Maintainability
  * Readability
* 인스턴스에 종속성이 없는 변수, 메소드를 원할 경우 정적 클래스로 작성

## Inheritance and Polymorphism

* 상위 클래스의 모든 멤버가 복사됨

```cs
[C#]
class MySubClass : MyClass
{
    public void WriteMore()
    {
        Console.WriteLine("Hi again!");
    }
}
```
```java
[java]
class MySubClass extends MyClass
{
    public void WriteMore()
    {
        System.out.println("Hi again!");
    }
}
```

```objc
[objective-c]
@interface MySubClass : MyClass
-(void)WriteMore;
@end

@implementation MySubClass
-(void)WriteMore{
    NSLog(@"Hi again!");
}
@end
```

* 객체 자체는 4가지 메소드를 정의함
  * ToString : 인스턴스를 문자열로 표시
  * Equals : 임의의 두 객체를 비교
  * GetHashCode : 두 객체가 같을 수 있는 경우 숫자 indicator를 가져옴
  * GetType : 현재 인스턴스의 특정 유형에 대한 메타 정보 가져옴

```cs
[C#]
class MyClass
{
    //This method is now marked as re-implementable
    public virtual void Write(string name)
    {
        //Using a placeholder in a string.Format()-able method
        Console.WriteLine("Hi {0} from MyClass!", name);
    }
}

class MySubClass : MyClass
{
    //This method is now marked as re-implemented
    public override void Write(string name)
    {
        Console.WriteLine("Hi {0} from MySubClass!", name);
    }
}
```

* `virtual` : 자식 클래스가 메소드를 다시 구현할 수 있음을 지정
* `override` : 자식 클래스가 재정의하는 경우 명시적으로 `override` 표시

```java
[java]
class MyClass
{
    public void Write(string name)
    {
       System.out.println("Hi " + name + "from MyClass!");
    }
}

class MySubClass : MyClass
{
    @Override
    public void Write(string name)
    {
        System.out.println("Hi " + name + "from MySubClass!");
    }
}
```

* Polymorphism : 기본적으로 클래스가 특정 메소드를 다시 구현할 수 있음
```cs
using System;

class Program
{
    static void Main(string[] args)
    {
        MyClassOne one = new MyClassOne();
        MyClassTwo two = new MyClassTwo();

        Console.WriteLine(one);//Displays a strange string that is basically the type's name
        Console.WriteLine(two);//Displays "This is my own class output"
    }
}

class MyClassOne
{
    /* Here we do not override anything */
}

class MyClassTwo
{
    public override string ToString()
    {
        return "This is my own class output";
    }
}
```
 * WriteLine 메소드는 인스턴스를 인자로 받으면 인스턴스의 ToString함수를 호출함

## Access modifiers

|keyword|description|
|---|:---|
|private|객체 외부에서 볼 수 없음, 상속X|
|protected|객체 외부에서 볼 수 없음, 상속O|
|internal|객체 외부에서 볼 수 있지만 현재 프로젝트 외부에서는 볼 수 없음|
|public|어디에서나 볼 수 있음|

* namespace에 직접 배치된 타입은 기본적으로 internal
* 클래스나 구조체에 배치된 타입 및 멤버는 기본적으로 private

```cs
using System;

//No modifier, i.e., the class Program is internal
class Program
{
    //No modifier, i.e., the method Main() is private
    static void Main()
    {
        MyClass c = new MyClass();
        //Works
        int num = c.WhatNumber();
        //Does not work
        //int num = c.RightNumber();
    }
}

//MyClass is visible from this library and other libraries
public class MyClass
{
    //This one can only be accessed from MyClass
    private int a;

    //Classes inheriting from MyClass can access b like MyClass can
    protected int b;

    //No modifier, i.e., the method RightNumber() is private
    int RightNumber()
    {
        return a;
    }

    //This will be seen from the outside
    public int WhatNumber()
    {
        //Access inside the class is possible
        return RightNumber();
    }
}

//MySubClass is only visible from this library
internal class MySubClass : MyClass
{
    int AnotherRightNumber()
    {
        //Works
        b = 8;
        //Does not work - a cannot be accessed since it is private
        return a;
    }
}
```

* internal MyClass, public MySubclass는 설정X
  * 컴파일러가 알 수 없음
* 모든 비정적 메소드는 클래스의 인스턴스 포인터 변수 `this`에 접근 가능
* `this`는 현재 클래스의 인스턴스를 가리킴
* `base` : 부모 클래스의 멤버에 접근 가능

## Properties

```cs
private int myVariable;

public int MyVariable
{
    get { return myVariable; }
    set { myVariable = value; }
}
```
* 내부적으로, 컴파일러는 여전히 get/set메소드를 생성할 것이지만 신경쓰지 않음

## The constructor

* 암시적으로만 호출할 수 있는 특별한 종류의 메서드
* `new` 로 메모리를 할당하면 자동으로 호출됨
* 매개 변수에 따라 여러 정의를 사용하여 생성자를 오버로드 할 수 있음
* 생성자를 정의하지 않으면 기본 생성자를 배치, 생성자를 정의하면 컴파일러는 기본 생성자를 삽입하지 않음
* 새로운 인스턴스를 암시적으로 반환하므로 리턴형X, 생성자는 클래스의 이름과 같은 이름으로 정의

```cs
[C#]
class MyClass
{
    public MyClass()
    {
        //Empty default constructor
    }

    public MyClass(int a)
    {
        //Constructor with one argument
    }

    public MyClass(int a, string b)
    {
        //Constructor with two arguments
    }

    public MyClass(int a, int b)
    {
        //Another constructor with two arguments
    }
}

class MyClass
{
    public MyClass()
        : this(1, 1) //This calls the constructor with 2 arguments
    {
    }

    public MyClass(int a, int b)
    {
        //Do something with a and b
    }
}
```
* chaining constructor : 하나의 생성자를 실행할 때 다른 생성자도 호출

```java
public MyClass()
{
    this(1,1);
}

public MyClass(int a, int b)
{
}
```
```objc
@interface MyClass
-(id)init;
-(id)init:(int)a b:(int)b;
@end
@implementation MyClass
-(id)init{
    [self init:1 b:1];
}
-(id)init:(int)a:b(int)b{

}
@end
```
### Singleton pattern
```cs
[Singleton pattern - C#]

class MyClass
{
    private static MyClass instance;

    private MyClass() { }

    public static MyClass Instance
    {
        get 
        {
            if (instance == null)
                instance = new MyClass();

            return instance;
        }
    }
}
```
```java
[Singleton pattern - java]

class MyClass
{
    private static MyClass instance;

    private MyClass() { }

    public static MyClass Instance()
    {
        if(instance == null) {
        	instance = new MyClass();
        }
        return instance;
    }
}
```
```objc
[Singleton pattern - objective-c]
@interface MyClass
+(MyClass *)Instance;
@end

@implementation MyClass
+(MyClass *)Instance{
    static MyClass* instance = nil;
    if(instance == nil){
        instance = [[MyClass alloc] init];
    }
    return instance;
}
@end
```
* 클래스의 인스턴스를 만들 수 없지만 MyClass.Instance를 사용해 접근할 수 있음
  * Instance 속성 메서드 내부에서 인스턴스가 만들어지기 때문에 원하지 않는 종속성을 초래할 수 있지만 추가 기능(하위 클래스 인스턴스화 등)을 수행할 수 있음
  * 오브젝트가 인스턴스를 요구할 때까지 인스턴스화가 수행되지 않음

## Abstract classes and interfaces

* `Abstract` : 클래스에 대한 템플릿을 만드는 키워드
  * 인스턴스화 될 수 없음
* C#에서 상속은 한 클래스만 가능함
```cs
[C#]
abstract class MyClass
{
    public abstract void Write(string name);
}

class MySubClass : MyClass
{
    public override void Write(string name)
    {
        Console.WriteLine("Hi {0} from an implementation of Write!", name);
    }
}
```
```java
[java]
abstract class MyClass
{
    public abstract void Write(String name);
}

class MySubClass extends MyClass
{
	@Override
	public void Write(String name) {
		System.out.println("Hi " + name + "from an implementation of Write!");
	}
}
```
* `interface` : 인터페이스를 구현하는 클래스가 제공해야 하는 메소드를 정의
  * 변수 없는 추상클래스라고 생각할 수 있음
* 모든 메소드는 자동으로 `public`으로 지정
```cs
[C#]
interface MyInterface
{
    void DoSomething();

    string GetSomething(int number);
}

class MyOtherClass : MyInterface
{
    public void DoSomething()
    { }

    public string GetSomething(int number)
    {
        return number.ToString();
    }
}

class MySubSubClass : MySubClass, MyInterface
{
    public void DoSomething()
    { }

    public string GetSomething(int number)
    {
        return number.ToString();
    }       
}
```
```java
interface MyInterface
{
    void DoSomething();

    String GetSomething(int number);
}

class MyOtherClass implements MyInterface
{
    public void DoSomething()
    { }

    public String GetSomething(int number)
    {
        return Integer.toString(number);
    }
}

class MySubSubClass extends MySubClass implements MyInterface
{
    public void DoSomething()
    { }

    public String GetSomething(int number)
    {
        return Integer.toString(number);
    }       
}
```
```objc
@protocol MyInterface
-(void)DoSomething;
-(NSString*) GetSomething:(int)number;
@end

@interface MyOtherClass : NSObject<MyInterface>
@end

@implementation MyOtherClass
-(void)DoSomething{
    NSLog(@"DoSomething");
}
-(NSString*) GetSomething:(int)number{
    return [NSString stringWithFormat:@"%d",number];
}
@end
```
* interface를 인스턴스화할수는 없지만 타입으로는 사용가능
```cs
MyInterface myif = new MySubSubClass();
```

* 여러 개의 인터페이스를 구현할 수 있기 때문에 동일한 이름과 매개변수를 가진 메소드가 포함될 수 있음
* 명시적으로 구별
* 인터페이스에 인스턴스를 캐스팅한 경우에만 인터페이스에서 정의한 메서드에 접근 가능
```cs
class MySubSubClass : MySubClass, MyInterface
{
    //Explicit (no public and MyInterface in front)
    void MyInterface.DoSomething()
    { }

    //Explicit (no public and MyInterface in front)
    string MyInterface.GetSomething(int number)
    {
        return number.ToString();
    }       
}
```

## Exception handling

* 모든 예외는 System네임스페이스에 배치된 Exception 클래스에서 파생되어야함
* 예외 처리가 가능하면 처리해야함

```cs
byte[] content = null;

try
{
    content = File.ReadAllBytes(/* ... */);
}
catch (PathTooLongException)
{
    //React if the path is too long
}
catch (FileNotFoundException)
{
    //React if the file has not been found
}
catch (UnauthorizedAccessException)
{
    //React if we have insufficient rights
}
catch (IOException)
{
    //React to a general IO exception
}
catch (Exception)
{
    //React to any exception that is not yet handled
}
```

* 각각 catch 블록을 지정할 수 있음
* 가장 구체적인 예외가 먼저, 일반적인 예외가 나중에 나오는 순서로 지정
* 예외 변수의 이름을 지정하지 않아도됨

```cs
void MyBuggyMethod()
{
    Console.WriteLine("Entering my method");
    throw new Exception("This is my exception");
    Console.WriteLine("Leaving my method");
}
```
* `throw` : throw 키워드를 이용해 예외를 던질 수 있음

* 적절한 try-catch-block이 발견(버블링)되지 않으면 프로그램이 중단됨

* Exception 클래스에서 파생된 예외 클래스 작성 가능
```cs
class MyException : Exception
{
    public MyException()
        : base("This is my exception")
    {
    }
}

void MyBuggyMethod()
{
    Console.WriteLine("Entering my method");
    throw new MyException();
    Console.WriteLine("Leaving my method");
}
```

* `finally` : try,catch 블록에 종속되지 않는 마지막 작업을 수행하는 블록

```cs
FileStream fs = null;

try
{
    fs = new FileStream("Path to the file", FileMode.Open);
    /* ... */
}
catch (Exception)
{
    Console.WriteLine("An exception occurred.");
    return;
}
finally
{
    if (fs != null)
        fs.Close();
}
```