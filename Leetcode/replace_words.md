# Problem

In English, we have a concept called root, which can be followed by some other words to form another longer word - let's call this word successor. For example, the root an, followed by other, which can form another word another.

Now, given a dictionary consisting of many roots and a sentence. You need to replace all the successor in the sentence with the root forming it. If a successor has many roots can form it, replace it with the root with the shortest length.

You need to output the sentence after the replacement.

#### Example :

```swift
Input: dict = ["cat", "bat", "rat"]
sentence = "the cattle was rattled by the battery"
Output: "the cat was rat by the bat"
```

#### Note :

* The input will only have lower-case letters.
* 1 <= dict words number <= 1000
* 1 <= sentence words number <= 1000
* 1 <= root length <= 100
* 1 <= sentence words length <= 1000

# My Answer

* `dict`를 이용해서, `Trie`를 구성하자.
* `sentence`를 단어 기준으로 분리하고, 각 단어에 대해서 `Trie`에서 `prefix`를 찾자.
* 만약 `prefix`를 찾았다면, 해당 단어를 반환, 못 찾았다면 원래 단어를 반환
* 반환된 단어들을 이용해서 하나의 문장을 구성하자.

```java
class Solution {
    class Trie {
        public boolean isWord;
        public Trie[] children;
        public Trie(){
            children = new Trie[26];
        }    
        
        public void insert(String word) {
            Trie cur = this;
            
            for(char c : word.toCharArray()) {
                if ( cur.children[c-'a'] == null ) {
                    cur.children[c-'a'] = new Trie();
                }
                
                cur = cur.children[c-'a'];
            }
            
            cur.isWord = true;
        }
        
        public String replace(String word) {
            
            Trie cur = this;
            int i=0;
            for(char c : word.toCharArray()){
                if ( cur.children[c-'a'] == null ) {
                    return word;
                }
                
                cur = cur.children[c-'a'];
                i++;
                if ( cur.isWord )
                    break;
            }
            //check exist
            //not = return word
            //ok = rebuild word
            return word.substring(0, i);
        }
    }
    
    public String replaceWords(List<String> dict, String sentence) {
        Trie trie = new Trie();
        
        for( String word : dict) {
            trie.insert(word);
        }
        
        String[] words = sentence.split(" ");
        
        StringBuilder builder = new StringBuilder();
        for(int i=0;i<words.length;i++) {
            String word = words[i];
            word = trie.replace(word);    
            
            builder.append(word);
            
            if ( i != words.length-1) {
                builder.append(" ");
            }
        }
        
        return builder.toString();
        
    }
}
```

