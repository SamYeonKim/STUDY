# Problem

Implement int sqrt(int x).

Compute and return the square root of x, where x is guaranteed to be a non-negative integer.

Since the return type is an integer, the decimal digits are truncated and only the integer part of the result is returned.

#### Example 1:

```swift
Input: 4
Output: 2
```

#### Example 2:

```swift
Input: 8
Output: 2
Explanation: The square root of 8 is 2.82842..., and since 
             the decimal part is truncated, 2 is returned.
```

# My Answer

* 소숫점은 알 필요 없으니까 제곱수 중에서 가장 가까운 정수를 구하면 된다.
* `l`은 타겟 까지의 작은 수, `r`은 타겟 까지의 가장 가까운 큰 수이다.
* `while`문에서는 `l + r`한 값을 오른쪽 쉬프트해서 중간 값을 찾는다. 예를 들어 다음과 같이 계산 된다.
    ```java
    l = 0 = 0000, r = 7 = 0111;
    l+r = 7 = 0111
    l+r >> 1 = 3 = 0011

    l = 1 = 0001, r = 9 = 1001;
    l+r = 10 = 1010
    l+r >> 1 = 5 = 0101
    ```
* 중간값을 제곱한것이 `x`와 같으면 정답이고, 만약 `x`보다 작다면, 더 큰 중간값을 찾아야 한다는 의미이기때문에, `l`을 증가시키고, `x`보다 크다면 `r`을 감소 시키면 된다.
    
```java
class Solution {
    public int mySqrt(int x) {
        if (x < 0) return -1;
        int l = 0;
        int r = x;
        while (l <=r) {
            int mid = (l+r)>>1;
            long midVal = (long) mid * mid;
            if (midVal == x) return mid;
            if (midVal < x) {
                l = mid + 1;
            } else {
                r = mid - 1;
            }
        }
        return r;
    }
}
```

