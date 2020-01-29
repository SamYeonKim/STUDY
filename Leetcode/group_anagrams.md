# Problem

Given an array of strings, group anagrams together.

#### Example :

```swift
Input: ["eat", "tea", "tan", "ate", "nat", "bat"],
Output:
[
  ["ate","eat","tea"],
  ["nat","tan"],
  ["bat"]
]
```

#### Note:

* All inputs will be in lowercase.
* The order of your output does not matter.

# My Answer

* `HashMap`을 이용하자. 문자열의 정렬된 형태의 문자열을 키로 하고, 문자열의 리스트를 값으로 할당.
* `strs`를 순회하면서, `strs[i]`를 정렬한다.
* `정렬된 strs[i]`가 `HashMap`의 키로 존재 하는지 확인 하고, 없다면 `List<String>`을 만들어서 `HashMap`과 결과 리스트에 추가 한다. `List<String>`를 참조 타입이기 때문에 수정하기 용이 하다.
* `HashMap`에서 `정렬된 strs[i]`의 값에 `strs[i]`를 넣으면 된다.
  
```java
class Solution {
    public List<List<String>> groupAnagrams(String[] strs) {
        List<List<String>> result = new ArrayList<>();
        Map<String, List<String>> hashmap = new HashMap<>();
        
        for ( String s : strs ) {
            char[] arr = s.toCharArray();
            
            Arrays.sort(arr);
            
            String sorted_str = new String(arr);
            
            if ( !hashmap.containsKey(sorted_str) ) {
                List<String> sub_result = new ArrayList<>();                
                hashmap.put(sorted_str, sub_result);
                result.add(sub_result);                
            }
            
            hashmap.get(sorted_str).add(s);
        }
        
        return result;
    }
}
```

