- [개요](#%ea%b0%9c%ec%9a%94)
- [설정방법](#%ec%84%a4%ec%a0%95%eb%b0%a9%eb%b2%95)
  - [앱id, 인증서, 프로비저닝 파일 생성](#%ec%95%b1id-%ec%9d%b8%ec%a6%9d%ec%84%9c-%ed%94%84%eb%a1%9c%eb%b9%84%ec%a0%80%eb%8b%9d-%ed%8c%8c%ec%9d%bc-%ec%83%9d%ec%84%b1)
    - [AppID 생성](#appid-%ec%83%9d%ec%84%b1)
    - [인증서 생성](#%ec%9d%b8%ec%a6%9d%ec%84%9c-%ec%83%9d%ec%84%b1)
    - [프로비저닝 파일 생성](#%ed%94%84%eb%a1%9c%eb%b9%84%ec%a0%80%eb%8b%9d-%ed%8c%8c%ec%9d%bc-%ec%83%9d%ec%84%b1)
  - [app store connect 앱 설정](#app-store-connect-%ec%95%b1-%ec%84%a4%ec%a0%95)
    - [앱 업로드](#%ec%95%b1-%ec%97%85%eb%a1%9c%eb%93%9c)
    - [Xcode 아카이브를 이용한 업로드](#xcode-%ec%95%84%ec%b9%b4%ec%9d%b4%eb%b8%8c%eb%a5%bc-%ec%9d%b4%ec%9a%a9%ed%95%9c-%ec%97%85%eb%a1%9c%eb%93%9c)
    - [Application Loader를 이용한 업로드](#application-loader%eb%a5%bc-%ec%9d%b4%ec%9a%a9%ed%95%9c-%ec%97%85%eb%a1%9c%eb%93%9c)
- [참고](#%ec%b0%b8%ea%b3%a0)
  - [CSR(Certificate Signing Request) 파일 만들기](#csrcertificate-signing-request-%ed%8c%8c%ec%9d%bc-%eb%a7%8c%eb%93%a4%ea%b8%b0)

----
  
# 개요

* appstore에 앱을 업로드 하기 위한 일련의 과정들을 설명한 문서
* 앱을 만들어 테스트하기 위해서는 인증서와 프로비저닝 파일이 필요하고 [애플 개발자 홈페이지](https://developer.apple.com/)에서 생성할 수 있다. 
* 앱스토어 커넥트에서 앱을 하나 생성한 후 앱을 업로드, 스토어에서 사용할 메타정보들을 입력한다. 
* 앱을 업로드할때는 xcode를 이용해 바로 app store connect에 앱을 업로드하거나 ipa 파일을 뽑아 application loader를 통해 업로드한다.

# 설정방법

## 앱id, 인증서, 프로비저닝 파일 생성

* 프로비저닝 프로파일 : 기기에서 앱을 실행하고 서비스를 사용하고자 할 때 사용하는 파일
* iOS앱은 코드 서명을 해야 iOS 기기에서 실행 가능하고, 코드 서명은 Apple에서 발급한 인증서로 진행되어야 한다.
* Apple의 인증서가 아닌 Apple이 발급한 `개발자 인증서`로 코드 서명한 앱을 기기에 설치할 때는 프로비저닝 프로파일이 반드시 필요하다.

### AppID 생성

* AppID를 생성하기 위해 [애플 개발자 홈페이지](https://developer.apple.com/)에 접속한다.
* 상단의 `Account` 메뉴를 클릭해 아래 이미지에 보이는 `Certificates, Identifiers & Profiles` 항목을 선택한다.

* 왼쪽의 `Identifiers` 항목을 선택 후 `+`버튼을 눌러 AppId를 생성한다.
  
![](img/AppstoreRelease/appid0.png)

* `App IDs`를 선택한 후 다음으로 넘어간다.

![](img/AppstoreRelease/appid1.png)

* 생성할 앱에 맞게 항목들을 채운다.
   * `플랫폼`을 선택, `이름`을 정의한다.
   * `Bundle ID`는 연결될 프로젝트의 `Bundle Identifier`와 같아야한다.
     * `Bundle ID`는 AppID 생성 이후 수정이 불가능하므로 다르다면 프로젝트의 `Bundle Identifier`를 변경한다.
   * `Capabilities`는 이용할 수 있는 서비스이고 앱에서 사용할 서비스를 선택한다.
     * `Capabilities`는 AppID 생성 이후 수정할 수 있다.

![](img/AppstoreRelease/appid2.png)
![](img/AppstoreRelease/appid3.png)

  
### 인증서 생성

* 인증서를 생성하기 위해 [애플 개발자 홈페이지](https://developer.apple.com/)에 접속한다.
* 상단의 `Account` 메뉴를 클릭해 아래 이미지에 보이는 `Certificates, Identifiers & Profiles` 항목을 선택한다.

![](img/AppstoreRelease/certification1.png)

* 왼쪽의 `Certificates` 항목을 선택 후 `+`버튼을 눌러 인증서를 생성한다.

![](img/AppstoreRelease/certification2.png)

* 생성할 인증서 종류와 인증서가 사용할 서비스들을 선택하고 다음으로 넘어간다.

![](img/AppstoreRelease/certification3.png)
![](img/AppstoreRelease/certification4.png)

* CSR(Certificate Signing Request) 파일을 만들어 등록한다.
  * 인증기관에 인증서를 요청하기 위해 CSR파일을 만든다.
  * [CSR 파일 만들기](#CSRCertificate-Signing-Request-%ED%8C%8C%EC%9D%BC-%EB%A7%8C%EB%93%A4%EA%B8%B0)

![](img/AppstoreRelease/certification5.png)

* 생성된 인증서를 다운받은 후 키체인에 등록한다.

![](img/AppstoreRelease/certification6.png)

### 프로비저닝 파일 생성

* 프로비저닝 파일을 생성하기 위해 [애플 개발자 홈페이지](https://developer.apple.com/)에 접속한다.
* 상단의 `Account` 메뉴를 클릭해 아래 이미지에 보이는 `Certificates, Identifiers & Profiles` 항목을 선택한다.

![](img/AppstoreRelease/certification1.png)

* 왼쪽의 `Profiles` 항목을 선택 후 `+`버튼을 눌러 프로비저닝 파일을 생성한다.

![](img/AppstoreRelease/provision0.png)

* 개발이나 배포등 프로비저닝 파일을 사용할 목적을 선택한다.

![](img/AppstoreRelease/provision1.png)

* 프로비저닝 파일에 등록할 AppID를 선택한다.

![](img/AppstoreRelease/provision2.png)

* 프로비저닝 파일에 등록할 인증서를 선택한다.

![](img/AppstoreRelease/provision3.png)

* 프로비저닝 파일에 등록할 기기를 선택한다.

![](img/AppstoreRelease/provision4.png)

* 프로비저닝 파일 이름을 설정한다.

![](img/AppstoreRelease/provision5.png)

* 위에서 설정한 모든 항목(AppID, 인증서, 기기, 파일 이름)은 프로비저닝 파일 생성 이후에도 수정 가능하다.

## app store connect 앱 설정

* [앱스토어 커넥트](https://appstoreconnect.apple.com/)에 접속한다.
* `나의 앱`을 선택한 뒤 `+`버튼을 눌러 앱을 생성한다.

![](img/AppstoreRelease/앱생성0.png)

* 생성할 앱과 관련된 정보들을 기입한다.
  * `SKU` : 앱스토어에 노출되지 않는 앱의 고유한 ID(시리얼 넘버와 비슷)

![](img/AppstoreRelease/앱생성1.png)

### 앱 업로드

* 애플에서 제공하는 서비스(ex) 인앱구매)를 테스트할 때에는 앱이 하나는 업로드되어 있어야 한다.
* 앱을 업로드하는 방법에는 2가지가 있다.
  * Xcode 아카이브를 이용한 업로드
  * Application Loader를 이용한 `.ipa` 업로드

### Xcode 아카이브를 이용한 업로드

* 버전번호와 빌드번호를 입력한다.
  * 빌드번호만 올려 업로드가 가능하다.
  * 버전번호가 다르면 빌드번호는 같아도 상관없다.
    * ex) 1.0.1(0)과 1.0.2(0)은 빌드번호는 같지만 영향을 주지 않는다.
  
* 메뉴바의 `Product` -> `Archive` 를 선택한다.

![](img/AppstoreRelease/archive0.png)

* 빌드가 완료되면 오른쪽의 `Distribute App`을 선택한다.

![](img/AppstoreRelease/archive1.png)

* 앱을 업로드하거나 업로드용 `.ipa`파일을 만들기 위해 `iOS App Store` 를 선택하고 다음으로 넘어간다.

![](img/AppstoreRelease/archive2.png)

* 앱을 업로드 할 때는 `Upload`, 업로드용 `.ipa`파일을 만들때는 `Export`를 선택하고 다음으로 넘어간다.
  * `Application Loader`를 사용해 업로드용 `.ipa`파일을 `앱스토어 커넥트`에 올릴 수 있다.

![](img/AppstoreRelease/archive3.png)

* 해당화면에서는 상황에 맞게 선택 후 다음으로 넘어간다.

![](img/AppstoreRelease/archive4.png)

* 앱을 업로드하거나 업로드용 파일을 만들때는 `distribution` 인증서와 프로비저닝 파일이 필요하다.
  * 빌드시에 `development` 인증서를 사용했다면 `Manually manage signing(수동 사이닝)`을 선택 후 다음으로 넘어간다.

![](img/AppstoreRelease/archive5.png)

* `Manually manage signing`을 선택했다면 `distribution`인증서와 프로비저닝 파일을 선택한다.
  * 만약, 다운로드 받지 않았다면 [애플 개발자 사이트](https://developer.apple.com/)에서 다운로드 받는다.

![](img/AppstoreRelease/archive6.png)

* 업로드하기 위해선 인증서의 개인키 비밀번호도 필요하다.

![](img/AppstoreRelease/archive7.png)

* `Upload` 버튼을 눌러 `앱스토어 커넥트`에 앱을 업로드한다.
  * 업로드용 `.ipa`파일을 생성하는 것이라면 `Export`버튼을 눌러 앱을 생성한다.

![](img/AppstoreRelease/archive8.png)

![](img/AppstoreRelease/archive10.png)

### Application Loader를 이용한 업로드

* `Application Loader`를 이용해 앱을 업로드하기 위해 `.ipa`파일이 필요하다.
* `Xcode`의 `Archive`을 이용해 `.ipa`파일을 만들 수 있고 [Xcode 아카이브를 이용한 업로드](#Xcode-%EC%95%84%EC%B9%B4%EC%9D%B4%EB%B8%8C%EB%A5%BC-%EC%9D%B4%EC%9A%A9%ED%95%9C-%EC%97%85%EB%A1%9C%EB%93%9C)와 유사한 과정을 수행한다.
* 앱을 생성했다면 `Application Loader`를 실행시켜 앱을 선택한다.

![](img/AppstoreRelease/loader0.png)

* `다음`버튼을 눌러 앱을 업로드한다.

![](img/AppstoreRelease/loader1.png)

* 업로드가 완료되면 해당 화면을 볼 수 있다.

![](img/AppstoreRelease/loader2.png)

* `아카이브를 이용한 업로드`와는 다르게 앱이 `앱스토어 커넥트`에 완전히 업로드될 때까지 시간이 걸린다. (5분 정도)

![](img/AppstoreRelease/loader3.png)

# 참고

## CSR(Certificate Signing Request) 파일 만들기
  
* `키체인 접근` -> `인증서 지원` -> `인증 기관에서 인증서 요청` 항목을 선택한다.

![](img/AppstoreRelease/인증서요청1.png)

* 애플 개발자 계정, 이름을 입력한다.
* CA Email Address는 비워두고 디스크에 저장한다.

![](img/AppstoreRelease/인증서요청2.png)
