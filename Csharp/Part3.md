## Events

* `Windows Form`에서 UI 개발의 핵심개념
* `callback`의 특별한 형태
  * `event` 키워드를 사용
  * 할당이 불가능하고 추가, 제거만 가능
  * 제거 시 대응 함수가 없으면 예외는 발생하지 않지만, 실제로 핸들러를 제거한다고 생각해서 예기치 않은 동작이 발생할 수 있다.
* 여러 메소드를 추가할 수 있다.
  * `Delegate` 클래스의 [Combine](https://docs.microsoft.com/ko-kr/dotnet/api/system.delegate.combine?view=netframework-4.8) 메소드를 사용하여 여러 delegate를 묶는다.
  * 이 때, 컴파일러가 [CompareExchange](https://docs.microsoft.com/ko-kr/dotnet/api/system.threading.interlocked.compareexchange?view=netframework-4.8) 명령으로 `lock` 문을 삽입하여 `thread-safe`하게 만든다.

```cs
class Program {
   static void Main(string[] args) {
      Application.callback += () => Console.WriteLine("Number hit");
      Application.Run();
   }
}

static class Application {
   public static event Action callback;

   public static void Run() {
      Random r = new Random(14);

      while (true) {
            double p = r.NextDouble();

            if (p < 0.0001 && callback != null)
               callback();
            else if (p > 0.9999)
               break;
      }
   }
}
```

```java
import designPattern.Application.EventDelegate;

public class Main {
	public static void main(String[] args) {
	    Application a = new Application();
	    a.callback.addListener("print", () -> {
	    	System.out.println("Number hit");
	    });
	    a.Run();
	}
}

class Application {
   @FunctionalInterface
   public interface EventDelegate {
       void invoke();
   }
   public Event<EventDelegate> callback = new Event<EventDelegate>();

   void RaiseEvent() {
	   for (EventDelegate listener : callback.listeners()) {
           listener.invoke();
       }
   }
   
   public void Run() {
      while (true) {
            double p = Math.random();

            if (p < 0.0001 && callback != null)
            	RaiseEvent();
            else if (p > 0.9999)
               break;
      }
   }
}

//----------------------------------------------------------------------------------------
//Copyright © 2007 - 2019 Tangible Software Solutions, Inc.
//This class can be used by anyone provided that the copyright notice remains intact.
//
//This class is used to convert C# events to Java.
//----------------------------------------------------------------------------------------
class Event<T> {
    private java.util.Map<String, T> namedListeners = new java.util.HashMap<String, T>();
    public void addListener(String methodName, T namedEventHandlerMethod) {
        if (!namedListeners.containsKey(methodName))
            namedListeners.put(methodName, namedEventHandlerMethod);
    }
    public void removeListener(String methodName) {
        if (namedListeners.containsKey(methodName))
            namedListeners.remove(methodName);
    }

    private java.util.List<T> anonymousListeners = new java.util.ArrayList<T>();
    public void addListener(T unnamedEventHandlerMethod) {
        anonymousListeners.add(unnamedEventHandlerMethod);
    }

    public java.util.List<T> listeners() {
        java.util.List<T> allListeners = new java.util.ArrayList<T>();
        allListeners.addAll(namedListeners.values());
        allListeners.addAll(anonymousListeners);
        return allListeners;
    }
}
```

```objc
#import <Foundation/Foundation.h>

@protocol EventDelegate
- (void) invoke;
@end

@interface MyEvent:NSObject<EventDelegate> {
    NSMutableArray *callback;   
}
- (id)init;
- (void) invoke;
- (void) addListener:(id)eventHandler;
@end

@interface Application:NSObject {
    MyEvent *event;
}
- (id)init;
- (void) Run;
- (void) SetEvent:(id)event;
@end

@implementation MyEvent:NSObject
- (id)init {
    if(self == [super init]){
        callback = [[NSMutableArray alloc] init];
    }
    return self;
}

- (void) invoke {
    int i;
    for (i = 0; i < [callback count]; i++)
        NSLog([callback objectAtIndex: i]);
}

- (void) addListener:(id)eventHandler {
    [callback addObject:eventHandler];
}
@end

@implementation Application:NSObject
- (id)init {
    if(self == [super init]){
        event = [[MyEvent alloc] init];
    }
    return self;
}


- (void) Run {
    [event invoke];
}

- (void) SetEvent:(id)del_event {
    event = del_event;
}
@end

int main (int argc, const char * argv[]) {
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    
    MyEvent *event = [[MyEvent alloc] init];
    //[event addListener:^(void){ NSLog(@"Number hit"); }];
    [event addListener:@"aa"];
    Application *application = [[Application alloc] init];
    [application SetEvent:event];
    [application Run];
    
    [pool drain];
    return 0;
}
```

```python
import random

class Application:

	def __init__(self):
		self.callback = []

	def Run(self):
		while True:
			r = random.random()
			if r < 0.0001 and self.callback is not None:
				for func in self.callback:
					func()
			elif r > 0.9999:
				break

application = Application()
application.callback.append(lambda: print("Number hit"))
application.Run()
```

```cpp
#include <iostream>
#include <vector>
#include <memory>
#include <functional>

class EventHandler {
public:
	using Func = std::function<void()>;

private:
	Func _func;

public:
	int id;
	int counter;

	EventHandler() : id{ 0 } {}
	EventHandler(const Func &func) : _func{ func } {
		id = ++counter;
	}
	void operator()() {
		_func();
	}
	void operator=(const EventHandler &func) {
		if (_func == nullptr) {
			_func = func;
			id = ++EventHandler::counter;
		} else
			std::cerr << "안됨" << std::endl;
	}
	bool operator==(const EventHandler &del) {
		return id == del.id;
	}
	bool operator!=(nullptr_t) {
		return _func != nullptr;
	}
};

class Event {
public:
	void addHandler(const EventHandler &handler) {
		handlers.push_back(std::unique_ptr<EventHandler>(new EventHandler{ handler }));
	}
	void removeHandler(const EventHandler &handler) {
		for (std::vector<std::unique_ptr<EventHandler>>::iterator to_remove = handlers.begin();
			   to_remove != handlers.end(); ++to_remove) {
			if (*(*to_remove) == handler) {
				handlers.erase(to_remove);
				break;
			}
		}
	}
	void operator()() {
		notifyHandlers();
	}
	Event &operator+=(const EventHandler &handler) {
		addHandler(handler);
		return *this;
	}
	Event &operator-=(const EventHandler &handler) {
		removeHandler(handler);
		return *this;
	}

private:
	std::vector<std::unique_ptr<EventHandler>> handlers;

	void notifyHandlers() {
		for (std::vector<std::unique_ptr<EventHandler>>::iterator func = handlers.begin();
			 func != handlers.end(); ++func) {
			if (*func != nullptr)
				(*(*func))();
		}
	}
};


class Application {
public:
	Event callback;

	void Run() {
		while (true) {
			double p = std::rand() / (double)RAND_MAX;

			if (p < 0.0001)
				callback();
			else if (p > 0.9999)
				break;
		}
	}
};
int main() {
	Application a;
	EventHandler func([]() {
		std::cout << "Number hit" << std::endl;
	});
	a.callback.addHandler(func);
	a.Run();

	return 0;
}
```

----
## The .NET standard event pattern

* `.NET`에서 제공하는 이벤트 처리 패턴

* 이벤트 송신자는 핸들러가 있는지 없는지 알 필요가 없다.
* 이벤트의 송신자를 구분하기 위해 첫 번째 인수를 이벤트 송신자로 지정한다.
  * `.NET`은 `object`를 첫 번째 파라미터로 받는다.
* 두번째 파라미터는 현재의 변수 / 상태를 전송하는 객체를 사용한다.

```cs
delegate void EventHandler(object sender, EventArgs e);
```

----
## Reflection

* 어셈블리와 관련된 메타 데이터 정보 집합에 접근하는 방법
  * 메타 데이터에는 정의된 유형, 메소드에 대한 정보가 포함된다.
* 사용방법은 여러가지가 있지만 3개만 본다.
  * 인스턴스의 타입 가져오기 - GetType()
  * 컴파일 타임에 `Type` 인스턴스 취득 - typeof()
  * `Assembly` 클래스를 사용하여 어셈블리를 로드 
* 아래의 팩토리 메소드 예제는 리플렉션 사용의 장점을 보여준다.
  * 코드는 길어지지만, 유지 관리가 편리하다.

```cs
using System.Reflection;

class Program {
   static void Main(string[] args) {
      var test = Document.CreateElement("img");
      Console.WriteLine("test : " + test.GetType());
      test.Print();
      var test2 = Document2.CreateElement("img");
      Console.WriteLine("test : " + test.GetType());
      test2.Print();
   }
}

class HTMLElement {
   string m_tag;

   public HTMLElement(string tag) {
      m_tag = tag;
   }

   public string Tag {
      get { return m_tag; }
   }

   public virtual void Print() {
      Console.WriteLine("HTMLElement, tag : " + m_tag);
   }
}

class HTMLImageElement : HTMLElement {
   public HTMLImageElement() : base("img") {
   }

   public override void Print() {
      Console.WriteLine("HTMLImageElement, tag : " + Tag);
   }
}

class HTMLParagraphElement : HTMLElement {
   public HTMLParagraphElement() : base("p") {
   }

   public override void Print() {
      Console.WriteLine("HTMLParagraphElement, tag : " + Tag);
   }
}

// 리플렉션 x
static class Document {
   public static HTMLElement CreateElement(string tag) {
      switch (tag) {
         case "img":
         return new HTMLImageElement();
         case "p":
         return new HTMLParagraphElement();
         default:
         return new HTMLElement(tag);
      }
   }
}

// 리플렉션 o
static class Document2 {
   static Dictionary<string, ConstructorInfo> specialized;

   public static HTMLElement CreateElement(string tag) {
      if (specialized == null) {
         specialized = new Dictionary<string, ConstructorInfo>();
         // 현재 실행중인 메소드의 어셈블리를 가져와 저장된 모든 타입을 반환 (that includes those HTMLElement types)
         Type[] types = Assembly.GetCallingAssembly().GetTypes();
         
         foreach (var type in types) {
            // HTMLElement이거나 상속 받았는지 확인
            if (type.IsSubclassOf(typeof(HTMLElement)) || type == typeof(HTMLElement)) {
               // 파라미터가 없는 생성자를 서치
               ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
               
               if (ctor != null) {
                  // 생성자 호출 후 인스턴스를 변수에 저장
                  var element = ctor.Invoke(null) as HTMLElement;

                  // 딕셔너리에 인스턴스 저장
                  if (element != null)
                     specialized.Add(element.Tag, ctor);
               }
            }
         }
      }

      // 저장된 생성자를 실행한다.
      if (specialized.ContainsKey(tag))
            return specialized[tag].Invoke(null) as HTMLElement;

      //Otherwise this is an object without a special implementation; we know how to handle this!
      return new HTMLElement(tag);
   }
}

// output
test : Facade.HTMLImageElement
HTMLImageElement, tag : img
test : Facade.HTMLImageElement
HTMLImageElement, tag : img
```

```java
import java.lang.reflect.*;

public class Main {
	public static void main(String[] args) throws IllegalArgumentException, IllegalAccessException, NoSuchFieldException, SecurityException, InstantiationException, InvocationTargetException, NoSuchMethodException {
		HTMLElement test = Document.CreateElement("img");
		System.out.println("test : " + test.getClass());
		test.Print();
		HTMLElement test2 = Document2.CreateElement("img");
		System.out.println("test : " + test.getClass());
		test2.Print();
	}
}

public class HTMLElement {
	   String m_tag;

	   public HTMLElement(String tag) {
	      m_tag = tag;
	   }

	   public String GetTag() {
	       return m_tag;
	   }

	   public void Print() {
	      System.out.println("HTMLElement, tag : " + m_tag);
	   }
}
public class HTMLImageElement extends HTMLElement {
   public HTMLImageElement() {
	   super("img");
	   m_tag = "img";
   }
   
   @Override
   public void Print() {
	   System.out.println("HTMLImageElement, tag : " + m_tag);
   }
}
public class HTMLParagraphElement extends HTMLElement {
   public HTMLParagraphElement() {
	   super("p");
   }

   @Override
   public void Print() {
	   System.out.println("HTMLParagraphElement, tag : " + m_tag);
   }
}

public class Document {
   public static HTMLElement CreateElement(String tag) {
	     switch (tag) {
         case "img":
         return new HTMLImageElement();
         case "p":
         return new HTMLParagraphElement();
         default:
         return new HTMLElement(tag);
      }
   }
}

import java.lang.reflect.*;
import java.util.HashMap;
import java.util.Map;
import java.util.Vector;

public class Document2 {
   static Map<String, Constructor> specialized;

   public static HTMLElement CreateElement(String tag) throws IllegalArgumentException, IllegalAccessException, NoSuchFieldException, SecurityException, InstantiationException, InvocationTargetException, NoSuchMethodException {
      if (specialized == null) {
         specialized = new HashMap<String, Constructor>();
         
         Field f = ClassLoader.class.getDeclaredField("classes");
         f.setAccessible(true);
         ClassLoader classLoader = Thread.currentThread().getContextClassLoader();
         Vector<Class> classes =  (Vector<Class>) f.get(classLoader);
         for (int i = 0; i < classes.size(); ++i) {
            Class c = classes.get(i);
            if (c.getSuperclass().equals(HTMLElement.class)) {
                  Constructor cons = c.getConstructor();
                  
                  HTMLElement tmp = (HTMLElement)cons.newInstance(null);
                  specialized.put(tmp.m_tag, cons);
            }
         }
      }

      // 저장된 생성자를 실행한다.
      if (specialized.containsKey(tag))
            return (HTMLElement)specialized.get(tag).newInstance(null);

      //Otherwise this is an object without a special implementation; we know how to handle this!
      return new HTMLElement(tag);
   }
}

// output
test : class HTMLImageElement
HTMLImageElement, tag : img
test : class HTMLImageElement
HTMLImageElement, tag : img
```

```objc
//introspection
#import <Foundation/Foundation.h>

@interface HTMLElement : NSObject {
    NSString *m_tag;
}
- (id) init:(NSString*)tag;
- (NSString*) GetTag;
- (void) Print;
@end
    
@implementation HTMLElement

- (id)init:(NSString*)tag {
   self = [super init];
   m_tag = tag;
   return self;
}
- (NSString*) GetTag {
    return m_tag;
}
- (void) Print {
    NSLog (@"HTMLElement, tag : %@", m_tag);
}

@end

@interface HTMLImageElement : HTMLElement
- (void) Print;
@end
    
@implementation HTMLImageElement
- (id)init {
   self = [super init:@"img"];
   return self;
}
- (void) Print {
    NSLog (@"HTMLImageElement, tag : %@", m_tag);
}
@end

@interface HTMLParagraphElement : HTMLElement
- (void) Print;
@end
    
@implementation HTMLParagraphElement
- (id)init {
   self = [super init:@"p"];
   return self;
}
- (void) Print {
    NSLog (@"HTMLParagraphElement, tag : %@", m_tag);
}
@end

@interface Document : NSObject
- (HTMLElement*) CreateElement:(NSString*)tag;
@end

@implementation Document
- (HTMLElement*) CreateElement:(NSString*)tag {
    if([tag isEqualToString:@"img"]) {
        return [[HTMLImageElement alloc] init];
    } else if ([tag isEqualToString:@"img"]) {
        return [[HTMLParagraphElement alloc] init];
    } else {
        return [[HTMLElement alloc] init:tag];
    }
}
@end

@interface Document2 : NSObject {
    NSMutableDictionary *specialized;
}

- (HTMLElement*) CreateElement:(NSString*)tag;
@end

@implementation Document2
- (HTMLElement*) CreateElement:(NSString*)tag {
    if (specialized == nil) {
       specialized = [[NSMutableDictionary alloc] init];
        
        int numClasses;
        Class *classes = NULL;
        numClasses = objc_getClassList(NULL, 0);
        if (numClasses > 0) {
            classes = malloc(sizeof(Class) * numClasses);
            numClasses = objc_getClassList(classes, numClasses);
            int i = 0;
            for (; i < numClasses; i++) {
              @try {
                Class cls = classes[i];
                if(![[cls class] isKindOfClass:[HTMLElement class]] && 
                   [cls superclass] == [HTMLElement class]) {
                    //NSLog(@"Class is %@, and super is %@.", [cls class], [cls superclass]);
                    HTMLElement *obj = [[cls alloc] init];
                    [specialized setObject:obj forKey:[obj GetTag]];
                }
              }
              @catch (NSException *e) {
                // Ignore any exceptions thrown by class initialization.
              }
            }
            free(classes);
        }

        //NSEnumerator *it = [specialized keyEnumerator];
        //for (id theKey in specialized ) {
            //[[specialized objectForKey:theKey] performSelector:@selector(Print)];
        //}
    }
    if ([specialized objectForKey:tag]) {
        return [specialized objectForKey:tag];
    }
    
    return [[HTMLElement alloc] init:tag];
}

@end

int main (int argc, const char * argv[]) {
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    Document *d1 = [[Document alloc] init];
    Document2 *d2 = [[Document2 alloc] init];
    
    id test = [d1 CreateElement:@"img"];
    NSLog(@"test : %@", [test class]);
    [test Print];
    
    id test2 = [d2 CreateElement:@"img"];
    NSLog(@"test2 : %@", [test2 class]);
    [test2 performSelector:@selector(Print)];
    
    [pool drain];
    return 0;
}

// output
2019-06-02 08:49:27.555 a.out[9474] test : HTMLImageElement
2019-06-02 08:49:27.556 a.out[9474] HTMLImageElement, tag : img
2019-06-02 08:49:27.560 a.out[9474] test2 : HTMLImageElement
2019-06-02 08:49:27.560 a.out[9474] HTMLImageElement, tag : img
```

```python

```

```cpp

```

* 리플렉션을 사용하는 또다른 예시
* `anonymous object`를 사용하는 한가지 방법은 `var` 키워드로 초기화하는 것이다.
* 위의 객체의 멤버에 액세스할 수 있는 방법은 3가지 이다. 3번째를 시도한다.
  * `anonymous type`은 사용하지 않지만, 데이터 캡슐화를 하는 클래스를 만든다.
  * 다음 절의 `Dynamic Language Runtime`을 사용한다.
  * 호출자 메소드의 파라미터를 object 유형으로 변경하고 리플렉션을 사용한다.

```cs
class Program {
   static void Main(string[] args) {
      VarTest t = new VarTest();
      t.CreateObject();
   }
}

class VarTest {
   public void CreateObject() {
      var person = new { Name = "Florian", Age = 28 };
      AnalyzeObject(person);
   }

   public void AnalyzeObject(object o) {
      Type type = o.GetType();
      PropertyInfo[] properties = type.GetProperties();

      foreach (var property in properties) {
            // 프로퍼티 이름
            string propertyName = property.Name;
            // 프로퍼티 타입의 이름
            string propertyType = property.PropertyType.Name;
            // 해당 인스턴스에서 프로퍼티의 값
            object propertyValue = property.GetValue(o);
            Console.WriteLine("{0}\t{1}\t{2}", propertyName, propertyType, propertyValue);
      }
   }
}

// output 
Name    String  Florian
Age     Int32   28
```

```java
import java.lang.reflect.Field;

public class Main {
	public static void main(String[] args) throws IllegalArgumentException, IllegalAccessException {
		Test t = new Test();
	      t.CreateObject();
	}
}

class Person {
	public String Name;
	public int Age;
	
	public Person(String name, int age) {
		Name = name;
		Age = age;
	}
}

class Test {
   public void CreateObject() throws IllegalArgumentException, IllegalAccessException {
	  Person person = new Person("Florian", 28);
      AnalyzeObject(person);
   }

   public void AnalyzeObject(Object o) throws IllegalArgumentException, IllegalAccessException {
	  Field[] fields = o.getClass().getDeclaredFields();

      for (int i = 0; i < fields.length; ++i) {
    	  String name = fields[i].getName();
    	  String type = fields[i].getType().getName();
    	  Object value = fields[i].get(o);
    	  System.out.println(name + "\t" + type + "\t" + value);
      }
   }
}

// output 
Name	java.lang.String	Florian
Age	int	28
```

```objc
#import <Foundation/Foundation.h>

@interface Person : NSObject {
    NSString* Name;
    int Age;
}
@property (nonatomic, copy) NSString* Name;
@property (nonatomic, assign) int Age;

- (id) init:(NSString*)name :(int)age;
@end
    
@implementation Person
@synthesize Name;
@synthesize Age;
    
- (id)init:(NSString*)name :(int)age{
   self = [super init];
   self.Name = name;
   self.Age = age;
   return self;
}

@end

@interface Test : NSObject

- (void) CreateObject;
- (void) AnalyzeObject:(id)o;
@end
    
@implementation Test
    
- (void)CreateObject {
    Person *p = [[Person alloc] init:@"Florian" :28];
    [self AnalyzeObject:p];
}
- (void)AnalyzeObject:(id)o {
    unsigned int count; 
    objc_property_t *properties = class_copyPropertyList([o class], &count);
    while (count--) { 
        objc_property_t property = properties[count]; 
        NSString *name = [NSString stringWithFormat:@"%s", property_getName(property)];
        NSLog(@"%@\t%@\t%@\t", name, [property class],[o valueForKey:name]);
    } 
    free(properties);
}

@end


int main (int argc, const char * argv[]) {
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    Test *t = [[Test alloc] init];
    [t CreateObject];
    
    [pool drain];
    return 0;
}
```

```python

```

```cpp

```

* `static property`는 아래와 같이 인스턴스로 null을 전달하면 값을 가져올 수 있다.

```cs
var propertyInfo = typeof(MyClass).GetProperty("Instances");
var value = propertyInfo.GetValue(null);
```

* 메소드 정보를 얻는 것은 프로퍼티 정보를 얻는것과 비슷하다.
* [PropertyInfo](https://docs.microsoft.com/ko-kr/dotnet/api/system.reflection.propertyinfo?view=netframework-4.8), [MethodInfo](https://docs.microsoft.com/ko-kr/dotnet/api/system.reflection.methodinfo?view=netframework-4.8), [ConstructorInfo](https://docs.microsoft.com/ko-kr/dotnet/api/system.reflection.constructorinfo?view=netframework-4.8)는 모두 `MemberInfo`를 상속한다.
* `MethodInfo`, `ConstructorInfo`는 `MethodBase`를 상속한다. `MethodBase`는 `MemberInfo`를 상속한다.

```cs
class Program {
   static void Main(string[] args) {
      VarTest t = new VarTest();
      t.CreateObject();
   }
}

class VarTest {
   public void CreateObject() {
      var person = new { Name = "Florian", Age = 28 };
      AnalyzeObject(person);
   }

   public void AnalyzeObject(object o) {
      Type type = o.GetType();
      MethodInfo[] methods = type.GetMethods();

      //Iterate over all methods
      foreach (var method in methods) {
            string methodName = method.Name;
            string methodReturnType = method.ReturnType.Name;
            Console.WriteLine("{0}\t{1}", methodName, methodReturnType);
      }
   }
}

// output
get_Name        String
get_Age Int32
Equals  Boolean
GetHashCode     Int32
ToString        String
GetType Type
```

```java
import java.lang.reflect.Method;

public class Main {
	public static void main(String[] args) throws IllegalArgumentException, IllegalAccessException {
		Test t = new Test();
	      t.CreateObject();
	}
}

class Person {
	public String Name;
	public int Age;
	
	public Person(String name, int age) {
		Name = name;
		Age = age;
	}
}

class Test {
   public void CreateObject() throws IllegalArgumentException, IllegalAccessException {
	  Person person = new Person("Florian", 28);
      AnalyzeObject(person);
   }

   public void AnalyzeObject(Object o) throws IllegalArgumentException, IllegalAccessException {
	   Method[] methods = o.getClass().getMethods();
	   Field[] fields = o.getClass().getDeclaredFields();

      for (int i = 0; i < methods.length; ++i) {
    	  String name = methods[i].getName();
    	  System.out.print("Parameter : ");
    	  for (int j = 0; j < methods[i].getParameterTypes().length; ++j)
        	  System.out.print(methods[i].getParameterTypes()[j].getName() + "\t");
    	  System.out.println("");
    	  String type = methods[i].getReturnType().getName();

    	  System.out.println("Info : " + name + "\t" + type);
      }
   }
}

// output
Parameter : 
Info : wait	void
Parameter : long	int	
Info : wait	void
Parameter : long	
Info : wait	void
Parameter : java.lang.Object	
Info : equals	boolean
Parameter : 
Info : toString	java.lang.String
Parameter : 
Info : hashCode	int
Parameter : 
Info : getClass	java.lang.Class
Parameter : 
Info : notify	void
Parameter : 
Info : notifyAll	void
```

```objc
#import <Foundation/Foundation.h>

@interface Person : NSObject {
    NSString* Name;
    int Age;
}
@property (nonatomic, copy) NSString* Name;
@property (nonatomic, assign) int Age;

- (id) init:(NSString*)name :(int)age;
@end
    
@implementation Person
@synthesize Name;
@synthesize Age;
    
- (id)init:(NSString*)name :(int)age{
   self = [super init];
   self.Name = name;
   self.Age = age;
   return self;
}

@end

@interface Test : NSObject

- (void) CreateObject;
- (void) AnalyzeObject:(id)o;
@end
    
@implementation Test
    
- (void)CreateObject {
    Person *p = [[Person alloc] init:@"Florian" :28];
    [self AnalyzeObject:p];
}
- (void)AnalyzeObject:(id)o {
    unsigned int count; 
    Method *methods = class_copyMethodList([o class], &count);

    int i = 0;
    for (; i < count; i++) {
        Method method = methods[i];
        NSString *name = [NSString stringWithFormat:@"%s", sel_getName(method_getName(method))];
        NSString *parameter = [NSString stringWithFormat:@"%s", method_getTypeEncoding(method)];
        NSLog(@"%@ %@", name, parameter);
    }
    free(methods);
}

@end


int main (int argc, const char * argv[]) {
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    Test *t = [[Test alloc] init];
    [t CreateObject];
    
    [pool drain];
    return 0;
}

// output
2019-06-02 10:04:17.451 a.out[15676] setName: v24@0:8@16
2019-06-02 10:04:17.451 a.out[15676] Name @16@0:8
2019-06-02 10:04:17.451 a.out[15676] setAge: v20@0:8i16
2019-06-02 10:04:17.451 a.out[15676] Age i16@0:8
2019-06-02 10:04:17.452 a.out[15676] init:: @28@0:8@16i24
```

```python

```

```cpp

```

* 컴파일러는 프로퍼티를 메소드로 변환한다. 모든 프로퍼티는 메소드이다. (get_Name, get_Age)
* `GetProperty`, `GetProperties` 메소드는 모든 메소드를 찾지 않고 간단하게 특정 메소드에 접근할 수 있는 방법이다.

----
## Dynamic Types

* `anonymous object` 의 멤버에 액세스할 수 있는 방법 중 하나
* `dynamic` 타입을 사용한다.
* 컴파일러에게 오류를 무시하고 런타임에 맵핑하도록 지시한다.

```cs
class Program {
   static void Main(string[] args) {
      Test t = new Test();
      t.CreateObject();
   }
}

class Test {
   public void CreateObject() {
      var person = new { Name = "Florian", Age = 28 };
      UseObject(person);
   }
   public void UseObject(dynamic o) {
      Console.Write("The name is . . . ");
      Console.WriteLine(o.Name);
   }
}

// output
The name is . . . Florian
```

```java
x
```

```objc
x
```

```python
class Anonymous:
    def __init__(self, **entries): self.__dict__.update(entries)

class Test:
	def CreateObject(self):
		person = Anonymous(Name = "Florian", Age = 28)
		self.UseObject(person)

	def UseObject(self, o):
		print("The name is ... %s" % (o.Name))

t = Test()
t.CreateObject()
```

```cpp
x
```

* 모든 표준 [CLR](https://docs.microsoft.com/ko-kr/dotnet/standard/clr) 객체를 `dynamic type`으로 처리할 수 있다.
* 아래의 예시에서 나오는 네 변수는 다 다르다.

```cs
int a = 1;        // type Int32
var b = 1;        // type Int32 (inferred)
dynamic c = 1;    // only known at runtime (but will be Int32)
object d = 1;     // type object, but the actual type is Int32

var a2 = a + 2;   // works, Int32 + Int32 = Int32
var b2 = b + 2;   // works, Int32 + Int32 = Int32
var c2 = c + 2;   // works, dynamic + Int32 = dynamic
var d2 = d + 2;   // 오류 CS0019  '+' 연산자는 'object' 및 'int' 형식의 피연산자에 적용할 수 없습니다.

a = "hi";         // compilation errors
b = "hi";         // compilation errors
c = "hi";         // Works!
d = "hi";         // Works!
```

* `dynamic type`은 컴파일 단계에서 에러가 나지 않기 때문에 사용에 주의가 필요하다.

```cs
dynamic a = "32";
var b = a * 5;

// output
처리되지 않은 예외: Microsoft.CSharp.RuntimeBinder.RuntimeBinderException: '*' 연산자는 'string' 및 'int' 형식의 피연산 자에 적용할 수 없습니다.
```

* `dynamic type`은 매핑 기능에서 정적 타이핑보다 더 정확하게 동작한다.

```cs
var a = (object)2;//This will be inferred to be Object
dynamic b = (object)2;//This is dynamic and the actual type is Int32
Take(a); //Received an object
Take(b); //Received an integer

void Take(object o) {
   Console.WriteLine("Received an object");
}

void Take(int i) {
   Console.WriteLine("Received an integer");
}

// output
Received an object
Received an integer
```

* [Dynamic Language Runtine](https://docs.microsoft.com/ko-kr/dotnet/framework/reflection-and-codedom/dynamic-language-runtime-overview) : 동적 객체를 바인딩 하는 계층
* `DLR`은 특별한 종류의 인터페이스를 구현하여 유형을 정의한다.
* [ExpandoObject](https://docs.microsoft.com/ko-kr/dotnet/api/system.dynamic.expandoobject?view=netframework-4.8), [DynamicObject](https://docs.microsoft.com/ko-kr/dotnet/api/system.dynamic.dynamicobject?view=netframework-4.8)는 해당 인터페이스를 구현한 클래스이다.
  * `ExpandoObject`는 런타임에 멤버를 추가 및 제거하지만 특정 작업을 정의할 필요가 없고, 정적 멤버가 없을 때 사용.
  * 상효운용성 프로토콜에 참여하는 방법을 정의하거나, 동적 디스패치 캐싱을 관리하는 것과 같은 고급 시나리오가 있는 경우 `IDynamicMetaObjectProvider` 인터페이스를 구현하여 사용.
* 객체를 매우 쉽게 확장할 수 있다.

```cs
using System.Dynamic;

class Program {
   static void Main(string[] args) {
      dynamic person = new Person();
      person.Name = "Florian";
      person.Age = 28;
      Console.WriteLine(person);
      person.Country = "Germany";
      Console.WriteLine(person);
   }
}

class Person : DynamicObject {
   //This will be responsible for storing the properties
   Dictionary<string, object> properties = new Dictionary<string, object>();

   public override bool TryGetMember(GetMemberBinder binder, out object result) {
      //This will get the corresponding value from the properties
      return properties.TryGetValue(binder.Name, out result);
   }

   public override bool TrySetMember(SetMemberBinder binder, object value) {
      //binder.Name contains the name of the variable
      properties[binder.Name] = value;
      return true;
   }

   public Dictionary<string, object> GetProperties() {
      return properties;
   }

   public override string ToString() {
      //Our object also has a specialized string output
      StringBuilder sb = new StringBuilder();
      sb.AppendLine("--- Person attributes ---");

      foreach (var key in properties.Keys) {
            //We use the chaining property of the StringBuilder methods
            sb.Append(key).Append(": ").AppendLine(properties[key].ToString());
      }

      return sb.ToString();
   }
}

// output
--- Person attributes ---
Name: Florian
Age: 28

--- Person attributes ---
Name: Florian
Age: 28
Country: Germany
```

```java
import java.util.HashMap;
import java.util.Map;

public class Main {
	public static void main(String[] args) throws IllegalArgumentException, IllegalAccessException {
		Person person = new Person();
	    person.TrySetMember("Name", "Florian");
	    person.TrySetMember("Age", 28);
  	  	System.out.println(person);
	    person.TrySetMember("Country", "Germany");
  	  	System.out.println(person);
	}
}

class Person {
	Map<String, Object> properties = new HashMap<String, Object>();
	
	public Object TryGetMember(String key) {
		return properties.get(key);
	}
	
	public void TrySetMember(String key, Object value) {
		properties.put(key, value);
	}
	
	public Map<String, Object> GetProperties() {
		return properties;
	}
	
	@Override
	public String toString() {
		StringBuilder sb = new StringBuilder();
		sb.append("--- Person attributes ---\n");
		
		for(String key : properties.keySet()) {
			sb.append(key).append(": ").append(properties.get(key).toString() + "\n");
		}
		
		return sb.toString();
	}
}

// output
--- Person attributes ---
Age: 28
Name: Florian

--- Person attributes ---
Country: Germany
Age: 28
Name: Florian
```

```objc
// objc_property_attribute_t + class_addProperty로 기존 로컬 변수에 프로퍼티를 추가하는 방법은 있음.
#import <Foundation/Foundation.h>

@interface Person : NSObject {
    NSMutableDictionary *properties;
}
    
- (NSObject *) TryGetMember:(NSString *)key;
- (void) TrySetMember:(NSString *)key :(NSObject *)value;
- (NSMutableDictionary *) GetProperties;

- (NSString *) description;
@end
    
@implementation Person
    
- (id)init {
   self = [super init];
   properties = [[NSMutableDictionary alloc] init];
   return self;
}

- (NSObject *) TryGetMember:(NSString *)key {
    return [properties objectForKey:key];
}
- (void) TrySetMember:(NSString *)key :(NSObject *)value {
    [properties setObject:value forKey:key];
}
- (NSMutableDictionary *) GetProperties {
    return properties;
}

- (NSString *) description {
    NSMutableString *body;
    body = [NSMutableString string];
    //body = [NSMutableString stringWithCapacity:100];
    [body appendString:@"--- Person attributes ---\n"];
    
    for (id key in properties ) {
        [body appendFormat:@"%@: %@\n", key, [properties objectForKey:key]];
    }
    return body;
}

@end


int main (int argc, const char * argv[]) {
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    Person *p = [[Person alloc] init];
    [p TrySetMember:@"Name" :@"Florian"];
    [p TrySetMember:@"Age" :@"28"];
    NSLog(@"%@", p);
    [p TrySetMember:@"Country" :@"Germany"];
    NSLog(@"%@", p);
    
    [pool drain];
    return 0;
}

// output
2019-06-02 15:28:30.873 a.out[16828] --- Person attributes ---
Age: 28
Name: Florian

2019-06-02 15:28:30.874 a.out[16828] --- Person attributes ---
Country: Germany
Age: 28
Name: Florian
```

```python

```

```cpp

```

----
## Accessing the file system

* 디렉토리, 파일, 드라이브의 정보를 가져올 수 있다.
* `DirectoryInfo`, `FileInfo`, `DriveInfo`

```cs
using System.IO;

class Program {
   static void Main(string[] args) {
      Test t = new Test();
      t.Print();
   }
}

class Test {
   public void Print() {
      string path = @"C:\Windows";
      string[] files = Directory.GetFiles(path);
      DriveInfo[] drives = DriveInfo.GetDrives();
      
      foreach (var file in files) {
            FileInfo fi = new FileInfo(file);
            Console.WriteLine(fi.FullName);
      }
      
      foreach (var drive in drives) {
            Console.WriteLine(drive.Name);
      }
   }
}

// output
C:\Windows\mib.bin
C:\Windows\notepad.exe
..
C:\
D:\
E:\
..
```

```java
import java.io.*;
import javax.swing.filechooser.*;

public class Main {
	public static void main(String[] args) {
		Test t = new Test();
		t.Print();
	}
}

class Test {
	public void Print() {
	    String path = "C:/Users";
	    File dir = new File(path);
	    File[] files = dir.listFiles();
	    for (File file : files) {
	    	System.out.println(file.getAbsolutePath());
	    }

		FileSystemView fsv = FileSystemView.getFileSystemView();
	    File[] roots = fsv.getRoots();
//	    for (int i = 0; i < roots.length; i++) {
//	        System.out.println("Root: " + roots[i]);
//	    }
//	    System.out.println("Home directory: " + fsv.getHomeDirectory());
	    File[] f = File.listRoots();
	    for (int i = 0; i < f.length; i++) {
	        System.out.println("Drive: " + f[i]);
	        //System.out.println("Display name: " + fsv.getSystemDisplayName(f[i]));
	    }
	}	
}

// output
C:\Users\All Users
C:\Users\Default
C:\Users\Default User
C:\Users\Default.migrated
C:\Users\desktop.ini
C:\Users\Kanghyuk
C:\Users\Public
Drive: C:\
Drive: D:\
Drive: E:\
Drive: F:\
Drive: O:\
```

```objc
#import <Foundation/Foundation.h>

@interface Test : NSObject 
- (void) Print;
@end
    
@implementation Test
    
- (void)Print {
    NSString *path;
    NSFileManager *fm;
    NSDirectoryEnumerator *dirEnum;
    NSArray *dirArray;

    fm = [NSFileManager defaultManager];
    path = [fm currentDirectoryPath];
    
    // cur directory + recursive
    dirEnum= [fm enumeratorAtPath:path];
    NSLog(@"Contents of %@", path);
    while ((path = [dirEnum nextObject]) != nil) {
        NSLog(@"%@",path);
    }
    
    // cur directory
    dirArray = [fm contentsOfDirectoryAtPath:[fm currentDirectoryPath] error:NULL];
    NSLog(@"Contents using contentsOfDirectoryAtPath:error:");
    for (path in dirArray) {
        NSLog(@"%@",path);
    }
    
    NSString *tmpDir;
    tmpDir = NSTemporaryDirectory();
    NSLog(@"Temporary Directory is %@", tmpDir);
    
    NSString *homedir = NSHomeDirectory();
    NSLog(@"Your home directory is %@", homedir);
    
    NSArray *components = [homedir pathComponents];
    for (path in components) {
        NSLog(@"%@",path);
    }

    // storage capacity
    //NSURL *fileURL = [[NSURL alloc] initFileURLWithPath:@"/"];
    //NSError *error = nil;
    //NSDictionary *results = [fileURL resourceValuesForKeys:@[NSURLVolumeAvailableCapacityForImportantUsageKey] error:&error];
    //if (!results) {
    //    NSLog(@"Error retrieving resource keys: %@\n%@", [error localizedDescription], [error userInfo]);
    //    abort();
    //}
    //NSLog(@"Available capacity for important usage: %@", results[NSURLVolumeAvailableCapacityForImportantUsageKey]);
}

@end


int main (int argc, const char * argv[]) {
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    Test *t = [[Test alloc] init];
    [t Print];
    
    [pool drain];
    return 0;
}

// output
2019-06-02 16:35:56.160 a.out[13244] Contents of /var/www/service/usercode
2019-06-02 16:35:56.161 a.out[13244] 309345069
2019-06-02 16:35:56.161 a.out[13244] 309345069/source.m
2019-06-02 16:35:56.161 a.out[13244] 309345069/a.d
2019-06-02 16:35:56.161 a.out[13244] 309345069/a.out
2019-06-02 16:35:56.161 a.out[13244] Text1.txt
2019-06-02 16:35:56.161 a.out[13244] file
2019-06-02 16:35:56.161 a.out[13244] Contents using contentsOfDirectoryAtPath:error:
2019-06-02 16:35:56.161 a.out[13244] 309345069
2019-06-02 16:35:56.161 a.out[13244] Text1.txt
2019-06-02 16:35:56.161 a.out[13244] file
2019-06-02 16:35:56.161 a.out[13244] Temporary Directory is /tmp/GNUstepSecure33
2019-06-02 16:35:56.161 a.out[13244] Your home directory is /var/www
```

```python

```

```cpp

```


----
## Streams

* 입력 및 출력 동작은 스트림으로 관리된다.
* 메모리, 파일, 입출력 등의 스트림이 존재한다.

```cs
using System.IO;

class Program {
   static void Main(string[] args) {
      Test t = new Test();
      t.Print();
   }
}

class Test {
   public void Print() {
      FileStream fs = new FileStream("test.txt", FileMode.Create);
      byte[] arr = new byte[] { (byte)'a', (byte)'b', (byte)'c'};
      fs.Write(arr, 0, arr.Length);
      fs.Close();

      fs = new FileStream("test.txt", FileMode.Open);
      int val;
      while ((val = fs.ReadByte()) != -1) {
            Console.WriteLine("Could read some more byte! : " + val);
      }

      fs.Position = 0;
      byte[] firstTenBytes = new byte[5];
      while (fs.Read(firstTenBytes, 0, firstTenBytes.Length) != 0) {
            foreach (byte b in firstTenBytes)
               Console.WriteLine("Could read some more bytes! : " + b);
      }
      fs.Close();
   }
}

// output 
Could read some more byte! : 97
Could read some more byte! : 98
Could read some more byte! : 99
Could read some more bytes! : 97
Could read some more bytes! : 98
Could read some more bytes! : 99
Could read some more bytes! : 0
Could read some more bytes! : 0
```

```java
import java.io.*;

public class Main {
	public static void main(String[] args) {
		Test t = new Test();
		t.Print();
	}
}

class Test {
	public void Print() {
	    File file = new File("./test.txt");
	    try {
	    	FileWriter fw = new FileWriter(file);
	    	char[] arr = new char[] {'a', 'b', 'c'};
			fw.write(arr);
	    	fw.close();
	    } catch (IOException e) {
	    	e.printStackTrace();
	    }
	    
	    file = new File("./test.txt");
	    try{
	      if(file.exists() && file.isFile() && file.canRead()){
	        FileReader fr = new FileReader(file);
	        int ch;
	        while((ch = fr.read()) != -1){
	          System.out.println("Could read some more byte! : " + ch);
	        }
	      }else{
	        System.out.println("파일에 접근할 수 없습니다.");
	      }
	    } catch (FileNotFoundException ex){
	      //파일을 찾을 수 없을때
	      ex.printStackTrace();
	    } catch (IOException e) {
	      //파일 읽기 중 에러가 발생했을 때
	      e.printStackTrace();
	    }
	}	
}

// output
Could read some more byte! : 97
Could read some more byte! : 98
Could read some more byte! : 99
```

```objc
#import <Foundation/Foundation.h>

@interface Test : NSObject 
- (void) Print;
@end
    
@implementation Test
    
- (void)Print {
    NSFileManager *fm = [NSFileManager defaultManager];
    NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentationDirectory, NSUserDomainMask, YES); 
    NSString *doc = [[paths objectAtIndex:0] stringByAppendingPathComponent:@"directory"]; 
    
    NSString *path = [doc stringByAppendingPathComponent:@"test.txt"];
    [fm createFileAtPath:path contents:nil attributes:nil];

    NSError *err = NULL;
    NSString *logStr = [NSString stringWithFormat:@"abc"]; 
    if([logStr writeToFile:path atomically:false encoding:NSUTF8StringEncoding error:&err]) {
        NSLog(@"success"); 
    } else { 
        NSLog(@"fail error : %@",err);
    }
}

@end


int main (int argc, const char * argv[]) {
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    Test *t = [[Test alloc] init];
    [t Print];
    
    [pool drain];
    return 0;
}

// output
2019-06-02 17:16:36.784 a.out[26946] File NSData.m: 1436. In -[NSData writeToFile:options:error:] Open (/var/www/directory/test.txt) failed - No such file or directory
2019-06-02 17:16:36.785 a.out[26946] fail error : (null)
```

```python

```

```cpp

```

* `TextReader`, `TextWriter` 클래스는 `Stream`을 상속받지 않고 따로 구현되어 있다. - 인코딩 때문에
* `StreamReader`, `StreamWriter` 클래스는 `TextReader`, `TextWriter`를 상속받는다. 인코딩 지정이 가능하다.

```cs
using System.IO;

class Program {
   static void Main(string[] args) {
      Test t = new Test();
      t.Print();
   }
}

class Test {
   public void Print() {
      StreamWriter sw = new StreamWriter("myasciifile.txt", false, Encoding.ASCII);
      sw.WriteLine("My First ASCII Line!");
      sw.WriteLine("How does ASCII handle umlauts äöü?");
      sw.Close();
      StreamReader sr = new StreamReader("myasciifile.txt", Encoding.ASCII);
      string line;
      while ((line = sr.ReadLine()) != null) {
            Console.WriteLine("read line : " + line);
      }
      sr.Close();
      
   }
}

// output
read line : My First ASCII Line!
read line : How does ASCII handle umlauts ????
```

----
## Threads

* UI스레드에서 많은 작업을 수행하여 앱이 응답하지 않는 현상을 방지하기 위한 모델
* 모든 응용 프로그램은 이미 스레드가 1개 있다 - 응용프로그램 / GUI 스레드
* `Thread` 클래스를 실행하려면 실행할 메소드가 1개 필요하다.
* 여러 스레드를 생성하면 오버헤드가 발생하기 때문에 `ThreadPool` 클래스를 사용해야 한다.
* Window Form에서 UI를 worker thread에서 변경하는 것은 불가능하다.
* `race condition`을 피해야 한다.

```cs
using System.Threading;

class Program {
   static void Main(string[] args) {
      Thread t = new Thread(DoALotOfWork);
      t.Start("a");
      Thread t2 = new Thread(DoALotOfWork);
      t2.Start("b");
   }

   static void DoALotOfWork(object t) {
      Console.WriteLine("A lot of work has been started!");

      int i = 0;
      while (i < 100) {
            Console.WriteLine(t.ToString() + " thread i : " + ++i);
      }

      Console.WriteLine("The thread has been stopped!");
   }
}

// output
A lot of work has been started!
A lot of work has been started!
b thread i : 1
b thread i : 2
...
```

```java
public class Main {
	public static void main(String[] args) {
		Thread t = new Thread(() ->  {
			DoALotOfWork("a");
		});
		t.start();
		Thread t2 = new Thread(() ->  {
			DoALotOfWork("b");
		});
		t2.start();
	}

    static void DoALotOfWork(Object t) {
    	System.out.println("A lot of work has been started!");

        int i = 0;
        while (i < 100) {
        	System.out.println(t.toString() + " thread i : " + ++i);
        }

        System.out.println("The thread has been stopped!");
    }
}

// output
A lot of work has been started!
A lot of work has been started!
a thread i : 1
b thread i : 1
a thread i : 2
a thread i : 3
a thread i : 4
b thread i : 2
```

```objc
#import <Foundation/Foundation.h>

@interface Test : NSObject 
- (void) Run;
- (void) DoALotOfWork:(NSObject *)t;
@end
    
@implementation Test
    
- (void)Run {
    NSThread *t1 = [[NSThread alloc] initWithTarget:self selector:@selector(DoALotOfWork:) object:@"a"];
    NSThread *t2 = [[NSThread alloc] initWithTarget:self selector:@selector(DoALotOfWork:) object:@"b"];
    
    [t1 start];
    [t2 start];
    
    [t1 release];
    [t2 release];
}
    
- (void)DoALotOfWork:(NSObject *)t {
    NSLog(@"A lot of work has been started!");
    int i = 0;
    for (;i < 100; i++) {
        NSLog(@"%@ thread i : %d", t, i);
    }

    NSLog(@"The thread has been stopped!");
}

@end


int main (int argc, const char * argv[]) {
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    Test *t = [[Test alloc] init];
    [t Run];
    
    [NSThread sleepForTimeInterval:3];
    [pool drain];
    return 0;
}
// output
2019-06-02 20:07:54.907 a.out[7141] A lot of work has been started!
2019-06-02 20:07:54.900 a.out[7141] WARNING - unable to create shared user defaults!
2019-06-02 20:07:54.908 a.out[7141] A lot of work has been started!
2019-06-02 20:07:54.908 a.out[7141] b thread i : 0
2019-06-02 20:07:54.908 a.out[7141] b thread i : 1
...
2019-06-02 20:07:54.908 a.out[7141] b thread i : 57
2019-06-02 20:07:54.908 a.out[7141] a thread i : 0
...
```

```python

```

```cpp

```

----
## Thread-communication

* 스레드를 동기화하기 위해 `lock` 키워드를 사용한다.
* 키워드의 포인터는 임의의 `Object`와 같은 참조 유형으로 사용이 가능하다.

```cs
using System.Threading;

class Program {
   static Object myLock = new Object();
   static void Main(string[] args) {
      Thread t1 = new Thread(FirstWorker);
      Thread t2 = new Thread(SecondWorker);

      t1.Start();
      t2.Start();
   }

   static void FirstWorker() {
      Console.WriteLine("First worker started!");
      
      lock (myLock) {
            Console.WriteLine("First worker entered the critical block!");
            Thread.Sleep(1000);
            Console.WriteLine("First worker left the critical block!");
      }
      
      Console.WriteLine("First worker completed!");
   }

   static void SecondWorker() {
      Console.WriteLine("Second worker started!");

      lock (myLock) {
            Console.WriteLine("Second worker entered the critical block!");
            Thread.Sleep(5000);
            Console.WriteLine("Second worker left the critical block!");
      }
      
      Console.WriteLine("Second worker completed!");
   }
}

// output
First worker started!
First worker entered the critical block!
Second worker started!
First worker left the critical block!
Second worker entered the critical block!
First worker completed!
Second worker left the critical block!
Second worker completed!
```

```java
public class Main {
	public static void main(String[] args) {
		Test t = new Test();
		Thread t1 = new Thread(() ->  {
			t.FirstWorker();
		});
		t1.start();
		Thread t2 = new Thread(() ->  {
			t.SecondWorker();
		});
		t2.start();
	}
}

class Test {
    public void FirstWorker() {
    	System.out.println("First worker started!");
    	synchronized(this) {
        	System.out.println("First worker entered the critical block!");
			try {
				Thread.sleep(1000);
			} catch (Exception e) {
				e.printStackTrace();
			}
            System.out.println("First worker left the critical block!");
    	}
        System.out.println("First worker completed!");
    }

    public void SecondWorker() {
    	System.out.println("Second worker started!");
		synchronized(this) {
			System.out.println("Second worker entered the critical block!");
			try {
				Thread.sleep(1000);
			} catch (Exception e) {
				e.printStackTrace();
			}
		    System.out.println("Second worker left the critical block!");
		}
        System.out.println("Second worker completed!");
    }
}
// output
First worker started!
First worker entered the critical block!
Second worker started!
First worker left the critical block!
First worker completed!
Second worker entered the critical block!
Second worker left the critical block!
Second worker completed!
```

```objc
#import <Foundation/Foundation.h>

@interface Test : NSObject 
- (void) Run;
- (void) FirstWorker;
- (void) SecondWorker;
@end
    
@implementation Test
    
- (void)Run {
    NSThread *t1 = [[NSThread alloc] initWithTarget:self selector:@selector(FirstWorker) object:nil];
    NSThread *t2 = [[NSThread alloc] initWithTarget:self selector:@selector(SecondWorker) object:nil];
    
    [t1 start];
    [t2 start];
    
    [t1 release];
    [t2 release];
}
    
- (void)FirstWorker {
    NSLog(@"First worker started!");
    @synchronized(self) {
        NSLog(@"First worker entered the critical block!");
        [NSThread sleepForTimeInterval:1];
        NSLog(@"First worker left the critical block!");
    }
    NSLog(@"First worker completed!");
}
    
- (void)SecondWorker {
    NSLog(@"Second worker started!");
    @synchronized(self) {
        NSLog(@"Second worker entered the critical block!");
        [NSThread sleepForTimeInterval:1];
        NSLog(@"Second worker left the critical block!");
    }
    NSLog(@"Second worker completed!");
}

@end


int main (int argc, const char * argv[]) {
    NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
    Test *t = [[Test alloc] init];
    [t Run];
    [NSThread sleepForTimeInterval:3];
    [pool drain];
    return 0;
}
// output
2019-06-02 20:00:46.033 a.out[9635] Second worker started!
2019-06-02 20:00:46.026 a.out[9635] WARNING - unable to create shared user defaults!
2019-06-02 20:00:46.033 a.out[9635] First worker started!
2019-06-02 20:00:46.033 a.out[9635] First worker entered the critical block!
2019-06-02 20:00:47.033 a.out[9635] First worker left the critical block!
2019-06-02 20:00:47.033 a.out[9635] First worker completed!
2019-06-02 20:00:47.033 a.out[9635] Second worker entered the critical block!
2019-06-02 20:00:48.033 a.out[9635] Second worker left the critical block!
2019-06-02 20:00:48.033 a.out[9635] Second worker completed!
```

```python

```

```cpp

```

----
## The Task Parallel Library

* .NET Framework 4.0 부터 [Task Parallel Library](https://docs.microsoft.com/ko-kr/dotnet/standard/parallel-programming/task-parallel-library-tpl) 가 도입되었다.
* `TPL`은 `System.Threading` 및 `System.Threading.Tasks` 네임스페이스에 포함된 공용 형식 및 API의 집합이다.
  * `PLINQ` 형식의 LINQ에 대한 작업 및 확장을 위한 클래스와 메소드의 집합
* `TPL`은 단기간에 여러 스레드를 생성, 결합하는데 특화된 스레드풀을 처리한다.
* 작업의 양이 많지 않거나 실행되는 반복횟수가 적으면 병렬화에 따른 오버헤드로 코드 실행이 오히려 느려질 수 있다.
* `Parallel.For()`를 사용하면 루프를 여러 스레드가 계산하도록 작업을 분리한다.

```cs
using System.Threading.Tasks;

class Program {
   static void Main(string[] args) {
      Test t = new Test();
      Console.WriteLine(t.Print());
   }
}

class Test {
   public double Print() {
      // No Parallel
      int N = 10000000;
      double sum = 0.0;
      double step = 1.0 / N;

      for (var i = 0; i < N; i++) {
         double x = (i + 0.5) * step;
         sum += 4.0 / (1.0 + x * x);
      }

      return sum * step;

      // Parallel Loop
      object _ = new object();
      int N = 10000000;
      double sum = 0.0;
      double step = 1.0 / N;

      Parallel.For(0, N, i => {
         double x = (i + 0.5) * step;
         double y = 4.0 / (1.0 + x * x);
         lock (_) {
            sum += y;
         }
      });

      return sum * step;
   }
}

// output
3.14159265358945
```

```java
import java.util.stream.IntStream;

public class Main {
	public static void main(String[] args) {
		Test t = new Test();
		System.out.println(t.Print());
	}
}

class Test {
    public double sum;
    public double Print() {

        // No Parallel
      //  int N = 10000000;
      //  double sum = 0.0;
      //  double step = 1.0 / N;

      //  for (int i = 0; i < N; i++) {
      //     double x = (i + 0.5) * step;
      //     sum += 4.0 / (1.0 + x * x);
      //  }

      //  return sum * step;

        // Parallel Loop
        int N = 10000;
        sum = 0.0;
        double step = 1.0 / N;
        
        IntStream.range(0, N).parallel().forEach(i -> {
            double x = (i + 0.5) * step;
            double y = 4.0 / (1.0 + x * x);
            sum += y;
        });

        return sum * step;
    }
}
// output
3.1415926544231327
```

```objc

```

```python

```

```cpp

```

* 위의 코드보다 효율적인 코드는 `Parallel.For<TLocal>()` 함수를 사용하는 것이다.
* `TLocal`은 return값의 타입이다.
* 세번째 파라미터는 각 스레드에서 사용할 로컬 변수를 리턴하는 액션이다. 초기화는 아님.
* `state` 변수는 `ParallelLoopState` 클래스이며 루프 실행을 중단, 중지하는 등의 작업에 액세스할 수 있게 한다.
* `local` 변수는 현재 스레드의 로컬 변수값이다.

```cs
using System.Threading.Tasks;

class Program {
   static void Main(string[] args) {
      Test t = new Test();
      Console.WriteLine(t.Print());
   }
}

class Test {
   public double Print() {
      // Parallel Loop + Local
      object _ = new object();
      int N = 10000000;
      double sum = 0.0;
      double step = 1.0 / N;

      Parallel.For<double>(0, N, () => 0.0, (i, state, local) => {
         double x = (i + 0.5) * step;
         return local + 4.0 / (1.0 + x * x);
      }, local => {
         lock (_) {
            sum += local;
            Console.WriteLine("sum ? " + sum);
         }
      });

      return sum * step;
   }
}

// output
sum ? 8513904.30711955
sum ? 15487647.58378
sum ? 25774959.3024131
sum ? 31415926.535897
3.1415926535897
```

----
## Tasks and threads

* `Task`와 `Thread`는 차이가 있다.
* `Thread`는 OS에서 나온 일종의 자원이다.
* `Task`가 `Thread`의 특수화라고 할 수 있지만, 실행중인 `Task`인스턴스가 모두 `Thread`를 기반으로 하는 것은 아니다.
* `Task<T>`를 반환하는 모든 IO 바인딩 비동기 메소드는 스레드를 사용하지 않고 콜백을 기반으로 한다.
  * IO 바인딩은 스레드를 사용하는 것보다 콜백을 사용하는 것이 훨씬 적합하다. (`asnyc`)
* `Task` 를 사용하면 스레드 생성과 콜백 사용 두 가지 방법을 하나의 코드로 구현할 수 있다.

```cs
using System.Threading.Tasks;

class Program {
   static Object myLock = new Object();
   static void Main(string[] args) {
      Test t = new Test();
      t.Print();
   }

}

class Test {
   public void Print() {
      var tasks = new List<Task<long>>();
      for (int ctr = 1; ctr <= 10; ctr++) {
         tasks.Add(SimulationAsync(ctr));
      }
      var continuation = Task.WhenAll(tasks);
      try {
         //continuation.Wait();
      } catch (AggregateException) { }

      if (continuation.Status == TaskStatus.RanToCompletion) {
         long grandTotal = 0;
         foreach (var result in continuation.Result) {
            grandTotal += result;
            Console.WriteLine("Mean: {0:N2}, n = 1,000", result / 1000.0);
         }

         Console.WriteLine("\nMean of Means: {0:N2}, n = 10,000", grandTotal / 10000);
      } else {
         foreach (var t in tasks) {
            Console.WriteLine("Task {0}: {1}", t.Id, t.Status);
         }
      }
   }

   Task<long> SimulationAsync(int seed) {
      var task = new Task<long>(() => {
         long total = 0;
         var rnd = new Random(seed);
         // Generate 1,000 random numbers.
         for (int n = 1; n <= 1000; n++)
            total += rnd.Next(0, 1000);
         return total;
      });
      
      task.Start();
      return task;
   }
}

// output
Mean: 503.09, n = 1,000
Mean: 519.38, n = 1,000
Mean: 494.67, n = 1,000
Mean: 495.93, n = 1,000
Mean: 495.24, n = 1,000
Mean: 508.53, n = 1,000
Mean: 477.83, n = 1,000
Mean: 495.10, n = 1,000
Mean: 505.39, n = 1,000
Mean: 491.69, n = 1,000

Mean of Means: 498.00, n = 10,000
```

```java
import java.util.concurrent.*;

public class Main {
	public static void main(String[] args) {
		Test t = new Test();
		t.Print();
	}
}

class Test {
    public void Print() {
        ExecutorService execService = Executors.newFixedThreadPool(5); 
        Callable<Long> task = () -> {
            long total = 0;
			// Generate 1,000 random numbers.
			for (int n = 1; n <= 1000; n++)
			   total += ThreadLocalRandom.current().nextLong(0,1000);
			return total;
        };
        
        Future<Long>[] arr_future = new Future[10];
        for (int i = 0; i < 10; i++) {
        	arr_future[i] = execService.submit(task);
            
            try {
                Thread.sleep(10);
            } catch (InterruptedException e) {
                e.printStackTrace();
            } 
        }
        long grandTotal = 0;
        for (int i = 0; i < 10; i++) {
        	try {
        		long result = arr_future[i].get();
				grandTotal += result;
	            System.out.printf("Mean: %f, n = 1,000\n", result / 1000.0);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (ExecutionException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
        }
        
        System.out.printf("\nMean of Means: %f, n = 10,000", grandTotal / 10000.0);

        execService.shutdown();
    }
}
// output
Mean: 506.384000, n = 1,000
Mean: 507.424000, n = 1,000
Mean: 509.990000, n = 1,000
Mean: 512.003000, n = 1,000
Mean: 505.992000, n = 1,000
Mean: 505.738000, n = 1,000
Mean: 495.995000, n = 1,000
Mean: 506.455000, n = 1,000
Mean: 492.205000, n = 1,000
Mean: 502.770000, n = 1,000

Mean of Means: 504.495600, n = 10,000
```

```objc

```

```python

```

```cpp

```

----
## [Awaiting async methods](https://msdn.microsoft.com/en-us/magazine/jj991977.aspx)

* C# 5부터 `await`, `async` 키워드 도입
* 메소드를 비동기식으로 표시한다.
* 반환값이 없는 경우 `Task`를, 반환값이 있는경우 `Task<T>`를 반환값으로 사용한다.
  * `Task`를 반환하지 않는것은 `anti-pattern`이다. async void 함수는 이벤트 핸들러와 함께 사용해야 하지만 쓰지 않는게 좋다.
  * void 반환 비동기 메소드는 `Task` 객체가 없기 때문에 exception을 자연스럽게 잡을 수 없다. (SynchronizationContext 에서 직접 발생)

```cs
private async void ThrowExceptionAsync() {
  throw new InvalidOperationException();
}
public void AsyncVoidExceptions_CannotBeCaughtByCatch() {
   try {
      ThrowExceptionAsync();
   } catch (Exception) {
      // The exception is never caught here!
      Console.WriteLine(e.ToString());
      throw;
   }
}

// 아래와 같이 수정
private async Task AsyncTask() {
   throw new InvalidOperationException();
}

public async Task CallAsyncTask() {
   try {
      await AsyncTask();
   } catch (Exception e) {
      Console.WriteLine(e.ToString());
      throw;
   }
}
```

* `ASP.NET context`에서 아래와 같은 코드는 `deadlock`이 발생한다.
* 콘솔 응용 프로그램에서는 발생하지 않는다. `thread pool SynchronizationContext` 대신 `one-chunk-at-a-time SynchronizationContext`을 가지고 있기 때문에.
* 해결방법은 `ConfigureAwait(false)` 를 await 구문에 사용하는 것.

```cs
public static class DeadlockDemo {
  private static async Task DelayAsync() {
      //await Task.Delay(1000);
      await Task.Delay(1000).ConfigureAwait(false);
  }
  // This method causes a deadlock when called in a GUI or ASP.NET context.
  public static void Test() {
      // Start the delay.
      var delayTask = DelayAsync();
      // Wait for the delay to complete.
      delayTask.Wait();
  }
}
```

![](async.png)

```cs
using System.Threading.Tasks;
using System.Net.Http;

class Program {
   static void Main(string[] args) {
      Test t = new Test();
      t.Print();
   }
}

class Test {
   public void Print() {
      Console.WriteLine("task start");
      Task<int> task = AccessTheWebAsync();
      //task.Start();
      Console.WriteLine("result : " + task.Result);
   }

   async Task<int> AccessTheWebAsync() {
      using (HttpClient client = new HttpClient()) {
            Task<string> getStringTask = client.GetStringAsync("https://docs.microsoft.com");

            // GetStringAsync로부터 얻는 string 값과 관계 없는 작업을 여기서 진행한다. 
            Console.WriteLine("DoIndependentWork");

            string urlContents = await getStringTask;

            return urlContents.Length;
      }
   }
}
```

```java
x
```

```objc
x
```

```python
# pip install aiohttp
import asyncio
import aiohttp

async def Print():
	print("task start")
	html = await AccessTheWebAsync()
	print("result : " + str(len(html)))

async def AccessTheWebAsync():
	async with aiohttp.ClientSession() as session:
		async with session.get("https://docs.microsoft.com") as response:
			return await response.text()

loop = asyncio.get_event_loop()
loop.run_until_complete(Print())
```
```cpp
x
```