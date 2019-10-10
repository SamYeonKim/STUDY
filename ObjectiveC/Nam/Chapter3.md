# Allocating and Initializing Objects

Objective-C 를 사용하여 객체를 만들때에는 두 단계가 필요하다.

+ 새 객체에 동적으로 메모리 할당
+ 새로 할당된 메모리를 적절한 값으로 초기화

두 단계가 완료 될 때 까지는 객체가 완전히 기능하지 않음
각 단계는 별도의 메소드로 수행되지만 보통 한줄의 코드로 수행할 수 있음

```
// 할당과 초기화를 분리하여 각 단계를 개별적으로 제어
id Object = [[CustomClass alloc] init];
```

## Allocating

새 객체의 메모리는 NSObject 클래스에 정의된 클래스 메소드 `alloc` 를 사용하여 할당 됨

```
CustomClass *c = [CustomClass alloc];

alloc : 해당 클래스에 메모리를 할당한다. (새로 할당된 객체의 isa 인스턴스 변수가 객체의 클래스를 가리키도록 하고, 모든 인스턴스 변수를 0으로 초기화함)
```

## Init

클래스의 인스턴스를 초기화 하고 반환한다.

```
c = [c init];
```

Objective-C 에서 초기화 메소드를 작성하려면 다음 규칙을 따라야 한다.

1. 초기화 메소드는 반드시 슈퍼클래스의 초기화 메소드를 호출해야 한다.
2. 반드시 self 변수에 적절한 값을 설정해야 한다.
3. 반드시 nil 인스턴스인지 확인해야 한다.
4. 반드시 자기 스스로에 대한 포인터를 반환해야 한다.
5. 인스턴스 변수의 값을 설정하는 경우 접근자 메소드를 사용하지 않고, 직접 할당을 사용한다.

```
-(instancetype)init {
    self = [super init];
    if(self != nil) {
        // custom init code
    }
    return self;
}
```

+ 전통적으로 인수가 필요한 초기화 메소드의 경우 "init" 뒤에 인수이름을 접미사로 붙힌다. ex) initWithFrame
+ [CustomClass new] 라는 단축 명령이 존재하는데 이는 내부적으로 NSObject의 +(id)alloc 및 -(id)init 메소드를 호출한다. 인자가 없는 초기화 메소드 경우에만 사용 가능
+ `alloc`을 하는 시점에 모든 인스턴스 변수는 `0`이나 `nil`로 채워지기 때문에 불필요한 `init` 메소드를 정의하지 않도록 한다.

하나 이상의 인수를 받는 초기화 메소드를 정의할 필요가 있는 경우 다음과 같이 사용한다.

```
-(instancetype)initWith<Name>:(Type)arg <Name>:(Type)arg

-(instancetype)initWithInt:(NSInteger)number1 number:(NSInterger)number2 {
    // custom code
}
```

## Handling Initialization Failure

초기화 메소드에서 문제가 발생하면 [self release]를 호출하고 nil을 리턴해야 한다.

* 초기화 메소드에서 `nil`을 받는 객체는 이를 처리할 수 있어야 한다.
* 부분적으로 초기화 된 객체가 있는 경우 `dealloc` 메소드가 안전한지 확인해야 한다.

```
-(instancetype)initWithName:(NSString*)name {
    if(name == nil) {
        [self release];
        return nil;
    }

    self = [super init];
    if(self != nil) {
        // custom init code
    }
    return self;
}
```

### self release

```
-(void)release

참조 카운트를 감소시키고, 0이 되었을 때 dealloc을 호출한다.
```
Objective-C 에서는 메모리 누수가 생기지 않도록 참조수(Refrence Count) 라는 것이 존재한다.
`new, alloc, copy`로 객체를 생성하는 경우 해당 시점에 참조수가 증가하고, 객체가 더이상 필요없는 시점에는 `release` 메시지를 보내어 참조수를 감소시킨다.

## Coordinating Classes

상속관계에 있는 클래스를 구성할 때 초기화 메소드는 상속관계에 있는 모든 초기화 메소드가 동작하도록 보장해야 한다. 
`init` 메소드는 클래스에서 선언된 인스턴스 변수들만을 초기화 하기 때문에 `super init` 을 사용하여 초기화 하도록 한다.

```
-(id)InitWithName:(NSString *)string {
    if(self = [super init]) {
        name = [string copy];
    }
    return self;
}

-(id)init {
    return [self initWithName:"default"]
}
```

## Designated Initializer

Object-C 클래스 가운데 일부는 서브클래스의 초기화 메소드에서 슈퍼 클래스의 특정 초기화 메소드를 사용해야 한다고 지정한 경우가 있다. 이를 지정 초기화 메소드라고 한다.

모든 지정 초기화 메소드들은 상위 클래스의 지정 초기화 메소드와 연결되어야 하며, 이니셜라이저를 구현할 때 상위 클래스의 지정된 이니셜라이저를 호출하도록 해야한다.

상위 클래스의 지정 초기화 메소드와 하위 클래스의 지정 초기화 메소드가 다르다면 상위 클래스의 지정 초기화 메소드를 재정의 하여 구현한다.
