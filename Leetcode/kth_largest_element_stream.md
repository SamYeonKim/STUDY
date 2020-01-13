# Problem

Design a class to find the kth largest element in a stream. Note that it is the kth largest element in the sorted order, not the kth distinct element.

Your KthLargest class will have a constructor which accepts an integer k and an integer array nums, which contains initial elements from the stream. For each call to the method KthLargest.add, return the element representing the kth largest element in the stream.

#### Example :

```swift
int k = 3;
int[] arr = [4,5,8,2];
KthLargest kthLargest = new KthLargest(3, arr);
kthLargest.add(3);   // returns 4
kthLargest.add(5);   // returns 5
kthLargest.add(10);  // returns 5
kthLargest.add(9);   // returns 8
kthLargest.add(4);   // returns 8
```

#### Note:

* You may assume that `nums`'length >= `k-1` and `k` >= 1.
* `k`는 `KthLargest` 생성자의 파라미터로 정해져 있다.
* `KthLargest.add` 함수가 호출 될 때 마다 `arr`에 파라미터가 추가 되고, `k`번째 큰 숫자를 리턴한다.

# My Answer

* `Binary Search Tree`를 이용하자.
* `cnt : 현재 노드를 루트로 하는 서브트리의 갯수`, `val : 현재 노드의 값`, `left : 왼쪽 노드`, `right : 오른쪽 노드`를 멤버로 갖는 `TreeNode` 클래스를 추가하자.
* `KthLargest` 생성자에서 `nums`를 이용해서 `BST`를 구성한다.
* `add` 함수에선 파라미터로 넘어온값을 기존에 만들어 놓은 `BST`에 추가하고, `k`번째에 있는 숫자를 찾는다.
* 찾는 방식은 다음과 같다.
  * `현재노드(curr)`의 `right`가 `null`이 아니라면, `right::cnt`의 값이 `k`보다 같거나 큰지 확인한다.
  * 만약 `k`보다 같거나 크다면, `curr` 보다 큰 숫자가 `k`만큼 있다는 것이니, 다음 `curr`은 `curr.right`가 된다.
  * 만약 `k`보다 작다면, `curr` 보다 큰 숫자가 `k` 보다 적다는 것이니, `k`에서 `right::cnt` 만큼을 빼준다.
    * 위에서 뺀고난 이후 `k`가 `1`이라면 `curr`이 `k`번째 큰 숫자란 뜻.
    * `k` 가 `1` 보다 크다면, `curr`은 `curr.left`가 된다. 즉 이전 `curr` ~ `curr::right` 는 후보에서 제외되니까 `k`에 1을 추가로 빼준다.
  * 위 예제는 다음과 같은 흐름으로 된다.
    ```java
    k = 3
    BST = 
        4(4)
       /   \
      2(1) 5(2)
             \ 
             8(1)
    add(3):
        4(6)
       /   \
      2(2) 5(2)
       \    \ 
       3(1) 8(1)
       
       1:
        curr:4
        5의 cnt = 2, k > 5'cnt (2)
        k = 3 - 2 = 1
        k == 1, 4가 정답
    ``` 
  
```java
class TreeNode {
    public int val;
    public int cnt;
    public TreeNode left;
    public TreeNode right;
    
    public TreeNode(int _val) {
        val = _val;  
        cnt = 1;
    }
}

class KthLargest {    
    int m_k;
    
    TreeNode m_root;
    
    public KthLargest(int k, int[] nums) {
        m_k=k;    

        for(int n : nums)
            insert(n);
    }
    
    public int add(int val) {
        insert(val);
        
        int k = m_k;
        TreeNode curr = m_root;
        
        while(curr != null) {
            if ( curr.right != null ) {
                if ( curr.right.cnt >= k ) {
                    curr = curr.right;
                    continue;
                } else {
                    k -= curr.right.cnt;                    
                }                
            } 
            
            if ( k == 1 ) {                
                break;
            } else {
                k -= 1;
                curr = curr.left;                
            }
        }
        
        return curr.val;
    }
    
    void insert(int val) {
        if ( m_root == null ) {
            m_root = new TreeNode(val);
        } else {   
            TreeNode curr = m_root;
            
            while ( curr != null ) {
                curr.cnt++;
                
                boolean is_left = curr.val >= val;
                TreeNode next = is_left ? curr.left : curr.right;
                
                if ( next == null ) {
                    if ( is_left ) {
                        curr.left = new TreeNode(val);                        
                    } else {
                        curr.right = new TreeNode(val);
                    }
                    curr = null;
                } else {
                    curr = next;
                }                            
            }            
        }          
    }
}
```

# Fastest Answer

* `Heap`을 이용하자.
  * [Heap이 뭐야?](https://tinyfool.org/2019/05/heap-and-heap-sort-algorithm-java-code-and-leetcode-problems/)
  * 완전 이진 트리를 배열로 표기 하기 위한 방법
  * 부모 노드의 index는 자식 노드의 index를 `2로 나눈 값`이다.
  * 특정 노드의 index가 `i`일때, 왼쪽 자식 노드의 index는 `2*i+1`, 오른쪽 자식 노드의 indexsms `2*i+2`이다. 
    ```
         4
       /   \
      2     7
     / \   / \
    1   3 5   6
    => [4,2,7,1,3,5,6]
    =>  0,1,2,3,4,5,6
    4의 index=0, left(2)의 index=2*0+1=1, right(7)의 index=2*0+2=2   
    2의 index=1, left(1)의 index=2*1+1=3, right(3)의 index=2*1+2=4
    7의 index=2, left(5)의 index=2*2+1=5, right(6)의 index=2*2+2=6   
    ```      
* `heap`의 첫번째 원소를 `k`만큼 큰 숫자로 유지하자.
* `addItem`에서 현재 배열의 갯수가 `k`만큼 안될땐 `siftUp` 아니라면 `siftDown`을 호출

```java
class KthLargest {
    final smallHeap heap;
    public KthLargest(int k, int[] nums) {
        heap = new smallHeap(k);
        for (int i = 0; i < nums.length; ++i) {
            heap.addItem(nums[i]);
        }
    }
    
    public int add(int val) {
        heap.addItem(val);
        return heap.peek();
    }
    
    private static class smallHeap {
        int capacity;
        int[] heap;
        public smallHeap(int x) {
            capacity = 0;
            heap = new int[x];
        }
        
        public void addItem(int x) {
            if (capacity < heap.length) {
                heap[capacity] = x;
                siftUp(capacity);
                capacity++;
            } else {
               if (x > heap[0]) {
                   heap[0] = x;
                   siftDown(capacity);
               }
            }
        }
        
        private void siftUp(int i) {
            while (i > 0) {
                int parent = (i-1)/2;
                if (heap[i] <= heap[parent]) {
                    swapNum(i, parent);
                    i = parent;
                } else {
                    return;
                }
            }
        }
        
        private void siftDown(int len) {
            int cur = 0;
            while (cur < capacity/2) {
                int left = cur * 2 + 1;
                int right = cur * 2 +2;
                int swap;
                if (right < len && heap[right] < heap[left])
                    swap = right;
                else
                    swap = left;
                if (heap[cur] > heap[swap]) {
                    swapNum(cur,swap);
                    cur = swap;
                } else {
                    return;
                }
            }
        }
        
        private void swapNum(int x, int y) {
            int temp = heap[x];
            heap[x] = heap[y];
            heap[y] = temp;
        }
        
        public int peek() {
            return heap[0];
        }
    }
}
```

