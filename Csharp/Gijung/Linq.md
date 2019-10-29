# LINQ

## Group

### [`사용 방법`]

`group [그룹화 대상] by [그룹 조건] into [그룹화된 정보]`

### [`사용 예시`]

```csharp
using UnityEngine;
using System.Linq;

enum PersonType {
	Man,
	Woman,
}

class Person {
	public PersonType m_type;
	string m_name;

	public Person(PersonType type, string name) {
		m_type = type;
		m_name = name;
	}

	public void Itroduce() {
		Debug.Log ("성별 : " + m_type + ", 이름 : " + m_name);
	}
}

public class A : MonoBehaviour {
	void Start () {
		Person[] arr_person_group = new Person[] {
			new Person(PersonType.Woman, "A"),
			new Person(PersonType.Man, "B"),
			new Person(PersonType.Woman, "C"),
			new Person(PersonType.Man, "D"),
		};

		// 그룹화
		var grouping_example = from person in arr_person_group
			group person by person.m_type into new_group
			select new {person_type = new_group.Key, people = new_group};

		// 그룹화된 사람들중 남자만 출력
		foreach (var group in grouping_example) {
			if (group.person_type == PersonType.Man) {
				foreach (Person person in group.people) {
					person.Itroduce ();
				}
			}
		}
	}
}

// Output
"성별 : Man, 이름 : B"
"성별 : Man, 이름 : D"
```

## Aggregate

+ 사용자가 직접 데이터 처리를 작성 할 수 있다.

### [`사용 방법`]

`Aggregate([초기값], [데이터 처리], [최종 데이터 처리])`

### [`사용 예시`]

```csharp
using UnityEngine;
using System.Linq;

public class A : MonoBehaviour {
	void Start () {
		string[] names = { "abc", "kim", "lee", "han", "park" };

		Debug.Log(names.Aggregate("Friends : ", (str1, str2)=>str1 + " " + str2, name=>name.ToUpper()));
	}
}

// Output
"FRIENDS :  ABC KIM LEE HAN PARK"
```

## Conversion

### ToArray, ToList, ToDictionary

### [`사용 예시`]

```csharp
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class A : MonoBehaviour {
	void Start () {
		List<int> input = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; 

		// ToArray
		int[] array = (from num in input 
			where (num % 2) == 0 
			select num).ToArray();

		for (int idx = 0; idx < array.Length; idx++) {
			Debug.Log(array[idx]);
		}

		// Output
		"2"
		"4"
		"6"
		"8"
		"10"
		
		// ToList
		List<int> list = (from num in array 
			where (num % 2) == 0 
			select num).ToList(); 

		list.Add (12);

		for (int idx = 0; idx < list.Count; idx++) {
			Debug.Log(list[idx]);
		}

		// Output
		"2"
		"4"
		"6"
		"8"
		"10"
		"12"

		// ToDictionary
		Dictionary<string, int> dic = (from num in list
			where (num % 2) == 0
			select num).ToDictionary ((x) => x.ToString() + "abc", (x)=>x);
		
		foreach (var item in dic) { 
			Debug.Log("key : " + item.Key + ", value : " + item.Value);
		}

		// Output
		"key : 2abc, value : 2"
		"key : 4abc, value : 4"
		"key : 6abc, value : 6"
		"key : 8abc, value : 8"
		"key : 10abc, value : 10"
		"key : 12abc, value : 12"
	}
}
```

## Element

### First, ElementAt

+ 요소에 접근.

### [`사용 예시`]

```csharp
string[] names = { 
			"Hartono, Tommy", 
			"Adams, Terry", 
			"Andersen, Henriette Thaulow",
			"Hedlund, Magnus", "Ito, Shu" 
		};

var categorized_name = from name in names
		where name.Contains("o")
		select name;

Debug.Log("First Element  : " + categorized_name.First());
Debug.Log("Second Element : " + categorized_name.ElementAt(1));

// Output
"First Element  : Hartono, Tommy"
"Second Element : Andersen, Henriette Thaulow"
```

## Generation Operators

+ 그룹을 새로운 형태로 변환하여 반환.

### [`사용 예시`]

```csharp
var numbers =
	from n in Enumerable.Range(100, 50)	// 100부터 시작하여 50개
	select new { Number = n, OddEven = n % 2 == 1 ? "odd" : "even" };

foreach (var n in numbers) {
	Debug.Log(string.Format("The number {0} is {1}.", n.Number, n.OddEven));
}

// Output
"The number 100 is even."
"The number 101 is odd."
"The number 102 is even."
...
```

## Join

+ 두 데이터를 통합.

### [`사용 방법`]

`join [비교 대상] on [비교 조건] (into [결과 저장])`

### [`사용 예시`]

```csharp
class Person {
	public string m_name;
	public string m_country;

	public Person(string name, string country) {
		m_name = name;
		m_country = country;
	}
}

class Age {
	public string m_name;
	public int m_n_age;

	public Age(string name, int n_age){
		m_name = name;
		m_n_age = n_age;
	}
}

public class A : MonoBehaviour {
	void Start () {
		Person[] arr_person = new Person[] {
			new Person("A", "korea"),
			new Person("B", "canada"),
			new Person("C", "usa"),
		};

		Age[] arr_age = new Age[] {
			new Age("A", 12),
			new Age("B", 13),
			new Age("C", 9),
		};

		var quary =
			from person in arr_person
			join age in arr_age on person.m_name equals age.m_name
			select new {Name = person.m_name, Country = person.m_country, Age = age.m_n_age};

		foreach (var person in quary) {
			Debug.Log(person);
		}
	}
}

// Output
"{ Name = A, Country = korea, Age = 12 }"
"{ Name = B, Country = canada, Age = 13 }"
"{ Name = C, Country = usa, Age = 9 }"
```

