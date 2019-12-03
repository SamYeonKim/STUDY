# Problem

Given a binary tree, check whether it is a mirror of itself (ie, symmetric around its center).

#### Example 1:

```swift
    1
   / \
  2   2
 / \ / \
3  4 4  3

Input: [1,2,2,3,4,4,3]
Output: true
```

#### Example 2:

```swift
    1
   / \
  2   2
   \   \
   3    3

Input: [1,2,2,null,3,null,3]
Output: false
```

# My Answer ( Recursive )

* 재귀를 이용해서 해결
* `isMirror` 함수를 이용해서 확인
* `t1` 과 `t2` 둘 다 `null` 이라면 `true`
* `t1` 과 `t2` 둘 중에 하나만 `null` 이라면 `false`
* 둘 다 `null`이 아닐땐, 둘다 값이 동일 하고 재귀 호출로 `t1.right`, `t2.left`를 확인하고, `t1.left, t2.right`를 확인해서 결정

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
    public boolean isSymmetric(TreeNode root) {
        return isMirror(root, root);
    }
    
    public boolean isMirror(TreeNode t1, TreeNode t2) {
        if (t1 == null && t2 == null) return true;
        if (t1 == null || t2 == null) return false;
        return (t1.val == t2.val)
            && isMirror(t1.right, t2.left)
            && isMirror(t1.left, t2.right);
    }
}
```

# My Answer ( Iteration )

* 반복문을 이용해서 해결
* `Queue`에 2개씩 넣고 조건에 맞춰서 비교 하자.

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
    public boolean isSymmetric(TreeNode root) {
        if ( root == null )
            return true;
        
        Queue<TreeNode> q = new LinkedList<>();
        
        q.add(root);
        q.add(root);
        
        while(!q.isEmpty()) {
            TreeNode t1 = q.poll();
            TreeNode t2 = q.poll();
            
            if ( t1 == null && t2 == null )
                continue;
            
            if ( t1 == null || t2 == null )
                return false;
            
            if ( t1.val != t2.val )
                return false;
            
            q.add(t1.left);
            q.add(t2.right);
            q.add(t1.right);
            q.add(t2.left);
        }
        
        return q.isEmpty();
    }
}
```