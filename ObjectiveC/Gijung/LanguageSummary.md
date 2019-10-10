# Language Summary

## Messages

`[receiver message]`

### receiver에 들어갈 수 있는 것들

+ class
+ super
+ self

### message에 들어가는 것들

+ 메소드 이름 + 매개변수

```objc
ex)

@interface TestReceiver : NSObject
+ (void)TestMethod;
+ (void)TestMethod2:(float)f_a value:(float)f_b;
@end

@implementation TestReceiver
+ (void)TestMethod {
    [super alloc];                          // super 사용
    [self TestMethod2:1.0f value:2.0f];     // self, 매개변수가 있는 메소드 사용
}

+ (void)TestMethod2:(float)f_a value:(float)f_b {
    
}
@end

int main(int argc, char * argv[]) {
    // 클래스 이름 사용
    [TestReceiver TestMethod];              
    [NSString string];
}
```
## Preprocessor Directives

| Type | Definition |
|---|:---|
| `#import` | 헤더 파일을 가져올때 사용.|
| `//` | 주석으로 사용. |

```
ex)

#import <UIKit/UIKit.h>
#import "AppDelegate.h"

//NSLog(@"abc");
```

## Compiler Directives

### 선언과 구현

| Directive | Definition |
|---|:---|
| `@interface` | 클래스나 카테고리 선언시 사용.|
| `@implementation` | 클래스나 카테고리 구현시 사용.|
| `@protocol` | 프로토콜 선언시 사용.|
| `@end` | 클래스, 카테고리, 프로토콜의 선언, 구현 마지막에 사용.|

```objc
ex)

// 카테고리 선언
@interface NSString (TestCategory)
@end

// 클래스 선언
@interface TestReceiver : NSObject
@end

// 클래스 구현
@implementation TestReceiver
@end

// 프로토콜 선언
@protocol TestProtocol
@end
```

### 접근 권한

| Directive | Definition |
|---|:---|
| `@private` | 접근 권한을 클래스 내부에서만 사용하도록 제한.|
| `@protected` | 접근 권한을 클래스 내부와 상속받은 클래스로 제한.|
| `@public` | 접근 제한이 없다.|

```
ex)

@interface Test : NSObject {
@private
    float m_f_a;
@protected
    float m_f_b;
@public
    float m_f_c;
}
@end
```

+ 기본 값으로 @protected가 사용된다.

### 예외상황

| Directive | Definition |
|---|:---|
| `@try` | 예외상황이 생길 수 있는 코드 작성에 사용. |
| `@throw` | exception Object를 보낼때 사용.|
| `@catch` | try에서 발생한 예외상황을 처리하는데 사용.|
| `@finally` | 예외상황 유무에 상관없이 무조건 실행.|

```objc
ex) a 클래스에 존재하지 않는 abc 함수를 호출했을때의 예외상황.

int main(int argc, char * argv[]) {
    PropertyTest *a = [[PropertyTest alloc] init];
    
    @try {
        [a abc];
    } @catch (NSException *exception) {
        NSLog(@"main: Caught %@: %@", [exception name], [exception  reason]);
    } @finally {
        NSLog(@"무조건 실행");
    }
}
```

### 프로퍼티

| Directive | Definition |
|---|:---|
| `@property` | 프로퍼티 선언에 사용.|
| `@synthesize` | 컴파일러에게 접근 메소드 생성 요청에 사용.|
| `@dynamic` | 구현부가 외부 클래스에 있다고 알려주어 컴파일러 경고를 받지않도록 할때 사용.|

```objc
ex)

@interface PropertyTest : NSObject {
    float value;
}

@property (getter=Abc, setter=Abc:) float value;
- (void) setAbc : (float)input;
@end

@implementation PropertyTest
@synthesize rTest;
@end

@implementation DynamicTest
@dynamic dTest;
@end
```

### etc

| Directive | Definition |
|---|:---|
| `@class` | 다른곳에서 정의된 클래스 선언.|
| `@selector(method_name)` | 정의된 method_name의 selector를 반환.|
| `@protocol(protocol_name)` | 프로토콜 인스턴스를 반환.|
| `@encode(type_spec)` | type_spec로 인코딩한것을 반환.|
| `@"string"` | NSString 정의 및 지정된 "string"으로 초기화.|
| `@synchronized()` | 한번에 하나의 스레드만 실행되어야하는 코드를 정의.|

```objc
int intArray[5] = {1, 2, 3, 4, 5};
float floatArray[3] = {0.1f, 0.2f, 0.3f};
    
NSLog(@"int        : %s", @encode(int));
NSLog(@"int[]      : %s", @encode(typeof(intArray)));
NSLog(@"float[]    : %s", @encode(typeof(floatArray)));
```

![](Images/Error_5.png)

## Classes

+ `새로운 클래스를 선언`하기 위해서는 `@interface를 사용`하여 선언한다.
+ 상속 클래스를 사용하기위해서는 반드시 부모 클래스 import 해야한다.

```objc
#import "ItsSuperclass.h"
@interface ClassName : ItsSuperclass
@end
```

+ 상속 클래스를 콜론(:) 뒤에 사용하지 않는다면 root 클래스가 된다.

```objc
@implementation ClassName
@end
```

## Categories

+ 클래스와 선언 방식이 비슷하다.
+ 직접 수정 할 수 없는 클래스에 추가적인 기능을 구현하고자 할때 사용한다.

```objc
@implementation ClassName ( CategoryName )
@end
```

## Formal Protocols

+ c#의 인터페이스와 유사하다.
+ 함수를 정의만하고 사용하고자 하는 곳에서 구현하는 방식을 사용한다. 

```objc
@protocol ProtocolName
          declarations of required methods
@optional   // 상속받는 부분에서 필요한 경우에만 구현
          declarations of optional methods
@required   // 상속받는 부분에서 꼭 구현
          declarations of required methods
@end
```

## Method Declarations

+ 클래스 메소드 일때 함수 앞에 `"+"`를 붙인다.
+ 인스턴스 메소드일때 함수 앞에 `"-"`를 붙인다.
+ 매개변수는 `":"`뒤에 선언하여 사용한다.
+ `":"` 앞에 라벨로 매개변수의 이름을 선언하여 사용한다.

```objc

@interface A : NSObject {
    float value;
}
+ (float) GetValue;
- (float) GetValue2;

- (void) SetValue:(float)val_1 value:(float) val_2;
@end

@implementation A

+ (float) GetValue {
    return 1.0f;
}

- (float) GetValue2 {
    return 2.0f;
}

- (void) SetValue:(float)val_1 value:(float) val_2 {
    value = val_1 + val_2;
}

@end

int main(int argc, char * argv[]) {
    A *a = [[A alloc] init];
    
    NSLog(@"%f", [A GetValue]);
    NSLog(@"%f", [a GetValue2]);
    
    [a SetValue:1.0f value:2.0f];
    
    @autoreleasepool {
        return UIApplicationMain(argc, argv, nil, NSStringFromClass([AppDelegate class]));
    }
}
```

## Naming Conventions

+ 선언부는 [.h], 구현부는 [.m] 파일에 작성한다.
+ class, category, protocol의 이름은 대문자로 시작한다.
+ 메소드 이름, 인스턴스 변수 이름은 소문자로 시작한다.