#include <glad/glad.h> // 먼저 등록해야 올바른 OpenGL 파일을 참조할 수 있다.
#include <GLFW/glfw3.h>

#include <iostream>

void framebuffer_size_callback(GLFWwindow* window, int width, int height);
void processInput(GLFWwindow *window);

// settings
const unsigned int SCR_WIDTH = 800;
const unsigned int SCR_HEIGHT = 600;

// GLSL 버텍스 쉐이더 코드
const char *vertexShaderSource = "#version 330 core\n" // 셰이더는 버전 선언부터 시작한다. 330 -> OpenGL 3.3 version
"layout (location = 0) in vec3 aPos;\n" // in : vertex shader에 입력받은 모든 vertex들을 선언 (해당 예제에서는 1개)
										// layout(location=0) : 입력 변수의 위치를 구체적으로 설정 (??)
"void main()\n"
"{\n"
"   gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n" // 입력받은 정점 값을 출력 변수에 저장 (원래는 정규화 과정을 거쳐야 하지만 지금은 입력값이 정규화 좌표이다)
"}\0";
// GLSL 프래그먼트 쉐이더 코드
const char *fragmentShaderSource = "#version 330 core\n"
"out vec4 FragColor;\n" // out : 출력되는 최종 컬러를 정의하는 벡터
"void main()\n"
"{\n"
"   FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);\n" // 오렌지색으로 렌더링
"}\n\0";

int main() {
	glfwInit(); // glfw 초기화
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3); // 윈도우 구성 옵션, 옵션 값
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

#ifdef __APPLE__
	glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE); // 맥에서 사용해야 함
