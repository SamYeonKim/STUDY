# Problem

Given preorder and inorder traversal of a tree, construct the binary tree.

#### Note:

You may assume that duplicates do not exist in the tree.

#### Example :

```swift
Input: 
preorder = [3,9,20,15,7]
inorder = [9,3,15,20,7]
Output: 
    3
   / \
  9  20
    /  \
   15   7
```

# My Answer

* 재귀를 이용해서 해결
* `preorder` 의 `i_l`의 위치에 있는 값이 `Node`의 값이다.        
* `inorder`에서 `Node` 값의 `index` 기준으로 왼쪽 값들이 left를 구성할 후보군, 오른쪽 값들이 right를 구성할 후보군이다.
* 만약 각 후보군이 하나라면, 해당 값으로 `left or right`의 값이다.
* 만약 `left` 후보군이 다수 라면, `generateTree` 함수를 재귀 호출 하면서 `inorder` 배열의 `0~index` 까지를 파라미터로 넘기고, `i_l` 에서 하나 더한 값을 파라미터로 넘긴다.
* 만약 `right` 후보군이 다수 라면, `generateTree` 함수를 재귀 호출 하면서 `inorder` 배열의 `index+1~inorder갯수` 까지를 파라미터로 넘기고, `i_l` 에서 `index` + 1을 더한값을 파라미터로 넘긴다.
* 위 예제는 다음과 같은 흐름으로 진행 된다.
    ```swift
    1. preorder = [3,9,20,15,7], inorder = [9,3,15,20,7], i_l = 0    
    preorder의 i_l에 해당하는 index의 값은 3이기 때문에 root의 값은 3
    inorder에서 3의 인덱스는 1이므로, [9]가 left 후보군, [15,20,7]이 오른쪽 후보군
    left 후보군의 갯수가 1개 이기 때문에 root.left는 9
    right 후보군의 갯수가 1개 이상 이기 때문에 재귀 호출 하면서, i_l + 1 + 1인 2를 넘김
    2. preorder = [3,9,20,15,7], inorder = [15,20,7], i_l = 2    
    preorder의 i_l에 해당하는 index의 값은 20이기 때문에 root의 값은 20
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
    public TreeNode buildTree(int[] preorder, int[] inorder) {
        if ( preorder == null || preorder.length == 0 )
            return null;
        
        TreeNode root = generateTree(preorder, inorder, 0);
        
        return root;
    }
    
    TreeNode generateTree(int[] preorder, int[] inorder, int i_l) {
        TreeNode root = new TreeNode(preorder[i_l]);
        
        int pivot = -1;     //inorder에서의 root의 값과 동일한 값의 index
        for(int i=0;i<inorder.length;i++) {
            if ( inorder[i] == root.val) {
                pivot = i;
                break;
            }                
        }
        
        if ( pivot == 1 ) {     //pivot 기준으로 왼쪽에 하나 밖에 없다면, 재귀 호출 필요 없음
            root.left = new TreeNode(inorder[0]);
        } else if ( pivot >= 2 ) {     //pivot 기준으로 왼쪽에 두개 이상이라면, 재귀 호출
            int[] left_array = Arrays.copyOfRange(inorder, 0, pivot);
            root.left = generateTree(preorder, left_array, i_l+1);
        }
        
        if ( inorder.length - pivot == 2 ) {    //pivot 기준으로 오른쪽에 하나 밖에 없다면, 재귀 호출 필요 없음
            root.right = new TreeNode(inorder[inorder.length - 1]);
        } else if ( inorder.length - pivot >= 3 ) { //pivot 기준으로 오른쪽에 두개 이상이라면, 재귀 호출
            int[] right_array = Arrays.copyOfRange(inorder, pivot +1 , inorder.length);
            // i_l에서 하나 다음 index에다가, 왼쪽 후보군의 갯수 (pivot)
            root.right = generateTree(preorder, right_array, i_l+1 +  pivot);
        }
        
        return root;
    }
}
```

