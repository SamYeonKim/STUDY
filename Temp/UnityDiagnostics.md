# Setting up Cloud Diagnostics

## User Reporting

사용자가 인게임 내에서 직접 작성한 버그 보고서 또는 피드백 문서를 수집하여 확인할 수 있는 기능

## 사용 방법

1. 유니티 에디터를 켜고 상단 툴바에 Windows - Services 를 누른다.

-----
* 사전에 자신의 프로젝트를 `Unity Services` 와 연동하여야 한다. 만약 연동이 안되어 있다면 [유니티 서비스 등록](https://docs.unity3d.com/Manual/SettingUpProjectServices.html) 문서를 참고하여 먼저 내 프로젝트와 유니티 서비스를 연동하자
-----

2. 오른쪽에 표시되는 메뉴중 `Cloud Diagnostics` 클릭
3. `User Reporting` 항목에서 <u>download the SDK</u> 클릭
4. `Sdk`라는 이름의 압축파일이 다운로드 되는데 압축 해제후 자신의 버전에 맞는 유니티 패키지를 `import` 한다.
5. 하이어라키에 `UserReportingPrefab` 을 끌어다 놓고 실행한다.
6. 양식에 맞게 보고서 작성후 전송을 하면 `Unity Dashboard` 에 `현재 프로젝트 - Cloud Diagnostics - User Reports` 에서 확인이 가능하다.

[User Reporting 참고 문서](https://unitytech.github.io/clouddiagnostics/userreporting/UnityCloudDiagnosticsUserReports.html)

## Crashes and Exceptions

빌드된 유니티 앱을 실행하거나 유니티 에디터상에서 Play 모드일 때 `Crash & Exception`을 수집하는 기능

## 사용 방법

-----
* UserReporting과 똑같이 `Unity Serivces`에 자신의 프로젝트가 연동이 되어있어야 한다.
-----

1. 위의 1번 2번과 동일한 과정
2. `Crashes and Exceptions` 토글 버튼을 활성화 시킨다. (파란색이 되도록)
3. 발생한 `Exception` 들을 `Unity Dashboard - Crashes & Exceptions` 에서 확인

* 릴리즈 빌드에서는 Exception이 수집은 되지만, 코드 라인이 기록되지 않는다.
* 하루에 Report를 수집할 수 있는 갯수 제한이 존재한다.

-----
* 런타임 상에서 Exception을 수집할지 안할지를 해당 변수로 제어할 수 있다.
> UnityEngine.CrashReportHandler.CrashReportHandler.enableCaptureExceptions
-----

### Manage Symbols

Unity Plus or Pro 에서만 지원되는 `Native Crash Report`의 콜스택을 메모리 주소가 아닌 사람이 읽을 수 있는 함수 이름으로 보여주는 기능, 일반적으로 `Crash and Exception` 기능이 활성화 되어 있는 상태에서 앱을 빌드할경우 유니티가 자동으로 심볼 파일을 유니티 서버에 업로드한다.

하지만 업로드에 실패했을 경우 유니티 서비스 대시보드에 `symbols missing` 이라는 메시지가 뜬다. 이 경우 `첫번째` 방법으로 `Editor.log` 가 위치한 폴더에 `symbol_upload.log` 라는 로그 파일이 존재하게 되고, 해당 파일을 확인하여 심볼의 처리 및 업로드 중에 발생한 오류와 함께 어떤 심볼이 발견되고 처리되었는지를 확인해야 한다.
`두번째` 방법으로는 유니티 대시보드에 `Manage Symbols` 탭에서 직접 심볼 파일을 업로드 하여야 한다.

-----
각 심볼파일의 형식은 다음과 같다.

| OS | Format |
|:---|:-------|
| Apple | dSYM folders |
| Android | .so files |
| Windows | .pdb files |

* 안드로이드의 경우 프로젝트를 gradle로 `Export`하면 해당 폴더 내에 CPU 아키텍쳐에 맞는 `*.so` 파일들을 압축하여 업로드 할 수 있다.
* IOS의 경우 ~/Library/Developer/Xcode/DerivedData/(build id)/Build/Products/(build type)/appname.dSYM 에 존재한다.
-----

[Crashes and Exception 참고 문서](https://unitytech.github.io/clouddiagnostics/crashesandexceptions/UnityCloudDiagnosticsCrashesExceptions.html)

[Diagnostics 빠른 시작 참고 문서](https://unitytech.github.io/clouddiagnostics/userreporting/UnityCloudDiagnosticsSettingUp.html)