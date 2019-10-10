## How and when to use Exceptions

### Checked and unchecked exceptions

java에는 두가지 종류의 Exception이 존재하는데 `Checked` 와 `Unchecked` 이다.
`Checked`는 코딩 하는 타이밍에 이미 예측 가능한 예외이고, `Unchecked`는 런타임에 발생하는 예외이다.

### Checked

개발자의 실수로 발생한 예외가 아닌 외부 요인에 의해 발생할 수 있는 예외들이다.
`IOException`, `FileNotFoundException` 등이 대표적인 예이다.

```java
class FoolException extends Exception {}

class Test {
    public void sayNick(String nick) {
        throw new FoolException();  //컴파일 에러가 발생 한다. 해결 하기 위해선 try, catch로 감싸거나, 메소드에 throws구문을 넣어야 한다.
    }

    public void sayNick(String nick) {
        try {
            throw new FoolException(); 
        } catch (FoolException e) {
            ...
        } 
    }

    public void sayNick(String nick) throws FoolException {
        throw new FoolException(); 
    }
}
```

### Unchecked

개발자의 실수로 발생하는 예외들 이다. 
`IndexOutOfBoundsException`, `NullPointerException` 등이 대표적인 예이다.
```java
class FoolException extends RuntimeException {}

class Test {
    public void sayNick(String nick) {
        throw new FoolException();  //컴파일 에러가 발생 하지 않는다.
    }
}
```

## Using try-with-resources

Exception의 발생은 프로그램의 흐름을 강제로 끊어 버리는 행위이기 때문에, Exception이 발생하기 전에 사용하던 리소스의 해제가 안될 가능성이 굉장히 높다. 그렇기 때문에, 리소스를 제대로 해제 하기 위해선 아래와 같이 코드를 작성 해야 한다.

```java
public void readFile( final File file ) {
    InputStream in = null;
    try {
        in = new FileInputStream( file );        
    } catch( IOException ex ) {
    } finally {
        if( in != null ) {
            try {
                in.close();
            } catch( final IOException ex ) {
            } 
        }
    } 
}
```

하지만 위와 같이 `finally` 블록에서도 Exception이 발생할 여지가 있을 경우 `try/catch` 문을 또 작성해 줘야 한다. Java 7 버전 부터는 위와 같은 코드를 아래와 같이 간결하게 작성 할 수 있는 기능을 제공하는데 `try-with-resources` 라고 한다.

```java
//JAVA
public void readFile( final File file ) {
    try( InputStream in = new FileInputStream( file ) ) {        
    } catch( final IOException ex ) {
    }
}
```

`try` 문 안에서 Exception이 발생하여도, `Close` 함수가 호출되기 때문에, 별도의 처리를 해주지 않아도 된다. 하지만 `try` 문에 작성 하는한 클래스는 `AutoCloseable`을 상속 받은 클래스만 가능 하다.

```cs
//C#
try {
    using ( var fs = new System.IO.FileStream( "README", FileMode.Open ) ) {    
    }
} catch (System.Exception e) {

}
```

## Handling Checked Exception in Lambda

Lambda 식에서 `CheckedException` 에 대한 처리는 간결하게 이루어 지지 않는다.
아래 예제는 컴파일 에러가 발생하지 않겠끔만 처리한 코드이다. `Files.readAllBytes` 는 `CheckedException` 의 한 종류인 `IOException` 을 발생시킬 수 있는 함수 이기 때문에, 컴파일을 하려면 반드시 `try/catch` 구문으로 감싸줘야 한다. 하지만 예제 처럼만 처리하게 되면 Lambda 식을 호출하는 코드에서는 예외에 발생 유무를 알 수 있는 방법이 없다.

```java
    public void readFile()  {
        run( () -> {
            try {
                Files.readAllBytes( new File( "some.txt" ).toPath() );
            } catch (IOException e) {
                e.printStackTrace();
            }
        } );
    }
    public void run( final Runnable runnable ) {
        runnable.run();
    }

    public static void main(String args[]) {
        MyClass m = new MyClass();
        m.readFile();
        System.out.println("After");
    }
```

따라서 아래와 같이 처리를 해줘야한다.

```java
    public void readFile()  {
        run( () -> {
            try {
                Files.readAllBytes( new File( "some.txt" ).toPath() );
            } catch (IOException e) {
                throw new RuntimeException(e.getMessage());
            }
        } );
    }
    public void run( final Runnable runnable ) {
        runnable.run();
    }

    public static void main(String args[]) {
        MyClass m = new MyClass();
        m.readFile();
        System.out.println("After");
    }
```

## Defining your own exceptions

Java에서 제공하는 `Exception`이 아니라 커스탐하게 `Exception`을 만들 수 있다. Java에서는 일반적인 상황에서는 `Custom Exception` 을 만들때 `RuntimeException`을 상속 받도록 권장하고 있다.

```java
public class NotAuthenticatedException extends RuntimeException {
    public NotAuthenticatedException() { super(); }
    public NotAuthenticatedException(final String message ) { super(message); }
}

public void signin( final String username, final String password ) {
    if( !exists( username, password ) ) {
        throw new NotAuthenticatedException("User / Password combination is not recognized" );
    }
}
```

```cs
public class NotAuthenticatedException : System.Exception {
    public NotAuthenticatedException ( ) : base() { }
    public NotAuthenticatedException (string msg) : base( msg ) { }
}

public void signin( string username, string password ) {
    if( !exists( username, password ) ) {
        throw new NotAuthenticatedException("User / Password combination is not recognized" );
    }
}
```

