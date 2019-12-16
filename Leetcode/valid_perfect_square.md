# Problem

Given a positive integer num, write a function which returns True if num is a perfect square else False.

#### Example 1:

```swift
Input: 16
Output: true
```

#### Example 2:

```swift
Input: 14
Output: false
```

#### Note:

Do not use any built-in library function such as sqrt.

# My Answer
  
* 특정 수의 제곱수 인지 확인 하는 문제.
* `low`, `high`의 중간값 `mid`를 구한다.
* `num`을 `mid`로 나눈값이 `mid`보다 작을 경우엔 `num`의 제곱근이 `mid`보다 크다는 것이기 때문에, `low`를 `mid+1`값으로 할당한다.
* `num`을 `mid`로 나눈값이 `mid`보다 클 경우엔 `num`의 제곱근이 `mid`보다 작다는 것이기 때문에, `high`를 `mid-1`값으로 할당한다.
* 최종적으로 `low`는 `num`의 제곱근이거나, 가장 가까운 수가 된다.

```java
class Solution {
    public boolean isPerfectSquare(int num) {
        int low=1;
        int high=num;
        
        while(low <= high) {
            int mid = low + ((high-low)/2);
            
            if ( mid < num/mid) {
                low=mid+1;
            } else {
                high=mid-1;
            }
        }
        
        return low*low == num;
    }
}
```