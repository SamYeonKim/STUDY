# Problem

Implement pow(x, n), which calculates x raised to the power n (xn).

#### Example 1:

```swift
Input: 2.00000, 10
Output: 1024.00000
```

#### Example 2:

```swift
Input: 2.10000, 3
Output: 9.26100
```

#### Example 3:

```swift
Input: 2.00000, -2
Output: 0.25000
Explanation: 1/4 = 0.25
```

# My Answer
  
```java
class Solution {
    public double myPow(double x, int n) {
        if ( n == 0 || x == 1 )
            return 1;
        
        if ( x == -1 ) {
            boolean b_is_even = n % 2 == 0;
            return b_is_even ? 1 : -1;
        }
        
        if ( x == 0 || n == Integer.MAX_VALUE || n == Integer.MIN_VALUE)
            return 0;
        
        return tailPow(x, 1, n);
    }
    
    public double tailPow(double x, double total, int n) {
        if ( n == 0 )
            return total;
        
        boolean b_is_negative = n < 0;
        
        if ( b_is_negative ) {
            return tailPow(x, total/x, ++n);
        } else {
            return tailPow(x, x * total, --n);    
        }        
    }
}
```

