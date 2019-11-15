# Problem

Write a program to solve a Sudoku puzzle by filling the empty cells.

A sudoku solution must satisfy all of the following rules:

Each of the digits 1-9 must occur exactly once in each row.
Each of the digits 1-9 must occur exactly once in each column.
Each of the the digits 1-9 must occur exactly once in each of the 9 3x3 sub-boxes of the grid.
Empty cells are indicated by the character '.'.

### Note:

* The given board contain only digits 1-9 and the character '.'.
* You may assume that the given Sudoku puzzle will have a single unique solution.
* The given board size is always 9x9.


# My Answer

* `BackTracking`을 이용해서 해결
* 넣을 수 있는 문자 `1~9`를 배열로 미리 선언해 놓는다.
* `solver`를 재귀로 호출, `solver`를 재귀로 호출 한 이후 결과값이 `false`이면 이번에 넣었던 숫자는 맞지 않는다는 의미이기 때문에 `.`으로 설정한다.
* `isValid` 함수에서 현재 넣으려는 Row, Column 그리고 숫자를 이용해서 스도쿠의 규칙에 위배 되지 않는지를 검증한다.
  
```java
class Solution {
    char[] candidate = {'1','2','3','4','5','6','7','8','9'};
    
    public void solveSudoku(char[][] board) {
        solver(board);
    }
    
    boolean solver(char[][] board){           
        boolean is_empty = false;
        int next_row = -1;
        int next_col = -1;
        
        for(int row = 0; row < 9; row++) {
            for(int col = 0; col < 9; col++) {
                if ( board[row][col] == '.') {
                    is_empty = true;
                    next_row = row;
                    next_col = col;
                    break;
                }                    
            }
            
            if ( is_empty )
                break;
        }
        
        if ( !is_empty ) {
            return true;
        }
        
        for(char number : candidate ) {    
            if ( isValid(board, next_row, next_col, number)) {
                board[next_row][next_col] = number;
                if ( solver(board) ) {
                    return true;
                } else {
                    board[next_row][next_col] = '.';    //재귀 호출 이후 결과값이 false가 나왔다는 것은 다른 숫자를 넣어야 한다는것이니 빈값으로 설정
                }
            }
        }
        
        return false;
    }
    
    boolean isValid(char[][] board, int cur_row, int cur_col, char number) {
        for(int col = 0; col < 9; col++) {      //현재 Row에 number와 중복된 숫자가 있는지 확인
            if ( board[cur_row][col] == number )
                return false;
        }
        
        for(int row = 0; row < 9; row++) {      //현재 Column에 number와 중복된 숫자가 있는지 확인
            if ( board[row][cur_col] == number )
                return false;
        }
        
        int start_row = getStartIndex(cur_row);
        int start_col = getStartIndex(cur_col);
        
        for(int row = start_row; row < start_row +3; row++) {       //현재 Row와 Column이 속해 있는 3x3 영역에 중복된 숫자가 있는지 확인
            for(int col = start_col; col < start_col +3; col++) {
                if ( board[row][col] == number )
                    return false;
            }
        }
        
        return true;
    }
    
    int getStartIndex(int target) {
        if ( target < 3 ) {
            return 0;
        } else if ( target < 6 ) {
            return 3;
        } else {
            return 6;
        }            
    }
}
```

