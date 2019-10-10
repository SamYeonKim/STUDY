# Perfomance Tips

## 전력 소모 줄이기

+ `다음 사항을 최적화 하므로서 배터리 수명을 늘릴 수 있다.`

```
1. The CPU
2. Wi-Fi, Bluetooth, and baseband (EDGE, 3G) radios
3. The Core Location framework
4. The accelerometers
5. The disk
```

+ `최적화된 알고리즘일지라도 배터리 수명에 부정적 영향을 끼칠 수 있다.`

```
1. 폴링(동기화 처리등의 목적으로 다른 프로그램의 상태를 주기적으로 검사하는 방식)을 피하라.
1-1. 대신 NSRunLoop or NSTimer classes를 사용해라.

2. UIApplication object의 idleTimerDisabled property의 속성을 No로 설정해야한다.
2-1. 이로 인해 사이드 이펙트가 생긴다면 위의 property를 사용하지 않는 방향으로 코드를 수정해야 한다.

3. 가능할때마다 코드 병합을 실시한다.
3-1. 작은 단위의 것들을 자주 호출하는 것보다 한번에 하나를 호출하는 것이 전력 소모가 적다.

4. 빈번한 디스크 접근을 피하라.

5. UIAccelerometer class가 필요없을때는 비활성화 하도록 처리한다.
```

+ `네트워크에 액세스하는 것은 가장 많은 전력을 소비하는 작업이다.`

```
1. 필요할때만 네트워크에 연결하도록 한다.

2. 나눠서 보내지 않고 한번에 보내도록 한다.
2-1. NSURLSession 클래스를 사용하면 여러 개의 업로드 또는 다운로드 작업을 대기열에 저장하여 한번에 처리가 가능하다.

3. 가능하다면 Wi-Fi로 연결한다.
3-1. Wi-Fi 사용이 전력을 덜 사용한다.

4. 프레임워크를 통해 위치 정보를 수집하는 경우 업데이트를 비활성화하고 거리 및 위치에 따라 업데이트 되도록 수정해야 한다.
```

## 메모리 효율적으로 사용하기

### 메모리 부족 경고

+ 메모리 경고는 즉시 일어나야 한다.

```
다음과 같은 API를 사용한다

1. applicationDidReceiveMemoryWarning method
2. UIViewController 클래스의 didReceiveMemoryWarning method
3. UIApplicationDidReceiveMemoryWarningNotification notification.
4. DISPATCH_SOURCE_TYPE_MEMORYPRESSURE.
4-1. 메모리 압력의 심각도를 구별 할 수 있는 유일한 방법이다. 
```

+ 경고 메시지를 받았을때 불필요한 메모리를 즉시 해제해야 한다.

#### 메모리 사용량 줄이기

| Tip | Actions to take |
| ------ | ------- |
| 메모리 누출 제거 | Instruments 응용 프로그램을 사용하여 시뮬레이터 및 실제 장치에서 코드의 누출을 추적|
| 리소스 파일을 작게 만든다 | NSPropertyListSerialization 클래스를 사용하여 이진 형식으로 속성 목록 파일을 작성하여 속성 목록 파일을 더 작게 만들 수 있다|
| 대규모 데이터 세트에는 Core Data 또는 SQLite를 사용 | 대량의 데이터를 메모리에 한번에 저장하지 않는 이점을 제공|
| 리소스를 느슨하게 로드 | 리소스를 미리 로드하는 것은 프로그램 속도를 저하시키므로 미리 로드하지 않는다 | 

### 메모리 현명하게 할당하기

| Tip | Actions to take |
| ------ | ------- |
| 리소스 크기 제한 | 대용량 리소스 파일을 사용해야하는 경우 주어진 시간에 필요한 파일 부분만 로드한다|
| 메모리를 초과하는 계산 피하기 | 세트에 사용 가능한 메모리보다 많은 메모리가 필요한 경우 앱에서 계산을 완료하지 못할 수 있다 |

## Tune Your Networking Code

### 효율적인 네트워크를 위한 팁

+ 가능한 데이터 형태를 Compact하게 한다.
+ 채팅 프로토콜을 피한다.
+ 데이터를 한번에 보낸다.

### Wi-Fi 사용

+ UIRequiresPersistentWiFi 키를 통해 사용을 요청하지 않은 경우 전력 소모를 줄이기 위해 30 분 후 하드웨어를 완전히 끄는 내장 타이머를 사용한다.

## 파일 관리 기능 향상

+ 적은양의 변경을 위해 전체 파일 작성을 하지 말아야 한다.
+ 수정해야 할 내용을 그룹화하여 매번 디스크에 기록해야 하는 양을 줄인다.
+ 자주 접근해야 하는 정보로 구성되어 있다면 Core Data persistent store 또는 a SQLite database에 저장해야 한다.

## 효율적으로 백업 만들기

### 백업 위치

+ 다시 생성하지 못하는 중요 데이터는 "<Application_Data>/Documents" 경로에 저장해야한다.
+ 어플리케이션 다운로드 및 실행 파일을 지원파일이라고 한다. 버전별로 다른 위치에 저장된다.
```
1. IOS 5.1 이후 버전
   : <Application_Data>/Library/Application Support 경로
2. IOS 5.0 이전 버전
   : <Application_Data>/Library/Caches 경로
```
+ 캐시 데이터는 "<Application_Data>/Library/Caches" 경로에 저장된다.
+ 임시 파일은 "<Application_Data>/tmp" 경로에 저장된다.

### 어플리케이션 업데이트 동안 파일 저장

+ <Application_Data>/Documents 또는 <Application_Data>/Library 경로에 저장된다.




