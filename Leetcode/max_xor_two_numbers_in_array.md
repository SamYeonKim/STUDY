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
    public Trie one;
    public Trie zero;
    
    public void insert(int n) {
        Trie cur = this;
        
        for(int i=31;i>=0;i--) {
            int bit = ( n >> i) & 1;
            if ( bit == 0 && cur.zero == null ) {
                cur.zero = new Trie();
            } else if ( bit == 1 && cur.one == null ) {
                cur.one = new Trie();  
            }    
            
            cur = bit == 1 ? cur.one : cur.zero;
        }
    }
}

class Solution {
    public int findMaximumXOR(int[] nums) {
        Trie trie = new Trie();
        
        for(int n : nums) {
            trie.insert(n);
        }
        
        int max = Integer.MIN_VALUE;
        
        for(int n : nums) {
            Trie cur = trie;
            int max_xor = 0;
            for(int i=31;i>=0;i--) {
                int bit = ( n >> i) & 1;
                Trie next = bit == 1 ? cur.zero : cur.one;
                if ( next != null ) {
                    max_xor += 1 << i;
                    cur = next;
                } else {
                    cur = bit == 1 ? cur.one : cur.zero;
                }                
            }
            max = max > max_xor ? max : max_xor;
        }
        
        return max;
    }
}
```

