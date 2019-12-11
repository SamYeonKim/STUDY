# Protocol

* 선언만 되어있고 구현되지 않은 메소드의 집합.
* c# 의 `interface`와 거의 동일하다.

----
## Methods for Others to Implement

* 다른 객체에 메시지를 보낼 때, 해당 파일의 타입이 정해지지 않았다면, `selector`를 사용해야 해당 객체의 메소드에 접근할 수 있다. 프로토콜은 비교하는 번거로움 없이 바로 접근이 가능
* 이러한 디자인이 가장 많이 쓰이는 곳이 델리게이트

```objc
// main.h
@interface TestSelector : NSObject {
    id assistant;
}
//-(void)setAssistant:(id)obj;
-(BOOL)doWork;
@end
@interface HelpSelector : NSObject
-(void)helpOut:(id)sender;
@end

@protocol DelegateProtocol
-(void)helpOut:(id)sender;
@end

@interface TestDelegate : NSObject<DelegateProtocol> {
    id<DelegateProtocol> delegate;
}
//-(void)setDelegate:(id<DelegateProtocol>)del;
-(BOOL)doWork;
@end
@interface HelpDelegate : NSObject <DelegateProtocol>
@end

// main.m
#import <Foundation/Foundation.h>
#import <main.h>

@implementation TestSelector

-(BOOL)doWork {
    if ( [assistant respondsToSelector:@selector(helpOut:)]) {
      [assistant helpOut:self];
      return YES;
    }
    return NO;
}
-(void)setAssistant:val {
   assistant = val;
}
@end
@implementation HelpSelector
-(void)helpOut:(id)sender {
   NSLog(@"HelpSelector call helpOut : %@", sender);
}
@end

@implementation TestDelegate
-(BOOL)doWork {
    [delegate helpOut:self];
    return YES;
}
-(void)setDelegate:val {
   delegate = val;
}
@end
@implementation HelpDelegate
-(void)helpOut:(id)sender {
   NSLog(@"HelpDelegate call helpOut : %@", sender);
}
@end

int main(int argc, const char * argv[]) {
   TestSelector *ts = [[TestSelector alloc]init];
   HelpSelector *hs = [[HelpSelector alloc]init];
   ts.assistant = hs;
   //[ts setAssistant:hs];
   [ts doWork];
   TestDelegate *td = [[TestDelegate alloc]init];
   HelpDelegate *hd = [[HelpDelegate alloc]init];
   //[td setDelegate:hd];
   td.delegate = hd;
   [td doWork];
   
   return 0;
}

// output
2019-04-18 14:44:16.662 a.out[12] HelpSelector call helpOut : <TestSelector: 0x18eab48>  
2019-04-18 14:44:16.681 a.out[12] HelpDelegate call helpOut : <TestDelegate: 0x1a4c5c8>
```

----
## Formal Protocols

* `@protocol` 로 시작 `@end` 로 끝낸다.
* `@required`, `@optional` 키워드를 사용할 수 있다. 디폴트는 `@required`
  * `@required`: 필수로 구현해야 하는 메소드
  * `@optional`: 선택적으로 구현할 수 있는 메소드
* 이름 뒤에 `:`을 붙여서 다른 프로토콜을 상속할 수 있다.

```objc
@protocol MyProtocol : NSObject
-(void)requiredMethod;
@optional
-(void)anOptionalMethod;
-(void)anotherOptionalMethod;
@required
-(void)anotherRequiredMethod;
@end;
```

----
## Informal Protocols

* 카테고리를 프로토콜처럼 사용하는 방법도 있다.
* `NSObject`는 모든 객체가 상속받기 때문에 `NSObject`의 카테고리로 메소드를 선언하면 별도의 프로토콜 없이 해당 메소드를 선언한 것과 동일한 효과가 나온다.

----
## Protocol Objects

* 오브젝트로 저장할 수 있다.

```objc
Protocol *myProtocol = @protocol(ProtocolDelegate);
```

----
## Adopting a Protocol

* 객체가 프로토콜을 사용할 때 `angle brackets(<>)`를 사용한다.
* 여러개도 가능

```objc
@interface ClassName : ItsSuperclass < protocol list >
@interface ClassName ( CategoryName ) < protocol list >
@interface Formatter : NSObject < Formatting, Prettifying >
```

