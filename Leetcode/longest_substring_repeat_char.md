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

# My Answer ( Use HashSet)
  
* `HashSet`을 이용하자.
* `s`를 순회하면서 `HashSet`에 포함되어 있는 문자 인지 확인하고, 포함되어 있는 문자라면 `HashSet`을 초기화 하면서 기존의 `max`보다 멀리 왔는지 확인.

```java
class Solution {
    public int lengthOfLongestSubstring(String s) {
        Set<Character> hashset = new HashSet<>();
        int length = s.length();
        int max = 0;
        int sub_count = 0;
        int i=0;
        
        while ( i < length ) {
            char c = s.charAt(i);
            if (hashset.contains(c)) {
                max = max > sub_count ? max : sub_count;  
                
                if (i == 0 || c != s.charAt(i-1)) 
                    i=i-sub_count + 1;                
                
                hashset.clear();
                sub_count=0;                                
            } else {
                hashset.add(c);            
                sub_count++;            
                i++;
            }
        }
        
        max = max > sub_count ? max : sub_count;  
        return max;
    }
}
```

# My Answer

* `ASCII코드`에 해당하는 문자만 들어오기 때문에, 128개의 `int배열`로 해결 할 수 있다.
* 알고리즘은 다음과 같다.
    1. 문자열 `s`에 대해서, 특정 문자가 반복되기 전까지의 `substring`의 길이를 구하고 최대 값을 유지 한다.
    2. `시작index` ~ `문자 중복 발생 or 문자열의 끝`이 최대 값을 비교 할 수 있는 후보이다.
    3. 특정 문자에 대해서 중복이 발생했을때 `시작index`를 변경해 줘야 한다.
       * 예를들어 `s[1,2,3]`을 확인중에 `s[4]`가 `s[1]`과 중복문자 라면 다음 확인해야 하는 `substring`은 `s[2]` 부터 시작하는 문자열 이어야 한다.
       * 예를들어 `s[1,2,3]`을 확인중에 `s[4]`가 `s[2]`와 중복문자 라면 다음 확인해야 하는 `substring`은 `s[3]` 부터 시작하는 문자열 이어야 한다.
       * 따라서, 각 문자에 대해서 중복이 발생했을때 변경할 `시작index`를 저장하자.
* 문자 `s[i]`가 곧 `int배열(arr)`의 `index`가 되고, `arr`의 값은 문자의 문자열 `s`의 위치 + 1이 된다.
* 현재 확인중인 `substring`의 시작 위치를 의미하는 변수 `start`를 사용.
* `s`를 순회 하면서 다음과 같은 흐름으로 진행한다.
    1. `start` 갱신 : 기존 `start`와 `arr[s[i]]` 중 최대 값.
    2. `최대 문자열(max)` 갱신 : 기존 `최대 문자열`과 `i-start+1` 중 최대 값.
    3. `arr[s[i]]` 갱신 : `i`+1
* 예를 들어 다음과 같은 흐름으로 진행 된다. 
    ```
    Input: abcbdef
    1:
        c=s[0]='a'
        start=0, max=Max(max, i-start+1=1)=1
        arr['a']=1
    2:
        c=s[1]=`b`
        start=0, max=Max(max,i-start+1=2)=2
        arr['b']=2
    3:
        c=s[2]='c'
        start=0, max=Max(max,i-start+1=3)=3
        arr['b']=3
    4:
        c=s[3]='b'  
        start=Max(start, arr['b']=2)=2      //중복 발생이기때문에 arr['b']의 값으로 start가 변경된다. 즉 기존엔 a,b,c가 확인해야할 substring 이였다면 지금부턴 c부터가 확인해야할 substring이 된다.
        max=Max(max,i-start+1=3-2+1)=3      
        arr['b']=4
    5:
        c=s[4]='d'
        start=2, max=Max(max,i-start+1=4-2+1)=3
        arr['d']=5
    ...
    ```

```java
class Solution {
    public int lengthOfLongestSubstring(String s) {
        int[] arr = new int[128];
        int length = s.length();
        int max = 0;
        int start=0;

        for(int i=0;i<length;i++) {            
            start = Math.max(start, arr[s.charAt(i)]);
            max = Math.max(max,i-start+1);
            
            arr[s.charAt(i)]=i+1;                
        }

        return max;
    }    
}
```

