# Problem

Given inorder and postorder traversal of a tree, construct the binary tree

#### Note:

You may assume that duplicates do not exist in the tree.

#### Example :

```swift
Input: 
inorder = [9,3,15,20,7]
postorder = [9,15,7,20,3]
Output: 
    3
   / \
  9  20
    /  \
   15   7
```

# My Answer

* 재귀를 이용해서 해결
* `postorder` 의 `i_r`의 위치에 있는 값이 root이다.        
* * inorder에서 root 값 기준으로 왼쪽이 left를 구성할 후보군, 오른쪽이 right를 구성할 후보군이다.


* 만약 후보군이 하나라면 당첨, 후보군이 여러개 라면 한번더 구성
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
    public TreeNode buildTree(int[] inorder, int[] postorder) {
        if ( inorder == null || inorder.length == 0 )
            return null;     
        
        TreeNode root = generateTree(inorder, postorder, postorder.length - 1);
        
        return root;
    }
    
    TreeNode generateTree(int[] inorder, int[] postorder, int i_r) {
        TreeNode root = new TreeNode(postorder[i_r]);    

        int i = -1;        
        for(int n=0;n < inorder.length;n++) {
            if ( inorder[n] == postorder[i_r]) {
                i = n;
                break;
            }                
        }
        
        if ( i + 1 == inorder.length - 1 ) {
            root.right = new TreeNode(inorder[inorder.length - 1]);
        } else if ( i >= 0 && i + 2< inorder.length ) {
            int[] right_array = Arrays.copyOfRange(inorder, i+1, inorder.length);    
            root.right = generateTree(right_array, postorder, i_r - 1);
        }
        
        if ( i-1 == 0 ) {
            root.left = new TreeNode(inorder[0]);
        } else if ( i -1 > 0 ) {
            int[] left_array = Arrays.copyOfRange(inorder, 0, i); 
            root.left = generateTree(left_array, postorder, i_r -1 - (inorder.length - i -1));
        } 
        
        return root;
    }
}
```

