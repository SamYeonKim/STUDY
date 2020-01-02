# Problem

Given the root node of a binary search tree (BST) and a value to be inserted into the tree, insert the value into the BST. Return the root node of the BST after the insertion. It is guaranteed that the new value does not exist in the original BST.

Note that there may exist multiple valid ways for the insertion, as long as the tree remains a BST after insertion. You can return any of them.

#### Example :

```swift
Input: 
        4
       / \
      2   7
     / \
    1   3
    insert : 5
Output: 
         4
       /   \
      2     7
     / \   /
    1   3 5
```

# My Answer ( Iteration )
  
* 만약 `root`가 `null`이라면, `val`을 이용해서 새로운 `root`를 만들고 반환
* `leaf`노드가 `null`인 노드를 찾고 해당 노드의 `left`혹은 `right`에 새로운 노드를 만든다.

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
    public TreeNode insertIntoBST(TreeNode root, int val) {
        if ( root == null ) {
            root = new TreeNode(val);
            return root;
        }
        
        TreeNode curr = root;
        
        while ( curr != null ) {
            boolean is_left = curr.val > val;
            TreeNode next = is_left ? curr.left : curr.right; 
            
            if ( next == null ) {
                if ( is_left ) curr.left = new TreeNode(val);
                else curr.right = new TreeNode(val);
                break;
            } else {
                curr = next;
            }            
        }
        
        return root;
    }
}
```

# My Answer ( Recursive )

* 재귀를 이용해서 해결

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
    public TreeNode insertIntoBST(TreeNode root, int val) {
        if ( root == null ) {
            root = new TreeNode(val);
            return root;
        }
        
        _insertIntoBST(root, val);
        
        return root;
    }
    
    void _insertIntoBST(TreeNode root, int val) {
        boolean is_left = root.val > val;
        TreeNode next = is_left ? root.left : root.right; 
            
        if ( next == null ) {
            if ( is_left ) root.left = new TreeNode(val);
            else root.right = new TreeNode(val);
            return;
        }
        
        if ( is_left ) {
            insertIntoBST(root.left, val);
        } else {
            insertIntoBST(root.right, val);
        }
    }
}
```

