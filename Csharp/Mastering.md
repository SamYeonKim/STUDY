# Mastering C# - Lecture Notes Part 2 of 4

## Enumerations (열거형)

+ 함수의 매개변수로 string이나 int형 대신 enum형을 추천.

### `[기본적인 열거형]`

```csharp
enum MyEnumeration {
    None = 0,
    First = 1,
    Second = 2,
    Last = 100
}
```

### `[열거형에 사용되는 형식을 선언]`

+ 기본적으로 int를 제공.
+ 열거형으로 승인된 형식은 `byte, sbyte, short, ushort, int, uint, long 또는 ulong`입니다.

```csharp
enum MyEnumeration : int {
	None,
	First,
	Second,
	Last
}

enum Range : long {
    Max = 2147483648L,
    Min = 255L 
};
```

### `[Flag]`

+ [Flag]가 없다고 비트연산이 안되지는 않지만 표현방식이 달라진다.

```csharp
[Flags]
public enum CarOptions {
	SunRoof = 1<<0,
	Spoiler = 1<<1,
	FogLights = 1<<2,
	TintedWindows = 1<<3,
}

void Start () {
	CarOptions options = CarOptions.SunRoof | CarOptions.FogLights;
	Debug.Log("Exist SunRoof In options : " + ((options & CarOptions.SunRoof) != 0));
	Debug.Log(options);
}

// Output with [Flag] Attribute
"Exist SunRoof In options : True"
"SunRoof, FogLights"

// Output without [Flag] Attribute
"Exist SunRoof In options : True"
"5"
```

### `[다른 언어의 enum]`

+ java

```java
enum PersonType {
	MAN,
	WOMAN,
}
```

+ objc

```objectivec
enum PersonType {
    MAN,
    WOMAN,
};
```

+ python

```python
class PersonType(enum):
	MAN = 0,
	WOMAN = 1,
```

+ c++

```cpp
enum PersonType {
	MAN,
	WOMAN,
};
```

## Delegates

### `[선언 방법]`

`delegate [return type] [name] (parameter)`

### `[사용 예시]`

