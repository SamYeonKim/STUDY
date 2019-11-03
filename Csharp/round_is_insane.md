## C#에서 반올림 ( System.Math.Round)과 Unity 라이브러리 ( UnityEngine.Mathf )는 `중간점 값`을 `Banker's rounding` 으로 동작한다.

* `Banker's rounding` : 가장 가까운 짝수로 반올림 한다.
* `Standard rounding` : 가장 가까운 정수로 반올림 한다.

```cs
double[] arr_d = {0.3, 1.6, 2.6, 3.5, 4.5};           
for (int i = 0; i < arr_d.Length; i++) {
    Console.WriteLine(System.Math.Round(arr_d[i]));
}
//Output : 0, 2, 3, 4, 4

float[] arr_f = {0.3f, 1.6f, 2.6f, 3.5f, 4.5f};           
for (int i = 0; i < arr_f.Length; i++) {
    Debug.Log(UnityEngine.Mathf.Round(arr_f[i]));
}
//Output : 0, 2, 3, 4, 4
```

* 모든 소수점에 대해서 `Banker's rounding`을 쓴다는 이야기는 전혀 아니다.
* MSDN의 System.Math.Round (double d) 의 정의 -'배정밀도 부동 소수점 값을 가장 가까운 정수 값으로 반올림하고 중간점 값을 가장 가까운 짝수로 반올림합니다'- 처럼 `중간점` 값일때만 저렇게 동작된다.
* 위 처럼 동작 되기 때문에, 전투 로직 같은곳에서 데미지 계산시 의도치 않게 데미지 1이 더 추가 될 수 있겠다.
* System.Math.Round(0.5, MidpointRounding.AwayFromZero) 이런식으로 작성 하게 되면 의도한 대로 결과 값이 나온다.

## 참조

* [한글 좋아](http://rapapa.net/?p=3503)
* [What is Banker's Rounding](http://www.xbeat.net/vbspeed/i_BankersRounding.htm)