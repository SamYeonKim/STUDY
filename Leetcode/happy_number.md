# Problem

Write an algorithm to determine if a number is "happy".

A happy number is a number defined by the following process: Starting with any positive integer, replace the number by the sum of the squares of its digits, and repeat the process until the number equals 1 (where it will stay), or it loops endlessly in a cycle which does not include 1. Those numbers for which this process ends in 1 are happy numbers.

#### Example 1:

```swift
Input: 19
Output: true
Explanation: 
1^2 + 9^2 = 82
8^2 + 2^2 = 68
6^2 + 8^2 = 100
1^2 + 0^2 + 0^2 = 1    
```

# My Answer
  
* `HashSet`을 이용하자.
* 자릿수마다 제곱수를 구한 후 전부 합하면 다음 반복수행때 사용할 `n`이 된다.
* `n`은 `HashSet`에 넣어 주자.
* 만약 `n`이 `HashSet`에 이미 있다는건, 반복 싸이클이 돈다는걸 의미 한다.
* 만약 `n`이 `1`이면 `HappyNumber`이다.

```java
class Solution {
    public boolean isHappy(int n) {
        Set<Integer> hashSet = new HashSet<>();
        
        while(!hashSet.contains(n)) {
            hashSet.add(n);
            n = getSquareSum(n);
            
            if ( n == 1 ) return true;
        }

        return false;
    }
    
    int getSquareSum(int n) {
        int res = 0;
        while( n > 0 ) {
            int digit = n % 10;
            res += digit * digit;
            n /= 10;
        }        
        return res;
    }
}
```

