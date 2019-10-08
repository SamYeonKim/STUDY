# Problem

Reverse a singly linked list recursively. 

#### Example :

```swift
Input: 1->2->3->4->5->NULL
Output: 5->4->3->2->1->NULL
```

# My Answer

* `head.next == null` 인 경우가 마지막 노드인데, 우린 마지막 노드를 `Head`로 사용 해야 한다.
* 이하 `head`로 되어 있지만, 그냥 `cur(현재 처리 중인 노드)`로 생각하는게, 이해하는데 도움 된다.
* `cur.next`는 현재 노드의 그 다음 노드를 가리키는데, 이것이 현재 노드의 새로운 `head`로서 역할해야 한다.
* `rest`는 사실상 맨 마지막 노드만 설정되고 계속 그 상태로 `reverseList`의 상위 호출 부분으로 가도 무방하다. 어짜피 참조타입이라 `reverseList` 함수를 재귀로 호출 하는 부분 위, 아래에서 열심히 새로운 형태로 뒤집고 있다. 결국 뒤집어진 완성형을 최종 리턴하기 위한 존재일 뿐.
  
```java
class Solution {
    public ListNode reverseList(ListNode head) {
        if ( head == null )
            return null;
        
        if ( head.next == null)
            return head;
        
        ListNode new_head = head.next;        
        ListNode rest = reverseList(head.next);
        head.next = null;
        new_head.next = head;
       
        return rest;
    }
}
```

