# Problem

Given an n-ary tree, return the preorder traversal of its nodes' values.

Nary-Tree input serialization is represented in their level order traversal, each group of children is separated by the null value (See examples).

#### Example 1:

```swift
      1
    / | \
   3  2  4
  / \
 5   6

Input: root = [1,null,3,2,4,null,5,6]
Output: [1,3,5,6,2,4]
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
Output: [1,2,3,6,7,11,14,4,8,12,5,9,13,10]
```

# My Answer ( Recursive )

* 재귀를 이용
* 재귀 함수 `search`에서 `Node`가 `null`이 아니라면, 결과 리스트에 `Node`의 값 부터 넣자.
* `Node.children`을 순회 하면서, `search`함수를 재귀 호출 하면 된다
  
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
    public List<Integer> preorder(Node root) {        
        List<Integer> result = new ArrayList<>();
        
        search(root, result);
        
        return result;
    }
    
    void search(Node root, List<Integer> result) {
        if ( root == null)
            return;
        
        result.add(root.val);
        
        for(Node node : root.children ) {
            search(node, result);
        }
    }
}
```

# My Answer ( Iteration )

* 반복문 이용
* `Stack`을 이용하자.
* 순회 돌때 마다 결과 리스트에 `curr`의 값을 넣자.
* 만약 `curr`의 `children`이 하나라도 있다면, `head`에 `curr`을 할당하고, `curr`에는 `head`의 첫번째 자식을 할당하자. 그리고 `head.children`에서 첫번째 자식을 뺀다.
* 만약 `curr`이 `null`이라면, `Stack`에 있는것들중 `children`이 있는것을 찾고 해당 `Node`의 `children`의 첫번째 자식을 `curr`에 할당 한다
  
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
    public List<Integer> preorder(Node root) {       
        List<Integer> result = new ArrayList<>();
        Stack<Node> q = new Stack<>();
        Node curr = root;
        
        while(curr != null) {
            result.add(curr.val);
            q.push(curr);
            
            if ( curr.children.size() > 0 ) {
                Node head = curr;
                curr = curr.children.get(0);
                head.children.remove(0);             
            } else {
                curr = null;
            }
            
            if ( curr == null ) {
                while(!q.isEmpty()) {
                    Node candidate = q.peek();
                    if ( candidate.children.size() > 0 ) {
                        curr = candidate.children.get(0);
                        candidate.children.remove(0);             
                        if ( candidate.children.size() == 0 )
                            q.pop();
                        break;
                    } else {
                        curr = null;
                        q.pop();
                    }
                }                    
            }
        }
        
        return result;
    }    
}
```
