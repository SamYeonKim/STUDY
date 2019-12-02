# Problem

Given a binary tree, return the postorder traversal of its nodes' values.

#### Example :

```swift
Input: [1,null,2,3]
   1
    \
     2
    /
   3

Output: [3,2,1]
```

# My Answer ( Recursive )

* 재귀로 해결
* `solver` 함수에서 결과 리스트에 추가 하는 순서를 `left` -> `right` -> `node` 순으로 진행

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
    public List<Integer> postorderTraversal(TreeNode root) {
        List<Integer> result = new ArrayList<Integer>();
        
        solver(result, root);
        
        return result;
    }
    
    void solver(List<Integer> result, TreeNode root) {
        if ( root == null )
            return;
        
        if ( root.left != null ) {
            solver(result, root.left);
        }
        
        if ( root.right != null ) {
            solver(result, root.right);
        }
        
        result.add(root.val);
    }
}
```

# My Answer ( Iteration )

* 반복문을 이용해서 해결
* `Stack`에 순서대로 넣기만 하면 된다, 여기서 순서는 `node` -> `right` -> `left`
* 결과 리스트에 넣을땐 위 순서의 역순으로 집어 넣는다. 항상 첫번째 Index에 현재 노드의 값을 넣는다.
* `curr`가 `null`이 라는것은 오른쪽은 다 했다는 의미니까 `Stack`에 있는것의 `left`를 `curr`에 할당해 주자.

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
    public List<Integer> postorderTraversal(TreeNode root) {
        List<Integer> result = new ArrayList<>();
        Stack<TreeNode> stack = new Stack<TreeNode>();

        if (root == null) {
            return result;
        }
        TreeNode curr = root;
        while ( !stack.isEmpty() || curr != null) {
            if (curr != null) {
                stack.push(curr);
                result.add(0, curr.val);
                curr = curr.right;
            } else {
                TreeNode node = stack.pop();
                curr = node.left;
            }
        }
        return result;
    }
}
```
