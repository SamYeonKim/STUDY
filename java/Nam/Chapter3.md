# C# 코드 적어라 꼭

# How to design Classes and Interfaces

## Interfaces

인터페이스란 클래스들이 구현해야 하는 동작을 지정하는데 사용되는 추상형이다.
어떤 객체가 있고 그 객체가 특정한 인터페이스를 사용한다면 그 객체는 반드시 인터페이스의 메소드들을 구현해야 한다.

인터페이스는 클래스 뒤에 `implements` 라는 키워드를 붙혀서 사용한다.

```java
// java
interface IHello {
    void PrintHello();
}

class Hello implements IHello {
    public void PrintHello() {
        System.out.println("Hello World");
    }
}
```

-----

```csharp
// csharp
interface IHello {
    void PrintHello ();
}

class Hello : IHello {
    public void PrintHello() {
        Console.WriteLine("Hello World!");
    }
}
```

### 인터페이스의 특성

인터페이스 사용시 컴파일러가 강제하는 암시적 제약조건이 존재한다.

1. 모든 인터페이스의 단일 메소드의 접근 제한자는 `public` 이며 암시적으로 `abstract`로 선언된다. 따라서 밑에 세 유형은 모두 같은 메소드이다.

```java
void PrintHello();
public void PrintHello();
public abstract void PrintHello();
```

2. 변수(상수 필드) 선언시 `public`, `static`, `final`이 암시적으로 붙는다.

```java
String m_str = "Hello";
public static final String m_str = "Hello";
```

3. 인터페이스 내 클래스, 인터페이스, 열거형은 `public` 이며 `static` 이 붙는다.


```java
class Hello { }
static class Hello { }
```

-----

C#의 경우 자동으로 `public`이 붙긴 하지만 명시적으로 접근 제한자를 사용할 수는 없다. 또한 `static`을 사용할 수 없다.

```csharp
// csharp
void PrintHello();
// compile error
// public void PrintHello();
// abstract void PrintHello();
// static void PrintHello();
// string m_str = "Hello";
```

## Maker Interfaces

마커 인터페이스는 메소드가 정의되어 있지 않은 특별한 종류의 인터페이스이다.

```java
public interface Cloneable { }
public interface Serializable { }
```

위 두 인터페이스는 자바에서 사용되는 마커 인터페이스로 대표적인 인터페이스이다.

자바에서 인스턴스화된 객체를 전달하기 위해 직렬화(Serialization)을 사용하는데 `Serializable` 인터페이스를 `implements` 하지 않았다면 직렬화를 할 수 없다.

마커 인터페이스는 구현해야 할 메소드가 하나도 없고, 단지 객체가 어떤 특성(복제 `Cloneable`, 직렬화 `Serializable`)이 가능한 대상임을 알려주는 일종의 타입 확인 용도인 마커 역할을 한다.
따라서 인터페이스의 주된 목적을 충족시키지는 않지만 객체 지향 디자인을 실현하는데에 위치가 있다.

## Functional interfaces, default and static methods

자바 8의 출시로 인터페이스에서 정적 메소드, 디폴트 메소드, 람다의 사용이 가능하다.

### Default Method

위 설명에서 인터페이스는 '메소드만 선언하고 구현을 불가하다.' 라는걸 강조해왔지만 `default` 키워드를 사용하면 인터페이스는 메소드 구현이 가능하다.

```java
interface IExample {
    default void Hello() {
        System.out.println("IExample - Hello World");
    }
}

class Example implements IExample { }

public class Main {
    public static void main(String args[]) {
        Example ex = new Example();
        ex.Hello();
    }
}
```

---

C# 8.0 에서 인터페이스의 'Default Method' 기능 추가가 제안되긴 하였으나 아직 사용하지 못함

### Static Method

디폴트 메소드는 인스턴스 메소드이기 때문에 각 구현자 객체에 의해 오버라이드 될 수 있지만, 정적 메소드도 사용이 가능하다.

```java
interface IExample {
    static void Hello() {
        System.out.println("IExample - Hello World");
    }
}
```

