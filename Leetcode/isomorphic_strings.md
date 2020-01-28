# Problem

Given two strings s and t, determine if they are isomorphic.

Two strings are isomorphic if the characters in s can be replaced to get t.

All occurrences of a character must be replaced with another character while preserving the order of characters. No two characters may map to the same character but a character may map to itself.

#### Example 1:

```swift
Input: s = "egg", t = "add"
Output: true
```

#### Example 2:

```swift
Input: s = "foo", t = "bar"
Output: false
```

#### Example 3:

```swift
Input: s = "paper", t = "title"
Output: true
```

#### Note :

* You may assume both `s` and `t` have the same length.

# My Answer

* `HashMap`을 이용하자. `HashMap`의 키는 `s`의 문자, 값은 `t`의 문자를 넣자.
* `s`를 순회 하면서, 문자가 `HashMap`의 키로 포함되어 있는지 확인하자.
* 만약 키로 포함되어 있으면서, 값이 `t`의 문자와 다르다면 `false`
* 만약 키로 포함되어 있지 않은데, `t`의 문자가 값으로 `HashMap`에 포함되어 있다면 `false`
* 위 조건을 만족하지 않는다면, `HashMap`에 넣고 다음 순회.
* 반복해서 돌았는데, `false`가 발생하지 않는다면, `true`이다.

```java
class Solution {
    public boolean isIsomorphic(String s, String t) {
        Map<Character, Character> hashmap = new HashMap<>();
        
        for(int i=0;i<s.length(); i++) {
            char s_c = s.charAt(i);
            
            if ( hashmap.containsKey(s_c) ) {
                if ( hashmap.get(s_c) != t.charAt(i)) return false;
            } else {
                if ( hashmap.containsValue(t.charAt(i))) return false;
                
                hashmap.put(s_c, t.charAt(i));               
            }             
        }
        
        return true;
    }
}
```

