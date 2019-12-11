## Enabling Static Behaviour

### What is Dynamic Typing
Objective-C 에서 모든 클래스 인스턴스는 `id`로 사용할 수 있다. 클래스 인스턴스의 선언부는 다음과 같이 작성 할 수 있다.

~~~objectivec
id InstanceName;
~~~

컴파일타임에는 클래스 타입을 명명하지 않고, 
런타임에 설정되도록 하는것이다. 이렇게 작성함으로써 반복되고 긴 클래스명을 적을 필요 없어진다.

-----

### Dynamic Typing Problem

Dynamic Typing은 작성의 용의함은 있으나, 문제를 야기 할 수 있다.

~~~objectivec
id a = [[A alloc]init];
[a show];    
id b = [[B alloc] init];
[b show];
~~~
`B` 클래스는 `A` 클래스를 상속 받았고, `show` 함수는 `B` 클래스에만 선언된 함수이다.

위와 같은 코드에서 컴파일러는 아무런 에러도 주지 않는다. 하지만 실행하게 되면 런타임에서 에러가 발생 하는데, `A`에 `show` 라는 함수를 찾을 수 없기 때문에, Exception이 발생한다.

하지만 위 코드를 아래와 같이 수정 하게 되면, 컴파일 단계에서 에러가 발생하기 때문에, 원인을 알 수 있다.

~~~objectivec
A *a = [[A alloc]init];
[a show];   //No visible @interface for `A` declarest the selector~
B *b = [[B alloc] init];
[b show];
~~~

위와 같이 Dynamic Typing은 문제를 야기 할 수 있기 때문에, Static Typing으로 코딩 할 필요가 있다.

-----

### Static Typing

`Static Typing` 은 `Dynamic Typing` 처럼 런타임에 타입이 결정되는것이 아니라, 컴파일 타임에 결정되기 때문에, 더 확실한 에러 확인이 가능 하다.




