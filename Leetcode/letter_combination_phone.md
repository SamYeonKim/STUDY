# Problem

Given a string containing digits from 2-9 inclusive, return all possible letter combinations that the number could represent.

A mapping of digit to letters (just like on the telephone buttons) is given below. Note that 1 does not map to any letters.

![](img/phone_number.png)

#### Example :

```swift
Input: "23"
Output: ["ad", "ae", "af", "bd", "be", "bf", "cd", "ce", "cf"].
```

# My Answer

* 재귀로 해결
* 사전에 `HashMap`을 이용해서 각 숫자별 매칭될 문자열을 지정해 주자.
* `digits`에 `HashMap`에 지정되지 않은 숫자가 넘어 올 수 있기 때문에, 이런 경우 빈 결과를 리턴
* `StringBuilder`를 이용해서 중간 과정들을 저장하고, `digits`의 숫자 갯수와 동일해 지면 `StringBuilder`의 문자열을 결과 리스트에 넣는다.
  
```java
class Solution {
    Map<Character, String> map_digits = new HashMap<Character, String>(){{
        put('2', "abc");
        put('3', "def");
        put('4', "ghi");
        put('5', "jkl");
        put('6', "mno");
        put('7', "pqrs");
        put('8', "tuv");
        put('9', "wxyz");
    }};
    
    public List<String> letterCombinations(String digits) {        
        List<String> result = new ArrayList<>();
        if ( digits.isEmpty() ) 
            return result;
        
        char[] input = digits.toCharArray();
        
        for(char c : input) {
            if ( !map_digits.containsKey(c) ) {
                return result;
            }
        }
        
        StringBuilder str = new StringBuilder();
        
        backTracking(input, result, str, 0);
        
        return result;
    }
    
    void backTracking(char[] input, List<String> result, StringBuilder str, int start) {
        if ( start == input.length ) {
            result.add(str.toString());
            return;
        }
        
        String alpha = map_digits.get(input[start]);
        
        char[] arr_alpha = alpha.toCharArray();
        
        for(int i = 0; i < arr_alpha.length; i++) {
            str.append(arr_alpha[i]);
            backTracking(input, result, str, start +1);
            str.deleteCharAt(str.length() - 1);
        }   
    }    
}
```

# Simple Answer

* 재귀를 이용해서 해결, `StringBuilder`나 사전에 `digits` 검사 없이 진행

```java
class Solution {
    Map<String, String> map = new HashMap<>();
    List<String> output = new ArrayList<>();

    public List<String> letterCombinations(String digits) {
        if(digits.length() == 0 || digits == null) {
            return output;
        }
        map.put("2","abc");
        map.put("3","def");
        map.put("4","ghi");
        map.put("5","jkl");
        map.put("6","mno");
        map.put("7","pqrs");
        map.put("8","tuv");
        map.put("9","wxyz");
        
        helper("", digits);
        return output;
    }
    
    private void helper(String combination, String nextDigits) {
        if(nextDigits.length() == 0) {
            output.add(combination);
            return;
        }
        
        String digit = String.valueOf(nextDigits.charAt(0));
        String letters = map.get(digit);
        for(int i = 0; i < letters.length(); i++) {
            String letter = String.valueOf(letters.charAt(i));
            helper(combination + letter, nextDigits.substring(1));
        }
    }
}
```

