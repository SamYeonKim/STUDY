# Analytics

+ `사용자들의 패턴`을 들여다 보고, 사용자들이 `원하는 기능 및 최적화`를 할 수 있도록 돕는 도구.

## Dashboard 설명

### Data explorer

+ 시간 경과에 따른 `이벤트의 변화량`을 보여준다.

#### 이벤트 설정

`[Event] [Segment]`

![](img/UnityAnalytics/img_1.PNG)

|Event 종류|설명|
|:---|:---|
|`DAU (Daily Active Users)`| 일간 활동한 유저|
|`MAU (Monthly Active Users)`|월간 활동한 유저|
|`DAU per MAU`|월간 활동한 유저중 일일 활동한 유저|
|`New Users`|게임을 처음 플레이한 유저|
|`Number of Users`| 90일동안 누적된 플레이어 수|
|`Percentage of Population`|플레이어 인구 비율|
|`Sessions per User`|일간 접속 횟수???|
|`Total Daily Playing Time`|일간 모든 플레이어의 사용 시간|
|`Total Daily Playing Time per Active User`|일간 사용 시간|
|`Total Sessions Today`|일간 모든 플레이어의 접속 횟수???|
|`Day 1 Retention`|처음 플레이 이후 하루뒤에 되돌아온 퍼센트|
|`Day 7 Retention`|처음 플레이 이후 일주일뒤에 되돌아온 퍼센트|
|`Day 30 Retention`|처음 플레이 이후 한달뒤에 되돌아온 퍼센트|
|`ARPDAU (Average Revenue Per Daily Active User)`|일간 유저의 평균 지출|
|`ARPPU (Average revenue Per Paying User)`|유저당 평균 지출|
|`Number of Unverified Transactions`|확인되지 않은 인앱구매 횟수|
|`Number of Verified Transactions`|앱스토어, 구글플레이 스토어를 통해 검증된 인앱구매 횟수|
|`Total IAP Revenue`|전체 인앱구매 수익|
|`Total Verified Revenue`|광고, 검증된 인앱구매의 전체 수익|
|`Unverified IAP Revenue`|검증을 지원하지 않거나, 검증에 실패한 인앱 구매의 수익|
|`Verified IAP Revenue`|검증된 인앱구매 수익|
|`Verified Paying Users`|검증된 인앱구매를 한 유저|
|||

|Segment 종류|설명|
|:---|:---|
|`1–3 Days`|1~3일 동안 플레이한 유저|
|`4–7 Days`|4~7일 동안 플레이한 유저|
|`8–14 Days`|8~14일 동안 플레이한 유저|
|`15–30 Days`|15~30일 동안 플레이한 유저|
|`31+ Days`|31일이상 플레이한 유저|
|`Australia`|호주에 거주하는 유저|
|`Canada`|캐나다에 거주하는 유저|
|`China`|중국에 거주하는 유저|
|`France`|프랑스에 거주하는 유저|
|`Germany`|독일에 거주하는 유저|
|`Japan`|일본에 거주하는 유저|
|`Russia`|러시아에 거주하는 유저|
|`South Korea`|한국에 거주하는 유저|
|`Taiwan`|대만에 거주하는 유저|
|`United Kingdom`|영국에 거주하는 유저|
|`United States`|미국에 거주하는 유저|
|`Rest of World`|기타 국가에 거주하는 유저|
|`Whales`|20달러 이상 사용한 유저|
|`Dolphins`|5 ~ 19.99달러를 사용한 유저|
|`Minnows`|5달러 미만 사용한 유저|
|`Never Monetized`|과금을 하지 않은 유저|
|`All Spenders`|인앱 구매를 해본 모든 유저|
|`Android Users`|안드로이드를 사용하는 유저|
|`iOS Users`|iOS를 사용하는 유저|
|||

#### 커스텀 이벤트 설정

`[Event Name] [Segment] [Parameter] [Calculation]`

![](img/UnityAnalytics/img_2.PNG)

`Parameter`

+ 특정 Event의 파라미터 이름.

|Parameter 종류|설명|
|:---|:---|
|`#`|Calculation을 사용 할 수 있는 파라미터|
|`A`|특정 그룹으로 분류될 수 있는 파라미터???|
|||

`Calculation`

+ 파라미터의 값 집계 방법.

