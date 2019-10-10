# Observer, State

## Observer

* 객체의 상태 변화를 관찰하는 옵저버들의 목록을 객체에 등록하여 상태 변화가 있을때마다 옵저버들에게 통지
![](observer.jpg)

```java
public abstract class Channel {
	protected String m_title;
	private List<Subscriber> m_subscribers = new ArrayList<Subscriber>();
	
	public void Attach(Subscriber subscriber) {
		m_subscribers.add(subscriber);
	}
	
	public void Detach(Subscriber subscriber) {
		m_subscribers.remove(subscriber);
	}
	
	public void Notify() {
		for(Subscriber subscriber : m_subscribers){
			subscriber.Update(this);
		}
	}
	
	public String GetTitle() {
		return m_title;
	}
}

public class KpopChannel extends Channel {
	public void Upload(String title) {
		m_title = title;
		Notify();
	}
}

public interface Subscriber {
	void Update(Channel channel);
}

public class Bob implements Subscriber {
	@Override
	public void Update(Channel channel) {
		System.out.println("Bob! " + channel.GetTitle() + " watch now!");
	}
}

public class John implements Subscriber {
	@Override
	public void Update(Channel channel) {
		System.out.println("John! " + channel.GetTitle() + " watch now!");
	}
}

public static void main(String[] args) {
    KpopChannel kpop = new KpopChannel();
    Subscriber bob = new Bob();

    kpop.Attach(bob);
    kpop.Attach(new John());
    
    kpop.Upload("new song");
    
    kpop.Detach(bob);
    
    kpop.Upload("another song");
}

//결과
Bob! new song watch now!
John! new song watch now!
John! another song watch now!
```
### C#
```cs
public abstract class Channel {
    protected string m_title;
    private List<Subscriber> m_subscribers = new List<Subscriber>();

    public void Attach(Subscriber subscriber) {
        m_subscribers.Add(subscriber);
    }

    public void Detach(Subscriber subscriber) {
        m_subscribers.Remove(subscriber);
    }

    public void Notify() {
        foreach(Subscriber subscriber in m_subscribers) {
            subscriber.Update(this);
        }
    }

    public string GetTitle() {
        return m_title;
    }
}

public class KpopChannel : Channel {
    public void Upload(string title) {
        m_title = title;
        Notify();
    }
}

public abstract class Subscriber {
    public abstract void Update(Channel channel);
}

public class Bob : Subscriber {
	public override void Update(Channel channel) {
        Console.WriteLine("Bob! " + channel.GetTitle() + " watch now!");
    }
}

public class John : Subscriber {
    public override void Update(Channel channel) {
            Console.WriteLine("John! " + channel.GetTitle() + " watch now!");
    }
}

static void Main(string[] args) {
    KpopChannel kpop = new KpopChannel();
    Subscriber bob = new Bob();

    kpop.Attach(bob);
    kpop.Attach(new John());

    kpop.Upload("new song");

    kpop.Detach(bob);

    kpop.Upload("another song");
}

//결과
Bob! new song watch now!
John! new song watch now!
John! another song watch now!
```

### C++
```cpp
class Channel {
private:
	vector<Subscriber*> m_subscribers;
protected:
	string m_title;

public:
	void Attach(Subscriber* subscriber) {
		m_subscribers.push_back(subscriber);
	}

	void Detach(Subscriber* subscriber) {
		vector<Subscriber*>::iterator it = std::find(m_subscribers.begin(), m_subscribers.end(), subscriber);
		m_subscribers.erase(it);
	}

	void Notify() {
		for (int i = 0; i < m_subscribers.size(); i++) {
			m_subscribers[i]->Update(this);
		}
	}

	string GetTitle() {
		return m_title;
	}
};

class KpopChannel : public Channel {
public:
	void Upload(string title) {
		m_title = title;
		Notify();
	}
};

// Subscriber.h
class Channel;

class Subscriber {
public:
	virtual void Update(Channel* channel) = 0;
};

class Bob : public Subscriber {
public:
	virtual void Update(Channel* channel);
};

class John : public Subscriber {
public:
	virtual void Update(Channel* channel);
};

// Subscriber.cpp
void Bob::Update(Channel* channel) {
	cout << "Bob! " << channel->GetTitle() << " watch now!" << endl;
}

void John::Update(Channel* channel) {
	cout << "John! " << channel->GetTitle() << " watch now!" << endl;
}

int main() {
	KpopChannel* kpop = new KpopChannel();
	Subscriber* bob = new Bob();

	kpop->Attach(bob);
	kpop->Attach(new John());

	kpop->Upload("new song");

	kpop->Detach(bob);

	kpop->Upload("another song");

	return 0;
}

// 결과
Bob! new song watch now!
John! new song watch now!
John! another song watch now!
```

