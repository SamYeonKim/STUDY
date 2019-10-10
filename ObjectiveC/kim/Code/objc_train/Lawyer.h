#import <Foundation/Foundation.h>

@interface Lawyer : Person
{
    int *identifier_code;
}
@property int *identifier_code;
- (void)showInfo;
@end

@implementation Lawyer
@synthesize identifier_code;
- (void)showInfo {
    NSLog(@"id : %d", identifier_code);
}
@end
