# Problem

Given a binary tree, find the lowest common ancestor (LCA) of two given nodes in the tree.

According to the definition of LCA on Wikipedia: “The lowest common ancestor is defined between two nodes p and q as the lowest node in T that has both p and q as descendants (where we allow a node to be a descendant of itself).”

Given the following binary tree:  root = [3,5,1,6,2,0,8,null,null,7,4]

![](img/lca_tree.png)

#### Example 1:

```swift
Input: root = [3,5,1,6,2,0,8,null,null,7,4], p = 5, q = 1
Output: 3
Explanation: The LCA of nodes 5 and 1 is 3.
```

#### Example 2:

```swift
Input: root = [3,5,1,6,2,0,8,null,null,7,4], p = 5, q = 4
Output: 5
Explanation: The LCA of nodes 5 and 4 is 5, since a node can be a descendant of itself according to the LCA definition.
```

#### Notes

* All of the nodes' values will be unique
* p and q are different and both values will exist in the binary tree


# My Answer

* `p` 와 `q`에 대해서 각각의 경로를 구한다.
* 각 경로를 하나씩 비교해 나가면서 값이 달라지는 순간 비교를 끝낸다.
* `search` 함수에서 원하는 값에 대응되는 `TreeNode` 를 찾을때 까지 재귀호출 하고, 찾게되면 `Stack`에 하나씩 넣는다.
* 각 `Stack`을 순회 하면서, `TreeNode`의 값이 달라지는 순간 까지 순회 한다.
* 값이 달라지기 직전의 `TreeNode`가 `LCA`이다.
  
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
    public TreeNode lowestCommonAncestor(TreeNode root, TreeNode p, TreeNode q) {
        Stack<TreeNode> history_q = new Stack<>();
        Stack<TreeNode> history_p = new Stack<>();
        
        search(root, p.val, history_p);
        search(root, q.val, history_q);
        
        TreeNode result = root;
        
        while ( !history_p.isEmpty() && !history_q.isEmpty()) {
            TreeNode root_p = history_p.pop();    
            TreeNode root_q = history_q.pop();    
            
            if ( root_p.val != root_q.val )
                break;
            
            result = root_p;    //둘중에 뭘 넣든 상관없다. 값이 같다는것은 똑같은 객체라는 것이니까.
        }
            
        return result;
    }
    
    boolean search(TreeNode root, int val, Stack<TreeNode> history) {
        if ( root.val == val ) {
            history.push(root);
            return true;
        }
            
        if ( root.left != null && search(root.left, val, history)) {
            history.push(root);
            return true;
        }
        
        if ( root.right != null && search(root.right, val, history)) {
            history.push(root);
            return true;
        }
        
        return false;
    }
}
```
