* `const` is `let` and `var` is `var`
  ```c#
  //c#
  const <Type> <ConstantName> = value;
  const int MAX_COUNT = 3;
  
  var name = "Kim";
  ```
  ```swift
  //swift
  let <ConstantName> :  <Type> = value;
  let MAX_COUNT : Int = 3
  let MAX_COUNT = 3
  
  var name = "Kim"
  var name : String = "Kim"
  ```
  * `semicolon(;)`은 c#에서는 반드시 라인 별로 넣어야 하는데, swift에서는 넣어도 되고, 안 넣어도 된다.
  넣는 경우는 한 라인에서 여러개의 값을 선언할때 사용한다.
    ```swift
    let MAX_COUNT=3; var name ="KIM"
    ```
  * c#에서 `const`는 반드시 CompileTime에 값이 설정 되어야 하는데, swift에서는 그럴필요 없다.
  Runtime에 설정 할 수 있다. 대신 반드시 타입이 선언 되어야 한다.
    ```swift
    let MAX_COUNT   //컴파일 에러 발생
    let MAX_COUNT : Int //OK
    ```
* `Class`
  ```c#
  class Employee {
    //Field 
    private string m_name;
    //Property
    public string Name {
      get { return m_name; }
      set { m_name = value; }
    }
    //Constructor
    public Employee( string name ) {
      m_name = name;
    }
    //Method
    public string GetName() {
      return m_name;
    }
  }
  
  //Create Instance
  Employee e = new Employee("Kim");
  string name = e.GetName();
  ```
  ```swift
  class Employee {
    var m_name : String
    var Name : String {
      get { return m_name }
      set { m_name = newValue }
    }
    
    init(name : String ) {
      self.m_name = name
    }
    
    func GetName() -> String {
      return m_name
    }
  }
  
  var e : Employee = Employee(name: "Kim")
  var name : String = e.GetName()
  ```
  
  * swift에는 c#의 `Constructor` 대신 `Initializer`가 반드시 있어야 한다.

* `Inheritance`
  * `class`
  ```c# 
  class Manager : Employee {
    public Manager(string name) : base(name)  //call Employee(name)
    public override string GetName(){}  //Only If the method in base class is marked as "virtual"
  }
  ```
  ```swift
  class Manager : Employee {
    override init(name : String) {
      super.init(name:name) //call Employee(name)
    }
    
    override func GetName() -> String {}
  }  
  ```
  * swift에서는 함수의 override를 위해서 c#처럼 `virtual`을 선언할 필요 없다.
  * `Multiple Inheritnce` With `interface` or `protocol`
    ```c# 
    interface IProjectManager {
      string projectName{ get; set; }
      int GetEmployeeCount();
    }
    
    class Manager : Employee, IProjectManager {
      prviate string m_p_name;
      public string projectName {
        get { return m_p_name; }
        set { m_p_name = value; }
      }
      
      public int GetEmployeeCount() {
        return 10;
      }
    }
    ```
    ```swift
    protocol ProjectManager {
      var projectName : String {get set}
      func GetEmployeeCount() -> Int
    }
    
    class Manager : Employee, ProjectManager {
      var m_p_name : String
      var projectName : String {
        get { return m_p_name }
        set { m_p_name = newValue }
      }
      func GetEmployeeCount() -> Int {
        return 15
      }
      
      override init(name: String) {
        m_p_name = "DMTC" //super ( Employee)에는 m_p_name에 할당 하는 부분이 없기 때문에, super.init을 하기 전에 할당 해줘야 한다.
        super.init(name: name);
      }
    }
    ```
* `Lambdas` are `Closures`
  ```c#
  List<string> l_names = new List<string>{"John", "Aria", "Rajesh", "Xavier"};
  l_names.Sort((s2, s1)=> s2.CompareTo(s1));
  
  foreach(string name in l_names ) {
    Console.WriteLine(name);    
  }
  
  //ouput :
  // Aria \n John \n Rajesh \n Xavier
  ```
  ```swift
  var l_names = ["John", "Aria", "Rajesh", "Xavier"]  
  l_names = l_names.sorted(by: {(s1: String, s2:String}-> Bool in return s2 > s1)
  
  for name in l_names {
    print(name)
  }
  //ouput :
  // Aria \n John \n Rajesh \n Xavier
  ```
* `Generic` is `Generic`
  ```c#
  void SwapTwoValues<T>( ref T a, ref T b ) {
    T temp;
    temp = a;
    a = b;
    b = temp;
  }
  int first = 3;
  int second = 15;  
  SwapTwoValues(ref first, ref second);
  Console.WriteLine(string.format("After Swap = {0} : {1}", first, second );
  //output
  //After Swap = 15 : 3  
  ```
  ```swift
  func SwapTwoValues<T>(_ a : inout T,_ b : inout T ) {
    let temp = a
    a = b
    b = temp
  }
  
  var first = 3
  var second = 15
  SwapTwoValues(&first, &second)
  print("After Swap = \(first) : \(second)")
  //output
  //After Swap = 15 : 3
  ```
  * swift에서 파라미터 선언시 `_`를 앞에 붙이게 되면 명시적인 파라미터 이름을 할당 하지 않겠다는 의미 이다.
    ```swift
    func A(_ first:Int, second:Int)
    A(15, second:100)
    ```

  

  
  
  
