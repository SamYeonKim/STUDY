# Problem

Given an array of integers, find out whether there are two distinct indices i and j in the array such that the absolute difference between nums[i] and nums[j] is at most t and the absolute difference between i and j is at most k.

#### Example 1:

```swift
Input: nums = [1,2,3,1], k = 3, t = 0
Output: true
```

#### Example 2:

```swift
Input: nums = [1,0,1,1], k = 1, t = 2
Output: true
```

#### Example 3:

```swift
Input: nums = [1,5,9,1,5,9], k = 2, t = 3
Output: false
```

#### Note:
* $j-i \le k$
* $| nums[j]-nums[i] | \le t$

# My Answer

* index `i`와 `j`의 차가 `k`보다 크지 않도록 유지 한다.
* `nums[i]` 와 `nums[j]`의 차가 `t`보다 크지 않도록 유지 한다.
* 만약 위 조건을 만족하는 `i`가 있다면 `true`
* `i~j <= k` 범위에 있는 index `l`, `r`을 이용하자.
* `l`과 `r`이 같지 않으면서, `nums[r]-nums[l] <= t`를 만족한다면 `true`이다.
* 만약 `r-l == k`라면 `l`을 증가 시켜야 한다. 왜냐 하면, `i~j <= k` 조건을 만족해야 하기 때문.
* 만약 `r == nums.length -1`라면 `l`을 증가 시켜야 한다. 왜냐 하면, `nums[r]-nums[l] <= t`을 구하려면 `r`이 `nums`의 범위를 넘어 가선 안되기 때문이다.
* `l`이 증가 될때, `t`가 0이 아니라면 `r`을 `l+1`로 변경해줘야 한다. 왜냐하면, 비교해야 하는 케이스를 건너띌수 있기 때문이다. 예를들어 다음과 같이 될 수 있다.
    ```java
    예를 들어
    Input: nums = [1,8,9,5], k = 3, t = 0

    2-1 <= 3
    nums[2]-nums[1] == 9-8 == 1 의 케이스가 있기 때문에 true이다. 하지만 r을 l+1로 해주는 코드가 없다면 다음과 같은 흐름이 된다.
    
    1:
        l=0,r=0
    2:
        l=0,r=1
    3:
        l=0,r=2
    4:
        l=0,r=3
    5:
        l=1,r=3
    6: 
        l=2,r=3

    2, 1을 비교 하는 케이스가 없어지기 때문에 false로 나온다.    
    ``` 
  
```java
class Solution {
    public boolean containsNearbyAlmostDuplicate(int[] nums, int k, int t) {
        if ( k == 0 )
            return false;
        
        int l=0;
        int r=0;
        
        while( l < nums.length -1 ) {
            if ( r != l && Math.abs((long)nums[r] - (long)nums[l]) <= t )
                return true;
            
            if ( r-l == k || r == nums.length - 1 ) {
                l++;
                
                if(t!=0)
                    r=l+1;                
            } else {
                r++;
            }            
        }
        
        return false;
    }
}
```

