# Problem

Given an n-ary tree, return the level order traversal of its nodes' values.

Nary-Tree input serialization is represented in their level order traversal, each group of children is separated by the null value (See examples).

#### Example 1:

```swift
      1
    / | \
   3  2  4
  / \
 5   6

Input: root = [1,null,3,2,4,null,5,6]
Output: [[1],[3,2,4],[5,6]]
```

#### Example 2:

```swift
        1
     / | | \
    2  3 4  5
      /| |  |\
     6 7 8  9 10
       | |  | 
      11 12 13
       |
      14

Input: root = [1,null,2,3,4,5,null,null,6,7,null,8,null,9,10,null,null,11,null,12,null,13,null,null,14]
Output: [[1],[2,3,4,5],[6,7,8,9,10],[11,12,13],[14]]
```

# My Answer

* 반복문 이용
* `Queue`에는 기존 `Queue`에 있던것 만큼 순회돌면서, 결과 리스트에 넣고 `children`을 `Queue`에 넣는다. 
  
```java
/*
// Definition for a Node.
class Node {
    public int val;
    public List<Node> children;

    public Node() {}

    public Node(int _val) {
        val = _val;
    }

    public Node(int _val, List<Node> _children) {
        val = _val;
        children = _children;
    }
};
*/
class Solution {
    public List<List<Integer>> levelOrder(Node root) {
        List<List<Integer>> result = new ArrayList<>();
        if ( root == null )
            return result;
        
        Queue<Node> q = new LinkedList<>();
        q.add(root);
        
        while(!q.isEmpty()) {
            List<Integer> sub_result = new ArrayList<>();
            
            int n = q.size();
            
            for(int x=0;x<n;x++) {
                Node node = q.poll();    
                sub_result.add(node.val);
                
                for(Node child : node.children ) {
                    q.add(child);
                }
            }
            
            result.add(sub_result);
        }
        
        return result;
    }
}
```