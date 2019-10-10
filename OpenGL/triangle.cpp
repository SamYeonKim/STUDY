#include <glad/glad.h> // ���� ����ؾ� �ùٸ� OpenGL ������ ������ �� �ִ�.
#include <GLFW/glfw3.h>

#include <iostream>

void framebuffer_size_callback(GLFWwindow* window, int width, int height);
void processInput(GLFWwindow *window);

// settings
const unsigned int SCR_WIDTH = 800;
const unsigned int SCR_HEIGHT = 600;

// GLSL ���ؽ� ���̴� �ڵ�
const char *vertexShaderSource = "#version 330 core\n" // ���̴��� ���� ������� �����Ѵ�. 330 -> OpenGL 3.3 version
"layout (location = 0) in vec3 aPos;\n" // in : vertex shader�� �Է¹��� ��� vertex���� ���� (�ش� ���������� 1��)
										// layout(location=0) : �Է� ������ ��ġ�� ��ü������ ���� (??)
"void main()\n"
"{\n"
"   gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n" // �Է¹��� ���� ���� ��� ������ ���� (������ ����ȭ ������ ���ľ� ������ ������ �Է°��� ����ȭ ��ǥ�̴�)
"}\0";
// GLSL �����׸�Ʈ ���̴� �ڵ�
const char *fragmentShaderSource = "#version 330 core\n"
"out vec4 FragColor;\n" // out : ��µǴ� ���� �÷��� �����ϴ� ����
"void main()\n"
"{\n"
"   FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);\n" // ������������ ������
"}\n\0";

int main() {
	glfwInit(); // glfw �ʱ�ȭ
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3); // ������ ���� �ɼ�, �ɼ� ��
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 3);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);

#ifdef __APPLE__
	glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE); // �ƿ��� ����ؾ� ��
