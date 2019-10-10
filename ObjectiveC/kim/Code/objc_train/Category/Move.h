
#import "Person.h"

@interface Person (Move)
- (void) moveToHome;
@end

@implementation Person (Move)
- (void) showInfo {
    NSLog(@"Iam Mover");
}
- (void) moveToHome {
    NSLog(@"GoTo Home");
}
@end
