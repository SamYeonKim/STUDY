using System;
using System.Collections.Generic;

/// ProtoType이란?
/// 새로운 객체를 생성하기 위해 만들어 놓은 객체를 복사하여 사용하는 방식 

class MainApp {
	static void Main() {
		List<Phone> l_phone = new List<Phone> ();

		l_phone.Add (new Iphone ("iphone 7"));
		l_phone.Add (new Iphone ("iphone 7+"));
		l_phone.Add (new Iphone ("iphone 8"));
		l_phone.Add (new Iphone ("iphone 8+"));

		Phone p = l_phone.Find ((x) => x.m_name == "iphone 7").Clone();
	}
}

/// <summary>
/// ProtoType
/// </summary>
public abstract class Phone {
	public string m_name;	// 휴대폰 이름
	public abstract Phone Clone(); 
}

/// <summary>
/// The 'ConcretePrototype' class
/// </summary>
public class Iphone : Phone {
	public Iphone (string name) {
		m_name = name; 
	}

	public override Phone Clone() {
		return this;
	}
}