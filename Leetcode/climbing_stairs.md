# Problem

You are climbing a stair case. It takes n steps to reach to the top.

Each time you can either climb 1 or 2 steps. In how many distinct ways can you climb to the top?

Given n will be a positive integer.

#### Example 1:

```swift
Input: 2
Output: 2
Explanation: There are two ways to climb to the top.
1. 1 step + 1 step
2. 2 steps
```

#### Example 2:

```swift
Input: 3
Output: 3
Explanation: There are three ways to climb to the top.
1. 1 step + 1 step + 1 step
2. 1 step + 2 steps
3. 2 steps + 1 step
```

# My Answer

* `n`이 `1`일때 결과는 `1`
* `n`이 `2`일때 결과는 `2`
* `n`이 `3`일때 결과는 `3`
* `n`이 `4`일때 결과는 `5`
* `n`이 `4`일때 결과는 `8`
* 위 처럼 결과가 나오기 때문에, [Fibonacci number](./fibonacci_number.md) 처럼 처리 할 수 있다.

```java
class Solution {
    HashMap<Integer, Integer> m_cache = new HashMap<Integer, Integer>();
    
    public int climbStairs(int n) {
        if ( n == 0 )    
            return 0;
        
        if ( n == 1 )
            return 1;
        
        if ( n == 2 )
            return 2;
        
        if ( m_cache.containsKey(n))
            return m_cache.get(n);
        
        int result = climbStairs(n -1) + climbStairs(n - 2);
        m_cache.put(n, result);
        
        return result;
    }
}
```

