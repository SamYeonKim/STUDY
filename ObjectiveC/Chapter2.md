
----
## Source File

* 선언부와 구현부를 구분하여 저장
* Interface는 .h 파일에, Implementation 파일은 .m 파일에 저장

----
## Class Interface

* `@interface` 로 시작 `@end` 로 끝낸다.
* `+`는 클래스 메소드, `-`는 인스턴스 메소드를 의미한다.

```objc
@interface Test:NSObject {
    int temp;
}
- (int)max:(int)num1 :(int)num2;
@end
```

### Importing the Interface

* c++의 include, c#의 using 과 동일하다.

```objc
#import <Foundation/Foundation.h>
```

### Referring to Other Classes

* `@class` 명령어를 사용하여 이름만 선언이 가능하다.
* `@interface` 내부에서 사용된 class의 헤더파일을 import 하지 않고 사용이 가능하기 때문에 컴파일러의 부담을 줄여줄 수 있다.

```objc
@class A;

@interface Test:NSObject {
    A *a;
    int temp;
}
- (int)max:(int)num1 :(int)num2;
@end
```

----
## Class Implementation

* `@implementation` 로 시작 `@end` 로 끝낸다.

```objc
@implementation Test
- (int)max:(int)num1 :(int)num2 {
   int result;
 
   if (num1 > num2) {
      result = num1;
   } else {
      result = num2;
   }
 
   return result; 
}

@end
```

### Referring to Instance Variables

* 파라미터로 받은 변수는 그냥 사용할 수 있다.
* 외부에서 객체의 변수에 접근할 때, 구조체 포인터 연산자(`->`)를 사용한다.

```objc
int main () {
   int a = 100;
   int b = 200;
   int ret;
   
   Test *test = [[Test alloc]init];
   NSLog(@"Temp value is : %d\n", test->temp );
   
   ret = [test max:a :b];
 
   NSLog(@"Max value is : %d\n", ret );
   return 0;
}
```
* output
```
2019-04-17 23:27:59.168 a.out[12] Temp value is : 0 
2019-04-17 23:27:59.246 a.out[12] Max value is : 200
```

### The Scope of Instance Variables

|Directive|Meaning|
|:--|:--|
|@private|선언된 클래스 내부에서만 접근 가능|
|@protected|선언된 클래스와 상속받은 클래스에서만 접근 가능|
|@public|어디서나 접근 가능|
|@package|동일한 프레임 워크, 라이브러리 또는 실행 파일의 코드에서만 접근 가능|

* 아무것도 선언하지 않는 다면 기본적으로 @protected가 적용된다.
```
@interface Test:NSObject {
    A *a;
@public
    int temp;
@private
    int temp_private;
@package
    int temp_package;
}
```

## Messages to self and super

```objc
// main.h
@interface Parent:NSObject
-(void)selfDisplay;
@end

@class A;
@interface Test:Parent {
    A *a;
@public
    int temp;
@private
    int temp_private;
@package
    int temp_package;
}
-(void)display;
-(void)selfDisplay;
- (int)max:(int)num1 :(int)num2;
@end

// main.m
#import <Foundation/Foundation.h>
#import <main.h>

@implementation Parent
-(void)selfDisplay {
   NSLog(@"My name is Parent");
}

@end

@implementation Test
-(void)display {
   [self selfDisplay];
}
-(void)selfDisplay {
   NSLog(@"My name is Test");
   [super selfDisplay];
}

- (int)max:(int)num1 :(int)num2 {
   int result;
 
   if (num1 > num2) {
      result = num1;
   } else {
      result = num2;
   }
 
   return result; 
}

@end

int main () {
   int a = 100;
   int b = 200;
   int ret;
   
   Test *test = [[Test alloc]init];
   [test selfDisplay];
   [test display];
   
   ret = [test max:a :b];
 
   NSLog(@"Max value is : %d\n", ret );
   return 0;
}

// output
2019-04-17 23:47:20.574 a.out[12] My name is Test                                                               
2019-04-17 23:47:20.588 a.out[12] My name is Parent                                                             
2019-04-17 23:47:20.588 a.out[12] My name is Test                                                               
2019-04-17 23:47:20.589 a.out[12] My name is Parent                                                             
2019-04-17 23:47:20.589 a.out[12] Max value is : 200 
```