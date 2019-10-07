# Problem

Given a non-negative integer numRows, generate the first numRows of Pascal's triangle.

#### Example :

```swift
Input: 5
Output:
[
     [1],
    [1,1],
   [1,2,1],
  [1,3,3,1],
 [1,4,6,4,1]
]
```

# My Answer

* level이 1일땐 원소가 1 하나 밖에 없는 리스트 이다.
* level이 2이상일땐 리스트의 앞, 뒤로 1을 넣어 주고 그 사이를 level-1 만큼 돌면서 이전에 추가된 리스트 ( 이번 level의 값)을 참조해서 원소를 추가 한다.
  
```java
class Solution {
   public List<List<Integer>> generate(int numRows) {
        List<List<Integer>> res = new ArrayList<>();
        if (numRows == 0) {
            return res;
        }
        res.add(Arrays.asList(1));
        for (int level = 2; level <= numRows; ++level) {
            List<Integer> row = new ArrayList<>();
            row.add(1);
            List<Integer> prevRow = res.get(level-2);
            for (int i = 1; i < level-1; ++i) {
                row.add(prevRow.get(i-1)+prevRow.get(i));
            }
            row.add(1);
            res.add(row);
        }
        return res;
    }
}
```

