# How and when to use Enums and Annotations

* Enums : special type of classes
* Annotations : special type of interfaces

## Enums as special classes
* enum 값들은 대문자를 사용
```java
// enum이 도입되기 전 사용하던 방식
public class DaysOfTheWeekConstants {
    public static final int MONDAY = 0;
    public static final int TUESDAY = 1;
    public static final int WEDNESDAY = 2;
    public static final int THURSDAY = 3;
    public static final int FRIDAY = 4;
    public static final int SATURDAY = 5;
    public static final int SUNDAY = 6;

    public boolean isWeekend(int day){
    return (day == SATURDAY || day == SUNDAY);
    }
}

// enum 값들은 인스턴스
public enum DaysOfTheWeek {
    MONDAY,
    TUESDAY,
    WEDNESDAY,
    THURSDAY,
    FRIDAY,
    SATURDAY,
    SUNDAY;

    public boolean isWeekend(DaysOfTheWeek day){
   return (day == SATURDAY || day == SUNDAY);
    }
}
```
## Enums and Instance Fields

* instance field, constructors, methods를 가질 수 있음
* 인수 없는 생성자는 만들 수 없고, 생성자는 private로 선언해야함
    * enum은 외부에서 생성이 불가능함
```java
public enum DaysOfWeek {
    MONDAY(false),
    TUESDAY(false),
    WEDNESDAY(false),
    THURSDAY(false),
    FRIDAY(false),
    SATURDAY(true),
    SUNDAY(true);
	
	private final boolean isWeekend;
    
	private DaysOfWeek(final boolean isWeekend) {
		this.isWeekend = isWeekend;
	}
	
	public boolean isWeekend() {
		return isWeekend;
	}
}
```
* C# enum : 상수집합
	* 정수형만 할당 가능
```cs
public enum DaysOfTheWeek {
	MONDAY = 1,
	TUESDAY = 2,
	WEDNESDAY = 3,
	THURSDAY = 4,
	FRIDAY = 5,
	SATURDAY = 6,
	SUNDAY = 7,
}
```
## Enums and interfaces
* 인터페이스를 구현할 수 있음
```java
public interface DayOfWeek {
	boolean isWeekend();
}

public enum DaysOfTheWeekInterfaces implements DayOfWeek {
	MONDAY() {
		@Override
		public boolean isWeekend() {
		return false;
		}
	},
	TUESDAY() {
		@Override
		public boolean isWeekend() {
		return false;
		}
	},
	WEDNESDAY() {
		@Override
		public boolean isWeekend() {
		return false;
		}
	},
	THURSDAY() {
		@Override
		public boolean isWeekend() {
		return false;
		}
	},
	FRIDAY() {
		@Override
		public boolean isWeekend() {
		return false;
		}
	},
	SATURDAY() {
		@Override
		public boolean isWeekend() {
		return true;
		}
	},
	SUNDAY() {
		@Override
		public boolean isWeekend() {
		return true;
		}
	};
}

public static void main(String[] args) {
		DaysOfTheWeekInterfaces day = DaysOfTheWeekInterfaces.FRIDAY;
		System.out.println(day.isWeekend());
}

// 결과
false
```

## Enums and generics
* enum은 클래스에 enum이 자동으로 상속됨
* interface는 구현할 수 있지만 다른 클래스 상속을 받을 수는 없다
* Java 컴파일러는 컴파일 시 개발자를 대신하여 이러한 변환을 수행
```java
public class DaysOfTheWeek extends Enum <DaysOfTheWeek > {
}
```
## Convenient Enums mothods
|Method|Description|
|---|:---|
|String name()|enum 값의 이름 반환|
|int ordinal()|enum 값의 위치 반환(첫번째 enum : 0)|

