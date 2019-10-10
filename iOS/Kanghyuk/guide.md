# The App Life Cycle
> 앱 : 코드 + 시스템 프레임워크

> 시스템 프레임워크는 모든 앱이 실행해야 하는 기본 인프라와 디테일에 필요한 코드를 제공.

> iOS 프레임워크는 MVC와 delegation과 같은 디자인 패턴을 사용한다.

-----
## The Main Function
* iOS 앱의 경우 main 함수를 Xcode가 직접 만들어준다.
* main 함수는 UIKit 프레임워크에 컨트롤을 넘긴다.
* UIApplicationMain 함수는 인터페이스 로드, 초기 설정을 수행하는 코드 호출, 앱을 루프에 넣는 등의 일을 한다.

-----
## The Structure of an App
* 모든 iOS 앱의 핵심은 시스템과 다른 객체 간의 상호 작용을 촉진하는 앱 UIApplication 오브젝트이다.
* MVC 아키텍쳐를 사용하기 때문에 로직과 뷰가 구분되어 잇다.
* UIApplicationMain 함수는 인터페이스 로드, 초기 설정을 수행하는 코드 호출, 앱을 루프에 넣는 등의 일을 한다.

![main](https://developer.apple.com/library/archive/documentation/iPhone/Conceptual/iPhoneOSProgrammingGuide/Art/core_objects_2x.png)

* UIApplication object : 이벤트 루프와 프로그램 동작을 관리. 주요 앱 전환과 일부 특수 이벤트를 delegate에 알린다.
* App delegate object : UIApplication object와 같이 동작하면서 앱 초기화, 상태 전환 및 많은 고급 응용 프로그램 이벤트를 처리한다. 모든 응용 프로그램에 존재할 수 있는 유일한 객체이기 때문에 응용 프로그램의 초기 데이터 구조를 설정하는 데 자주 사용된다.
* Documents and data model objects : data model objects는 앱의 컨텐츠를 저장하며 특정 앱에만 적용된다. (금융 앱은 금융 거래 데이터베이스를 저장할 수 있고, 페인팅 앱은 이미지 객체나 그리기 명령 시퀀스를 저장할 수 있다)  앱은 document objects(UIDocument의 커스텀 서브클래스)를 사용하여 데이터 모델을 관리할 수 있다.
* View controller objects : 화면에 보일 컨텐츠들을 관리한다. UIViewController클래스는 모든 뷰 컨트롤러 객체의 기본 클래스로 갖가지 기본 기능을 제공한다. UIKit과 기타 프레임 워크는 표준 시스템 인터페이스를 구현하기위한 추가 view controller classes를 정의한다.
* UIWindow object : 화면에 보이는 뷰들을 조정한다. 윈도우 자체는 하나고 그 안의 컨텐츠들을 변경한다. UIApplication object 와 함께 뷰에 이벤트를 전달하고 컨트롤러를 표시한다.
* View objects, control objects, and layer objects : View object는 내용을 그리고 이벤트에 응답하는 개체, control object는 버튼, 텍스트필드, 토글과 같은 인터페이스를 구현하는 특수 유형의 뷰, layer object는 실제 시각적인 컨텐츠를 나타내는 data objects.(복잡한 애니메이션이나 정교한 시각 효과를 구현할 수 있다.)

-----
## The Main Run Loop
* 모든 사용자 관련 이벤트를 처리함.
* 메인쓰레드에서 실행되며 사용자 관련 이벤트가 수신 된 순서대로 처리되도록 한다.
* 이벤트 대부분은 main run loop를 사용하여 전달되지만 일부는 처리가 안된다.

![loop](https://developer.apple.com/library/archive/documentation/iPhone/Conceptual/iPhoneOSProgrammingGuide/Art/event_draw_cycle_a_2x.png)

* 터치 이벤트는 이벤트가 발생한 객체에 이벤트를 전달.
* Accelerometer, Magnetometer, Gyroscope는 사용자가 지정한 객체에 이벤트를 전달.
* Redraw 이벤트는 업데이트가 필요한 view에 이벤트를 전달.
* 대부분의 이벤트는 특정 응답 객체를 대상으로 하지만 필요한 경우 responder chain을 통해 다른 응답 객체에 전달이 가능하다.

-----
## Execution States for Apps
* 앱 상태는 5가지로 나눌 수 있다.
* Not running : 앱이 시작되지 않았거나 시스템에서 종료됨.
* Inactive : foreground에서 실행 중이지만 이벤트를 수신하지 않음. 다른 상태로 변경될 때 잠깐 머무는 상태.
* Active : foreground에서 실행중이며 이벤트를 수신함.
* Background : background에서 코드를 실행중. suspended되기 전 잠시 이 상태가 된다. 추가작업을 하면 이 상태를 유지할 수 있음. background에서 실행되는 앱은 inactive 대신 background상태가 된다.
* Suspended : background에서 코드를 실행하지 않음. 시스템은 앱을 자동으로 이 상태로 이동시키고 앱에 알리지 않음. 메모리에 남아있지만 코드는 실행하지 않는다. 메모리가 부족하면 예고없이 앱을 제거한다.

![state](https://developer.apple.com/library/archive/documentation/iPhone/Conceptual/iPhoneOSProgrammingGuide/Art/high_level_flow_2x.png)

* 각각의 상태변화에 앱은 delegate object의 특정 메소드를 호출한다.
* application:willFinishLaunchingWithOptions -> 앱 실행 전 호출.
* application:didFinishLaunchingWithOptions -> 앱이 사용자에게 표시되기 전 호출.
* applicationDidBecomeActive -> foreground가 되기 전 호출. 
* applicationWillResignActive ->  foreground에서 inactive 되기 전 호출.
* applicationDidEnterBackground ->  background가 되기 전 호출.
* applicationWillEnterForeground ->  background에서 foreground가 될 때 호출. inactive 상태임.
* applicationWillTerminate -> 앱을 종료할 때 호출. suspended 일 때는 호출하지 않음.

-----
## App Termination
* suspended된 앱은 종료할 때 알림을 받지 않는다. 
* background 상태인 앱이 종료될 때 시스템은 applicationWillTerminate을 호출한다. 장치가 재부팅 될 때는 호출하지 않는다.
* 사용자는 멀티 태스킹 UI를 사용하여 앱을 명시 적으로 종료 할 수 있다. 사용자 시작 종료는 suspended된 앱 종료와 동일한 효과가 있다.(앱에 알림이 전송되지 않는다.)

-----
## Threads and Concurrency
* iOS 앱의 경우 스레드를 직접 만들고 관리하는 것보다 Grand Central Dispatch(GCD)나 operation objects 및 기타 비동기 프로그래밍 인터페이스를 사용하는 것이 좋다.
* 시스템에서 해당 작업을 어떤 CPU에서 실행할 지 결정하게 할 수 있다.(??)
* 시스템이 스레드 관리를 하면 코드가 단순해지고 정확성이 보장되며 성능이 향상된다.

* views, Core Animation, 많은 UIKit classes와 관련된 작업들은 일반적으로 메인스레드에서 처리해야 한다. (몇 가지 예외가 존재)
* 길이가 긴 작업은 background 스레드에서 수행해야 한다.
* 앱을 시작할 때 가능하면 작업을 다른 스레드로 이동시키는 것이 좋다. 사용자 인터페이스 설정을 빨리 수행해야 하기 때문.


# Strategies for Handling App State Transitions
> 앱의 상태가 변경될 때 해당 코드를 구현하는 방법을 설명

-----
## What to Do at Launch Time
* 앱이 foreground나 background로 실행되면 application:willFinishLaunchingWithOptions:와 application:didFinishLaunchingWithOptions: 메소드로 작업을 수행할 수 있다. 주로 데이터 구조 초기화, 디스플레이 준비에 사용된다. 윈도우는 willFinish~ 메소드에서 보여진다.
* 시작 시 시스템은 앱의 기본 스토리보드 파일, 뷰 컨트롤러를 호출한다. 상태 복원을 지원하는 앱은 application:willFinishLaunchingWithOptions:와 application:didFinishLaunchingWithOptions: 메소드 사이에 복원이 일어난다. 
* 위의 두 메소드는 시간제한이 있어 그 전에 완료되지 않으면 시스템을 종료한다. 네트워크 액세스 같은 느린 작업은 보조스레드에서 수행해야한다.

-----
## The Launch Cycle
* 실행주기의 일부로 시스템은 프로세스와 메인스레드를 생성하고, 메인스레드에서 main함수를 호출한다. main함수는 UIKit 프레임워크를 컨트롤할 수 있다.
* 아래의 그림은 앱이 foreground로 실행될 때 발생하는 이벤트의 순서이다.

![foreground](https://developer.apple.com/library/archive/documentation/iPhone/Conceptual/iPhoneOSProgrammingGuide/Art/app_launch_fg_2x.png)

* 백그라운드로 시작되면 이후 일시중지될 수 있고, 유저 인터페이스 파일을 로드하지만 앱의 윈도우를 표시하지 않는다는 차이점이 있다.

![background](https://developer.apple.com/library/archive/documentation/iPhone/Conceptual/iPhoneOSProgrammingGuide/Art/app_launch_bg_2x.png)

* 앱이 foreground나 background로 실행되는지의 여부는 위의 willFinish~, didFinish메소드의 공유 UIApplication객체의 applicationState 프로퍼티로 확인할 수 있다.
* 앱은 항상 세로모드로 실행되며, 필요할 때 뷰 컨트롤러가 회전을 처리하도록 하여 가로모드를 지원할 수 있다. 가로모드만 사용할 경우 Info.plist에 UIInterfaceOrientation 키를 넣어 설정할 수 있다. 아니면 뷰 컨트롤러의 shouldAutorotateToInterfaceOrientation 메소드를 재정의하여 설정할 수 있다.
* 앱의 첫 실행주기를 사용하여 실행해야하는 데이터나 configuration파일을 설정할 수 있다. 이 파일들은 앱 샌드박스의 Library/Application Support/<bundleID>/ 디렉토리에 만들어야 한다. 
* 앱 번들 내부의 파일을 수정하면 앱 서명이 무효화되지만 Application Support 디렉토리에 복사하고 수정하면 안전하게 사용이 가능하다.

-----
## What to Do When Your App Is Interrupted Temporarily
* 앱이 중단되면 터치 이벤트를 수신하지 않는다. (가속도 체크 등의 이벤트는 수신된다.) 중단될 경우 applicationWillResignActive 메소드를 호출한다. 여기서 데이터나 타이머 관련 작업을 중단하고 일시 중지 상태로 들어가야 한다.
* 활성상태로 돌아가면 applicationDidBecomeActive 메소드를 호출한다.
* 전화 통화 같은 알림 기반의 인터럽트가 발생하면 앱은 일시적으로 비활성 상태로 전환되며 사용자가 알림을 닫을 때 까지 유지된다.

![interrupt](https://developer.apple.com/library/archive/documentation/iPhone/Conceptual/iPhoneOSProgrammingGuide/Art/app_interruptions_2x.png)

-----
## What to Do When Your App Enters the Foreground
* foreground로 들어갈 때, applicationDidEnterBackground 메소드에서 한 일을 되돌리는 applicationWillEnterForeground 메소드를 호출하고, 실행 시점에 했던 일을 하는 applicationDidBecomeActive 메소드를 호출한다.

![toForeground](https://developer.apple.com/library/archive/documentation/iPhone/Conceptual/iPhoneOSProgrammingGuide/Art/app_enter_foreground_2x.png)

* 일시 중지 상태의 앱은 코드를 실행하지 않기 때문에 앱이 코드 실행을 다시 시작하는 즉시 앱의 모양이나 상태에 영향을 주는 알림을 앱에 전달한다. 해당 알림은 앱의 메인 루프에 전달되어 터치이벤트나 다른 사용자 입력 전에 전달된다.
* iCloud 상태가 변경되면 시스템은 NSUbiquityIdentityDidChangeNotification 알림을 전송한다. 이 때 캐시와 iCloud 관련 사용자 인터페이스 요소를 업데이트 해야 한다.
* 앱에 설정 앱이 관리하는 요소가 있을 때  NSUserDefaultsDidChangeNotification 알림을 사용하여 관련 설정을 다시 로드하고 사용자 인터페이스를 재설장할 수 있다. 

-----
## What to Do When Your App Enters the Background
* background로 들어갈 때, applicationDidEnterBackground 메소드에서 앱 상태 정보 저장, 메모리 확보 등의 작업을 해야 한다.
* foreground에서 넘어갈 때는 비활성 상태로 전환 후 background 상태가 되기 때문에 applicationWillResignActive 메소드와 applicationDidEnterBackground 메소드를 차례로 호출한다.

![toBackground](https://developer.apple.com/library/archive/documentation/iPhone/Conceptual/iPhoneOSProgrammingGuide/Art/app_bg_life_cycle_2x.png)

* background로 들어갈 때 메모리가 부족하면 일시 중지 된 응용 프로그램을 종료하여 메모리를 회수한다.


# Inter-App Communication
> 앱은 기기의 다른 앱과 간접적으로만 통신한다. AirDrop으로 파일 및 데이터를 공유할 수 있고, 커스텀 URL 스키마를 정의해서 URL으로 앱에 정보를 보내게 할 수 있다.

-----
## Using URL Schemes to Communicate with Apps
* 커스텀 URL 스키마를 구현한 앱에 데이터를 보내려면 적절하게 형식이 지정된 URL을 만들고 openURL 메소드를 호출해야 한다.
```
NSURL * myURL = [NSURL URLWithString : @ "todolist : //www.acme.com? Quarterly % 20Report # 200806231300"];
[[UIApplication sharedApplication] openURL : myURL];
```
* 앱에서 특수 형식의 URL을 수신 할 수 있는 경우 해당 URL 스키마를 시스템에 등록해야 한다. 앱은 종종 맞춤 URL 스키마(ex. 지도 앱의 특정지도 위치를 표시하기 위한 URL)를 사용하여 다른 앱에 서비스를 제공한다.
* 앱의 URL 유형을 등록하려면 Info.plist파일에 CFBundleURLTypes 키를 추가해야 한다. 해당 키는 딕셔너리 배열 형태로, 앱이 지원하는 URL 스키마를 정의한다.
* 모든 URL은 실행시, 앱 실행중, background에서 app delegate에게 전달된다. URL을 처리하려면 application:willFinishLaunchingWithOptions와 application:didFinishLaunchingWithOptions 메소드를 사용하여 URL에 대한 정보를 검색하고 URL을 열 것인지 정할 수 있다. 하나라도 no를 반환하면 URL처리 코드가 호출되지 않는다.(????)
* 실제 URL의 실행은 application:openURL:sourceApplication:annotation 메소드에서 이루어진다.

![url](https://developer.apple.com/library/archive/documentation/iPhone/Conceptual/iPhoneOSProgrammingGuide/Art/app_open_url_2x.png)

* URL 요청이 도착하면 background, 일시 중지된 상태에서 foreground 로 이동한다. 그 후 시스템은 application:openURL:sourceApplication:annotation 메소드르 호출하여 URL을 확인하고 그것을 연다.

![urlBackground](https://developer.apple.com/library/archive/documentation/iPhone/Conceptual/iPhoneOSProgrammingGuide/Art/app_bg_open_url_2x.png)

* 모든 URL은 NSURL의 객체에 전달된다. URL형식을 정의하는 것은 사용자가 하지만, NSURL 클래스는 RFC 1808 사양을 준수하므로 대부분의 URL 형식 규칙을 지원한다.
* URL 스키마를 지원하는 앱은 각 스키마에 커스텀 시작 이미지를 넣을 수 있다. 시스템이 URL을 처리하기 위해 앱을 실행하고 관련 스냅샷을 사용할 수 없는 경우 지정한 이미지를 표시한다.
* 이름 규칙은 <basename> -<url_scheme> <other_modifiers>.png이다. basename은 Info.plist의 UILaunchImgaeFile 키에 정의된 이름, url_scheme는 스키마 이름을 넣으면 된다. other_modifier는 이미지의 옵션을 의미한다.
* ex) Default-myapp@2x.png (@2x는 이미지가 레티나 디스플레이에서 사용될 것이라고 알려줌. 표준 해상도 디스플레이를 지원한다면 없어도 무방하다)