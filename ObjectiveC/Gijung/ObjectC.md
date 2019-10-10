`Source File`

+ 객체지향 프로그래밍에 따라 선언부와 구현부를 구분하여 저장한다.
+ Interface는 .h 파일에, Implementation 파일은 .m 파일에 저장한다.

`Class Interface`

+ 선언은 `@interface` 로 시작 `@end` 로 끝난다.

```
@interface Shape {
    // 멤버 변수 선언
    float m_width;
    float m_height;
}

// 함수 선언
- (void)display;    
- (void)SetVariable;
@end
```

+ Importing the Interface

```
// .h에 선언된 Interface를 사용, 상속을 받기 위해서는 Import를 해줘야한다.
// 그렇지 않으면 Interface가 없다는 에러가 발생한다.

#import <Foundation/Foundation.h>

@interface Shape : NSObject{
    ...
}
...

@end
```

+ Referring to Other Classes

```
// Class = Interface + Implementation
// @class 명령을 사용하면 클래스를 정의하지 않고 이름만 선언할 수 있다.
// 참조만 가능할 뿐 메소드, 인스턴스 변수 등의 기능은 사용할 수 없다.
// @interface에서 내부에서 선언된 Class의 .h를 Import하지 않고 사용 할 수 있어 컴파일러의 부담을 줄여 줄 수 있다.

#import <Foundation/Foundation.h>

@class A;
@class B;

@interface Shape : NSObject {
    float width;
    float height;
    
    A *a;
    B *b;
}

- (void)SetVariable;
- (void)display;
@end
```

`Class Implementation`

+ 선언은 `@implementation` 로 시작, `@end` 로 끝난다.

```
#import "Shape.h"

@implementation Shape

- (void)SetVariable {
    ...
}

- (void) Display {
    ...
}
@end
```

+ 함수 선언 방식

```
- (void)SetVariable {
    ...
}

- (void)Display {
    ...
}
```

+ Referring to Instance Variables

```
- (void)SetExternalVariable:(int)w { // 매개변수로 넘어온 w 함수 내부에서 사용한다.
    width = w;
}
```

+ The Scope of Instance Variables

|Directive|Meaning|
|:--|:--|
|@private|선언된 클래스 내부에서만 접근 가능|
|@protected|선언된 클래스와 상속받은 클래스에서만 접근 가능|
|@public|어디서나 접근 가능|
|@package|동일한 프레임 워크, 라이브러리 또는 실행 파일의 코드에서만 접근 가능|

```
// 아무것도 선언하지 않는 다면 기본적으로 @protected가 적용된다.

@public
    float width;
    
@private
    float height;

@protected
    A *a;
    B *b;
```

`Messages to self and super`

```
@implementation Rectagle

- (void)Display {
    NSLog(@"Hello World");
}

- (void)SelfDisplay {   // Self 는 자신이 속한 클래스의 함수를 호출.
    [self Display];
}

- (void)SuperDisplay {  // Super 는 상속받은 부모 클래스의 함수를 호출. 
    [super Display];
}
@end

2018-08-13 01:29:35.071 ClassTest[2485:95027] Hello World
2018-08-13 01:29:35.071 ClassTest[2485:95027] 0.000000, 0.000000
```