* 대칭키 암호화
  * 단일키 사용
  * 키가 임의로 지정되고, 길이는 보안 수준에 따라 128비트 or 256비트
  * 훨씬 빠르고, 적은 연산만으로도 가능 하지만 키가 공개되면 무용지물
  * 대표적인 예로 AES가 있음 
* 비대칭 암호화
  * 서로 다른 2개의 키 사용 ( 공개키, 개인키)
  * 2048비트 길이의 키를 보통 사용
  * 공개키와 개인키 사이에 수학적 패턴이 존재
  * 대칭키 암호화에 비해 속도가 현전히 느리고, 상대적으로 많은 연산이 필요
-----
* SSL-TLS
  * 인터넷 통신할때 주고 받는 데이털르 보호하기 위한 표준화된 암호화 프로토콜
  * Transport Layer에 적용되는 방식이라서 HTTP, FTP 등 Application Layer프로토콜의 종류에 상관없이 사용 할 수 있다.
  * 기본적으로 인증, 암호화, 무결성 지원
  * 인증을 위한 Handshake 과정이 필요

  