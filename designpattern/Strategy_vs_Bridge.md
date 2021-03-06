# Creational vs Structural vs Behavioral

1. Creational : 객체 생성에 대한 구조를 중점적으로 다루는 패턴이며, 객체 생성의 기본 형태가 설계 문제를 야기하거나 코드 기반의 복잡성을 증가시킬 수 있는 상황에서 사용

2. Structural : 객체 간의 구성에 대해 설계를 용이하게 하여 새로운 기능을 얻기 위해 객체를 작성하는 방법을 정의하는 패턴

3. Behavioral : 객체가 상호작용하는 방식을 용이하게 하기 위함, 서로 다른 객체들이 어떻게 메시지를 보내고 작업을 일어나게 하는지에 대한 객체 간의 의사소통과 관련이 있는 패턴

# Strategy vs Bridge

Strategy 패턴의 목적은 런타임에 동작을 교환할 수 있도록 하는 것이며, Strategy의 인터페이스 참조 및 동작을 호출하는 쪽에서 알거나 포함하고 있어야 한다.

Bridge 패턴의 기능 자체는 Strategy와 동일하지만 목적은 추상화를 구현과 완전히 분리하여 독립적으로 변경할 수 있는 결합도를 낮추는 구조에 있다. Bridge의 추상화 인터페이스에서는 구현 인터페이스의 참조를 알지 못하거나 포함하지 않는다.