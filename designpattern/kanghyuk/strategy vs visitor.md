# Strategy, Visitor 차이

* `Visitor`는 `element structure`에 여러 element들을 넣어놓고 각각의 element가 행동을 정의한 특정 객체를 받아 각각 다른 일을 하도록 만드는 패턴.
* 각각의 element들은 같은 객체를 사용하지만, 하는 일은 다르다. 해당 일의 구현은 완전히 다른 객체가 전담한다. `Strategy`와 비교했을 때, 객체마다 하는일이 다르다는건 공통점, 행동을 다른 객체에서 구현한다는 것이 차이점이다.
* 약간 모호하지만, `Strategy`는 한 element의 행동을 여러가지로 구현하는 것이고, `Visitor`는 여러 element들의 행동을 각 visitor class의 목적에 맞게 여러가지로 구현하는 것이라고 한다.
** 1:n 매칭, m:n 매칭이 차이점이다.
* `Strategy`는 알고리즘이 바뀔 때 해당 클래스를 직접 수정해야 되지만. `Visitor`는 외부 객체에 알고리즘이 구현되어 있어 클래스를 수정하지 않아도 된다.
* `Visitor`는 `Overloadding`을 사용하여 각 객체마다 다른 행동을 정의해야 한다.