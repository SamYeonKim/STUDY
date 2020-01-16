# Problem

Implement a trie with insert, search, and startsWith methods.

#### Example :

```swift
Trie trie = new Trie();

trie.insert("apple");
trie.search("apple");   // returns true
trie.search("app");     // returns false
trie.startsWith("app"); // returns true
trie.insert("app");   
trie.search("app");     // returns true
```

#### Note :

* You may assume that all inputs are consist of lowercase letters `a-z`
* All inputs are guaranteed to be non-empty strings

# My Answer
  
```java
class TrieNode {
    public boolean is_leaf;
    TrieNode[] children = new TrieNode[26];    
    
    boolean containsKey(char c) {
        return children[c - 'a'] != null;
    }
    
    TrieNode get(char c) {
        return children[c - 'a'];
    }
    
    void put(char c, TrieNode node) {
        children[c - 'a'] = node;
    }
}

class Trie {
    TrieNode m_root;
    /** Initialize your data structure here. */
    public Trie() {
        m_root = new TrieNode();
    }
    
    /** Inserts a word into the trie. */
    public void insert(String word) {        
        TrieNode cur = m_root;
        
        for(char c : word.toCharArray()) {
            if ( !cur.containsKey(c)) {
                cur.put(c, new TrieNode());
            } 
            
            cur = cur.get(c); 
        }
        
        cur.is_leaf = true;
    }
    
    /** Returns if the word is in the trie. */
    public boolean search(String word) {
        TrieNode cur = m_root;
        
        for(char c : word.toCharArray()) {
            if ( !cur.containsKey(c))
                return false;
            
            cur = cur.get(c);         
        }
        
        return cur.is_leaf;
    }
    
    /** Returns if there is any word in the trie that starts with the given prefix. */
    public boolean startsWith(String prefix) {
        TrieNode cur = m_root;
        
        for(char c : prefix.toCharArray()) {            
            if ( !cur.containsKey(c))
                return false;
            
            cur = cur.get(c);         
        }
        
        return true;
    }
}

/**
 * Your Trie object will be instantiated and called as such:
 * Trie obj = new Trie();
 * obj.insert(word);
 * boolean param_2 = obj.search(word);
 * boolean param_3 = obj.startsWith(prefix);
 */
```

