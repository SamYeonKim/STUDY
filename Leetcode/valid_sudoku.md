# Problem

Determine if a 9x9 Sudoku board is valid. Only the filled cells need to be validated according to the following rules:

1. Each row must contain the digits `1-9` without repetition.
2. Each column must contain the digits `1-9` without repetition.
3. Each of the 9 `3x3` sub-boxes of the grid must contain the digits `1-9` without repetition.

#### Example 1:

```swift
Input:
[
  ["5","3",".",".","7",".",".",".","."],
  ["6",".",".","1","9","5",".",".","."],
  [".","9","8",".",".",".",".","6","."],
  ["8",".",".",".","6",".",".",".","3"],
  ["4",".",".","8",".","3",".",".","1"],
  ["7",".",".",".","2",".",".",".","6"],
  [".","6",".",".",".",".","2","8","."],
  [".",".",".","4","1","9",".",".","5"],
  [".",".",".",".","8",".",".","7","9"]
]
Output: true
```

#### Example 2:

```swift
Input:
[
  ["8","3",".",".","7",".",".",".","."],
  ["6",".",".","1","9","5",".",".","."],
  [".","9","8",".",".",".",".","6","."],
  ["8",".",".",".","6",".",".",".","3"],
  ["4",".",".","8",".","3",".",".","1"],
  ["7",".",".",".","2",".",".",".","6"],
  [".","6",".",".",".",".","2","8","."],
  [".",".",".","4","1","9",".",".","5"],
  [".",".",".",".","8",".",".","7","9"]
]
Output: false
Explanation: Same as Example 1, except with the 5 in the top left corner being 
    modified to 8. Since there are two 8's in the top left 3x3 sub-box, it is invalid.
```

#### Note :

* A Sudoku board(partially filled) could be valid but is not necessarily solvable.
* Only the filled cells need to be validated according to the mentioned rules.
* The given board contain only digits `1-9` and where empty cells are filled with the character `'.'`.
* The given board size is always `9x9`.
* 

# My Answer

* `HashMap`을 이용하자. `HashMap`의 키는 다음과 같이 선정된다.
  * row_1~9, column_1~9, block_1~9
  * 코드에선 키 생성을 편하게 하기 위해서 `숫자 범위`로 한다.
  * `row` = [0~height-1] = [0~8]
  * `column` = [height~height+width-1] = [9~17]
  * `block` = [height+width~height+width+width-1] = [18~26]
* `HashMap`의 값은 `Board`의 문자가 된다.
* `Board`를 순회 하면서, 현재 위치의 `row, column, block`의 키를 구하고, 각 키에 대응되는 값에 현재 문자가 포함되어 있는지 확인한다.
* 만약 포함되어 있다면, 중복 이기때문에 `false`
* 포함되어 있지 않다면, 현재 문자를 `HashMap`의 값에 넣는다.

```java
class Solution {
    public boolean isValidSudoku(char[][] board) {
        Map<Integer, List<Character>> hashmap = new HashMap<>();        
        int width = board[0].length;
        int height = board.length;
        
        for(int i=0;i<height;i++) {
            for(int j=0;j<width;j++) {
                char c = board[i][j];
                if ( c == '.') continue;    //빈 공간은 체크할 필요 없다

                //#region Row Check
                int row = i;                                
                if ( !hashmap.containsKey(row) ) {
                    List<Character> l_row = new ArrayList<>();
                    hashmap.put(row, l_row);
                }
                
                if ( hashmap.get(row).contains(c) ) return false;
                hashmap.get(row).add(c);
                //#endregion

                //#region Column Check
                int column = height + j;                
                if ( !hashmap.containsKey(column) ) {
                    List<Character> l_column = new ArrayList<>();
                    hashmap.put(column, l_column);
                }
                
                if ( hashmap.get(column).contains(c) ) return false;
                hashmap.get(column).add(c);
                //#endregion
                
                //#region Block Check
                int block = height + width + (i/3*3 + j/3);
                if ( !hashmap.containsKey(block) ) {
                    List<Character> l_block = new ArrayList<>();
                    hashmap.put(block, l_block);
                }
                
                if ( hashmap.get(block).contains(c) ) return false;
                hashmap.get(block).add(c);
                //#endregion
            }
        }
            
        return true;
    }
}
```