### Objective-C
```objc
@interface Subscriber : NSObject
-(void) Update:(Channel*)channel;
@end

@interface Bob : Subscriber {
@end

@interface John : Subscriber {
@end

@implementation Bob
-(void) Update:(Channel*)channel {
	NSLog(@"Bob! %@ watch now!", [channel GetTitle]);
}
@end

@implementation John
-(void) Update:(Channel*)channel {
	NSLog(@"John! %@ watch now!", [channel GetTitle]);
}
@end

@interface Channel : NSObject {
    NSMutableArray* m_subscribers;
    NSString* m_title;
}

-(void) Attach:(Subscriber*)subscriber;
-(void) Detach:(Subscriber*)subscriber;
-(void) Notify;
-(NSString*) GetTitle;
@end


@interface KpopChannel : Channel
-(void) Upload:(NSString*)title);
@end

@implementation KpopChannel
-(void) Attach:(Subscriber*)subscriber {
    [m_subscribers addObject:subscriber];
}

-(void) Detach:(Subscriber*)subscriber {
    [m_subscribers removeObject:subscriber];
}

-(void) Detach {
    for(Subscriber* subscriber in m_subscribers) {
        [subscriber Update:this];
    }
}

-(NSString*) GetTitle {
	return m_title;
}

-(void) Upload:(NSString*)title{
    m_title = title;
    [this Notify];
}
@end

int main() { 
    KpopChannel* kpop = [[KpopChannel alloc] init];
    Subscriber* bob = [[Bob alloc] init];

    [kpop Attach:bob];
    [kpop Attach:[[John alloc] init]];

    [kpop Upload:"new song"];
    [kpop Detach:bob];

    [kpop Upload:"another song"];

    return 0;
}
```

* * *
  
## State

* 객체의 내부 상태가 바뀜에 따라 객체의 행동을 바꿀 수 있음, 마치 객체의 클래스가 바뀌는 것과 같은 결과를 얻음
* 상태를 별도의 클래스로 캡슐화
* 상태가 변할 때마다 새로운 상태를 `new`하면 메모리 낭비
![](state.jpg)

### java
```java
public abstract class State {
	Computer m_computer;
	public State(Computer computer) {
		m_computer = computer;
	}
	public abstract void Change();
}

public class OnState extends State {
	public OnState(Computer computer) {
		super(computer);
	}

	@Override
	public void Change() {
		System.out.println("on -> save");
		m_computer.m_state = new SaveState(m_computer);
	}
}

public class SaveState extends State {
	public SaveState(Computer computer) {
		super(computer);
	}

	@Override
	public void Change() {
		System.out.println("save -> off");
		m_computer.m_state = new OffState(m_computer);
	}
}

public class OffState extends State {
	public OffState(Computer computer) {
		super(computer);
	}

	@Override
	public void Change() {
		System.out.println("off -> on");
		m_computer.m_state = new OnState(m_computer);
	}
}

public class Computer {
	public State m_state;
	
	public Computer() {
		m_state = new OnState(this);
	}
	
	public void PushPower() {
		m_state.Change();
	}
}

public class test {
	public static void main(String[] args) {
		Computer computer = new Computer(); // default : OnState
		
		computer.PushPower();
		computer.PushPower();
		computer.PushPower();
    }
}

//결과
on -> save
save -> off
off -> on
```
### C#
```cs
public abstract class State {
    protected Computer m_computer;
    public State(Computer computer) {
        m_computer = computer;
    }
    public abstract void Change();
}

public class OnState : State {
    public OnState(Computer computer) : base(computer) { }

    public override void Change() {
        Console.WriteLine("on -> save");
        m_computer.m_state = new SaveState(m_computer);
    }
}

public class SaveState : State {
    public SaveState(Computer computer) : base(computer) { }

    public override void Change() {
        Console.WriteLine("save -> off");
        m_computer.m_state = new OffState(m_computer);
    }
}

public class OffState : State {
    public OffState(Computer computer) : base(computer) { }

    public override void Change() {
        Console.WriteLine("off -> on");
        m_computer.m_state = new OnState(m_computer);
    }
}

public class Computer {
    public State m_state;

    public Computer() {
        m_state = new OnState(this);
    }

    public void PushPower() {
        m_state.Change();
    }
}

static void Main(string[] args) {
    Computer computer = new Computer(); // default : OnState

    computer.PushPower();
    computer.PushPower();
    computer.PushPower();
}

// 결과
Bob! new song watch now!
John! new song watch now!
John! another song watch now!
```

