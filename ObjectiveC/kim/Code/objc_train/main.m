#import "Person.h"
#import "Category/Move.h"
#import "Category/Eat.h"
#import "Lawyer.h"
#import "Category/Etc.h"
int main (int argc, const char * argv[])
{
    
    /*
     * Category Example
     */
    Person *human = [[Person alloc]init];
    [human setAge:12];
    [human setName:@"kin"];
    [human showInfo];
    [human moveToHome];
    [human die];
    [human setMemorialDay:25];
    
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
    
    return 0;
    
    
}
