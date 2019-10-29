# Professional techniques for C#

## 연산자 오버로딩

C# 에서는 해당 연산자들이 오버로드가 가능하다.

* +, -, !, ~, ++, --, true, false, +, -, *, /, %, &, |, ^, <<, >>, ==, !=, <, >, <=, >=

* `=` 할당 연산자는 오버로드가 불가능하다.

인덱스 연산자 `[]`는 오버로드 할 수 없지만, 자체 인덱서 구현이 가능하다. 인덱서는 속성과 비슷하지만, 특별한 이름(`this`)을 가지며 하나 이상의 매개변수를 지정해주어야 한다.

```csharp
class Example {
    public int this[int n_idx] {
        get { return n_idx + 1; }
    }
}

class Program {
    static void Main(string[] args) {
        Example ex = new Example();
        Console.WriteLine(ex[0]);
        // result : 1
    }
}
```

연산자 오버로딩이 어떻게 작동하는지 확인하기 위해 복소수(Complex)를 표현하고 연산하기 위해 프로그램을 작성한다고 가정한다.

```csharp
struct Complex {
    // 복소수란 실수와 허수로 이루어진 수
    // a + bi로 표현한다.
    double real;      // 실수부
    double imaginary; // 허수부

    public Complex(double real, double imaginary) {
        this.real = real;
        this.imaginary = imaginary;
    }
}
```

다음은 형변환을 위한 암시적, 명시적 형 변환 연산자의 구현이다. 모든 연산자 정의에는 `operator` 키워드가 필요하며 암시적 변환에는 `implicit`, 명시적 변환에는 `explicit` 키워드가 필요하다.

```csharp
// 암시적
public static implicit operator Complex(double real) {
    return new Complex(real, 0.0);
}
// Complex c = 1.0d;

// 명시적
public static explicit operator Complex(double real) {
    return new Complex(real, 0.0);
}
// Complex c = (Complex)1.0d;

public static explicit operator double(Complex c) {
    return Math.Sqrt(c.real * c.real + c.imaginary * c.imaginary);
}
// double d = (double)c;
```

다음은 `true`, `false`의 연산자 오버로딩이다. 참 혹은 거짓 어느 한쪽 하나만 구현은 불가하고, 하나 구현시 반대쪽도 무조건 구현해줘야 한다.

```csharp
public static bool operator true(Complex c) {
    return Math.Abs(c.real) > double.Epsilon || Math.Abs(c.imaginary) > double.Epsilon;
}
// Complex c = new Complex(0);
// if(c)

public static bool operator false(Complex c) {
    return Math.Abs(c.real) <= double.Epsilon || Math.Abs(c.imaginary) <= double.Epsilon;
}
```

다음은 필수 산술 연산자에 대한 오버로딩이다.

```csharp
public static Complex operator +(Complex c1, Complex c2) 
{
    return new Complex(c1.re + c2.re, c1.im + c2.im);
}

public static Complex operator -(Complex c1, Complex c2) 
{
    return new Complex(c1.re - c2.re, c1.im - c2.im);
}

public static Complex operator *(Complex c1, Complex c2) 
{
    return new Complex(c1.re * c2.re - c1.im * c2.im, c1.re * c2.im + c1.im * c2.re);
}

public static Complex operator /(Complex c1, Complex c2) 
{
    double nrm = Math.Sqrt(c2.re * c2.re + c2.im * c2.im);
    return new Complex((c1.re * c2.re + c1.im * c2.im) / nrm, (c2.re * c1.im - c2.im * c1.re) / nrm);
}

// Complex c1 = 2.0; // Implicit cast in action
// Complex c2 = new Complex(0.0, 4.0);
// Complex c3 = c1 * c2;
// Complex c4 = c1 + c2;
// Complex c5 = c1 / c2;
// double d = (double)c5; // Explicit cast in action
```

