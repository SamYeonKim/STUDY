- [배경지식](#%EB%B0%B0%EA%B2%BD%EC%A7%80%EC%8B%9D)
  - [미리 읽어야할 문서들](#%EB%AF%B8%EB%A6%AC-%EC%9D%BD%EC%96%B4%EC%95%BC%ED%95%A0-%EB%AC%B8%EC%84%9C%EB%93%A4)
  - [미리 알아두면 좋은 내용들](#%EB%AF%B8%EB%A6%AC-%EC%95%8C%EC%95%84%EB%91%90%EB%A9%B4-%EC%A2%8B%EC%9D%80-%EB%82%B4%EC%9A%A9%EB%93%A4)
- [설정방법](#%EC%84%A4%EC%A0%95%EB%B0%A9%EB%B2%95)
  - [개발자 콘솔 설정하기](#%EA%B0%9C%EB%B0%9C%EC%9E%90-%EC%BD%98%EC%86%94-%EC%84%A4%EC%A0%95%ED%95%98%EA%B8%B0)
  - [clientid 설정하기](#clientid-%EC%84%A4%EC%A0%95%ED%95%98%EA%B8%B0)
- [사용방법](#%EC%82%AC%EC%9A%A9%EB%B0%A9%EB%B2%95)
  - [로그인하기](#%EB%A1%9C%EA%B7%B8%EC%9D%B8%ED%95%98%EA%B8%B0)
  - [로그아웃하기](#%EB%A1%9C%EA%B7%B8%EC%95%84%EC%9B%83%ED%95%98%EA%B8%B0)
- [주의사항](#%EC%A3%BC%EC%9D%98%EC%82%AC%ED%95%AD)

-----

# 배경지식

## 미리 읽어야할 문서들

* [play-games-plugin-for-unity](https://github.com/playgameservices/play-games-plugin-for-unity)
  * google 에서 배포하는 google play game service unity plugin
* [Authenticate with a backend server](https://developers.google.com/identity/sign-in/android/backend-auth)
  * google play game service 인증을 위한 backend server
* [Google Sign-In for server-side apps](https://developers.google.com/identity/sign-in/web/server-side-flow)
  * google sign-in server side flow

## 미리 알아두면 좋은 내용들

* https://www.youtube.com/watch?time_continue=8&v=j_31hJtWjlw
  * 클라는 구글에 로그인하여 id token 을 얻는다.
  * 클라는 서버로 id token 을 전송한다.
  * 서버는 구글과 통신하여 id token 을 해독한다.
  * 유저를 식별하기 위한 키를 획득한다.


# 설정방법

## 개발자 콘솔 설정하기

* [개발자 콘솔](https://play.google.com/apps/publish)에서 애플리케이션 만들기를 진행한다.  
  * 애플리케이션은 `출시 준비`까지만 진행하면 된다. 출시하면 나중에 제거하지 못한다.
  * 게임서비스를 출시하면 구글 API를 사용할 수 있다.

* 필요한 모든 정보를 기입한다.
  * 콘텐츠 등급은 앱을 등록하고 난 뒤에 설정이 가능하다.
* 알파테스트에 앱을 등록하고 필요한 테스터를 추가한다.
  
![](img/AuthGooglePlay/alpha_track.png)

* 개발자 콘솔 게임서비스 탭에서 새 게임 추가를 누른다.
* 필요한 정보를 등록하고 연결된 앱 탭을 누른 뒤 만든 앱을 등록한다.
* 아무 업적이나 1개 생성한다.
  * 유니티 설정에 필요한 `리소스 xml`을 알 수 있으면 만들지 않아도 된다.
  
![](img/AuthGooglePlay/achievement.png)

  * 유니티 설정 시 `리소스 받기`를 눌러 `xml`을 복사 붙여넣기 해야한다.
  
![](img/AuthGooglePlay/achievement_resource.png)

* 테스트 탭에서 테스트 계정을 추가한다.
  
![](img/AuthGooglePlay/tester.png)

* 게임 세부정보 탭 하단의 API 콘솔 프로젝트로 들어간다.
* 사용자 인증 정보에 등록되어 있는 OAuth 2.0 클라이언트 ID를 눌러 들어간다.
  * 없으면 사용자 인증 정보 만들기로 생성한다.
* 서명 인증서 지문을 모든 애플리케이션 -> 출시 관리 -> 앱 서명 -> 업로드 인증서 -> SHA-1 인증서 지문으로 교체한다.
  
![](img/AuthGooglePlay/signning.png)
![](img/AuthGooglePlay/android_client.png)

  * 테스트가 아닌 실제 빌드에서는 앱 서명 인증서를 사용한다.
* 사용자 인증 정보 만들기로 웹 애플리케이션 클라이언트 ID를 생성한다.
  
![](img/AuthGooglePlay/web_client.png)

  * 아무것도 적지 않아도 저장이 된다.
* 개발자 콘솔로 돌아가서 게임 서비스 -> 게임 출시 를 한다.
  
![](img/AuthGooglePlay/product.png)

## clientid 설정하기

* 유니티 에디터에서 `Windows | Google Play Games | Setup | Android setup...` 를 선택한다.
  
![](img/AuthGooglePlay/unity_menu.png)

* 개발자 콘솔에서 `리소스 받기`로 가져온 xml을 `Resources Definition`에 붙여넣는다.
* API 콘솔 프로젝트에서 만든 웹 애플리케이션 사용자 인증 정보의 클라이언트 ID를 Client ID에 붙여넣는다.
  * Constants class name에 적은 이름은 static class로 만들어진다. achievement의 고유 아이디를 string 변수로 가지는 클래스 이기 때문에 플러그인에 포함되지 않는다.
  
![](img/AuthGooglePlay/unity_setting.png)

* setup을 누르고 resolver가 생성 / 수정될때까지 기다린다.
  * 에러가 나서 멈췄다면 `Assets | Play Services Resolver | Android Resolver | Force Resolver` 를 눌러 다시 생성한다.
  
![](img/AuthGooglePlay/force_resolver.png)


# 사용방법

* [GPGSAdapter.cs](../Lib/Auth/Assets/.Auth/Script/PlatformServiceAdapter/GPGSAdapter.cs) 참고 

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

* 유니티 빌드할 때 `Script Debugging` 옵션이 켜져있으면 APK가 안올라간다.
  * 업데이트 할 때, 다른 서명이라고 나온다.
  * 구글이 `앱 서명`용 키를 따로 만들기 때문. 업로드용 키는 테스트용, 앱 서명용 키는 배포용으로 나누어져 있다.
  * 혹시 옵션이 꺼져있어도 안된다면 `Development Build`를 꺼보자.
* 구글의 정책 변경으로 빌드를 할때 `Target Architectures`의 `ARM64`를 꼭 켜줘야한다. 근데 정상적으로 리졸빙하면 `Plugins/com.google.games.gpgs-plugin-support-0.9.62.aar` 내부의 `jni` 폴더 아래에 `arm64-v8a`폴더가 사라진다.
  * 해결방법은 미리 `aar`파일에 `arm64-v8a`폴더를 넣어놓는다.
  * 위치는 `GooglePlayGames/Editor/m2repository/com/google/games/gpgs-plugin-support/0.9.62/gpgs-plugin-support-0.9.62.srcaar` 파일 안에 있다.
  * 현재 `unity resolver`의 버전이 높아져서(`1.2.116`) 정상작동을 한다. 위의 정보는 무시해도됨