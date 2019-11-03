- [라이브러리 설명](#%EB%9D%BC%EC%9D%B4%EB%B8%8C%EB%9F%AC%EB%A6%AC-%EC%84%A4%EB%AA%85)
- [외부라이브러리 이름 및 버전 등등](#%EC%99%B8%EB%B6%80%EB%9D%BC%EC%9D%B4%EB%B8%8C%EB%9F%AC%EB%A6%AC-%EC%9D%B4%EB%A6%84-%EB%B0%8F-%EB%B2%84%EC%A0%84-%EB%93%B1%EB%93%B1)
- [시나리오](#%EC%8B%9C%EB%82%98%EB%A6%AC%EC%98%A4)
  - [웹뷰가 필요 할 때](#%EC%9B%B9%EB%B7%B0%EA%B0%80-%ED%95%84%EC%9A%94-%ED%95%A0-%EB%95%8C)
  - [웹뷰가 필요 없을 때](#%EC%9B%B9%EB%B7%B0%EA%B0%80-%ED%95%84%EC%9A%94-%EC%97%86%EC%9D%84-%EB%95%8C)
- [설치방법](#%EC%84%A4%EC%B9%98%EB%B0%A9%EB%B2%95)
- [사용방법](#%EC%82%AC%EC%9A%A9%EB%B0%A9%EB%B2%95)

-----

# 라이브러리 설명

* 웹뷰를 생성하는 기능을 제공한다.


# 외부라이브러리 이름 및 버전 등등

* [gree/unity-webview](https://github.com/gree/unity-webview) 


# 시나리오

## 웹뷰가 필요 할 때
1. (Client) WebviewBehaviour 컴포넌트를 붙일 게임오브젝트, URL, Margin, 웹뷰 결과를 받을 액션 선정 
2. (Client) WebviewAdpater::CreateWebview 호출
3. (WebviewAdapter) 넘겨 받은 URL이 빈값인지 확인
4. (WebviewAdpater) 넘겨 받은 게임오브젝트에 WebviewBehaviour가 있는지 확인 후 없으면 AddComponent
5. (WebviewAdpater) WebviewBehaviour::ShowWebview 호출
6. (WebviewBehaviour) 한번이라도 Init 함수를 호출 한 적이 있는지 확인, 호출한 적이 없다면, WebViewObject::Init 함수 호출
7. (WebviewBehaviour) Margin 설정 후, URL을 이용해서 웹뷰 로드

## 웹뷰가 필요 없을 때
1. (Client) 더이상 웹뷰가 필요 없다고 판단하는 게임오브젝트 선정
2. (Client) WebviewAdapter::DesotryWebview 호출
3. (WebviewAdapter) WebviewBehaviour::HideWebview 호출
4. (WebviewAdapter) 넘겨 받은 게임오브젝트에서 WebviewBehaviour 컴포넌트 삭제


# 설치방법

* `Webview.unitypackage` 패키지를 임포트 한다.


# 사용방법

* [WebviewAdapter.cs](../Lib/Webview/Assets/.Webview/Script/WebviewAdapter.cs) 참고
* [WebviewBehaviour.cs](../Lib/Webview/Assets/.Webview/Script/WebviewBehaviour.cs) 참고