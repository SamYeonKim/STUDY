# How and when to use Generics

+ Genric은 `일반적인 이란 뜻`을 가진 단어.
+ 일반적인 코드를 작성하고, 이 코드를 `다양한 타입의 객체에 대하여 재사용`하는 프로그래밍 기법.

## Interfaces

`interface [Interface Name] <T>`

+ 인터페이스로 하나의 Type을 받을 경우.

```java
[Java Code]

interface A <T> {
    void SetMethod (T t);
    T GetMethod();
}

class B implements A<String> {
    String m_a;
    
    @Override
    public void SetMethod(String t) {
        m_a = t;
    }
    
    @Override
    public String GetMethod() {
        return m_a;
    }
}

public class MainClass{
     public static void main(String []args){
         B b = new B();
         b.SetMethod("HI");
         System.out.println(b.GetMethod());
     }
}
```

```csharp
[C# Code]

using System;
					
interface A <T> {
    void SetMethod (T t);
    T GetMethod();
}

class B : A<String> {
    String m_a;
    
    public void SetMethod(String t) {
        m_a = t;
    }
    
    public String GetMethod() {
        return m_a;
    }
}

public class Program {
	public static void Main() {
		B b = new B();
        b.SetMethod("HI");
		Console.WriteLine(b.GetMethod());
	}
}
```

`interface [Interface Name] <T1, T2>`

+ 인터페이스로 두개 이상의 Type을 받을 경우.

```java
[Java Code]

interface A <T1, T2> {
    void SetMethod (T1 t, T2 t2);
    T1 GetMethod();
    T2 GetMethod2();
}

class B implements A<String, Integer> {
    String m_a;
    Integer m_b;
    
    @Override
    public void SetMethod(String t, Integer t2) {
        m_a = t;
        m_b = t2;
    }
    
    @Override
    public String GetMethod() {
        return m_a;
    }
    
    @Override
    public Integer GetMethod2() {
        return m_b;
    }
}

public class MainClass{
     public static void main(String []args){
         B b = new B();
         b.SetMethod("HI", 1);
         
         System.out.println(b.GetMethod());
         System.out.println(b.GetMethod2());
     }
}
```

```csharp
[C# Code]

using System;
					
interface A <T1, T2> {
    void SetMethod (T1 t, T2 t2);
    T1 GetMethod();
    T2 GetMethod2();
}

class B : A<String, int> {
    String m_a;
    int m_b;
    
    public void SetMethod(String t, int t2) {
        m_a = t;
        m_b = t2;
    }
    
    public String GetMethod() {
        return m_a;
    }
    
    public int GetMethod2() {
        return m_b;
    }
}

public class Program {
	public static void Main() {
		B b = new B();
        b.SetMethod("HI", 1);
       
		Console.WriteLine(b.GetMethod());
		Console.WriteLine(b.GetMethod2());
	}
}
```

## Classes

`class [Class Name] <T>`

+ 클래스를 이용한 경우.

```java
[Java Code]

class A <T> {
    T m_a;
    
    void SetMethod (T t) {
        m_a = t;
    };
    
    T GetMethod() {
        return m_a;
    };
}

public class MainClass{
     public static void main(String []args){
         A a = new A<String>();
         a.SetMethod("HI");
         
         System.out.println(a.GetMethod());
     }
}
```

```csharp
[C# Code]

using System;
					
class A <T> {
    T m_a;
    
    public void SetMethod (T t) {
        m_a = t;
    }
    
    public T GetMethod() {
        return m_a;
    }
}

public class Program {
	public static void Main() {
		A<String> a = new A<String>();
        a.SetMethod("HI");
       
		Console.WriteLine(a.GetMethod());
	}
}
```

## Methods

`<T>[Return Type]<T> [Method Name] ()`

+ 메소드에만 선언하여 사용하려는 경우.

