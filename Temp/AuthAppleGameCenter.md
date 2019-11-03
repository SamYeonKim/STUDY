- [배경지식](#%EB%B0%B0%EA%B2%BD%EC%A7%80%EC%8B%9D)
  - [미리 읽어야할 문서들](#%EB%AF%B8%EB%A6%AC-%EC%9D%BD%EC%96%B4%EC%95%BC%ED%95%A0-%EB%AC%B8%EC%84%9C%EB%93%A4)
  - [미리 알아두면 좋은 내용들](#%EB%AF%B8%EB%A6%AC-%EC%95%8C%EC%95%84%EB%91%90%EB%A9%B4-%EC%A2%8B%EC%9D%80-%EB%82%B4%EC%9A%A9%EB%93%A4)
- [설정방법](#%EC%84%A4%EC%A0%95%EB%B0%A9%EB%B2%95)
  - [Developer Console 설정하기](#Developer-Console-%EC%84%A4%EC%A0%95%ED%95%98%EA%B8%B0)
- [사용방법](#%EC%82%AC%EC%9A%A9%EB%B0%A9%EB%B2%95)
  - [로그인하기](#%EB%A1%9C%EA%B7%B8%EC%9D%B8%ED%95%98%EA%B8%B0)
  - [로그아웃하기](#%EB%A1%9C%EA%B7%B8%EC%95%84%EC%9B%83%ED%95%98%EA%B8%B0)
- [주의 사항](#%EC%A3%BC%EC%9D%98-%EC%82%AC%ED%95%AD)

-----

# 배경지식

## 미리 읽어야할 문서들

* [Generates signature](https://developer.apple.com/documentation/gamekit/gklocalplayer/1515407-generateidentityverificationsign?language=objc)
  * 서버가 유저인증을 위해 검증해야 하는 토큰을 생성하는 함수 설명
* [What is X.509](https://proneer.tistory.com/entry/%E3%85%8C%EC%95%84%EB%9F%AC)
  * X.509가 무엇인지 설명
* [What is salt](https://d2.naver.com/helloworld/318732)
  * salt가 무엇인지 설명

## 미리 알아두면 좋은 내용들

* [구현 예시](https://stackoverflow.com/a/32692503)
  1. Send the publicKeyURL, signature, salt, and timestamp parameters to the third party server used for authentication.
  2. Use the publicKeyURL on the third party server to download the public key.
  3. Verify with the appropriate signing authority that the public key is signed by Apple.
  4. Retrieve the player’s playerID and bundleID.
  5. Concatenate into a data buffer the following information, in the order listed
       * The playerID parameter in UTF-8 format
       * The bundleID parameter in UTF-8 format
       * The timestamp parameter in Big-Endian UInt-64 format
       * The salt parameter
  6. Generate a SHA-256 hash value for the buffer.
  7. Using the public key downloaded in step 3, verify that the hash value generated in step 7 matches the signature parameter provided by the API.


# 설정방법

## Developer Console 설정하기

* [Developer Console](https://developer.apple.com/account/ios/certificate/)에서 Provisioning Profile을 제작한다.
  1. Certification을 추가한다.
   
![](img/AuthApple/console.png)
    
     * 키체인에서 CSR 파일을 생성해서 등록할 수 있다.
  
![](img/AuthApple/keychain.png)

  2. 테스트 기기를 등록한다.
  3. AppID를 등록한다.

![](img/AuthApple/console_id.png)

  4. Provisioning Profile 을 생성한다.
   
![](img/AuthApple/console_profile.png)

* 유니티에서 앱을 빌드한다. (XCodeProj 필요)
* [Appstore Connect](https://appstoreconnect.apple.com/)에 회사 계정으로 로그인한다.

![](img/AuthApple/appstore_connect.png)

* 신규 앱을 생성한다.
  * SKU는 고유한 앱 아이디이다. 아무거나 등록하면 됨
  
![](img/AuthApple/appstore_register.png)

* 앱의 XcodeProject를 켜서 `Product | Archive`를 실행한다.
  
![](img/AuthApple/menu_archive.png)
![](img/AuthApple/archive.png)

* 앱을 업로드하고 Appstore Connect에 버전이 올라갔는지 확인한다.
* 등록된 앱 클릭 -> 제출 준비 중 탭 클릭 후 Game Center 항목을 켜준다.

![](img/AuthApple/gamecenter.png)


# 사용방법

* `IPlatformService`을 상속하는 `AGCAdapter`는 플러그인 코드가 존재하기 때문에 싱글톤 패턴을 사용한다.

* [AGCAdapter.cs](../Lib/Auth/Assets/.Auth/Script/PlatformServiceAdapter/AGCAdapter.cs) 참고 

## 로그인하기

* `AuthAdapter`를 통해 호출된다.
* 로그인 시 상단의 UI를 띄우고 싶지 않으면 `b_silent`를 `true`로 넘겨준다.
* 인증 성공 시 NativeCode로 제작한 `AppleGameCenterPlaform_ReqVerifyData` 함수를 통해 idToken을 가져온다.
* NativeCode는 코드 수행 시 성공하면 `OnSuccessFromNative`를, 실패하면 `OnErrorFromNative`를 호출한다.
* `OnSuccessFromNative` 호출 시 `AppleGameCenterPlaform_GetKey` 함수를 호출하여 idToken을 Json 형태로 생성하고, `AGCAdapter.inst.m_token_json`에 저장한다.
*  미리 등록해 놓은 m_listener의 `OnAuthLogin` 함수를 호출한다.
* `OnAuthLogin`에서 게임 서버로의 통신을 진행해야 한다.

## 로그아웃하기

* GameCenter는 로그아웃이 없다.


# 주의 사항

* `AGCAdapter`에 저장되는 idToken `m_token_json`은 Json형태로 저장되어 있다. `AuthAdapter.GetIdToken()` 함수를 통해 가져온 뒤 `Dictionary<string, string>` 형태로 컨버팅해서 사용할 수 있다.