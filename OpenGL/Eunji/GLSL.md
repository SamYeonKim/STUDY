# Vertex Shader

```cpp
in vec4 a_position;
void main() {
   gl_Position = a_position;
}
```
* clipspace 좌표를 생성
* vertex마다 호출

## Data

* `Attributes` : 버퍼에서 가져온 데이터
* `Uniforms` : 단일 draw call에서 모든 정점에 대해 동일하게 유지되는값
* `Textures` : 픽셀/텍셀 데이터

### Attributes

* 버퍼 및 속성을 사용하는 방법

```cpp
// attribute 선언
attribute vec4 a_position;
void main() {
   gl_Position = a_position
}

// 정점 선언
float vertices[] = {
    // first triangle
    -0.9f, -0.5f, 0.0f,  // left 
    -0.0f, -0.5f, 0.0f,  // right
    -0.45f, 0.5f, 0.0f,  // top 
};

unsigned int VBO, VAO;

glGenVertexArrays(1, &VAO);
// 버퍼 생성
glGenBuffers(1, &VBO);
glBindVertexArray(VAO);

glBindBuffer(GL_ARRAY_BUFFER, VBO);
// 미리 정의된 정점 데이터를 버퍼의 메모리에 복사
glBufferData(GL_ARRAY_BUFFER, sizeof(vertices), vertices, GL_STATIC_DRAW);

// a_position의 index를 가져옴
int position = glGetAttribLocation(ShaderProgram, "a_position");
// 버퍼를 해석하는 방법
glVertexAttribPointer(position, 3, GL_FLOAT, GL_FALSE, 3 * sizeof(float), (void*)0);

glEnableVertexAttribArray(position);
```

### Uniforms

* shader에 전달되는 값으로 모든 vertex에서 동일하게 유지
* 응용프로그램에서 값 설정
  
```cpp
[vertex shader]
attribute vec4 a_position;
uniform vec4 u_offset; 
void main() {
   gl_Position = a_position + u_offset;
}

while (!glfwWindowShouldClose(window)) {
    glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
    glClear(GL_COLOR_BUFFER_BIT);

    // glUniform을 호출하기 전에 shader를 활성화 시켜야함 
    glUseProgram(shaderProgram);

    // uniform 갱신
    // 시간에 따라 위치가 바뀜
    float timeValue = glfwGetTime();
    float y_value = sin(timeValue) / 2.0f + 0.5f;

    // uniform 변수의 위치를 찾음
    int vertexValueLocation = glGetUniformLocation(shaderProgram, "u_position");
    // uniform 변수 값 지정
    glUniform4f(vertexValueLocation, 0.0f, y_value, 0.0f, 1.0f);
    
    // render the triangle
    glDrawArrays(GL_TRIANGLES, 0, 3);

    glfwSwapBuffers(window);
    glfwPollEvents();
}
```

