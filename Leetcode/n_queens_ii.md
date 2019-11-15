# Problem

The n-queens puzzle is the problem of placing n queens on an n×n chessboard such that no two queens attack each other.

![](img/n_queen_ii.png)

Given an integer n, return the number of distinct solutions to the n-queens puzzle.

#### Example :

```swift
Input: 4
Output: 2
Explanation: There are two distinct solutions to the 4-queens puzzle as shown below.
[
 [".Q..",  // Solution 1
  "...Q",
  "Q...",
  "..Q."],

 ["..Q.",  // Solution 2
  "Q...",
  "...Q",
  ".Q.."]
]
```

# My Answer

* 재귀를 이용해서 해결
* `board 배열`의 index는 Row를 의미하고, 해당 index의 값은 Column을 의미 한다. 예를 들어 
  `zero base, board[1]의 값이 2` 라면, 1번 줄의 2번 칸에 퀸이 위치해 있다는 의미.
* 각 Column 별로 퀸을 위치 시킬수 있는지 `isValid` 함수로 확인
* `isValid` 함수에서는 현재 Row이전 `board` 값을 이용해서 위치 시킬수 있는지 확인
  
```java
class Solution {
    int cnt = 0;    //NQueens를 위치 시킬수 있는 방법의 총 갯수를 의미
    
    public int totalNQueens(int n) {        
        placeQueen(new int[n], 0, n);
        return cnt;
    }

    void placeQueen(int[] board, int row, int n_queens) {
        if (row == n_queens) {      //현재 Row와 n_queens가 같다는 의미는 모든 Row에 대해서 위치 시켰다는 의미이기 때문에, 횟수 증가
            cnt++;
            return;
        }
        for (int i = 0; i < n_queens; i++) {
            board[row] = i;
            if (isValid(board, row)) {
                placeQueen(board, row + 1, n_queens);
            }
        }
    }

    boolean isValid(int[] board, int row) {
        for (int i = 0; i < row; i++) {     //현재 위치 시키려는 Row이전 board값들을 이용해서 확인
            /*
            1. board[i] == board[row] 는 현재 위치 시키려는 Row의 값을 이미 이전 Row에서 사용하고 있다. 즉 같은 Column에 배치 하려고 한다.
            2. row - i == Math.abs(board[i] - board[row]) 는 현재 위치 시키려는 Column과 대각선으로 위치 하고 있는 값이 있다.
            예를 들어 0번째 Row의 3번째 Column에 이미 퀸이 위치해 있고, 1번째 Row의 2번째 Column에 배치 하려고 할때 위 조건을 만족 하게 된다.
            [-][-][-][X]
            [-][-][X][-]
            ....
            row(1) - i(0) == Math.abs(board[i](3) - board[row](2))
            => 1 - 0 == Math.abs(3 - 2)
            => 1 == 1 = true
            */
            if (board[i] == board[row] || row - i == Math.abs(board[i] - board[row])) {
                return false;
            }
        }
        return true;
    }
};
```

