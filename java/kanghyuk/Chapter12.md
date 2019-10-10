# Dynamic languages support

* 동적 언어 지원에 대해 이야기한다.
* 동적 언어는 프로그램의 동작이 런타임에 정의된다.
* `Java` 응용 프로그램에 동적 언어를 응용하는 방법에 대해 설명한다.

----
##  Dynamic Languages Support

* java 컴파일러는 `JVM`에서 해석하는 바이트 코드를 생성한다.
* 동적 언어는 런타임에 유형 검사를 수행한다.
* `Java7` 이후 `JVM`이 메소드 호출 위치와 메소드 구현 사이의 연결을 사용자 정의 할 수 있는 명령어 `invokedynamic`을 도입했다.

----
##  Scripting API

* `Java6`에서 `javax.script` 패키지에 스크립팅 API가 추가되었다.
  * 대부분 스크립트 언어를 플러그인하고 `JVM` 플랫폼에서 실행하도록 설계되었다.
* 스크립팅 API 작업 방법
  * `ScriptEngineManager` 클래스 인스턴스 생성
    * 사용가능한 스크립팅 엔진을 검색할 수 있는 기능을 제공
    * 스크립팅 엔진은 `ScriptEngine` 구현을 사용하여 표현됨.
  * `eval()` 함수를 사용하여 스크립트를 실행한다.
* `JavaScript`, `Groovy`, `Ruby/JRuby`, `Python/Jython` 등을 지원한다.

----
## JavaScript on JVM

* `eval()` 함수는 스크립트를 실행하여 일반 `Java`문자열로 전달한다.
* 변수나 프로퍼티를 출력하려면 변수 바인딩을 사용하여 함수를 호출한다.
  * `ScriptEngine.put(key, val)`로 변수 선언이 가능하다.
* Java8부터 `Nashorn`이라는 새로운 스크립팅 엔진 구현이 추가되었음.

```java
import javax.script.Bindings;
import javax.script.ScriptEngine;
import javax.script.ScriptEngineManager;
import javax.script.ScriptException;

public class Main {
	public static void main( String[] args ) throws ScriptException {
        final ScriptEngineManager factory = new ScriptEngineManager();
        final ScriptEngine engine = factory.getEngineByName( "JavaScript" ); // "Nashorn" 사용 가능
        
        engine.put( "str", "Calling JavaScript" );
        engine.eval( "print(str)");

        final Bindings bindings = engine.createBindings();
        bindings.put( "str", "Calling JavaScript" );
        bindings.put( "engine", engine );
        
        engine.eval( "print(str + ' from ' + engine.getClass().getSimpleName() )", bindings );
	}
}

// output 
Calling JavaScript
Calling JavaScript from NashornScriptEngine
```

* cs로는 동일하게 사용할 수 없고, `ASP.NET`에서 `Response.Write()`라는 함수를 통해 스크립트를 호출할 수 있다.

----
## Groovy on JVM

* groovy : JVM 위에서 동작하는 동적 타이핑 프로그래밍 언어
* 

```java
// Book.java
package test;

public class Book {
    private final String author;
    private final String title;
    
    public Book(final String author, final String title) {
        this.author = author;
        this.title = title;
    }

    public String getAuthor() {
        return author;
    }

    public String getTitle() {
        return title;
    }
}

// Main.java
package test;
import javax.script.Bindings;
import javax.script.ScriptEngine;
import javax.script.ScriptEngineManager;
import javax.script.ScriptException;

import java.io.IOException;
import java.io.InputStreamReader;
import java.io.Reader;
import java.util.Arrays;
import java.util.Collection;

public class Main {
	public static void main(String[] args) throws ScriptException, IOException {
	    final ScriptEngineManager factory = new ScriptEngineManager();
	    final ScriptEngine engine = factory.getEngineByName( "groovy" );

	    final Collection< Book > books = Arrays.asList(
	            new Book( "Venkat Subramaniam", "Programming Groovy 2" ),
	            new Book( "Ken Kousen", "Making Java Groovy" )
	        );
	            
	    final Bindings bindings = engine.createBindings();
	    bindings.put( "books", books );
        bindings.put( "engine", engine );
      
        try( final Reader reader = new InputStreamReader( Book.class.getResourceAsStream("/test/script.groovy" ) ) ) {
            engine.eval( reader, bindings );        
      }
	}
}

// output
Book Programming Groovy 2 is written by Venkat Subramaniam
Book Making Java Groovy is written by Ken Kousen
Executed by GroovyScriptEngineImpl
Free memory (bytes): 227448640
```

```groovy
books.each {
println "Book ’$it.title’ is written by $it.author"
}
println "Executed by ${engine.getClass().simpleName}"
println "Free memory (bytes): " + Runtime.getRuntime().freeMemory()
```

----
## Ruby on JVM

* 엔진이름 : `jruby`

----
## Python on JVM

* 엔진이름 : `jython`

```java
package test;
import javax.script.Bindings;
import javax.script.ScriptEngine;
import javax.script.ScriptEngineManager;
import javax.script.ScriptException;

import java.io.IOException;
import java.io.InputStreamReader;
import java.io.Reader;
import java.util.Arrays;
import java.util.Collection;

public class Main {
	public static void main(String[] args) throws ScriptException, IOException {
	    final ScriptEngineManager factory = new ScriptEngineManager();
	    final ScriptEngine engine = factory.getEngineByName( "jython" );

	    final Collection< Book > books = Arrays.asList(
	            new Book( "Mark Lutz", "Learning Python" ),
	            new Book( "Jamie Chan", "Learn Python in One Day and Learn It Well" )
	        );
	            
	    final Bindings bindings = engine.createBindings();
	    bindings.put( "books", books );
        bindings.put( "engine", engine );

        try( final Reader reader = new InputStreamReader( Book.class.getResourceAsStream("/test/script.py" ) ) ) {
            engine.eval( reader, bindings );        
        }
	}
}

// output
Book Learning Python is written by Mark Lutz
Book Learn Python in One Day and Learn It Well is written by Jamie Chan
Executed by PyScriptEngine
Free memory (bytes): 212510968
```

```python
from java.lang import Runtime

for it in books:
    print "Book %s is written by %s" % (it.title, it.author)
    
print "Executed by " + engine.getClass().simpleName
print "Free memory (bytes): " + str( Runtime.getRuntime().freeMemory() )

```

----
## Using Scripting API

* `Java scripting API`는 `domain-specific languages (DSLs)`를 연결하는 가장 간단한 방법이다.