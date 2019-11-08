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

* 2개의 index 포인터를 이용해서 스왑한다.
* `l_idx` 가 `r_idx`보다 작을때만 순회한다. 만약 `l_idx`가 `r_idx`와 같거나 커지는 경우는 더이상 스왑할 것이 없다는 의미.
  
```java
class Solution {
    public void reverseString(char[] s) {
        int l_idx = 0;
        int r_idx = s.length - 1;
        
        while( l_idx < r_idx ) {
            char src = s[l_idx];
            char dst = s[r_idx];
            s[l_idx++] = dst;
            s[r_idx--] = src;   
        }
    }    
}
```

