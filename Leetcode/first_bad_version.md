# Problem

You are a product manager and currently leading a team to develop a new product. Unfortunately, the latest version of your product fails the quality check. Since each version is developed based on the previous version, all the versions after a bad version are also bad.

Suppose you have n versions [1, 2, ..., n] and you want to find out the first bad one, which causes all the following ones to be bad.

You are given an API bool isBadVersion(version) which will return whether version is bad. Implement a function to find the first bad version. You should minimize the number of calls to the API.

#### Example :

```swift
Given n = 5, and version = 4 is the first bad version.

call isBadVersion(3) -> false
call isBadVersion(5) -> true
call isBadVersion(4) -> true

Then 4 is the first bad version. 
```

# My Answer
  
* `isBadVersion`함수의 리턴값이 `true`인 최소 값을 찾자.
* `중간값 (mid)`을 이용해서 `isBadVersion`의 결과값을 확인해서 `true`이면 `res`에 `mid`를 저장해 놓고, `low~mid-1`을 확인하자.
* 만약 `isBadVersion`의 결과값이 `false`라면 `mid`값 보다 큰 영역에 찾는 값이 있다는 뜻이므로 `mid+1~ high`를 확인 하자.

```java
/* The isBadVersion API is defined in the parent class VersionControl.
      boolean isBadVersion(int version); */

public class Solution extends VersionControl {
    public int firstBadVersion(int n) {
        int low = 1;
        int high = n;
        int res = -1;
        
        while(low <= high) {
            int mid = low + ((high-low)>>1);
            
            if ( isBadVersion(mid) ) {
                res = mid;
                high = mid -1;
            } else {
                low = mid + 1;
            }            
        }
        
        return res;
    }
}
```

