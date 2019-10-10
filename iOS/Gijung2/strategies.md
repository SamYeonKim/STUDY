# Strategies for Implementing Specific App Features

## 개인 정보 보호 전략

+ 시스템 프레임 워크는 연락처와 같은 데이터를 관리하기 위해 개인 정보 제어 기능을 제공하지만 로컬에서 사용하는 데이터를 보호하기위해서는 조치를 취해야한다.

### On-Disk 암호화를 사용한 보호

+ 파일을 암호화된 형식으로 저장하고, 필요할때 복호화를 한다.
+ 새로 파일을 작성할때는 NSData 클래스의 writeToFile:options:error: 메소드를 사용.
+ 기존에 존재하던 파일을 위해서는 NSFileManager 클래스의 setAttributes:ofItemAtPath:error: 메소드를 사용하여 NSFileProtectionKey를 설정.

```
* 파일 보호 레벨

1. No protection
: 파일은 암호화되지만 암호로 보호되지 않으며 장치가 잠길 때 사용할 수 있다.

2. Complete
: 장치가 잠겨있는 동안 파일이 암호화되어 접근을 할 수 없다.

3. Complete unless already open
: 장치가 잠겨있는 동안 암호화된 파일에 접근을 할 수 없다.
: 파일을 열었을때 잠김 설정이 된다면 계속해서 접근이 가능하다.

4. Complete until first login
: 파일은 장치가 부팅되고 사용자가 한 번 잠금을 해제 할 때까지 암호화되고 액세스 할 수 없다.
```

```
* 보호된 상태 변경 방법

1. applicationProtectedDataWillBecomeUnavailable: 또는 applicationProtectedDataDidBecomeAvailable: 메소드를 사용.

2. UIApplicationProtectedDataWillBecomeUnavailable 또는 UIApplicationProtectedDataDidBecomeAvailable 알림 등록.

3. 파일 접근을 위한 UIApplication 오브젝트의 the protectedDataAvailable 프로퍼티 값 변경.
```

### 고유 사용자 식별

#### 사용자를 특정 계정에 연결하려고 할때

+ 유저가 안전하게 연결 할 수 있도록 로그인 화면을 준비한다.
+ 수집 한 유저의 정보는 암호화된 형태로 저장한다.

#### 다른기기에서 실행중일때 다른 인스턴스를 원할때

+ 사용자의 ID를 획득하기 위해서 UIDevice 클래스의 identifierForVendor 프로퍼티 사용.

#### 광고의 목적으로 사용자를 식별하려고 할때

+ 사용자의 ID를 획득하기 위해서 ASIdentifierManager 클래스의 advertisingIdentifier 프로퍼티 사용.

## 제한 사항 설정

+ 소비하고자 원하는 미디어의 등급을 지정하는 제한 사항 설정을 할 수 있다.
+ 제한 사항을 설정했다면 설정이 변경될때 처리를 해줘야 한다.

| Media rating key | Value |
| ------ | ------- |
|com.apple.content-rating.ExplicitBooksAllowed|Boolean, No로 설정 할 경우 예약을 제공하지 않는다.|
|com.apple.content-rating.ExplicitMusicPodcastsAllowed|Boolean, No로 설정 할 경우 음악, 영화등을 허락하지 않는다.|
|com.apple.content-rating.AppRating|NSNumber, 0~1000 범위의 키, 등급이 현재 키 값보다 높은 앱은 허용되지 않습니다.|
|com.apple.content-rating.MovieRating|NSNumber, 0~1000 범위의 키, 등급이 현재 키 값보다 높은 영화는 허용되지 않습니다.|
|com.apple.content-rating.TVShowRating|NSNumber, 0~1000 범위의 키, 등급이 현재 키 값보다 높은 TV 프로그램은 허용되지 않습니다.|

## 다양한 버전의 IOS 제공

+ 오래된 버전의 IOS에서 앱을 사용하기 위해서는 충돌 검사를 위해 Runetime Check이 필요하다.
+ 아래와 같은 예외처리를 만들 수 있다.

```
ex) 1. 클래스가 존재하는지 판단하기 위해 null 체크

if ([UIPrintInteractionController class]) {
   // Create an instance of the class and use it.
}
else {
   // The print interaction controller is not available so use an alternative technique.
}
```

```
ex) 2. 클래스에서 사용가능한 메소드가 있는지 판단하기 위해 instancesRespondToSelector,respondsToSelector의 메소드 사용.
```

```
ex) 3. c 기반의 함수를 사용 가능한지 판단

if (UIGraphicsBeginPDFPage != NULL) {
    UIGraphicsBeginPDFPage();
}
```

## 출시 전반에 걸쳐 앱의 시각적 모양 유지

+ Delegate 객체를 이용해 최상의 상태를 관리.
+ View Controller 객체를 이용해 인터페이스 전체르 관리.
+ Custom View를 이용해 Custom Data를 관리.

### 보존 및 복구 상태 활성화

+ application:shouldSaveApplicationState 또는
application:shouldRestoreApplicationState Deleagte 사용.

### 보존 및 복구 상태 실행을 위한 체크 리스트

+ (필수) application:shouldSaveApplicationState: 와 application:shouldRestoreApplicationState: 함수 실행.
+ (필수) restorationIdentifier property 할당.
+ (필수) 스크롤의 위치 또는 앱과 연관된 비트들을 저장하기 위해 application:willFinishLaunchingWithOptions: 함수 이용.
+ View Controller에 저장 클래스 할당.
+ (추천) View와 View Controller의 상태를 Ecode/Decode 하기위해 encodeRestorableStateWithCoder와 decodeRestorableStateWithCoder 함수 사용.
+ 버전 정보 또는 추가적인 상태 정보를 Encode/Decode 하기 위해서는 application:willEncodeRestorableStateWithCoder와 application:didDecodeRestorableStateWithCoder 함수 사용.
+ 테이블 뷰 및 컬렉션 뷰의 데이터 소스 역할을하는 오브젝트를 사용하려면 UIDataSourceModelAssociation 프로토콜을 구현해야한다.


