# Property

## Property 선언과 구현

### 1. 선언

`@property(attributes) type name`

#### 1-1. 속성(Attributes) 종류

`getter=getterName`

`setter=setterName`

+ getter, setter를 이용하여 원하는 함수 이름을 설정 할 수 있다.
+ getter 함수는 자동으로 함수를 만들어주지만 `다른이름의 setter를 설정 할 경우는 함수를 만들어줘야 제대로 작동`한다.

```
[.h]

@interface PropertyTest : NSObject {
    float value;
}

@property (getter=Abc, setter=Abc:) float value;
- (void) setAbc : (float)input;

@end
```

```
[.m]

@implementation PropertyTest

@synthesize value;

- (void) setAbc : (float)input {
    value = input;
}

@end
```

```
[main.m]

int main(int argc, char * argv[]) {
    PropertyTest* a = [[PropertyTest alloc] init];
    
    a.Abc = 1.0f;
    
    NSLog(@"%f", a.Abc);
}
```

`readwrite`

+ 기본으로 설정되는 속성.
+ 읽기, 쓰기 권한 부여.

```
[.h]

@interface PropertyTest : NSObject {
    float value;
}

@property (readwrite) float value;
```

`readonly`

+ 읽기 권한만 부여.

```
[.h]

@interface PropertyTest : NSObject {
    float value;
}

@property (readonly) float value;
```

+ Setter 접근시 에러

![](./Images/Error_1.png)

`assign`

+ 기본으로 제공되는 속성.
+ 단순 값을 지니는 변수를 사용할때 사용.

`retain`

+ 외부에서 파괴되는 경우를 막기위해 자체적으로 참조가 필요한 경우 사용.
+ 참조가 가능한 Object Type에서만 사용 가능 ex) NSString

`copy`

+ 객체의 복사본을 넘겨줘야 할때 사용.

```
[.m]

@implementation PropertyTest

@synthesize rTest;

- (void) setrTest:(NSString *)input {
    [rTest release];
    
    rTest = [input copy/retain];
}
@end
```

`automic / nonautomic`

+ 기본으로 automic 속성이 제공.
+ Thread Safe 기능 사용 여부 결정.
+ Thread Safe 기능을 사용 할 경우 비용이 많이 든다.

` Thread Safe`

+ 해당 프로퍼티가 동시에 접근되는것을 막아주는 역할.

### 2. 구현

`@synthesize [프로퍼티 이름]`

```
[.m]

@implementation PropertyTest
@synthesize rTest;
@end
```

`@dynamic [프로퍼티 이름]`

+ getter/setter의 구현부가 외부 클래스에 있다고 알려주어 컴파일러 경고를 받지 않도록 해준다.

```
[.m]

@implementation DynamicTest
@dynamic dTest;
@end
```

## 프로퍼티 사용

### 재설정

+ 공용으로 사용하는 헤더파일(.h)에 프로퍼티를 선언하더라도 재설정을 할 수 있다.

```
[공용.h]

@interface PropertyTest : NSObject {
    float value;
}

@property (readonly) float value;
@end
```

```
[main.m]

@interface PropertyTest()
@property (readwrite) float value;
@end

@implementation PropertyTest
@synthesize value;
@end

int main(int argc, char * argv[]) {
    PropertyTest *a = [[PropertyTest alloc] init];
    
    a.value = 1.0f;
    
    NSLog(@"%f", a.value);
    
    @autoreleasepool {
        return UIApplicationMain(argc, argv, nil, NSStringFromClass([AppDelegate class]));
    }
}
```

## 서브 클래스에서 사용

+ 부모 클래스에서 선언한 프로퍼티를 재선언하여 사용 할 수 있다.

* `a.h`

```objc

@interface PropertyTest : NSObject {
    NSString* value;
}

@property (readonly) NSString* value;
@end

@interface SubClass : PropertyTest
@property (readwrite) NSString* value;
@end
```

* `a.m`

```objc
[.m]

@implementation PropertyTest
@synthesize value;
@end

@implementation SubClass
@dynamic value;
@end
```

## Performance and Threading

+ 기본적으로 프로퍼티를 만들때 속성이 Automic이므로 비용이 많이든다.

## Runtime Difference

+ Legacy Runetime을 사용하는 컴파일러는 클래스에 선언되어있지 않은 프로퍼티가 있는 경우 에러를 반환.
+ Modern Runetime을 사용하는 컴파일러는 선언되어있지 않은 프로퍼티가 있는 경우 인스턴스를 만든다.

h1. Q

* `setter=` 에서 `:` 의 비밀은???