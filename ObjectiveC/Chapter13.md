# Using C++ With Objective-C

Objective-C++ 에서 C++ 코드의 언어와 혼용하여 사용이 가능합니다.
예를 들면 C++ 클래스의 데이터 멤버로 Objc 객체에 대한 포인터를 포함할 수 있으며 Objc 객체의 인스턴스 변수로 C++ 객체에 대한 포인터를 포함할 수 있습니다.

```objc
/*
g++ -I"c:/GNUstep/GNUstep/System/Library/Headers" -L "c:/GNUstep/GNUstep/System/Library/Libraries" -x -objective-c++ -o hellocpp hellocpp.mm -lobjc -lgnustep-base -fconstant-string-class=NSConstantString
*/

// 이하단 코드는 예제를 위한 극한의 혼종 코드

#import <Foundation/Foundation.h>
// C++ 클래스 문법 및 선언 방식
class HelloCpp {
private:
    id greeting_text; // Objc id 타입
public:
    HelloCpp() {
       greeting_text = @"Hello, world!";
    }
    HelloCpp(const char* initial_greeting_text) {
        greeting_text = [[NSString alloc]
        initWithUTF8String:initial_greeting_text];
    }

    void say_hello() {
        printf("%s\n", [greeting_text UTF8String]);
    }
};

// Objc 클래스 문법 및 선언방식
@interface Greeting : NSObject {
    @private
        HelloCpp *hello; // C++ 객체에 대한 포인터
}
- (id)init;
- (void)dealloc;
- (void)sayGreeting;
- (void)sayGreeting:(HelloCpp*)greeting;
@end

@implementation Greeting
- (id)init {
    if (self = [super init]) {
        hello = new HelloCpp();
    }
    return self;
}
- (void)dealloc {
    delete hello;
    [super dealloc];
}
- (void)sayGreeting {
    hello->say_hello();
}
- (void)sayGreeting:(HelloCpp*)greeting {
    greeting->say_hello();
}
@end

int main() {
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];

    Greeting *greeting = [[Greeting alloc] init];
    [greeting sayGreeting]; // > Hello, world!

    HelloCpp *hello = new HelloCpp("Bonjour, monde!");
    [greeting sayGreeting:hello]; // > Bonjour, monde!

    delete hello;
    [greeting release];
    [pool release];

    return 0;
}
```

이외에도 Objc 인터페이스 내부에 C 구조체를 선언할 수 있으며, C++의 클래스 선언도 가능합니다.

```objc
@interface Foo {
 class Bar { ... } // OK
}
@end
Bar *barPtr; // OK

// --------------------- //

@interface Foo {
 struct CStruct { ... };
 struct CStruct bigIvar; // OK
} ... @end
```

위 처럼 혼용 사용은 가능하지만 몇가지 예외상황이 존재합니다.

1. Objc 객체와 C++ 클래스 간의 상속은 불가합니다. 두 언어 간의 내부적인 객체 모델이 서로 다르기 때문에 두 유형의 계층을 혼합할 수 없습니다.

2. C++ 클래스에 가상함수가 존재할경우 Objc 인스턴스 변수로 사용할 수 없습니다. C++ 의 경우 가상 함수 테이블이라는게 존재하여 테이블에서 적절한 포인터를 가져오는게 가능하지만 Objc는 가상 함수 테이블을 사용할 수 없습니다.

3. Objc는 C++ 생성자나 소멸자를 호출할 수 없습니다.

4. `class` 를 변수명을로서 사용할 수 없습니다. (objc 키워드이기 때문)

5. Objc 구문을 사용하여 C++ 객체를 호출할 수 없으며 Objc 객체에 생성자 또는 소멸자를 추가할 수 없습니다. 즉, Objc++는 C++ 기능을 Objc 클래스에 추가하지 않으며 반대도 마찬가지 입니다.

6. self와 this 키워드를 상호 교대로 사용할 수 없습니다.