|Static Method|Description|
|---|:---|
|T[] values()| 모든 선언된 enum 값 반환|
|T valueOf(String name)|name과 일치하는 enum 값 반환|
```java
public static void main(String[] args) {
		System.out.println(DaysOfWeek.FRIDAY.name());
		System.out.println(DaysOfWeek.FRIDAY.ordinal());
		for(DaysOfWeek day : DaysOfWeek.values()) {
			System.out.println(day);
		}
		DaysOfWeek day2 = DaysOfWeek.valueOf("MONDAY");
		System.out.println(day2.isWeekend());
}

// 출력
FRIDAY
4
MONDAY
TUESDAY
WEDNESDAY
THURSDAY
FRIDAY
SATURDAY
SUNDAY
false
```
## Specialized Collections: EnumSet and EnumMap
* EnumSet 예제
```java
public static void main(String[] args) {
		EnumSet<DaysOfWeek> es = EnumSet.allOf(DaysOfWeek.class);
		System.out.println("all : "+ es);
		EnumSet<DaysOfWeek> es2 = EnumSet.copyOf(es);
		System.out.println("copy : "+ es2);
		es = EnumSet.noneOf(DaysOfWeek.class);
		System.out.println("none : "+ es);
		es = EnumSet.of(DaysOfWeek.MONDAY, DaysOfWeek.THURSDAY);
		System.out.println("of : " + es);
		es = EnumSet.complementOf(es);
		System.out.println("complement of : " + es);
		es = EnumSet.range(DaysOfWeek.MONDAY, DaysOfWeek.THURSDAY);
		System.out.println("range : " + es);
		es = EnumSet.range(DaysOfWeek.THURSDAY, DaysOfWeek.MONDAY);
		System.out.println("range : " + es);
}

//출력
all : [MONDAY, TUESDAY, WEDNESDAY, THURSDAY, FRIDAY, SATURDAY, SUNDAY]
copy : [MONDAY, TUESDAY, WEDNESDAY, THURSDAY, FRIDAY, SATURDAY, SUNDAY]
none : []
of : [MONDAY, THURSDAY]
complement of : [TUESDAY, WEDNESDAY, FRIDAY, SATURDAY, SUNDAY]
range : [MONDAY, TUESDAY, WEDNESDAY, THURSDAY]
Exception in thread "main" java.lang.IllegalArgumentException: THURSDAY > MONDAY
	at java.util.EnumSet.range(Unknown Source)
```
* EnumMap : map과 비슷하지만 key값이 enum값
* EnumMap 예제
```java
public static void main(String[] args) {
    EnumMap em = new EnumMap(DaysOfWeek.class);
    em.put(DaysOfWeek.MONDAY, "월요일");
    em.put(DaysOfWeek.TUESDAY, "화요일");
    em.put(DaysOfWeek.WEDNESDAY, "수요일");
    em.put(DaysOfWeek.THURSDAY, "목요일");
    em.put(DaysOfWeek.FRIDAY, "금요일");
    em.put(DaysOfWeek.SATURDAY, "토요일");
    em.put(DaysOfWeek.SUNDAY, "일요일");
    
    for(DaysOfWeek day : DaysOfWeek.values()) {
        System.out.println(em.get(day));
    }
}

//출력
월요일
화요일
수요일
목요일
금요일
토요일
일요일
```
## Annotations
* 소스코드에 메타데이터를 표현
```java
@Target(ElementType.FIELD)
@Retention(RetentionPolicy.RUNTIME)
public @interface StringInjector {
	String value() default "Default";
}

class Test {
	@StringInjector
	private String defaultValue;
	@StringInjector("name")
	private String name;
	
	public String getName() {
		return name;
	}
	public String getDefault() {
		return defaultValue;
	}
}

public static void main(String[] args) throws IllegalArgumentException, IllegalAccessException {
  	Test test = new Test();
	System.out.println(test.getDefault());
	System.out.println(test.getName());
	Field[] fields = test.getClass().getDeclaredFields();
	for(Field field : fields) {
		System.out.println(field.getName());
	}
}

//출력
null
null
Default
name
```
```cs
[AttributeUsage(AttributeTargets.Field)]
class StringInjectorAttribute : Attribute {
	String value;

	public StringInjectorAttribute() {
		value = "Default";
	}
	public StringInjectorAttribute(String value) {
		this.value = value;
	}
}

class Test {
	[StringInjector]
	public String defaultValue;

	[StringInjector("name")]
	public String name;

	public String getName() {
		return name;
	}

	public String getDefault() {
		return defaultValue;
	}
}

class Program {
	static void Main() {
		Test test = new Test();
		Console.WriteLine(test.getDefault());
		Console.WriteLine(test.getName());

		System.Reflection.FieldInfo[] fields = test.GetType().GetFields();
		foreach(System.Reflection.FieldInfo field in fields) { 
			Console.WriteLine(field.Name);
		}
	}
}
```
## Annotations and retention policy
|Policy|Description|
|---|:---|
|SOURCE|소스상에서만 어노테이션 정보 유지|
|CLASS|바이트 코드 파일까지 어노테이션 정보 유지, 리플렉션을 이용해서 어노테이션 정보를 얻을 수는 없음|
|RUNTIME|바이트 코드 파일까지 어노테이션 정보 유지, 리플렉션을 이용해 런타임시에 어노테이션 정보를 얻을 수 있음|
* 리플렉션 : 구체적인 클래스 타입을 알지 못해도 컴파일된 바이트 코드를 통해 역으로 클래스의 정보를 알아내어 클래스를 사용할 수 있는 기법
```java
@Target(ElementType.TYPE)
@Retention(RetentionPolicy.SOURCE)
public @interface SourceAnnotation {
}

@Target(ElementType.TYPE)
@Retention(RetentionPolicy.CLASS)
public @interface CompileAnnotation {
}

@Target(ElementType.TYPE)
@Retention(RetentionPolicy.RUNTIME)
public @interface RuntimeAnnotation {
}

@SourceAnnotation
class SourceTest { }

@CompileAnnotation
class CompileTest { }

@RuntimeAnnotation
class RuntimeTest {	}

public static void main(String[] args) throws IllegalArgumentException, IllegalAccessException {
	SourceTest st = new SourceTest();
	CompileTest ct = new CompileTest();
	RuntimeTest rt = new RuntimeTest();
		
	Annotation annotation = st.getClass().getAnnotation(SourceAnnotation.class);
	System.out.println(annotation.toString()); //  java.lang.NullPointerException
		
	annotation = ct.getClass().getAnnotation(CompileAnnotation.class);
	System.out.println(annotation.toString()); //  java.lang.NullPointerException
		
	annotation = rt.getClass().getAnnotation(RuntimeAnnotation.class);
	System.out.println(annotation.toString()); // @RuntimeAnnotation()
}
```
## Annotations and elements types
|ElementType|적용대상|
|---|:---|
|TYPE|클래스, 인터페이스, enum|
|ANNOTATION_TYPE|어노테이션|
|FIELD|필드|
|CONSTRUCTOR|생성자|
|METHOD|메소드|
|LOCAL_VARIABLE|로컬 변수|
|PACKAGE|패키지|
* retention policy와 다르게 여러개 선언가능
```java
@Target({ElementType.FIELD, ElementType.METHOD})
public @interface AnnotationWithTarget(){
}
```
## Annotations and inheritance
* 기본적으로, 자식클래스는 부모클래스에 선언된 어노테이션을 상속받지 않음
* @Inherited 어노테이션을 사용해 클래스 계층 전체에 어노테이션을 전할 수 있음
```java
@Target( { ElementType.TYPE } )
@Retention( RetentionPolicy.RUNTIME )
@Inherited
@interface InheritableAnnotation {
}

@InheritableAnnotation
public class Parent { }

public class Child extends Parent { }

public static void main(String[] args) throws IllegalArgumentException, IllegalAccessException {
		Parent parent = new Parent();
		Child child = new Child();
		
		Annotation annotation = parent.getClass().getAnnotation(InheritableAnnotation.class);
		System.out.println(annotation.toString());
		
		annotation = child.getClass().getAnnotation(InheritableAnnotation.class);
		System.out.println(annotation.toString());
}

// 출력
@InheritableAnnotation(value=Annoation)
@InheritableAnnotation(value=Annoation)
```
## Repeatable annotations
```java
@Target( ElementType.METHOD )
@Retention( RetentionPolicy.RUNTIME )
public @interface RepeatableAnnotations {
RepeatableAnnotation[] value();
}

@Target( ElementType.METHOD )
@Retention( RetentionPolicy.RUNTIME )
@Repeatable( RepeatableAnnotations.class )
public @interface RepeatableAnnotation {
String value();
};

class Test {
	@RepeatableAnnotation( "repeatition 1" )
	@RepeatableAnnotation( "repeatition 2" )
	public void performAction() {
	}	
}

public static void main(String[] args) throws IllegalArgumentException, IllegalAccessException {
	Test test = new Test();
	Method[] methods = test.getClass().getMethods();
	for(Method m : methods) {
		Annotation[] annotations = m.getAnnotations();
		for(Annotation an : annotations) {
			System.out.println(an.toString());
		}
	}
}

// 출력
@RepeatableAnnotations(value=[@RepeatableAnnotation(value=repeatition 1), @RepeatableAnnotation(value=repeatition 2)])
```

## When to use annotations
|Annotation|Description|
|---|:---|
|@Deprecated|메소드를 사용하지 않도록 유도, 사용시 컴파일 경고|
|@Override|메소드가 오버라이드 됐는지 검증, 해당 메소드를 찾을 수 없으면 컴파일 오류|
|@SuppressWarnings|컴파일 경고 무시|
|@SafeVarargs|제너릭 같은 가변인자 매개변수 사용할 때 경고 무시|
|@Retention|어떤 시점까지 어노테이션이 영향을 미치는지 결정|
|@Documented|문서에도 어노테이션 정보 표현|
|@Inherited|자식 클래스가 어노테이션 상속받을 수 있음|
|@Repeatable|반복적으로 어노테이션 선언 가능|
|@Target|어노테이션이 적용할 위치 결정|