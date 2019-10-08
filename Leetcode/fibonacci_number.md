# Problem

The Fibonacci numbers, commonly denoted F(n) form a sequence, called the Fibonacci sequence, such that each number is the sum of the two preceding ones, starting from 0 and 1. That is,
```
F(0) = 0,   F(1) = 1
F(N) = F(N - 1) + F(N - 2), for N > 1.
```

Given N, calculate F(N).

#### Example 1:

```swift
Input: 2
Output: 1
Explanation: F(2) = F(1) + F(0) = 1 + 0 = 1.
```

#### Example 2:

```swift
Input: 3
Output: 2
Explanation: F(3) = F(2) + F(1) = 1 + 1 = 2.
```

# My Answer

* 중복 연산으로 인해서 `N`이 커질수록 연산시간이 오래 걸리기 때문에, 중간 결과값을 `m_cache`에서 기록 했다가 나중에 쓰자.
  
```java
class Solution {
    HashMap<Integer, Integer> m_cache = new HashMap<Integer, Integer>();
    
    public int fib(int N) {
        if ( N == 0 )
            return 0;
        
        if ( N == 1)
            return 1;
        
        if ( m_cache.containsKey(N))
            return m_cache.get(N);
        
        int result = fib(N-1) + fib(N-2);
        m_cache.put(N, result);
        return result;
    }
}
```

