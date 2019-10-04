# Problem

Given two arrays, write a function to compute their intersection.

#### Note:

* Each element in the result should appear as many times as it shows in both arrays.
* The result can be in any order.

#### Example 1:

```swift
Input: nums1 = [1,2,2,1], nums2 = [2,2]
Output: [2,2]
```

#### Example 2:

```swift
Input: nums1 = [4,9,5], nums2 = [9,4,9,8,4]
Output: [4,9]
```

# My Answer

* nums1, nums2 중에 갯수가 작은 것 기준으로 순회하자.
* 갯수가 작은것과 큰것을 따로 구분해서 lower, higher 라는 이름의 배열을 사용하자.
* 결과를 담을 리스트와 이미 체크한 higher의 index를 담을 Set을 선언하자
* lower 기준으로 순회 하면서 higher의 원소와 같은지 확인 하고, 이미 Set에 저장된 index라면 볼것도 없이 continue
  
```java
class Solution {
    public int[] intersect(int[] nums1, int[] nums2) {
        if ( nums1.length == 0) 
            return nums1;
        
        if ( nums2.length == 0)
            return nums2;
        
        int[] lower = nums1.length > nums2.length ? nums2 : nums1;
        int[] higher = nums1.length > nums2.length ? nums1 : nums2;
        
        ArrayList<Integer> arrayList = new ArrayList<>();
        Set<Integer> checked_index = new HashSet<>();      
       
        long n_checked_index = 0;
        for(int i = 0; i < lower.length; i++) {
            int n_1 = lower[i];
            for(int j = 1;j<= higher.length; j++) {
                if ( checked_index.contains(j))
                    continue;
                
                int n_2 = higher[j-1];
                if ( n_1 == n_2) {
                    arrayList.add(n_1);
                    checked_index.add(j);
                    break;
                }
            }
        }
                    		
        return arrayList.stream().mapToInt(Integer::intValue).toArray();
    }
}
```

# Fastest Answer

* nums1, nums2를 정렬, 1차 결과를 담을 배열(result)선언 사이즈는 nums1로 하든 nums2로 하든 상관없다.
* nums1, nums2를 둘다 다 순회 할때 까지 while
* 만약 같으면 result에 담고 i, j, k 증가.
* 만약 nums1이 더 작다면 i만 증가.
* 최종 결과를 담을 배열(res)선언 1차 결과 배열에서 중첩된 갯수만 최종 결과로 옮겨 담는다.

```java
class Solution {
    public int[] intersect(int[] nums1, int[] nums2) {
        int[] result = new int[nums1.length];
        
        int i = 0;
        int j = 0;
        int k = 0;
        Arrays.sort(nums1);
        Arrays.sort(nums2);
        
        while(i<nums1.length && j < nums2.length){
            if(nums1[i] == nums2[j]){
                result[k] = nums1[i];
                k++;
                i++;
                j++;
            }else if(nums1[i] < nums2[j]){
                i++;
            }else{
                j++;
            }
            
        }
        
        int[] res = new int[k];
        
        for(int l=0; l<k; l++){
            res[l] = result[l];
        }
        
        return res;
    }
}
```