```cpp
#include "pch.h"
#include <iostream>

using namespace std;

struct Complex {
	double real;
	double img;

	Complex(double r, double i) : real(r), img(i) { }

	// 형변환 연산자는 멤버로만 가능하다.
	operator double() {
		return sqrt(real * real + img * img);
	}
};

Complex operator+(const Complex c1, const Complex c2) {
	return Complex(c1.real + c2.real, c1.img + c2.img);
}

Complex operator-(const Complex c1, const Complex c2) {
	return Complex(c1.real - c2.real, c1.img - c2.img);
}

Complex operator*(const Complex c1, const Complex c2) {
	return Complex(c1.real * c2.real - c2.img * c2.img, c1.real * c2.img + c1.img * c2.real);
}

Complex operator/(const Complex c1, const Complex c2) {
	double nrm = sqrt(c2.real * c2.real + c2.img * c2.img);
	return Complex((c1.real * c2.real + c1.img * c2.img) / nrm, (c2.real * c1.img - c2.img * c1.real) / nrm);
}

int main() {
	double d = Complex(1, 1);
	Complex c1 = Complex(1, 1);
	Complex c2 = Complex(2, 2);
	Complex c3 = c1 + c2;
	cout << c3.real << " " << c3.img << endl;
	Complex c4 = c1 - c2;
	cout << c4.real << " " << c4.img << endl;
	Complex c5 = c1 * c2;
	cout << c5.real << " " << c5.img << endl;
	Complex c6 = c1 / c2;
	cout << c6.real << " " << c6.img << endl;
}
```

```python
import math

class Complex:
    def __init__(self, r, i):
        self.r = r
        self.i = i
        
    def __add__(c1, c2):
        return Complex(c1.r + c2.r, c1.i + c2.i)
    def __sub__(c1, c2):
        return Complex(c1.r - c2.r, c1.i - c2.i);
    def __mul__(c1, c2):
        return Complex(c1.r * c2.r - c2.i * c2.i, c1.r * c2.i + c1.i * c2.r);
    def __truediv__(c1, c2):
        nrm = math.sqrt(c2.r * c2.r + c2.i * c2.i);
        return Complex((c1.r * c2.r + c1.i * c2.i) / nrm, (c2.r * c1.i - c2.i * c1.r) / nrm);
        
c1 = Complex(1, 1)
c2 = Complex(2, 2)
c3 = c1 + c2
print(c3.r, c3.i)
c4 = c1 - c2
print(c4.r, c4.i)
c5 = c1 * c2
print(c5.r, c5.i)
c6 = c1 / c2
print(c6.r, c6.i)
```

## 반복기

C#의 `foreach`는 일반 for문에 비해 몇가지 단점이 있지만 일반적으로 사용되는 루프이다. 하나의 큰 단점은 루프 반복자의 불변성이다. 따라서 다음 코드는 불가능하다.

```csharp
var arr = new [] { 1, 2, 3, 4, 5 };
foreach(var value in arr)
    value = value * value;
```

이는 `foreach` 루프가 IEnumerable<T> 인터페이스를 구현하는 요소에서만 자연스럽게 작동하기 때문이다.

IEnumerable 인터페이스에는 foreach 구문에서 필요한 IEnumerator 개체를 반환하는 GetEnumerator 메소드를 제공한다.

```csharp
namespace System.Collections {
    // 제네릭이 아닌 컬렉션에서 단순하게 반복할 수 있도록 지원하는 열거자를 노출합니다. 
    public interface IEnumerable {
        IEnumerator GetEnumerator();
    }
}
namespace System.Collections {
    // 제네릭이 아닌 컬렉션을 단순하게 반복할 수 있도록 지원합니다.
    public interface IEnumerator {
        object Current { get; }
        bool MoveNext();
        void Reset();
    }
}
```

```csharp
class Example : IEnumerable<int> {
    public IEnumerator<int> GetEnumerator() {
        return new Squares();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return new Squares();
    }

    class Squares : IEnumerator<int> {
        int m_n_number;
        int m_n_current;

        public int Current {
            get { return m_n_current; }
        }

        object IEnumerator.Current {
            get { return m_n_current; }
        }

        public bool MoveNext() {
            m_n_number++;
            m_n_current = m_n_number * m_n_number;
            return true;
        }

        public void Reset() {
            m_n_number = 0;
            m_n_current = 0;
        }

        public void Dispose() { }
    }
}

class Program {
    static void Main(string[] args) {
        var squares = new Example();
        foreach(var square in squares) {
            Console.WriteLine(square);
            if (square == 100)
                break;
        }
    }
}
```

