# How to create and destroy objects

## Instance Construction
### Implicit(Generated) Constructor
* 생성자가 없는 클래스를 정의할 수 있음
* Java 컴파일러가 암시적으로 하나를 생성함
* `new` 키워드 사용해 인스턴스 생성
```java
public class NoConstructor {

}

NoConstructor noConstructorInstance = new NoConstructor();
```
### Constructors without Arguments
* 인수가 없는 생성자는 Java컴파일러의 작업을 명시적으로 수행
```java
public class NoArgConstructor {
    public NoArgConstructor(){

    }
}

NoArgConstructor noArgConstructor = new NoArgConstructor();
```

### Constructors with Arguments
```java
public class ConstructorWithArguments {
    public ConstructorWithArguments(String arg1, String arg2){

    }
}

ConstructorWithArguments contructorWithArguments = new ConstructorWithArguments("arg1","arg2");
```
* `this`키워드를 사용해 생성자 안에서 다른 생성자를 호출할 수 있음
* 코드 중복을 줄이고, 생성자들을 하나의 초기화 entry point로 이끌 수 있음
```java
public ConstructorWithArguments(String arg1){
    this(arg1, null);
}
```
### Initialization Blocks
* 초기화 블록을 이용한 초기화 로직을 제공함
    * 거의 사용되지 않지만 알아두면 좋다
* 여러개의 초기화 블록을 만들 수 있음
    * 순차적으로 실행됨
* 초기화 블록은 생성자를 대체하지 않음
* 항상 생성자 앞에 호출됨
```java
public class InitializationBlockAndConstructor {
    String arg1, arg2;
    //initialization block
    {
        arg1 = "aaa";
        arg2 = "bbb";
    }
    //initialization block
    {
        arg1 = "abc";
        arg2 = "qqq";
    }

    public InitializationBlockAndConstructor() {

    }

    void PrintArg(){
        System.out.println("arg1 : "+ arg1 + ", arg2 : " + arg2);
    }
}

InitializationBlockAndConstructor myClass = new InitializationBlockAndConstructor();
my.Class();     // arg1 : aaa, arg2 : qqq;
```

### Construction guarantee
* 초기화되지 않은 인스턴스 및 클래스(정적) 변수는 기본값으로 자동 초기화

### Visibility
| Modifier | Package | Subclass | Everyone Else |
|---|:---|:---|:---|
|public|접근 가능|접근 가능|접근 가능|
|protected|접근 가능|접근 가능|접근 불가능|
|default|접근가능|접근 불가능|접근 불가능|
|private|접근 불가능|접근 불가능|접근 불가능|

### Garbage collection

* 자바는 자동으로 가비지 컬렉션을 사용함
* 오브젝트가 생성될 때 메모리는 자동으로 할당되고, 오브젝트가 더이상 참조되지 않을 때 파괴되고 메모리도 회수됨

### Finalizers
* 자바에서 오브젝트의 생명주기를 관리하는 것은 가비지 콜렉터의 영역이지만 `finalize()`라는 소멸자와 비슷하지만 메모리가 수행될 때 가비지콜렉터에 의해 실행되는 함수가 있음
* 많은 side-effect와 퍼포먼스 문제가 있으므로 되도록 사용을 피해야함
    * 신속하게 실행된다는 보장이 없음
    * 반드시 실행된다는 보장이 없음
    * 예외가 발생하면 무시됨
* 사용해도 적합한 경우
    * 객체의 종료 메서드 호출을 깜빡한 경우를 대비한 안전망 역할
    * Native peer 객체(Native(C, C++ 언어) API와 연관된 자바 객체)를 생성할 경우
* finalizer 대안
* try-catch-resourses : try에서 자원할당 후 try가 끝나면 자원해제됨
    * try 인수 안에는 AutoCloseable 구현체만 들어갈 수 있음
```java
public static void main(String[] args) {
		try(A a = new A()){
			System.out.println("a create");
		}catch(Exception e) {
			
		}
	}

class A implements AutoCloseable {
	@Override
	public void close() throws Exception {
		System.out.println("a call");
	}
}

// 결과
a create
a call
```
### Static Initialization
* 클래스 로딩시 호출
* 인스턴스 변수나 인스턴스 메소드에 접근 못함

```java
class A {
	int instanceValue;
	static int classValue;
	static {
		//instanceValue = 1;	//error
		classValue = 2;
		System.out.println("a call");
	}
	static {
		System.out.println("b call");
	}
	
	static void PrintValue() {
		System.out.println("class value : "+classValue);
	}
}

public static void main(String[] args) {
		A.classValue = 3;
		A.PrintValue();
}

// 결과
a call
b call
class value : 3
```
### Construction Patterns
#### Singleton
* 하나의 인스턴스만 생성
* 싱글톤 방식은 테스트하기 어려운 코드를 만들기 때문에 대부분 좋은 선택이 아님
```java
public class NaiveSingleton {
	private static NaiveSingleton instance;
	
	public static NaiveSingleton getInstance() {
		if(instance == null) {
			instance = new NaiveSingleton();
		}
		
		return instance;
	}

    void Print() {
		System.out.println("Singleton");
	}
}

public static void main(String[] args) {
		NaiveSingleton.getInstance().Print();
}
```

