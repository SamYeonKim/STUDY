# Problem

Given an array of integers and an integer k, find out whether there are two distinct indices i and j in the array such that nums[i] = nums[j] and the absolute difference between i and j is at most k.

#### Example 1:

```swift
Input: nums = [1,2,3,1], k = 3
Output: true
```

#### Example 2:

```swift
Input: nums = [1,0,1,1], k = 1
Output: true
```

#### Example 3:

```swift
Input: nums = [1,2,3,1,2,3], k = 2
Output: false
```

# My Answer ( Use HashMap )

* `HashMap`을 이용하자.
* `nums`를 순회하면서, `nums[i]`를 `HashMap`의 키로, `index`를 값으로 할당하자.
* 만약 `nums[i]`가 `Hashmap`의 키로 존재하면서, 현재 `index` - 해당 값이 `k`보다 같거나 작으면 `true`
  
```java
class Solution {
    public boolean containsNearbyDuplicate(int[] nums, int k) {
        Map<Integer, Integer> hashmap = new HashMap<>();
        
        for(int i=0;i<nums.length;i++) {
            if ( hashmap.containsKey(nums[i])) {
                if ( i - hashmap.get(nums[i]) <= k ) {
                    return true;                
                } 
            }
            
            hashmap.put(nums[i], i);    
        }
        
        return false;
    }
}
```

# My Answer

* `index l,r`을 이용해서, `nums[l]`과 `nums[r]`을 비교해서 알아내자.
* 만약 `r-l`이 `k`랑 같거나, `r`이 `nums.length - 1`과 같다면, `l`을 증가 시키자.
* 그렇지 않다면 `r`을 증가 시키자.
* 이런식으로 비교 하다가 `nums[l]`과 `nums[r]`이 같아지면 `true`이다.

```java
class Solution {
    public boolean containsNearbyDuplicate(int[] nums, int k) {
        if ( k == 0 ) return false; 
        
        int l=0;
        int r=0;
        
        while( l < nums.length -1 ) {            
            if ( l != r && nums[l] == nums[r] ) return true;
            
            if ( r-l == k || r == nums.length -1 ) {
                l++;                
            } else {
                r++;
            }            
        }
        
        return false;
    }
}
```


