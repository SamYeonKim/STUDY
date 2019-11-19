# Problem

Given n pairs of parentheses, write a function to generate all combinations of well-formed parentheses.

#### Example :

```swift
Input: 3
Output:
[
  "((()))",
  "(()())",
  "(())()",
  "()(())",
  "()()()"
]
```

# My Answer ( Recursive )

* 재귀를 이용해서 해결
  
```java
class Solution {
    public List<String> generateParenthesis(int n) {
        List<String> result = new ArrayList();
        backtrack(result, "", 0, 0, n);
        return result;
    }

    public void backtrack(List<String> result, String cur, int open, int close, int max){
        if (cur.length() == max * 2) {
            result.add(cur);
            return;
        }

        if (open < max)            
            backtrack(result, cur+"(", open+1, close, max);
            
        if (close < open)            
            backtrack(result, cur+")", open, close+1, max);            
    }
}
```

# My Answer ( Iteration )

* 다음과 같은 알고리즘이 나온다
    ```
    f(0) = Empty
    f(1) = ()
    f(2) = ()() = () + f(1)
           (()) = ( f(1) )
    f(3) = ()()() = (f(0)) + f(2_1)
           ()(()) = (f(0)) + f(2_2)
           (())() = (f(1)) + f(1)
           (()()) = (f(2_1)) + f(0)
           ((())) = (f(2_2)) + f(0)
    first_sol = sol(0 ~ n - 1)
    second_sol = sol(n - 1 ~ 0)

    for(String x : first_sol)
        for(String y : second_sol)
            sol(n).add( (+ x +) + y )
    ```
* 0번째와 1번째는 미리 `Solutions`에 넣어 줬다.

```java
class Solution {
    public List<String> generateParenthesis(int n) {
        List<List<String>> solutions = new ArrayList<List<String>>();
        
        List<String> l_zero = new ArrayList();
        l_zero.add("");
        solutions.add(l_zero);
        
        List<String> l_one = new ArrayList();
        l_one.add("()");
        solutions.add(l_one);        
        
        for ( int i = 2; i <= n; i++ ) {
            int x_idx = 0;
            int y_idx = i-1;
            
            List<String> sol = new ArrayList();
            
            while(x_idx < i) {
                List<String> first = solutions.get(x_idx);
                List<String> second = solutions.get(y_idx);
                
                for(String x : first) {
                    for(String y : second ) {
                        sol.add(String.format("(%s)%s", x, y));    
                    }
                }

                x_idx++;
                y_idx--;
            }
            
            solutions.add(sol);
        }
        
        return solutions.get(n);
    }
}
```



