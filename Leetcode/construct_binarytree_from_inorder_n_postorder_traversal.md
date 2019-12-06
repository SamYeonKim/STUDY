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
* `postorder` 의 `i_r`의 위치에 있는 값이 `Node`의 값이다.        
* `inorder`에서 `Node` 값의 `index` 기준으로 왼쪽 값들이 left를 구성할 후보군, 오른쪽 값들이 right를 구성할 후보군이다.
* 만약 각 후보군이 하나라면, 해당 값으로 `left or right`의 값이다.
* 만약 `left` 후보군이 다수 라면, `generateTree` 함수를 재귀 호출 하면서 `inorder` 배열의 `0~index` 까지를 파라미터로 넘기고, `i_r` 에서 `오른쪽 후보군의 갯수 + 1`만큼을 뺀 값을 파라미터로 넘긴다.
* 만약 `right` 후보군이 다수 라면, `generateTree` 함수를 재귀 호출 하면서 `inorder` 배열의 `index+1~inorder갯수` 까지를 파라미터로 넘기고, `i_r` 에서 하나 뺀 값을 파라미터로 넘긴다.
* 위 예제는 다음과 같은 흐름으로 진행 된다.
    ```swift
    1. inorder = [9,3,15,20,7], postorder = [9,15,7,20,3], i_r = 4    
    postorder의 i_r에 해당하는 index의 값은 3이기 때문에 root의 값은 3
    inorder에서 3의 인덱스는 1이므로, [9]가 left 후보군, [15,20,7]이 오른쪽 후보군
    left 후보군의 갯수가 1개 이기 때문에 root.left는 9
    right 후보군의 갯수가 1개 이상 이기 때문에 재귀 호출 하면서, 마지막 인덱스 -1인 3을 넘김
    2. inorder = [15,20,7], postorder = [9,15,7,20,3], i_r = 3    
    postorder의 i_r에 해당하는 index의 값은 20이기 때문에 root의 값은 20
    inorder에서 20의 인덱스는 1이므로, [15]가 left 후보군, [7]이 오른쪽 후보군
    left 후보군의 갯수가 1개 이기 때문에 root.left는 15
    right 후보군의 갯수가 1개 이기 때문에 root.right는 7
    ```

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

        int i = -1;     //inorder의 기준이 될 숫자의 index = pivot        
        for(int n=0;n < inorder.length;n++) {
            if ( inorder[n] == postorder[i_r]) {
                i = n;
                break;
            }                
        }
        
        if ( i + 1 == inorder.length - 1 ) {    //pivot 기준으로 우측에 1개 밖에 없다면, 재귀 호출 필요 없이 바로 root.right
            root.right = new TreeNode(inorder[inorder.length - 1]);
        } else if ( i >= 0 && i + 2< inorder.length ) {     //pivot 기준으로 우측에 2개 이상 있다면, 재귀 호출
            int[] right_array = Arrays.copyOfRange(inorder, i+1, inorder.length);    
            root.right = generateTree(right_array, postorder, i_r - 1);
        }
        
        if ( i-1 == 0 ) {       //pivot 기준으로 좌측에 1개 밖에 없다면, 재귀 호출 필요 없이 바로 root.left
            root.left = new TreeNode(inorder[0]);
        } else if ( i -1 > 0 ) {    //pivor 기준으로 좌측에 2개 이상 있다면, 재귀 호출
            int[] left_array = Arrays.copyOfRange(inorder, 0, i); 
            //다음 i_r의 값은 현재 i_r에서 pivot 기준 오른쪽 요소들의 갯수 + 1을 뺀 값
            root.left = generateTree(left_array, postorder, i_r -1 - (inorder.length - i -1));
        } 
        
        return root;
    }
}
```