|Calculation 종류|설명|
|:---|:---|
|`sum`|합계|
|`count`|개수|
|`average`|평균|
|||

![](img/UnityAnalytics/img_3.PNG)

#### Annotaions

+ 특정 날짜의 `표시 및 메모 기능`.

![](img/UnityAnalytics/img_13.PNG)

### Funnel Analyzer

+ 유저의 `진행도 상황을 쉽게 파악`하기 위해 사용한다.
+ 튜토리얼이나 퀘스트같은 컨텐츠의 진행 상황을 파악하기 위해 사용한다.

#### Funnel 생성

+ 아래 스크린샷 처럼 `단계별 이벤트를 적용`하여 유저들의 `이벤트 진행상황을 확인` 할 수 있다.

![](img/UnityAnalytics/img_4.PNG)
![](img/UnityAnalytics/img_14.PNG)

### Segments

+ 특정 이벤트의 `필터기능을 하는 조건문`으로 사용한다.
+ Data Explorer, Funnel Analyzer에서 사용한다.

#### Segment 생성

+ Rules를 이용하여 `특정 조건`을 적용한 Segment를 만들 수 있다.
+ 조건은 `"ADD GROUP"`과 `"AND, OR, NONE"`을 조합하여 만들 수 있다.

|Rules|설명|
|:---|:---|
|`AND`|조건 그룹을 모두 만족할때|
|`OR`|조건 그룹 중 하나만 만족할때|
|`NONE`|조건 그룹의 반대 내용을 만족할때|

![](img/UnityAnalytics/img_6.PNG)

### Livestream

+ `실시간 갱신된 데이터 정보`를 보여준다.
+ `Pro부터 사용`이 가능하다

### Event Manager

+ 게임으로부터 받은 `이벤트와 파라미터를 관리해준다`.

![](img/UnityAnalytics/img_7.PNG)

### Raw Data Export

+ 데이터에서 Analytics의 `모든 제어 기능을 사용`하기 위하여 RawData를 다운 받을 수 있는 기능.
+ `Pro부터 사용`이 가능하다

### Market Insights

+ 유니티 게임을 하기위해 `사용되는 하드웨어의 정보`를 보여준다.

|종류|설명|
|:---|:---|
|`Aspect Ratio`|해상도|
|`CPU`|중앙 제어장치의 모델 이름|
|`CPU Count`|CPU 코어 개수|
|`CPU Vendor`|CPU 제조사명|
|`GFX Name`|GPU 모델 이름|
|`GFX Vendor`|GPU 제조사명|
|`OS Version`|운영체제 버전|
|`Platform`|플렛폼 이름|
|`RAM`|사용가능한 메모리량|
|`Screen`|스크린의 픽셀 수치|
|`Vendor`|디바이스 제조사명|

## 설치 및 사용 방법

1. Unity의 Services에서 `Analytics 활성화`

![](img/UnityAnalytics/img_8.PNG)

2. 테스트환경을 인증하기 위해 `Play를 실행`한다.

3. Services의 `"Go to Dashboard"`를 이용하여 분석화면으로 이동.

4. 확인하고자 하는 Project의 Analytics 확인.

5. 다음과 같이 코딩한다.

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class A : MonoBehaviour {
    private void OnGUI() {
        // StandardEvent
        if (GUILayout.Button("StandardEvent", GUILayout.Width(100), GUILayout.Height(100))) {
            AnalyticsEvent.TutorialStart("Tutorial", new Dictionary<string, object> {
                {"하나", 1},
                {"둘", 2},
                {"셋", 3}
            });
        }

        // CustomEvent
        if (GUILayout.Button("CustomEvent", GUILayout.Width(100), GUILayout.Height(100))) {    
            AnalyticsEvent.Custom("TEST", new Dictionary<string, object> {
                {"하나", 1},
                {"둘", 2},
                {"셋", 3}
            });
        }
    }
}
```

+ Event를 이용하여 `Data Explorer, Funnel Analyzer에서 사용이 가능`하다.

![](img/UnityAnalytics/img_11.PNG)
![](img/UnityAnalytics/img_12.PNG)

### Event 확인 방법

+ `Unity의 Services`와 `Analytics의 Event Manager`에서 호출된 이벤트를 확인 할 수 있다.

![](img/UnityAnalytics/img_9.PNG)
![](img/UnityAnalytics/img_10.PNG)