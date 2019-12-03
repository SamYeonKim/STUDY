# Problem

Given a binary tree and a sum, determine if the tree has a root-to-leaf path such that adding up all the values along the path equals the given sum.

#### Note:

A leaf is a node with no children.

#### Example :

```swift
Input: 
[5,4,8,11,null,13,4,7,2,null,null,null,1]
22

      5
     / \
    4   8
   /   / \
  11  13  4
 /  \      \
7    2      1

Output: true
Explanation: as there exist a root-to-leaf path 5->4->11->2 which sum is 22.
```

# My Answer

* 재귀를 이용해서 해결
* `leaf` 이면서 최종 `sum`이 0일때만 `true`이다.
  
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
    public boolean hasPathSum(TreeNode root, int sum) {
        if ( root == null )
            return false;
        
        sum -= root.val;
        
        if ( root.left == null && root.right == null && sum == 0 )
            return true;
        
        if ( root.left != null ) {
            if ( hasPathSum(root.left, sum) )
                return true;
        }
        
        if ( root.right != null ) {
            if ( hasPathSum(root.right, sum))
                return true;
        }
        
        return false;
    }
}
```

