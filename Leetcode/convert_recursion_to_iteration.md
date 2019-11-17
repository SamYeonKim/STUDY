#### Definition

재귀적으로 알고리즘을 구현하는것은 좋은 접근이나, 다음과 같은 이유에 의해서 재귀적으로 구현 못 하는경우가 발생한다.

1. Stackoverflow의 발생    
    재귀적인 알고리즘을 제대로 구현 하지 못했을때, Stackoverflow를 발생시킬 여지가 있다. 
2. 효율성
   재귀적인 함수 호출은 System Stack에 추가적인 메모리 할당이 요구 된다. 간혹 중복 계산의 문제가 발생할 수 있다. 예를 들어 [Pascal's Triangle](/Leetcode/pascals_triangle.md)
3. 복잡성
    재귀적인 알고리즘은 코드의 흐름을 이해하기 어렵게 만든다.

#### Example

2개의 Binary Tree가 있을때 같은지 다른지 확인 하는 코드를 `재귀적`으로, `반복적`으로 구현하면 다음과 같이 할 수 있다.

```java
[Recursion]

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
  public boolean isSameTree(TreeNode p, TreeNode q) {
    // p and q are both null
    if (p == null && q == null) return true;
    // one of p and q is null
    if (q == null || p == null) return false;
    if (p.val != q.val) return false;
    return isSameTree(p.right, q.right) &&
            isSameTree(p.left, q.left);
  }
}
```

```java
[Iteration]

class Solution {
  public boolean check(TreeNode p, TreeNode q) {
    // p and q are null
    if (p == null && q == null) return true;
    // one of p and q is null
    if (q == null || p == null) return false;
    if (p.val != q.val) return false;
    return true;
  }

  public boolean isSameTree(TreeNode p, TreeNode q) {
    if (p == null && q == null) return true;
    if (!check(p, q)) return false;
    // init deques
    ArrayDeque<TreeNode> deqP = new ArrayDeque<TreeNode>();
    ArrayDeque<TreeNode> deqQ = new ArrayDeque<TreeNode>();
    deqP.addLast(p);
    deqQ.addLast(q);

    while (!deqP.isEmpty()) {
      p = deqP.removeFirst();
      q = deqQ.removeFirst();

      if (!check(p, q)) return false;
      if (p != null) {
        // in Java nulls are not allowed in Deque
        if (!check(p.left, q.left)) return false;
        if (p.left != null) {
          deqP.addLast(p.left);
          deqQ.addLast(q.left);
        }
        if (!check(p.right, q.right)) return false;
        if (p.right != null) {
          deqP.addLast(p.right);
          deqQ.addLast(q.right);
        }
      }
    }
    return true;
  }
}
```

#### Conclusion

재귀적인 알고리즘을 반복적인 알고리즘으로 변경할 땐 다음과 같은 흐름으로 하면 된다.

1. Stack이나 Queue같은 데이터 구조를 이용해서 재귀함수를 호출할 때 사용할 파라미터들을 추가해 주면된다.
2. 미리 구성해 놓은 데이터 구조를 이용해서 반복 수행 하면 된다.