```java
[Java Code]

import java.util.ArrayList;
import java.util.List;

class A {
    static <T>List<T> SetMethod (List<T> t, T t2) {
        t.add(t2);
        return t;
    };
}

public class MainClass{
     public static void main(String []args){
         List<String> a = new ArrayList();
         a.add("Hello");
         
         a = A.SetMethod(a, "World");
         
         System.out.println(a.get(0) + " " + a.get(1));
     }
}
```

```csharp
[C# Code]

using System;
using System.Collections.Generic;
					
class A {
    public List<T> SetMethod<T> (List<T> t, T t2) {
        t.Add(t2);
        return t;
    }
}

public class Program {
	public static void Main() {
		A a = new A();
		List<String> b = new List<String>();
        b.Add("Hello");
         
        b = a.SetMethod<String>(b, "World");
       
		Console.WriteLine(b[0] + " " + b[1]);
	}
}
```

## Limitation

+ 비객체형 타입(primitive type)인 경우 generics에서 사용하지 못하므로 wrapper를 사용해야 한다.
+ Type Erasure???

| Primitive Type | Wrapper Class|
|---|:---|
| `byte` | Byte |
| `short` | Short |
| `int` | Integer |
| `long` | Long |
| `float` | Float |
| `double` | Double |
| `boolean` | Boolean |
| `character` | Character |
|||

## Wildcards and bounded types

`<?>`
  + 모든 클래스나 인터페이스 타입이 올 수 있다.

`<? Super 하위 타입>`
+ 상위 타입만 올 수 있습니다.

`<? extends 상위 타입>`
+ 하위 타입만 올 수 있습니다.

```java
[Java Code]

class House<T> {
    Person m_person;
    
    House(Person person) {
        m_person = person;
    }
    
    void SayMyName() {
        m_person.SayMyName();
    }
}

class Person {
    void SayMyName() {
        System.out.println("Person");
    }
}
class Gijung extends Person {
    @Override
    void SayMyName() {
        System.out.println("Gijung");
    }
}
class Hands extends Gijung {
    @Override
    void SayMyName() {
        System.out.println("Gijung's Hands");
    }
}

class WildTest {
    /// 모두 처리 가능.
    void SayWildName(House<?> person) {
        person.SayMyName();
    }
    
    /// Person, Gijung 클래스만 처리 가능
    void SayUpperWildName(House<? super Gijung> gijung) {
        gijung.SayMyName();
    }
    
    /// Gijung, Hands 클래스만 처리 가능
    void SayLowerWildName(House<? extends Gijung> gijung) {
        gijung.SayMyName();
    }
}

public class MainClass{
     public static void main(String []args){
         WildTest a = new WildTest();
         
         Person p = new Person();
         Gijung g = new Gijung();
         Hands h = new Hands();
         
         House<Person> h_p = new House<>(p);
         House<Gijung> h_g = new House<>(g);
         House<Hands> h_h = new House<>(h);
         
         a.SayWildName(h_p);
         a.SayWildName(h_g);
         a.SayWildName(h_h);
         System.out.println("----- End -----");
         
         a.SayUpperWildName(h_p);
         a.SayUpperWildName(h_g);
         //a.SayUpperWildName(h_h);
         System.out.println("----- End -----");
         
         //a.SayLowerWildName(h_p);
         a.SayLowerWildName(h_g);
         a.SayLowerWildName(h_h);
         System.out.println("----- End -----");
     }
}
```

![](Images/Result_1.png)

```csharp
[C# Code]

using System;
					
class House<T> where T : Gijung {   // WildeCard 대신 제약조건으로 처리.
    Person m_person;
    
    public House(Person person) {
        m_person = person;
    }
    
    void SayMyName() {
        m_person.SayMyName();
    }
}

class Person {
    public virtual void SayMyName() {
        Console.WriteLine("Person");
    }
}
class Gijung : Person {
    public override void SayMyName() {
        Console.WriteLine("Gijung");
    }
}
class Hands : Gijung {
    public override void SayMyName() {
        Console.WriteLine("Gijung's Hands");
    }
}

public class Program {
	public static void Main() {
		Person p = new Person();
        Gijung g = new Gijung();
        Hands h = new Hands();
         
        House<Person> h_p = new House<Person>(p);
        House<Gijung> h_g = new House<Gijung>(g);
        House<Hands> h_h = new House<Hands>(h);
	}
}
```

