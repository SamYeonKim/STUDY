# Problem

Merge two sorted linked lists and return it as a new list. The new list should be made by splicing together the nodes of the first two lists.

#### Example:

```swift
Input: 1->2->4, 1->3->4
Output: 1->1->2->3->4->4
```


# My Answer

* 재귀로 구현
* `base case` 는 `l1`, `l2` 둘다 `null`일때
* `l1`, `l2` 둘중에 값이 작은 것을 선택한다.
* 만약 둘중에 `null`인 것이 있다면 다른것을 선택하자.
  
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
    public ListNode mergeTwoLists(ListNode l1, ListNode l2) {
        ListNode head = new ListNode(0);
        
        mergeTwoLists(head, l1, l2);
        
        return head.next;
    }
    
    void mergeTwoLists(ListNode result, ListNode l1, ListNode l2) {
        if ( l1 == null && l2 == null )
            return;
        
        if ( l1 == null || (l2 != null && l1.val > l2.val) ) {
            result.next = l2;
            l2 = l2.next;
        } else {
            result.next = l1;
            l1 = l1.next;
        }
        
        mergeTwoLists(result.next, l1, l2);
    }
}
```

# Fastest Answer 

* 반복을 이용해서 구현
* `l1`, `l2` 둘다 `null`이 될때까지 수행
* 비교해서 최종 리스트에 넣는다.
  
```java
class Solution {
    public ListNode mergeTwoLists(ListNode l1, ListNode l2) {
        if(l1 == null) return l2;
        if(l2 == null) return l1;
        
        ListNode start;
        if (l1.val <=  l2.val){
            start = l1;
            l1 = l1.next;
        } else {
            start = l2;
            l2 = l2.next;
        }
        ListNode call = start;
        
        while(l1 != null || l2 != null){
            if (l1 == null){
                start.next = l2;
                l2 = null;                
            } else if (l2 == null){
                start.next =l1;
                l1 = null;                
            } else if (l1.val <= l2.val){
                start.next = l1;
                l1 = l1.next;
            } else {
                start.next = l2;
                l2 = l2.next;
            }
            start = start.next;
            
        }
        return call;
    }
}
```
