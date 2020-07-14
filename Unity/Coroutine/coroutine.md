- [Coroutine에 대해 알아보자](#coroutine에-대해-알아보자)
  - [Coroutine 의 실행](#coroutine-의-실행)
  - [Coroutine 끝내기](#coroutine-끝내기)
  - [Yiled 문](#yiled-문)
  - [Custom Yield 문](#custom-yield-문)

# Coroutine에 대해 알아보자

* Coroutine은 Thread와 전혀 다르다.
* Coroutine과 일반 함수의 차이는 일시정지가 있다는것이다.

```cs
IEnumerator Timer()
{
    int seconds = 0;
    while( true )
    {
        Debug.Log("Ticking : " + seconds++);
        
        yield return new WaitForSeconds(1);
    }
}
```

## Coroutine 의 실행

* StartCoroutine 함수를 이용해서 실행 시킬 수 있다.
* StartCoroutine(IEnumerator routine);
* StartCoroutine(String routineName);
  * routine을 넘기는것에 비해 Overhead가 발생한다.
* StartCoroutine(String routineName, object parameter);
  * 오직 하나의 파라미터만 사용할 수 있다.

```cs
void Start()
{
    //Call With Function
    StartCoroutine(Timer());
    //Call With Function Name
    StartCoroutine("Timer");
    //Call With Function Name And One Parameter
    StartCoroutine("Timer", 1);
}
```

## Coroutine 끝내기

* StopCoroutine(IEnumerator routine)
* StopCoroutine(Coroutine routine)
* StopCoroutine(String routineName)
* StopAllCoroutines()
* disable MonoBehaviour

```cs
void Start()
{
    //Stop With IEnumerator
    IEnumerator routine = Timer();
    StopCoroutine(routine);
    //Stop With Coroutine
    Coroutine coroutine = StartCoroutine(Timer());
    StopCoroutine(coroutine);
    //Stop All Coroutine
    StartCoroutine(Timer());
    StopAllCoroutines();
}
```

## Yiled 문

* WaitForEndOfFrame
* WaitForFixedUpdate
* WaitForSeconds
* WaitForSecondsRealTime
* WaitUntil
* WaitWhile
* Coroutine
* null

## Custom Yield 문

[Manual](https://docs.unity3d.com/ScriptReference/CustomYieldInstruction.html)

