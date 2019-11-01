# Mastering C# - Lecture Notes Part 2 of 4

## Enumerations (열거형)

### `[기본적인 열거형]`

```cs
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

```cs
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

+ `[Flags]`가 없다고 비트연산이 안되지는 않지만 표현방식이 달라진다.

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

* 참조 : [C# Flag 연산 총정리](https://andromedarabbit.net/csharp_flag_operations)

## Delegates

* `Method`를 참조 해서 사용할 수 있다.

### `[선언 방법]`

`delegate [return type] [name] (parameter)`

```cs
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

## Auto-generated properties

```cs
private int myVariable;
public int MyVariable {
	get { return myVariable; }
	set { myVariable = value; }
}
```

* 일반적인 사용은 위와 같은 형식이 될 것인데, `return ~`과 ` ~ = value` 구문이 반복되는 문제가 있기 위와 같은 반복 구문을 축약해서 쓸 수 있도록 컴파일러가 기능 제공 한다,
* get, set 내부에 조건처리가 없을 경우 사용하기 좋음.

```cs
public int MyVariable { get; set; }

// 접근자 사용
public int MyVariable { get; private set; }
```

## Generic types

* 타입이 다른데 동일한 로직을 사용하고 싶을때 좋다.

### `[선언 방법]`

`class [name] <T>{}`

`delegate T [name]<T>()`

```cs
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

## Generic methods

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

* `Generic type T`를 원하는 조건에 해당 하는것만 사용할 수 있도록 하자.

### `[사용 방법]`

[Class]

`class [name] <T> Where T : [조건] {}`

[Method]

`T [name]<T>() where T : [조건] {}`

```cs
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

* 참조 : [제약 조건 종류](https://docs.microsoft.com/ko-kr/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters)

## Lambda expressions

* 하나의 매개변수를 사용할 경우엔 소괄호를 쓰지 않아도 된다.
* 하나 이상의 매개변수를 사용하거나, 아예 안쓰는 경우엔 소괄호를 써야 한다.
* 우측항을 한줄로 표현하는 경우, 자동으로 해당 값이 반환 값이 된다. 만약 `void` 타입이라면 반환값은 없다.
* 우측항을 여러줄로 표현시에는 `return` 키워드를 이용해야만 반환 값을 사용 할 수 있다.

```cs
x => x * x;
(x, y) => x * x + y;
() => 42;
x => { 
    return x > 0 ? -1 : (x < 0 ? 1 : 0); 
};
() => {
    Console.WriteLine("Current time: " + DateTime.Now.ToShortTimeString());
    Console.WriteLine("Current date: " + DateTime.Now.ToShortDateString());
};
```

* 반환 타입 사용 여부에 따라 두가지 delegate를 사용가능하다.

`delegate Func<[매개변수 타입], [반환 타입]>`
`delegate Action<[매개변수 타입]>`

```cs
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

```cs
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

* `var` 키워드를 이용해서, 런타임에 형식을 결정하는 Anonymous 타입의 변수를 제공한다.

```cs
var anonymous = new { Name = "Florian", Age = 28 }; //Name, Age를 갖는 익명 클래스를 만든다.
var a_int = 3;  //a_int는 int 타입으로 인식된다.
Debug.Log (anonymous.Name);

// Output
Florian
```

## Extension methods

+ 기존 클래스 파일을 수정 하지 않고 메소드를 추가 할 수 있는 기능을 제공한다.
+ 확장 메소드는 반드시 `static`을 사용해야 한다.

`public static [리턴 타입] [함수 이름] (this [확장 타입] [매개변수]) {}`

```cs
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

## LINQ

+ Language Integrated Quary
+ 데이터에서 원하는 정보를 검색하는 기능을 제공.
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