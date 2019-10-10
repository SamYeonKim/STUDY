# GLFW

* C로 작성된 라이브러리
* window, context를 만들고, Window 매개변수를 정의, 필요한 모든 사용자 입력을 처리할 수 있음
* opengl 라이브러리에는 위의 내용 처리 구현 X

## 관련 함수

`glfwInit()`
* GLFW 초기화

`glfwWindowHint(int hint, int value)`
* GLFW 구성
* hint : 구성하고자 하는 옵션
* value : 옵션값
* ex) `glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);`
  
`glfwMakeContextCurrent(GLFWwindow* window)`
* 호출중인 스레드에서 지정된 window의 opengl 컨텍스트를 최신 상태로 만듦  
  
`glfwGetKey(GLFWWindow* window, int key)`
* 지정된 키의 마지막 상태 반환
* 반환값 : GLFW_PRESS / GLFW_RELEASE 
  
`glfwSetWindowShouldClose(GLFWWindow* window, int value)`
* 지정된 윈도우의 close flag 설정 
  
`glfwPollEvents()`
* 이벤트를 처리하면 해당 이벤트와 연관된 입력 콜백이 호출됨