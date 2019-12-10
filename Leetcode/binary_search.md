# Problem

Given a sorted (in ascending order) integer array nums of n elements and a target value, write a function to search target in nums. If target exists, then return its index, otherwise return -1.

#### Example 1:

```swift
Input: nums = [-1,0,3,5,9,12], target = 9
Output: 4
Explanation: 9 exists in nums and its index is 4
```

#### Example 2:

```swift
Input: nums = [-1,0,3,5,9,12], target = 2
Output: -1
Explanation: 2 does not exist in nums so return -1
```

#### Note :

* You may assume that all elements in `nums` are unique.
* `n` will be in the range `[1,10000]`
* The value of each element in `nums` will be in the range `[-9999,9999]`.

# My Answer

* 재귀를 이용해서 해결
* `l`, `r`은 각각 `nums`에서 확인 해야할 시작 `index`와 마지막 `index`이다.
* `nums`가 이미 정렬되어있기 때문에, `l`과 `r`의 중간값이 찾으려는 `target`인지 확인해서 중간값 과 같다면 해당 index를, `target` 보다 크다면 `search`를 재귀 호출 하면서 `r`에 `pivot`값을 넘기면 된다. `target`보다 작다면, `l`에 `pivot+1`을 넘기자.

```java
class Solution {
    public int search(int[] nums, int target) {
        return search(nums, target, 0, nums.length);        
    }
    
    int search(int[] nums, int target, int l, int r) {        
        if ( l >= r )
            return -1;
        
        int pivot = (l + r) / 2;
        if ( nums[pivot] == target ) {
            return pivot;
        } else if ( nums[pivot] > target ) {
            return search(nums, target, l, pivot);
        } else {
            return search(nums, target, pivot + 1, r);
        }
    }
}
```

