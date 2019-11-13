# Problem

Given a binary tree, determine if it is a valid binary search tree (BST).

Assume a BST is defined as follows:

* The left subtree of a node contains only nodes with keys less than the node's key.
* The right subtree of a node contains only nodes with keys greater than the node's key.
* Both the left and right subtrees must also be binary search trees.

#### Example 1:

```swift
    2
   / \
  1   3

Input: [2,1,3]
Output: true
```

#### Example 2:

```swift
    5
   / \
  1   4
     / \
    3   6

Input: [5,1,4,null,null,3,6]
Output: false
Explanation: The root node's value is 5 but its right child's value is 4.
```

# My Answer
* 현재 노드가 Null이라면 true
* 현재 노드의 값이 최소값과 최대값 사이에 있는지 확인해야 한다.
* 자식 노드가 없다면 true
* 재귀를 이용해서 왼쪽과 오른쪽의 검증도 수행
* 오른쪽의 최소 값은 현재 노드의 값이다. BST의 오른쪽 노드는 현재 노드의 값보다 커야 하기 때문
* 왼쪽의 최대 값은 현재 노드의 값이다. BST의 왼쪽 노드는 현재 노드의 값보다 작아야 하기 때문

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
    public boolean isValidBST(TreeNode root) {
        return isValidBST(root, null, null);
    }
    
    boolean isValidBST(TreeNode node, Integer min, Integer max ) {
        if ( node == null )
            return true;
        
        int val = node.val;
        if (min != null && val <= min)
            return false;
        
        if (max != null && val >= max)
            return false;

        if ( node.left == null && node.right == null )
            return true;
        
        if (! isValidBST(node.right, val, max))
            return false;
        
        if (! isValidBST(node.left, min, val))
            return false;
            
        return true;        
    }
}
```