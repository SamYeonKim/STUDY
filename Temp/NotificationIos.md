- [배경지식](#%EB%B0%B0%EA%B2%BD%EC%A7%80%EC%8B%9D)
  - [미리 읽어야할 문서들](#%EB%AF%B8%EB%A6%AC-%EC%9D%BD%EC%96%B4%EC%95%BC%ED%95%A0-%EB%AC%B8%EC%84%9C%EB%93%A4)
- [설정방법](#%EC%84%A4%EC%A0%95%EB%B0%A9%EB%B2%95)
  - [원격 알림 테스트](#%EC%9B%90%EA%B2%A9-%EC%95%8C%EB%A6%BC-%ED%85%8C%EC%8A%A4%ED%8A%B8)
- [사용방법](#%EC%82%AC%EC%9A%A9%EB%B0%A9%EB%B2%95)
  - [알림 초기화하기](#%EC%95%8C%EB%A6%BC-%EC%B4%88%EA%B8%B0%ED%99%94%ED%95%98%EA%B8%B0)
  - [원격 메시지 처리](#%EC%9B%90%EA%B2%A9-%EB%A9%94%EC%8B%9C%EC%A7%80-%EC%B2%98%EB%A6%AC)
  - [로컬 알림 등록하기](#%EB%A1%9C%EC%BB%AC-%EC%95%8C%EB%A6%BC-%EB%93%B1%EB%A1%9D%ED%95%98%EA%B8%B0)

-----

# 배경지식

## 미리 읽어야할 문서들

* [Unity RemoteNotification Docs](https://docs.unity3d.com/ScriptReference/iOS.RemoteNotification.html)
  * 유니티에서 사용하는 원격 알림 기능을 제공하는 클래스 문서
* [Unity and IOS push Notificatoin](https://docs.google.com/document/d/1uASypQ6NLfNfu4AMhXtTaA8FZc8olkJt3Wn5LUszeVQ/edit)
  * 다른 사용자가 정리한 UnOfficial한 IOS Notification Guide


# 설정방법

* [애플 개발자 콘솔](https://developer.apple.com/account/resources/certificates/list) 에서 `Apple Push Notification service SSL` 을 생성한다.
  * 생성 방법은 [여기](http://monibu1548.github.io/2018/05/29/push-cert/) 참고
* 키체인에 발급받은 인증서를 등록한 후 `인증서 내보내기` 를 한다.
* `.p12` 로 끝나는 인증서를 서버에게 전달한다.
* 자세한 생성과정은 [여기](https://devmjun.github.io/archive/APNs) 참고


# 사용방법
  
* [IosNotification.cs](../Lib/Notification/Assets/.LocalNotification/Script/IosNotification.cs) 참조

## 알림 초기화하기

* `NotificationAdapter.Open()`을 통해 호출된다.
* `NotificationBehaviour` 클래스의 `Open()`에서 `StartNotification()`을 호출한다.
* `nityEngine.iOS.NotificationServices.RegisterForNotifications()` 함수를 호출하여 로컬, 리모트 알림 기능 사용을 활성화한다.
* 리모트의 경우, 유니티 API가 `APNS`로부터 토큰을 받았는지 확인하기 위해 주기적으로 `CheckRegister()`를 호출한다. 성공 혹은 실패할 경우 `NotificationBehaviour.OnOpened()`를 호출한다.
* 로컬의 경우, 유니티가 기억하고 있던 스케쥴링 되어있는 알림들을 `m_d_scheduled`에 저장하고, 알림을 처리하는 로직을 바로 수행한다.

## 원격 메시지 처리

* `StartNotification()` 이후 애플리케이션은 `StartRecieveMessage()`를 실행하여 주기적으로 원격 알림이 왔는지 확인한다.
* `StartRecieveMessage()`는 `InvokeRepeating()`을 실행하여 주기적으로 `ReceiveMessage()`를 호출한다.
* `ReceiveMessage()`는 유니티에서 제공해주는 `UnityEngine.iOS.NotificationServices` 클래스를 통해 원격 알림이 있는지 확인하고 만약 알림이 있다면 `NotificationBehaviour.OnRemoteMessage()`를 호출한다.

## 로컬 알림 등록하기

* `NotificationAdapter.ReserveOnce()`을 통해 호출된다.
  * 반복 알림을 등록하고 싶다면 `NotificationAdapter.ReserveRepeat()` 함수를 호출한다.
* `NotificationBehaviour` 클래스의 `Send()`에서 `SendCustom()`을 호출한다.
* 파라미터로 받은 알림 정보인 `LocalNotificationParams` 인스턴스에서 iOS에 사용할 정보를 빼내어 `UnityEngine.iOS.LocalNotification` 인스턴스에 추가한다.
* 추가적인 정보는 `UnityEngine.iOS.LocalNotification.userInfo`에 저장되어 로컬 알림 세팅 시 사용된다.
  * 야간 알림, 인게임 내 메시지에 관한 정보가 포함된다.
* `UnityEngine.iOS.NotificationServices.ScheduleLocalNotification()`에 구성한 정보를 등록하고 `m_d_scheduled`에도 추가한다.
* 상단부에 알림 위젯을 생성하는 작업은 유니티 API가 한다.
* 인게임에서 알림이 왔는지 확인하기 위해서, 초기화 시 주기적으로 실행해도록 설정한 `CheckAndNoticeLocalScheduled()` 함수가 로컬 알림이 있는지 판단하여 `NotificationBehaviour.OnLocalMessage()`를 호출한다.