```cpp
template<typename T>
class Node {
public:
	T data;
	Node* next;

	Node(T t, Node *n) : data(t), next(n) { }
};

template<typename T>
class ilist_iterator {
private:
	Node<T> *current;
public:
	ilist_iterator(Node<T> *p = 0) : current(p) { }

	ilist_iterator& operator++() {
		current = current->next;
		return *this;
	}

	T& operator*() {
		return current->data;
	}

	bool operator==(const ilist_iterator& p) {
		return current == p.current;
	}
	bool operator!=(const ilist_iterator& p) {
		return current != p.current;
	}
};

template<typename T>
class ilist {
private:
	Node<T> *head;
public:
	ilist() : head(0) { }

	void push(const T& item) {
		head = new Node<T>(item, head);
	}

	typedef ilist_iterator<T> iterator;

	iterator begin() {
		return iterator(head);
	}
	iterator end() {
		return iterator(0);
	}
};

int main() {
	ilist<int> temp;
	
	for (int n_idx = 0; n_idx < 10; n_idx++) {
		temp.push(n_idx + 1);
	}

	ilist<int>::iterator it = temp.begin();

	while (it != temp.end()) {
		cout << *it << endl;
		++it;
	}
}
```

```java
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;

public class Main {
	public static void main(String args[]) {
		Example<String> ex = new Example<String>();
		ex.Add("A");
		ex.Add("b");
		ex.Add("C");
		ex.Add("d");
		
		Iterator<String> it = ex.iterator();
		while(it.hasNext()) {
			String item = it.next();
			System.out.println(item);
		}
	}
}

class Example<T> implements Iterable<T> {
	List<T> l_temp = new ArrayList<T>();
	
	void Add(T item) {
		l_temp.add(item);
	}
	
	@Override
	public Iterator<T> iterator() {
		return new Iterator<T>() {
			int n_curr = 0;
			
			@Override
			public boolean hasNext() {
				return n_curr < l_temp.size();
			}

			@Override
			public T next() {
				return l_temp.get(n_curr++);
			}
		};
	}
}
```

```objc
```

```python
```

## yield 문

C#의 yield 키워드는 호출자에게 컬렉션 데이터를 하나씩 리턴할 때 사용한다. Enumerator(Iterator) 라고 불리우는 이 기능은 데이터 집합으로부터 데이터를 하나씩 호출자에게 보내주는 역할을 한다.

yield는 `yield return` 또는 `yield break` 와 같은 2가지 방식으로 사용되는데 첫번째인 `yield return`은 데이터를 하나씩 리턴하는데 사용되고, 두번째인 `yield break`는 리턴을 중지하고 루프를 빠져 나올 때 사용된다.

위의 반복기에서 yield 문을 사용하면 더 간단하게 코드 작성이 가능하다.

```csharp
class Example : IEnumerable<int> {
    public IEnumerator<int> GetEnumerator() {
        for(int n_idx = 1; ; n_idx++) {
            int n_val = n_idx * n_idx;
            if (n_val > 100)
                yield break;

            yield return n_val;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}

class Program {
    static void Main(string[] args) {
        var squares = new Example();
        foreach(var square in squares)
        {
            Console.WriteLine(square);
        }
    }
}
```

## 공변성(Covariance) 반공변성(Contravariance)

공변성과 반공변성을 통칭하여 가변성이라고 부르는 경향도 있다. 반대되는 의미로 불변성이 있다.

* 공변성(Covariant) : X -> Y 가 가능할 때 `C<T> 가 C<X> -> C<Y>`로 가능하다면 이는 공변이다. 작은 타입을 큰 타입으로 변환할 수 있게 해준다. `out` 키워드를 사용하여 타입을 공변으로 만들 수 있다.
* 반공변성(Contravariant) : X - > Y 가 가능할 때 `C<T> 가 C<Y> -> C<X>` 로 사용 가능하다면 이는 반공변이다. 큰 타입을 더 작은 타입으로 변환할 수 있게 해준다. `in` 키워드를 사용하여 타입을 반공변으로 만들 수 있다.

* 불변성(Invariant) : X -> Y 가 가능하더라도 `C<X> 는 C<X>` 로만 사용할 수 있다. 기본적으로 제네릭은 불변이다.

```csharp
class Base { }

class Derived : Base { }

interface Covariance<out T> { }     // 공변성
interface Contravariance<in T> { }  // 반공변성
interface Invariance<T> { }         // 불변성

class Program {
    static void Main(string[] args) {
        Covariance<Base> a = (Covariance<Derived>)null; // 컴파일 가능
        Covariance<Derived> b = (Covariance<Base>)null; // 컴파일 에러

        Contravariance<Base> c = (Contravariance<Derived>)null; // 컴파일 에러
        Contravariance<Der41ived> d = (Contravariance<Base>)null; // 컴파일 가능

        // 둘다 컴파일 에러
        Invariance<Base> e = (Invariance<Derived>)null;
        Invariance<Derived> f = (Invariance<Base>)null;
    }
}
```

