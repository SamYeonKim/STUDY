# Fast Enumeration

* 컬렉션의 내용을 열거할 수 있는 언어 기능을 제공함

* 컬렉션 : 개체를 효율적으로 저장하고 검색하는 일반적인 방법을 제공
ex) NSArray, NSDictionary

```c
for (Type newVariable in expression) { statements }
of
Type existingItem;
for(existing in expression) { statements }

// newVariable or existing : iterating 변수
```

* iterating 변수는 반환된 객체의 각 항목에 차례로 설정되고, statements에 정의된 코드가 실행
* fast enumeration의 이점
    * NSEnumerator를 직접 사용하는 것보다 훨씬 효율적
    * 구문 간결
```c
- (NSUInteger)countByEnumeratingWithState:(NSFastEnumerationState *)state
                                  objects:(id *)stackbuf
                                    count:(NSUInteger)len
```
* countByEnumeratingWithState:objects:count: 메소드를 통해 컬렉션의 멤버들을 버퍼에 담음. 이를 통해 for문이 Single스레드에서 작동하는 것과는 달리, 객체들이 동시에 로드됨. 사용가능한 시스템자원으로 병렬작업을 하기 때문에 더 나은 퍼포먼스를 제공.

#### Objective-C
```c
// NSArray 예제
NSArray *array = [NSArray arrayWithObject:@"One", @"Two", @"Three", @"Four", nil];

for(NSString *element in array){
    NSLog(@"%@", element);
}
```

#### C#

```c
string[] str = new string[] { "One", "Two", "Three", "Four" };

foreach (string item in str)
{
    System.Console.WriteLine(item);
}
```

```c
//인덱스는 제공하지 않으므로 다음과 같이 사용
int index = 0;
for(NSString *element in array){
    NSLog(@"%@", element);
    index++; 
}

// NSDictionary 예제
NSDictionary * dictionary = NSDictionary dictionaryWithObjectsAndKeys:
@"quattor", @"four",        // value, key 순
@"quinque", @"five", nil];

NSString* key;
for(key in dictionary){
    NSLog(@"%@, %@", key, [dictionary valueForKey:key]);
}

// NSEnumerator를 사용한 예제
NSEnumerator *Eenumerator = [array reverseObjectEnumerator];

for(NSString *element in enumerator){
    if([element isEqualToString:@"Three"]){
        break;
    }
}

```

* Q
* disassemble 해서 비교해보기