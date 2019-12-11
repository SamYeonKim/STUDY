#import <Foundation/Foundation.h>
#import "Person.h"
#import "Category/Etc.h"

@implementation Person
//@synthesize age;
@synthesize name;
@synthesize memorialDay;
@synthesize cost;

- (void) setCast:(int)input {
    cost = input;
}
-(id)init {
    if (!(self = [super init]))
        return nil;
    
    age = 15;
    name = @"KIM";
    
    NSLog(@"call super");
    
    return self;
}

-(Person*)initWithName:(NSString*)Name{
    self = [self init];
    
    NSLog(@"call");
    name = Name;
    
    return self;
}

-   (void)showInfo {
    NSLog(@"AGE : %d", age);
    NSLog(@"NAME : %@",name);
}

- (void) die {
    NSLog(@"Dead");
}

-(int) max:(int)num1 addNum:(int)num2 {
    return num1 * num2;    
}

-(void) showUniqueId {
    NSLog(@"%@", name);
}
@end
