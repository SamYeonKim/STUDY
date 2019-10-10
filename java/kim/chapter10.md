## Built-in Serialization techniques

### What it Serialization

Java 객체들을 파일 따위로 저장하거나, 나중에 다시 재생산을 하기 위해 변환하는 과정이다.

### Serializable interface

Java에서 특정 클래스에 Serialization 기능을 부여하려면 `java.io.Serializable` 을 상속 받으면 된다.

```java
public class MyClass implements Serializable {    
}
public static void main(String args[]) throws IOException, ClassNotFoundException {
    final Path storage = new File( "object.ser" ).toPath();

    MyClass myclass = new MyClass();

    try( final ObjectOutputStream out =
                    new ObjectOutputStream( Files.newOutputStream( storage ) ) ) {
        out.writeObject( myclass );
    }

    try( final ObjectInputStream in =
                    new ObjectInputStream( Files.newInputStream( storage ) ) ) {
        final MyClass instance = ( MyClass )in.readObject();
    }
}
```
```cs
[System.Serializable]
public class MyClass {
}

public static void Main(){
    string file_path = "C:\tmp\object.ser";
    BinaryFormatter formatter = new BinaryFormatter();

    MyClass myclass = new MyClass();
    using( FileStream serialize_stream = new FileStream( file_path, FileMode.CreateNew )) {
        formatter.Serialize( serialize_stream, myclass );
    }
    
    using ( FileStream deserialize_stream = new FileStream( file_path, FileMode.Open ) ) {
        MyClass saved_class = (MyClass)formatter.Deserialize( deserialize_stream );
    }
}
```

Java에서 `Serialize`한 이후 다시 객체화 시키는 것을 `Deserialization` 이라고 하는데, `Deserialization`을 수행할 때 검증 과정을 거친다. `Serialize`된 클래스와 `Deserialize`하려는 클래스가 동일한지 확인한다. 이를 위해서 `serialVersionUID`라는 값으로 비교를 하는데, 명시적으로 선언을 하지 않으면, 기본적으로 Runtime에 생성되는데, 기본적으로 선언되는 값은 매우 민감하다. 예를 들어 아래 클래스는 서로 다른 `serialVersionUID`를 갖는다.

```java
public class MyClass implements Serializable {    
}

public class MyClass implements Serializable {
    public int m_n_num;
}
```

그렇기 때문에, 첫번째 `MyClass` 를 이용해서 `Serialize`하고, `MyClass`에 멤버를 추가한 이후에 `Deserialize`를 하게 되면 아래와 같은 Exception이 발생한다.

```
java.io.InvalidClassException: com.example.lib.MyClass; local class incompatible: stream classdesc serialVersionUID = 1293618723, local class serialVersionUID = 5477323136109195461
```

하지만, 명시적으로 `serialVersionUID`을 선언하게 되면 위와 같은 행위를 하여도 Exception이 발생 하지 않는다. `serialVersionUID`를 선언할땐 반드시 `static, final` 이어야만 하고, `long` 타입이어야 한다.
```java
public class MyClass implements Serializable {    
    private static final long serialVersionUID = 12939123123L;
}
```

> 하지만 C#에서는 위와 같이 멤버를 추가 하더라도 `Deserialize` 할 때 Exception이 발생 하지 않는다.
멤버의 이름이 변경됬을때 Exception이 발생하고, 오히려 Java에서는 멤버의 이름이 변경되었더라도, `serialVersionUID`가 동일하면, Exception이 발생하지 않는다.

### Externalizable interface

Java에는 `java.io.Serializable` 인터페이스와는 다르게 커스텀하게 `Serialize, Deserialize`를 컨트롤 할 수 있는 `java.io.Externalizable` 이 있다.

```java
public class MyClass implements Externalizable {
    public int m_n_num;

    public MyClass(){}

    @Override       //Deserialize 할 때 호출됨
    public void readExternal(ObjectInput objectInput) throws IOException, ClassNotFoundException {
        m_n_num = objectInput.readInt();
    }

    @Override       //Serialize 할 때 호출됨
    public void writeExternal(ObjectOutput objectOutput) throws IOException {
        objectOutput.writeInt(m_n_num);
    }
}
```

한가지 유념해야 할 사항은 `readExternal` 에서 저장된 Object에서 값을 가져올때 임의로 가져 올 수 없다는 사실이다. 예를 들어 아래와 같은 상황이 벌어 질 수 있다.

```java
public class MyClass implements Externalizable {
    public int m_n_num_1;
    public int m_n_num_2;

    public MyClass(){}

    @Override
    public void readExternal(ObjectInput objectInput) throws IOException, ClassNotFoundException {
        m_n_num_2 = objectInput.readInt();
        m_n_num_1 = objectInput.readInt();
    }

    @Override
    public void writeExternal(ObjectOutput objectOutput) throws IOException {
        objectOutput.writeInt(m_n_num_1);
        objectOutput.writeInt(m_n_num_2);
    }
}

public static void main(String args[]) throws IOException, ClassNotFoundException {
    MyClass myclass = new MyClass();
    myclass.m_n_num_1 = 100;
    myclass.m_n_num_2 = 200;

    //Serialize
    //Deserialize
    
    System.out.println(deserialized_myclass.m_n_num_1); //200
    System.out.println(deserialized_myclass.m_n_num_2); //100
}
```

`myclass`의 num_1에는 100, num_2에는 200을 할당하고, `Serialize, Deserialize` 를 수행한 이후 결과값은 서로 저장된 값이 달라진것을 볼 수 있다. 이유는 `writeExternal`에서 `writeInt`의 순서와 `readExternal`에서 `readInt`의 순서가 서로 달라서 발생한다.



