* Object는 데이터에 영향을 받는 특정 작업을 수행하는 데이터의 일종이다. Objective-C에서 이러한 작업을 object의 메소드라고 한다. 영향을 주는 데이터는 인스턴스의 변수이다.

----
## id

* object identifiers.
* 클래스에 관계없이 어떠한 종류의 오브젝트도 될 수 있는 general type.
* 인스턴스와 클래스 오브젝트 둘 다 사용이 가능하다.

----
## Memory Management

* Objective-C에서 더이상 사용하지 않는 오브젝트를 해제하는 것이 중요하다.
* Reference counting, Garbage collection 두 가지 환경을 제공한다.

----
## Object Messaging

* 어떤 일을 하기 위해 오브젝트를 얻으려면, 메소드에 메시지를 보내야 한다. receiver는 오브젝트, message는 메소드와 해당 메소드의 변수를 나타낸다.

```
[receiver message]
```

* 메시지가 보내지면, 런타임 시스템은 적절한 메소드를 선택하고 호출한다.
* 메소드 이름은 메소드 구현을 선택하는 역할을 하기 때문에, 메시지에서 `selector`라고 부른다.
* 메소드의 매개변수를 표현할 때는 콜론(:)을 사용하고 뒤에 값을 적는다.
* 콜론 앞에 변수명을 적는 것이 좋은 표기법이다. 안써도 작동은 한다.

```
[myRectangle setOrigin:30.0 :50.0]; // This is a bad example of multiple arguments
[myRectangle setOriginX: 30.0 y: 50.0];  // This is a good example of multiple arguments
```

* 가변 인수를 매개변수로 받는 메소드의 경우 콤마(,)로 구분한다.

```
[receiver makeGroup:group, memberOne, memberTwo, memberThree];
```

* nil인 오브젝트에 메시지를 보낼 수도 있다. 이 경우 메소드의 리턴타입에 맞는 기본값을 리턴한다.

* 메시지를 보낼 때 메소드 이름에 변수를 넣으면 인스턴스의 변수로 자동 접근한다.

* 접근자 메소드(변수를 수정하기 위해 자동으로 만들어지는 메소드, set, get)를 사용할 때  bracket([])대신 dot(.)을 사용할 수 있다.
* 다른 인스턴스의 변수를 수정하려면 포인터접근을 해야한다.(otherObject->variable) 

```
myInstance.value = 10;
printf("myInstance value: %d", myInstance.value);

// 위의 식은 아래와 동일한 결과를 출력한다.
[myInstance setValue:10];
printf("myInstance value: %d", [myInstance value]);
```

* dot syntax는 read-only property에 접근하려고 할 때 에러를 호출한다.
* dot syntax는 타입이 정해지지 않은 오브젝트(id 타입)로는 접근할 수 없다.
* dot syntax는 nil 값을 가지는 property로 접근할 수 있다. 다음 예시의 식들은 완전히 동일한 결과를 출력한다.

```
x = person.address.street.name;
x = [[[person address] street] name];

y = window.contentView.bounds.origin.y;
y = [[window contentView] bounds].origin.y;

person.address.street.name = @"Oxford Road";
[[[person address] street] setName: @"Oxford Road"];
```

* `self.age = 10` 에서 `age`는 property의 setter이다. `self.`를 없애면 `age`는 인스턴스 변수가 된다.


----
## Class

* 컴파일러는 각 클래스마다 하나의 오브젝트를 생성한다. 이 클래스 오브젝트는 런타임에 인스턴스를 생성한다.
* 모든 클래스는 `NSObject`를 상속받는다. `NSObject`는 superclass가 없는 root class이다.
* 다른 언어와 다르게 Objective-C는 추상 클래스를 표현하는 문법이 없다. 인스턴스를 선언할 수 있다.

----
## Class Types

* 클래스 정의는 오브젝트에 대한 세부적 명시이다.
* 오브젝트의 타입을 명시하는 것을 `Static Typing`이라고 한다.

```
Rectangle *myRectangle;

// Rectangle은 Graphic의 subclass이다. (a kind of)
Graphic *myRectangle;
```

* `isMemberOfClass:` 메소드는 런타임에 어떤 인스턴스가 해당 클래스의 인스턴스인지 여부를 확인할 수 있다.
* `isKindOfClass:` 메소드는 해당 인스턴스가 클래스의 상속 경로에 있는지 확인할 수 있다.

