# Singleton

+ 클래스에 단하나의 인스턴스만을 유지하기위한 패턴.

## Diagram

+ PersonMgr의 인스턴스에 변수 값 세팅 후 다시 인스턴스 호출을 했을때 값이 유지되도록 구성.

![](img/singleton.jpeg)

## Code

`[C# Code]`

```csharp
using System;

// Singleton
class PersonMgr {
	public string m_current_person_name;
	
	static PersonMgr m_instance;
	public static PersonMgr inst {
		get {
			if (m_instance == null) {
				m_instance = new PersonMgr();
			}
			
			return m_instance;
		}
	}
}

public class Program {
	public static void Main() {
		PersonMgr person_mgr = PersonMgr.inst;
		person_mgr.m_current_person_name = "기정";
		
		person_mgr = PersonMgr.inst;
		Console.WriteLine("Current Person Name : " + person_mgr.m_current_person_name);
	}
}
```

`[Java Code]`

```java
// Singleton
class PersonMgr {
	public String m_current_person_name;
	
	static PersonMgr m_instance;
	public static PersonMgr inst() {
		if (m_instance == null) {
				m_instance = new PersonMgr();
			}
			
			return m_instance;
	}
}

public class MainClass{
     public static void main(String []args){
        PersonMgr person_mgr = PersonMgr.inst();
		person_mgr.m_current_person_name = "기정";
		
		person_mgr = PersonMgr.inst();
		System.out.println("Current Person Name : " + person_mgr.m_current_person_name);
     }
}
```

`[Objective C Code]`

```objc
@class PersonMgr;

// Singleton
static PersonMgr *m_instance;

@interface PersonMgr : NSObject {
@public
    NSString *m_current_person_name;
}

+ (PersonMgr *) inst;
@end

@implementation PersonMgr
+ (PersonMgr *) inst {
    if(m_instance == NULL) {
        m_instance = [[PersonMgr alloc] init];
    }
    
    return m_instance;
}
@end

int main(int argc, char * argv[]) {
    PersonMgr *person_mgr = PersonMgr.inst;
    person_mgr->m_current_person_name = @"기정";
    
    person_mgr = PersonMgr.inst;
    NSLog(@"Current Person Name : %@", person_mgr->m_current_person_name);
}
```

`[Python Code]`

```python
class PersonMgr:
	m_instance = None
	m_current_person_name = ""

	@classmethod
	def getinstance(cls):
		return cls.m_instance

	@classmethod
	def instance(cls, *args, **kargs):
		cls.m_instance = cls(*args, **kargs)
		cls.instance = cls.getinstance
		return cls.m_instance

person_mgr = PersonMgr.instance();
person_mgr.m_current_person_name = "기정";
		
person_mgr = PersonMgr.instance();
print("Current Person Name : " + person_mgr.m_current_person_name);
```

`[C++ Code]`

```cpp
#include <iostream>

using namespace std;

class PersonMgr {
public:
    static PersonMgr *m_instance;
	string m_current_person_name;
	
	static PersonMgr* inst() {
		if (m_instance == NULL) {
			m_instance = new PersonMgr();
		}
			
		return m_instance;
	}
};

int main() {
    PersonMgr *person_mgr = PersonMgr::inst();
	person_mgr->m_current_person_name = "기정";
		
	person_mgr = PersonMgr::inst();
	cout << "Current Person Name : " << person_mgr->m_current_person_name;
}
```