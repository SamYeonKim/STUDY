# Problem

Given a list of sorted characters letters containing only lowercase letters, and given a target letter target, find the smallest element in the list that is larger than the given target.

Letters also wrap around. For example, if the target is target = 'z' and letters = ['a', 'b'], the answer is 'a'.

#### Note:

1. `letters` has a length in range `[2, 10000]`.
2. `letters` consists of lowercase letters, and contains at least 2 unique letters.
3. `target` is a lowercase letter.

#### Example:

```swift
Input:
letters = ["c", "f", "j"]
target = "a"
Output: "c"

Input:
letters = ["c", "f", "j"]
target = "c"
Output: "f"

Input:
letters = ["c", "f", "j"]
target = "d"
Output: "f"

Input:
letters = ["c", "f", "j"]
target = "g"
Output: "j"

Input:
letters = ["c", "f", "j"]
target = "j"
Output: "c"

Input:
letters = ["c", "f", "j"]
target = "k"
Output: "c"
```


# My Answer

* `low`, `high` index를 이용해서 중간값 `mid`를 찾고 `target`보다 큰 문자 인지 확인
* 만약 `letters[mid]`가 `target`보다 크면, `high`에 `mid`를 할당
* 만약 `letters[mid]`가 `target`과 같거나 작으면, `low`에 `mid+1`을 할당한다.
* `letters[low]`가 정답인데, `letters[low]`가 `target`과 같거나 작은 경우가 발생할 수 있기 때문에 다음과 같이 보정해 준다.
  * `low`를 하나 증가 시킨다.
  * 만약 `low`가 `letters.length` 와 같아지면 `low`를 `0`으로 할당해 준다. 
  
```java
class Solution {
    public char nextGreatestLetter(char[] letters, char target) {
        int low=0;
        int high=letters.length-1;
        
        while(low < high) {
            int mid = (low + high)/2;
            
            if ( letters[mid] > target ) {
                high = mid;
            } else {
                low = mid +1;
            }            
        }
        
        if ( letters[low] <= target ) {
            low++;
            if ( low >= letters.length ) {
                low = 0;
            }
        }
        
        return letters[low];
    }
}
```