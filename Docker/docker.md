# Docker

## 이미지, 컨테이너

```
이미지 = 실행 파일.
컨테이너 = 이미지를 실행 시킨 상태.
```

## 이미지 파일 다운로드

`docker pull [이미지 이름]`

```
ex) docker pull jenkins
ex) docker pull ubuntu:14.04
ex) docker pull qdwer/test:jenkins_5
```

## 이미지 실행

`docker run [이미지 이름]`

`docker exec [이미지 이름]`

```
ex) docker run -i -t ubuntu:14.04 /bin/bash

-i : 사용자가 입출력을 하겠다는 의미.
-t : 가상 터미널 환경을 만들겠다는 의미.
/bin/bash : 컨테이너 내의 메인 실행파일로 bash를 실행하겠다는 의미.
```

## 이미지 삭제

`docker rmi [이미지 이름]`

```
ex) docker rmi qdwer/test:jenkins_5
```

## 컨테이너 상태 확인

`docker ps [-a]`

## 실행중인 컨테이너 정지

`docker stop [컨테이너 이름]`

```
ex) docker stop jenkins_11
```

## 컨테이너 제거

`docker rm [컨테이너 이름]`

## 이미지 생성

`docker commit [컨테이너 이름] [이미지의 이름]`

```
ex) docker commit jenkins_5 qdwer/test:jenkins_6
```

## 생성한 이미지를 docker hub에 올리기

`docker push [이미지 이름]`

```
ex) docker push qdwer/test:jenkins_6

주의) 태그 이름을 [저장소]:[태그]로 만들어야 저장소로 올라가진다.
```

# DockerFile

## DockerFile 제작

```
FROM ubuntu:12.04
MAINTAINER Gijung Lee <bst_pro@naver.com>

# Run Upgrade
RUN echo "업그레이드 중입니다."
RUN apt-get update

# Inatall Git
RUN echo "Git을 설치 중입니다."
RUN apt-get install -y git

# End
RUN echo "모든 것이 완료되었습니다."
```

`FROM [이미지 이름]`

```
어떤 이미지를 기반으로 아래의 구문을 실행 할지 지정 할 수 있다.
```

`MAINTAINER [생성, 관리자 이름]`

```
해당 파일의 관리자 명명 할 수 있다.
```

`RUN [명령어]`

```
Shell 명령어를 실행 할 수 있도록 한다.
```

`apt-get`

```
리눅스에서 사용되는 패키지 관리 명령어 도구이다.
```

## DockerFile 이미지 생성

`docker build [-t 이름] [Dockerfile 위치]`

```
ex) docker build -t qdwer/test:docker_git /Volumes/SL-BG1/Personal/A
```

# Registry

## Registry Image 설치

```
docker pull registry
```

## Registry 컨테이너 실행

```
docker run -d -p 5000:5000 -v /tmp/registry:/tmp/registry --name registry registry:latest
```

## Registry에 Push/Pull

```
docker push localhost:5000/registry_test

docker pull localhost:5000/registry_test
```

# Jenkins 실행

`docker run [-d] [-p] [포트] [-v] [--name] [-u] [이미지 이름]`

```
ex) docker run -d -p 8080:8080 -v jenkins_home:/var/jenkins_home --name jenkins_11 -u root qdwer/test:jenkins_5

-d : 데몬 상태로 실행한다는 뜻이다. 이 옵션을 주지 않으면, 실행되는 로그를 바로 보여준다.
-p : 컨테이너 내부의 포트를 외부로 내보낼 포트로 연결시켜준다.

-v : 호스트에 볼륨을 지정해 주는 것이다. 굳이 하지 않아도 되지만, 만약 해당 컨테이너가 삭제되면 내부에 작성했던 스크립트 등의 데이터가 다 없어지기 때문에 볼륨을 지정해 외부에 백업하는 용도로 볼륨을 잡았다.

--name : 해당 컨테이너의 이름을 지정해준다.

-u : root 사용자로 실행되게 하기 위해 지정해 줬다.
```