[C#]

```csharp
delegate void delegateTest (int d);

void AAA (int d) {
	Debug.Log ("In AAA : " + d);
}

void BBB (int d) {
	Debug.Log ("In BBB : " + d);
}

void Start () {
	delegateTest a = new delegateTest (AAA);
    a += BBB;

    a.Invoke (3);
}

// Output
"In AAA : 3"
"In BBB : 3"
```

[JAVA]

```java
import java.util.ArrayList;
import java.util.List;

@FunctionalInterface
interface Runnable {
    void run();
}

public class MainClass{
    static void AAA () {
	    System.out.println("In AAA");
    }

    static void BBB () {
	    System.out.println("In BBB");
    }
    
     public static void main(String []args){
        List<Runnable> queue = new ArrayList<>();
        queue.add(() -> AAA());
        queue.add(() -> BBB());
     }
}
```

## Auto-generated properties

### `[일반적인 사용 예시]`

[C#]

```csharp
private int myVariable;
public int MyVariable {
	get { return myVariable; }
	set { myVariable = value; }
}
```

[JAVA]

```java
public class MyVariable {
    private int myVariable;

    public int getVariable() { return this.myVariable; }
    public void setVariable(int variable) { this.myVariable = variable; }
}
```

[Obj-c]

```objc
[.h]

@interface PropertyTest : NSObject {
    int myVariable;
}

@property int myVariable;
@end
```

```objc
[.m]

@implementation PropertyTest
@synthesize myVariable;
@end
```

[Python]

```python
class MyVariable(object):
    myVariable = 0;
    
    def __init__(self):
        self.myVariable = 0

    @property
    def x(self):
        return self.myVariable

    @x.setter
    def x(self, value):
        self.myVariable = value
```

### `[Auto-generated 예시]`

+ get, set 내부에 조건처리가 없을 경우 사용하기 좋음.

```csharp
public int MyVariable { get; set; }

// 접근자 사용
public int MyVariable { get; private set; }
```

## Generic types

+ 같은 함수를 여러타입으로 재사용하고 싶을때 유용.

### `[선언 방법]`

`class [name] <T>{}`

`delegate T [name]<T>()`

### `[사용 예시]`

```csharp
public class GenericTest<T> {
    T m_a;

    public GenericTest(T a) {
        m_a = a;
    }

    public void ShowTypeAndValue(){
        Debug.Log ("Type : " + m_a.GetType() + ", Value : " + m_a); 
    }
}

void Start () {
    GenericTest<int> a = new GenericTest<int> (1);
    a.ShowTypeAndValue ();

    GenericTest<double> b = new GenericTest<double> (3);
    b.ShowTypeAndValue ();
}

// Output
"Type : System.Int32, Value : 1"
"Type : System.Double, Value : 3"
```

```csharp
delegate T GenericDelegate<T>(T a);

int AAA(int a) {
	Debug.Log("In A : " + a);
	return a;
}

float BBB(float b) {
	Debug.Log("In B : " + b);
	return b;
}

void Start () {
	GenericDelegate<int> a = new GenericDelegate<int> (AAA);
	a.Invoke (1);

	GenericDelegate<float> b = new GenericDelegate<float> (BBB);
	b.Invoke (3.0f);
}

// Output
"In A : 1"
"In B : 3"
```

[Java]

```java
class GenericTest<T> {
    T m_a;

    public void SetTest(T a) {
        m_a = a;
    }

    public void ShowTypeAndValue(){
        System.out.println ("Value : " + m_a); 
    }
}

public class MainClass{
     public static void main(String []args){
         GenericTest<String> a = new GenericTest();
         a.SetTest("aa");
         a.ShowTypeAndValue ();
     }
}
```

[C++]

```cpp
template<typename T>
class GenericTest {
    T *m_Items;
    
    void SetTest(const T &item){
        m_Items = item;
        
    }
    
    void ShowTypeAndValue(){
        cout << m_Items;
        
    }
};
```

## Generic methods

### `[사용 예시]`

```csharp
class MyClass {
    public static void Swap<T>(ref T l, ref T r) {
        T temp = r;
        r = l;
        l = temp;
    }
}

void Start () {
    int a = 3;
    int b = 4;
    MyClass.Swap(ref a, ref b);
    Debug.Log ("a : " + a + ", b : " + b);
}

// OutPut
"a : 4, b : 3"
```

## Constraints

+ 제약조건을 이용하여 사용을 원치않는 타입을 필터링 할 수 있다.

### `[사용 방법]`

[Class]

`class [name] <T> Where T : [조건] {}`

[Method]

`T [name]<T>() where T : [조건] {}`

### `[사용 예시]`

[C#]

```csharp
class MyClass{
    public T CreateClass<T>() where T : new() {
        return new T ();
    }
}

public class NewTest {
    public NewTest() {
        Debug.Log("생성자 발동");
    }
}

void Start () {
    MyClass my_class = new MyClass();
    NewTest new_test = my_class.CreateClass<NewTest> ();
}
```

+ new() 제약조건은 만들려는 클래스나 구조체에 매개변수를 받지 않는 기본 생성자를 가지고 있어야 한다.
+ 매개변수를 받는 생성자가 있는 경우 매개변수가 없는 기본 생성자를 만들어주지 않으므로 에러를 뱉는다.

```csharp
class MyClass{
		public T CreateClass<T>() where T : new() {
			return new T ();
		}
}

public class NewTest {
    public NewTest(int a) {
        Debug.Log("생성자 발동, " + a);
    }
}

void Start () {
    MyClass my_class = new MyClass();
    NewTest new_test = my_class.CreateClass<NewTest> ();
}

// Output
"The type `A.NewTest' must have a public parameterless constructor in order to use it as parameter `T' in the generic type or method `A.MyClass.CreateClass<T>()'"
```

[JAVA]

```java
class AA {
}

class BB extends AA{
}

class Foo<T extends AA> {
}

public class MainClass{
     public static void main(String []args){
        Foo<BB> b = new Foo<BB>();
     }
}

// C#의 new, class 제약조건은 java에서 지원하지 않는다.
```

### `[제약 조건 참조]`

[제약 조건 종류](https://docs.microsoft.com/ko-kr/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters)

## Lambda expressions

+ 반환 타입 사용 여부에 따라 두가지 delegate를 사용가능하다.

### `[사용 방법]`

`delegate Func<[매개변수 타입], [반환 타입]>`

### `[사용 예시]`

[C#]

```csharp
Func<int> zero_parameter = () => 1;
Debug.Log (zero_parameter ());

Func<int, int> one_parameter = (x) => x+2;
Debug.Log (one_parameter (3));

Func<int, int, int> two_parameter = (x, y) => x + y;
Debug.Log (two_parameter (3, 7));

// Output
"1"
"5"
"10"
```

[Java]

```java
@FunctionalInterface
interface Func {
	public int calc(int a, int b);
}

public class MainClass{
     public static void main(String []args){
         
        Func TwoParameter = (int a, int b) -> a +b;
        System.out.println(TwoParameter.calc(3, 7));
     }
}
```

[Python]

```python
(lambda x,y: x + y)(3, 7)
```

### `[사용 방법]`

`delegate void Action()`

### `[사용 예시]`

```csharp
void ShowOneParamter(int param) {
    Debug.Log("ShowOneParamter : " + param);
}

void Start () {
    Action zero_parameter = () => Debug.Log("Zero Paramter");
    Action<int> one_parameter = (x)=> Debug.Log("One Paramter : " + x);
    Action<int, int> two_parameter = (x, y)=> Debug.Log("Two Paramter : " + x + ", " + y);

    zero_parameter ();
    one_parameter (1);
    two_parameter (3, 7);

    one_parameter = ShowOneParamter;
    one_parameter (10);
}

// Output
"Zero Paramter"
"One Paramter : 1"
"Two Paramter : 3, 7"
"ShowOneParamter : 10"
```

## Anonymous objects & inferring types

+ 암시적으로 형식을 결정하는 Anonymous 타입의 변수를 제공한다.

### `[사용 방법]`

`var`

### `[사용 예시]`

+ _var의 Name, Age로 접근 할 수 있는 `프로퍼티를 제공`한다.

```csharp
object anonymous = new { Name = "Florian", Age = 28 };

var _var = new { Name = "Florian", Age = 28 };
Debug.Log (_var.Name);

// Output
Florian
```

## Extension methods

+ 기존 형식에 메소드를 추가 할 수 있는 기능을 제공한다.
+ 확장 메소드는 반드시 `static`을 사용해야 한다.

### `[사용 방법]`

`public static [리턴 타입] [함수 이름] (this [확장 타입] [매개변수]) {}`

### `[사용 예시]`

[C#]

```csharp
static class MyExtensions {
	public static string AddString(this String str) {
		return str + ", Hi";
	}

	public static void AddInt(this int i) {
		Debug.Log (i + 2);
	}
}

public class A : MonoBehaviour {
	void Start () {
		string a = "How are you?";
		Debug.Log (a.AddString());

		int b = 3;
		b.AddInt ();
	}
}

// Output
"How are you?, Hi"
"5"
```

[obj-c]

```objc
@interface NSString (extention)
- (void) ShowInfo;
@end

@implementation NSString (extention)
- (void) ShowInfo{
    NSLog(@"%@, Hi", self);
}
@end


int main(int argc, char * argv[]) {
    NSString *a = @"abc";
    [a ShowInfo];
}
```

[C++]

```cpp
#include <iostream>
#include <string>

using namespace std;

class ContainsExtension final {
public:
    static void ExtensionMethod(const std::string &myParam) {
        cout << myParam << ", Hi";
    }
};

int main() {
    std::string s = "abc";
    ContainsExtension::ExtensionMethod(s);

    return 0;
}
```

## LINQ

+ Language Integrated Quary
+ 데이터에서 원하는 정보를 검색하는 기능을 제공.

### `[사용 방법]`

+ `from` - 추출을 원하는 데이터 배열 지정.
+ `where` - 필터링 조건 지정.
+ `orderby` - 순서 지정.
+ `select` - 출력 형태 지정.

### `[사용 예시]`

```csharp
var names = new[] { "Tom", "Dick", "Harry", "Joe", "Mary" };

// 사용 방법1
IEnumerable<string> query = from m in names
			where m.Contains("a")
			orderby m.Length
			select m + ": a";

// 사용방법 2
IEnumerable<string> query = names.Where((m)=>m.Contains("a"))
			.OrderBy((m)=>m.Length)
			.Select((m)=>m + ": a");

foreach(var a in query) {
	Debug.Log (a);
}

// Output
"Mary: a"
"Harry: a"
```