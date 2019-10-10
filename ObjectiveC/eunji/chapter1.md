# Chapter 1. Objects, Classes, and Messaging

* Object, Class, Messaging 의 기초와 Objective-C 런타임을 알아보자

## Runtime

* Objective - c 는 많은 결정을 런타임 단계에서 한다.
* 객체 생성 및 호출할 메소드 결정 같은 작업을 동적으로 실행
* 컴파일러 + 컴파일된 코드를 실행시키기위한 런타임 시스템을 필요로함

## Object

### Objective-C
```c
// 클래스 인터페이스
@interface MyClass : NSObject {
    int number; // 인스턴스 변수
}

// 인스턴스 메소드
 - (void) myMethod;
 // 클래스 메소드 (= C# 의 static 메소드)
 + (void) classMethod;
@end

// 클래스 구현
@implementation MyClass
- (void) myMethod{

}

...

@end

MyClass * myClass;  // MyClass : Class , myClass : Object
myClass = [[MyClass alloc] init];   // 메모리 할당 및 초기화
[myClass myMethod]; // myMethod : instance method
[MyClass classMethod];  // classMethod : class method
```

### C#
```c
// 클래스 선언
class MyClass{
    int number; // 인스턴스 변수

    // 인스턴스 메소드
    public void myMethod(){

    }

    // 클래스 메소드
    public static void classMethod(){

    }
}

MyClass myClass;    // MyClass : Class , myClass : Object
myClass = new MyClass();  // 메모리 할당
myClass.myMethod(); // myMethod : instance method
MyClass.classMethod(); // classMethod : class method
```
* 객체 = 데이터구조(인스턴스 변수) + 메소드
* 겍체는 데이터를 사용하거나 데이터를 사용할 수 있는 특정 작업(메소드)과 데이터를 연결
* 일반적으로 객체의 메소드를 통해서만 객체 인스턴스에 액세스 할 수 있음
    * 객체는 인스턴스 변수와 메서드 구현을 모두 숨김
```c
id myClass = [[MyClass alloc] init];
[myClass myMethod]; // 이 구문 실행할 때 myMethod가 MyClass의 메소드인것을 식별 
```
* id : 클래스에 관계 없는 모든 유형의 객체에 대한 일반적인 유형
    *  객체 데이터 구조에 대한 포인터로 사용 가능
    * id는 객체라는 것을 제외하고 객체에 대한 정보를 산출하지 못함
* isa variable (isa pointer) : 클래스를 알려주는 변수
    * isa 변수는 객체가 자신(또는 다른 객체)에 대해 알아보기 위해 내부 검사 수행
    * isa 변수를 이용해 객체는 런타임에 동적으로 입력됨, 런타임 시스템은 객체를 묻는 것만으로 객체가 속한 정확한 클래스를 찾을 수 있음
* 클래스 유형은 포인터로 정의, 객체는 주소로 식별됨
* nil : null 개체, 값이 0인 id

* objective-c의 기본적인 타입(id, nil 등)은 objc/objc.h에 정의

* 컴파일러는 런타임 시스템이 사용할 데이터 구조의 클래스 정의에 대한 정보를 기록

### Memory Management
* Reference Counting : 개체의 라이프타임에 대한 궁극적인 책임이 있는 참조 계산
* Garbage Collection : 개체의 라이프타임에 대한 책임을  collector에게 전달

## Message

* 메세지를 보내면 런타임 시스템에서 적절한 메소드를 선택하고 호출함
* 메시지의 메소드 이름을 종종 selector라고 함
* selector의 이름은 콜론(:)을 포함하여 모든 키워드를 포함하지만 리턴 유형이나 매개 변수 유형같은 다른것은 포함하지 않음
* objective-c에서는 nil에 메시지를 보내면 런타임시 아무런 효과가 없음
* 메소드 리턴시 Mac OS X ABI function 에 정의된 struct는 nil로 리턴될시 0으로 채워지지만 다른 struct는 그렇지 않음

### Objective-C
```c
MyClass* myclass = nil;

if([myclass myMethod] == 0){
    // 실행됨
}
```
### C#
```c
MyClass myclass = null;

if(myclass.myMethod == 0){ // 에러남

}
```
* 함수 호출과 메시지 사이의 차이점
    * 함수 호출 : 함수와 그 인수가 컴파일된 코드에서 함께 결합
    * 메시지 : 프로그램이 실행중이고 메시지가 전송될때까지 메시지 수신 객체가 통합되지 않는다
