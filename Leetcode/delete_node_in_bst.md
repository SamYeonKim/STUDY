# Problem

Given a root node reference of a BST and a key, delete the node with the given key in the BST. Return the root node reference (possibly updated) of the BST.

Basically, the deletion can be divided into two stages:

Search for a node to remove.
If the node is found, delete the node.

#### Example :

```swift
Input: 
root = [5,3,6,2,4,null,7]
key = 3
    5
   / \
  3   6
 / \   \
2   4   7

Output: [5,4,6,2,null,null,7]
    5
   / \
  4   6
 /     \
2       7


Explanation: Given key to delete is 3. So we find the node with value 3 and delete it.
```

# My Answer

* `key`에 해당하는 `target`노드를 찾는다.
* 만약 `target` 노드의 자식이 없는 경우, `target`노드가 이전 노드의 왼쪽이면 `prev.left`를 오른쪽이면 `prev.right`를 `null`로 한다.
* 만약 `target` 노드의 자식이 하나만 있는 경우, `target`노드의 `left`가 있다면 `prev.left = target.left`, `right`가 있다면 `prev.right = target.right`
* 만약 `target` 노드의 자식이 둘 다 있는 경우, `target`노드의 자식 중 적절한 노드를 찾는다. `target.right`부터해서 `left`가 `null`이 아닐때 까지 찾는다.
    > `BST`에서 `root`를 대체 하기 가장 좋은 노드는 오른쪽의 가장 왼쪽 즉 오른쪽 노드들 중 가장 작은 것이다.
    * 적절한 자식노드를 찾았다면, 해당 노드의 `right`가 `null`이 아니라면 이전 노드의 `left`에 할당하고, 그렇지 않다면 이전 노드의 `left`에 `null`을 할당한다.
        > 위 작업에 의해 적절한 자식노드가 없어져서 빈공간이 발생하는 것을 해결한다.
    * `target`노드의 값을 찾은 적절한 자식노드의 값으로 변경해 준다.
  
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
    public TreeNode deleteNode(TreeNode root, int key) {
        TreeNode prev = null;
        TreeNode curr = root;        
        boolean is_left = false;
        
        while( curr != null ) {            
            if ( curr.val == key ) {
                if ( curr.left == null && curr.right == null ) {        //자식이 아무것도 없는 경우
                    if ( prev == null ) {
                        root = null;                        
                    } else {
                        if ( is_left ) prev.left = null;
                        else prev.right = null;
                    }
                } else if ( curr.left != null && curr.right != null ) { //자식이 둘다 있는 경우
                    TreeNode properChild = searchChild(curr);
                    
                    if( curr.right.val == properChild.val ) {
                        curr.right = properChild.right;
                    }
                    
                    curr.val = properChild.val;
                } else {    //자식이 둘중 하나만 있는 경우
                    if ( prev == null ) {
                        if ( curr.left != null ) {
                            root = curr.left;
                        } else {
                            root = curr.right;
                        }                            
                    } else {
                        if ( is_left ) {
                            prev.left = curr.left != null ? curr.left : curr.right;                     
                        } else {
                            prev.right = curr.left != null ? curr.left : curr.right;                     
                        }
                        
                    }                    
                }
                break;
            } else {
                is_left = curr.val > key;
                prev = curr;
                curr = is_left ? curr.left : curr.right;    
            }
        }
        
        return root;        
    }
    
    TreeNode searchChild(TreeNode root) {
        TreeNode prev = root;
        TreeNode curr = root.right;
        
        while( curr.left != null ) {
            prev = curr;
            curr = curr.left;
            
            if ( curr.left == null ) {
                if ( curr.right != null ) 
                    prev.left = curr.right;  
                else 
                    prev.left = null;
            } 
        }
        
        return curr;        
    }
}
```

# Fastest Answer

* 재귀를 이용
* 순회 보다 훨씬 깔끔하다.

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
    public TreeNode deleteNode(TreeNode root, int key) {
        if (root == null) {
            return root;
        }
        if (root.val == key) {
            // only one children
            if (root.left == null) {
                return root.right;
            } else if (root.right == null) {
                return root.left;
            }
            // two children
            TreeNode suc = inorderSuc(root.right);
            root.val = suc.val;
            root.right = deleteNode(root.right, root.val);
        } else if (root.val > key) {
            root.left = deleteNode(root.left, key);
        } else {
            root.right = deleteNode(root.right, key);
        }
        return root;
    }
    
    public TreeNode inorderSuc(TreeNode root) {
        while (root.left!= null) {
            root = root.left;
        }
        return root;
    }
}
```