----
## Class Objects

* 클래스 정의는 다양한 정보를 포함하며, 클래스의 인스턴스에 관한 많은 정보를 포함한다.
* 클래스 오브젝트는 컴파일러가 생성한다. 인스턴스 정보에는 접근할 수 없다. 
* 클래스 오브젝트는 클래스 이름으로 표현된다. 리시버에 클래스 이름을 사용할 수 있지만 다른 곳에 사용하려면 오브젝트 자체가 필요하다.

```
Class aClass = [anObject class];
Class rectClass = [Rectangle class];
```


* 클래스 오브젝트는 주로 런타임에 인스턴스를 생성하는 역할을 한다.
* alloc 메소드는 새 오브젝트 인스턴스에 동적으로 메모리를 할당하고 모든 것을 0으로 초기화한다. 하지만 init을 바로 호출하는 것이 좋다.
* 모든 클래스 오브젝트는 alloc과 같은 메소드를 적어도 1개 이상 가진다. 인스턴스는 init과 같은 메소드를 적어도 1개 이상 가진다.


* 클래스 오브젝트를 만든 이유중 하나는 구조를 크게 바꾸지 않고 새로운 기능을 추가할 수 있는 코드를 만들게 도와주는 것이다.(??)


* 인스턴스는 각자의 변수를 가지지만 클래스 변수는 따로 없다. 클래스 오브젝트는 인스턴스 변수에 접근할 수 없다.
* 모든 인스턴스들이 데이터를 공유하려면 외부(external) 변수를 사용해야 한다.

```
int MCLSGlobalVariable;
@implementation MyClass
// implementation continues
```

* 더 좋은 방법은 static 변수를 선언하고 클래스 메소드가 관리하게 하는 것이다. static 변수는 범위가 클래스로 제한된다?
* 외부 변수를 선언할 때 static 을 사용하지 않아도 되지만, 스태틱 변수의 제한된 범위가 데이터를 캡슐화 하는 목적에 더 적합하다.


* 인스턴스를 할당하는 것 외에 다른 작업을 위해 클래스 오브젝트를 사용하려면 인스턴스처럼 초기화를 해야한다. 
* 런타임 시스템은 모든 클래스 오브젝트의 initialize에게 가장 먼저 메시지를 보낸다.(부모->자식 순)
* 초기화가 필요하지 않으면 initialize 메소드를 작성할 필요가 없다. 하지만 상속관계가 있으면 initialize를 구현하지 않은 메소드의 메시지는 수퍼클래스로 넘어간다.
* 초기화 로직이 1번 이상 불리는 것을 방지하기 위해 초기화 메소드를 구현할 때 다음과 같은 템플릿을 사용해야 한다.
```
+ (void)initialize {
 if (self == [ThisClass class]) {
 // Perform initialization here.
 ...
 }
}
```

* 클래스나 인스턴스 같은 모든 오브젝트는 런타임 시스템에 대한 인터페이스를 필요로 한다. 이 인터페이스를 제공하는 것은 NSObject 클래스의 영역이다.
* 클래스 오브젝트가 클래스 메소드로 응답할 수 없는 메시지를 받으면 런타임 시스템은 응답할 수 있는 루트 인스턴스 메소드가 있는지 여부를 확인한 후 수행한다.

----
## Class Name in Source Code

* 클래스 이름은 데이터 타입과 클래스 오브젝트 두 가지 역할을 할 수 있다.
* 데이터 타입 : static typing
* 인스턴스만 static typing이 가능하다. 클래스 오브젝트는 Class 데이터 타입에 속하기 때문에 안된다.
* 클래스 오브젝트 : 메시지 표현의 리시버
* 컴파일 타임에 클래스 이름을 모르고 런타임에 스트링으로 가진다면, NSClassFromString을 사용하여 클래스 오브젝트를 리턴할 수 있다.

----
## Testing Class Equality

* 직접 포인터 비교를 사용하여 두 클래스 오브젝트가 같은지 테스트할 수 있다. 
* 클래스 동등성을 테스트할 때 오버라이드 등의 이유로 하위 수준 함수에서 반환하는 값 대신 클래스 메소드에서 반환한 값을 비교해야 한다.