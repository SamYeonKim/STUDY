# Problem

You're given strings `J` representing the types of stones that are jewels, and `S` representing the stones you have.  Each character in `S` is a type of stone you have.  You want to know how many of the stones you have are also jewels.

The letters in `J` are guaranteed distinct, and all characters in `J` and `S` are letters. Letters are case sensitive, so `"a"` is considered a different type of stone from `"A"`.

#### Example 1:

```swift
Input: J = "aA", S = "aAAbbbb"
Output: 3
```

#### Example 2:

```swift
Input: J = "z", S = "ZZ"
Output: 0
```

#### Note:

* `S` and `J` will consist of letters and have length at most 50.
* The characters in `J` are distinct.

# My Answer
  
* `HashSet`을 이용하자.
* `J`를 순회하면서, 문자를 `HashSet`에 저장하고, `S`를 순회하면서 `HashSet`에 포함되어 있는지 확인.

```java
class Solution {
    public int numJewelsInStones(String J, String S) {
        Set<Character> hashset = new HashSet<>();
        
        for(int i=0;i<J.length();i++) {
            hashset.add(J.charAt(i));
        }
        
        int total = 0;
        for(int i=0;i<S.length();i++) {
            if ( hashset.contains(S.charAt(i))) total++;
        }
        
        return total;
    }
}
```