## Type inference (타입 추론)

+ 컴파일러가 왼쪽의 타입을 보고 오른쪽 타입을 추론하는 기능.

```java
class A<T> {
    T m_a;
    
    A(T a) {
        m_a = a;
    }
    
    void SayMember() {
        System.out.println("member : " + m_a);
    }
}

public class MainClass{
     public static void main(String []args){
        // 기존 선언 방식
        A<String> a = new A<String>("Hello");
        a.SayMember();
        
        // 자동 추론 방식
        A<String> b = new A<>("World");
        b.SayMember();
     }
}
```

## Annotations

+ 주석
+ Meta Annotaion을 이용하여 Custom Annotaion을 만들 수 있다.

| Built-in Annotation |종류| 설명|
|---|:---|---|
| `@Override` || 메소드가 오버라이드 됐는지 검증. |
| `@Deprecated` || 메소드를 사용하지 말도록 유도. |
| `@SuppressWarnings` || 컴파일 경고를 무시. |
||all|모든 경고를 억제.|
||cast|캐스트 오퍼레이션과 관련된 경고를 억제.|
||unused|사용하지 않은 코드 및 불필요한 코드와 관련된 경고를 억제.|
||...|
| `@SafeVarargs` | 제너릭 같은 가변인자 매개변수를 사용할 때 경고를 무시. |
| `@FunctionalInterface` | 람다 함수등을 위한 인터페이스를 지정. |
|||

| Meta Annotations| 종류 | 설명|
|---|:---|---|
| `@Retention` || 특정 시점까지 영향을 미치는지를 결정. |
||RetentionPolicy.SOURCE|컴파일 전까지만 유효.|
||RetentionPolicy.CLASS|컴파일러가 클래스를 참조할 때까지 유효.|
||RetentionPolicy.RUNTIME|컴파일 이후에도 계속 참조가 가능.|
| `@Target` || 어노테이션이 적용할 위치를 선택. |
||ElementType.PACKAGE|패키지 선언.|
||ElementType.TYPE|타입 선언.|
||ElementType.ANNOTATION_TYPE|어노테이션 타입 선언.|
||ElementType.CONSTRUCTOR|생성자 선언.|
||ElementType.FIELD|멤버 변수 선언.|
||ElementType.LOCAL_VARIABLE|지역 변수 선언.|
||ElementType.METHOD|메서드 선언.|
||ElementType.PARAMETER|전달인자 선언.|
||ElementType.TYPE_PARAMETER|전달인자 타입 선언.|
||ElementType.TYPE_USE|타입 선언.|
| `@Inherited` || 자식클래스가 어노테이션을 상속. |
| `@Repeatable` || 반복적으로 어노테이션을 선언. |
| `@Documented` || 해당 어노테이션을 Javadoc에 포함. |
|||

```java
[Java Code]

class A {
    @Deprecated
    void SayName() {
        System.out.println("Hello");
    }
    
    @SuppressWarnings("all")
    String GetName() {
        return "Hi";
    }
}

class B extends A {
    @Override
    void SayName() {
        System.out.println("World");
    }
}

public class MainClass{
     public static void main(String []args){
        A a = new A();
        int name = a.GetName();
     }
}
```


```java
[Java Code]

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Retention(RetentionPolicy.RUNTIME)
@Target(ElementType.METHOD)
@interface TestAnno{
    public boolean enabled() default false;
}

class A {
    String m_a;
    
    @TestAnno
    void SayMember() {
        System.out.println("member : " + m_a);
    }
}

public class MainClass{
     public static void main(String []args){
        A a = new A();
        a.m_a = "Hi";
        a.SayMember();
     }
}
```