### 공변성

> X -> Y 가 가능할 때 `C<T> 가 C<X> -> C<Y>` 로 가능하다면 이는 공변이다.

제네릭타입은 기본적으로 불변성이기 때문에 class C<T> 가 정의되어 있더라도 C<Base> 에 C<Derived> 를 할당할 수 없다.

하지만 C#의 대표적인 IEnuerable<T>는 IEnumerable<Base> 변수에 IEnumerable<Derived> 인스턴스를 할당할 수 있는데 그 이유는 IEnumerable이 공변적(`<out T>`)으로 지정되었기 때문이다.

```csharp
    [TypeDependencyAttribute("System.SZArrayHelper")]
    public interface IEnumerable<out T> : IEnumerable {
        IEnumerator<T> GetEnumerator();
    }

IEnumerable<Base> temp = new List<Derived>(); // ok
```

C#에서 `out` 키워드는 공변성(Covariant)를 의미하고 이는 출력위치에서만 쓰겠다는 것을 의미한다. 출력 위치라 함은 아래와 같다.

* 함수의 반환 타입
* Get 접근자

제네릭의 공변성이 왜 출력위치에서만 쓰여야하냐면 출력위치가 아닌 입력위치에서 쓰이면 안전하지 않게 된다. 

```csharp
// C#의 배열은 공변적이다. 해당 코드는 문제가 없다.
Base[] temp = new Derived[5];
temp[0] = new Derived();
Base item = temp[0];
item.Print();

// 타입에 대해 안전하지 않다.
object[] temp = new string[5];
temp[0] = 10;
```

### 반공변성

> X -> Y 가 가능할 때 `C<T> 가 C<Y> -> C<X>` 로 사용 가능하다면 이는 반공변

위와 반대로 반공변성은 `in` 키워드를 제네릭 타입 앞에 붙힘으로서 지정할 수 있다. `in`을 지정하게 되면 컴파일러에게 이 타입을 입력위치에 쓰겠다고 알려주게 된다.

* 함수의 인자 타입
* Set 접근자

C# 에서는 IComparable<T> 인터페이스가 `in` 제네릭 한정자를 사용한다. 

```csharp
class Base : IComparable<Derived> {
    public int CompareTo(Derived other) {
        throw new NotImplementedException();
    }
}

class Program {
    static void Main(string[] args) {
        IComparable<Derived> temp = new Base();
    }
}
```

```csharp
abstract class Shape {
    public virtual double area { get { return 0; } }
}

class Circle : Shape {
    double r;

    public Circle(double radius) { r = radius; }
    
    public override double area { get { return Math.PI * r * r; } }
}

class ShapeAreaComparer : IComparer<Shape> {
    int IComparer<Shape>.Compare(Shape a, Shape b) {
        if (a == null)
            return b == null ? 0 : -1;
        return b == null ? 1 : a.area.CompareTo(b.area);
    }
}

class Program {
    static void Main() {
        SortedSet<Circle> circlesByArea = new SortedSet<Circle>(new ShapeAreaComparer()) {
            new Circle(7.2), new Circle(100), null, new Circle(.01)
        };

        foreach (Circle c in circlesByArea) {
            Console.WriteLine(c == null ? "null" : "Circle with area " + c.area);
        }
    }
}
```

공변성과 마찬가지로 반공변성이 출력위치에서 사용할수 있다고 가정하면, 입력할때는 아무값이나 넣을 수 있지만 반환되는 값에 대해선 어떤 타입인지 전혀 추론이 불가능하기 때문이다.

## Attributes

특성은 컴파일러에게 코드에 대한 추가 정보를 제공한다. 보통 시스템이 제공하는 공통 Attribute와 직접 정의하는 CustomAttribute로 구분한다.

모든 특성은 `Attribute` 클래스를 상속받아 구현됩니다. `AttributeUsage` 라는 특성을 사용하여 해당 특성이 어느 구문에서 유효한지 설정이 가능

```csharp
[AttributeUsage(AttributeTargets.Class)]
class LinkAttribute : Attribute {
    public LinkAttribute(string url) {
        Url = url;
    }

    public string Url { get; private set; }
}
```

속성을 사용할때는 `Attribute` 이름은 생략이 가능하다. 속성의 경우 컴파일러에게 의미있는 값을 나타내지 않으므로 따로 읽어들여야 한다. 다음은 해당 속성의 값을 리플렉션을 사용하여 읽어오는 예제이다.

