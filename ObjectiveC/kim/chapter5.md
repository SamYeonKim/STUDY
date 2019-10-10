## Category and Extensions

### Category

* 직접 수정 할 수 없는 클래스에 대해서 추가 적인 기능을 구현하고자 할 때 사용
    > c#의 `extension method` 와 유사
* 멤버 변수는 선언 할 수 없고, 함수만 정의 할 수 있다.
* 클래스의 덩어리가 너무 커졌을때 기능 별로 묶음의 용도로 사용 할 수 있다.
* 선언 방식은 아래와 같다.
    ~~~ objectivec
    #import "ClassName.h"
    @interface ClassName ( CategoryName )
    @end
    ~~~
* 만약 동일 한 이름의 함수를 재정의 할 경우 새로 선언한 Category의 함수가 호출 된다.
* 만약 동일 한 이름의 함수를 2개 이상의 Category에서 재정의 할 경우 호출되는것은 마지막으로 선언된 `#import` 된 Category이다.
##### Example

~~~objectivec

Person.h {
    @interface Person : NSObject
    {
        int *age;
        NSString *name;
    }
    @property int *age;
    @property NSString *name;
    - (void) showInfo;
    @end

    @implementation Person
    @synthesize age;
    @synthesize name;

    - (void)showInfo {
        NSLog(@"AGE : %d", age);
        NSLog(@"NAME : %@",name);
    }
    @end
}

Move.h {
    #import "Person.h"

    @interface Person (Move)
    - (void) moveToHome;
    @end

    @implementation Person (Move)
    - (void) showInfo {
        NSLog(@"Iam Mover");
    }
    - (void) moveToHome {
        NSLog(@"GoTo Home");
    }
    @end
}

main.m {
    Person *human = [[Person alloc]init];
    [human setAge:12];
    [human setName:@"kim"];
    [human showInfo];
    [human moveToHome];
}
~~~

* 위 코드는 `Person`, `Person (Move)` 의 내용이다.
* `Person (Move)` 는 기존에 없는 새로운 `moveToHome` 이라는 함수를 정의 했고,  `showInfo` 를 재 정의 했다.
* main 함수를 수행 하게 되면 아래와 같은 결과가 나온다.
    > Iam Mover
    GoTo Home
----

### Extensions

* `익명`의 Category 이다.
* 명명된 Category와 다른 점은 함수의 정의를 확장 하려는 클래스의 @implementation 에서 해야 한다는것 이다.
* 함수의 선언은 별도로 할 수 있기때문에, 선언부를 노출 하고 싶지 않을때 사용한다.
* Category와는 다르게 멤버 변수를 선언 할 수 있는 특징이 있다.

##### Example

~~~objectivec

Person.h {
    @interface Person : NSObject
    {
        int *age;
        NSString *name;
    }
    @property int *age;
    @property NSString *name;
    - (void) showInfo;
    @end
}

Person.m{
    @implementation Person
    @synthesize age;
    @synthesize name;
    @synthesize memorialDay;

    - (void)showInfo {
        NSLog(@"AGE : %d", age);
        NSLog(@"NAME : %@",name);
    }
    - (void) die {
       NSLog(@"Dead");
    }
    @end
}

Etc.h {
    @interface Person () {
        int memorialDay;
    }
    @property int memorialDay;
    - (void) die;
    @end
}
~~~

* `die` 라는 함수의 선언은 `Etc.h` 에서 했지만, 구현은 원래 클래스인 `Person.h` 에서 했다.
* 그리고 Category의 선언에서 빈 칸으로 되어 있다는 것을 알 수 있다,
    ~~~objectivec
    @interface Person ()
    ~~~
