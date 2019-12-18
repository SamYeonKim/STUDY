# Problem

Given an array of integers that is already sorted in ascending order, find two numbers such that they add up to a specific target number.

The function twoSum should return indices of the two numbers such that they add up to the target, where index1 must be less than index2.

#### Example 1:

```swift
Input: numbers = [2,7,11,15], target = 9
Output: [1,2]
Explanation: The sum of 2 and 7 is 9. Therefore index1 = 1, index2 = 2.
```

#### Note:

* Your returned answers (both index1 and index2) are not zero-based.
* You may assume that each input would have exactly one solution and you may not use the same element twice.

# My Answer

* `l`, `r`은 각각 왼쪽 index, 오른쪽 index를 의미 한다.
* `numbers[l]`과 `numbers[r]`의 합(`sum`)이 `target`과 동일하면 정답이다.
* 만약 `sum`이 `target`보다 크다면, `r`을 하나 감소 시키자.
* 만약 `sum`이 `target`보다 작다면, `l`을 하나 증가 시키자.
    
```java
class Solution {
    public int[] twoSum(int[] numbers, int target) {
        int l=0;
        int r=numbers.length-1;
        
        int[] result = new int[2];
        
        while( l < r ) {
            int sum = numbers[l] + numbers[r];
            if ( sum == target ) {
                result[0] = l+1;
                result[1] = r+1;
                break;
            }
            
            if ( sum > target ) {
                r--;
            } else {
                l++;
            }
        }
        
        return result;
    }    
}
```

