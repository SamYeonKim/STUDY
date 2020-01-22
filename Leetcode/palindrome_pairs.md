# Problem

Given a list of unique words, find all pairs of distinct indices (i, j) in the given list, so that the concatenation of the two words, i.e. words[i] + words[j] is a palindrome.

#### Example 1:

```swift
Input: ["abcd","dcba","lls","s","sssll"]
Output: [[0,1],[1,0],[3,2],[2,4]] 
Explanation: The palindromes are ["dcbaabcd","abcddcba","slls","llssssll"]
```

#### Example 2:

```swift
Input: ["bat","tab","cat"]
Output: [[0,1],[1,0]] 
Explanation: The palindromes are ["battab","tabbat"]
```

# My Answer

* `Palindrome`은 가운데 기준으로 좌,우에 배치된 문자가 동일한것을 의미 한다.
* `Trie`에는 `words`의 역순에 해당하는 문자열을 이용해서 구성한다.
  * 예를 들어 `words=["abc","def"]` 라면 `"cba"`, `"fed"`를 활용
  * 위와 같이 역순으로 넣음으로써, `Palindrome`을 찾기 수월해 진다.
  * 각 마지막 `Trie`노드에는 원래 단어의 `index`
  * 각 노드의 이하 `Trie`노드가 `Palindrome`일때 원래 단어의 `index`리스트를 저장하기 위한 `p_index`. 다음과 같은 흐름이 된다.
    ```java
    words=["abc", "bcba", "sscba"]
    trie=c->b[0]->a(0), a->b->c[1,2]->b(1)
                                    ->s[2]->s(2)                                                                
    words[0]="abc"의 Palindrome을 찾을때 :
    trie->a->b->c 노드 까지 찾을 수 있고,
    c노드의 p_index=[1,2] 이기 때문에, words[1], words[2]와 Palindrome을 구성 한 다는것을 알 수 있다.
    words[0] + words[1] = "abcbcba"
    words[0] + words[1] = "abcsscba"
    ```

```java
class Solution {
    class Trie {
        public int index;               //현재 단어가 갖고 있던 index
        public List<Integer> p_index;   //현재 캐릭터 이하의 경로가 Palindrome일 때, index
        public Trie[] children;
        
        public Trie () {
            index = -1;
        }
    }
    
    public List<List<Integer>> palindromePairs(String[] words) {
        List<List<Integer>> result = new ArrayList<>();   
        
        Trie trie = buildTrie(words);
        
        int i=0;
        for(String word : words ) {
            findPalindrome(trie, word, i++, result);
        }
        
        return result;
    }

    void findPalindrome ( Trie trie, String word, int index, List<List<Integer>> result ) {
        int i=0;
        int count = word.length();
        while(i < count) {      //Trie에 역순으로 들어가 있기 때문에, 정상적으로 앞에서 부터 비교해 나가면 된다.            
            if ( trie.children == null ) break;
            
            char c = word.charAt(i);
            
            if ( trie.children[c-'a'] == null ) break;                
            
            if ( trie.index >= 0 && trie.index != index && isPalindrome(word, i,count-1) ) {    //word 보다 작은길이가 trie에 이미 있으면서, word의 나머지가 Palindrome이다.
                result.add(Arrays.asList(index, trie.index));    
            }
        
            trie = trie.children[c-'a'];
            i++;
        }
        
        if ( !isPalindrome(word, i,count-1) )
            return;
        
        if ( trie.index >= 0 &&  trie.index != index ) {
            result.add(Arrays.asList(index, trie.index));
        } 
        
        if ( i == count && trie.p_index != null ) {
            for(int p_index : trie.p_index ) {
                result.add(Arrays.asList(index, p_index));    
            }            
        }
    }
    
    Trie buildTrie( String[] words ) {
        Trie root = new Trie();
        
        int i=0;
        for(String word : words ) {
            insert(root, word, i++);
        }
        
        return root;
    }
    
    void insert( Trie root, String word, int index ) {
        int count = word.length();
        for(int i=count-1;i>=0;i--) {   //word의 역순으로 Trie를 구성하자.
            if ( root.children == null ) 
                root.children = new Trie[26];
            
            char c = word.charAt(i);
            
            if ( root.children[c-'a'] == null ) 
                root.children[c-'a'] = new Trie();
            
            if ( isPalindrome(word, 0, i) ) {       //이하 단어 만으로 Palindrome을 만족하는지 확인하자. 만족한다면, 현재 노드까지만 일치 하더라도, 이하는 확인할 필요 없이 만족 한다는것을 증명할 수 있다.
                if ( root.p_index == null )
                    root.p_index = new ArrayList<>();
                root.p_index.add(index);
            }
            
            root = root.children[c-'a'];
        }

        root.index = index;
    }
    
    boolean isPalindrome(String word, int start, int end ) {
        while( start < end ) {
            if ( word.charAt(start) != word.charAt(end) ) return false;                
            start++;
            end--;
        }
        
        return true;
    }
}
```