# Problem

You are given two non-empty linked lists representing two non-negative integers. The digits are stored in reverse order and each of their nodes contain a single digit. Add the two numbers and return it as a linked list.

You may assume the two numbers do not contain any leading zero, except the number 0 itself.


#### Example :

```swift
Input: (2 -> 4 -> 3) + (5 -> 6 -> 4)
Output: 7 -> 0 -> 8
Explanation: 342 + 465 = 807.
```

# My Answer

* l1와 l2의 끝 Node까지 순회하자.
* 순회하면서, l1, l2의 val의 합을 구하자.
* 만약 합이 10 이상이면 다음 Node의 합을 구할때 1을 추가로 더해줘야 하기 때문에, int 변수에 저장하자.
* 만약 l1, l2의 끝 까지 순회 했는데 마지막 연산으로 인해 10이상이 됬을수도 있으니, 확인해서 마지막에 Node(1)을 추가해 주자.
  
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
    public ListNode addTwoNumbers(ListNode l1, ListNode l2) {
        ListNode res = new ListNode(0);        
        ListNode cur = res;
        
        int n_ex = 0;
        while(l1 != null || l2 != null ) {
            int n_1 = 0;
            int n_2 = 0;
            
            if ( l1 != null ) {
                n_1 = l1.val;
                l1 = l1.next;
            }
            
            if ( l2 != null ) {
                n_2 = l2.val;
                l2 = l2.next;
            }
            
            int sum = n_1 + n_2 + n_ex;
            if ( sum >= 10 ) {
                n_ex = 1;
                sum %= 10;
            } else {
                n_ex = 0;
            }
            
            cur.next = new ListNode(sum);
            cur = cur.next;
        }
        
        if ( n_ex > 0 ) {
            cur.next = new ListNode(1);
        }
            
        return res.next;        
    }
    
    
    
}
```

# Fastest Answer

* l1, l2의 합 연산 과 올림에 의한것도 while에서 한큐에 진행
* 필요 없는 int 변수를 덜 만들었다.

```java
class Solution {
public ListNode addTwoNumbers(ListNode l1, ListNode l2) {
    int carry = 0;
    ListNode p, dummy = new ListNode(0);
    p = dummy;
    while (l1 != null || l2 != null || carry != 0) {
        if (l1 != null) {
            carry += l1.val;
            l1 = l1.next;
        }
        if (l2 != null) {
            carry += l2.val;
            l2 = l2.next;
        }
        p.next = new ListNode(carry%10);
        carry /= 10;
        p = p.next;
    }
    return dummy.next;
}

}
```

