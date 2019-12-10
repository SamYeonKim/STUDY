# Problem

Given an n-ary tree, return the postorder traversal of its nodes' values.

Nary-Tree input serialization is represented in their level order traversal, each group of children is separated by the null value (See examples).

#### Example 1:

```swift
      1
    / | \
   3  2  4
  / \
 5   6

Input: root = [1,null,3,2,4,null,5,6]
Output: [5,6,3,2,4,1]
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
Output: [2,6,14,11,7,3,12,8,4,13,9,10,5,1]
```

# My Answer ( Recursive )

* 재귀를 이용
* 재귀 함수 `search`에서 `Node`가 `null`이 아니라면, `Node.children`을 순회 하면서, `search`함수를 재귀 호출하자.
* 결과 리스트에 `Node`의 값을 넣자
  
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
    public List<Integer> postorder(Node root) {
        List<Integer> result = new ArrayList<>();
        
        search(root, result);
        
        return result;
    }
    
    void search(Node root, List<Integer> result) {
        if ( root == null )
            return;
        
        for(Node node : root.children ) {
            search(node, result);
        }
        
        result.add(root.val);
    }
}
```

# My Answer ( Iteration )

* 반복문 이용
* `Stack`을 이용하자.
* 만약 `curr`이 `null`이라면, `Stack`에서 `children`이 있는것들중 해당 `Node`의 마지막 자식을 `curr`에 할당하자.
* 만약 `curr`이 `null`이 아니라면, 결과 리스트의 첫번째에 `curr`의 값을 넣고, `children`의 마지막 자식을 `curr`에 할당하자. 만약 자식이 없다면 `curr`을 `null`로 할당하자.
  
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
    public List<Integer> postorder(Node root) {
        List<Integer> result = new ArrayList<>();
        
        Stack<Node> s = new Stack<>();
        Node curr = root;
        
        while( !s.isEmpty() || curr != null ) {
            if ( curr != null ) {
                s.push(curr);
                result.add(0, curr.val);
                
                if ( curr.children.size() > 0 ) {
                    Node head = curr;                
                    curr = head.children.get(head.children.size() -1);
                    head.children.remove(head.children.size() - 1);
                } else {
                    curr = null;
                }
            } else {
                curr = s.peek();
                
                if ( curr.children.size() > 0 ) {
                    Node head = curr;                
                    curr = head.children.get(head.children.size() -1);
                    head.children.remove(head.children.size() - 1);
                    if ( head.children.size() == 0 )
                        s.pop();
                } else {
                    curr = null;
                    s.pop();
                }
            }            
        }

        return result;
    }
}
```
