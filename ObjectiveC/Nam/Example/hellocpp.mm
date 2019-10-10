/* Hello.mm
 * Compile with: g++ -x objective-c++ -framework Foundation Hello.mm -o hello
 */
#import <Foundation/Foundation.h>

class HelloCpp {
private:
    id greeting_text; // holds an NSString
public:
    HelloCpp() {
       greeting_text = @"Hello, world!";
    }
    HelloCpp(const char* initial_greeting_text) {
        greeting_text = [[NSString alloc]
        initWithUTF8String:initial_greeting_text];
    }

    void say_hello() {
        printf("%s\n", [greeting_text UTF8String]);
    }
    
    virtual void say_hellocpp() {
        printf("Test Virtual Function in HelloCpp Class");
    }
};

@interface Greeting : NSObject {
    @private
        HelloCpp *hello;
}
- (id)init;
- (void)dealloc;
- (void)sayGreeting;
- (void)sayGreeting:(HelloCpp*)greeting;
@end

@implementation Greeting
- (id)init {
    if (self = [super init]) {
        hello = new HelloCpp();
    }
    return self;
}
- (void)dealloc {
    delete hello;
    [super dealloc];
}
- (void)sayGreeting {
    hello->say_hello();
}
- (void)sayGreeting:(HelloCpp*)greeting {
    greeting->say_hello();
}
@end

int main() {
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];

    Greeting *greeting = [[Greeting alloc] init];
    [greeting sayGreeting]; // > Hello, world!

    HelloCpp *hello = new HelloCpp("Bonjour, monde!");
    [greeting sayGreeting:hello]; // > Bonjour, monde!

    delete hello;
    [greeting release];
    [pool release];

    return 0;
}