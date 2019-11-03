- [배경지식](#%EB%B0%B0%EA%B2%BD%EC%A7%80%EC%8B%9D)
- [사용방법](#%EC%82%AC%EC%9A%A9%EB%B0%A9%EB%B2%95)
  
-----

# 배경지식

* [HTTPS 및 SSL을 사용한 보안](https://developer.android.com/training/articles/security-ssl?hl=ko#SelfSigned)
  * 안드로이드에서 HTTPS를 설정 및 사용하는 방법
* [X.509란?](https://proneer.tistory.com/entry/%E3%85%8C%EC%95%84%EB%9F%AC)
  * HTTPS에서 사용하는 인증서 형식의 설명
* [TrustManager...](https://developer.android.com/reference/javax/net/ssl/TrustManager)
  * 안드로이드 API인 `TrustManager`의 메뉴얼
  * java 코드에서 사용하는 모든 인증서 관련 API들도 한번쯤 읽어야 한다.
* [AsyncTask](https://developer.android.com/reference/android/os/AsyncTask)
  * 스레드를 쉽게 사용하도록 비동기 함수를 제공해주는 추상 클래스


# 사용방법

1. 외부에서 다운로드를 위해 `Download` 함수 호출
   
2. `Download()` 함수에서는 파라미터로 받은 정보들을 저장 후 스레드를 생성하여 내부에서 `AsyncTask` 클래스를 상속받는 `DownloadTask`를 생성
3. `DownloadTask.execute()` 함수를 실행, 내부적으로 `doInBackground` 함수 호출
4. `m_cert_path`가 비어있지 않으면 해당 경로에서 인증서를 가져와서 검증
   * `GetContentBody(path)` 함수로 인증서 전문을 저장
   * `SetConnectionVerifier()` 함수의 파라미터로 인증서를 넘김
   * 받은 인증서를 디코딩하여 키와 인증서를 저장하는 [KeyStore](https://developer.android.com/reference/java/security/KeyStore) 클래스를 생성
   * `KeyStore` 인스턴스를 사용하여 신뢰할 수 있는 목록을 지정하는 [TrustManagerFactory](https://developer.android.com/reference/javax/net/ssl/TrustManagerFactory) 인스턴스를 생성 및 초기화
   * 소켓 프로토콜의 구현체인 [SSLContext](https://developer.android.com/reference/javax/net/ssl/SSLContext?hl=en) 인스턴스를 생성 및 초기화
   * `HttpsURLConnection`의 디폴트 소켓 팩토리를 위에서 만든 인스턴스로 변경
   * `HostnameVerifier` 의 비교 대상을 `Download()` 함수의 인자로 받은 `m_arr_hostname` 로 설정, `HttpsURLConnection`의 기본 `HostnameVerifier` 변경
  
5. 4번 과정이 실패하면 유니티에 구현되어 있는 `OnActComplete` 함수 호출 후 종료
6. `Download()` 함수의 인자로 받은 파일 리스트를 순회하면서 한 파일마다 `DownloadFile()`, `UnTar()` 함수 호출
   * `URLConnection.openConnection()` 함수를 통해 파일 서버에 연결, 한번에 `m_data_buffer` 크기 만큼 파일을 읽고 `publishProgress()` 함수 호출
   * `publishProgress()` 함수는 `DownloadTask` 클래스가 구현한 `onProgressUpdate()` 함수를 호출
   * `onProgressUpdate()` 함수는 유니티가 foreground에 있을 때 진행상황을 메시지로 넘겨준다.
  
7. 다운로드가 끝나면 유니티에 구현되어 있는 `OnActComplete` 함수 호출 후 리턴
8. `doInBackground()` 함수가 반환된 이후 내부적으로 `onPostExecute()` 함수를 호출, 사용한 자원을 초기화

* 함수에 대한 자세한 설명은 [ShadowDownload.java](../LibAndroid/ShadowDownload/shadowdownload/src/main/java/com/nadagames/shadowdownload/ShadowDownload.java) 파일 참고