## Miscellaneous Operators (여러가지 Operator)

### Concat

+ 대상 통합.

### [`사용 예시`]

```csharp
class Person {
	public string m_name;

	public Person(string name) {
		m_name = name;
	}
}

class Country {
	public string m_country;

	public Country(string country){
		m_country = country;
	}
}

public class A : MonoBehaviour {
	void Start () {
		Person[] arr_person = new Person[] {
			new Person("A"),
			new Person("B"),
			new Person("C"),
		};

		Country[] arr_country = new Country[] {
			new Country("korea"),
			new Country("canada"),
			new Country("usa"),
		};

		var name = from person in arr_person
					select person.m_name;

		var country = from _conutry in arr_country
			select _conutry.m_country;

		var quary = name.Concat (country);
			
		foreach (var person in quary) {
			Debug.Log(person);
		}
	}
}

// Output
"A"
"B"
"C"
"korea"
"canada"
"usa"
```

## SequenceEqual

+ 두 대상이 동일한지 판단

### [`사용 예시`]

```csharp
Person[] arr_person = new Person[] {
	new Person("A"),
	new Person("B"),
	new Person("C"),
};

Country[] arr_country = new Country[] {
	new Country("korea"),
	new Country("canada"),
	new Country("usa"),
};

var name = from person in arr_person
			select person.m_name;

var country = from _conutry in arr_country
	select _conutry.m_country;

Debug.Log (name.SequenceEqual (country));
Debug.Log (name.SequenceEqual (name));

// Ouput
"False"
"True"
```

## Ordering Operators

+ 순서 설정

### [`사용 방법`]

`orderby [정렬 조건] [ascending/descending]`

+ 기본으로 ascending(오름차순) 제공.

### [`사용 예시`]

```csharp
class Person {
	public string m_name;
	public int m_n_age;

	public Person(string name, int n_age) {
		m_name = name;
		m_n_age = n_age;
	}
}

public class A : MonoBehaviour {
	void Start () {
		Person[] arr_person = new Person[] {
			new Person("A", 11),
			new Person("B", 23),
			new Person("C", 17),
		};

		var names = from person in arr_person
			orderby person.m_n_age descending
					select new {Name = person.m_name, Age = person.m_n_age};
		
		foreach (var name in names) {
			Debug.Log(name);
		}
	}
}

// Ouput
"{ Name = B, Age = 23 }"
"{ Name = C, Age = 17 }"
"{ Name = A, Age = 11 }"
```

## Partitioning Operators

### [`사용 방법`]

+ `Take(Count)` - 처음부터 카운트 개수만큼의 값을 반환.
+ `Skip(Count)` - 처음부터 카운트 개수만큼의 값들을 제외한 나머지를 반환.
+ `TakeWhile(조건)` - 조건이 false가 되기 전까지의 값들을 반환하고 나머지는 건너뛴다.
+ `SkipWhile(조건)` - 조건이 false가 된 이후부터의 값들을 조건 검사 없이 모두 반환한다.

### [`사용 예시`]

```csharp
List<String> Days = new List<string>() {  
	"Monday", "Tuesday", "Wednesday","Thrusday", "Friday", "Saturday","Sunday"  
}; 

// Take Example
var result = Days.Take (3);

foreach (var r in result) {
	Debug.Log(r);
}

// Output
"Monday"
"Tuesday"
"Wednesday"
```

```csharp
// Skip Example
var result = Days.Skip(3);

foreach (var r in result) {
	Debug.Log(r);
}

// Output
"Thrusday"
"Friday"
"Saturday"
"Sunday"
```

```csharp
// TakeWhile Example

// Monday는 "o"가 들어있어 True 이지만 다음 Tuesday에는 "o"가 없어 Monday까지만 반환.
var result = Days.TakeWhile((x)=>x.Contains("o"));

foreach (var r in result) {
	Debug.Log(r);
}

// Output
"Monday"
```

```csharp
// TakeSkip Example

// Monday는 "o"가 들어있어 True 이지만 다음 Tuesday에는 "o"가 없어 Monday를 제외한 나머지가 조건검사 없이 모두 반환.
var result = Days.SkipWhile((x)=>x.Contains("o"));

foreach (var r in result) {
	Debug.Log(r);
}

// Output
"Tuesday"
"Wednesday"
"Thrusday"
"Friday"
"Saturday"
"Sunday"
```

## Quantifiers 

### [`사용 방법`]

+ `All(조건)` - 모든 요소가 해당 조건에 부합하는지 판단.
+ `Any(조건)` - 하나의 요소라도 해당 조건에 부합하는지 판단.
+ `Contains(String)` - 요소들에 해당 String이 포함되어 있는지 판단.

### [`사용 예시`]

```csharp
// All Example

List<String> Days = new List<string>() {  
	"Monday", "Tuesday", "Wednesday","Thrusday", "Friday", "Saturday","Sunday"  
}; 

Debug.Log(Days.All((x)=>x.Contains("o")));
Debug.Log(Days.All((x)=>x.Contains("day")));

// Output
"False"
"True"
```

```csharp
// Any Example

Debug.Log(Days.Any((x)=>x.Contains("k")));
Debug.Log(Days.Any((x)=>x.Contains("o")));

// Output
"False"
"True"
```

```csharp
// Contains Example

Debug.Log (Days.Contains ("Monday"));
Debug.Log (Days.Contains ("Mondays"));

// Output
"True"
"False"
```