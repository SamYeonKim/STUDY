# Problem

Reverse bits of a given 32 bits unsigned integer.

#### Note:

* Note that in some languages such as Java, there is no unsigned integer type. In this case, both input and output will be given as signed integer type and should not affect your implementation, as the internal binary representation of the integer is the same whether it is signed or unsigned.
* In Java, the compiler represents the signed integers using 2's complement notation. Therefore, in Example 2 above the input represents the signed integer -3 and the output represents the signed integer -1073741825.

#### Example 1:

```swift
Input: 00000010100101000001111010011100
Output: 00111001011110000010100101000000
Explanation: The input binary string 00000010100101000001111010011100 represents the unsigned integer 43261596, so return 964176192 which its binary representation is 00111001011110000010100101000000.
```

#### Example 2:

```swift
Input: 11111111111111111111111111111101
Output: 10111111111111111111111111111111
Explanation: The input binary string 11111111111111111111111111111101 represents the unsigned integer 4294967293, so return 3221225471 which its binary representation is 10101111110010110010011101101001.
```

# My Answer

* 32bit 이기 때문에 32번 순회돌자.
* 순회 돌면서 n의 첫번째 비트의 값을 가져오자 (1 or 0 ) 그러고 나서 n을 오른쪽 시프트해서 그 다음 비트를 첫번째 비트로 변경해 주자.
* 순회 돌면서 res를 왼쪽 시프트해서 첫번째 비트에 값을 넣을 준비하자.
* n의 첫번째 비트의 값을 res에 OR 연산해서 넣어 주자
  
```java
public class Solution {
    // you need treat n as an unsigned value
    public int reverseBits(int n) {
        int res = 0;
        
        for(int i=0;i<32;i++) {            
            int x = n & 1;            
            n = n >> 1;
            res = res << 1;
            res |= x;
        }
        
        return res;
    }
}
```

