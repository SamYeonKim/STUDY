# Problem

Given an array nums containing n + 1 integers where each integer is between 1 and n (inclusive), prove that at least one duplicate number must exist. Assume that there is only one duplicate number, find the duplicate one.

#### Example 1:

```swift
Input: [1,3,4,2,2]
Output: 2
```

#### Example 2:

```swift
Input: [3,1,3,4,2]
Output: 3
```

#### Note:

* You must not modify the array ( assume the array is read only )
* You must use only constant. O(1) extra space
* Your runtime complexity should be less than O(n^2)
* There is only one duplicate number in the array, but it could be repeated more than once


# My Answer
  
* `중간값(mid)`를 이용해서 해결
* `nums`에 있는 모든 요소의 범위는 `1~n`이다.
* `nums`에 있는 요소들은 특정 값 `x`보다 같거나 작은것의 갯수는 중복 되는 것이 없을때 `x`가 나와야 한다.
    > [1,2,3,4], if `x`==2 일때 [1,2]=2, if `x`==3 일때 [1,2,3]
* `nums`에 있는 요소들은 특정 값 `x`보다 같거나 작은것의 갯수는 중복 되는 것이 있을때 `x`보다 큰 값이 나와야 한다.
    > [1,2,2,3], if `x`==2 일때 [1,2,2]=3, if `x`==1 일때 [1]
* 위 조건대로라면, `mid`를 `x`라고 했을때 `x`보다 작거나 같은것의 갯수가 `x`보다 큰지 작은지에 따라 `mid`의 값을 변경해 주면 된다.

```java
class Solution {
    public int findDuplicate(int[] nums) {
        int l=0;
        int r=nums.length-1;
        
        while(l<r){
            int mid = (l+r)/2;
            int count=0;    
            
            for(int n : nums) {
                if ( n <= mid )
                    count++;         
            }
            
            if ( count > mid ) {
                r--;
            } else {
                l++;
            }            
        }
        
        return l;        
    }
}
```

# Fastest Answer

* `nums`에 있는 모든 요소의 범위는 `1~n`이다.
* `slow`, `fast`는 각각 `늦은 index`와 `빠른 index`를 의미 한다.
* 첫번째 `do~while` 문에서 `slow`는 순차 대로 접근 하고, `fast`는 `slow` 보다 2배 빠른 접근 한다. 그래서, `slow`와 `fast`를 동일한 빠르기로 접근할 수 있는 값을 찾는다. 예를 들어 다음과 같은 흐름이다.
    ```
    Input: [1,3,4,2,2]
    slow = 0, fast = 0
    1:
        slow = nums[0] = 1
        fast = nums[nums[0]] = nums[1] = 3
    2:
        slow = nums[1] = 3
        fast = nums[nums[3]] = nums[2] = 4
    3:
        slow = nums[3] = 2
        fast = nums[nums[4]] = nums[2] = 4
    4:
        slow = nums[2] = 4
        fast = nums[nums[4]] = nums[2] = 4
    slow와 fast가 같아졌기 때문에 종료
    ```
* 두번째 `while`문에선 `slow`와 `fast` 둘다 동일한 순차 접근으로 동일한 값을 찾는다. 예를 들어 다음과 같은 흐름이다.
    ```
    Input: [1,3,4,2,2]
    위의 do~while을 통해 fast가 4로 결정났다.
    slow = 0, fast = 4
    1:
        slow = nums[0] = 1
        fast = nums[4] = 2
    2:
        slow = nums[1] = 3
        fast = nums[2] = 4
    3:
        slow = nums[3] = 2
        fast = nums[4] = 2
    slow와 fast가 같아졌기 때문에 종료
    ```

```java
class Solution {
    public int findDuplicate(int[] nums) {
        int slow=0;
        int fast=0;
        
        do {
            slow = nums[slow];
            fast = nums[nums[fast]];
        } while(slow != fast);

        slow = 0;
        
        while(slow != fast ) {
            slow = nums[slow];
            fast = nums[fast];
        }
        
        return slow;
    }
}
```

