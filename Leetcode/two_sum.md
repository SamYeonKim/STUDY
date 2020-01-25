# Problem

Given an array of integers, return indices of the two numbers such that they add up to a specific target.

You may assume that each input would have exactly one solution, and you may not use the same element twice.

#### Example :

```swift
Input: [2, 7, 11, 15], target = 9
Output: [0, 1]
Explanation: nums[0] + nums[1] = 2 + 7 = 9
```

# My Answer
  
* `HashMap`을 이용하자.
* `nums`를 순회하면서, target을 만들 수 있는 값을 찾자.
* `target-nums[i]`가 `nums[i]`를 이용해서 `target`을 만들 수 있는 값이고, 이것이 `HashMap`에 있다면 정답.

```java
class Solution {
    public int[] twoSum(int[] nums, int target) {        
        Map<Integer, Integer> map = new HashMap<>();
        for (int i = 0; i < nums.length; i++) {
            int complement = target - nums[i];
            if (map.containsKey(complement)) {
                return new int[] { map.get(complement), i };
            }
            map.put(nums[i], i);
        }
        
        return null;
    }
}
```

