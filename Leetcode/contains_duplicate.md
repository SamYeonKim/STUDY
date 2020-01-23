# Problem

Given an array of integers, find if the array contains any duplicates.

Your function should return true if any value appears at least twice in the array, and it should return false if every element is distinct.

#### Example 1:

```swift
Input: [1,2,3,1]
Output: true
```

#### Example 2:

```swift
Input: [1,2,3,4]
Output: true
```

#### Example 3:

```swift
Input: [1,1,1,3,3,4,3,2,4,2]
Output: true
```

# My Answer

* `nums`에서 최소값과 최대값을 찾자. `nums`는 최소값과 최대값 사이에 있는 수로 이루어져 있기 때문에, 중복 여부를 체크할 `boolean배열`을 구성하는데, `최대값 - 최소값 +1` 사이즈의 배열(`table`)을 만들자.
* `nums`를 순회 하면서, 현재 숫자 (`i`)에서 최소값을 빼면 `table`의 `index`가 나오고, `table[index]`가 `true`면 중복된 수가 있다는 의미이다.

```java
class Solution {
    public boolean containsDuplicate(int[] nums) {
        int max = Integer.MIN_VALUE;
        int min = Integer.MAX_VALUE;
        for(int i : nums){
            if(i > max) max = i;
            if(i < min) min = i;
        }

        boolean[] table = new boolean[max - min + 1];
        for(int i : nums){
            if(table[i - min]) return true;

            table[i - min] = true;
        }
        return false;
    }
}
```

# My Answer ( Use HashSet )

```java
class Solution {
    public boolean containsDuplicate(int[] nums) {
        Set<Integer> hashSet = new HashSet<>();          
        for(int n : nums){
            if ( hashSet.contains(n)) return true;
            
            hashSet.add(n);
        }
        return false;
    }
}
```
