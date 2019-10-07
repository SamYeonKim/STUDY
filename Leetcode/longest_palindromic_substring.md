# Problem

Given a string s, find the longest palindromic substring in s. You may assume that the maximum length of s is 1000.

#### Example 1:

```swift
Input: "babad"
Output: "bab"
Note: "aba" is also a valid answer.
```

#### Example 2:

```swift
Input: "cbbd"
Output: "bb"
```

# My Answer

* 문자열을 순회 하면서, 각 index를 중심으로 좌, 우로 비교해 나가면서 동일한 값의 길이를 구한다.
  
```java
class Solution {
    public String longestPalindrome(String s) {
        if (s == null || s.length() < 1) return "";
        int start = 0, end = 0;
        for (int i = 0; i < s.length(); i++) {
            int len1 = expandAroundCenter(s, i, i);
            int len2 = expandAroundCenter(s, i, i + 1);
            int len = Math.max(len1, len2);
            if (len > end - start) {
                start = i - (len - 1) / 2;
                end = i + len / 2;
            }
        }
        return s.substring(start, end + 1);
    }

    private int expandAroundCenter(String s, int left, int right) {
        int L = left, R = right;
        while (L >= 0 && R < s.length() && s.charAt(L) == s.charAt(R)) {
            L--;
            R++;
        }
        return R - L - 1;
    }
}
```