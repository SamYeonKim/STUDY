- [Overview](#Overview)
- [사용방법](#%EC%82%AC%EC%9A%A9%EB%B0%A9%EB%B2%95)
  - [unity 에서 사용하는 방법](#unity-%EC%97%90%EC%84%9C-%EC%82%AC%EC%9A%A9%ED%95%98%EB%8A%94-%EB%B0%A9%EB%B2%95)
  - [Proguard 파일 작성 방법](#Proguard-%ED%8C%8C%EC%9D%BC-%EC%9E%91%EC%84%B1-%EB%B0%A9%EB%B2%95)
- [주의사항](#%EC%A3%BC%EC%9D%98%EC%82%AC%ED%95%AD)
- [참고](#%EC%B0%B8%EA%B3%A0)

-----

# Overview

* [What is Proguard?](https://www.guardsquare.com/en/products/proguard/manual/introduction)
  * `Java class file` 을 최적화, 난독화 하는 프로그램
  * `shrinking`, `optimizing`, `obfuscating`, `preverifying` 의 네 단계를 지원한다.

![](img/Proguard/intro.png)

1. Shrinking
   * `Java bytecode` 내부의 불필요한 클래스, 필드, 메소드들을 분석하고 제거한다.
  
   * 프로그램의 진행에 아무런 영향도 끼치지 않는다.
  
2. Optimizing
   * `bytecode`레벨에서 `제어 흐름 분석`, `데이터 흐름 분석`, `부분 평가`, `전역 값 넘버링`, `활성화 분석` 등의 기술을 사용하여 최적화한다.
  
   * 아래의 기능들을 행한다.
     * 상수 표현식 평가
     * 불필요한 필드 접근자와 메소드 호출 제거
     * 불필요한 브랜치 제거
     * 불필요한 비교와 테스트 케이스 제거
     * 사용하지 않는 블록 제거
     * 동격의 블록 머징
     * 변수 할당 축소
     * 쓰기전용 필드와 사용하지 않는 메소드 파라미터 제거
     * 상수 필드, 메소드 파라미터, 리턴 값 인라이닝
     * 짧거나 한번 불리는 메소드 인라이닝
     * `Tail Recursion` 단순화
     * 클래스와 인터페이스 머징
     * `private, static, final` 메소드 생성 (가능할때)
     * `static, final` 클래스 생성 (가능할때)
     * 인터페이스를 간단한 구현체로 변경
     * 200개 이상의 [Peephole Optimization](https://en.wikipedia.org/wiki/Peephole_optimization) 수행
     * 로깅 코드 부분적으로 제거
   * 상수 표현식을 루프 밖으로 빼내는 작업과 `escape analysis` 가 요구하는 최적화는 지원하지 않는다.
3. Obfuscating
   * 디버깅 정보를 제거하고 모든 코드의 이름들을 의미없는 문자로 변경시켜 리버스 엔지니어링이 힘들도록 만든다.
  
   * 덤으로 코드를 약간 간결하게 만든다.
   * 클래스 이름, 메소드 이름, 익셉션 트레이스에 사용되는 라인 넘버링을 제외하고는 구조적으로 동일하다.
4. Preverifying
   * 클래스 로더가 `bytecode` 를 로드할 때 수행하는 verifying 단계에 검증 정보를 추가하여 클래스 로더의 실제 검증을 단순화한다.
  
   * `proguard-android.txt` 파일에는 `-dontpreverifying` 옵션이 들어있다. (사용하지 않는다.)

# 사용방법

## unity 에서 사용하는 방법

* `Player Settings | Publishing Settings` 의 `User Proguard File` 을 체크한다.
  * 빌드 스크립트에 `EditorUserBuildSettings.androidDebugMinification = AndroidMinification.Proguard;` 코드를 추가해도 된다.
  * `Assets | Plugins | Android | proguard-user.txt` 를 사용한다.
  * 해당 파일이 없으면 빈 파일을 생성해준다.
* `proguard-user.txt` 을 작성한다.
* 만약 `Custom Gradle Template` 를 사용한다면, `Assets | Plugins | Android | mainTemplate.gradle` 파일에 아래의 코드가 추가되어있는지 확인한다.
  * `**`가 붙은 키워드는 유니티 빌드 프로세스에서 채워진다.

```groovy
buildTypes {
    debug {
        minifyEnabled **MINIFY_DEBUG**
        useProguard **PROGUARD_DEBUG**
        proguardFiles getDefaultProguardFile('proguard-android.txt'), 'proguard-unity.txt'**USER_PROGUARD**
        jniDebuggable true
    }
    release {
        minifyEnabled **MINIFY_RELEASE**
        useProguard **PROGUARD_RELEASE**
        proguardFiles getDefaultProguardFile('proguard-android.txt'), 'proguard-unity.txt'**USER_PROGUARD**
        **SIGNCONFIG**
    }
}
```

## Proguard 파일 작성 방법

* 아래의 테이블은 현재 프로젝트에서 사용하고 있거나 사용될 수 있는 키워드를 정리한 것이다.

* 키워드를 사용하여 `proguard-user.txt` 파일을 작성한다.
* 자세한 키워드 내용은 [manual](https://www.guardsquare.com/en/products/proguard/manual/usage) 참고

|  category  |   keyword   |      meaning       |
|:----------|:-----------|:------------------|
| Keep Options | -keep [,modifier,...] [class_specification](https://www.guardsquare.com/en/products/proguard/manual/usage#classspecification) |                      해당 클래스는 `entry point`로 취급되어 `shrinking`, `optimizing`, `obfuscating`, `preverifying`의 대상이 되지 않는다. [modifier](https://www.guardsquare.com/en/products/proguard/manual/usage#keepoptionmodifiers)를 사용하여 특정 단계만 진행할 수 있다. |
| | -keepclassmembers [,modifier,...] `class_specification` |                       해당 클래스의 멤버만 `proguard` 대상이 되지 않는다. 나머지 옵션은 `-keep` 옵션과 동일하다. |
| | -keepclasseswithmembers [,modifier,...] `class_specification` |                 해당 클래스가 명시된 특정 멤버들을 가지고 있을 때 `proguard` 대상이 되지 않는다. |
| |     -keepnames `class_specification`     |                                      `-keep,allowshrinking class_specification` 의 단축 키워드 |
| Shrinking options | -dontshrink | `shrinkking` 단계를 수행하지 않는다. |
| Optimization options | -dontoptimize | `optimizing` 단계를 수행하지 않는다.  |
| Obfuscation options | -dontobfuscate | `obfuscating` 단계를 수행하지 않는다. |
| | -dontusemixedcaseclassnames | 난독화 시 대소문자를 섞어서 사용하지 않는다. |
| | -keepattributes [attribute_filter] |                                            난독화 하지 않을 특성을 지정한다. 예를 들어, 스택 트레이싱을 하기 위해서는 `SourceFile`과 `LineNumberTable` 특성이 필요하다. 목록은 [attribute](https://www.guardsquare.com/en/products/proguard/manual/usage/attributes)를 참조한다. |
| Preverification options | -dontpreverify |                                                                `preverifying` 단계를 수행하지 않는다. |
| General options | -verbose |                                                                      익셉션 발생 시 전체 스택 정보를 출력하도록 정보를 구성한다. |
| | -dontwarn [class_filter] |                                                      해결되지 않은 참조나 기타 문제에 대해 경고하지 않도록 지정한다. |
| | -ignorewarnings | 전체 경고를 출력하지 않는다. |

* Example

  * [proguard-user.txt](/..//Assets/Plugins/Android/proguard-user.txt)

# 주의사항

* 코드 내 문자열 상수는 암호화되지 않는다. 
* 새로운 라이브러리가 추가될 때마다 해당 패키지에 `-keep` 옵션을 줘야하는지 검증해야 한다.

# 참고

* [GUARDSQUARE Proguard manual](https://www.guardsquare.com/en/products/proguard/manual)