#endif

	// 윈도우 객체 생성 : 모든 윈도우 데이터 보유
	GLFWwindow* window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "Triangle", NULL, NULL);
	if (window == NULL) {
		std::cout << "Failed to create GLFW window" << std::endl;
		glfwTerminate();
		return -1;
	}
	glfwMakeContextCurrent(window); // window를 현재 스레드의 주 컨텍스트로 지정
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback); // 윈도우 크기가 바뀔때마다 호출할 함수를 등록

	// GLAD가 OpenGL용 함수 포인터를 관리하도록 만들기 위해 GLAD 초기화
	// glfwGetProcAddress : 컴파일 할 OS에 따라 올바른 함수를 정의하는 함수
	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress)) {
		std::cout << "Failed to initialize GLAD" << std::endl;
		return -1;
	}


	// 버텍스 쉐이더를 런타임에 동적으로 컴파일하는 과정
	int vertexShader = glCreateShader(GL_VERTEX_SHADER); // 쉐이더 생성(버텍스 쉐이더 유형)
	glShaderSource(vertexShader, 1, &vertexShaderSource, NULL); // 쉐이더 소스 코드를 생성한 객체에 첨부한다
	glCompileShader(vertexShader); // 쉐이더를 컴파일

	// 쉐이더 컴파일 중 오류가 발생했는지 검사
	int success;
	char infoLog[512];
	glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success); // 컴파일이 성공했는지 확인
	if (!success) {
		glGetShaderInfoLog(vertexShader, 512, NULL, infoLog);
		std::cout << "ERROR::SHADER::VERTEX::COMPILATION_FAILED\n" << infoLog << std::endl;
	}

	// fragment shader
	int fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
	glShaderSource(fragmentShader, 1, &fragmentShaderSource, NULL);
	glCompileShader(fragmentShader);

	// check for shader compile errors
	glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &success);
	if (!success) {
		glGetShaderInfoLog(fragmentShader, 512, NULL, infoLog);
		std::cout << "ERROR::SHADER::FRAGMENT::COMPILATION_FAILED\n" << infoLog << std::endl;
	}


	// 위에서 생성한 쉐이더 객체를 렌더링에 사용할 수 있는 쉐이더 프로그램에 연결한다.
	// 각 쉐이더의 출력을 다음 쉐이더의 입력에 연결한다. 일치하지 않으면 오류 발생
	int shaderProgram = glCreateProgram();
	glAttachShader(shaderProgram, vertexShader);
	glAttachShader(shaderProgram, fragmentShader);
	glLinkProgram(shaderProgram);

	// check for linking errors
	glGetProgramiv(shaderProgram, GL_LINK_STATUS, &success);
	if (!success) {
		glGetProgramInfoLog(shaderProgram, 512, NULL, infoLog);
		std::cout << "ERROR::SHADER::PROGRAM::LINKING_FAILED\n" << infoLog << std::endl;
	}
	// 연결에 성공하면 이전에 만든 쉐이더 객체를 제거한다.
	glDeleteShader(vertexShader);
	glDeleteShader(fragmentShader);

	// 버텍스 데이터 (OpenGL은 노멀라이즈된 값(Normalized Device Coordinates)만 처리한다. 나머지는 화면에서 벗어나기 때문)
	// vertex shader에 입력으로 보내진다.
	float vertices[] = {
		-0.5f, -0.5f, 0.0f,
		0.5f, -0.5f, 0.0f,
		0.0f,  0.5f, 0.0f
	};

	// 버퍼 ID를 가진 버퍼를 생성
	unsigned int VBO, VAO;
	glGenVertexArrays(1, &VAO);
	glGenBuffers(1, &VBO);
	// Vertex Array Object(VAO) : 아래에서 설정한 버텍스 속성이 저장됨, 버텍스 속성을 구성할 때 한번만 호출해야 한다.
	// 버텍스 배열 오브젝트 바인딩 -> 버텍스 버퍼 바인딩 -> 버텍스 속성 구성
	glBindVertexArray(VAO);

	// GL_ARRAY_BUFFER : vertex 버퍼 오브젝트의 버퍼 타입
	glBindBuffer(GL_ARRAY_BUFFER, VBO); // VBO 버퍼를 GL_ARRAY_BUFFER 대상에 바인딩
	// 사용자가 정의한 데이터(vertices)를 바인드된 버퍼(VBO)에 복사하는 함수
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);  // 버퍼 유형, 버퍼 크기, 버퍼 데이터, 그래픽카드가 데이터를 관리하는 방법 (GL_STATIC_DRAW : 데이터가 거의 바뀌지 않음)

	// 버텍스 쉐이더에 값을 전달할 때 해당 데이터를 해석하는 방법을 지정해주어야 한다.
	// 1 : position vertex attribute의 위치.  vertex shader에서 layout location으로 지정해준 값
	// 2 : 버텍스 크기(개수)
	// 3 : 데이터 유형
	// 4 : 데이터를 normalize할 것인지 여부를 지정
	// 5 : 버텍스 사이의 데이터 간격(여기서는 x,y,z를 포함하는 점 데이터의 크기와 동일)
	// 6 : 위치 데이터가 버퍼에서 시작되는 위치의 오프셋(??)
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
	glEnableVertexAttribArray(0); // 설정한 속성을 활성화하는 함수(기본은 비활성화되어 있음)

	// VBO 바인딩을 해제
	glBindBuffer(GL_ARRAY_BUFFER, 0);

	// VAO 바인딩을 해제
	glBindVertexArray(0);


	// 프론트, 백 버퍼에 선만 그리고 싶은 경우 사용
	glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);

	// 중지 명령이 들어오기 전까지 윈도우에 계속해서 그리는 작업(렌더링) 반복
	while (!glfwWindowShouldClose(window)) {
		// input
		processInput(window); // 매 프레임마다 키 입력 확인을 위해 넣은 함수

		// render
		glClearColor(0.2f, 0.3f, 0.3f, 1.0f); // 컬러 버퍼에 해당 컬러를 채움, 상태 설정 함수
		glClear(GL_COLOR_BUFFER_BIT); // 컬러 버퍼의 상태를 적용, 상태 사용 함수

		glUseProgram(shaderProgram); // 위에서 만든 쉐이더 프로그램을 사용. 이후의 모든 쉐이더, 렌더링 호출은 해당 객체를 사용한다.
		glBindVertexArray(VAO); // 버텍스 속성 정보를 바인딩
		// 버텍스 속성과 버텍스 데이터(VAO를 통해 간접적으로 바인딩됨)을 사용하여 윈도우에 그림
		glDrawArrays(GL_TRIANGLES, 0, 3); // 그려지는 OpenGL primitive 타입, 버텍스 배열의 시작 인덱스, 버텍스 개수
		// glBindVertexArray(0); // 버텍스 속성 정보의 바인딩 해제, 매번 할 필요 없다.

		// glfw: swap buffer & poll IO event
		glfwSwapBuffers(window); // 백 버퍼를 프론트 버퍼로 교체
		glfwPollEvents(); // 키보드 입력, 마우스 이동 등의 이벤트를 확인하고 윈도우 상태 업데이트
	}

	// 할당된 객체들의 메모리를 해제
	glDeleteVertexArrays(1, &VAO);
	glDeleteBuffers(1, &VBO);

	glfwTerminate(); // 할당된 GLFW의 모든 자원을 지움
	return 0;
}



void processInput(GLFWwindow *window) {
	// 현재 GLFW_KEY_ESCAPE 키가 눌려져 있는지 확인
	if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
		glfwSetWindowShouldClose(window, true);
}

void framebuffer_size_callback(GLFWwindow* window, int width, int height) {
	// 2D 좌표를 화면의 좌표로 변환하는 함수
	// ex) 800x600 기준 (-0.5, 0,5)는 (200, 450)에 매핑됨
	glViewport(0, 0, width, height);
}