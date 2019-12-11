# Exception Handling

## 사용법

`@try, @catch, @throw, @finally`를 이용.

```
@try {
    // 예외상황이 생길 수 있는 코드 작성
} @catch (NSException *exception) {
    // @try 문에서 발생한 예외상황을 처리 할 코드 작성.
} @finally {
    // 예외상황이 발생 유무에 상관없이 무조건 실행되는 코드 작성.
}
```

```
ex) a 클래스에 존재하지 않는 abc 함수를 호출했을때의 예외상황.

int main(int argc, char * argv[]) {
    PropertyTest *a = [[PropertyTest alloc] init];
    
    @try {
        [a abc];
    } @catch (NSException *exception) {
        NSLog(@"main: Caught %@: %@", [exception name], [exception  reason]);
    } @finally {
        NSLog(@"무조건 실행");
    }
    
    @autoreleasepool {
        return UIApplicationMain(argc, argv, nil, NSStringFromClass([AppDelegate class]));
    }
}
```

![](Images/Error_2.png)

## Custom Exception

+ NSException을 상속받아 커스텀한 Exception을 만들 수 있다.

```
@interface CustomException : NSException {
    NSString* company;
}
@property NSString* company;
@end
@implementation CustomException
@synthesize company;
@end
``` 

```
ex)

int main(int argc, char * argv[]) {
    @try {
        CustomException *exception = [[CustomException alloc] initWithName:@"Test" reason:@"Test" userInfo:nil];
        exception.company = @"nadagames";
        
        @throw exception;
    } @catch (CustomException *custom_exception) {
        NSLog(@"main: Caught %@: %@: %@", [custom_exception name], [custom_exception  reason], [custom_exception company]);
    } @finally {
        NSLog(@"무조건 실행");
    }
    
    @autoreleasepool {
        return UIApplicationMain(argc, argv, nil, NSStringFromClass([AppDelegate class]));
    }
}
```

![](Images/Error_3.png)

## Exception 클래스 사용하기

+ NSException를 직접 선언하여 예외를 처리 할 수도 있다.

```
ex)

int main(int argc, char * argv[]) {
    @try {
        NSException *exception = [NSException exceptionWithName:@"TestException"
                                                         reason:@"Test"  userInfo:nil];
        
        @throw exception;
    } @catch (NSException *exception) {
        NSLog(@"main: Caught %@: %@", [exception name], [exception  reason]);
    } @finally {
        NSLog(@"무조건 실행");
    }
    
    @autoreleasepool {
        return UIApplicationMain(argc, argv, nil, NSStringFromClass([AppDelegate class]));
    }
}
```

![](Images/Error_4.png)