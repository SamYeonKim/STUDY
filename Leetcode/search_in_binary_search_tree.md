# Problem

Given the root node of a binary search tree (BST) and a value. You need to find the node in the BST that the node's value equals the given value. Return the subtree rooted with that node. If such node doesn't exist, you should return NULL.

#### Example :

```swift
Input: 
        4
       / \
      2   7
     / \
    1   3
    search : 2
Output: 
      2     
     / \   
    1   3
```

# My Answer

* `BST` 이기 때문에 `nxt.val`과 `val`의 차이에 따라 `nxt`혹은 결과가 나온다.
* 만약 `nxt.val`이 `val`보다 작다면, 정답은 `nxt`의 우측에 있다는 의미
* 만약 `nxt.val`이 `val`보다 크다면, 정답은 `nxt`의 좌측에 있다는 의미
* 만약 `nxt.val`이 `val`과 같다면, 정답
  
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
    public TreeNode searchBST(TreeNode root, int val) {
        TreeNode res = null;
        TreeNode nxt = root;
        
        while ( nxt != null ) {
            if ( nxt.val == val ) {
                res = nxt;
                break;
            }    
            
            if ( nxt.val < val ) nxt = nxt.right;
            else nxt = nxt.left;
        }
        
        return res;
    }
}
```

