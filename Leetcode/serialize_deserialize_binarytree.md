# Problem

Serialization is the process of converting a data structure or object into a sequence of bits so that it can be stored in a file or memory buffer, or transmitted across a network connection link to be reconstructed later in the same or another computer environment.

Design an algorithm to serialize and deserialize a binary tree. There is no restriction on how your serialization/deserialization algorithm should work. You just need to ensure that a binary tree can be serialized to a string and this string can be deserialized to the original tree structure.

#### Example

```swift
    1
   / \
  2   3
     / \
    4   5

"[1,2,3,null,null,4,5]"
```

#### Clarification

The above format is the same as how LeetCode serializes a binary tree. You do not necessarily need to follow this format, so please be creative and come up with different approaches yourself.

#### Note:

Do not use class member/global/static variables to store states. Your serialize and deserialize algorithms should be stateless.


# My Answer

* `DFS` 방식을 이용하자.
* `serialize`
  * `DFS`를 이용해서 다음 `depth`의 `Node`를 수집 하면서 순회하자.
  * 현재 `depth`의 `Node`를 순회 하면서, `null`이면 `"null"`을 기록하고 아니라면, `value`를 기록 하자.
  * `Node`가 `null`이 아니라면, `curr_value_count`를 하나씩 감소 시키자.
  * `Node`의 `left or right`가 `null`이 아니라면 `child_value_count`를 하나씩 증가 시키자.
  * 만약 `child_value_count`가 0이면 다음 `depth`는 존재 하지 않는다.
  * 순회가 끝나면, 다음 `depth`처리를 위해 `curr_value_count`에 `child_value_count`의 값을 할당하고, `child_value_count`는 0으로 할당 하자.
* `deserialize`
  * 문자열의 포멧이 `"[x,y,z....]"` 이기 때문에, 맨 앞과 맨뒤의 문자를 제거하고, `","`를 이용해서 `split`해서, `splitted`배열에 저장하자.
  * `DFS`를 이용하자.
  * 현재 `Node`의 `left or right`를 구성할 때, `splitted`에서 가져올 것이 있다면, 가져와서 새로운 `TreeNode`객체를 만들어서 할당하자.
  * `splitted`에서 더이상 가져올 것이 없다면, 전부 구성 했다는 의미니까 종료.

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
public class Codec {

    // Encodes a tree to a single string.
    public String serialize(TreeNode root) {
        if ( root == null )
            return "";
        
        StringBuilder builder = new StringBuilder();
        
        Queue<TreeNode> q = new LinkedList<>();
        
        builder.append("[");
        q.add(root);
        
        int curr_value_count = 0;
        int child_value_count = 0;
        
        while(!q.isEmpty()) {    
            int n = q.size();
            
            for(int i=0;i<n;i++) {
                TreeNode node = q.poll();
                if ( n == 1) {
                    builder.append(String.format("%s", node == null ? "null" : node.val));
                } else {
                    if ( node != null )
                        curr_value_count--;
                    
                    builder.append(String.format(",%s", node == null ? "null" : node.val));    
                }

                if ( node == null ) 
                    continue; 
                
                if ( node.left != null ) {                                    
                    child_value_count++;
                }

                if ( node.right != null ) {
                    child_value_count++;
                }   

                q.add(node.left);
                q.add(node.right);
                
                if ( curr_value_count == 0 )
                    break;
            }            
            
            if ( child_value_count == 0 )
                break;
            
            curr_value_count = child_value_count;
            child_value_count = 0;
        }
        
        builder.append("]");
        
        return builder.toString();        
    }

    // Decodes your encoded data to tree.
    public TreeNode deserialize(String data) {
        if ( data == null || data.isEmpty())
            return null;
        
        String values = data.substring(1, data.length() - 1);
        String[] splitted = values.split(",");
        int i = 0;
        TreeNode root = new TreeNode(Integer.parseInt(splitted[i++]));
        
        Queue<TreeNode> q = new LinkedList<>();
        q.add(root);
        
        while ( !q.isEmpty() && i < splitted.length ) {
            int n = q.size();
            outerloop:
            for(int x=0;x<n;x++) {
                TreeNode node = q.poll();
                
                for(int y=0;y<2;y++) {
                    if ( i >= splitted.length)
                        break outerloop;
                    
                    String val = splitted[i++];
                    try {
                        if ( y == 0 ) {
                            node.left = new TreeNode(Integer.parseInt(val));                     
                            q.add(node.left);    
                        } else {
                            node.right = new TreeNode(Integer.parseInt(val));                     
                            q.add(node.right);    
                        }                        
                    } catch (NumberFormatException e) {                        
                    }                                        
                }                           
            }
        }
        
        return root;
    }
}

// Your Codec object will be instantiated and called as such:
// Codec codec = new Codec();
// codec.deserialize(codec.serialize(root));
```

