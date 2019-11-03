- [배경지식](#%EB%B0%B0%EA%B2%BD%EC%A7%80%EC%8B%9D)
  - [미리 읽어야할 문서들](#%EB%AF%B8%EB%A6%AC-%EC%9D%BD%EC%96%B4%EC%95%BC%ED%95%A0-%EB%AC%B8%EC%84%9C%EB%93%A4)
- [설정방법](#%EC%84%A4%EC%A0%95%EB%B0%A9%EB%B2%95)
  - [Firebase 콘솔 설정하기](#firebase-%EC%BD%98%EC%86%94-%EC%84%A4%EC%A0%95%ED%95%98%EA%B8%B0)
  - [원격 알림 테스트](#%EC%9B%90%EA%B2%A9-%EC%95%8C%EB%A6%BC-%ED%85%8C%EC%8A%A4%ED%8A%B8)
- [사용방법](#%EC%82%AC%EC%9A%A9%EB%B0%A9%EB%B2%95)
  - [원격 알림 초기화하기](#%EC%9B%90%EA%B2%A9-%EC%95%8C%EB%A6%BC-%EC%B4%88%EA%B8%B0%ED%99%94%ED%95%98%EA%B8%B0)
  - [원격 메시지를 받은 후 처리](#%EC%9B%90%EA%B2%A9-%EB%A9%94%EC%8B%9C%EC%A7%80%EB%A5%BC-%EB%B0%9B%EC%9D%80-%ED%9B%84-%EC%B2%98%EB%A6%AC)
  - [로컬 알림 등록하기](#%EB%A1%9C%EC%BB%AC-%EC%95%8C%EB%A6%BC-%EB%93%B1%EB%A1%9D%ED%95%98%EA%B8%B0)

-----

# 배경지식

* 안드로이드 원격 알림 기능은 `FCM` 으로 구현하였다.

## 미리 읽어야할 문서들

* [Firebase Unity SDK](https://firebase.google.com/docs/unity/setup?hl=ko)
  * google 에서 배포하는 유니티용 Firebase SDK
* [Message Receive Process](https://firebase.google.com/docs/cloud-messaging/android/receive?hl=ko)
  * Android 앱에서 원격 알림을 수신하였을 때 처리하는 방법


# 설정방법

## Firebase 콘솔 설정하기

* [Firebase 콘솔](https://console.firebase.google.com/u/0/?hl=ko)에서 `프로젝트 추가`를 진행한다.

* 프로젝트 이름과 ID를 작성한다. ID는 유니티에서 사용하는 `BundleIdentifier`를 적어주어야 한다.
  
![](img/Notification/console_add.png)

* 메인 화면에서 유니티용 앱을 추가한다.
  
![](img/Notification/console2.png)

* 필요한 정보를 모두 기입하고 `구성 파일`을 다운로드 받는다.
  
![](img/Notification/console3.png)

* 다운로드 받은 구성 파일을 `Assets` 폴더 아래 어느곳이든 추가한다.
  * 현재 프로젝트는 `Assets | Plugins | Android | Firebase` 아래에 존재한다.
* 콘솔의 `프로젝트 설정 | 클라우드 메시징` 탭에 들어간다.
  
![](img/Notification/console4.png)

* `서버 키` 를 서버팀에 전달한다.

## 원격 알림 테스트

* [Firebase 콘솔](https://console.firebase.google.com/u/0/?hl=ko)에서 `성장` 탭의 `Cloud Messaging`을 선택한다.
  
![](img/Notification/msg1.png)

* `Send your first message` 버튼을 클릭한다.
* 알림 제목과 텍스트를 작성한 후 `테스트 메시지 전송` 버튼을 클릭한다.
* 해당 플러그인이 추가된 애플리케이션을 빌드한 후, `INotificationListener.OnNotificationRegistered()` 에서 받은 토큰 id를 추가한다.
  
![](img/Notification/msg2.png)

* 알림이 도착했는지 확인한다.


# 사용방법
  
* [AndroidNotification.cs](../Lib/Notification/Assets/.LocalNotification/Script/AndroidNotification.cs) 참조

## 원격 알림 초기화하기

* `NotificationAdapter.Open()`을 통해 호출된다.
* `NotificationBehaviour` 클래스의 `Open()`에서 `StartNotification()`을 호출한다.
* `Firebase` 클래스에 구현된 델리게이트에 콜백 함수를 등록하면 `Firebase Initialize` 이후 해당 앱의 토큰이 파라미터에 추가되어 콜백함수가 호출된다.
* 해당 토큰을 `Firebase`에 등록하는 과정이 필요하다.

## 원격 메시지를 받은 후 처리

* `Firebase.Messaging.FirebaseMessaging.TokenReceived` 에 등록된 콜백 함수가 호출된다.
* `notification`이 `null`이면 잘못된 메시지라고 판단하고 이벤트 핸들러를 호출하지 않는다.

## 로컬 알림 등록하기

* `NotificationAdapter.ReserveOnce()`을 통해 호출된다.
  * 반복 알림을 등록하고 싶다면 `NotificationAdapter.ReserveRepeat()` 함수를 호출한다.
* `NotificationBehaviour` 클래스의 `Send()`에서 `SendCustom()`을 호출한다.
* 안드로이드 알림 설정에 필요한 모든 정보는 `LocalNotificationParams`에 추가한다.
* 추가적인 정보는 `LocalNotificationParams.CallbackData`에 저장되어 플러그인 내부에서 해당 알림을 해석하는데 사용된다.
  * 야간 알림, 인게임 내 메시지에 관한 정보가 포함된다.
* 정해진 시간이 되면 플러그인 내부에서 `onReceive()` 가 호출되고, 해당 함수 내부에서 상단바에 알림 위젯을 생성하거나, 유니티에 메시지를 전달한다.
  
* [Controller.java](../LibAndroid/Push/push/src/main/java/com/nadagames/push/Controller.java) 의 `onReceive()` 참조