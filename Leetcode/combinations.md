# Problem

Given two integers n and k, return all possible combinations of k numbers out of 1 ... n.

#### Example :

```swift
Input: n = 4, k = 2
Output:
[
  [2,4],
  [3,4],
  [2,3],
  [1,2],
  [1,3],
  [1,4],
]
```

# My Answer

* 재귀를 이용해서 해결
* 하나의 `combination`을 채우기 위한 추가 리스트 `item`을 만들고 여기에 순차대로 넣어 주자.
* `solver`에서 `start`가 이번에 `item`에 넣을 숫자가 되고, 만약 `item`의 사이즈가 `k`와 동일할경우 결과 리스트 `result`에 현재 만들어진 `item`의 복사본을 만들어서 넣어 주자.
* `solver`를 재귀 호출 한 이후 마지막 요소를 빼주면 다음 숫자를 넣을 수 있다.
* 만약 `Example` 처럼 n = 4, k = 2로 하고, `for(int i = start ~` 구문의 내용을 다음과 같이 수정 하면 
    ```java
    item.add(i);
    System.out.format("add %d, %s\n", i, item);
    solver(n,k,i+1, item, result);
    item.remove(item.size() -1);
    System.out.format("remove %s\n", item);
    ```
    Console에 다음과 같이 출력된다.
    ```
    add 1, [1]
    add 2, [1, 2]
    remove [1]
    add 3, [1, 3]
    remove [1]
    add 4, [1, 4]
    remove [1]
    remove []
    add 2, [2]
    add 3, [2, 3]
    remove [2]
    add 4, [2, 4]
    remove [2]
    remove []
    add 3, [3]
    add 4, [3, 4]
    remove [3]
    remove []
    add 4, [4]
    remove []
    ```
  
```java
class Solution {
    public List<List<Integer>> combine(int n, int k) {
        List<List<Integer>> result = new ArrayList<List<Integer>>();
        
        List<Integer> item = new ArrayList<Integer>();
        solver(n, k, 1, item, result);
            
        return result;
    }
    
    void solver(int n, int k, int start, List<Integer> item, List<List<Integer>> result) {
        if ( item.size() == k ) {
            result.add(new ArrayList<Integer>(item));
            return;
        }
        
        for(int i = start; i <= n; i++) {
            item.add(i);
            solver(n,k,i+1, item, result);
            item.remove(item.size() -1);
        }
    }
}
```

