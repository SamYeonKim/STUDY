# Problem

Given an array which consists of non-negative integers and an integer m, you can split the array into m non-empty continuous subarrays. Write an algorithm to minimize the largest sum among these m subarrays.

#### Example :

```swift
Input:
nums = [7,2,5,10,8]
m = 2

Output:
18

Explanation:
There are four ways to split nums into two subarrays.
The best way is to split it into [7,2,5] and [10,8],
where the largest sum among the two subarrays is only 18.
```

#### Note:

If *n* is the length of array, assume the following constraints are satisfied.

* $1 \le n \le 1000$
* $1 \le m \le min(50,n)$

# My Answer

* 중간값을 이용해서 해결
* `l`, `r`은 각각 `subarray`의 최대 값과, 전체 배열의 최대값을 의미 한다.
* 만약 `m==1` 이라면, 전체 배열을 의미 하니까 `r`이 정답.
* 만약 `m==nums.length` 라면, 원소 하나하나가 `subarray`라는 의미니까 `l`이 정답.
* `m`으로 분할한 `각 subarray의 총합(s)`의 범위는 $l \le s \le r$이 되고, 이중 `m`으로 분할되면서 작은 값을 찾으면 된다. 위 예제를 다음과 같은 흐름으로 찾을 수 있다.
    ```java
    nums = [7,2,5,10,8], m = 2
    l=10, r=7+2+5+10+8=32, s=10~32    
    1:
        mid=(10+32)/2=21      
        [7,2,5][10,8]
        subarray 갯수가 7+2+5 에서 1, 10+8 에서 2가 되면서 subarray의 합이 21이 안되기 때문에
        r=21
    2:
        mid=(10+21)/2=15
        [7,2,5][10][8]
        subarray 갯수가 7+2+5 에서 1, 10에서 2, 8에서 3이 되면서 m 보다 커져서
        l=16
    3:
        mid=(16+21)/2=18
        [7,2,5][10,8]
        subarray 갯수가 7+2+5 에서 1, 10+8 에서 2가 되면서 subarray의 합이 18이 안되기 때문에
        r=18
    4:
        mid=(16+18)/2=17
        [7,2,5][10,8]
        subarray 갯수가 7+2+5 에서 1, 10에서 2, 8에서 3이 되면서 m 보다 커져서
        l=18
    l과 r이 동일해 졌기 때문에, 반복 종료 정답은 18         
    ```
  
```java
class Solution {
    public int splitArray(int[] nums, int m) {
        int l = 0;
        int r = 0;
        
        for(int n : nums) {
            l = Math.max(l, n);
            r += n;
        }
        
        if ( m == 1 )
            return r;
        
        if ( nums.length == m )
            return l;
        
        while ( l < r ) {
            int mid = (l+r)/2;
            
            if ( canSplit(nums, m, mid)) {
                r = mid;
            } else {
                l = mid + 1;
            }
        }
        
        return l;
    }
    
    boolean canSplit(int[] nums, int m, int max ) {
        int split_count = 1;
        int sum = 0;
        
        for(int n : nums ) {
            sum += n;
            
            if ( sum > max ) {
                sum = n;
                split_count++;
                
                if ( split_count > m ) {
                    return false;
                }
            }
        }        
        return true;
    }
}
```

