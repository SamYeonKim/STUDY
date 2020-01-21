# Problem

Given a 2D board and a list of words from the dictionary, find all words in the board.

Each word must be constructed from letters of sequentially adjacent cell, where "adjacent" cells are those horizontally or vertically neighboring. The same letter cell may not be used more than once in a word.

#### Example 1:

```swift
Input: 
board = [
  ['o','a','a','n'],
  ['e','t','a','e'],
  ['i','h','k','r'],
  ['i','f','l','v']
]
words = ["oath","pea","eat","rain"]

Output: ["eat","oath"]
```

#### Note :

* All inputs are consist of lowercase letters `a-z`
* The values of `words` are distinct

# My Answer

* `board`의 원소들을 순회 하면서 각 원소의 사방으로 이웃한 노드를 `Trie`로 구성한다.
* 반복횟수가 너무 많은 문제가 있다.

```java
class Trie {
    public boolean use;
    public char val;
    public List<Trie> neighbors;
    public Trie(char c) {
        neighbors = new ArrayList<>();
        val = c;
    }
   
    public boolean search(Trie trie, char[] array, int index ) {
        if ( trie == null ) return false;
        if ( array.length == index ) return true;
        
        char c = array[index];
        for ( Trie next : trie.neighbors ) {
            if ( next.val != c ) continue;
            if ( next.use ) continue;
            next.use = true;
            if ( search(next, array, index + 1)) {
                next.use = false;
                return true;                  
            } 
            
            next.use = false;
        }
        
        return false;            
    }
}

class Solution {
    public List<String> findWords(char[][] board, String[] words) {
        Trie trie = new Trie('-');
        int height=board.length;
        int width=board[0].length;
        
        for(int y=0;y<height;y++) {
            for(int x=0;x<width;x++) {
                trie.neighbors.add(new Trie(board[y][x]));
            }
        }
        
        for(int y=0;y<height;y++) {
            for(int x=0;x<width;x++) {
                int cur_idx = x + width * y;
                Trie center = trie.neighbors.get(cur_idx);
                
                for(int i=0;i<4;i++) {
                    int new_x=x;
                    int new_y=y;
                    int neighbor_idx = 0;
                    if ( i == 0 ) {
                        new_x = x-1;                        
                    } else if( i == 1) {
                        new_x = x+1;
                    } else if ( i == 2) {
                        new_y = y-1;
                    } else {
                        new_y = y+1;
                    }
                    
                    if ( new_x < 0 || new_x >= width || new_y < 0 || new_y >= height )
                        continue;
                    
                    neighbor_idx = new_x + width * new_y;
                    
                    if ( neighbor_idx < 0 || neighbor_idx >= trie.neighbors.size())
                        continue;
                    
                    center.neighbors.add(trie.neighbors.get(neighbor_idx));
                }
            }
        }
        
        List<String> result = new ArrayList<>();
        
        for (String word : words ) {
            if ( trie.search(trie, word.toCharArray(), 0) ) {
                result.add(word);
            }
        }

        return result;
    }    
}
```

# Fastest Answer

* `words`의 원소들을 이용해서 `Trie`를 구성하고, `board`에 맞는것을 찾자.

```java
class Trie {
    public String word;    
    public Trie[] children;
    public Trie() {
        children = new Trie[26];
    }   
    
    public void insert(String str) {
        Trie cur = this;
        for(char c : str.toCharArray()) {
            if ( cur.children[c-'a'] == null )
                cur.children[c-'a'] = new Trie();
            
            cur = cur.children[c-'a'];
        }
        
        cur.word = str;
    }
}

class Solution {
    public List<String> findWords(char[][] board, String[] words) {
        Trie trie = new Trie();
        
        for( String word : words ) {
            trie.insert(word);
        }
        
        List<String> result = new ArrayList<>();
        
        for(int i=0;i<board.length;i++)
            for(int j=0;j<board[0].length;j++)
                search(board, i, j, trie, result);

        return result;
    }   
    
    void search(char[][] board, int i, int j, Trie trie, List<String> result) {
        if ( i < 0 || j < 0 || i >= board.length || j >= board[0].length || board[i][j] == '@' || trie.children[board[i][j]-'a'] == null )
            return;
        
        trie = trie.children[board[i][j]-'a'];
        if ( trie == null )
            return;
        
        if ( trie.word != null ) {
            result.add(trie.word);
            trie.word = null;
        }
        
        char c = board[i][j];
        board[i][j]='@';        //임의의 값으로 치환하자. 이미 방문 했다는 의미이다.
        search(board, i-1,j, trie, result);
        search(board, i+1,j, trie, result);
        search(board, i,j-1, trie, result);
        search(board, i,j+1, trie, result);
        board[i][j]=c;
    }
}
```

