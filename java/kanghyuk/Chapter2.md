# Using methods common to all objects

* 클래스 최상위에는 `Object` 클래스가 존재한다.
* 다음의 메소드를 가진다.
  * protected Object clone()
  * protected void finalize() : 오브젝트를 참조하는 곳이 없으면 파괴 요청
  * boolean equals(Object obj)
  * int hashCode()
  * String toString()
  * void notify() : 현재 오브젝트를 모니터링하는 스레드 한개를 깨운다
  * void notifyAll() : 현재 오브젝트를 모니터링하는 모든 스레드를 깨운다
  * void wait() : 현재 스레드를 대기시킨다

----
##  Methods equals and hashCode

* `equal()` 함수를 오버라이드 해서 사용할 수 있다.
* 다음의 규칙을 준수해야한다. 컴파일 단계에서 이 조건을 걸 수 없기 때문에 조심해야함.
  * Reflexive : 자신과 동일해야 한다.
  * Symmetric : x.equal(y) 이면 y.equal(x) 여야 한다.
  * Transitive : x.equal(y) == y.equal(z) -> x.equal(z) 여야 한다.
  * Consistent : 여러번 호출해도 동일한 값이 나와야 한다.
  * Equal TO Null : equal(null) == false 여야 한다.

```java
public class Person {
   private final String firstName;
   private final String lastName;
   private final String email;
   public Person( final String firstName, final String lastName, final String email ) {
      this.firstName = firstName;
      this.lastName = lastName;
      this.email = email;
   }
   public String getEmail() {
      return email;
   }
   public String getFirstName() {
      return firstName;
   }
   public String getLastName() {
      return lastName;
   }
   // Step 0: Please add the @Override annotation, it will ensure that your
   // intention is to change the default implementation.
   @Override
   public boolean equals( Object obj ) {
      // Step 1: Check if the ’obj’ is null
      if ( obj == null ) {
         return false;
      }
      // Step 2: Check if the ’obj’ is pointing to the this instance
      if ( this == obj ) {
         return true;
      }
      // Step 3: Check classes equality. Note of caution here: please do not use the
      // ’instanceof’ operator unless class is declared as final. It may cause
      // an issues within class hierarchies.
      if ( getClass() != obj.getClass() ) {
         return false;
      }
      // Step 4: Check individual fields equality
      final Person other = (Person) obj;
      if ( email == null ) {
         if ( other.email != null ) {
            return false;
         }
      } else if( !email.equals( other.email ) ) {
         return false;
      }
      if ( firstName == null ) {
         if ( other.firstName != null ) {
            return false;
         }
      } else if ( !firstName.equals( other.firstName ) ) {
         return false;
      }
      if ( lastName == null ) {
         if ( other.lastName != null ) {
            return false;
         }
      } else if ( !lastName.equals( other.lastName ) ) {
         return false;
      }
      return true;
   }
}
```

* `equal()` 메소드 재정의 시 `hasCode()` 메소드도 같이 재정의해줘야 한다.
* 재정의 할때 `final` 필드를 사용하면 필드에 영향을 받지 않는 메소드의 동작을 보장한다.?

```java
// Please add the @Override annotation, it will ensure that your
// intention is to change the default implementation.
@Override
public int hashCode() {
   final int prime = 31;
   int result = 1;
   result = prime * result + ( ( email == null ) ? 0 : email.hashCode() );
   result = prime * result + ( ( firstName == null ) ? 0 : firstName.hashCode() );
   result = prime * result + ( ( lastName == null ) ? 0 : lastName.hashCode() );
   return result;
}
```

----
## Method toString

* `toString()`은 디버깅용으로 많이 오버라이드 한다.
* 디폴트는 클래스 이름 + 해시코드를 리턴한다.

```java
@Override
public String toString() {
   return String.format("%s[email=%s, first name=%s, last name=%s]",
                        getClass().getSimpleName(), email, firstName, lastName);
}

// output before override
Person@6104e2ee
// output after override
Person[email=john.smith@domain.com, first name=John, last name=Smith]
```


----
## Method clone

* `clone()` 함수는 오버라이딩이 까다롭다.
  * `protected`이기 때문에 외부에서 사용하려면 `public`으로 재정의 해야 한다.
  * 오버라이드 클래스는 `Cloneable` 인터페이스를 구현해야 한다. 그렇지 않으면 `CloneNotSupportedException` 예외가 발생한다.
  * 구현 시 `super.clone()`을 먼저 호출해야 한다.
* copy 시 생성자가 호출되지 않기 때문에 의도하지 않은 결과가 나올 수 있다.
* 정확한 복사본을 원한다면 생성자 복사나 팩토리 메서드를 사용하는 것을 추천한다.

```java
public class Person implements Cloneable {
   // Please add the @Override annotation, it will ensure that your
   // intention is to change the default implementation.
   @Override
   public Person clone() throws CloneNotSupportedException {
      return ( Person )super.clone();
   }
}
```

----
## Method equals and == operator

