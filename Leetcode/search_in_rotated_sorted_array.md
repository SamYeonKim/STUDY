# Problem

Suppose an array sorted in ascending order is rotated at some pivot unknown to you beforehand.

(i.e., [0,1,2,4,5,6,7] might become [4,5,6,7,0,1,2]).

You are given a target value to search. If found in the array return its index, otherwise return -1.

You may assume no duplicate exists in the array.

Your algorithm's runtime complexity must be in the order of O(log n).

#### Example 1:

```swift
Input: nums = [4,5,6,7,0,1,2], target = 0
Output: 4
```

#### Example 2:

```swift
Input: nums = [4,5,6,7,0,1,2], target = 3
Output: -1
```

# My Answer
  
* `O(log n)`의 복잡도를 따르기 위해선 배열의 모든 요소를 돌아선 안된다.
* 배열의 반절씩만 확인하고, 중간값이 찾는 `target`인지 확인해야 한다.
* `low`는 최소 `index`를, `high`는 최대 `index`를 의미, `while`문에서는 `low`와 `high`의 중간 `index (mid)`를 이용해서 해당 값이 `target`과 같은지 확인해서 같으면 끝.
* 같지 않으면, 오름차순으로 되어 있는 구간을 찾고, 그 구간에 찾는 `target`이 들어가는지에 따라서, 다음 `low`와 `high`가 결정난다.

```java
class Solution {
    public int search(int[] nums, int target) {
        int low = 0;
        int high = nums.length - 1;
        
        while (low <= high) {
            int mid = low + ((high - low) >> 1);
            
            if (nums[mid] == target) {
                return mid;
            }
            
            if (nums[mid] <= nums[high]) {      //mid ~ high가 오름차순으로 되어 있다.
                if (nums[mid] < target && target <= nums[high]) {   //target이 nums[mid]~nums[high]사이에 있는 값이라면 low를 mid+1로 하자.
                    low = mid + 1;
                } else {
                    high = mid - 1;     //target이 nums[mid]~[high]사이에 없다. 그럼 nums[low]~nums[mid-1]사이에 있다는 의미기 때문에 high를 변경 하자.
                }
            } else {
                if (nums[low] <= target && target < nums[mid]) {
                    high = mid - 1;
                } else {
                    low = mid + 1;
                }
            }
        }
        return -1;
    }
}
```

