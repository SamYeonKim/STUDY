# Problem

Given an array of integers nums sorted in ascending order, find the starting and ending position of a given target value.

Your algorithm's runtime complexity must be in the order of O(log n).

If the target is not found in the array, return [-1, -1].

#### Example 1:

```swift
Input: nums = [5,7,7,8,8,10], target = 8
Output: [3,4]
```

#### Example 2:

```swift
Input: nums = [5,7,7,8,8,10], target = 6
Output: [-1,-1]
```

# My Answer
  
* `searchIndex`함수를 이용해서 `target`과 일치 하는 `최소 index(l)`, `target`과 일치하는 `최대 index(r)`을 찾자.
* 만약 `l`이 `nums.length`와 같다는 것은 `target`을 찾지 못했고, `target`보다 작은 값들만 있다는 것이고, `nums[l]`이 `target`과 다르다는것은 `target`을 찾지 못했고, 큰 값과 작은 값이 섞여 있었다는 것이다.
* 위와 같은 상황이 아니라면, `r`을 찾자.
* `searchIndex`에선 `l`을 찾을땐 `nums[mid]` 가 `target`보다 크거나, 같으면 `high`를 줄여서 작은 범위에서 더 찾는다.
* `searchIndex`에선 `r`을 찾을땐 `nums[mid]` 가 `target`과 같거나, 작을때 `low`를 늘려서 큰 범위에서 더 찾는다.
* 예를 들어 다음과 같은 흐름으로 진행된다.
    ```java
    Input: nums = [5,7,7,8,8,10], target = 8
    l:
        1:
            low=0,high=6,mid=3,nums[mid]=8
            left && nums[mid] == target
                high=3
        2:
            low=0,high=3,mid=1,nums[mid]=7
            nums[mid] < target
                low=mid+1=2
        3:
            low=3,high=3, 반복 종료
    l이 3이고 nums[3]=8 == target 이기 때문에 r 찾기 수행
    r:
        1:
            low=0,high=6,mid=3,nums[mid]=8
            nums[mid] == target
                low=mid+1=4
        2:
            low=4,high=6,mid=5,nums[mid]=10
            nums[mid] > target
                high=5
        3:
            low=4,high=5, mid=4, nums[mid]=8
            nums[mid] == target
                low=mid+1=5
        4:
            low=5, high=5, 반복 종료
    searchIndex의 결과 값이 5인데 여기서 1을빼서 r은 4
    ```

```java
class Solution {
    public int[] searchRange(int[] nums, int target) {
        int[] result = {-1, -1};
        
        int l = searchIndex(nums, target, true);
        
        if ( l == nums.length || nums[l] != target ) {
            return result;
        }
        
        result[0] = l;
        result[1] = searchIndex(nums, target, false) - 1;
        
        return result;
    }
    
    int searchIndex(int[] nums, int target, boolean left ) {
        int low = 0;
        int high = nums.length;
        
        while( low < high ) {
            int mid = (low + high)>>1;
            
            if ( nums[mid] > target || ( left && nums[mid] == target )) {
                high = mid;
            } else {
                low = mid+1;
            }
        }
        
        return low;
    }
}
```

