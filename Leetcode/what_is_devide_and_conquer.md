#### Definition

재귀의 형태로 구현되는데 2개 이상의 작은 형태로 분할 해서 해결하는 방식

#### Flow

1. `Divide` : 문제 <img src="https://render.githubusercontent.com/render/math?math=S"> 를 다음과 같이 여러개의 서브 문제로 분할 한다. <img src="https://render.githubusercontent.com/render/math?math=\{S_1,S_2,...S_n\}, n \geq 2">
2. `Conquer` : 재귀적으로 서브 문제들을 해결한다.
3. `Combine` : 각 서브 문제들의 결과 값을 합친다.



