- [라이브러리 설명](#%eb%9d%bc%ec%9d%b4%eb%b8%8c%eb%9f%ac%eb%a6%ac-%ec%84%a4%eb%aa%85)
- [배경지식](#%eb%b0%b0%ea%b2%bd%ec%a7%80%ec%8b%9d)
- [외부라이브러리 이름 및 버전 등등](#%ec%99%b8%eb%b6%80%eb%9d%bc%ec%9d%b4%eb%b8%8c%eb%9f%ac%eb%a6%ac-%ec%9d%b4%eb%a6%84-%eb%b0%8f-%eb%b2%84%ec%a0%84-%eb%93%b1%eb%93%b1)
- [시나리오](#%ec%8b%9c%eb%82%98%eb%a6%ac%ec%98%a4)
  - [로컬 알림 등록](#%eb%a1%9c%ec%bb%ac-%ec%95%8c%eb%a6%bc-%eb%93%b1%eb%a1%9d)
- [설치방법](#%ec%84%a4%ec%b9%98%eb%b0%a9%eb%b2%95)
- [사용방법](#%ec%82%ac%ec%9a%a9%eb%b0%a9%eb%b2%95)
  - [NotificationBehaviour](#notificationbehaviour)
  - [INotificationListener](#inotificationlistener)
- [주의사항](#%ec%a3%bc%ec%9d%98%ec%82%ac%ed%95%ad)

-----

# 라이브러리 설명

* 알림과 관련된 기능을 제공한다.
  * 로컬 알림 등록하기
  * 등록한 알림 삭제하기
  * 특정 알림 삭제하기
  * 현재 화면에 보이는 알림 삭제하기
  * 등록된 야간 알림 제거하기
  * 야간 알림 시간 변경하기


# 배경지식

* 아래의 문서들을 참조
  * [NotificationAndroid.md](NotificationAndroid.md)
  * [NotificationIos.md](NotificationIos.md)


# 외부라이브러리 이름 및 버전 등등

* [Firebase Unity SDK](https://firebase.google.com/docs/unity/setup?hl=ko)
  * Version : [6.4.0](https://firebase.google.com/support/release-notes/unity)


# 시나리오

* 각 플랫폼에서 행해지는 작업은 아래의 문서를 참조
  
  * [Android](NotificationAndroid.md)
  * [iOS](NotificationIos.md)


## 로컬 알림 등록
  
* (NotificationAdapter) 1회성 알림이면 `ReserveOnce()` 함수를, 반복 알림이면 `ReserveRepeat()` 함수를 호출
* (NotificationBehaviour) `CheckNightNotification()` 함수로 야간 알림인지 확인
* (IPlatformNotification) `SendCustom()` 함수 호출 
* (IPlatformNotification) 콜백 함수 정보, 야간 알림 정보, 인게임 메시징 사용 정보를 콜백데이터에 등록한 후 시스템에 알림 등록


# 설치방법

* `Notification.unitypackage` 패키지를 임포트 한다.
* `INotificationListener` 를 상속받은 클래스를 구현한다.


# 사용방법

* [NotificationAdapter.cs](../Lib/Notification/Assets/.LocalNotification/Script/NotificationAdapter.cs) 에 구현된 `static` 함수를 필요에 맞게 호출한다.
  * 자세한 설명은 스크립트를 참조

* `INotificationListener` 를 상속받은 클래스를 구현한다.

## NotificationBehaviour

* 실제 작업을 처리하는 클래스
  
* [NotificationBehaviour.cs](../Lib/Notification/Assets/.LocalNotification/Script/NotificationBehaviour.cs) 참조

## INotificationListener

* 초기화 이후 호출되는 이벤트 핸들러
* 알림이 왔을 때 호출되는 이벤트 핸들러
  
* [INotificationListener.cs](../Lib/Notification/Assets/.LocalNotification/Script/INotificationListener.cs) 참조


# 주의사항

* 야간 알림 기능 사용 시, `SetNightTime()` 함수로 기준시간과 시각을 맞춰주어야 한다.