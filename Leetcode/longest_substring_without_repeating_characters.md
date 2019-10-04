# Problem

Given a string, find the length of the longest substring without repeating characters.

#### Example 1:

```swift
Input: "abcabcbb"
Output: 3 
Explanation: The answer is "abc", with the length of 3. 
```

#### Example 2:

```swift
Input: "bbbbb"
Output: 1
Explanation: The answer is "b", with the length of 1.
```

#### Example 3:

```swift
Input: "pwwkew"
Output: 3
Explanation: The answer is "wke", with the length of 3. 
             Note that the answer must be a substring, "pwke" is a subsequence and not a substring.
```

# My Answer

* 문자열을 char 단위로 확인하자.
* start_idx 부터 char를 Set에 넣다가, 이미 있는 char가 발견되면, 발견되기 까지의 길이를 저장하고, Set를 클리어 한이후, start_index를 바로 이전 start_index에 +1하자.
  
```java
class Solution {
    public int lengthOfLongestSubstring(String s) {
        int res = 0;
        int max = 0;
        int n_start_idx = 0;
        Set<Character> st = new HashSet<Character>(); 
        char[] string = s.toCharArray();
        
        for(int i = 0; i< string.length; i++) {
            char c = string[i];

            if ( st.contains(c)) {
                i = ++n_start_idx - 1;
                st.clear();    
                res = max > res ? max : res;
                max = 0;
                continue;
            }
            
            max++;
            st.add(c);
        }

        res = max > res ? max : res;
        
        return res;        
    }
}
```

# Fastest Answer

* 기본 ASCII 코드가 128개 라는것을 이용해서 128크기의 배열로 중복 체크를 한다.
  > 반드시 기억하자. 쓸데 없이 Set같은걸 써서 중복 체크할 필요 없다. 
  > ASCII 확장까지 치면 256개 이다.

```java
class Solution {
    public int lengthOfLongestSubstring(String s) {
        int length = s.length();
        int start = 0;
        int max = 0;
        int[] arr = new int [128];
        for (int i =0; i<128; i++)
            arr[i] = -1;
        for (int i=0; i< length; i++) {
            char c = s.charAt(i);
            int cnum = (int) c ;
            if (arr[cnum] >= start) {
                start = arr[cnum] +1;
            }
            arr[cnum] = i;
            if (i - start +1> max) 
                max = i -start+1;
        }
        return max;
    }
}
```

