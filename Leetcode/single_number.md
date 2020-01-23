# Problem

Given a non-empty array of integers, every element appears twice except for one. Find that single one.

#### Example 1:

```swift
Input: [2,2,1]
Output: 1
```

#### Example 2:

```swift
Input: [4,1,2,1,2]
Output: 4
```

# My Answer

* 하나의 값을 제외한 나머지 값이 중복된다는 것을 이용해서 `XOR`연산을 이용하자.
* `XOR`연산의 특징이 A^A=0, A^0=A 이다.
* `nums`을 순회하면서 `XOR`연산을 반복하면 중복되지 않는 값만 남는다.

```java
class Solution {
    public int singleNumber(int[] nums) {
        int n = 0;
        for(int i : nums) {
            n ^= i;
        }
        
        return n;        
    }
}
```

# My Answer ( Use HashSet )

```java
class Solution {
    public int singleNumber(int[] nums) {
        Set<Integer> hashSet = new HashSet<>();
        
        for ( int n : nums) {
            if ( hashSet.contains(n)) {
                hashSet.remove(n);
            } else {
                hashSet.add(n);
            }
        }
        
        return hashSet.iterator().next();
    }
}
```

