# Problem

Given a binary tree, return all duplicate subtrees. For each kind of duplicate subtrees, you only need to return the root node of any one of them.

Two trees are duplicate if they have the same structure with same node values.

#### Example :

```swift
        1
       / \
      2   3
     /   / \
    4   2   4
       /
      4

The following are two duplicate subtrees.

      2
     /
    4
and
    4
```

# Solution
  
* `HashMap`을 이용하자. 각 `TreeNode`별로 고유값을 반환할 `lookup`함수를 이용.
* `lookup`함수에선, 각 `TreeNode`별 고유문자열을 만들고 이것을 키로 하고, 고유문자열에 대응되는 번호를 매겨서 값으로 사용하는 `HashMap trees`를 사용
* `trees`의 값 별로 발생 횟수를 저장할 `HashMap count`를 사용
* 만약 `count`의 값이 `2`라면, 중복 발생했다는 의미 이기 때문에, 결과 리스트에 추가

```java
/**
 * Definition for a binary tree node.
 * public class TreeNode {
 *     int val;
 *     TreeNode left;
 *     TreeNode right;
 *     TreeNode(int x) { val = x; }
 * }
 */
class Solution {
    int t;                          //고유 번호를 생성하기 위한 변수
    Map<String, Integer> trees;     //각 TreeNode의 고유 문자열, 고유 번호
    Map<Integer, Integer> count;    //각 TreeNode의 고유 번호, 발생 횟수
    List<TreeNode> ans;             //결과 리스트

    public List<TreeNode> findDuplicateSubtrees(TreeNode root) {
        t = 1;
        trees = new HashMap();
        count = new HashMap();
        ans = new ArrayList();
        lookup(root);
        return ans;
    }

    public int lookup(TreeNode node) {
        if (node == null) return 0;
        /*
        1. 고유 문자열 생성, 현재 TreeNode::val + "," + TreeNode::left의 고유번호 + "," + TreeNode::right의 고유번호
        2. 고유 문자열을 이용한 고유번호 생성 및 trees에 기록
        3. 고유번호의 발생 횟수 기록
        4. 고유번호의 발생 횟수가 2이면 정답
        */
        String serial = node.val + "," + lookup(node.left) + "," + lookup(node.right);        
        int uid = trees.computeIfAbsent(serial, x-> t++);
        count.put(uid, count.getOrDefault(uid, 0) + 1);
        if (count.get(uid) == 2)
            ans.add(node);
        return uid;
    }
}
```

