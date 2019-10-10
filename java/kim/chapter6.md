## How to write methods efficiently

### Method signatures

메소드를 작성함에 있어서 중요한 점은 `Signature(return type and arguments)` 이다. 3가지 규칙을 유념해서 작성해야 한다.

1. 메소드 이름은 메소드가 하는 일에 나타내어야 하고, 카멜 케이스로 작성 한다.
    ```java
    public void setTitleVisible() {}
    ```
2. 모든 arguments는 목적에 맞춰서 네이밍이 되어야 한다. 아무의미 없이 ex ) int i, string s 와 같이 작성하지 않아야 한다.
    ```java
    public void setTitleVisible( int length, String title ) {}
    ```
3. arguments의 갯수가 너무 많지 않아야 한다. Java에서는 최대 6개만 사용하길 추천한다.

### Method body

메소드의 구현부를 깔끔하고 읽기쉽도록 작성 할 수 있는 기본 가이드 라인은 다음과 같다.

1. 메소드는 하나의 목적을 위해서 작성되어야 한다.
    ```java
    public String setgetTitle( String new_title ) {}
    //-> 아래와 같이 수정 하는것이 좋다.
    public void setTitle( String new_title ) {}
    public String getTitle() {}    
    ```
2. 작성 할 때 너무 길게 작성해서 안된다. 몇 페이지에 걸쳐서 작성 할 경우 코드를 이해하기 어렵다.
3. `return` 키워드를 중간중간 많이 넣지 않아야 한다. 많이 넣게 되면 코드 흐름을 파악 하기 어렵다.
    ```java
    public int getMultipliedNum( int num ){
        int result = 0;
        if ( num == 1 ) {
            return 2;
        } else if ( num == 2 ) {
            return 3;
        } else {
            result = num * num;
        }
        return result;
    }
    ```
### Method overloading

동일한 이름을 유지 하면서 `arguments` 의 타입이나 조합만 변경해서 사용 할 수 있다. 하지만 반환형은 같아야 한다.

```java 
JAVA
public String numberToString( Long number ) {
    return Long.toString( number );
}
public String numberToString( BigDecimal number ) {
    return number.toString();
}
```

```cs
C#
public string numberToString( long number ) {
    return Long.toString( number );
}
public string numberToString( decimal number ) {
    return number.toString();
}
```

### Method overriding

부모 클래스에서 정의한 메소드를 재정의 해서 사용 할 수 있다.

```java
JAVA
public class Parent {
    public Object toString( Number number ) {
        return number.toString();
    }
}
public class Child extends Parent {
    @Override
    public String toString( Number number ) {
        return number.toString();
    }
}
```
```cs
C#
public class Parent {
    public virtual string toString( int number ) {
        return number.ToString();
    }
}
public class Child : Parent {
    public override string toString( int number ) {
        return number.ToString();
    }
}
```

위 예제 코드에서 `@Override`가 없어도 재정의하는데 전혀 문제가 없다. 하지만 항상 재정의 할 땐 `@Override`를 항상 붙이는 것을 강력히 권고 하는데, 그 이유는 컴파일러에게 재정의하는 함수라는 것을 알리고, 올바르게 재정의 하는지 컴파일 타임에 알 수 있기 때문이다.

```java
public class Parent {
    public Object toString( Number number ) {
        return number.toString();
    }
}
public class Child extends Parent {
    //원래 함수는 toString 인데, toStrig으로 오타를 낸 상황
    public String toStrig( Number number ) {
        return number.toString();
    }
}
```

위 예제 코드 처럼 오타가 난 상황이 발생할 때 컴파일러는 아무런 에러도 발생시키지 않는다, 왜냐 하면 재정의 하는 것인지 알 수 없기 때문에, 새로운 메소드로만 바라보기 때문이다.

### Method References

Java8 버전에서 추가된 기능으로, 메소드를 참조 형식으로 사용 할 수 있다. 자바에 있는 람다식을 이용하던 부분들 중에 단순히 이미 정의되어 있는 함수를 호출 하는것에 그치는 부분을 람다식대신 직접 호출될 함수를 전달하는 방식이다.

아래와 같은 방식으로 사용 가능 하다.

1. Static 메소드 참조

```java
List<String> l_words = Arrays.asList("hello", "world", "korea");
//Method Reference
l_words.forEach(System.out::println);
//Lambda
l_words.forEach((word)->System.out.println(word));
```

2. 생성자 참조

```java
class Word {
    String m_value;
    public Word(String _word) {
        m_value = _word;
    }
}

//Method Reference
l_words.stream().map(Word::new);
//Lambda
l_words.stream().map((w)->new Word(w));
```

3. 인스턴스 메소드 참조

```java
class Word {
    public void ToUpper(String _word) {
        System.out.println(_word.toUpperCase());
    }
}
Word w = new Word();
//Method Reference
l_words.forEach(w::ToUpper);
//Lambda
l_words.forEach(w->ToUpper(w));
```