### Functional Interface

```java
interface IMovable {
    void Move();
}

// Main
IMovable movable = new IMovable() {
    public void Move() {
        System.out.println("Anonymous - Move()");
    }
};
```

위 코드는 익명클래스 객체를 구현하여 사용하고 있다. 어떤 방식을 쓰던 아무리 못해도 5줄은 필요하다.

#### Lambda Expression

함수형 인터페이스의 핵심은 '지울 수 있는건 모두 지우자' 이다.

```java
IMovable movable = () -> System.out.println("Lambda - Move()");
// IMovable movable = (arg) -> System.out.println("Lambda - Move() + arg");
movable.Move();
```

5줄이던 코드가 1줄로 줄어들었다. 위 코드처럼 람다 표현식 사용이 가능하다.

위 코드에서 확인한 람다 표현식은 '구현해야될 추상 메소드가 1개인 인터페이스' 를 구현한 것이다. 인터페이스를 구현하는데 어짜피 메소드가 1개뿐이니 다 지워버린것이다. 그럼 메소드가 2개인 경우는 어떻게 해야할까???

람다로는 지원하지 않는다. 람다 표현식으로 구현이 가능한 인터페이스는 오직 추상 메소드가 1개뿐인 인터페이스만 가능하며 그렇기 때문에 추상 메소드가 1개인 인터페이스를 부르는 명칭이 추가되었다. 그것이 함수형 인터페이스이다.

```java
@FunctionalInterface
interface IMovable {
    void Move();
    void Stop();
}
```

인터페이스에는 추상메소드 갯수를 제한하는 방법은 없으므로 람다 표현식이 있는걸 모르고 추상메소드를 추가할 수 있다. 이런 불상사를 막기 위해 `@FunctionalInterface`라는 어노테이션이 존재한다.

위 어노테이션은 추상메소드가 1개가 아닐경우 컴파일 에러를 낸다.

## Abstract Class

추상클래스의 개념 자체는 인터페이스와 아주 유사하다. 일단 일반 클래스와 달리 인스턴스화 될 수 없고 상속을 통해서만 사용이 가능하다. 또한 추상 메소드를 포함할 수 있다.

```java
public abstract class AbstractExample {
	public void Hello() {
		System.out.println("AbstractExample - Hello World");
	}
	public abstract void Hello2();
}

class Example extends AbstractExample {
	public void Hello2() {
		System.out.println("AbstractExample - Hello World2");
	}
}

// Main
Example ex = new Example2();
ex2.Hello();
ex2.Hello2();

// compile error
// AbstractExample ex2 = new AbstractExample 
```

-----

C#의 추상클래스도 자바와 똑같지만 다른점은 자바의 추상클래스는 모두 가상 메소드이지만 C#의 경우 `virtual` 키워드나 `abstract` 키워드로 명시적으로 선언해야지만 파생클래스에서 오버라이드하여 사용이 가능하다.

```csharp
// csharp
abstract class AbstractExample {
    public void PrintHello() {
        Console.WriteLine("AbstractExample - Hello World!");
    }

    public virtual void PrintHello2() {
        Console.WriteLine("AbstractExample - Hello World2!");
    }

    public abstract void PrintHello3();
}

class Example : AbstractExample {
    // compile error
    // public override void PrintHello() { }

    public override void PrintHello2() {
        Console.WriteLine("Inherit AbstractExample - Hello World2!");
    }

    public override void PrintHello3() {
       Console.WriteLine("Inherit AbstractExample - Hello World3!");
    }
}
```

## Immutable Classes

불변 클래스는 멀티 코어 시스템에서 유용하게 사용되는 클래스이다. 자바는 클래스의 불변성에 대해 강력하게 지원하진 않지만 디자인 하는것은 가능하다.

### 불변 클래스의 특징

