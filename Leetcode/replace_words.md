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

# Fastest Answer

* `String::split` 과 `StringBuilder`를 사용해서 재 조합하는 대신 `String::substring`을 이용하도록 변경
* `start, end`를 이용, 각각 `sentence`에서 단어의 `시작 index`와 `끝 + 1 index`를 의미
* `Trie::replace`를 `Trie::getPrefixLength` 로 변경하고, 기능을 특정 단어에 대해서 변경해야 할 단어가 있는지 확인후 변경할 것이 있다면, 변경해야 하는 단어의 길이를 반환.
* 만약 `Trie::getPrefixLength`의 반환 값이 0보다 크다면, 기존 단어에서 필요 없는 부분을 잘라내야 한다. 그래서 `0 ~ start + len` 부분과 `end ~` 부분을 합쳐서 새로운 `sentence`를 만든다. 

```java
class Solution {
    class Trie {
        public boolean isWord;
        public Trie[] children;
        
        public Trie() {
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
        
        public int getPrefixLength(String sentence, int start, int end ) {
            String word = sentence.substring(start,end);
            
            Trie cur = this;
            
            int len = 0;
            
            for(char c : word.toCharArray()) {
                if ( cur.children[c-'a'] == null ) {
                    return -1;
                }
                
                len++;
                cur = cur.children[c-'a'];
                if ( cur.isWord )
                    break;
            }                    
            
            return len;            
        }
    }
    
    public String replaceWords(List<String> dict, String sentence) {
        Trie trie = new Trie();
        for(String word : dict ) {
            trie.insert(word);
        }

        int start = 0;
        int end = 0;    
        
        while(start < sentence.length() ) {
            end = sentence.indexOf(' ', start);
            if ( end == -1 ) {
                end = sentence.length();
            }
            
            int len = trie.getPrefixLength(sentence, start, end );
            
            if ( len > 0 ) {
                sentence = sentence.substring(0, start + len) + sentence.substring(end);
                start = start + len + 1;
            } else {
                start = end + 1;    
            }
        }
        
        return sentence;
    }
}
```

