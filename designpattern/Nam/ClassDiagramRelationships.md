# Class Diagram Relationships

1. Dependency(의존)

+ 모양 : 점선 화살표 '-->'
+ Dependency는 한 클래스의 변화가 이것을 사용하는 다른 클래스에 영향을 미치는 관계
+ 의존 클래스를 사용하는 클래스는 이를 참조하고 있는 인스턴스를 유지하지 않음

2. Association(연관)

+ 모양 : 실선 화살표 'ㅡ>'
+ Association은 한 클래스가 다른 클래스와 연결되어 있음을 나타내는 관계
+ 연관 관계인 두 클래스가 서로를 알고 있다면(두 클래스가 참조하고 있다면) 양방향 관계, 한 클래스만 알고 있다면 단방향 관계 

3. Aggregation(집합 연관)

+ 모양 : 다이아몬드가 달려 있는 실선 '◇ㅡ'
+ 연관의 특수 관계중 하나이며 연관 관계 이면서 부분과 전체인 관계
+ 연결된 클래스의 인스턴스 생성과 소멸이 같냐, 같지 않냐로 Composition과 Aggregation으로 나뉜다. Aggregation은 후자

4. Composition(복합 연관 또는 합성 연관)

+ 모양 : 다이아몬드 색이 채워져 있는 실선 '◆ㅡ'

```
class A {
    B b;
    C c;
    D d;

    public A() {
        b = new B();
    }

    public A(C c) {
        this.c = c;
    }

    public void Set(D d) {
        this.d = d;
    }

    public void Set(E e) {
        return e.isDependency();
    }
}
```

위 코드에서 각 관계는 아래와 같다.
1. A와 B의 관계는 Composition이다.
B의 인스턴스는 A와 LifeCycle을 같이하고 있기 때문이다. -> 강한 집합관계

2. A와 C의 관계는 Aggregation 이다.
C의 인스턴스는 외부에서 만들어지기 때문에 A의 LifeCycle과 상관이 없다.

3. A와 D의 관계는 Association이다.
Aggragation과 마찬가지로 D의 LifeCycle은 A와 관계가 없다.
차이점은 A 클래스가 D의 인스턴스를 가질수도 있고, 안 가질수도 있다. 메소드 형태로 제공되기 때문에 A 클래스를 사용하는 곳 에서 해당 메소드를 사용할지 안할지 여부를 알 수가 없다. 즉 부분-전체 관계가 성립하지 않는다.

4. A와 E의 관계는 Dependency이다.
E의 인스턴스는 메소드 내부에서 사용하고 바로 소멸되기 때문에 인스턴스의 참조가 유지되지 않는다.