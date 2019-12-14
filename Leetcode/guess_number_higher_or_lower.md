# Problem

We are playing the Guess Game. The game is as follows:

I pick a number from 1 to n. You have to guess which number I picked.

Every time you guess wrong, I'll tell you whether the number is higher or lower.

You call a pre-defined API guess(int num) which returns 3 possible results (-1, 1, or 0):

```swift
-1 : My number is lower
 1 : My number is higher
 0 : Congrats! You got it!
```

#### Example :

```swift
Input: n = 10, pick = 6
Output: 6
```

# My Answer
  
* 1~n까지 분할해서 찾자.
* `l`은 최소값, `r`은 최대값이다. `while`문에서는 `l+r의 중간값 ( mid )`를 찾고 `mid`가 찾는 값인지 `guess`함수로 확인한다.
* 만약 `guess`함수의 리턴값이 `0`이면 답을 찾았으니 끝내고, `-1`이면 `mid`보다 작은 값을 찾아야 하니 `r`에 `mid`를 할당하고 다음 반복 수행, 
  `1`이면 `mid`보다 큰 값이니까 `l`에 `mid+`한 값을 할당하고 반복 수행 하면된다.
* 예를 들어 다음과 같은 흐름으로 진행된다.
    ```java
    Input: n = 10, pick = 7
    1:
        l=1, r=10, mid=5, guess(5)=1
    2:
        l=6, r=10, mid=8, guess(8)=-1
    3:
        l=6, r=8, mid=7, guess(7)=0
    ```

```java
/* The guess API is defined in the parent class GuessGame.
   @param num, your guess
   @return -1 if my number is lower, 1 if my number is higher, otherwise return 0
      int guess(int num); */

public class Solution extends GuessGame {
    public int guessNumber(int n) {
        int l = 1;
        int r = n;
        
        int res = 0;
        
        while( l <= r) {            
            int mid = l + ((r - l)>>1); // `(l+r)/2` 은 할 수 없다. l+r이 int의 범위를 넘을 수 있기 때문에.
            int g = guess( mid );
            
            if ( g == 0 ) {
                res = mid;
                break;
            } else if ( g == 1) {
                l = mid + 1;
            } else {
                r = mid;
            }
        }
        
        return res;
    }
}
```

