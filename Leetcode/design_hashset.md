# Problem

Design a HashSet without using any built-in hash table libraries.

To be specific, your design should include these functions:

* `add(value)` : Insert a value into the HashSet. 
* `contains(value)` : Return whether the value exists in the HashSet or not.
* `remove(value)` : Remove a value in the HashSet. If the value does not exist in the HashSet, do nothing.

#### Example :

```swift
MyHashSet hashSet = new MyHashSet();
hashSet.add(1);         
hashSet.add(2);         
hashSet.contains(1);    // returns true
hashSet.contains(3);    // returns false (not found)
hashSet.add(2);          
hashSet.contains(2);    // returns true
hashSet.remove(2);          
hashSet.contains(2);    // returns false (already removed)
```

#### Note:

* All values will be in the range of `[0, 1000000]`
* The number of operations will be in the range of `[1, 10000]`
* Please do not use the built-in HashSet library

# My Answer
  
* 2차원 배열을 이용해서 값을 저장하자.
* 첫번째 index가 어떤 `Bucket`인지를 결정. `value`의 범위가 `0~1000000`이기 때문에, `value / 1000`를 첫번째 index를 구하는 함수로 사용 (`getHash`).
* 두번째 index가 `Bucket`에서 저장될 위치를 결정. `value`의 범위가 `0~1000000`이기 때문에, `value % 1000`를 두번째 index를 구하는 함수로 사용 ( `getIndex`).
* `add`함수에서 파라미터로 넘어온 `value`에 맞춰서 `getHash`, `getIndex`를 구하고, 현재 2차원 배열의 길이가 `Hash, Index` 보다 작다면 사이즈를 키운다.

```java
class MyHashSet {
    int[][] values;
    
    public MyHashSet() {
        values= new int[0][0];        
    }
    
    public void add(int key) {
        if ( contains(key))
            return;
        
        int hash = getHash(key);
        if ( values.length <= hash ) {
            int[][] new_v = new int[hash + 1][];
            
            for(int i=0;i<new_v.length;i++) {
                if ( i < values.length) {
                    new_v[i] = new int[values[i].length];
                    System.arraycopy(values[i], 0, new_v[i], 0, values[i].length);    
                } else {
                    new_v[i] = new int[0];
                }                
            }
            values = new_v;
        }
        
        int index = getIndex(key);
        int sub_count = values[hash].length;
        if ( sub_count <= index ) {
            int[] new_sub = new int[index + 1];    
            for(int i=0;i<new_sub.length;i++) {
                int org_value = -1;
                if ( i < sub_count ) {
                    org_value = values[hash][i];
                } 
                
                new_sub[i] = org_value;
            }
            values[hash] = new_sub;
        }
        
        values[hash][index] = key;        
    }
    
    public void remove(int key) {
        if ( !contains(key)) return;
        
        int hash = getHash(key);
        int index = getIndex(key);
        
        values[hash][index] = -1;
    }
    
    /** Returns true if this set contains the specified element */
    public boolean contains(int key) {
         int hash = getHash(key);
        
        if ( values.length <= hash ) return false;
        
        int index = getIndex(key);
        if ( values[hash].length <= index ) return false;
        
        if ( values[hash][index] != key ) return false;
        
        return true;
    }
    
    int getHash(int key) {
        return key / 1000;        
    }
    
    int getIndex(int key) {
        return key % 1000;
    }
}

/**
 * Your MyHashSet object will be instantiated and called as such:
 * MyHashSet obj = new MyHashSet();
 * obj.add(key);
 * obj.remove(key);
 * boolean param_3 = obj.contains(key);
 */
```

