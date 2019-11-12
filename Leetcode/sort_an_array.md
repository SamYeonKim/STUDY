# Problem

Given an array of integers nums, sort the array in ascending order

#### Example 1:

```swift
Input: nums = [5,2,3,1]
Output: [1,2,3,5]
```

#### Example 2:

```swift
Input: nums = [5,1,1,2,0,0]
Output: [0,0,1,1,2,5]
```

#### Constraints

```java
1 <= nums.length <= 50000
-50000 <= nums[i] <= 50000
```

# My Answer

* `Divide and Conquer`에 충실하게 작성
* `Sort`함수에서 정렬과 중간 `index`를 이용해서 `left`, `right` 리스트를 나눠서 따로 정렬 하도록 한다.
* 마지막에 하나의 리스트에 정렬된 리스트를 다시 정렬해서 넣는다.
* 미친듯이 `List<Integer>` 객체를 만들어 내서 문제가 많다.

```java
class Solution {
    public List<Integer> sortArray(int[] nums) {
        if ( nums == null || nums.length == 0 )
            return null;
        
        List<Integer> l_nums = new ArrayList<Integer>(nums.length);
        for(int i : nums) {
            l_nums.add(i);
        }
        
        List<Integer> result = Sort(l_nums);        
        return result;
    }
    
    List<Integer> Sort(List<Integer> list) {
        int length = list.size();
        
        if ( length == 1) {
            return list;
        }
        
        int pivot = length / 2;
        List<Integer> l_left = Sort(list.subList(0, pivot));
        List<Integer> l_right = Sort(list.subList(pivot, length));
        
        int l=0,r=0;
        int left_count = l_left.size();
        int right_count = l_right.size();        
        
        List<Integer> result = new ArrayList<Integer>(length);
        
        while (l < left_count || r < right_count ) {
            int left = Integer.MAX_VALUE;
            int right = Integer.MAX_VALUE;
            
            if ( l < left_count ) {
                left = l_left.get(l);
            }
            
            if ( r < right_count ) {
                right = l_right.get(r);
            }
            
            if ( left < right ) {
                result.add(left);
                l++;
            } else {
                result.add(right);
                r++;
            }            
        }
        
        return result;
    }
}
```

# Fastest Answer ( Not D&C )

* `Devide and Conquer` 방식을 사용하지 않고 문제에 충실했다.
* 문제의 제약조건에 의해서 `nums[i]`에 들어갈수 있는 수는 `-50000 ~ 50000` 이기 때문에, `100001`개의 배열을 만들어서 `nums[0] = -50000`을 의미 할 수있도록 했다.

```java
class Solution {
    public List<Integer> sortArray(int[] nums) {
        int[] count = new int[100001];
        
        for (int i: nums) {
            count[i+50000]++;       //정렬되지 않은 배열을 만들면서 i=nums[x] 에다가 50000을 더한다. 그리고 발생횟수를 증가 시킨다.
        }
        
        List<Integer> result = new ArrayList<>();
   
        for (int i=0; i<count.length; i++) {    //100001번 순회 한다.
            
            while(count[i]>0) { //동일한 값이 반복해서 들어가 있을 수 있기때문에, 현재 값이 0이 될때 까지 돈다.
                result.add(i - 50000); //nums에 있던 값을 50000 더해서 변형해 줬기 때문에, 다시 원상 복귀 시켜서 결과 리스트에 넣어 준다.
                count[i]--;
            }             
        }        
        return result;
    }
}
```

# Fastest Answer ( Use D&C )

* `Devide and Conquer`에 충실했다.

```java
class Solution {
    public List<Integer> sortArray(int[] nums) {
        sort(nums, 0, nums.length - 1);
        List<Integer> result = new ArrayList<>();
        
        for (int num : nums) {
            result.add(num);
        }
        
        return result;
    }
    
    private void sort(int[] nums, int lo, int hi) {
        if (lo >= hi) {
            return;
        }
        
        int pIdx = partition(nums, lo, hi);
        
        sort(nums, lo, pIdx - 1);
        sort(nums, pIdx + 1, hi);
    }
    
    private int partition(int[] nums, int start, int end) {
        if (start >= end) {
            return start;
        }
        
        int pivot = nums[start];
        
        int j = start + 1;
        
        for (int i = start + 1; i <= end; i++) {
            if (nums[i] < pivot) {
                swap(nums, j++, i);
            }
        }
        
        swap(nums, start, j - 1);
        
        return j - 1;
    }
    
    private void swap(int[] nums, int i, int j) {
        int tmp = nums[i];
        nums[i] = nums[j];
        nums[j] = tmp;
    }
}
```
