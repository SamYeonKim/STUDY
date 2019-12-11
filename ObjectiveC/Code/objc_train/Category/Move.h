
#import "Person.h"

@interface Person (Move)
- (void) moveToHome;
@end

@implementation Person (Move)
- (void) showInfo {
    static dispatch_once_t *pred = 0;
    _dispatch_once(&pred, ^{
       NSLog(@"Iam Mover");
    });
}
- (void) moveToHome {
    NSLog(@"GoTo Home");
}
@end
