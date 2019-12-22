# Problem

There are two sorted arrays nums1 and nums2 of size m and n respectively.

Find the median of the two sorted arrays. The overall run time complexity should be O(log (m+n)).

You may assume nums1 and nums2 cannot be both empty.

#### Example 1:

```swift
nums1 = [1, 3]
nums2 = [2]

The median is 2.0
```

#### Example 2:

```swift
nums1 = [1, 2]
nums2 = [3, 4]

The median is (2 + 3)/2 = 2.5
```
# What is Median

* `Median(중앙값)`은 하나의 집합을 두개의 각기 다른 집합으로 나누는 특성을 갖는 값이다.
* 작은 값들로만 이루어진 집합(`A`)와  큰 값들로만 이루어진 집합(`B`)로 나눈다.
* `A`집합의 가장 큰값은 `B`집합의 가장 작은 값보다 작다.
* `A`집합의 갯수와 `B`집합의 갯수는 동일하다.

# Solution
  
* `LeetCode`의 Solution 내용이다.
* `median(m)`을 찾기 위해,다음의 조건식을 만족한다.     
    1. $m \le n$    
    2. $A[0], A[1],..., A[i-1], A[i],..., A[m-1]$
    3. $B[0], B[1],..., B[j-1], B[j],..., B[n-1]$
    4. $i = 0 \sim m$
    5. $j = \frac{m+n+1}{2} - i$
    6. $B[j-1] \le A[i]$
    7. $A[i-1] \le B[j]$
    8. $max(A[i-1], B[j-1])$, when $m+n$ is odd
    9. $\frac{max(A[i-1], B[j-1]) + min(A[i],B[j])}{2}$, when $m+n$ is even    
* `m`이 `n`보다 작거나 같아야 한다. $m > n$가 되면, 5번 수식에 의해서 `j`가 음수가 나오기 때문이다. 따라서 수식에서 `A`는 `B`보다 갯수가 작거나 같다.
* 중앙값을 기준으로 좌, 우의 집합의 갯수가 같아야 하기 때문에, $m+n$이 짝수면 중간값을 산출해야 하고, 홀수면 중간값이 중앙값이다.

```java
class Solution {
    public double findMedianSortedArrays(int[] nums1, int[] nums2) {
        int m = nums1.length;
        int n = nums2.length;
        
        if ( m > n ) {
            int[] temp = nums1;
            nums1 = nums2;
            nums2 = temp;
            int t = m;
            m = n;
            n = t;           
        }
        
        int l=0;
        int r=m;
        int half = (m + n + 1)/2;
        
        while( l <= r ) {
            int i = (l+r)/2;
            int j = half - i;
            
            if ( i < r && nums2[j-1] > nums1[i]) {// nums1[i]가 너무 작기 때문에, 다음 값을 찾기 위해서 증가 시켜준다.
                l = i + 1;
            } else if ( i > l && nums1[i-1] > nums2[j]) {  //nums1[i-1]이 너무 크기 때문에, 다음 값을 찾기 위해서 감소 시켜준다.
                r = i - 1;
            } else {    //적절한 i를 찾았다.
                int max_left = 0;
                if ( i == 0 ) {
                    max_left = nums2[j-1];
                } else if ( j == 0 ) {
                    max_left = nums1[i-1];
                } else {
                    max_left = Math.max(nums1[i-1], nums2[j-1]);
                }
                
                if ( (m+n)%2 == 1) {
                    return max_left;
                }
                
                int min_right = 0;
                if (i == m) {
                    min_right = nums2[j];
                } else if (j == n) { 
                    min_right = nums1[i];
                } else { 
                    min_right = Math.min(nums2[j], nums1[i]);
                }

                return (max_left + min_right) / 2.0;
            }
        }
        
        return 0;
    }
}
```

