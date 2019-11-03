- [배경지식](#%EB%B0%B0%EA%B2%BD%EC%A7%80%EC%8B%9D)
  - [미리 읽어야할 문서들](#%EB%AF%B8%EB%A6%AC-%EC%9D%BD%EC%96%B4%EC%95%BC%ED%95%A0-%EB%AC%B8%EC%84%9C%EB%93%A4)
  - [미리 알아두면 좋은 내용들](#%EB%AF%B8%EB%A6%AC-%EC%95%8C%EC%95%84%EB%91%90%EB%A9%B4-%EC%A2%8B%EC%9D%80-%EB%82%B4%EC%9A%A9%EB%93%A4)
- [설정방법](#%EC%84%A4%EC%A0%95%EB%B0%A9%EB%B2%95)
  - [개발자 콘솔 설정하기](#%EA%B0%9C%EB%B0%9C%EC%9E%90-%EC%BD%98%EC%86%94-%EC%84%A4%EC%A0%95%ED%95%98%EA%B8%B0)
  - [안드로이드 추가 설정](#%EC%95%88%EB%93%9C%EB%A1%9C%EC%9D%B4%EB%93%9C-%EC%B6%94%EA%B0%80-%EC%84%A4%EC%A0%95)
  - [iOS 추가 설정](#iOS-%EC%B6%94%EA%B0%80-%EC%84%A4%EC%A0%95)
  - [clientid 설정하기](#clientid-%EC%84%A4%EC%A0%95%ED%95%98%EA%B8%B0)
- [사용방법](#%EC%82%AC%EC%9A%A9%EB%B0%A9%EB%B2%95)
  - [로그인하기](#%EB%A1%9C%EA%B7%B8%EC%9D%B8%ED%95%98%EA%B8%B0)
  - [로그아웃하기](#%EB%A1%9C%EA%B7%B8%EC%95%84%EC%9B%83%ED%95%98%EA%B8%B0)
- [주의사항](#%EC%A3%BC%EC%9D%98%EC%82%AC%ED%95%AD)

-----

# 배경지식

## 미리 읽어야할 문서들

* [Facebook SDK for Unity](https://developers.facebook.com/docs/unity)
  * 페이스북 SDK 유니티 패키지 공식 문서

## 미리 알아두면 좋은 내용들

* https://medium.com/@byn9826/verify-facebook-login-by-python-e02ac1e23e37
* https://developers.facebook.com/docs/facebook-loginmanually-build-a-login-flow#checktoken


# 설정방법

## 개발자 콘솔 설정하기

* [개발자 콘솔](https://developers.facebook.com/apps/)에서 애플리케이션 만들기를 진행한다.

* `설정 | 기본 설정` 에서 앱 이름을 등록한다.
* `설정 | 고급 설정` 에서 `네이티브 앱 또는 데스크톱 앱인가요?` 옵션을 활성화한다.
  
![](img/AuthFacebook/console1.png)

## 안드로이드 추가 설정

* `설정 | 기본 설정` 하단부의 `플랫폼 추가` 버튼을 클릭 후 안드로이드 아이콘을 선택한다.
  
![](img/AuthFacebook/console2.png)
![](img/AuthFacebook/console3.png)

* 패키지이름과 클래스 이름, 키 해시를 입력한다. 키 해시는 유니티 에디터의 `Facebook | Edit Settings` 에 적혀있다.
  
![](img/AuthFacebook/client3.png)

* 로그인 시 아래와 같은 에러가 발생하면 에러에 적힌 키 해시를 콘솔에 붙여넣는다.
  
![](img/AuthFacebook/client4.png)

## iOS 추가 설정

* `설정 | 기본 설정` 하단부의 `플랫폼 추가` 버튼을 클릭 후 사과 아이콘을 선택한다.
  
![](img/AuthFacebook/console3.png)

* 유니티 번들 아이디를 입력 후 저장한다.
  
![](img/AuthFacebook/console4.png)

* 유니티 에디터의 `PlayerSettings`에서 `Target minimum iOS Version`을 `8.0`으로 변경한다.
  
![](img/AuthFacebook/console5.png)

## clientid 설정하기

* 유니티 에디터에서 `Facebook | Edit Settings` 를 선택한다.
 
![](img/AuthFacebook/client1.png)

* `App Name`과 `App Id`에 등록한 앱의 정보를 기입한다.
  
![](img/AuthFacebook/client2.png)


# 사용방법

* [FacebookAdapter.cs](../Lib/Auth/Assets/.Auth/Script/PlatformServiceAdapter/FacebookAdapter.cs) 참고 

## 로그인하기

* AuthAdapter를 통해 호출된다.
* 로그인 시 상단의 UI를 띄우고 싶지 않으면 `b_silent`를 `true`로 넘겨준다.
* 인증 성공 시 미리 등록해 놓은 m_listener의 `OnAuthLogin` 함수를 호출한다.
* `OnAuthLogin`에서 게임 서버로의 통신을 진행해야 한다.

## 로그아웃하기

* AuthAdapter를 통해 호출된다.
* `AuthAdapter.Logout` 에서 이벤트 핸들러를 호출한다.
  * 멀티 링크 계정의 로그아웃 때문에 분리


# 주의사항

* 빌드 시 아래와 같은 에러가 난다면  `Assets | Play Services Resolver | Android Resolver | Resolver` 를 정상적으로 수행하도록 한다.
* `error: resource style/Theme.AppCompat.NoActionBar (aka com.nadagames.twatauth:style/Theme.AppCompat.NoActionBar) not found.`
  * `Google Play Service`의 `Resolver`와 버전이 달라서 발생하는 문제이다.
  * 둘 중 최신의 `Resolver`를 사용하자. 보통 `Google Play Service`의 버전이 더 높다.

![](img/AuthFacebook/resolver.png)

* `Resolver` 중간에 에러가 나면 에디터를 리부팅하고 위의 절차를 다시 진행한다.