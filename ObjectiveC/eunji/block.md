# Block
```objc
반환형(^블록명)(파라미터 타입) = ^(인수){
//구현 코드
}

void (^theBlock)() = ^ {
    NSLog(@"Hello Block");
}
```
* 애플이 추가한 것으로 표준 ANSI C 정의에는 포함되지 않음
* 함수와 비슷하게 생겼고 동작도 유사함
* 함수와 달리 정의한 함수나 메서드 안에서 정의할 수 있고, 동일한 범위의 블록 바깥에서 정의한 변수에도 접근 가능(값의 변경은 불가능)
```objc
#import <Foundation/Foundation.h>
 
int main() {
    int (^result)(int, int) = ^(int a, int b) {
        return a + b;
    };
    
    NSLog(@"%d", result(5,-2));     // 3
    
    int c = 10;
    int (^result2)(int, int) = ^(int a, int b) {
        //c = 12;               //error
        return c + a + b;
    };
    
    NSLog(@"%d", result2(1,1));     // 12
    return 0;
}
```

```cs
class Program {
    delegate int Result(int a, int b);
    static void Main() {
        Result res = (int a, int b) => {
            return a + b;
        };

        Console.WriteLine(res(5, -2));  // 3

        int c = 10;
        Result res2 = (int a, int b) => {
            c = 12;         //C#은 값 변경 가능
            return c + a + b;
        };

        Console.WriteLine(res2(1, 1));  // 14
    }
}
```