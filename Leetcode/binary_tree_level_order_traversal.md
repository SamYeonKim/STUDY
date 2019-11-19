# Problem

Given a binary tree, return the level order traversal of its nodes' values. (ie, from left to right, level by level).

#### Example :

```swift
Input: [3,9,20,null,null,15,7]
    3
   / \
  9  20
    /  \
   15   7
Output:
[
  [3],
  [9,20],
  [15,7]
]
```

# My Answer ( Recursive )

* 재귀로 해결
* `solver` 함수로 현재 `Level`과 `Level`별 값을 저장할 최종 리스트를 같이 넘겨서 `Level` 값을 기준으로 넣어야할 리스트를 찾아서 현재 `Node`의 값을 넣어 준다.

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
    public List<List<Integer>> levelOrder(TreeNode root) {
        List<List<Integer>> result = new ArrayList<List<Integer>>();
        
        solver(result, root, 0);
        
        return result;
    }
    
    void solver(List<List<Integer>> ans, TreeNode root, int level) {
        if ( root == null )
            return;
        
        List<Integer> l_level = null;
        if ( ans.size() <= level ) {
            l_level = new ArrayList<Integer>();
            ans.add(l_level);
        } else {
            l_level = ans.get(level);
        }
        
        l_level.add(root.val);
        
        if ( root.left != null ) {
            solver(ans, root.left, level + 1);
        }
        
        if ( root.right != null ) {
            solver(ans, root.right, level + 1);
        }
    }
}
```

# My Answer ( Iteration )

* 반복문을 이용해서 해결
* 다음 `Level`의 `node`들을 `next_nodes`에 저장하고, `while`문에서는 `next_nodes`를 순회 돌면서, `node`들의 값을 결과 리스트에 추가 한다.
* `node`의 `left`와 `right`가 `null`이 아니라면 다음 순회를 위해서 `next_nodes` 리스트에 추가 하기 위해, `temp_nodes` 리스트에 추가 한다.
* `next_nodes` 순회가 끝나면, `next_nodes`를 비우고 `temp_nodes`의 내용을 `next_nodes`에 채운 후, `temp_nodes`도 비운다.

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
    public List<List<Integer>> levelOrder(TreeNode root) {
        List<List<Integer>> result = new ArrayList<List<Integer>>();
        if ( root == null ) {
            return result;
        }
        
        List<TreeNode> next_nodes = new ArrayList<TreeNode>();      //다음 Level의 Node들을 담고 있는 리스트
        List<TreeNode> temp_nodes = new ArrayList<TreeNode>();      //result 리스트에 Node의 값을 넣은 이후 다음 Level의 Node들을 담을 임시 리스트
        next_nodes.add(root);
        
        while (next_nodes.size() > 0 ) {        //next_nodes의 원소 갯수가 0개 라는것은 다음 Level에 해당하는 Node가 없다는 의미
            List<Integer> sub_result = new ArrayList<Integer>();
            
            for(TreeNode node : next_nodes ) {
                sub_result.add(node.val);
                
                if ( node.left != null )
                    temp_nodes.add(node.left);
                if ( node.right != null )
                    temp_nodes.add(node.right);
            }
            next_nodes.clear();
            
            for(TreeNode node : temp_nodes ) {
                next_nodes.add(node);
            }
            temp_nodes.clear();
            
            result.add(sub_result);
        }        
        return result;
    }
}
```

