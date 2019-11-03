- [배경지식](#%EB%B0%B0%EA%B2%BD%EC%A7%80%EC%8B%9D)
- [설정방법](#%EC%84%A4%EC%A0%95%EB%B0%A9%EB%B2%95)
- [사용방법](#%EC%82%AC%EC%9A%A9%EB%B0%A9%EB%B2%95)
  
-----

# 배경지식

* [NSURLSession](https://developer.apple.com/documentation/foundation/nsurlsession) : http/https 를 통한 다운로드 관련 클래스
* [NSURLSessionDownloadTask](https://developer.apple.com/documentation/foundation/nsurlsessiondownloadtask) : 다운로드한 데이터를 파일에 저장하는 작업 관련 클래스


# 설정방법

* 인증서 검증을 위한 API 사용을 위해 `Security` 라이브러리를 추가해야 한다.
* `Build Phases` -> `Link Binary With Libraries`에서 하단의 `+`버튼을 누른다.

![](img/ShadowDownload/add_ios_library_1.png)

* `security` 라이브러리 추가

![](img/ShadowDownload/add_ios_library_2.png)


# 사용방법

1. 외부에서 다운로드를 위해 `ShadowDownload_Download` 함수 호출
2. `ShadowDownload_Download`에서 `Download` 함수 호출
3. `Download`함수에서는 파라미터로 받은 정보들을 저장 후 `DownloadFile` 호출
4. `DownloadFile`에서 다운로드 요청
5. `https`일 경우 서버에서 받은 인증서를 검증하는 작업이 필요하기 때문에 
  `URLSession:didReceiveChallenge:completionHandler:`델리게이트를 제공해야함
6. 델리게이트 함수 안에서 인증서 검증 과정을 거쳐 다운로드 실행 여부 결정
7. 다운로드가 실행될 경우 
   `URLSession:downloadTask:didWriteData:totalBytesWritten:totalBytesExpectedToWrite:` 
   델리게이트를 통해 다운로드 현황을 받음
8. 다운로드가 완료됐을 경우 `URLSession:downloadTask:didFinishDownloadingToURL:` 
   델리게이트를 통해 결과를 전달받고 다운로드한 파일을 영구폴더로 복사
9.  모든 파일들이 다운로드될 때 까지 7~8 과정 반복, 반복하는 도중에 오류가 발생할 경우      
    `URLSession:task:didCompleteWithError:` 델리게이트를 통해 오류를 전달받고 다운로드 종료
10. 모든 파일들이 다운로드 됐을 경우 다운로드 완료 핸들러 호출

* 함수에 대한 자세한 설명은 [ShadowDownload.m](../Lib/ShadowDownload/Assets/Plugins/IOS/ShadowDownload.m) 파일 참고
