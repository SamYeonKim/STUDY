# IOS GUIDE

## 앱 동작

### 필요한 리소스 제공

+ 앱과 상호작용하는데 필요한 메타데이터인 Info.plist 제공.
+ 앱 사용 또는 장치에 필요한 필수 기능에 대한 선언 가능.
+ 홈 화면에 사용되는 하나 이상의 아이콘을 설정.
+ 앱이 실행되는 동안 잠시 실행되는 이미지 설정.

#### 앱 번들

+ 실행 파일과 리소스의 그룹.

| File | Example | Description |
| ------ | ------- | ----- |
| App executable | MyApp | 컴파일 코드를 포함하는 실행파일.|
| The information property list file | Info.plist | 앱의 구성정보를 포함한다. |
| App icons| *.png, *@2x.png | 장치의 메인화면에 사용되는 아이콘.|
| Launch images | *.png | 앱이 실행되는 동안 잠시 백그라운드 배경으로 사용.|
| Storyboard files | *.storyboard | 스크린에 앱을 나타내기 위해 view와 view controller를 포함한다. |
| Ad hoc distribution icon | iTunesArtwork | ad hoc으로 배포된 앱은 App Store를 통과하지 못하기 때문에 앱 번들 대신에 아이콘(512 x 512 픽셀 버전)이 있어야 한다.|
| Settings bundle | *.bundle | 환경설정을 공개하려는 경우 Settings bundle이 필요하다. | 
| Nonlocalized resource files | *.png | 앱에서 사용하는 이미지들, 사운드 파일, 동영상, custom data들이 포함된다. |
| Subdirectories for localized resources | *.lproj | Localized resources는 언어별 지정된 프로젝트 경로에 놓여야하고, ISO 639-1 언어로 구성된 이름은 “.lproj”를 붙여줘야 한다.|

#### Info.plist (information property-list file)

+ 앱을 운용하기 위한 메타데이터를 저장.
+ 프로젝트를 생성할때 기본적으로 생성.
+ 앱 구성의 중요한 요소를 포함.

##### 앱 동작을 위한 최소 CPU레벨 설정

+ UIRequiredDeviceCapabilities Key로 설정.
+ 기본값이 설정되어있고, 추가적으로 설정 및 수정이 가능.

##### 네트워크를 통해 서버와 의사소통을 하고 싶을 경우 설정
+ UIRequiresPersistentWiFi Key로 설정.

##### 가판대에 해당 앱을 전시하고 싶을 경을 설정
+ UINewsstandApp Key로 설정.

##### 특정 URL을 설정하여 앱 실행 설정
+ Info/URL Types의 URL Schemes에 설정으로 앱을 실행 시킬 수 있다.

```
ex)

URL Schemes가 abc일 경우
"abc://"로 연결하면 해당 앱이 열리는 것을 확인 할 수 있다. 
```

##### 홈 아이콘

+ 앱의 홈 스크린 또는 앱 스토어에 나오는 이미지.
+ 상황에 따른 다른 아이콘을 사용 할 수 있다.

##### 실행 아이콘

+ 앱 실행시 나오는 이미지.

### 사용자 개인 정보 보호 지원

+ IOS FrameWork에서 인증 요청 제공.

| Data or resources | Keys | Authorization APIs |
| ------ | ------- | ----- |
| Calendar data | NSCalendarsUsageDescription | Calendar에 접근하기 위한 인증 상태 체크|
| Camera | NSCameraUsageDescription | Camera에 접근하기 위한 인증 상태 체크|
| Photos | NSPhotoLibraryUsageDescription | 사진첩에 접근하기 위한 인증 상태 체크|
|Reminders | NSRemindersUsageDescription | reminder 데이터에 접근하기 위한 인증 상태 체크|


### 앱 국제화

+ 언어별 리소스파일을 제작한다.

#### 스토리보드 (.storyboard)

+ 텍스트 라벨 및 다양한 컨텐츠를 포함하고 있으므로 지역화가 필요하다.

#### 문자열 파일 (.string)

+ 지역화 된 버전이 포함되어 있습니다.

#### 이미지 파일 (.jpg, .png)

+ 이미지 파일에 텍스트를 함께 저장하지 말아야 한다.
+ 텍스트는 .string으로 따로 사용하도록 한다.

#### 비디오 또는 음악 파일

+ 이미지 파일과 마찬가지고 해석이 담겨있는 텍스트를 포함하여 함께 저장하지 말아야 한다.
+ 텍스트는 .string으로 따로 사용하도록 한다.