#### Utility/Helper Class
* non-instantiable class 이며 final로 선언, static method만 가짐
* 공유될 수 있는 연관성 없는 메소드들의 컨테이너가 될 수 있음

```java
public final class HelperClass {
	private HelperClass() {
		
	}
	
	public static void helperMethod() {
		
	}
}
```
#### Factory
* 객체 생성을 처리하는 클래스 : 팩토리
* 클래스의 인스턴스를 만드는 방법을 서브클래스에서 결정하도록 하는 방식
    * 팩토리 메소드 패턴 : 조건에 따른 객체 생성을 팩토리 클래스로 위임하여, 팩토리 클래스에서 객체를 생성하는 패턴
        * 하나의 제품 항목에 대해 객체를 만드는 팩토리 생성  
    * 추상 팩토리 패턴 :  서로 관련이 있는 객체들을 통째로 묶어서 팩토리 클래스로 만들고, 이들 팩토리를 조건에 따라 생성하도록 다시 팩토리를 만들어서 객체를 생성하는 패턴
    *   * 여러개의 제품 항목을 묶어 객체를 만드는 팩토리 생성
#### 팩토리 메소드 패턴
```java
// Product
public abstract class Keyboard {
}

// ConcreteProduct
public class LGKeyboard extends Keyboard {
}

// ConcreteProduct
public class SamsungKeyboard extends Keyboard {
}

// Product
public abstract class Mouse {
}

// ConcreteProduct
public class LGMouse extends Mouse {
}

// ConcreteProduct
public class SamsungMouse extends Mouse {
}

// Creator
public abstract class KeyboardFactory {
	// Factory Method
	public abstract Keyboard CreateKeyboard();
}

// ConcreteCreator
public class LGKeyboardFactory extends KeyboardFactory {
	public Keyboard CreateKeyboard(){
		return new LGKeyboard();
	}
}

// ConcreteCreator
public class SamsungKeyboardFactory extends KeyboardFactory {
	public Keyboard CreateKeyboard(){
		return new SamsungKeyboard();
	}
}

// Creator
public abstract class MouseFactory {
	// Factory Method
	public abstract Mouse CreateMouse();
}

// ConcreteCreator
public class LGMouseFactory extends MouseFactory {
	public Keyboard CreateMouse(){
		return new LGMouse();
	}
}

// ConcreteCreator
public class SamsungMouseFactory extends MouseFactory {
	public Keyboard CreateMouse(){
		return new SamsungMouse();
	}
}

public static void Main() {
	KeyboardFactory k_factories = new KeyboardFactory[2];
	k_factories[0] = new LGKeyboardFactory();
	k_factories[1] = new SamsungKeyboardFactory();
	MouseFactory m_factories = new MouseFactory[2];
	m_factories[0] = new LGMouseFactory();
	m_factories[1] = new SamsungMouseFactory(); 
}
```
#### 추상 팩토리 패턴
```java
// AbstractFactory
public abstract class ComputerFactory {
	public abstract Keyboard CreateKeyboard();
	public abstract Mouse CreateMouse();
}

// ConcreateFactory
public class LGComputerFactory extends ComputerFactory {
	public Keyboard CreateKeyboard(){
		return LGKeyboard();
	}

	public Mouse CreateMouse() {
		return LGMouse();
	}
}

// ConcreateFactory
public class SamsungComputerFactory extends ComputerFactory {
	public Keyboard CreateKeyboard(){
		return SamsungKeyboard();
	}

	public Mouse CreateMouse() {
		return SamsungMouse();
	}
}

// Client
public class Client {
	Keyboard _keyboard;
	Mouse _mouse;

	public Client(ComputerFactory computerFactory) {
		_keyboard = computerFactory.CreateKeyboard();
		_mouse = computerFactory.CreateMouse();
	}
}

public static void Main() {
	Client client = new Client(new LGComputerFactory());
}
```
#### Dependency Injection
* 일부 클래스 인스턴스가 다른 클래스 인스턴스에 의존하는 경우 인스턴스 생성의 종속성은 생성자, setter 등을 통해 제공되어야함

```java
public abstract class AnimalType {
	public abstract void sound();
}

public class Cat extends AnimalType {
	public void sound() {
		System.out.println("meow");
	}
}

public class Dog extends AnimalType {
	public void sound() {
		System.out.println("BowBow");
	}
}

public class PetOwner {
	public AnimalType animal;
	
	public PetOwner(AnimalType animal) {
		this.animal = animal;
	}
	
	public void play() {
		animal.sound();
	}
}

public static void main(String[] args) {
		PetOwner person = new PetOwner(new Cat());
		person.play();
}

//결과
meow
```

# Q

* factory method 와 abstract factory class 의 차이점은???
	* https://www.dofactory.com/net/factory-method-design-pattern
	* https://www.dofactory.com/net/abstract-factory-design-pattern