h1. Android build Trouble Shoot Guide

* build.gradle 의 sdk 버전 및 sdk build tools 버전을 수정하라.
* publishing Settings | build system은 internal 을 사용하라. (gradle은 fail)
* FaceBookSetting에서 [your android debug keystorefile is missing!] 이런 waring이 나올경우 아래 명령어 실행 해야 함
** keytool -genkey -alias androiddebugkey -keystore $HOME\.android\debug.keystore | openssl sha1 -binary | openssl base64 

h1. Ios build Trouble Shoot Guide

* 프로비저닝 프로파일 때문에 속이 탄다면 ~/Library/MobileDevice/Provisioning profiles/ 다 지우고 시도하라.

h1. 메모리 최적화 관련해서 알아낸 사실

* Development Build가 Non Development Build 보다 30mb 더 추가 된다.
* PostProcessingStack을 사용 할 경우 RenderTexture가 많이 생성되서 메모리 추가가 많이 된다.
* UI오브젝트 ( 패널, 위젯 )을 켜고 끌때 UIRect.OnEnable, UIRect.OnDisable이 호출 되는데, FPS를 많이 잡아 먹는다. 
    > 게임오브젝트를 끄는 것 대신, 부모에 해당하는 패널 (UIPanel ) 컴포넌트를 끄면 효과를 볼 수 있다.
    > ```cs
    > GameObject go_ui_panel;
    > go_ui_panel.SetActive(false or true ) -> 
    > UIPanel ui_panel = go_ui_panel.GetComponent<UIPanel>();
    > ui_panel.enabled = false or true;
    > ```
* IOS에서 PlayerSettings 에서 *Script Call Optimazation* 을 *Slow and Safe* 에서 *Fast but NoExceptions* 로 바꾸면 30mb 아낄 수 있다.
* Resources 모드가 RemoteAssetBundle 모드 보다 메모리가 많이 추가 된다.