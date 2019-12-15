# Problem

Suppose an array sorted in ascending order is rotated at some pivot unknown to you beforehand.

(i.e.,  [0,1,2,4,5,6,7] might become  [4,5,6,7,0,1,2]).

Find the minimum element.

You may assume no duplicate exists in the array.

#### Example 1:

```swift
Input: [3,4,5,1,2] 
Output: 1
```

#### Example 2:

```swift
Input: [4,5,6,7,0,1,2]
Output: 0
```

# My Answer
  
* 중간값을 이용해서 찾자.
* `l`과 `r`을 이용해서 `중간 index(mid)`를 찾고 `min`과 `nums[mid]` 중 작은값을 `min`에 할당하자.
* 회전되어 있는 상황인지 확인 하기 위해서, 다음과 같은 조건검사를 한다.
    ```java
    1. nums[l] < nums[mid] && nums[r] > nums[mid] => l -> mid -> r => 최소 값이 mid 보다 앞에 있다.
    2. nums[l] < nums[mid] && nums[r] < nums[mid] => l -> mid <- l => 최소 값이 mid 보다 뒤에 있다.
    3. nums[l] > nums[mid] && nums[r] > nums[mid] => l <- mid -> r => 최소 값이 mid 보다 앞에 있다.
    4. nums[l] > nums[mid] && nums[r] < nums[mid] => l <- mid <- l => 최소 값이 mid 보다 뒤에 있다.
    ```

```java
class Solution {
    public int findMin(int[] nums) {
        int l=0;
        int r=nums.length-1;
        int min = Integer.MAX_VALUE;
        
        while( l <= r ) {
            int mid = (l + r)/2;
            
            min = Math.min(nums[mid], min);
            
            if ( nums[l] < nums[mid]) {
                if ( nums[r] > nums[mid]) {
                    r = mid - 1;
                } else {
                    l = mid + 1;
                }
            } else {
                if ( nums[r] > nums[mid]) {
                    r = mid - 1;                                        
                } else {
                    l = mid + 1;                    
                }
            }
        }
        
        return min;
    }
}
```

