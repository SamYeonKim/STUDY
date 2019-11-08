# Problem

Write a function that reverses a string. The input string is given as an array of characters char[].

Do not allocate extra space for another array, you must do this by modifying the input array in-place with O(1) extra memory.

You may assume all the characters consist of printable ascii characters.

#### Example 1:

```swift
Input: ["h","e","l","l","o"]
Output: ["o","l","l","e","h"]
```

#### Example 2:

```swift
Input: ["H","a","n","n","a","h"]
Output: ["h","a","n","n","a","H"]
```

# My Answer

* 재귀 함수 `reverse`를 이용해서 처리
* `base case`는 `l_idx`가 `r_idx` 보다 커질때 이다. 이경우는 더 이상 스왑할것이 없다는 의미
  
```java
class Solution {
    public void reverseString(char[] s) {
        reverse(s, 0, s.length - 1);
    }
    
    void reverse(char[] s, int l_idx, int r_idx ) {
        if ( l_idx > r_idx )
            return;
        
        char src = s[l_idx];
        char dst = s[r_idx];
        s[l_idx] = dst;
        s[r_idx] = src;   
        
        reverse(s, ++l_idx, --r_idx);
    }
}
```

