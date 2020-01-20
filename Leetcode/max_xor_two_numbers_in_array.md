# Problem

Given a non-empty array of numbers, $a_0,a_1,a_2,...,a_{n-1}, where\ 0\le a_i < 2^{31}$

Find the maximum result of $a_i$ XOR $a_j$, where 0 ≤ i, j < n.

Could you do this in O(n) runtime?

#### Example :

```swift
Input: [3, 10, 5, 25, 2, 8]
Output: 28
Explanation: The maximum result is 5 ^ 25 = 28.
```

# My Answer

* `nums`의 숫자들을 `bit`표현으로 `Trie`를 구성해 놓는다. `int`는 32bit 이기 때문에, 32개로 구성.
* `nums`의 숫자들을 순회 하면서, 구성해 놓은 `Trie`와 반대되는 비트에 해당하는 자식을 다음 `Trie`노드로 사용한다.
    > 왜냐하면, `XOR`연산은 `bit`가 서로 다를때 `1`이고, 같을때 `0`이기 때문.
* 만약 다른 `bit`에 해당 하는 자식이 있다면, `max_xor`에 현재 비트 위치에 1을 할당한 값을 더한다. 
  
```java
class Trie {
    public Trie[] children;
    public Trie() {
        children = new Trie[2];
    }
    
    public void insert(int n) {
        Trie cur = this;
        
        for(int i=31;i>=0;i--) {        //int가 32비트 이기 때문에, 32bit로 표현한다.
            int bit = ( n >> i) & 1;    //현재 i번째 bit가 0인지 1인지 확인.
            if ( cur.children[bit] == null ) {     
                cur.children[bit] = new Trie();
            }
            cur = cur.children[bit];               
        }
    }
}

class Solution {
    public int findMaximumXOR(int[] nums) {
        Trie trie = new Trie();
        
        for(int n : nums) {     //nums에 있는 숫자들을 이용해서 Trie를 구성
            trie.insert(n);
        }
        
        int max = Integer.MIN_VALUE;
        
        for(int n : nums) {
            Trie cur = trie;
            int max_xor = 0;
            for(int i=31;i>=0;i--) {        //Trie를 구성할때 32비트로 했기 때문에 마찬가지로 32비트로 계산
                int bit = ( n >> i) & 1;
                
                if ( cur.children[bit == 1 ? 0 : 1] != null ) {     //XOR은 bit가 서로 다를때 1이기 때문에, 현재 bit와 반대의 자식이 있을경우를 확인
                    max_xor += 1 << i;      //XOR로 인해 현재 i번째 비트가 1이 될것이니, max_xor에 i번째 비트가 1일때의 값을 더한다.
                    cur = cur.children[bit == 1 ? 0 : 1] ;
                } else {
                    cur = cur.children[bit];
                }
            }
            max = max > max_xor ? max : max_xor;
        }
        
        return max;
    }
}
```

