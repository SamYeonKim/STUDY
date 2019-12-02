# Problem

Given a binary tree, return the preorder traversal of its nodes' values.

#### Example :

```swift
Input: [1,null,2,3]
   1
    \
     2
    /
   3

Output: [1,2,3]
```

# My Answer ( Recursive )

* 재귀로 해결
* `solver` 함수에서 결과 리스트에 추가 하는 순서를 `node` -> `left` -> `right` 순으로 진행

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
    public List<Integer> preorderTraversal(TreeNode root) {
        List<Integer> result = new ArrayList<Integer>();
        
        solver(result, root);
        
        return result;
    }
    
    void solver(List<Integer> result, TreeNode root) {
        if ( root == null )
            return;
        
        result.add(root.val);
        
        if ( root.left != null ) {
            solver(result, root.left);
        }
        
        if ( root.right != null ) {
            solver(result, root.right);
        }
    }
}
```

# My Answer ( Iteration )

* 반복문을 이용해서 해결
* `curr`이 `null`이 될때 까지 반복 수행
* 일단 `Stack`에 `curr`을 넣자.
* 만약 `curr`의 `left`가 `null`인 경우엔 `Stack`에 있는것들 중에서 `right`가 `null`이 아닌것을 찾고 `curr`에 찾은것의 `right`를 넣으면 된다.
* 순서가 `root` -> `left` -> `right` 이기 때문에, `left`가 `null`이 라는것은 현재 노드에서 더이상 왼쪽으로 갈 것이 없다는 것이니 오른쪽이 있는 것을 찾아서 그 오른쪽을 다음 결과 리스트에 넣을 타겟 노드로 지정 하면 된다.

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
    public List<Integer> preorderTraversal(TreeNode root) {
        List<Integer> result = new ArrayList<Integer>();
        
        Stack<TreeNode> s = new Stack<TreeNode>(); 
        TreeNode curr = root; 
        
        while ( curr != null ) {            
            result.add(curr.val);
            s.push(curr);
            curr = curr.left;
            
            if ( curr == null ) {
                while( s.size() > 0 ) {
                    curr = s.pop().right;
                    if ( curr != null )
                        break;
                }    
            }
        }
        
        return result;
    }
}
```