### C++
```cpp
// State.h
class Computer;

class State {
protected:
	Computer* m_computer;
public:
	State(Computer* computer);
	virtual void Change() = 0;
};

class OnState : public State {

public:
	OnState(Computer* computer);

	virtual void Change();
};

class SaveState : public State {

public:
	SaveState(Computer* computer);
	virtual void Change();
};

class OffState : public State {
public:
	OffState(Computer* computer);

	virtual void Change();
};

// State.cpp

State::State(Computer* computer) {
	m_computer = computer;
}

OnState::OnState(Computer* computer) : State(computer) { }

void OnState::Change() {
	cout << "on -> save" << endl;
	m_computer->m_state = new SaveState(m_computer);
}

SaveState::SaveState(Computer* computer) : State(computer) { }

void SaveState::Change() {
	cout << "save -> off" << endl;
	m_computer->m_state = new OffState(m_computer);
}

OffState::OffState(Computer* computer) : State(computer) { }

void OffState::Change() {
	cout << "off -> on" << endl;
	m_computer->m_state = new OnState(m_computer);
}

class Computer {
public:
	State* m_state;

	Computer();

	void PushPower();
};

Computer::Computer() {
	m_state = new OnState(this);
}

void Computer::PushPower() {
	m_state->Change();
}

int main() {
	Computer* computer = new Computer(); // default : OnState

	computer->PushPower();
	computer->PushPower();
	computer->PushPower();

	return 0;
}

//결과
on -> save
save -> off
off -> on
```

### Objective-C
```objc
@interface State : NSObject {
    Computer* m_computer;
}
-(id)init:(Computer*)computer;
-(void)Change;
@end

@interface OnState : State 
-(id)init:(Computer*)computer;
-(void)Change;
@end

@interface SaveState : State 
-(id)init:(Computer*)computer;
-(void)Change;
@end

@interface SaveState : State 
-(id)init:(Computer*)computer;
-(void)Change;
@end

@interface OffState : State 
-(id)init:(Computer*)computer;
-(void)Change;
@end

@interface State
-(id)init:(Computer*)computer {
    if(self == [super init]) {
        m_computer = computer;
    }
    return self;
}
@end

@implementation OnState : State 
-(id)init:(Computer*)computer{
    return [super init:computer];
}
-(void)Change{
    NSLog(@"on -> save");
    m_computer.m_state = [[SaveState alloc] init:m_computer];
}
@end

@implementation SaveState : State 
-(id)init:(Computer*)computer{
    return [super init:computer];
}
-(void)Change {
    NSLog(@"save -> off");
    m_computer.m_state = [[OffState alloc] init:m_computer];
}
@end


@implementation OffState : State 
-(id)init:(Computer*)computer{
    return [super init:computer];
}
-(void)Change {
    NSLog(@"off -> on");
    m_computer.m_state = [[OnState alloc] init:m_computer];
}
@end

@interface Computer : NSObject{
	State* m_state;
}
-(id)init;
-(void)PushPower;
@end

@implementation Computer
-(id)init{
    if(self == [super init]){
        m_state = [[OnState alloc] init:self];
    }
    return self;
}

-(void)PushPower{
    [m_state Change];
}
@end

int main (int argc, const char * argv[]) {
    Computer* computer = [[Computer alloc] init]; //default : OnState

    [computer PushPower];
    [computer PushPower];
    [computer PushPower];   

    return 0;
}
```