# Problem

A peak element is an element that is greater than its neighbors.

Given an input array nums, where nums[i] ≠ nums[i+1], find a peak element and return its index.

The array may contain multiple peaks, in that case return the index to any one of the peaks is fine.

You may imagine that nums[-1] = nums[n] = -∞.

#### Example 1:

```swift
Input: nums = [1,2,3,1]
Output: 2
Explanation: 3 is a peak element and your function should return the index number 2.
```

#### Example 2:

```swift
Input: nums = [1,2,1,3,5,6,4]
Output: 1 or 5 
Explanation: Your function can return either index number 1 where the peak element is 2, 
             or index number 5 where the peak element is 6.
```

#### Note:

Your solution should be in logarithmic complexity.

# My Answer
  
* 중간값을 이용해서 찾자.
* `l`과 `r`을 이용해서 `중간 index(mid)`를 찾고 `nums[mid]`와 `nums[mid+1]`이 만족하는지 확인하자. 만족 한다면 `peek` 조건의 우측항은 만족 했다는 의미.
* 우측항이 만족했다면, 좌측항이 만족 하는지 확인 하기 위해서 `r`에 `mid`를 할당한다.
* 좌측항이 만족했다면, `l`에 `mid + 1`을 할당한다.
* 최종 결과는 `l`이 된다.
* 예를 들어 다음과 같은 흐름이 된다.
    ```java
    Input: nums = [1,2,3,1]
    1:
        l=0, r=3, mid=1,
        nums[mid]=2, nums[mid+1]=3,
        nums[mid] < nums[mid+1] = 좌측항 만족, 우측항 확인 위해 l=mid+1
    2:
        l=2, r=3, mid=2,
        nums[mid]=3, nums[mid+1]=1,
        nums[mid] > nums[mid+1] = 우측항 만족, 좌측항 확인 위해 r=mid
    3: 
        l=2, r=2, r이 l보다 크지 않기 때문에 반복 종료, 결과는 2
    ```

```java
class Solution {
    public int findPeakElement(int[] nums) {
        int l = 0;
        int r = nums.length - 1;
        
        while ( l < r ) {
            int mid = (l + r)>>1;
            
            if ( nums[mid] > nums[mid +1]) {
                r = mid;                
            } else {
                l = mid + 1;
            }             
        }
        
        return l;
    }
}
```

