#import "Person.h"

@interface Person (Eat)
@end

@implementation Person (Eat)
- (void) showInfo {
    NSLog(@"Iam Eater");
}
@end
