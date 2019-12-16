# Problem

Suppose an array sorted in ascending order is rotated at some pivot unknown to you beforehand.

(i.e.,  [0,1,2,4,5,6,7] might become  [4,5,6,7,0,1,2]).

Find the minimum element.

The array may contain duplicates.

#### Example 1:

```swift
Input: [1,3,5]
Output: 1
```

#### Example 2:

```swift
Input: [2,2,2,0,1]
Output: 0
```

#### Note:

1. This is a follow up problem to [Find Minimum in Rotated Sorted Array](/Leetcode/find_minimum_in_rotated_sorted_array.md)
2. Would allow duplicates affect the run-time complexity? How and why?

# My Answer
  
* 중간값을 이용해서 찾자.
* 만약 `i`와 `j`인덱스가 다른데 값이 같다면 `i`를 증가 시키자 `[2,4,5,1,2]` 이런 케이스때문에.
* 오름차순이기 때문에, `nums[i]`가 `nums[j]`보다 작거나 같으면 `nums[i]`가 제일 작은 값이다.
* 만약 그렇지 않다면, 중간값을 이용해서 범위를 줄이자.

```java
class Solution {
    public int findMin(int[] nums) {
        int i=0;
        int j=nums.length-1;

        while(i<=j){
            while(nums[i]==nums[j] && i!=j){
                i++;
            }

            if(nums[i]<=nums[j]){
                return nums[i];
            }

            int m=(i+j)/2;
            if(nums[m]>=nums[i]){
                i=m+1;
            }else{
                j=m;
            }
        }

        return -1;
    }
}
```

