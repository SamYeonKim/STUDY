
#import <Foundation/Foundation.h>

@interface Person : NSObject
{
    int *age;
    NSString *name;
}
@property int *age;
@property NSString *name;
- (void) showInfo;
@end


