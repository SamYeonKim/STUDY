
#import <Foundation/Foundation.h>

@protocol Show
- (void) showUniqueId;
@end

@interface Person : NSObject<Show>
{
    @public
        int age;
        NSString *name;
}
//@property int *age;
@property NSString *name;
@property (setter=Cast:) int cost;
- (Person*) initWithName:(NSString*)Name;
- (void) setCast : (int)input;
- (void) showInfo;
- (int) max:(int) num1 addNum:(int) num2;
@end


