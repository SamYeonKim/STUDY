# Problem

Reverse a singly linked list iteratively. 

#### Example :

```swift
Input: 1->2->3->4->5->NULL
Output: 5->4->3->2->1->NULL
```

# My Answer

* `cur` 가 `null`이 아닐때 까지 순회돌자.
* `prev`에 `cur`을 할당하기 전까진 이전 노드를 가리키기 때문에, `cur.next = prev`를 하게 되면 현재 노드의 이전 노드를 다음 노드로 할당한다.
* 마지막 노드 까지 간 이후 `while문`을 나오게되면 현재 `prev`가 가리키고 있는건 마지막 노드이자 `Head`를 의미 한다.
  
```java
/**
 * Definition for singly-linked list.
 * public class ListNode {
 *     int val;
 *     ListNode next;
 *     ListNode(int x) { val = x; }
 * }
 */
class Solution {
    public ListNode reverseList(ListNode head) {
        
        ListNode cur = head;
        ListNode prev = null;
        
        while(cur != null ) {
            ListNode next = cur.next;
            cur.next = prev;
            prev = cur;   
            cur = next;
        }
        
        return prev;
    }
}
```

