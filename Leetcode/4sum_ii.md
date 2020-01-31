# Problem

Given four lists A, B, C, D of integer values, compute how many tuples `(i, j, k, l)` there are such that `A[i] + B[j] + C[k] + D[l]` is zero.

To make problem a bit easier, all A, B, C, D have same length of N where 0 ≤ N ≤ 500. All integers are in the range of -228 to 228 - 1 and the result is guaranteed to be at most 231 - 1.

#### Example :

```swift
Input:
A = [ 1, 2]
B = [-2,-1]
C = [-1, 2]
D = [ 0, 2]

Output:
2

Explanation:
The two tuples are:
1. (0, 0, 0, 1) -> A[0] + B[0] + C[0] + D[1] = 1 + (-2) + (-1) + 2 = 0
2. (1, 1, 0, 0) -> A[1] + B[1] + C[0] + D[0] = 2 + (-1) + (-1) + 0 = 0
```

# My Answer

* 위 문제는 다음과 같은 수식으로 생각 할 수 있다. <img src="/Leetcode/tex/bd2e9faacd42abe5253c7fcc330671c0.svg?invert_in_darkmode&sanitize=true" align=middle width=289.24452644999997pt height=24.65753399999998pt/>
* 즉, <img src="/Leetcode/tex/d721c621deee18fca2aea5bba55769bd.svg?invert_in_darkmode&sanitize=true" align=middle width=153.06938459999998pt height=24.65753399999998pt/> 를 만족하는 케이스를 찾아야 한다.
* `A,B` 배열의 조합을 통해 나올 수 있는 `A+B`를 `HashMap`의 키로하고, 발생 횟수를 값으로 하자.
* `C,D` 배열의 조합을 통해 나올 수 있는 `-(C+D)`를 `HashMap`의 키로 사용되는것을 찾고 결과에 더해 주자.

```java
class Solution {    
    public int fourSumCount(int[] A, int[] B, int[] C, int[] D) {
        int count = 0;
        Map<Integer, Integer> hashmap = new HashMap<>();
        
        for( int a : A ) {
            for( int b : B ) {
                hashmap.put(a+b, hashmap.getOrDefault(a+b, 0) + 1);
            }
        }
        
        for( int c : C ) {
            for( int d : D ) {
                count += hashmap.getOrDefault(-c-d, 0);
            }
        }
        
        return count;
    }
}
```

