#import <Foundation/Foundation.h>
#import "Person.h"
#import "Category/Etc.h"

@implementation Person
@synthesize age;
@synthesize name;
@synthesize memorialDay;
-   (void)showInfo {
    NSLog(@"AGE : %d", age);
    NSLog(@"NAME : %@",name);
}

- (void) die {
    NSLog(@"Dead");
}
@end
