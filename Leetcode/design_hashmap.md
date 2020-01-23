# Problem

Design a HashMap without using any built-in hash table libraries.

To be specific, your design should include these functions:

* `put(key, value)` : Insert a (key, value) pair into the HashMap. If the value already exists in the HashMap, update the value.
* `get(key)` : Returns the value to which the specified key is mapped, or -1 if this map contains no mapping for the key.
* `remove(key)` : Remove the mapping for the value key if this map contains the mapping for the key.


#### Example :

```swift
MyHashMap hashMap = new MyHashMap();
hashMap.put(1, 1);          
hashMap.put(2, 2);         
hashMap.get(1);            // returns 1
hashMap.get(3);            // returns -1 (not found)
hashMap.put(2, 1);          // update the existing value
hashMap.get(2);            // returns 1 
hashMap.remove(2);          // remove the mapping for 2
hashMap.get(2);            // returns -1 (not found) 
```

#### Note:

* All keys and values will be in the range of `[0, 1000000]`
* The number of operations will be in the range of `[1, 10000]`
* Please do not use the built-in HashMap library

# My Answer
  
* 2차원 배열을 이용해서 값을 저장하자.
* 첫번째 index가 어떤 `Bucket`인지를 결정. `value`의 범위가 `0~1000000`이기 때문에, `value / 1000`를 첫번째 index를 구하는 함수로 사용 (`getHash`).
* 두번째 index가 `Bucket`에서 저장될 위치를 결정. `value`의 범위가 `0~1000000`이기 때문에, `value % 1000`를 두번째 index를 구하는 함수로 사용 ( `getIndex`).
* `add`함수에서 파라미터로 넘어온 `value`에 맞춰서 `getHash`, `getIndex`를 구하고, 현재 2차원 배열의 길이가 `Hash, Index` 보다 작다면 사이즈를 키운다.

```java
class MyHashMap {
    int[][] values;
    /** Initialize your data structure here. */
    public MyHashMap() {
        values=new int[0][0];
    }
    
    /** value will always be non-negative. */
    public void put(int key, int value) {
        if ( get(key) >= 0 ) {
            int hash = getHash(key);    
            int index = getIndex(key);
            values[hash][index] = value;
            
            return;
        }
            
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
        
        values[hash][index] = value;
    }
    
    /** Returns the value to which the specified key is mapped, or -1 if this map contains no mapping for the key */
    public int get(int key) {
        int hash = getHash(key);
        
        if ( values.length <= hash ) return -1;
        
        int index = getIndex(key);
        if ( values[hash].length <= index ) return -1;
        
        return values[hash][index];
    }
    
    int getHash(int key) {
        return key / 1000;        
    }
    
    int getIndex(int key) {
        return key % 1000;
    }
    
    /** Removes the mapping of the specified value key if this map contains a mapping for the key */
    public void remove(int key) {
        if ( get(key) < 0 ) return;
        
        int hash = getHash(key);
        int index = getIndex(key);
        
        values[hash][index] = -1;
    }
}

/**
 * Your MyHashMap object will be instantiated and called as such:
 * MyHashMap obj = new MyHashMap();
 * obj.put(key,value);
 * int param_2 = obj.get(key);
 * obj.remove(key);
 */
```

