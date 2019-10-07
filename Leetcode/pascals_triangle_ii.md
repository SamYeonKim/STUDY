# Problem

Given a non-negative index k where k ≤ 33, return the kth index row of the Pascal's triangle.

Note that the row index starts from 0.

#### Example :

```swift
Input: 3
Output: [1,3,3,1]
```

# My Answer

* rowIndex의 이전 row의 리스트를 구하고, 그 리스트를 기반으로 새로운 row를 만든다.
  
```java
class Solution {
    public List<Integer> getRow(int rowIndex) {
        if(rowIndex == 0){
            return Arrays.asList(1);
        }
        List<Integer> list = getRow(rowIndex - 1);
        List<Integer> result = new ArrayList();
        result.add(1);
        for(int i = 1; i < list.size(); i++){
            result.add(list.get(i-1)+list.get(i));
        }
        result.add(1);
        return result;
    }
    
}
```

