# Aho-Corasick

- [Aho-Corasick](#aho-corasick)
    - [참조](#%ec%b0%b8%ec%a1%b0)
    - [정리](#%ec%a0%95%eb%a6%ac)
    - [구현](#%ea%b5%ac%ed%98%84)
    
### 참조

* [What is Aho-Corasick](https://www.geeksforgeeks.org/aho-corasick-algorithm-pattern-searching/)
* [Implemented by Java](https://github.com/robert-bor/aho-corasick/blob/master/src/main/java/org/ahocorasick/trie/Trie.java)
* [한글 자료 좋다!](https://m.blog.naver.com/kks227/220992598966)

### 정리

* 미리 구성해 놓은 패턴들을 이용해서 주어진 문장에 매치되는 패턴이 있는지 찾기 위한 알고리즘
* 채팅입력시 Slang이 포함되어 있는지 확인 하는 용도로 쓸 수 있겠다.
* 패턴을 구성해 놓는 시간이 오래 걸린다.
* 패턴을 찾는 속도는 엄청 빠르다.
    * 길이가 N인 문장(S)에서 찾아야하는 패턴 집합(P)의 갯수가 (K)이고 모든 패턴의 길이합이 M 이라면 다음과 같은 시간 복잡도가 된다고 한다.
    `O(N + M + K)`
    > 왜 그런지는 이해 못함. 그런데 실제로 Unity에서 3000개의 패턴을 이용해서 Naive하게 쭉 찾는거랑 비교하니까 말도 안되는 차이가 나온다.
    > TODO : 나중에 스샷 첨부하자.
* `Trie`, `Failure`, `Output`
    * `Trie` : 문자열을 [Tree](https://en.wikipedia.org/wiki/Tree_(data_structure)) 구조로 나타내는 방식, [예시](https://twpower.github.io/187-trie-concept-and-basic-problem)
    * `Failure` : 검색하다가 현재 노드와 불일치 할 때 다시 찾기 시작할 노드를 가리킨다.
    * `Output` : 각 패턴의 마지막 문자 노드

### 구현

`[C#]`

```cs
public class PatternNode {
    ///<summary>현재 노드의 char</summary>
    public readonly char m_value;
    ///<summary>종단 노드 인가?</summary>
    public bool m_b_term = false;
    ///<summary>실패 했을때 가리키는 노드</summary>
    PatternNode m_fail = null;
    ///<summary>다음 노드들</summary>
    List<PatternNode> m_l_next = new List<PatternNode>();
    
    public PatternNode(char c) {
        m_value = c;
    }           
    
    public bool ExistNextNode() {
        return m_l_next.Count > 0;
    }

    ///<summary>다수의 패턴을 추가 한다.</summary>
    public void InsertRange(List<string> items) {
        #region Trie 구성
        int n_count = items.Count;
        for (int i = 0; i < n_count; i++)
            Insert(items[i]);
        #endregion

        #region Failure 노드 구성
        Queue<PatternNode> queue = new Queue<PatternNode>();//깊이에 맞춰서 순환을 돌기 위해서 Queue 구성

        this.m_fail = this;                                 //루트 노드의 실패노드는 자기 자신
        for (int idx = 0; idx < m_l_next.Count; idx++) {    
            m_l_next[idx].m_fail = this;                    //Depth가 0인것들 ( 루트 노드의 다음 노드들 )은 실패 노드를 루트로 설정
            queue.Enqueue(m_l_next[idx]);
        }
        
        while(queue.Count != 0 ) {
            PatternNode current = queue.Dequeue();

            for (int idx = 0; idx < current.m_l_next.Count; idx++) {
                PatternNode next = current.m_l_next[idx];
                queue.Enqueue(next);

                PatternNode fail = current.m_fail;
                while( fail != this && fail.FindNextNode(next.m_value) == null ) {      //실패노드가 루트가 아닌데, 실패노드의 다음노드에 현재노드의 다음노드와 동일한 값에 해당하는 노드가 없을 경우
                    fail = fail.m_fail;
                }

                if ( fail.FindNextNode(next.m_value) != null ) {
                    fail = fail.FindNextNode(next.m_value);
                }

                next.m_fail = fail;
            }
        }
        #endregion
    }

    /*
    whitespace를 기준으로 단어를 쪼개기 때문에 
    예를 들어 ab를 체크해야 하는 패턴인데,
    `a b` 이런식의 문장이라면 체크 못한다.
    */
    ///<summary>문장 s에 대해서 패턴이 일치 하는 첫번째 단어를 찾는다.</summary>
    public string FindFirstMatchPattern(string sentence) {
        sentence = sentence.Trim();
        if ( string.IsNullOrEmpty(sentence) || sentence.Length == 1 )
            return string.Empty;
        
        string res = string.Empty;
        string[] arr_words = sentence.Split(null);
        int n_words_length = arr_words.Length;
        for (int idx = 0; idx < n_words_length; idx++) {
            string word = arr_words[idx];
            res = FindMatchedPattern(word);
            if ( !string.IsNullOrEmpty(res) ) {
                break;
            }
        }
        return res;
    }
    
    ///<summary>문장 s에 대해서 패턴이 일치 하는것이 하나라도 있나?</summary>
    public bool ExistPattern(string sentence) {
        sentence = sentence.Trim();
        if ( string.IsNullOrEmpty(sentence) || sentence.Length == 1 )
            return false;

        bool b_res = false;
        string[] arr_words = sentence.Split(null);
        int n_words_length = arr_words.Length;
        for (int idx = 0; idx < n_words_length; idx++) {
            string word = arr_words[idx];
            b_res = CheckPatternMatch(word);
            if ( b_res )
                break;
        }
        return b_res;
    }
    ///<summary>단어 word를 이용해서 Trie를 구성한다.</summary>
    void Insert(string word) {
        PatternNode current = this;
        
        foreach (char one_char in word) {
            PatternNode next = current.FindNextNode(one_char);
            if ( next == null ) {
                next = new PatternNode(one_char);
                current.m_l_next.Add(next);
            }
            current = next;                    
        }
        current.m_b_term = true;    
    }
    
    ///<summary>파라미터로 받은 단어가 다음 노드에 있는지 확인</summary>
    PatternNode FindNextNode(char one_char) {
        PatternNode next = null;
        int n_total_count = m_l_next.Count;
        for (int idx = 0; idx < n_total_count; idx++) {
            next = m_l_next[idx];
            if (next.m_value == one_char)
                return next;
        }
        return null;            
    }           
    
    ///<summary>단어 하나에 대해서 매치 되는 패턴 하나를 찾는다.</summary>
    string FindMatchedPattern( string word ) {
        System.Text.StringBuilder string_builder = new System.Text.StringBuilder();
        string res = string.Empty;

        PatternNode current = this;
        char[] arr_chars = word.ToCharArray();
        int n_length = arr_chars.Length;
        for (int idx = 0; idx < n_length; idx++) {              //String s를 char배열 형태로 변환한후 char를 비교                    
            PatternNode next = current.FindNextNode(arr_chars[idx]); //루트에서 부터 char와 매치되는 다음노드가 있는가?
            if ( next != null ) {
                current = next;                                 //매치되는 노드가 있다면, 다음 char의 매치여부 확인
                string_builder.Append(current.m_value);
            } else {
                if ( current != this ) {                        //매치되는 노드가 없는데, 루트가 아니라면, 지금 찾고 있었던 char를 한번더 찾는다.
                    idx--;                            
                }                        
                current = current.m_fail;                       //매치되는 노드가 없을땐, 실패 노드를 사용한다.
                string_builder.Remove(0, string_builder.Length);
            }

            if ( current.m_b_term ) {                        
                break;
            }
        }
        
        if ( current.m_b_term ) {
            res = string_builder.ToString();
        }
        return res;
    }

    ///<summary>단어 하나에 대해서 매치 되는 패턴이 있는지 확인</summary>
    bool CheckPatternMatch(string word) {
        PatternNode current = this;
        char[] arr_chars = word.ToCharArray();
        int n_length = arr_chars.Length;
        for (int idx = 0; idx < n_length; idx++) {              //String s를 char배열 형태로 변환한후 char를 비교                    
            PatternNode next = current.FindNextNode(arr_chars[idx]); //루트에서 부터 char와 매치되는 다음노드가 있는가?
            if ( next != null ) {
                current = next;                                 //매치되는 노드가 있다면, 다음 char의 매치여부 확인
            } else {
                if ( current != this ) {                        //매치되는 노드가 없는데, 루트가 아니라면, 지금 찾고 있었던 char를 한번더 찾는다.
                    idx--;                            
                }                        
                current = current.m_fail;                       //매치되는 노드가 없을땐, 실패 노드를 사용한다.
            }

            if ( current.m_b_term )                             //현재 노드가 종단이라면 더이상 찾을 필요도 없다.
                return true;
        }
        
        return current.m_b_term;
    }
}
```
                    