* 배열, 구조체도 사용가능
```cpp
// 배열

[vertex shader]
layout (location = 0) in vec3 aPos;
uniform vec4 u_value[2]; 
void main() {
   gl_Position = vec4(aPos, 1.0) + u_value[0];
}

[fragment shader]
out vec4 FragColor;
uniform vec4 u_value[2]; 
void main() {
   FragColor = u_value[1];
}

while (!glfwWindowShouldClose(window)) {
    glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
    glClear(GL_COLOR_BUFFER_BIT);

    // glUniform을 호출하기 전에 shader를 활성화 시켜야함 
    glUseProgram(shaderProgram);

    // uniform 갱신
    // 시간에 따라 위치와 색이 바뀜
    float timeValue = glfwGetTime();
    float value = sin(timeValue) / 2.0f + 0.5f;
    // uniform 변수의 위치를 찾음(배열의 위치 각각 찾아야함)
    int vertexValue0Location = glGetUniformLocation(shaderProgram, "u_value[0]");
    int vertexValue1Location = glGetUniformLocation(shaderProgram, "u_value[1]");
    // uniform 변수 값 지정
    glUniform4f(vertexValue0Location, value, 0.0f, 0.0f, 1.0f);
    glUniform4f(vertexValue1Location, 0.0f, value, 0.0f, 1.0f);
    
    // render the triangle
    glDrawArrays(GL_TRIANGLES, 0, 3);

    glfwSwapBuffers(window);
    glfwPollEvents();
}
```
```cpp
// 구조체

[vertex shader]
struct SomeStruct {
  vec4 position;
  vec4 color;
};
layout (location = 0) in vec3 aPos;
uniform SomeStruct u_someThing; 
void main() {
   gl_Position = vec4(aPos, 1.0) + u_someThing.position;
}

[fragment shader]
struct SomeStruct {
  vec4 position;
  vec4 color;
};
out vec4 FragColor;
uniform SomeStruct u_someThing; 
void main() {
   FragColor = u_someThing.color;
}

while (!glfwWindowShouldClose(window)) {
    glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
    glClear(GL_COLOR_BUFFER_BIT);

    // glUniform을 호출하기 전에 shader를 활성화 시켜야함 
    glUseProgram(shaderProgram);

    // uniform 갱신
    // 시간에 따라 위치와 색이 바뀜
    float timeValue = glfwGetTime();
    float value = sin(timeValue) / 2.0f + 0.5f;
    // uniform 변수의 위치를 찾음(구조체 멤버 변수 위치 각각 찾아야함)
    int vertexValue0Location = glGetUniformLocation(shaderProgram, "u_someThing.position");
    int vertexValue1Location = glGetUniformLocation(shaderProgram, "u_someThing.color");
    
    // uniform 변수 값 지정
    glUniform4f(vertexValue0Location, value, 0.0f, 0.0f, 1.0f);
    glUniform4f(vertexValue1Location, 0.0f, value, 0.0f, 1.0f);
    
    // render the triangle
    glDrawArrays(GL_TRIANGLES, 0, 3);

    glfwSwapBuffers(window);
    glfwPollEvents();
}
```

# Fragment shader

* 픽셀의 색(out 변수)을 결정
* 픽셀마다 호출

## Data

* `Uniforms` : 한번 호출될 때 모든 픽셀에서 동일하게 유지되는 데이터
* `Textures` : 픽셀/텍셀에서 가져온 데이터
* `Varyings` : 버텍스 쉐이더의 값을 프레그먼트 쉐이더로 전달

### Textures

* `sampler2D` 유니폼을 생성하고 GLSL 함수 `texture`를 사용하여 값을 추출

```cpp
uniform sampler2D u_texture; 
out vec4 outColor;
 
void main() {
   vec2 texcoord = vec2(0.5, 0.5)  
   outColor = texture(u_texture, texcoord); // 텍스처 중간에 있는 값을 얻음
}
```

### Varyings

* `varying`을 사용하기 위해 버텍스와 프레그먼트 쉐이더 두 곳에서 `같은 타입과 이름`의 변수 선언
* `vertex shader`는 `out`변수를 선언하고 `fragment shader`에서는 `in` 변수 선언

```cpp
[vertex shader]
in vec4 a_position;
out vec4 v_value;
void main() {
    gl_Position = a_position;
    v_value = vec4(1.0, 1.0, 0.0, 1.0);
}

[fragment shader]
out vec4 FragColor;
in vec4 v_value;
void main() {
    FragColor = v_value;
}
```

## GLSL( Graphics Library Shader Language )

* 쉐이더에서 쓰이는 언어
* 수학적 계산을 수행하도록 설계
* `vector` : 값을 표현
  * `vec2` : float 값 2개 표현
  * `vec3` : float 값 3개 표현 
* `matrix` : 행렬을 표현
  * `mat2` : 2X2 행렬 포현
  * `mat3` : 3X3 행렬 표현
* `swizzling` : 벡터의 구성요소를 혼합해서 사용할 수 있음
```cpp
vec2 someVec;
vec4 differentVec = someVec.xyxx;
vec3 anotherVec = differentVec.zyw;
vec4 otherVec = someVec.xxxx + anotherVec.yxzy;

vec4 someVec;
someVec.wzyx = vec4(1.0, 2.0, 3.0, 4.0);
someVec.zx = vec2(3.0, 5.0);
```
* GLSL은 타입에 매우 엄격함
```cpp
float f = 1; // ERROR : 1은 int

float f = 1.0;      // float 사영
float f = float(1); // 캐스팅

vec3 v = vec3(1.0, 1.0, 1.0);
vec4 v2 = vec4(v.rgb, 1);       // vec4가 내부적으로 float(1)로 캐스팅 
```