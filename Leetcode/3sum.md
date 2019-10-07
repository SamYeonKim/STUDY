# Problem

Given an array nums of n integers, are there elements a, b, c in nums such that a + b + c = 0? Find all unique triplets in the array which gives the sum of zero.

#### Note:

* The solution set must not contain duplicate triplets.

#### Example 1:

```swift
Given array nums = [-1, 0, 1, 2, -1, -4],

A solution set is:
[
  [-1, 0, 1],
  [-1, -1, 2]
]
```

# My Answer

* 정렬
* nums를 순회돌면서 각 index를 기준으로 이진탐색을 수행
* 만약 이전에 했던 기준값이라면 continue
* [Explain](https://youtu.be/vm1dyVWfW6M)  

```java
class Solution {
    public List<List<Integer>> threeSum(int[] nums) {
        Arrays.sort(nums);
        
        List<List<Integer>> res = new ArrayList<>();
        
        for(int i = 0;i < nums.length - 2;i++) {        //한번에 3개를 가지고 체크하기 때문에 끝에서 3번째 까지만 돌면 된다.
            int base = nums[i];
            
            if ( i > 0 && nums[i-1] == base )   //이전의 기준값과 같은 값이면 할 필요 없다
                continue;
            
            int left = i + 1;
            int right = nums.length - 1;
            
            while( left < right ) {           

                int sum = base + nums[left] + nums[right];
                
                if ( sum == 0 ) {
                    res.add(Arrays.asList(base, nums[left], nums[right]));
                    
                    int orgLeft = nums[left];
                    int orgRight = nums[right];
                    
                    while( left < nums.length && orgLeft == nums[left] ) {  //그 다음 값을 찾았는데, 이전에 사용했던 값과 동일할 수 있다 다를때까지 Index 증가 시키자
                        left++;
                    }          
                    
                    while( right >= 0 && orgRight == nums[right] ) {   //그 다음 값을 찾았는데, 이전에 사용했던 값과 동일할 수 있다 다를때까지 Index 감소 시키자
                        right--;
                    }
                } else if ( sum > 0 ){  
                    right--;
                } else {
                    left++;
                }
            }
        }        
        return res;
    }
}
```

