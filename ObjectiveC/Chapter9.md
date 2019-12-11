# Methods and Selectors

## Selector

셀렉터는 객체에 대해 실행할 메소드를 선택하는데 사용되는 이름이자 코드가 컴파일 될 때 이름을 대체하는 고유한 식별자입니다.
컴파일된 셀렉터는 `SEL` 이라는 데이터형으로 지정됩니다.

## Getting a Selector

셀렉터를 가져오는 방법으로는 두가지가 있습니다.

```
SEL selector = @selector(methodName);
```

또는 문자열 형태로도 셀렉터를 가져올 수 있습니다.

```
NSString* str = "my_func";
SEL selector = NSSelectorFromString(str);
```

## Using a Selector

NSObject의 performSelector 라는 함수를 사용하여 메소드 호출이 가능합니다.

```
- (id)performSelector:(SEL)aSelector;
- (id)performSelector:(SEL)aSelector withObject:(id)object;
- (id)performSelector:(SEL)aSelector withObject:(id)object withObject:(id)object;
```

밑의 3가지 방법은 모두 같은 함수를 호출합니다.

```
SEL selector = @selector(print);
[test performSelector:selector];

[test print];
[test performSelector:@selector(print)];
```

이러한 셀럭터의 특징은 런타임에 메소드를 변경할 수 있습니다.

```
// id helper = getTheReceiver();
SEL selector = [test getSelector];
[test performSelector:selector];
```

## Avoiding Messaging Errors

객체에 존재하지 않는 함수를 호출하면 오류가 발생하고, 셀렉터의 경우 런타임에 호출할 메소드를 결정할 일이 많기 때문에 발생하기도 쉽고 알아차리기도 어렵습니다.

NSObject에 respondsToSelector라는 메소드는 셀렉터를 인수로 받아 객체가 지정된 메소드에 응답할 수 있는 메소드가 있는지 혹은 상속하는지 여부를 나타냅니다.

```
-(BOOL)respondsToSelector:(SEL)aSelector;
```

```
if(![Sample respondsToSelector:@selector(print)]) {
    NSLog(@"Not Exist Method");
}
```