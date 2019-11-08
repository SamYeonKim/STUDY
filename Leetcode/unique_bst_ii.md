# Problem

Given an integer n, generate all structurally unique BST's (binary search trees) that store values 1 ... n.

#### Example :

```swift
Input: 3
Output:
[
  [1,null,3,2],
  [3,2,null,1],
  [3,1,null,null,2],
  [2,1,3],
  [1,null,2,null,3]
]
Explanation:
The above output corresponds to the 5 unique BST's shown below:

   1         3     3      2      1
    \       /     /      / \      \
     3     2     1      1   3      2
    /     /       \                 \
   2     1         2                 3
```

# My Answer

* `Binary Search Tree`는 `Parent Node` 보다 작은수가 왼쪽 큰 수가 오른쪽으로 위치한다.
* 작은 수에서 큰수로 반복 돌면서 좌, 우를 재귀로 구성.
  
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
    public List<TreeNode> generateTrees(int n) {
        if ( n < 1 ) {
            return new ArrayList<TreeNode>();
        }
        
        return generator(1, n);
    }
    
    List<TreeNode> generator(int low, int high ) {
        List<TreeNode> result = new ArrayList<TreeNode>();
        if ( low > high ) {            
            result.add(null);
            return result;
        }
        
        for(int n=low; n <= high; n++) {
            List<TreeNode> left_tree = generator(low, n -1);
            List<TreeNode> right_tree = generator(n+1, high);
            
            for(TreeNode l : left_tree) {
                for(TreeNode r : right_tree ) {
                    TreeNode root = new TreeNode(n);
                    root.left = l;
                    root.right = r;
                    result.add(root);
                }
            }
        }
        
        return result;
    }
}
```

