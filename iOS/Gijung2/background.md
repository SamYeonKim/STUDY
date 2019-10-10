# Background Execution

## 특정 업무 실행

### 앱이 실행 중이였거나 남은 업무가 남았을 경우

+ beginBackgroundTaskWithName:expirationHandler, beginBackgroundTaskWithExpirationHandler: 두 메소드 중 하나 호출.
+ 완료시 endBackgroundTask: 메소드 호출.

```
ex)

- (void)applicationDidEnterBackground:(UIApplication *)application
{
    bgTask = [application beginBackgroundTaskWithName:@"MyTask" expirationHandler:^{
        // Clean up any unfinished task business by marking where you
        // stopped or ending the task outright.
        [application endBackgroundTask:bgTask];
        bgTask = UIBackgroundTaskInvalid;
    }];
 
    // Start the long-running task and return immediately.
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
 
        // Do the work associated with the task, preferably in chunks.
 
        [application endBackgroundTask:bgTask];
        bgTask = UIBackgroundTaskInvalid;
    });
}

```

## Background에서 다운로드

1. NSURLSessionConfiguration의 메소드인 backgroundSessionConfigurationWithIdentifier 를 이용하여 오브젝트 생성.
2. sessionSendsLaunchEvents property를 Yes로 설정.
3. Forground에 있는 동안 다운로드가 실행되었다면 discretionary property를 설정.
4. 오브젝트의 다른 property들을 적절하게 구성.
5. NSURLSession 오브젝트를 만들기 위해 구성한 오브젝트를 이용.

## 오래걸리는 업무를 실행

### 앱에서 지원하는 Background 업무 선언

| Xcode background mode | UIBackgroundModes 변수 | 설명 |
| ------ | ------- | ----- |
|Audio and AirPlay|audio|사용자에게 Background에서 음악 재생 또는 오디오 녹음을 할 수 있도록 한다|
|Location updates|location|사용자의 위치 정보를 계속해서 제공|
|Voice over IP|voip|인터넷을 이용하여 전화를 할 수 있도록 제공|
|Newsstand downloads|newsstand-content|Background에서 잡지 또는 신문 콘텐츠를 다운로드 할 수 있도록 제공|
|External accessory communication|external-accessory|외부 엑세서리를 이용하여 정기적으로 업데이트를 해야 작동하도록 제공|
|Background fetch|fetch|네트워크를 통해 규칙적인 다운로드, 적은 양의 컨텐츠 처리|
|Remote notifications|remote-notification|푸시 알림이 도착했을때 다운로드 시작 처리|

#### 사용자 위치 추적

##### The significant-change location service

+ 정확한 위치 정보가 필요하지 않아 추천되는 방식.
+ 위치가 많이 변경되었을 경우에만 업데이트 되는 방식.
+ 위치 정보가 중요하지 않은 소셜앱에 적합한 방식.
+ IOS 4버전이후부터 사용이 가능한 방식.

##### The foreground-only, background location services 

+ 표준 코어 위치 서비스 (the standard location Core Location service)를 이용하여 위치를 검색하는 방식.
+ foreground-only 서비스의 경우 Forground에서만 갱신이 유효한 방식.
+ Info.plist의 UIBackgroundModes key와 location value를 이용하여 활성화 할 수 있다.

#### Background 오디오 재생 및 녹음

+ Info.plist의 UIBackgroundModes key와 audio value를 이용하여 활성화 할 수 있다.

##### Background에서 실행되는 앱들

+ 음악 재생 앱
+ 오디오 녹음 액
+ AirPlay를 통해 재생되는 음악 또는 오디오 앱
+ VoIP 앱

#### Fetching Small Amounts of Content Opportunistically

+ Info.plist의 UIBackgroundModes key와 fetch value를 이용하여 활성화 할 수 있다.
+ 특정 업데이트 상황이 있을 경우 앱이 Background로 실행되고, application:performFetchWithCompletionHandler 메소드가 실행될 것이다.

#### 푸시 알림을 이용하여 다운로드 시작.

+ Info.plist의 UIBackgroundModes key와 remote-notification value를 이용하여 활성화 할 수 있다.
+ 푸시 알림을 통해 다운로드가 실행되기 위해서는 알림 메시지에 content-available key와 value가 1로 포함되어 있어야 한다.
+ 키가 존재할때 Background에서 앱이 실행되고, application:didReceiveRemoteNotification:fetchCompletionHandler 메소드가 실행될 것이다.

## Background에 있는 동안 유저의 주의 얻기

+ 알림 표시, 아이콘, 소리로 주의를 얻을 수 있다.
+ UILocalNotification, UIApplication class를 이용하여 설정.
+ UIApplication class는 즉시 또는 예약된 시간을 설정 할 수 있는 옵션이 존재.

```
- (void)scheduleAlarmForDate:(NSDate*)theDate
{
    UIApplication* app = [UIApplication sharedApplication];
    NSArray*    oldNotifications = [app scheduledLocalNotifications];
 
    // Clear out the old notification before scheduling a new one.
    if ([oldNotifications count] > 0)
        [app cancelAllLocalNotifications];
 
    // Create a new notification.
    UILocalNotification* alarm = [[UILocalNotification alloc] init];
    if (alarm)
    {
        alarm.fireDate = theDate;
        alarm.timeZone = [NSTimeZone defaultTimeZone];
        alarm.repeatInterval = 0;
        alarm.soundName = @"alarmsound.caf";
        alarm.alertBody = @"Time to wake up!";
 
        [app scheduleLocalNotification:alarm];
    }
}

```

## 앱이 Background에서 실행될때의 이해

+ Background 실행을 지원하는 앱은 시스템 이벤트에 의한 재실행도 지원한다.
+ 앱이 어떠한 이유에서 종료가됐을때 아래 이벤트들 중 하나가 발생하면 앱이 실행이 된다.

### 위치 앱

+ 위치 업데이트를 수신 할 경우.
+ 등록된 지역을 들어오거나 나갈 경우.

### 오디오 앱

+ 오디오 프레임워크는 데이터를 처리하기 위해 앱이 필요하다 (???)

### Baground 다운로드 앱

+ 푸시 알림이 오거나, 알림에 content-available key가 포함된 경우.
+ 새로운 컨텐츠를 다운로드 받기위해 앱을 깨운 경우.

## 책임감 있는 Background 앱되기

1. 코드에서 Open GL ES 호출하지 않도록 한다.
2. 중지 상태가 되기전에 네트워크와 관련된 서비스를 취소시켜야 한다.
3. 네트워크 관련 서비스가 실패했을 때의 처리를 준비해놔야 한다.
4. Background 상태로 전환하기 전에 현재 앱 상태를 저장해야 한다.
5. Background 상태로 전환하기 전에 참조를 끊어야 한다.
6. Background 상태로 전환하기 전에 공유된 리소스 참조를 취소시켜야 한다.
7. 업데이트를 피해야한다.
8. 외부 접근에 대한 연결, 해제 알림을 보내야 한다.
9. Background 상태로 이동할때 활성화되어있는 알람의 리소스를 정리해야 한다.
10. Background 상태로 전환하기 전에 화면에 보이는 민감한 정보는 제거해야 한다.
11. Background 상태에 있는 동안에는 최소의 행위만 한다.

## Background 실행되지 않게 하기

+ Background에서 실행되기를 원하지 않는 다면 명시적으로 Inpo.plist 파일에 UIApplicationExitsOnSuspend key를 추가해야 한다.
