# Remote Messaging

* 특정 클라이언트 개체에게 메시지를 보내는 프로세스간 통신

----
## Distributed Objects

* Objective-C 메시지를 다른 작업의 개체로 보내거나 동일한 작업의 다른 스레드에서 메시지를 실행할 수 있는 객체
* 원격 메시지를 보내기 위해서 원격 수신기와 연결을 설정해야 한다.
* 연결을 설정하면 어플리케이션이 자체 주소 공간에 객체에 대한 프록시를 제공한다.
* 객체는 프록시를 통해 원격 객체와 통신한다.
* 어플리케이션은 프록시를 원격 객체인 것처럼 인식한다.
* 어플리케이션은 원격 객체의 클래스를 알 필요가 없고, 응답하는 메시지만 알면된다.
* 메시지를 수신하기 위해서는 송,수신 프로그램 모두 프로토콜을 선언한다.

----
## Language Support

* 원격 메시지의 의도에 대해 명시적으로 지시하기 위해서 메소드 선언 시 사용할 수 있는 여섯가지 형식 한정자를 제공한다.
* 이 한정자들은 공식 프로토콜에서만 사용할 수 있다. 카테고리나 클래스 선언시 사용이 불가능하다.
  * oneway
  * in
  * out
  * inout
  * bycopy
  * byref

### Synchronous and Asynchronous Messages

* 대부분의 원격 메시지는 `two-way(round trip)`, 양방향 이다.
* 송신 프로그램은 메시지를 보내고 수신 프로그램이 처리를 완료할 때 까지 기다린다. 이 과정은 동기화 상태를 유지하기 때문에 `synchronous`라고 부른다. 기본값이다.
* 비동기 메시지를 사용하려면 반환 형식 수정자 `oneway`를 사용하면 된다.
  * 비동기 메시지는 유효한 반환 값을 가질 수 없다.

```objc
-(oneway void)waltzAtWill;
 ```

### Pointer Arguments

* 포인터 파라미터를 가지는 메소드는 정확한 객체의 값을 전달하거나 전달받기 위해 `in`, `out`, `inout` 형식 수정자를 사용해야 한다.
* `in`은 정보가 메시지로 전달 된다는 것을 나타내고, `out`은 정보를 참조형식으로 반환한다는 것을 나타낸다. `inout`은 양방향 변화를 한다.

```objc
- setTune:(in struct tune *)aSong;
- getTune:(out struct tune *)theSong;
- adjustTune:(inout struct tune *)aSong;
```

* 코코아 분산 객체 시스템에서 `const`로 선언된 것을 제외한 모든 포인터 매개변수들은 `inout`을 기본 수정자로 사용한다. 기본값은 `in`이다.
* `in` 은 어떤 종류의 인수와도 사용가능하지만 `out`, `inout`은 포인터만 가능하다.
* 문자 포인터(`char *`)는 포인터로 쳐주지 않는다.

```objc
- getTuneTitle:(out char **)theTitle;
- adjustRectangle:(inout Rectangle **)theRect;
```

### Proxies and Copies

* 객체를 인수로 가지는 메소드는 해당 객체의 포인터가 프록시를 가리키고 있기 때문에 항상 프록시를 통해 원격 객체를 참조해야 한다.
* 프록시가 비효율적일 수 있기 때문에 객체의 복사본을 보낼 수 있는 `bycopy` 수정자를 제공한다.

```objc
- danceWith:(bycopy id)aClone;
- (bycopy)dancer;
```

* `byref` 키워드는 객체를 참조 형태로 전달하거나 반환할 수 있게 만들어주지만, objc의 기본 동작이기 때문에 거의 사용하지 않는다.
* 