#endif

	// ������ ��ü ���� : ��� ������ ������ ����
	GLFWwindow* window = glfwCreateWindow(SCR_WIDTH, SCR_HEIGHT, "Triangle", NULL, NULL);
	if (window == NULL) {
		std::cout << "Failed to create GLFW window" << std::endl;
		glfwTerminate();
		return -1;
	}
	glfwMakeContextCurrent(window); // window�� ���� �������� �� ���ؽ�Ʈ�� ����
	glfwSetFramebufferSizeCallback(window, framebuffer_size_callback); // ������ ũ�Ⱑ �ٲ𶧸��� ȣ���� �Լ��� ���

	// GLAD�� OpenGL�� �Լ� �����͸� �����ϵ��� ����� ���� GLAD �ʱ�ȭ
	// glfwGetProcAddress : ������ �� OS�� ���� �ùٸ� �Լ��� �����ϴ� �Լ�
	if (!gladLoadGLLoader((GLADloadproc)glfwGetProcAddress)) {
		std::cout << "Failed to initialize GLAD" << std::endl;
		return -1;
	}


	// ���ؽ� ���̴��� ��Ÿ�ӿ� �������� �������ϴ� ����
	int vertexShader = glCreateShader(GL_VERTEX_SHADER); // ���̴� ����(���ؽ� ���̴� ����)
	glShaderSource(vertexShader, 1, &vertexShaderSource, NULL); // ���̴� �ҽ� �ڵ带 ������ ��ü�� ÷���Ѵ�
	glCompileShader(vertexShader); // ���̴��� ������

	// ���̴� ������ �� ������ �߻��ߴ��� �˻�
	int success;
	char infoLog[512];
	glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &success); // �������� �����ߴ��� Ȯ��
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


	// ������ ������ ���̴� ��ü�� �������� ����� �� �ִ� ���̴� ���α׷��� �����Ѵ�.
	// �� ���̴��� ����� ���� ���̴��� �Է¿� �����Ѵ�. ��ġ���� ������ ���� �߻�
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
	// ���ῡ �����ϸ� ������ ���� ���̴� ��ü�� �����Ѵ�.
	glDeleteShader(vertexShader);
	glDeleteShader(fragmentShader);

	// ���ؽ� ������ (OpenGL�� ��ֶ������ ��(Normalized Device Coordinates)�� ó���Ѵ�. �������� ȭ�鿡�� ����� ����)
	// vertex shader�� �Է����� ��������.
	float vertices[] = {
		-0.5f, -0.5f, 0.0f,
		0.5f, -0.5f, 0.0f,
		0.0f,  0.5f, 0.0f
	};

	// ���� ID�� ���� ���۸� ����
	unsigned int VBO, VAO;
	glGenVertexArrays(1, &VAO);
	glGenBuffers(1, &VBO);
	// Vertex Array Object(VAO) : �Ʒ����� ������ ���ؽ� �Ӽ��� �����, ���ؽ� �Ӽ��� ������ �� �ѹ��� ȣ���ؾ� �Ѵ�.
	// ���ؽ� �迭 ������Ʈ ���ε� -> ���ؽ� ���� ���ε� -> ���ؽ� �Ӽ� ����
	glBindVertexArray(VAO);

	// GL_ARRAY_BUFFER : vertex ���� ������Ʈ�� ���� Ÿ��
	glBindBuffer(GL_ARRAY_BUFFER, VBO); // VBO ���۸� GL_ARRAY_BUFFER ��� ���ε�
	// ����ڰ� ������ ������(vertices)�� ���ε�� ����(VBO)�� �����ϴ� �Լ�
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);  // ���� ����, ���� ũ��, ���� ������, �׷���ī�尡 �����͸� �����ϴ� ��� (GL_STATIC_DRAW : �����Ͱ� ���� �ٲ��� ����)

	// ���ؽ� ���̴��� ���� ������ �� �ش� �����͸� �ؼ��ϴ� ����� �������־�� �Ѵ�.
	// 1 : position vertex attribute�� ��ġ.  vertex shader���� layout location���� �������� ��
	// 2 : ���ؽ� ũ��(����)
	// 3 : ������ ����
	// 4 : �����͸� normalize�� ������ ���θ� ����
	// 5 : ���ؽ� ������ ������ ����(���⼭�� x,y,z�� �����ϴ� �� �������� ũ��� ����)
	// 6 : ��ġ �����Ͱ� ���ۿ��� ���۵Ǵ� ��ġ�� ������(??)
	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);
	glEnableVertexAttribArray(0); // ������ �Ӽ��� Ȱ��ȭ�ϴ� �Լ�(�⺻�� ��Ȱ��ȭ�Ǿ� ����)

	// VBO ���ε��� ����
	glBindBuffer(GL_ARRAY_BUFFER, 0);

	// VAO ���ε��� ����
	glBindVertexArray(0);


	// ����Ʈ, �� ���ۿ� ���� �׸��� ���� ��� ���
	glPolygonMode(GL_FRONT_AND_BACK, GL_LINE);

	// ���� ����� ������ ������ �����쿡 ����ؼ� �׸��� �۾�(������) �ݺ�
	while (!glfwWindowShouldClose(window)) {
		// input
		processInput(window); // �� �����Ӹ��� Ű �Է� Ȯ���� ���� ���� �Լ�

		// render
		glClearColor(0.2f, 0.3f, 0.3f, 1.0f); // �÷� ���ۿ� �ش� �÷��� ä��, ���� ���� �Լ�
		glClear(GL_COLOR_BUFFER_BIT); // �÷� ������ ���¸� ����, ���� ��� �Լ�

		glUseProgram(shaderProgram); // ������ ���� ���̴� ���α׷��� ���. ������ ��� ���̴�, ������ ȣ���� �ش� ��ü�� ����Ѵ�.
		glBindVertexArray(VAO); // ���ؽ� �Ӽ� ������ ���ε�
		// ���ؽ� �Ӽ��� ���ؽ� ������(VAO�� ���� ���������� ���ε���)�� ����Ͽ� �����쿡 �׸�
		glDrawArrays(GL_TRIANGLES, 0, 3); // �׷����� OpenGL primitive Ÿ��, ���ؽ� �迭�� ���� �ε���, ���ؽ� ����
		// glBindVertexArray(0); // ���ؽ� �Ӽ� ������ ���ε� ����, �Ź� �� �ʿ� ����.

		// glfw: swap buffer & poll IO event
		glfwSwapBuffers(window); // �� ���۸� ����Ʈ ���۷� ��ü
		glfwPollEvents(); // Ű���� �Է�, ���콺 �̵� ���� �̺�Ʈ�� Ȯ���ϰ� ������ ���� ������Ʈ
	}

	// �Ҵ�� ��ü���� �޸𸮸� ����
	glDeleteVertexArrays(1, &VAO);
	glDeleteBuffers(1, &VBO);

	glfwTerminate(); // �Ҵ�� GLFW�� ��� �ڿ��� ����
	return 0;
}



void processInput(GLFWwindow *window) {
	// ���� GLFW_KEY_ESCAPE Ű�� ������ �ִ��� Ȯ��
	if (glfwGetKey(window, GLFW_KEY_ESCAPE) == GLFW_PRESS)
		glfwSetWindowShouldClose(window, true);
}

void framebuffer_size_callback(GLFWwindow* window, int width, int height) {
	// 2D ��ǥ�� ȭ���� ��ǥ�� ��ȯ�ϴ� �Լ�
	// ex) 800x600 ���� (-0.5, 0,5)�� (200, 450)�� ���ε�
	glViewport(0, 0, width, height);
}