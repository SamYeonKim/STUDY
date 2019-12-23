# Problem

Given an integer array, return the k-th smallest distance among all the pairs. The distance of a pair (A, B) is defined as the absolute difference between A and B.

#### Example :

```swift
Input:
nums = [1,3,1]
k = 1
Output: 0 
Explanation:
Here are all the pairs:
(1,3) -> 2
(1,1) -> 0
(3,1) -> 2
Then the 1st smallest distance pair is (1,1), and its distance is 0.
```

#### Note:

* 2 <= len(nums) <= 10000.
* 0 <= nums[i] < 1000000.
* 1 <= k <= len(nums) * (len(nums) - 1) / 2.

# Solution

* `lo`, `hi`는 각각 최소 거리와 최대 거리를 의미 한다. `nums`의 가장 마지막 원소에서 첫번째 원소를 뺀 만큼이 최대 거리이다.
* `for (int right = 0; right < nums.length; ++right)` 에서 현재 `중간거리(mi)`와 같거나 작은 거리의 갯수를 구한다. 예를들어 다음과 같은 흐름으로 계산 된다.
    ```java
    Input : [1,2,4,5], k=3
    lo=0, hi=5-1=4
    1:
        mi=(0+4)/2=2
        1-1:
            nums[1] - nums[0] = 2-1 = 1 < mi
            count += 1 - 0
        1-2:
            nums[2] - nums[0] = 4-1 = 3 > mi
            nums[2] - nums[1] = 4-2 = 2 = mi            
            count += 2 - 1
        1-3:
            nums[3] - nums[1] = 5-2 = 3 > mi
            nums[3] - nums[2] = 5-4 = 1 < mi
            count += 3 - 2
        count=3
        //mi=2 보다 같거나 작은 경우의 수
        //(1,2), (4,5), (2,4) = 3개
        
        count >= 3
        hi=2
    ```
* `count`가 `k`보다 같거나 크다면, `최대거리(hi)`에 `중간거리(mi)`를 할당하고 `k`보다 작다면 `최소거리(lo)`에 `중간거리(mi)` + 1을 할당 하자. 위 코드에서 이어서 수행하면 다음과 같이 계산된다.
    ```java
    Input : [1,2,4,5], k=3
    lo=0, hi=2
    1:
        mi=(0+2)/2=1
        1-1:
            nums[1] - nums[0] = 2-1 = 1 = mi
            count += 1 - 0
        1-2:
            nums[2] - nums[0] = 4-1 = 3 > mi
            nums[2] - nums[1] = 4-2 = 2 > mi            
            nums[2] - nums[2] = 4-4 = 0 < mi            
            count += 2 - 2
        1-3:
            nums[3] - nums[2] = 5-4 = 1 = mi
            count += 3 - 2
        count=2
        //mi=1 보다 같거나 작은 경우의 수
        //(1,2), (4,5) = 2개
        count<3
        lo=mi+1=2
    ```
* 최종적으로 `lo`가 `k`번째 작은거리이다.

```java
class Solution {
    public int smallestDistancePair(int[] nums, int k) {
        Arrays.sort(nums);

        int lo = 0;
        int hi = nums[nums.length - 1] - nums[0];
        while (lo < hi) {
            int mi = (lo + hi) / 2;
            int count = 0, left = 0;
            for (int right = 0; right < nums.length; ++right) {
                while (nums[right] - nums[left] > mi) {
                  left++;  
                } 
                
                count += right - left;
            }
            //count = number of pairs with distance <= mi
            if (count >= k) hi = mi;
            else lo = mi + 1;
        }
        return lo;
    }
}
```

