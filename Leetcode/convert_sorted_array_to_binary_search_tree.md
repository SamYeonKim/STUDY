# Problem

Given an array where elements are sorted in ascending order, convert it to a height balanced BST.

For this problem, a height-balanced binary tree is defined as a binary tree in which the depth of the two subtrees of every node never differ by more than 1.

#### Example:

```swift
Given the sorted array: [-10,-3,0,5,9],

One possible answer is: [0,-3,9,-10,null,5], which represents the following height balanced BST:

      0
     / \
   -3   9
   /   /
 -10  5
```


# My Answer

* 재귀를 이용해서, 중간 index의 값을 해당 뎁스의 root로 사용하고, right, left를 추가한다.
* 기본적으로 이진탐색으로 mid, start, end index를 구한다.
  
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
    public TreeNode sortedArrayToBST(int[] nums) {        
        TreeNode root = null;
        
        root = AddNode(nums, 0, nums.length -1);
        
        return root;            
    }
    
    TreeNode AddNode(int[] nums, int start_idx, int end_idx ) {
        if ( start_idx > end_idx )  //start가 end 보다 커질경우는 더이상 찾을 값이 없다는 의미 이기 때문에 null로 처리
            return null;
        
        int mid = (start_idx + end_idx) / 2;    //중간 index를 찾고 
        TreeNode root = new TreeNode(nums[mid]);    //현재 뎁스의 root로 사용
        
        if ( start_idx == end_idx ) //start와 end가 같다는건 mid를 제외한 다른 원소가 없다는 의미
            return root;
        
        root.right = AddNode(nums, mid +1, end_idx);    //mid +1 ~ end 까지를 이용해서 right를 채우고
        root.left = AddNode(nums, start_idx, mid - 1);  //start ~ mid -1 까지를 이용해서 left를 채운다.
        
        return root;
        
    }
}
```