```csharp
[Link("http://www.google.com")]
public class Google {
    public string GetLink(Google page) {
        var attrs = page.GetType().GetCustomAttributes(typeof(LinkAttribute), false);

        if (attrs.Length == 0)
            return string.Empty;
        return ((LinkAttribute)attrs[0]).Url;
    }
}
```

## Unsafe Code

C# 에서는 네이티브와 같은 방식으로 직접 메모리에 엑세스가 가능하다.
다음은 C 에서의 코드이다.

```c
int main() {
    int a = 35181;
    byte* b = (byte*)&a;
    byte b1 = *b;
    byte b2 = *(b + 1);
    byte b3 = *(b + 2);
    byte b4 = *(b + 3);
    return 0;
}
```

C# 에서는 `unsafe` 라는 키워드를 통해 위와 같이 작성이 가능하다. 프로젝트 속성에서 `안전하지 않은 코드 허용` 을 체크해줘야 한다.

```csharp
class Program {
    unsafe static void Main(string[] args) {
        int a = 35181;
        byte* b = (byte*)&a;
        byte b1 = *b;
        byte b2 = *(b + 1);
        byte b3 = *(b + 2);
        byte b4 = *(b + 3);
    }
}
```

위와 같이 `unsafe` 코드를 작성하게 되면 CLR에서 메모리 정리를 하면서 언제 이동시킬지 모르기 때문에 `fixed` 라는 키워드와 같이 사용하여야 한다. `fixed`는 가비지가 이동 가능한 변수를 재배치할 수 없도록 하는 키워드이다.

```csharp
class Program {
    static void Main(string[] args) {
        int[,] arr = new int[5,5];
        unsafe {
            fixed(int *p = arr) {
                for(int n_i = 0; n_i < arr.Length; n_i++)
                {
                    p[n_i] = n_i;
                }
            }
        }
    }
}
```

## Communication between native and managed code

Unmanaged Code와 통신을 할 경우 데이터를 넘겨야 할 때가 있는데 이 때 C# 에서는 특정한 방법으로 구조체를 선언 후 넘겨주어야 한다.

```csharp
struct Example {
    public int i;    // 4 byte
    public double d; // 8 byte
    public char c;   // 1 byte
}
```
위 구조체의 경우 이론적으로 13 바이트의 크기를 가진 구조체 이지만 실제로 크기를 출력해보면 24바이트로 나온다.

```csharp
Marshal.SizeOf(typeof(Example))
```

이는 필드를 메모리 바운더리 상에 정렬하는 Field Alignment 규칙에 따른것이다. 즉, 위의 구조체의 경우 8바이트 바운더리에 맞춘것이다. 이러한 Alignment는 닷넷에서 자동으로 처리되는데 정확히 13바이트로 존재해야하는 경우 다음과 같이 사용한다.

```csharp
[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct Example {
    public int i;
    public double d;
    public char c;
}
```

`Pack` 이라는 속성을 지정하여 특정 바이트 크기로 정렬을 할 수 있다. 위의 `Pack = 1`은 1바이트로 정렬된다는 의미로 이를 지정하면 위 구조체는 정확히 13바이트를 갖게 된다.

또 한가지 문제로는 각 필드의 순서가 틀려서는 안된다는 것이다. C# 에서는 클래스와 같은 레퍼런스 타입에 대해서 필드의 순서를 자동으로 변경하는데 (Auto Layout 이라고 함) 이러한 타입의 레이아웃은 Managed 영역 밖으로 데이터를 Export 하지 못한다. Managed Memory 에서도 필드 순서가 유지되도록 하려면 아래와 같은 방법을 사용해야 한다.

```csharp
[StructLayout(LayoutKind.Explicit, Pack = 1)]
class Example {
    [FieldOffset(0)]
    public int i;
    [FieldOffset(4)]
    public double d;
    [FieldOffset(12)]
    public char c;
}
```

C# 에서 기존의 Native DLL의 함수를 호출할 필요가 있을 때, `DllImport(name)` 이라는 속성을 사용하여 호출한다.

```csharp
class Program {
    [DllImport("Kernel32.dll")]
    public static extern bool Beep(uint freqeuncy, uint duration);

    static void Main(string[] args) {
        Beep(100, 1000);
        Beep(200, 1000);
        Beep(300, 1000);
    }
}
```