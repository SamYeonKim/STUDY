# Problem

Suppose Andy and Doris want to choose a restaurant for dinner, and they both have a list of favorite restaurants represented by strings.

You need to help them find out their common interest with the least list index sum. If there is a choice tie between answers, output all of them with no order requirement. You could assume there always exists an answer.

#### Example 1:

```swift
Input:
["Shogun", "Tapioca Express", "Burger King", "KFC"]
["Piatti", "The Grill at Torrey Pines", "Hungry Hunter Steakhouse", "Shogun"]
Output: ["Shogun"]
Explanation: The only restaurant they both like is "Shogun".
```

#### Example 2:

```swift
Input:
["Shogun", "Tapioca Express", "Burger King", "KFC"]
["KFC", "Shogun", "Burger King"]
Output: ["Shogun"]
Explanation: The restaurant they both like and have the least index sum is "Shogun" with index sum 1 (0+1).
```

#### Note :

* The length of both lists will be in the rang of [1,1000].
* The length of strings in both lists will be in the range of [1,30].
* The index is starting from 0 to the list length minus 1.
* No duplicates in both lists

# My Answer

* `HashMap`을 이용하자. `list1`을 순회하면서 `HashMap`의 키로 `list1[i]`를 넣고 값으로 index(`i`)를 넣자.
* `list2`를 순회하면서, `HashMap`에 `list2[i]`가 키로 있는것만 확인하면된다.
* `HashMap`의 값 + `i`가 `Sum`이 되고, `최소Sum`과 비교 하자.
* 만약 `최소Sum`보다 `Sum`이 작다면, `최소Sum`을 만족하는 값의 갯수를 1로 초기화.
* 만약 `최소Sum`과 `Sum`이 같다면, `최소Sum`을 만족하는 값의 갯수를 1증가.
* 위의 2가지 경우에 대해서, `list1[최소Sum을 만족하는 값의 갯수-1]`에 `list2[i]`를 할당하자. 차후에 결과 배열로 옮기기 위한 사전단계이다. 위 예제2는 다음과 같은 흐름으로 진행 된다.
    ```java
    list1=["Shogun", "Tapioca Express", "Burger King", "KFC"]
    list2=["KFC", "Shogun", "Burger King"]

    1:
        curr = list2[0] = KFC
        sum = HashMap[KFC] + 0 = 3 + 0 = 3
        min_sum > sum
            min_sum_count=1, list1[min_sum_count-1] = list1[0] = KFC
    2:
        curr = list2[1] = Shogun
        sum = HashMap[Shogun] + 1 = 0 + 1 = 1
        min_sum > sum
            min_sum_count=1, list1[min_sum_count-1] = list1[0] = Shogun
    3: 
        curr = list2[2] = Burger King
        sum = HashMap[Burger King] + 2 = 2 + 2 = 4
        min_sum < sum
    result = list1[0~min_sum_count-1] = list1[0] = [Shogun]
    ```
* `list2`의 순회가 끝나면, `list1[0 ~ 최소Sum을 만족하는 값의 갯수-1]`가 정답이다. 따라서 결과배열로 옮겨주자.

```java
class Solution {
    public String[] findRestaurant(String[] list1, String[] list2) {
        Map<String, Integer> hashmap = new HashMap<>();
        
        for(int i=0;i<list1.length;i++) {
            hashmap.put(list1[i],i);            
        }
        
        int min_sum = Integer.MAX_VALUE;
        int duple_count=0;
        for(int i=0;i<list2.length;i++) {
            if ( !hashmap.containsKey(list2[i])) continue;
            
            int sum = hashmap.get(list2[i]) + i;
            
            if ( min_sum >= sum ) {
                duple_count = min_sum > sum ? 1 : duple_count+1;                
                list1[duple_count-1] = list2[i];
            }            
            
            min_sum = min_sum < sum ? min_sum : sum;
        }  
        
        String[] result = new String[duple_count];
        
        System.arraycopy(list1, 0, result, 0, duple_count);
        
        return result;
    }
}
```

