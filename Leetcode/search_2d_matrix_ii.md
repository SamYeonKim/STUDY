# Problem

Write an efficient algorithm that searches for a value in an m x n matrix. This matrix has the following properties:

* Integers in each row are sorted in ascending from left to right.
* Integers in each column are sorted in ascending from top to bottom.
  
#### Example :

```swift
[
  [1,   4,  7, 11, 15],
  [2,   5,  8, 12, 19],
  [3,   6,  9, 16, 22],
  [10, 13, 14, 17, 24],
  [18, 21, 23, 26, 30]
]

Given target = 5, return true.
Given target = 20, return false.
```

# My Answer

* 사전에 이미 정렬이 된 상황이기때문에, 맨 끝에서 부터 찾아 가면 된다.
* 0번째 row의 맨 마지막 column부터 시작해서, 찾으려는 값과 동일하면 끝. 만약 값이 작다면 그 다음 row를 보고, 값이 크면 이전 column을 보면 됨
* 무식하게 m x n 만큼돌지 않는다.

```java
class Solution {
    public boolean searchMatrix(int[][] matrix, int target) {
        if (matrix.length == 0)
            return false;
        
        int row = 0;
        int col = matrix[0].length - 1;
        
        while(row < matrix.length && col >= 0) {
            int val = matrix[row][col];
            if ( val == target ) {
                return true;
            } else if ( val < target ) {
                row++;
            } else {
                col--;
            }
        }
        
        return false;
    }
}
```