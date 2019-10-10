#import "test.h"

@implementation Test
-(void)print {
    NSLog(@"Test - print()");
}
-(void)printWithInt:(int)num {
    NSLog(@"Test - printWithInt(), %d", num);
}
-(void)printWithString:(NSString*)str {
    NSLog(@"Test - printWithString(), %@", str);
}
-(void)printWithString:(NSString*)str number:(int)num {
    NSLog(@"Test - printWithString(), %@, %d", str, num);
}
-(void)execSelector {
    NSLog(@"Test - execSelector()");
    [self performSelector:@selector(print)];
}
@end
@implementation Test2
-(void)print {
    NSLog(@"Test2 - print()");
}
-(void)printWithInt:(int)num {
    NSLog(@"Test2 - printWithInt(), %d", num);
}
-(void)printWithString:(NSString*)str {
    NSLog(@"Test2 - printWithString(), %@", str);
}
-(void)execSelector {
    NSLog(@"Test2 - execSelector()");
    [self performSelector:@selector(print)];
}
@end

int main(void) {
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    
    Test *test = [[Test alloc] init];
    Test2 *test2 = [[Test2 alloc] init];
    //SEL sel = @selector(print);

    NSLog(@"[Class Test]");
    [test print];
    [test printWithInt:1];
    [test printWithString:@"Test"];
    [test printWithString:@"Test" number:10];
    [test execSelector];

    NSLog(@"[Class Test2]");
    [test2 print];
    [test2 printWithInt:2];
    [test2 printWithString:@"Test2"];
    [test2 execSelector];

    NSLog(@"[Selector - Test]");
    [test performSelector:@selector(print)];
    [test performSelector:@selector(printWithInt:) withObject:(id)10];
    [test performSelector:@selector(printWithString:) withObject:(NSString*)@"TestString"];
    [test performSelector:@selector(printWithString:number:) withObject:(NSString*)@"TestString" withObject:(id)10];
    [test performSelector:@selector(execSelector)];

    NSLog(@"[Selector - Test2]");
    [test2 performSelector:@selector(print)];
    [test2 performSelector:@selector(printWithInt:) withObject:(id)10];
    [test2 performSelector:@selector(printWithString:) withObject:(NSString*)@"TestString2"];
    [test2 performSelector:@selector(execSelector)];

    NSLog(@"[Test Case]");
    SEL sel1 = @selector(print);
    SEL sel2 = @selector(ABCDEFG); // 존재하지 않는 함수를 찍어봄
    SEL sel3 = @selector(print);
    NSLog(@"sel1 value : %x", sel1);
    NSLog(@"sel2 value : %x", sel2);
    NSLog(@"sel3 value : %x", sel3);
    [test performSelector:sel1];
    [test performSelector:sel2];
    [test2 performSelector:sel3];

    [pool drain];
}

/////////////////////////////////////////////////

// #import <Foundation/Foundation.h>

// @interface Car : NSObject {
//     int speed;
// }
// -(void) go;
// -(void) go:(int)a;
// -(void) go:(int)a speed:(int)s;
// -(void) go:(int)a speed:(int)s sound:(int)snd;
// @end

// @implementation Car
// -(void)go {
//     NSLog(@"go1");
// }

// -(void)go:(int)a {
//     NSLog(@"go2");
// }

// -(void)go:(int)a speed:(int)s {
//     NSLog(@"go3");
// }

// -(void)go:(int)a speed:(int)s sound:(int)snd {
//     NSLog(@"go4");
// }
// @end

// int main() {
//     NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
//     Car* a = [Car alloc];
//     [a go];

//     SEL s1 = @selector(go);
//     SEL s4 = @selector(abcdefg);

//     NSLog(@"%x",s1);
//     NSLog(@"%x",s4);

//     [a performSelector:s1];
//     [a performSelector:@selector(go:) withObject:(id)10];
//     [a performSelector:@selector(go:speed:) withObject:(id)10 withObject:(id)10];
//     [a performSelector:s4];

//     [pool drain];
//     [a release];
//     a = nil;

//     return 0;
// }