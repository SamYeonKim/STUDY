# Problem

Given a binary tree, find its maximum depth.

The maximum depth is the number of nodes along the longest path from the root node down to the farthest leaf node.

A leaf is a node with no children.

#### Note:

* Note that in some languages such as Java, there is no unsigned integer type. In this case, both input and output will be given as signed integer type and should not affect your implementation, as the internal binary representation of the integer is the same whether it is signed or unsigned.
* In Java, the compiler represents the signed integers using 2's complement notation. Therefore, in Example 2 above the input represents the signed integer -3 and the output represents the signed integer -1073741825.

#### Example :

```swift
Input:
[3,9,20,null,null,15,7],

    3
   / \
  9  20
    /  \
   15   7

Output: 3
```

# My Answer ( Recursive )

* 재귀를 이용하는데, `TreeNode::left`, `TreeNode::right`가 있으니 각각의 최대 깊이를 구해야 한다.
* 현재 `root`가 `null`이 아니라는것은 기본적으로 1의 값을 가지고 있다는 것이고, 자식 노드(left, right)의 최대 깊이 와 더하면 현재 root의 최대 깊이가 나온다.
  
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
    public int maxDepth(TreeNode root) {
        if ( root == null )
            return 0;
        
        int n_max_left = 0;
        int n_max_right = 0;
        int n_max_child = 0;
        
        if ( root.left != null ) {
            n_max_left = maxDepth(root.left);
        }
        
        if ( root.right != null ) {
            n_max_right = maxDepth(root.right);
        }
        
        n_max_child = n_max_left > n_max_right ? n_max_left : n_max_right;
        
        return 1 + n_max_child;
    }    
}
```

# My Answer ( Iteration )

* 반복문 이용
* `Queue`를 이용해서 뎁스 별로 자식 노드를 추가 하는 식으로 구현
* `q.size()` 가 의미 하는것이 현재 뎁스의 노드의 갯수 이다.
  
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
    public int maxDepth(TreeNode root) {
        int n_depth = 0;
        if ( root == null )
            return 0;
        Queue<TreeNode> q = new LinkedList<>();
        q.add(root);
        
        while (!q.isEmpty()) {               
            int n = q.size();
            for(int i = 0;i< n; i++) {
                TreeNode node = q.poll();    
                
                if ( node.left != null )
                    q.add(node.left);
                if ( node.right != null )
                    q.add(node.right);
            }
            n_depth++;   
        }
        
        return n_depth;
    }
}
```
