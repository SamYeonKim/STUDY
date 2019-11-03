- [Overview](#Overview)
- [사용방법](#%EC%82%AC%EC%9A%A9%EB%B0%A9%EB%B2%95)
  - [unity 에서 사용하는 방법](#unity-%EC%97%90%EC%84%9C-%EC%82%AC%EC%9A%A9%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95)
  - [gradle 템플릿 파일 작성 방법](#gradle-%ED%85%9C%ED%94%8C%EB%A6%BF-%ED%8C%8C%EC%9D%BC-%EC%9E%91%EC%84%B1-%EB%B0%A9%EB%B2%95)
- [참고](#%EC%B0%B8%EA%B3%A0)

-----

# Overview

* [What is Gradle?](https://docs.gradle.org/current/userguide/userguide.html)
  * 오픈소스 빌드 자동화 툴
  * `Groovy`, `Kotlin`으로 작성되어 있다.
  * `Android Studio`를 통해 쉽게 디버깅이 가능하다.
* 안드로이드 프로젝트의 경우 `build.gradle` 파일을 가지고 빌드를 진행한다.
  * 해당 파일을 변경하면 빌드 환경이 바뀐다.

# 사용방법

## unity 에서 사용하는 방법

* `Player Settings | Publishing Settings` 의 `User Proguard File` 을 체크한다.
  * 빌드 스크립트에 아래의 코드를 추가해도 된다. <pre>`EditorUserBuildSettings.androidDebugMinification = AndroidMinification.Proguard;`</pre>
  * `Assets | Plugins | Android | proguard-user.txt` 를 사용한다.
  * 해당 파일이 없으면 빈 파일을 생성해준다.
* `Player Settings | Publishing Settings | Build` 의 `Build System` 을 `Gradle` 로 변경한다.
  * 빌드 스크립트에 `EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;` 코드를 추가해도 된다.
  * 바로 `APK` 파일을 빌드하고 싶으면 `Build Settings | Android | Export Project` 를 체크한다. 빌드 스크립트에 아래의 코드를 추가해도 된다. <pre>`EditorUserBuildSettings.exportAsGoogleAndroidProject = false;`</pre>
* `Player Settings | Publishing Settings | Build` 의 `Custom Gradle Template` 을 체크한다.
  * `Assets | Plugins | Android | mainTemplate.gradle` 파일이 `build.gradle` 의 템플릿이 된다.
  * 해당 파일이 없으면 최소 조건만 포함된 템플릿이 생성된다.
* `mainTemplate.gradle` 을 작성한다.
  * `**`가 붙은 키워드는 유니티 빌드 프로세스에서 채워진다.

![](img/Gradle/setting.png)

## gradle 템플릿 파일 작성 방법

* `gradle` 파일은 `groovy`와 `kotlin` 문법을 사용한다.

* `Unity` 에서 사용하는 정보를 받아올 수 있다. 자세한 키워드 설명은 [메뉴얼](https://docs.unity3d.com/kr/2018.1/Manual/android-gradle-overview.html) 참고.
* `gradle build script`에서 가장 중요한 개념은 `Closure`이다.
  * 인수를 받고 값을 반환하며 변수를 할당할 수 있는 독립형 코드 블록
  * `상태를 가지고 있는 함수 객체`, 델리게이트와 비슷하다.
  * [참고 : Tricky Android](https://trickyandroid.com/gradle-tip-2-understanding-syntax/)
* 현재 `` 프로젝트에서 사용하고 있는 템플릿을 기준으로 설명한다.

1. [buildscript(Closure)](https://docs.gradle.org/current/dsl/org.gradle.api.Project.html#org.gradle.api.Project:buildscript(groovy.lang.Closure))
   * 현재 빌드 스크립트를 컴파일하고 실행하는 데 사용되는 클래스 경로를 선언할 수 있다.
  
   * [ScriptHandler](https://docs.gradle.org/current/javadoc/org/gradle/api/initialization/dsl/ScriptHandler.html) 인스턴스가 해당 `Closure` 를 실행한다.
   * 스크립트 클래스 경로 구성을 하려면 [dependencies](https://docs.gradle.org/current/dsl/org.gradle.api.Project.html#org.gradle.api.Project:dependencies(groovy.lang.Closure)) `Closure` 를 사용하여 종속성을 `classpath` 구성에 연결해야 한다.
   * 외부 종속성의 경우 [repositories](https://docs.gradle.org/current/dsl/org.gradle.api.Project.html#org.gradle.api.Project:repositories(groovy.lang.Closure)) `Closure` 를 사용하여 저장소를 선언해야 한다.
  
2. [dependencies(Closure)](https://docs.gradle.org/current/dsl/org.gradle.api.Project.html#org.gradle.api.Project:dependencies(groovy.lang.Closure))
   * 현재 프로젝트의 종속성을 구성한다.
  
   * [DependencyHandler](https://docs.gradle.org/current/dsl/org.gradle.api.artifacts.dsl.DependencyHandler.html) 인스턴스가 해당 `Closure` 를 실행한다.
   * [Configuration](https://docs.gradle.org/current/javadoc/org/gradle/api/artifacts/Configuration.html) 의 그룹들로 구성된다.
     * `Configuration` 은 [Artifacts](https://docs.gradle.org/current/javadoc/org/gradle/api/artifacts/package-summary.html) 들과 그들의 종속성들을 그룹화한 것을 나타낸다. 
       * 일종의 설정값들이다.
       * [ConfigurationContainer](https://docs.gradle.org/current/javadoc/org/gradle/api/artifacts/ConfigurationContainer.html) 가 관리한다.
     * `<configurationName> <dependencyNotation1>` 의 포맷을 따른다.
       * `<dependencyNotation1>`은 `group:name:version` 의 포맷을 따른다.
  
3. [repositories(Closure)](https://docs.gradle.org/current/dsl/org.gradle.api.Project.html#org.gradle.api.Project:repositories(groovy.lang.Closure))
   * 라이브러리를 다운로드 받을 수 있는 저장소를 구성한다.
  
   * [RepositoryHandler](https://docs.gradle.org/current/dsl/org.gradle.api.artifacts.dsl.RepositoryHandler.html) 인스턴스가 해당 `Closure` 를 실행한다.
   * `flatDir(Closure)` : 로컬 디렉토리를 저장소로 등록한다.

4. [allprojects(Closure)](https://docs.gradle.org/current/dsl/org.gradle.api.Project.html#org.gradle.api.Project:allprojects(groovy.lang.Closure))
   * 현재 프로젝트의 모든 모듈이 공유할 설정을 구성한다.
  
   * [Project](https://docs.gradle.org/current/dsl/org.gradle.api.Project.html) 인스턴스가 해당 `Closure` 를 실행한다.
   * `buildscript` 는 `gradle` 이 빌드를 수행할 수 있는 방법의 변경이고, `allprojects` 는 `gradle` 에 의해 구축되는 모듈을 대상으로 하는 설정이다.
     * [참고](https://stackoverflow.com/questions/30158971/whats-the-difference-between-buildscript-and-allprojects-in-build-gradle)
  
5. `apply plugin : 'com.android.application'`
   * `'com.android.application'` 플러그인을 사용하도록 선언한다.

6. `android(Closure)`
   * `'com.android.application'` 에 구현된 구성을 쓰기 위해 선언하는 블록.
   
   * `Project` 오브젝트를 확장한다.
   * `Closure` 내부에서 설정값을 적용하려면 미리 정의된 `Properties` 를 사용해야 한다. 

7. [packagingOptions(Closure)](https://google.github.io/android-gradle-dsl/current/com.android.build.gradle.internal.dsl.PackagingOptions.html)
   * `APK` 패키징 시 포함하거나 포함하지 않을 파일을 지정할 수 있다.

8. [defaultConfig(Closure)](https://google.github.io/android-gradle-dsl/current/com.android.build.gradle.AppExtension.html#com.android.build.gradle.AppExtension:defaultConfig)
   * 빌드에 적용될 기본 구성값들을 설정한다.
   
   * 해당 블록은 [ProductFlavor](https://google.github.io/android-gradle-dsl/current/com.android.build.gradle.internal.dsl.ProductFlavor.html) 클래스에 속해있다. 해당 링크에서 사용 가능한 프로퍼티들을 확인할 수 있다.
   
9.  [lintOptions(Closure)](https://google.github.io/android-gradle-dsl/current/com.android.build.gradle.internal.dsl.LintOptions.html)
    * `Lint` 를 사용할 때 적용할 수 있는 옵션들을 구성할 수 있다.
  
      * `Lint` : 코드의 구조적 문제를 식별하고 수정하는 코드 스캔 도구
      * `Android Studio` 를 사용할 경우 앱을 빌드할 때마다 자동으로 검사를 실행한다.
    * [Lint로 코드 개선](https://developer.android.com/studio/write/lint.html) 참조
    
10. [aaptOptions(Closure)](https://google.github.io/android-gradle-dsl/current/com.android.build.gradle.internal.dsl.AaptOptions.html)
    * `AAPT(Android Asset Packaging Tool)` 를 사용할 때 적용할 수 있는 옵션들을 구성할 수 있다.
  
      * `AAPT` : `APK` 를 만들 때 모든 클래스와 리소스를 묶어주는 `Android SDK` 의 기본 툴
      * 패키징할 때 기본적으로 압축이 들어간다.
    * [참고](https://stackoverflow.com/questions/28234671/what-is-aapt-android-asset-packaging-tool-and-how-does-it-work)
    
11. [buildTypes(Closure)](https://google.github.io/android-gradle-dsl/current/com.android.build.gradle.AppExtension.html#com.android.build.gradle.AppExtension:buildTypes)
    * 현재 프로젝트에 대한 모든 빌드 타입 구성을 캡슐화한다.
  
    * 기본적으로 `debug`, `release` 두 타입이 정의되어 있다.
    * [빌드 유형 구성](https://developer.android.com/studio/build/build-variants?hl=ko#build-types) 참고


* Example

  * [mainTemplate.gradle](/..//Assets/Plugins/Android/mainTemplate.gradle)

# 참고

* [Gradle Build Language Reference](https://docs.gradle.org/current/dsl/index.html)

* [Android 빌드 구성](https://developer.android.com/studio/build)
* [Android 종속성 구성](https://developer.android.com/studio/build/dependencies)
* [Android App Extension](https://google.github.io/android-gradle-dsl/current/com.android.build.gradle.AppExtension.html)
* [Tricky Android](https://trickyandroid.com/gradle-tip-2-understanding-syntax/)