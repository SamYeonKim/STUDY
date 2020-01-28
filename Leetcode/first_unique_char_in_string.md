# Problem

Given a string, find the first non-repeating character in it and return it's index. If it doesn't exist, return -1.

#### Example 1:

```swift
Input: "leetcode"
Output: 0
```

#### Example 2:

```swift
Input: "loveleetcode"
Output: 2
```

#### Note :

* You mau assume the string contain only lowercase letters.

# My Answer

* `HashMap`을 이용하자. `s`를 순회하면서, 문자의 발생횟수를 `HashMap`의 값으로 넣자.
* 이후 `s`를 순회하면서, `HashMap`의 값이 1인것이 나오면 해당 `index`가 정답이다.

```java
class Solution {
    public int firstUniqChar(String s) {
        HashMap<Character, Integer> count = new HashMap<Character, Integer>();
        int n = s.length();
        
        for (int i = 0; i < n; i++) {
            char c = s.charAt(i);
            count.put(c, count.getOrDefault(c, 0) + 1);
        }
        
        for (int i = 0; i < n; i++) {
            if ( count.get(s.charAt(i)) == 1)
                return i;
        }
        
        return -1;
    }
}
```

# Fastest Answer

* 소문자 문자 [a~z]만 포함되기 때문에, `s`에서 각 문자가 위치하고 있는 `가장 작은 index`와 `가장 큰 index`를 찾자.
* 만약 `가장 작은 index`가 -1이면, 해당 문자는 `s`에 없다는 뜻.
* 만약 `가장 작은 index`와 `가장 큰 index`가 같다면, 해당 문자는 `s`에서 하나만 있다는 뜻.
* 위의 조건을 만족하는 것들중 가장 작은 `index`값이 정답.

```java
class Solution {
    public int firstUniqChar(String s) {
        
        int min_index = -1;
        
        for(char c='a'; c<= 'z'; c++) {
            int first = s.indexOf(c);
            int last = s.lastIndexOf(c);
            
            if ( first == -1 || first != last ) continue;
                
            min_index = min_index == -1 || min_index > first ? first : min_index;
        }
        
        return min_index;        
    }
}
```

