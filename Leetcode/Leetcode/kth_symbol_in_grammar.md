# Problem

On the first row, we write a 0. Now in every subsequent row, we look at the previous row and replace each occurrence of 0 with 01, and each occurrence of 1 with 10.

Given row N and index K, return the K-th indexed symbol in row N. (The values of K are 1-indexed.) (1 indexed).

#### Example :

```swift
Input: N = 1, K = 1
Output: 0

Input: N = 2, K = 1
Output: 0

Input: N = 2, K = 2
Output: 1

Input: N = 4, K = 5
Output: 1

Explanation:
row 1: 0
row 2: 01
row 3: 0110
row 4: 01101001
```

# My Answer

* 이진 트리로 생각 할 수 있다.
* 부모노드가 0일때 왼쪽 자식 노드는 0, 오른쪽 자식 노드는 1
* 부모노드가 1일때 왼쪽 자식 노드는 1, 오른쪽 자식 노드는 0
* ![](../img/kth_tree.png)
* 만약 K가 짝수이면 `k/2`번째 노드의 오른쪽 노드이다.
* 만약 K가 홀수이면 `(k+1)/2` 번째 노드의 왼쪽 노드이다.
* 하지만, 부모노드의 값을 알아야 하기 때문에, 재귀로 부모 노드의 값이 뭔지 알아 낸다.

```java
class Solution {
    public int kthGrammar(int N, int K) {
        if ( N == 1 ) {     //N이 1이라는것은 루트노드 (0)을 의미 K는 의미 없다.
            return 0;  
        } else if ( K % 2 == 0 ) {  // K가 짝수라면, 이전 Row의 K/2의 값이 뭔지 알아내야 한다. 그것이 부모노드니까. 그리고, 부모노드가 0이라면 나는 1이고 1이라면 나는 0이다.
            return kthGrammar ( N -1, K / 2) == 0 ? 1 : 0;
        } else {      // K가 홀수라면, 이전 Row의 (K+1)/2의 값이 뭔지 알아내야 한다. 그것이 부모노드니까. 그리고, 부모노드가 0이라면 나는 0이고 1이라면 나는 1이다.
            return kthGrammar( N -1, (K + 1) /2) == 0 ? 0 : 1;       
        }
    }
}
```