* 자바에서 `==`와 `equal()`은 다르다.
* `==`는 참조값 비교를 한다. (더 엄격함)
* 동일한 인스턴스 비교를 원하지 않는다면 `equal()`을 사용해야 한다.

```java
final String str1 = new String( "bbb" );
System.out.println( "Using == operator: " + ( str1 == "bbb" ) );
System.out.println( "Using equals() method: " + str1.equals( "bbb" ) );

// output
Using == operator: false
Using equals() method: true
```

----
## Useful helper classes

* Java 7 이후 헬퍼클래스로 `Objects`가 추가되었다. (`java.util.Objects`)
* 오버라이드를 편하게 하도록 함수를 제공한다.
  * `static boolean equals(Object a, Object b)`
  * `static int hash(Object...values)` : 해시 코드를 생성한다.
  * `static int hashCode (Object o)`

```java
@Override
public boolean equals( Object obj ) {
   if ( obj == null ) {
      return false;
   }
   if ( this == obj ) {
      return true;
   }
   if ( getClass() != obj.getClass() ) {
      return false;
   }
   final Person other = (Person) obj;
   if( !Objects.equals( email, other.email ) ) {
      return false;
   } else if( !Objects.equals( firstName, other.firstName ) ) {
      return false;
   } else if( !Objects.equals( lastName, other.lastName ) ) {
      return false;
   }
   return true;
}
@Override
public int hashCode() {
   return Objects.hash( email, firstName, lastName );
}
```


## 전체 코드

```java
// Person.java
import java.util.Objects;

public class Person {
   private final String firstName;
   private final String lastName;
   private final String email;
   public Person( final String firstName, final String lastName, final String email ) {
      this.firstName = firstName;
      this.lastName = lastName;
      this.email = email;
   }
   public String getEmail() {
      return email;
   }
   public String getFirstName() {
      return firstName;
   }
   public String getLastName() {
      return lastName;
   }
   // Step 0: Please add the @Override annotation, it will ensure that your
   // intention is to change the default implementation.
   @Override
   public boolean equals( Object obj ) {
      // Step 1: Check if the ’obj’ is null
      if ( obj == null ) {
         return false;
      }
      // Step 2: Check if the ’obj’ is pointing to the this instance
      if ( this == obj ) {
         return true;
      }
      // Step 3: Check classes equality. Note of caution here: please do not use the
      // ’instanceof’ operator unless class is declared as final. It may cause
      // an issues within class hierarchies.
      if ( getClass() != obj.getClass() ) {
         return false;
      }
      final Person other = (Person) obj;
      if( !Objects.equals( email, other.email ) ) {
         return false;
      } else if( !Objects.equals( firstName, other.firstName ) ) {
         return false;
      } else if( !Objects.equals( lastName, other.lastName ) ) {
         return false;
      }
      return true;
      }
      @Override
   public int hashCode() {
      return Objects.hash( email, firstName, lastName );
   }
   @Override
   public String toString() {
      return String.format( "%s[email=%s, first name=%s, last name=%s]",
                              getClass().getSimpleName(), email, firstName, lastName );
   }
}

// Main.java
public class Main{
   public static void main(String []args){
      final Person person = new Person( "John", "Smith", "john.smith@domain.com" );
      System.out.println( person.toString() );

      String str1 = new String( "bbb" );
      System.out.println( "Using == operator: " + ( str1 == "bbb" ) );
      System.out.println( "Using equals() method: " + str1.equals( "bbb" ) );
   }
}
```

```cs
using System;

public class Person {
   private string firstName;
   private string lastName;
   private string email;
   public Person( string firstName, string lastName, string email ) {
      this.firstName = firstName;
      this.lastName = lastName;
      this.email = email;
   }
   public string getEmail() {
      return email;
   }
   public string getFirstName() {
      return firstName;
   }
   public string getLastName() {
      return lastName;
   }

   public bool equals( Object obj ) {
      if ( obj == null ) {
         return false;
      }
      if ( this == obj ) {
         return true;
      }
      if ( this.GetType() != obj.GetType() ) {
         return false;
      }
      Person other = (Person) obj;
      if( !email.Equals( other.email ) ) {
         return false;
      } else if( !firstName.Equals(other.firstName ) ) {
         return false;
      } else if( !lastName.Equals(other.lastName ) ) {
         return false;
      }
      return true;
      }
      
   public int hashCode() {
      return this.GetHashCode();
   }

   public override string ToString() {
      return string.Format( "{0}[email={1}, first name={2}, last name={3}]",
                              this.GetType(), email, firstName, lastName );
   }

}

class HelloWorld {
  static void Main() {
      Person person = new Person( "John", "Smith", "john.smith@domain.com" );
      Console.WriteLine( person.ToString() );

      string str1 = "bbb";
      Console.WriteLine( "Using == operator: " + ( str1 == "bbb" ) );
      Console.WriteLine( "Using equals() method: " + str1.Equals( "bbb" ) );
  }
}
```