* 따라서 메시지에 응답하기 위해 호출되는 정확한 메소드는 코드가 컴파일될때가 아니라 런타임시 결정될 수 있음

### Polymorphism
* 다른 클래스들 간에 동일한 메서드 이름을 사용할 수 있는 기능
```c
@ interface Foo : NSObject
- (void) print;
@end

@interface Bar : NSObject
- (void) print;
@end

Foo f = [[Foo alloc] init];
Bar b = [[Bar alloc] init];

f.print;
b.print;
```
### dot systax
```c
myInstance.value = 10;
[myInstance setValue:10];
```
* 컴파일러에서 접근자 메소드 호출로 변환되므로 실제로 인스턴스 변수에 직접 액세스 하지는 않음(캡슐화 유지)
* 장점 : 컴파일러가 읽기 전용 속성에 대한 쓰기를 감지할 때 오류를 알릴 수 있음(선언되지 않은 정보는 오류로 처리)

### self
#### Objective-C
```c
self.age = 10;
age = 10;
```
#### C#
```c
this.age = 10;
```

## class

* 객체의 프로토 타입, 인스턴스 변수를 선언, 메소드 정의
* 각 객체는 자체 인스턴스 변수를 가지지만, 메소드는 공유
* static typing : 객체를 선언할 때 클래스를 선언(객체의 정보를 컴파일러에 제공하는 것)
```c
MyClass* myClass = [[MyClass alloc] init];
```

### Inheritance
* 루트 클래스를 작성하지 않는 한, 만드는 모든 클래스는 다른 클래스의 서브클래스여야함
* NSObject : 루트클래스 - 오브젝트와 오브젝트 상호 작용을 위한 기본 프레임워크를 정의

### class object
```c
[MyClass classMethod];  //객체를 정의하지 않아도 클래스 메소드는 사용 가능
```
* 클래스정의를 컴파일할때 메타클래스를 생성해서 처리 -> 클래스 정의 자체도 이미 객체화
* 클래스 인스턴스는 런타임에 objective-c 오브젝트처럼 작동할 수 있어야함
* 클래스 인스턴스는 프로토 타입은 유지하지만 인스턴스 자체는 아님
* 정의한 새 클래스는 계층 구조에서 상위에 있는 모든 클래스에 대해 작성된 코드를 사용할 수 있음
### abstact class
* 여러가지 하위 클래스에서 공통 정의로 사용할 수 있는 메소드 및 인스턴스 변수를 그룹화
* objective-c 에는 클래스를 추상으로 표시하는 구문이 없음
```c
// 클래스 생성 시 상속받은 클래스로만 생성 할 수있도록 BaseClass로 생성하면 Exception 발생
- (id) init {
  if ([self class] == [BaseClass class]) {
    @throw [NSException exceptionWithName:NSInternalInconsistencyException
      reason:@"Error, attempting to instantiate AbstractClass directly." userInfo:nil];
  }
 
  self = [super init];
  if (self) {
    // Initialization code here.
  }
  return self;
}

// 추상함수로 사용할 클래스의 프로토타입을 만들고 구현부에서는 Exception이 발생되도록 처리, 하위 클래스가 상속받아 만들도록 강제함
- (id)someMethod {
  @throw [NSException exceptionWithName:NSInternalInconsistencyException
        reason:[NSString stringWithFormat:@"You must override in a subclass"]
        userInfo:nil];
}
```


### create instance
```c
myRectangle = [[Rectangle alloc] init];
```

* alloc : 새로운 객체의 인스턴스 변수에 동적으로 메모리를 할당하고 isa 변수를 제외한 나머지를 0으로 초기화
* init : 인스턴스의 초기 상태 설정
```c
MyClass.m

static int sNumber; // static 변수
@implementation MyClass

@end 
```
* 외부 변수나 static 변수는 implemantation(클래스 구현 파일)에서 선언 
* static 변수
    * 클래스의 공유 인스턴스를 정의하는 데 사용
    * 하위 클래스에 상속되거나 직접 조작 X
* 클래스 이름은 전역 변수 및 함수 이름과 동일한 네임 스페이스에 있음