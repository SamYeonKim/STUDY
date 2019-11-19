# Problem

Given a binary tree, return the inorder traversal of its nodes' values.

#### Example :

```swift
Input: [1,null,2,3]
   1
    \
     2
    /
   3

Output: [1,3,2]
```

# My Answer ( Recursive )

* 재귀로 해결
* `solver` 함수에서 결과 리스트에 추가 하는 순서를 `left` -> `node` -> `right` 순으로 진행

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
    public List<Integer> inorderTraversal(TreeNode root) {
        List<Integer> result = new ArrayList<Integer>();
        
        solver(result, root);
        
        return result;        
    }
    
    void solver(List<Integer> ans, TreeNode node) {       
        if ( node == null )
            return;
        
        if ( node.left != null ) {
            solver(ans, node.left);
        }
        
        ans.add(node.val);
        
        if ( node.right != null ) {
            solver(ans, node.right);
        }
    }
}
```

# My Answer ( Iteration )

* 반복문을 이용해서 해결
* `left` 노드를 우선으로 해서 `Stack`에 넣는다.
* `left` 노드의 가장 마지막 `leaf node` 가 `curr`이 되는 순간 `curr.left`는 `Null` 이기때문에 `while (curr !=  null) { ` 구문에서 빠져 나오게 되고, `Stack`의 최상단은 마지막 `curr`이 들어가 있다. 결과 리스트에 `curr`의 값을 넣고 `curr.right`를 넣는데, 이때 `leaf node`의 `right`는 `Null` 일것이기 때문에 `while (curr !=  null) { `구문을 수행 하지 않고, `curr = s.pop()`이 수행되서 이전 `left, leaf node`의 `parent node`를 가리키게 된다.

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
    public List<Integer> inorderTraversal(TreeNode root) {
        List<Integer> result = new ArrayList<Integer>();
        
        Stack<TreeNode> s = new Stack<TreeNode>(); 
        TreeNode curr = root; 

        while (curr != null || s.size() > 0) { 
            while (curr !=  null) {     //Left 노드 우선으로 Stack에 넣는다, 만약 기존 curr이 right이면서 leafnode 라면 s의 상단에 추가 된다.
                s.push(curr); 
                curr = curr.left; 
            } 
  
            curr = s.pop();
  
            result.add(curr.val);
  
            curr = curr.right; 
        } 
        
        return result;        
    }    
}
```
