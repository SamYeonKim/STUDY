#import <Foundation/Foundation.h>
#import "Category/Etc.h"

@interface Lawyer : Person
{
    int *identifier_code;
}
@property int *identifier_code;
- (void)showInfo;
@end

@implementation Lawyer
@synthesize identifier_code;
- (id)init {
    if ( !(self = [super init]))
        return nil;
    
    identifier_code = 100;
    NSLog(@"call lawyer");
    return self;
}
- (void)showInfo {
    NSLog(@"id : %d", identifier_code);
}
@end