----
## Conforming to a Protocol

* 해당 객체가 특정 프로토콜을 준수하는지 확인하려면 `conformsToProtocol` 메시지를 사용하면 된다.
* 해당 객체가 프로토콜의 함수를 구현했는지 확인하려면 `respondsToSelector:@selector`를 사용해야 한다.

```objc
if ([self conformsToProtocol:@protocol(ProtocolDelegate)]) {
      NSLog(@"conformsToProtocol true");
   } else {
      NSLog(@"conformsToProtocol false");
   }


   if(delegate && [delegate respondsToSelector:@selector(processCompleted)]){
      [delegate processCompleted];
   }
```

----
## Type Checking

* 특정 프로토콜을 사용하는 객체는 프로토콜을 타입 구분용으로 사용할 수 있다.

```objc
- (id <Formatting>)formattingService;
id <MyXMLSupport> anObject;
id <ProtocolDelegate> test = [[Test alloc]init];
```

----
## Protocols Within Protocols

* 프로토콜이 프로토콜을 사용할 수 있다.

```objc
@protocol ProtocolName < protocol list >
```

----
## Referring to Other Protocols

* 프로토콜을 쓰는데 아직 작성되지 않았다면, 먼저 선언만 해놓을 수 있다.
* `@class ClassName;`과 비슷함

```objc
@protocol ProtocolDelegate;
```

```objc
// main.h
@protocol ProtocolDelegate; // predefine Protocol

@interface Print :NSObject {
   id delegate;
}

- (void) printDetails;
- (void) setDelegate:(id)newDelegate;
@end

@interface Test:NSObject<ProtocolDelegate>
- (void)startAction;
@end

@interface NSObject (CategoryProtocol)
-(void)addCategoryMethod;
@end


@protocol ProtocolDelegate
@optional
- (void)processCompleted;
@end

// main.m
#import <Foundation/Foundation.h>
#import <main.h>

@implementation Print
- (void)printDetails {
   NSLog(@"Printing Details");
   if(delegate && [delegate respondsToSelector:@selector(processCompleted)]){
      [delegate processCompleted];
   }
}

- (void) setDelegate:(id)newDelegate {
   delegate = newDelegate;
}

@end

@implementation Test
- (void)startAction {
   Print *print = [[Print alloc]init];
   if ([self conformsToProtocol:@protocol(ProtocolDelegate)]) {
        NSLog(@"conformsToProtocol true");
   } else {
        NSLog(@"conformsToProtocol false");
   }
   [print setDelegate:self];
   [print printDetails];
}

-(void)processCompleted {
   NSLog(@"Printing Process Completed");
}

-(void)addCategoryMethod {
   NSLog(@"Add Category Method");
}

@end

int main(int argc, const char * argv[]) {
   NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
   id <ProtocolDelegate> *test = [[Test alloc]init];
   [test startAction];
   [test addCategoryMethod];
   [pool drain];
   return 0;
}

// output
2019-04-18 15:29:18.289 a.out[12] conformsToProtocol true                                
2019-04-18 15:29:18.436 a.out[12] Printing Details                                       
2019-04-18 15:29:18.436 a.out[12] Printing Process Completed                             
2019-04-18 15:29:18.436 a.out[12] Add Category Method   
```

```cs
using System;

interface ProtocolDelegate {
    void processCompleted();
}

public static class CategoryProtocol {
    public static void addCategoryMethod(this object obj) {
        Console.WriteLine("Add Category Method");
    }
}

class Print {
    ProtocolDelegate Delegate;

    public void printDetails() {
        Console.WriteLine("Printing Details");
        Delegate.processCompleted();
    }

    public void setDelegate(object newDelegate) {
        Delegate = (ProtocolDelegate)newDelegate;
    }
}

class Test : ProtocolDelegate {
    public void startAction() {
        Print print = new Print();
        print.setDelegate(this);
        print.printDetails();
    }
    public void processCompleted() {
        Console.WriteLine("Printing Process Completed");
    }
}
					
public class Program {
	public static void Main() {
		ProtocolDelegate test = new Test();
		((Test)test).startAction();
		((Test)test).addCategoryMethod();
	}
}

// output
Printing Details
Printing Process Completed
Add Category Method
```

# Q

* 메소드가 정의되었는지 확인하는 방법
* 프로토콜 포인터 사용 예제