# Problem

Given a binary tree, determine if it is height-balanced.

For this problem, a height-balanced binary tree is defined as: `a binary tree in which the left and right subtrees of every node differ in height by no more than 1.`

#### Example 1:

```swift
Input: [3,9,20,null,null,15,7]
    3
   / \
  9  20
    /  \
   15   7
Output: true
```

#### Example 2:

```swift
Input: [1,2,2,3,3,null,null,4,4]
       1
      / \
     2   2
    / \
   3   3
  / \
 4   4
Output: false
```

# My Answer

* 특정 `TreeNode` 기준으로 최대 깊이 값을 구하는 `getMaxHeight` 함수를 이용하자.
* `isBalanced`에서 `left, right` 기준으로 최대 깊이를 구하고, 깊이의 차가 1보다 같거나 작으면 `left, right`에 대해서 계속 확인 하자. `left`나 `right` 둘 중에 하나라도 만족하지 않는다면 `false`
* 더 최적화할 여지가 있을것 같다.
  * `getMaxHeight`함수에 의해서 각 노드에 대해서 최대 높이 값을 구해놓은 상태 이기 때문에, `isBalanced`를 재귀 호출 할 때 다른식으로 할 수 있지 않을까?
  
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
    public boolean isBalanced(TreeNode root) {
        if ( root == null )
            return true;

        int height_l = getMaxHeight(root.left, 0);
        int height_r = getMaxHeight(root.right, 0);
        
        if ( Math.abs(height_r - height_l) <= 1 ) { //현재 노드 기준으로는 만족 했다. left, right 노드에 대해서 확인 하자.
            return isBalanced(root.left) && isBalanced(root.right);    
        } else {
            return false;
        }
    }
    
    int getMaxHeight(TreeNode root, int height) {
        if ( root == null )
            return height;
        
        height++;
            
        if ( root.left == null && root.right == null )
            return height;
        
        int height_l = getMaxHeight(root.left, height);
        int height_r = getMaxHeight(root.right, height);
        
        return Math.max(height_l, height_r);
    }
}
```

