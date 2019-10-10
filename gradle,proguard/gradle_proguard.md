# [Gradle](https://developer.android.com/studio/build/)

* Player Setting에서 Custom Gradle Template를 체크하면 mainTemplate.gradle이라는 파일이 생긴다. 이것으로 APK를 빌드할 때 자신만의 build.gradle을 설정할 수 있다. 유니티 빌드 시 채울 수 있는 변수를 사용할 수 있다. 해당 변수는 [여기](https://docs.unity3d.com/Manual/android-gradle-overview.html)에서 확인 가능하다.

* buildscript {} : 모든 모듈에 공통되는 repositories와 dependencies를 정의하기 위해 사용하는 블록

* repositories {} : 의존성을 검색하거나 다운로드하는 데 사용하는 저장소를 구성하는 블록

* dependencies {} : 프로젝트를 빌드하기 위해 그래들이 사용해야하는 파일들을 적는 블록

* allprojects {} : 서드파티 플러그인이나 라이브러리와 같이 프로젝트의 모든 모듈에서 사용하는 reopositories와 dependencies를 구성하는 블록

* maven {} : 원격 저장소의 파일을 사용하고 싶을 때 해당 저장소의 url을 적는 블록. `mavenCentral()`은  기본 Maven의 저장소이다.

* ext {} : 모든 모듈에 공유하고 싶은 속성을 정의하는 블록. 변수 선언과 비슷하다.

* apply plugin: 'com.android.application' : 이 빌드에 Gradle 용 Android 플러그인을 적용하고 android {} 블록을 사용하여 Android 관련 빌드 옵션을 지정할 수 있도록 한다 ?

* android {} : 모든 Android 관련 설정을 구성하는 블록. compileSdkVersion, buildToolsVersion 과 같은 설정들이 들어간다.

* defaultConfig {} : 빌드 설정 및 항목을 캡슐화하고 main/AndroidManifest.xml의 일부 속성을 재정의할 수 있다. 서로 다른 버전의 앱을 사용할 때 이 값을 사용하도록 설정할 수 있다.

 multiDexEnabled : Android 앱(APK) 파일에는 DEX(Dalvik Executable) 파일 형식의 실행 가능 바이트코드 파일을 포함한다. Dalvik Executable 사양은 단일 DEX 파일 내에서 참조될 수 있는 메서드의 총 개수를 65,536으로 제한하며 여기에는 Android 프레임워크 메서드, 라이브러리 메서드, 코드에 있는 메서드가 포함된다. 앱과 이 앱이 참조하는 라이브러리의 총 참조 개수가 한계치를 넘을 때 해당 옵션으로 다중 DEX 파일을 빌드하고 읽을 수 있게 할 수 있다.

* dexOptions {} : 빌드 속도에 관련된 설정을 구성

 preDexLibraries : 증분 빌드가 더 빨라지도록 라이브러리 종속성을 pre-dex할지 여부를 선언 ???[링크](https://developer.android.com/studio/build/optimize-your-build?hl=ko#dex_options) 클린 빌드를 느리게 하기 때문에 CI(Continuous Integration) 서버에서 이 기능을 비활성화하는 것을 추천

 javaMaxHeapSize : DEX 컴파일러의 최대 힙 크기를 설정

* lintOptions {} : 앱을 빌드 할 때 Android Studio에 있는 Lint라는 코드 스캔 도구를 사용하는데, 검사 실행 또는 무시 등과 같은 Lint 옵션을 구성할 수 있는 블록

 abortOnError : true일 때, 에러가 나면 멈춘다.

* aaptOptions {} : Android Asset Packaging Tool(AAPT)의 특정 옵션을 설정하는 블록

 AAPT : 안드로이드 앱의 소스를 제외한 나머지 파일을 정리하여 apk로 패키징하는 안드로이드 SDK 내부 빌드 툴

 noCompress : 패키지에 포함할 때 압축하지 않을 파일들의 확장자를 설정. 쉼표로 구분하여 하나 이상 지정 가능. 

* buildTypes {} : 여러 빌드 유형을 구성할 수 있는 블록. 기본적으로 debug, release 두 가지 타입이 정의된다.

 minifyEnabled : 코드 스트립핑을 하고 싶을 때 true로 설정.

 useProguard : Proguard를 사용하고 싶을 때 true로 설정.

 proguardFiles : 해당 빌드에 적용할 proguard.txt들의 이름을 표기.

 jniDebuggable : 보안 Android 기기에서 앱을 디버깅할 수 있도록 일반 디버그 키스토어로 APK 서명을 구성한다 ??


# [Proguard](https://www.guardsquare.com/en/products/proguard/manual/usage)

* 코드를 축소시키고 난독화해주는 툴. 안드로이드 스튜디오에서 기본으로 제공.

* Player Setting에서 Minify 옵션에서 Proguard를 선택하면 빌드 시 코드를 제거할 수 있다. Player Setting에서 User Proguard File 옵션을 체크하면 커스텀 proguard.txt 파일을 생성할 수 있다.

* -keep : 유지할 클래스 및 클래스 멤버를 지정

* -keepclassmembers : 해당 클래스가 유지되는 경우 보존할 클래스 멤버를 지정

* -keepattributes : 유지할 속성(바이트 코드, 소스파일 이름, 행 번호 테이블 등을 포함하는)을 지정. 쉼표로 구분하여 하나 이상 지정 가능. 

 SourceFile : 원본 파일의 이름을 유지

 LineNumberTable : 메소드의 줄 번호를 유지

 Signature : 클래스, 필드 또는 메서드의 일반 서명을 지정. 컴파일러는 컴파일 된 라이브러리의 제네릭 형식을 사용하는 클래스를 제대로 컴파일하려면 이 정보가 필요할 수 있다.

* -dontwarn : 해결되지 않은 참조 및 기타 중요한 문제에 대해 경고하지 않도록 지정. 해당 이름과 일치하는 클래스에 대한 경고를 하지 않음.