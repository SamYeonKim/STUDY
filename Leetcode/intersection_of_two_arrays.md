# Problem

Given two arrays, write a function to compute their intersection.

#### Note:

* Each element in the result must be unique
* The result can be in any order.

#### Example 1:

```swift
Input: nums1 = [1,2,2,1], nums2 = [2,2]
Output: [2]
```

#### Example 2:

```swift
Input: nums1 = [4,9,5], nums2 = [9,4,9,8,4]
Output: [9,4]
```

# My Answer 1

* `nums1`, `nums2` 배열을 오름차순으로 정렬 한다.
* 두개의 배열중 갯수가 적은 것을 기준으로 반복 돌면서 `nums2`에서 일치 하는것을 찾는다.
* 만약 이전 반복에서 체크했던 값과 같을경우는 넘어간다. 

```java
class Solution {
    public int[] intersection(int[] nums1, int[] nums2) {
        int[] result;
        if ( nums1.length < nums2.length ) {
            result = intersectionArray(nums1, nums2);
        } else {
            result = intersectionArray(nums2, nums1);
        }
        
        return result;
    }
    
    int[] intersectionArray(int[] nums1, int[] nums2) {
        List<Integer> result = new ArrayList<>();
        Arrays.sort(nums1);
        Arrays.sort(nums2);
        
        for(int i=0; i< nums1.length;i++) {
            if ( i > 0 && nums1[i] == nums1[i-1])
                continue;
            
            int l=0;
            int r=nums2.length-1;
            
            while(l < r ) {
                int mid = (l+r)/2;
                if ( nums2[mid] == nums1[i]) {
                    l = mid;
                    break;
                }
                
                if ( nums2[mid] < nums1[i]) {
                    l = mid + 1;
                } else {
                    r = mid - 1;
                }
            }
            
            if ( nums2[l] == nums1[i]) {
                result.add(nums1[i]);
            }
        }
        
        int[] array = new int[result.size()];
        int i = 0;
        for(int x : result)
            array[i++] = x;
        return array;
    }
}
```

# My Answer 2

* `i`,`j`,`k`를 각각 `nums1`의 index, `nums2`의 index, `temp_res`의 index이자 결과 리스트에 넣을 index 로 활용하자.
* `nums1`, `nums2` 배열을 오름차순으로 정렬 한다.
* `nums1[i] == nums2[j]`일 경우, 결과 리스트에 넣을 기본 조건에 만족 한다. 추가로, `k`가 0보다 클땐 `temp_res[k-1]`과 다른지 확인 필요하다. 중첩된 값을 결과 리스트에 넣을 수 없기 때문에.
* `nums1[i] <= nums2` 일땐 `i`를 증가 시키고, `nums1[i] >= nums2[j]` 일땐 `j`를 증가 시키자.
* 최종적으로 `temp_res`에 있는 값들중 `k`만큼만 새로운 결과 배열에 옮겨 담으면 된다.


```java
class Solution {
    public int[] intersection(int[] nums1, int[] nums2) {
        int i=0;
        int j=0;
        int k=0;
        int[] temp_res = new int[nums1.length];
        
        Arrays.sort(nums1);
        Arrays.sort(nums2);
        
        while(i < nums1.length && j < nums2.length ) {
            if ( nums1[i] == nums2[j] && (k == 0 || (k > 0 && temp_res[k-1] != nums1[i])) ) {
                temp_res[k++] = nums1[i];                
                i++;
                j++;
            } else if ( nums1[i] <= nums2[j]) {            
                i++;
            } else if ( nums1[i] >= nums2[j]) {
                j++;
            }
        }
        
        int[] res = new int[k];
        for(int t=0;t<k;t++) {
            res[t] = temp_res[t];
        }
        
        return res;        
    }    
}
```