#import "Person.h"

#import "Category/Eat.h"
#import "Category/Move.h"
#import "Lawyer.h"
#import "Category/Etc.h"
int main (int argc, const char * argv[])
{
//    @autoreleasepool {
        
    
    /*
     * Category Example
     */
//    Person *human = [[Person alloc]init];
    Person *human = [[Person alloc] initWithName:@"Go"];
    NSLog(@"%@", human.name);
//    [human setAge:12];
    [human setName:@"kin"];
    human.Cast = 10;
    NSLog(@"%d", human.cost);
    
    int rate[10];
    
//    for (int a=0; a<100000; a++) {
//        Person *asc = [[Person alloc] initWithName:@"Go"];
//        
//    }
    
//    [human invokeDelegate:^(int num){
//        NSLog(@"%d", num + num);
//    }];
    
    NSLog(@"%d", [human max:2 addNum:3]);
    [human showInfo];
    [human showInfo];
    [human showInfo];
    [human moveToHome];
    [human die];
    [human setMemorialDay:25];
    
    Lawyer *lawyer = [Lawyer new];
    [lawyer showInfo];


    
    NSDictionary *dic = [NSDictionary dictionaryWithObjectsAndKeys:
                         @"1", @"a",
                         @"2", @"b",
                         @"3", @"c",
                         nil];
    
    for(id a in dic) {
        NSLog(@"%@, %@", a, [dic objectForKey:a]);
    }
        
        
    /*
     * Dynamic Typing
     * Error occured In RunTime
     */
    //    id person = [[Person alloc]init];
    //    [person setIdentifier_code:45];
    //    id lawyer = [[Lawyer alloc] init];
    //    [lawyer setIdentifier_code:45];
    
    /* Static Typing
     * Error occured In Compile Time
     */
    //    Person *man = [[Person alloc]init];
    //    [man setIdentifier_code:45];
    //    Lawyer *attorney = [[Lawyer alloc] init];
    //    [attorney setIdentifier_code:45];
//    }
    return 0;
    
    
}
