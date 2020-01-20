# Problem

Design a data structure that supports the following two operations:

```java
void addWord(word)
boolean search(word)
```

search(word) can search a literal word or a regular expression string containing only letters a-z or .. A . means it can represent any one letter.

#### Example :

```swift
addWord("bad")
addWord("dad")
addWord("mad")
search("pad") -> false
search("bad") -> true
search(".ad") -> true
search("b..") -> true
```

#### Note :

* You may assume that all words are consist of lowercase letters `a-z`

# My Answer

* `search` 에선 재귀 함수를 이용하자.
* 만약 `word`의 문자가 `.`이라면, 현재 `Trie`의 자식들을 순회하자. 하나라도 `true`라면 정답
  
```java
class Trie {
    public boolean isWord;
    public Trie[] children;
    
    public Trie() {
        children = new Trie[26];
    }
}

class WordDictionary {
    Trie root;
    /** Initialize your data structure here. */
    public WordDictionary() {
        root = new Trie();
    }
    
    /** Adds a word into the data structure. */
    public void addWord(String word) {
        Trie cur = root;
        char[] array = word.toCharArray();
        for(char c : array ) {
            if ( cur.children[c-'a'] == null ) {
                cur.children[c-'a'] = new Trie();                
            }
            
            cur = cur.children[c-'a'];   
        }
        
        cur.isWord = true;
    }
    
    /** Returns if the word is in the data structure. A word could contain the dot character '.' to represent any one letter. */
    public boolean search(String word) {
        return search(root, word.toCharArray(), 0);        
    }
    
    boolean search(Trie trie, char[] array, int idx) { 
        if ( trie == null ) return false;
        if ( idx == array.length ) return trie.isWord;
            
        char c = array[idx];
        
        if ( c == '.') {        //모든 자식들 중에 만족하는것이 있는지 확인해야 한다.
            for(Trie t : trie.children ) {
                if ( search(t, array, idx+1) )  //만족 하는게 있다면 더 이상 확인할 필요 없다.
                    return true;                            
            }
            
            return false;       //아무것도 만족 하지 않는 다면 false
        } 
        return search(trie.children[c-'a'], array, idx + 1);        
    }
}

/**
 * Your WordDictionary object will be instantiated and called as such:
 * WordDictionary obj = new WordDictionary();
 * obj.addWord(word);
 * boolean param_2 = obj.search(word);
 */
```

