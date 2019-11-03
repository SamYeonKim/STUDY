- [Overview](#overview)
- [Unit Test with Visual Studio](#unit-test-with-visual-studio)
  - [NUnit](#nunit)
    - [attributes](#attributes)
    - [Assertion](#assertion)
  - [사용법 with Unity](#%EC%82%AC%EC%9A%A9%EB%B2%95-with-unity)
- [Unit Test with Unity Test Runner](#unit-test-with-unity-test-runner)
  - [사용법](#%EC%82%AC%EC%9A%A9%EB%B2%95)
- [Coding Convention](#coding-convention)
- [주의사항](#%EC%A3%BC%EC%9D%98%EC%82%AC%ED%95%AD)
- [참고](#%EC%B0%B8%EA%B3%A0)

-----

# Overview

* Unit Test란?
  * 프로그램에 포함되어 있는 기능들 중 하나만 떼어내서 해당 기능이 정상적으로 동작하는지 확인하는 과정
  * 모든 함수와 메소드에 대한 테스트 케이스를 작성하는 절차
* Unit Test 실행 도구
  * Visual Studio의 `단위 테스트 프로젝트`를 생성한다.
  * Unity Test Runner 를 사용한다.

# Unit Test with Visual Studio

* 공식적으로 가이드가 작성되어 있는 Unit Test 프레임워크는 `MSTest`, `NUnit`, `xUnit` 3가지 이다.
* `NUnit`을 사용한다.
  * [비교 자료](https://stackoverflow.com/questions/38063903/vs-2013-mstest-vs-nunit-vs-xunit)

## NUnit

* `.NET` 언어에서 사용가능한 오픈소스 유닛 테스팅 프레임워크.
* `attribute` 를 사용하여 테스트 함수 구현
* 자세한 정보는 [공식 문서](https://github.com/nunit/docs)를 참조

### attributes

|     attribute    |                                       meaning                                       |
|:----------------:|:-----------------------------------------------------------------------------------:|
|       Test       | 테스트용 메소드 선언                                                                |
|    TestFixture   | 테스트용 클래스 선언                                                                |
|       Setup      | 테스트 케이스의 실행 전 호출된다.                                                      |
|     TearDown     | 테스트 케이스의 실행 후 호출된다.                                                      |
|   OneTimeSetup   | 모든 테스트 케이스의 실행 전 1번 호출된다.                                             |
|  OneTimeTearDown | 모든 테스트 케이스의 실행 후 1번 호출된다.                                             |
| Ignore("reason") | 테스트 실행 시 해당 테스트 케이스를 제외한다.                                          |
|     Property     | 테스트 케이스에 `name/value`의 메타데이터 제공한다.                                    |
|      Theory      | `DatapointSource` 또는 `Datapoint` attribute에서 데이터를 읽어 테스트 케이스를 진행한다.|
|   Category("")   | 테스트 케이스나 클래스에 라벨을 붙인다.                                              |
|   Repeat()   | 테스트 케이스를 여러번 반복할 수 있다.                                          |
|   Value(...)  | 파라미터에 사용하여 특정한 값을 주면서 테스트 할 수 있다.                                |
|   Random(min, max, repeat)  | 파라미터에 사용하여 랜덤한 값을 주면서 테스트 할 수 있다.                  |
|   Timeout(ms)  | 해당 테스트 케이스에 시간제한을 줄 수 있다.                  |

### Assertion

|   Function  |                                                        usage                                                        |
|:-----------:|:-------------------------------------------------------------------------------------------------------------------:|
|  동등 비교 - 값  | Assert.AreEqual(a, b), Assert.AreNotEqual(a, b) |
|  동등 비교 - 참조 | Assert.AreSame(a, b), Assert.AreNotSame(a, b) |
|   참 거짓   | Assert.IsTrue(a), Assert.IsFalse(a)                                                                                 |
|     null    | Assert.IsNull(a), Assert.IsNotNull(a)                                                                               |
|  타입 비교  | Assert.IsInstanceOf(a, b), Assert.IsNotInstanceOf(a, b)                                                             |
| 문자열 비교 | StringAssert.AreEqualIgnoringCase(a, b)                                                                             |
| 문자열 포함 | StringAssert.Contains(a, b), StringAssert.DoesNotContain(a, b), StringAssert.Contains(a, b)                         |
|    정규식   | StringAssert.IsMatch(a, @"정규식"), StringAssert.DoesNotMatch(a, @"정규식")                                         |
|    Exception   | Assert.Throws<Exception>(System.Action())                                         |

## 사용법 with Unity

* 유니티 프로젝트를 Visual Studio로 실행한다.
* 솔루션 우클릭 후 `추가 | 새 프로젝트`를 클릭한다.
  ![](img/UnitTest/nunit1.png)
* `Visual C# | 테스트` 탭에서 `단위 테스트 프로젝트`를 생성한다.
  * 유니티 프로젝트에 포함되지 않도록 생성한다.
  ![](img/UnitTest/nunit2.png)
* `도구 | NUGet 패키지 관리자 | 솔루션용 NuGet 패키지 관리` 에 들어간다.
  ![](img/UnitTest/nunit3.png)
* `NUnit` 패키지를 다운받는다.
  ![](img/UnitTest/nunit4.png)
* `참조 | 참조 추가` 를 클릭해서 테스트 하고싶은 유니티 프로젝트를 추가한다.
  ![](img/UnitTest/nunit5.png)
  ![](img/UnitTest/nunit6.png)
* 테스트 스크립트에 `using NUnit.Framework` 를 추가한다.
* 테스트 케이스를 작성한다.
* `테스트 | 실행 | 모든 테스트` 버튼으로 테스트를 실행할 수 있다.
  * 왼쪽에 `테스트 탐색기` 창이 활성화된다.
  ![](img/UnitTest/nunit7.png)
  ![](img/UnitTest/nunit8.png)

* Example
```cs
using System;
using NUnit.Framework;
using .IAP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace .IAP.Test {
    public class IAPAdapterTest {
        [Test][OneTimeSetUp]
        public void Open() {
            Assert.Throws<ArgumentNullException>(() => IAPAdapter.Open(null));
            Assert.AreEqual(IAPAdapter.m_store_type, UnityEngine.Purchasing.AppStore.NotSpecified);
            Console.Write("IAPAdapter.m_store_type : " + IAPAdapter.m_store_type);
        }

        [Test]
        public void RefreshProduccts() {
            Assert.Throws<ArgumentNullException>(() => IAPAdapter.RefreshProducts(null));
            List<ProductInfo> l_info = new List<ProductInfo>();
            Assert.Throws<NullReferenceException>(() => IAPAdapter.RefreshProducts(l_info));
        }

        [Test]
        public void Purchase() {
            Assert.Throws<ArgumentException>(() => IAPAdapter.Purchase(null, null));
            Assert.Throws<ArgumentException>(() => IAPAdapter.Purchase("", ""));
            Assert.Throws<ArgumentException>(() => IAPAdapter.Purchase("product_id", null));
            Assert.Throws<NullReferenceException>(() => IAPAdapter.Purchase("product_id", "account_id"));
        }

        [Test]
        public void Restore() {
            Assert.Throws<NullReferenceException>(() => IAPAdapter.Restore()); 
        }
    }
```

# Unit Test with Unity Test Runner

* `NUnit` 프레임워크를 사용한다.
* `Monobehaviour`를 상속받은 클래스의 사용이 가능하다.
* `PlayMode`는 빌드 시 포함되기 때문에 사용하지 않는다.

## 사용법

* `Window | Test Runner` 를 클릭한다.
* 
  ![](img/UnitTest/runner1.png)
* `EditMode`의 `Create EditMode test`를 사용하여 스크립트를 생성한다.
  * 수동으로 생성해도 된다. 이 경우 `using NUnit.Framework`를 추가해야 한다.
  * `using UnityEngine.TestTools`는 `PlayMode`에서 코루틴을 테스트하기 위해 사용된다. 지워도 상관없음.
  ![](img/UnitTest/runner2.png)
* 생성된 `Editor` 폴더를 `Host | Test` 아래로 옮긴다.
* 테스트 케이스를 작성한다.
* `Run All` 버튼을 이용하여 테스트 전체를 실행할 수 있다.
  ![](img/UnitTest/runner3.png)

# Coding Convention

* `namespace`는 테스트 대상이 되는 클래스의 네임스페이스에 `.test`를 붙인다.
```cs
namespace .IAP.Test
```
* 클래스 이름은 테스트 대상 클래스의 뒤에 `Test` 를 붙인다.
```cs
public class AuthAdapterTest
```
* 테스트 케이스 이름은 테스트 대상 메소드의 이름을 그대로 사용한다. 테스트케이스의 대상이 메소드가 아닐 경우 해당 케이스를 잘 표현해주는 이름을 사용한다.
```cs
[Test]
public void Open() {
    Assert.Throws<ArgumentNullException>(() => AuthAdapter.Open(null, PlatformServiceType.GooglePlay));
}
```

# 주의사항

* 기본적으로 Visual Studio의 테스트 도구를 사용한다.
* `Monobehaviour`를 사용하는 테스트의 경우, VS 테스트로는 불가능하다.
  * 오류 : `Message: System.Security.SecurityException : ECall 메서드는 시스템 모듈에 패키지되어 있어야 합니다.`
* 부득이하게 `Monobehaviour`를 사용해야 할 경우 Unity Test Runner의 `Edit Mode`를 사용하여 추가로 테스트 케이스를 생성한다.
* Unit Test에서 `Monobehaviour`를 사용하지 않게 소스 코드를 작성하자.
* Unity Test Runner를 사용하는 경우 폴더 구조는 아래와 같이 하자.
   <pre>Root / .*.Test / Editor</pre>    

# 참고

* [NUnit.org](https://nunit.org/)
* [NUnit Cheat Sheet](https://www.automatetheplanet.com/nunit-cheat-sheet/)
* [MS NUnit Test](https://docs.microsoft.com/ko-kr/dotnet/core/testing/unit-testing-with-nunit)