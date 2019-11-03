- [개요](#%EA%B0%9C%EC%9A%94)
- [설정방법](#%EC%84%A4%EC%A0%95%EB%B0%A9%EB%B2%95)
  - [개발자 콘솔 설정하기](#%EA%B0%9C%EB%B0%9C%EC%9E%90-%EC%BD%98%EC%86%94-%EC%84%A4%EC%A0%95%ED%95%98%EA%B8%B0)
  - [clientid 설정하기](#clientid-%EC%84%A4%EC%A0%95%ED%95%98%EA%B8%B0)
- [주의사항](#%EC%A3%BC%EC%9D%98%EC%82%AC%ED%95%AD)

-----

# 개요

* `Google Play Store` 에 앱을 등록하고 출시하기까지의 과정을 설명한다.

# 설정방법

* [AuthGooglePlayGameService.md](AuthGooglePlayGameService.md) 에서의 설정 + @ 를 수행한다.

## 개발자 콘솔 설정하기

* [개발자 콘솔](https://play.google.com/apps/publish)에서 애플리케이션 만들기를 진행한다.

![](img/PlaystoreRelease/console1.png)

* 기본 언어 설정, 제목 작성 후 `만들기` 버튼을 누른다.

![](img/PlaystoreRelease/console2.png)

* 아래의 스크린샷에 나오는 회색 토글 버튼이 모두 만족해야 출시가 가능하다.

![](img/PlaystoreRelease/console3.png)

* `스토어 등록정보` 에서 별표시가 되어있는 정보를 기입한다.
  * 이미지 등록 시 크기가 정확하게 맞아야 하고, 다른 앱이 등록한 이미지를 사용할 시 앱이 거부된다.

  * 콘텐츠 등급은 앱을 등록하고 난 뒤에 설정이 가능하다.
  * 프로덕션 출시를 할 경우 `개인정보처리방침`은 꼭 작성해 주어야 한다. 작성하지 않으면 앱이 내려간다.
  
* `앱 버전 | 알파` 탭의 관리에 들어가 `새 버전 출시하기` 버튼을 누른다.
  * `테스터 참여 대상 관리` 와 `알파 트랙 이용 가능 국가` 도 설정해 놓는다.
  
  * 여기의 테스터는 링크의 초대를 받아 마켓에서 알파 버전을 내려받아 테스트할 수 있는 사람을 의미한다.
  
![](img/PlaystoreRelease/console4.png)

* `계속` 버튼을 누른다.
  * `Google에서 앱 서명 키를 관리 및 보호` 옵션은 마켓 출시 시 필요한 서명 키를 Google이 대신 관리해주는 서비스이다.
  
  * 개발자는 APK 또는 AAB 등록 시 사용한 업로드 키만을 신경쓰면 된다. (달라진게 없다.)
  * 아래에 `게임 서비스` 에 등록할 `OAuth 사용자 인증 정보` 등록 시, 기본으로 `앱 서명 키` 의 인증 정보가 등록된다. 마켓에서 받지 않고 직접 빌드한 apk를 사용하여 `Google API` 를 테스트할 때 정상 작동하게 하려면 `업로드 키` 인증 정보도 생성해야 한다.

![](img/PlaystoreRelease/console5.png)

* APK 또는 AAB를 등록하고 출시명과 출시 노트를 작성한다.
  * 프로덕션 출시를 목표로 한다면 `APK` 대신 `Android App Bundle` 을 등록해야 한다.
  
  * 프로덕션 출시를 목표로 한다면 앱을 빌드할 때 64비트를 지원해야 한다.
    * 유니티의 경우 `Player Settings | Other Settings | Script Backend` 의 `IL2CPP` 옵션을 켠 후 `Target Architectures | ARM64` 항목을 체크해야 한다.
  * 테스트용 앱이라면 하단의 `검토` 버튼을 누르면 안된다. 출시되면 앱을 제거하지 못한다.
  
![](img/PlaystoreRelease/console6.png)

* `가격 및 배포` 탭에 들어가 배포 방법과 이용 가능한 국가를 설정한다.
  * 실제 마켓에 등록되는 국가 리스트이다.
  
* `콘텐츠 등급` 탭에 들어가 설문지를 작성한다.
  * 개발자가 임의로 하면 안된다. 꼭 디렉터와 함께 진행해야 한다.
  
![](img/PlaystoreRelease/console7.png)

* `App content` 탭에 들어가 타겟층 및 콘텐츠에 대한 정보를 작성한다.
  
![](img/PlaystoreRelease/console8.png)
  
* 모든 정보 작성이 끝나면 `앱 버전 | 트랙 관리` 에 들어가 `검토` 버튼을 누른다.
  
![](img/PlaystoreRelease/console9.png)

* 개발자 콘솔 메인 화면으로 나간 뒤 `게임 서비스 | 새 게임 추가` 버튼을 누른다.
  
![](img/PlaystoreRelease/console10.png)

* `Google API를 아직 사용하지 않습니다.` 탭에서 정보를 기입한 후 `계속` 버튼을 누른다.
  
![](img/PlaystoreRelease/console11.png)

* 애플리케이션에 작성한 `스토어 등록정보` 와 동일하게 `게임 세부정보` 를 작성한다.
  
* `연결된 앱` 탭에서 `Android` 버튼을 클릭하여 이전에 만든 애플리케이션과 연결한다.
  * `턴 방식 멀티플레이어`, `실시간 멀티플레이어`, `불법 복제 방지 사용` 은 `사용 안함` 으로 남겨놓는다.
  
![](img/PlaystoreRelease/console12.png)

* 2단계에서 `지금 앱 승인` 버튼을 누른 후 `Android OAuth 클라이언트 만들기` 창이 뜨면 `확인` 버튼을 누른다.
  
  * 여기서 생성되는 클라이언트 ID는 `앱 서명 키` 의 인증서 지문을 사용한다. 만약 스토어에 올라가지 않은 APK를 테스트할 경우, `업로드 키` 의 클라이언트 ID를 따로 등록해줘야 `Google API` 사용이 가능하다.
  
![](img/PlaystoreRelease/console13.png)

![](img/PlaystoreRelease/console14.png)

* `게임 세부정보` 탭 하단의 `API 콘솔 프로젝트` 에 들어간다.
* `사용자 인증 정보` 탭에 들어가 `사용자 인증 정보 만들기 | OAuth 클라이언트 ID` 버튼을 누른다.
* `웹 애플리케이션` 으로 생성한다.
  * 정보를 기입하지 않아도 바로 생성이 가능하다.
* 필요하다면 `업로드 키` 의 클라이언트 ID를 생성한다.
  * APK 등록 시 사용한 키스토어의 SHA-1 지문 값을 등록해야 한다.

* `테스트` 탭에 `Google API` 를 사용할 테스트 계정을 등록하고 `게임 출시` 탭으로 가서 게임을 출시한다.

![](img/PlaystoreRelease/console15.png)

![](img/PlaystoreRelease/console16.png)

* 게임이 출시된 후 충분한 내부테스트를 진행한다.
  * 해당 출시 APK는 하단부의 `clientid 설정하기` 를 완료한 앱이어야 한다.
  
![](img/PlaystoreRelease/console17.png)

* `애플리케이션 | 앱 정보 | 스토어 등록정보` 탭으로 진입하여 메타데이터를 마켓에 표시될 정보로 변경한다.
  * 앱 등록 시 해당 작업을 진행했다면 건너뛰어도 된다.
  
  * `개인정보처리방침` 은 꼭 작성해야 한다.

![](img/PlaystoreRelease/console18.png)

* 나머지 `애플리케이션 | 앱 정보` 들의 항목에 작성된 데이터를 확인한다.
  
* `출시 관리 | 앱 버전 | 버전 수정` 버튼을 눌러 제품 출시 페이지로 들어간다.
* 모든 설정이 완료되었으면 `프로덕션 트랙으로 출시` 버튼을 누른다.

![](img/PlaystoreRelease/console19.png)

* `알파 테스트` 등록과 동일하게 AAB 등록, 출시명 변경, 출시 노트 변경 후 `검토` 버튼을 누른다.
  
* `검토` 에 성공하여 출시가 되면 마켓에 정상 등록되었는지 확인한다.
  * `애플리케이션 | 앱 정보 | 가격 및 배포` 에 등록된 국가의 마켓에서만 접근이 가능하다.

## clientid 설정하기

* `resolver` 에서 자동으로 불필요한 .aar 파일을 만들어 주는 일을 막기 위해 `Assets | Play Services Resolver | Android Resolver | Settings` 를 아래 스크린샷과 같이 만든다.
  
![](img/PlaystoreRelease/resolver.png)

* 유니티 에디터에서 `Windows | Google Play Games | Setup | Android setup...` 를 선택한다.
  
![](img/AuthGooglePlay/unity_menu.png)

* `게임 서비스 | 업적 또는 리더보드 | 리소스 받기`로 가져온 xml을 `Resources Definition`에 붙여넣는다.
  * 업적 또는 리더보드를 등록하지 않은 경우 아래의 스크린샷에서 `<!-- achievement ac1 -->` 부분을 제거하고 나머지 정보를 해당 앱에 맞게 수정하여 진행할 수 있다.
* `API 콘솔 프로젝트`에서 만든 웹 애플리케이션 사용자 인증 정보의 클라이언트 ID를 Client ID에 붙여넣는다.
  * Constants class name에 적은 이름이 static class로 만들어진다. achievement의 고유 아이디를 string 변수로 가지는 클래스 이기 때문에 플러그인에 포함하지 않는다.
  
![](img/AuthGooglePlay/unity_setting.png)

* setup을 누르고 resolver가 생성 / 수정될때까지 기다린다.
  * 에러가 나서 멈췄다면 `Assets | Play Services Resolver | Android Resolver | Force Resolver` 를 눌러 다시 생성한다.
  
![](img/AuthGooglePlay/force_resolver.png)


# 주의사항

* 유니티 빌드할 때 `Script Debugging` 옵션이 켜져있으면 APK가 안올라간다.
  * 업데이트 할 때, 다른 서명이라고 나온다.
  * `Google에서 앱 서명 키를 관리 및 보호` 옵션을 적용하면 구글이 `앱 서명 키`를 따로 만들기 때문. `업로드 키`는 테스트용, `앱 서명 키`는 배포용으로 나누어져 있다.
  * 혹시 옵션이 꺼져있어도 안된다면 `Development Build`를 꺼보자.