* 생성자, 접근 메소드에 대한 방어 복사가 필요없다. 즉 불변(Thread-Safe)이기 때문에 객체가 변경에 대해 안전하다.
* 객체가 가지는 값 마다 새로운 객체가 필요하다. 새로운 객체를 계속 생성하기 때문에 메모리 관련 문제와 성능저하를 일으킨다.

자바의 대표적인 불변 클래스로는 `String` 클래스가 있다.

```java
String str = "Nam";
str.concat("Wonki");
```

위 두줄을 수행할 경우 str 변수가 참조하는 값을 변경하는 것이 아니고 새로운 문자열을 `new` 해서 반환한다.

불변 클래스를 디자인 하는 방법으로 3가지 규칙을 지켜야한다.

1. 클래스의 모든 필드가 `final` 이여야 한다.

```java
class ImmutableClass {
    private final long id;
	private final String[] arrayOfStrings;
	private final ArrayList<String> listOfString;
}
```

2. 적절한 초기화를 수행해야 한다. 필드가 배열이나 콜렉션에 대한 참조일 경우 해당 필드를 직접 할당하지 말고 생성자 인수에서 사본을 작성하여 할당하시오

```java
public ImmutableClass(final long id, final String[] arrayOfString, final ArrayList<String> listOfString) {
		this.id = id;
		this.arrayOfStrings = Arrays.copyOf(arrayOfString, arrayOfString.length);
		this.listOfString = new ArrayList<String>(listOfString);
	}
```

3. 올바른 접근자(getter)를 제공하십시오

* 콜렉션의 경우 Collections.unmodifiableCollection 함수를 사용하여 노출시켜야 한다.
```java
public ArrayList<String> getArrayListOfString() {
		return (ArrayList<String>) Collections.unmodifiableList(listOfString);
	}
```
* 배열의 경우 불변성을 보장하는 유일한 방법은 배열에 대한 참조를 반환하는 대신 복사본을 제공하는 것이다. (성능면에서 유용한지는 의문)
```java
public String[] getArrayOfString() {
		return Arrays.copyOf(arrayOfStrings, arrayOfStrings.length);
	}
```

-----

```csharp
// csharp
class ImmutableClass {
    readonly long id;
    readonly string[] arrayOfStrings;
    readonly List<string> listOfStrings;

    public ImmutableClass(long id, string[] arrayOfStrings, List<string> listOfStrings) {
        this.id = id;
        Array.Copy(arrayOfStrings, this.arrayOfStrings, arrayOfStrings.Length);
        this.listOfStrings = new List<string>(listOfStrings);
    }
}
```

## Anonymous Class

위 설명에서 Functional interfaces 에서 한번 등장하였다. 익명 클래스는 복잡한 과정 없이 내부 클래스 정의와 즉각적인 인스턴스화를 제공한다.

```java
class Anonymous {
	public int m_n_num = 1;
	public int GetNum() {
		return m_n_num; 
	}
}

// Ex1 - Main
Anonymous ex = new Anonymous() {
    int m_n_num = 10;
    public int GetNum() {
        return m_n_num;
    }
};
System.out.println(ex.GetNum());

Anonymous ex2 = new Anonymous();
System.out.println(ex2.GetNum());

// Output1
10
1

// Ex2 - Main
Anonymous ex = new Anonymous() {
    int m_n_num = 10;
};
System.out.println(ex.GetNum());

Anonymous ex2 = new Anonymous();
System.out.println(ex2.GetNum());

// Output2
1
1

// ------------------
interface IMovable {
    void Move();
}

// Ex3 - Main
IMovable movable = new IMovable() {
    public void Move() {
        System.out.println("Anonymous - Move()");
    }
};

// Output3
Anonymous - Move()
```

위 코드 경우 해당 형태로 사용하지는 않지만 익명 클래스를 사용하는 단적인 예이다.
익명 클래스를 사용하려면 첫째로 해당 함수가 무조건 '선언'이 되어있어야 하며, 함수 자체도 내부에서 다시 구현하여야 원하는 결과를 얻을 수 있다.

보통의 경우 인터페이스 자체를 익명 클래스로서 즉각 구현하여 사용한다.