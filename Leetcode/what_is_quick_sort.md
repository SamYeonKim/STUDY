#### Definition

이름 처럼 빠른 정렬 방식이고, `Merge Sort`에 비해 2~3배 빠르다.

#### Flow

1. 주어진 리스트를 2개로 나눌 `pivot` 값을 선정 한다. 하나의 서브 리스트에는 `pivot` 보다 작은 값들로만 구성되고, 다른 서브리스트에는 `pivot`과 동일하거나 큰 값으로 구성 한다. 이렇게 나누는 작업을 `partitioning` 이라 한다. `pivot` 값을 선정 하는 방식은 주어진 리스트의 첫번째 인자를 사용하거나, 랜덤하게 선택 하면 된다.
2. `partitioning` 이후 재귀저그로 2개의 서브 리스트를 정렬 한다.
3. 정렬된 2개의 서브리스트를 합치기만 하면 끝난다.

#### Implements

0. `pivot`은 가장 마지막 원소로 설정
1. [1,5,3,2,8,7,6,4] 리스트에서 `pivot`을 선정, 리스트의 가장 마지막 값인 `4`로 설정
2. `partitioning` 수행, `pivot`을 기준으로 작은 값들을 한쪽으로 몰고, 큰 쪽을 반대 쪽으로 몰자. 리스트의 시작 index 부터 비교해 나가면서 `pivot` 보다 작은게 나올수록 마지막으로 이동했던 index에 있는 값과 위치 변경하고 index를 증가.
3. 모든 비교가 끝나면 `pivot`의 값과 마지막으로 이동한 index의 값과 위치 변경
4. 모든 서브 리스트를 돌때 까지 2~3번 작업 반복

```
1차 수행
[1,5,3,2,8,7,6,4], pivot = 4
> 5 와 3 스왑 => [1,3,5,2,8,7,6,4]
> 5 와 2 스왑 => [1,3,2,5,8,7,6,4]
> 5 와 4 스왑 => [1,3,2,4,8,7,6,5]
partitioning 종료, 기준 index = 3

index 0~2 까지 정렬 수행
index 4~7 까지 정렬 수행
```


```java
public class Solution {

  public void quickSort(int [] lst) {
   /* Sorts an array in the ascending order in O(n log n) time */
    int n = lst.length;
    qSort(lst, 0, n - 1);
  }

  private void qSort(int [] lst, int lo, int hi) {
    if (lo < hi) {
      int p = partition(lst, lo, hi);
      qSort(lst, lo, p - 1);
      qSort(lst, p + 1, hi);
    }
  }

  private int partition(int [] lst, int lo, int hi) {
    /*
      Picks the last element hi as a pivot
      and returns the index of pivot value in the sorted array */
    int pivot = lst[hi];
    int i = lo;
    for (int j = lo; j < hi; ++j) {
      if (lst[j] < pivot) {
        int tmp = lst[i];
        lst[i] = lst[j];
        lst[j] = tmp;
        i++;
      }
    }
    int tmp = lst[i];
    lst[i] = lst[hi];
    lst[hi] = tmp;
    return i;
  }

}
```

#### 참조

* [한글 좋아](https://gmlwjd9405.github.io/2018/05/10/algorithm-quick-sort.html)