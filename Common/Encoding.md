#### Unicode 

* 특정 값과 문자를 1:1 매핑 해 놓은 테이블
* ASCII 코드로는 표현 할 수 없는 다른 문자열을 표현하기 위해 고안됨


#### Encoding

* __UTF-8__
    * 8bit 기반, unicode 기반
    * 문자에 따라 필요한 바이트수가 다르다.
        * 1 byte : ASCII에 명시된 캐릭터 128개
        * 2 byte : 라틴 계열 혹은 그리스어, 히브리어 등등
        * 3 byte : 한자, 일본어, 한국어 등등
        * 4 byte : 기타 문자
* __UTF-16__
    * 16bit 기반, unicode 기반
    * UTF-8 처럼 문자에 따라 필요한 바이트수가 다르다.
        * 2 byte : [BMP](https://en.wikipedia.org/wiki/Plane_(Unicode)#Basic_Multilingual_Plane) ( 다국어 기본 평면, 거의 모든 근대 문자와 특수 문자가 포함됨 )
        * 4 byte : 그 밖의 문자
    * ANSI 와 호환 안됨
    * 한글을 2 byte 만으로 표현 할 수 있기 때문에, UTF-8로 된 파일보다 용량이 적음
    * Endian 처리가 반드시 필요함
* __ASCII__
    * 8bit 기반
    * 7bit 를 이용해서 문자 표현
    * 33개의 출력 불가능한 제어 문자들
    * 95개의 출력 가능한 영문 및 숫자, 공백 포함 기본 특수 문자들
    * 총 128개의 문자 표현
*  __ANSI__
    * 8bit 기반
    * 앞 7bit를 이용해서 문자를 표현하고 마지막 1bit를 `CodePage`로 사용  




