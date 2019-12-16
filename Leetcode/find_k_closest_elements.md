# Problem

Given a sorted array, two integers k and x, find the k closest elements to x in the array. The result should also be sorted in ascending order. If there is a tie, the smaller elements are always preferred.

#### Example 1:

```swift
Input: [1,2,3,4,5], k=4, x=3
Output: [1,2,3,4]
```

#### Example 2:

```swift
Input: [1,2,3,4,5], k=4, x=-1
Output: [1,2,3,4]
```

#### Note:

1. The value k is positive and will always be smaller than the length of the sorted array.
2. Length of the given array is positive and will not exceed 10^4
3. Absolute value of elements in the array and x will not exceed 10^4

# My Answer
  
* `x`와 가장 가까운 연속된 원소 `k`개를 찾는 문제.
* 만약 첫번째 원소가 `x`와 같거나 크면 `arr[0~k]`가 정답
* 만약 마지막 원소가 `x`와 같거나 작으면 `arr[length-k~count]`가 정답
* 위 조건들을 만족 하지 않을 경우엔 다음과 같다.
  * `x`와 같거나 가장 가까운 값의 `index`를 찾는다.
  * `start`는 `0` 혹은 `index-k` 중에 큰 값으로 한다.
  * `end`는 `arr.length - 1` 혹은 `index+k`중에 작은 값으로 한다.
  * `start ~ end`의 갯수가 `k`가 될때 까지 반복한다.
  * 만약 `arr[end]-x`가 `x-arr[start]`보다 크면, 앞쪽이 더 `x`에 가깝다는 것이니 `end`를 하나 줄인다.
  * 만약 `arr[end]-x`가 `x-arr[start]`보다 작으면, 뒷쪽이 더 `x`에 가깝다는 것이니 `start`를 하나 늘린다.
  * 만약 `arr[end]-x`와 `x-arr[start]`가 같다면, 임의로 `end`를 하나 줄인다. 

```java
class Solution {
    public List<Integer> findClosestElements(int[] arr, int k, int x) {
        List<Integer> result = new ArrayList<>();        
        int count = arr.length;
        
        for(int i=0; i<count;i++) {
            result.add(arr[i]);
        }
        
        int start = 0;
        int end = count;
        
        if ( arr[0] >= x ) {
            result = result.subList(0, k);
        } else if ( arr[count - 1] <= x) {
            result = result.subList(count - k, count);
        } else {
            int index = searchIndex(arr, x);            
            start = Math.max(0, index-k);
            end = Math.min(count - 1, index+k);
            
            while( end-start+1 != k ) {
                if ( arr[end]-x > x - arr[start]) {
                    end--;
                } else if ( arr[end]-x < x - arr[start]) {
                    start++;
                } else {
                    end--;
                }
            }
            
            result = result.subList(start, end+1);
        }
        
        return result;
    }
    
    int searchIndex(int[] arr, int target) {
        int l=0;
        int h=arr.length-1;
        
        while( l < h ) {
            int mid = (l+h)/2;
            
            if ( arr[mid] == target ) {
                return mid;
            }
            
            if ( arr[mid] < target ) {
                l = mid+1;
            } else {
                h=mid;
            }
        }
        
        return l;
    }
}
```

# Fastest Answer

* 재귀를 이용
* 만약 `x-arr[mid]`가 `arr[mid+k]-x`보다 크면, 우측이 더 `x`에 가깝다는것이니 우측영역에서 다시 찾는다.
* 만약 `x-arr[mid]`가 `arr[mid+k]-x`보다 작거나 같으면, 좌측이 더 `x`에 가깝다는것이니 좌측영역에서 다시 찾는다.

```java
class Solution {
    public List<Integer> findClosestElements(int[] arr, int k, int x) {
        return find(arr, 0, arr.length - k, k, x);
    }
    
    private List<Integer> find(int[] arr, int left, int right, int k, int x) {
        if(left == right) {
            List<Integer> list = new ArrayList<>();
            for(int i = left; i < left + k; i++) list.add(arr[i]);
            return list;
        }
        int mid = (left + right) / 2;
        if(x - arr[mid] > arr[mid + k] - x)
            return find(arr, mid + 1, right, k, x);
        else
            return find(arr, left, mid, k, x);
    }
}
```

