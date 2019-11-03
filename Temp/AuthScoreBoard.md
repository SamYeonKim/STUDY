- [배경지식](#%EB%B0%B0%EA%B2%BD%EC%A7%80%EC%8B%9D)
  - [미리 읽어야할 문서들](#%EB%AF%B8%EB%A6%AC-%EC%9D%BD%EC%96%B4%EC%95%BC%ED%95%A0-%EB%AC%B8%EC%84%9C%EB%93%A4)
- [설정방법](#%EC%84%A4%EC%A0%95%EB%B0%A9%EB%B2%95)
  - [Android 업적, 리더보드 추가하기](#android-%EC%97%85%EC%A0%81-%EB%A6%AC%EB%8D%94%EB%B3%B4%EB%93%9C-%EC%B6%94%EA%B0%80%ED%95%98%EA%B8%B0)
  - [Android Resource Definition 설정하기](#android-resource-definition-%EC%84%A4%EC%A0%95%ED%95%98%EA%B8%B0)
  - [IOS 업적, 리더보드 추가하기](#ios-%EC%97%85%EC%A0%81-%EB%A6%AC%EB%8D%94%EB%B3%B4%EB%93%9C-%EC%B6%94%EA%B0%80%ED%95%98%EA%B8%B0)
- [사용방법](#%EC%82%AC%EC%9A%A9%EB%B0%A9%EB%B2%95)
  - [로그인 확인](#%EB%A1%9C%EA%B7%B8%EC%9D%B8-%ED%99%95%EC%9D%B8)
  - [업적 잠금해제하기](#%EC%97%85%EC%A0%81-%EC%9E%A0%EA%B8%88%ED%95%B4%EC%A0%9C%ED%95%98%EA%B8%B0)
  - [단계별 업적 잠금해제하기](#%EB%8B%A8%EA%B3%84%EB%B3%84-%EC%97%85%EC%A0%81-%EC%9E%A0%EA%B8%88%ED%95%B4%EC%A0%9C%ED%95%98%EA%B8%B0)
  - [리더보드 점수 갱신하기](#%EB%A6%AC%EB%8D%94%EB%B3%B4%EB%93%9C-%EC%A0%90%EC%88%98-%EA%B0%B1%EC%8B%A0%ED%95%98%EA%B8%B0)

-----

# 배경지식

## 미리 읽어야할 문서들

* [play-games-plugin-for-unity](https://github.com/playgameservices/play-games-plugin-for-unity)
  * google 에서 배포하는 google play game service unity plugin
* [Unity Docs](https://docs.unity3d.com/ScriptReference/Social.html)
  * 유니티 공식 스크립트 레퍼런스


# 설정방법

* [Android 개발자 콘솔](https://play.google.com/apps/publish), [Apple Appstore connect](https://appstoreconnect.apple.com/)에 앱이 등록되어 있어야 한다.

## Android 업적, 리더보드 추가하기

* `개발자 콘솔 | 게임 서비스 | 업적` 탭에 들어간다.

* `새 업적 추가`를 누른다.
  
![](img/AuthScoreBoard/google1.png)

* 필요한 정보를 기입하고 저장한다.
  * 단계별 업적을 등록하고 싶으면 토글을 선택한다.
  * 처음 숨겨진 업적을 등록하고 싶으면 초기상태를 변경한다.
  * 점수는 유저에게 x 100 의 경험치를 제공한다. 한 게임당 10만 경험치를 등록 가능.

![](img/AuthScoreBoard/google2.png)

* `개발자 콘솔 | 게임 서비스 | 리더보드` 탭에 들어간다.
* `새 리더보드 추가`를 누른 후 등록한다.
  
![](img/AuthScoreBoard/google3.png)

* `리소스 열기` 버튼을 눌러 리소스를 복사한다. 유니티에 붙여넣을 때 사용한다.
  * `업적` 탭의 `리소스 열기` 버튼과 기능은 동일하다.
  * 리소스는 업적과 리더보드를 모두 포함한다.
  
![](img/AuthScoreBoard/google4.png)

* `개발자 콘솔 | 게임 서비스 | 게임 출시` 탭에서 변경사항을 출시한다.

## Android Resource Definition 설정하기

* 유니티 에디터에서 `Windows | Google Play Games | Setup | Android setup...` 를 선택한다.
  
![](img/AuthGooglePlay/unity_menu.png)

* 개발자 콘솔에서 `리소스 받기`로 가져온 xml을 `Resources Definition`에 붙여넣는다.
  
![](img/AuthGooglePlay/unity_setting.png)

* setup을 누르고 resolver가 생성 / 수정될때까지 기다린다.
  * 에러가 나서 멈췄다면 `Assets | Play Services Resolver | Android Resolver | Force Resolver` 를 눌러 다시 생성한다.
  
![](img/AuthGooglePlay/force_resolver.png)

* `Constants class name`에 적혀있는 폴더/이름의 스크립트를 제거한다.
  * 이름을 바꿔서 사용할 수도 있다. 현재 플러그인에서는 제거하고 있다.

## IOS 업적, 리더보드 추가하기

* [Appstore Connect](https://appstoreconnect.apple.com/) 에 로그인한 뒤 `나의 앱`에 들어간다.

* `앱 내 추가 기능 | Game Center` 탭을 선택한다.

![](img/AuthScoreBoard/apple1.png)

* `목표 달성` 우측의 플러스 버튼을 누르고 정보를 기입한다.
  * `식별 정보`가 이름, `목표 달성 ID`가 코드 내에서 사용하는 id가 된다.
    * 안드로이드에서 자동으로 생성되는 ID를 사용하면 코드관리가 편하다.
    * 한번 등록하면 ID는 수정할 수 없다.
  * 언어에 적는 정보가 유저에게 실제로 보이게 된다.
  * 언어등록 시 이미지는 512x512 픽셀의 이미지여야 한다. (google 등록과 동일)

![](img/AuthScoreBoard/apple2.png)

![](img/AuthScoreBoard/apple3.png)

* `순위표` 우측의 플러스 버튼을 누르고 `개별 순위표` 버튼을 누른 후 정보를 기입한다.
  * `종합 순위표`는 순위표 점수의 합산을 보여준다.
  * `식별 정보`가 이름, `순위표 ID`가 코드 내에서 사용하는 id가 된다.
  * `최근 점수`는 현재 플러그인에 맞지 않는 방식이다.

![](img/AuthScoreBoard/apple4.png)

![](img/AuthScoreBoard/apple5.png)


# 사용방법

* [ScoreBoardAdapter.cs](../Lib/Auth/Assets/.Auth/ScoreBoard/Script/ScoreBoardAdapter.cs) 참고

## 로그인 확인

* 모든 기능의 진입점에 존재한다.
* 내부에서만 사용한다.
* 해당 플랫폼에서 제공하는 업적, 리더보드를 사용하기 위해서는 해당 플랫폼에 로그인이 되어있어야 한다.
  * Android - Google Play, Ios - Apple Game Center

## 업적 잠금해제하기

* `ScoreBoardAdapter`를 통해 호출된다.
* 숨겨진 업적을 드러내게만 하고싶을 때는 `b_clear`변수를 false로 세팅하면 된다.
* 미리 등록해 놓은 m_listener의 `OnScoreBoardUnlockAchievement()` 함수를 통해 결과를 확인할 수 있다.

## 단계별 업적 잠금해제하기

* `ScoreBoardAdapter`를 통해 호출된다.
* 현재 갱신하고 싶은 업적의 진행도가 `percentage` 변수보다 높을 경우 성공은 하지만 갱신은 되지 않는다.
* 미리 등록해 놓은 m_listener의 `OnScoreBoardUnlockAchievement()` 함수를 통해 결과를 확인할 수 있다.

## 리더보드 점수 갱신하기

* `ScoreBoardAdapter`를 통해 호출된다.
* 현재 리더보드에 등록되어 있는 점수가 `score` 변수보다 높을 경우 성공은 하지만 갱신은 되지 않는다.
* 미리 등록해 놓은 m_listener의 `OnScoreBoardUpdateLeaderBoard()` 함수를 통해 결과를 확인할 수 있다.