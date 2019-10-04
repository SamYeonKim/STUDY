# Problem

Given two sorted integer arrays nums1 and nums2, merge nums2 into nums1 as one sorted array.

#### Note:

* The number of elements initialized in nums1 and nums2 are m and n respectively.
* You may assume that nums1 has enough space (size that is greater or equal to m + n) to hold additional elements from nums2.

#### Example:

```swift
Input:
nums1 = [1,2,3,0,0,0], m = 3
nums2 = [2,5,6],       n = 3

Output: [1,2,2,3,5,6]
```


# My Answer

* nums2를 순회 하면서 nums1에서 넣을 위치를 찾자.
* 넣을 위치를 찾았는데, nums1의 실제 요소들의 갯수 ( m + i) 이라면 그냥 넣고 끝
  * m + i 이라는 의미는 넣을 위치가 현재 요소들 보다 큰 숫자이기 때문에, 맨 뒤에 넣으면 된다는 뜻
* 위 조건이 아니라면, 실제 요소들의 끝부터 넣을 위치 까지의 값들을 하나씩 뒤로 밀어 주자.
  그러고 나서, 넣을 위치에 num2를 넣는다.
* sorted 이기때문에, 마지막 넣은 위치를 기억해 놓고, 다음에 넣을 위치 찾을땐 마지막 넣은 위치 부터 순회 돌면 된다.
  
```java
class Solution {
    public void merge(int[] nums1, int m, int[] nums2, int n) {
        int n_checked_idx = 0;      //마지막으로 num2를 삽입한 위치
        for(int i = 0; i<n; i++ ) {
            int num2 = nums2[i];

            for(int j = n_checked_idx; j < m; j++) {
                int num1 = nums1[j];
                if ( num1 > num2 ) {
                    n_checked_idx = j;
                    break;
                } else if ( j +1 == m ) {
                    n_checked_idx = m;
                }
            }

            if ( n_checked_idx < nums1.length ) {
                for(int j = m - 1; j >= n_checked_idx; j--) {
                    int temp = nums1[j];
                    nums1[j] = nums1[j+1];
                    nums1[j+1] = temp;
                }   
            }

            nums1[n_checked_idx] = num2;

            m++;
        }
        
    }
}
```

# Fastest Answer 

* nums1의 복사본을 하나 만든다 (tmp) 이후 nums1은 결과를 담을 배열로서 활용
* tmp와 nums2를 처음 부터 순회하는데, 각각 m과 n보다 작을때 까지 돌면서 작은 값을 nums1의 앞부터 채운다.
* 그 다음 첫번째 조건에 의해서 m 혹은 n 갯수만큼 못 할경우가 발생하기 때문에, 각각 tmp, nums2의 나머지 부분을 nums1에 채워준다.
  
```java
class Solution {
    public void merge(int[] nums1, int m, int[] nums2, int n) {
        int tmp[] = new int[m];
        System.arraycopy(nums1,0,tmp,0,m);
        int p =0,q=0,k=0;
        while(p<m && q <n){
            if(tmp[p] <= nums2[q]){
                nums1[k++] = tmp[p++];
            }else{
                 nums1[k++] = nums2[q++];
            }
        }
        //output : [1,2,2,3,0,0]
        while(p<m){
             nums1[k++] = tmp[p++];
        }
        //output : [1,2,2,3,0,0]
        while(q<n){
            nums1[k++] = nums2[q++];
        }
        //output : [1,2,2,3,5,6]
    }
}
```
