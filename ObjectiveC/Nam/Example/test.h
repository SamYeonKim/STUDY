#import <Foundation/Foundation.h>

@interface Test : NSObject
-(void)print;
-(void)printWithInt:(int)num;
-(void)printWithString:(NSString*)str;
-(void)printWithString:(NSString*)str number:(int)num;
-(void)execSelector;
@end

@interface Test2 : NSObject
-(void)print;
-(void)printWithInt:(int)num;
-(void)printWithString:(NSString*)str;
-(void)execSelector;
@end