using System;
using System.Collections.Generic;

// 개별의 object과 composite을 하나의 인터페이스(component)에 둠으로서 사용에 용이하도록 만든 패턴.

namespace Composite
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Relation myFamily = new Relation ("이기정");

			myFamily.Add (new Person ("아버지"));
			myFamily.Add (new Person ("어머니"));
			myFamily.Add (new Person ("형"));

			myFamily.Display ();
		}
	}

	// Component
	abstract class Component
	{
		protected string relationName;

		public Component (){}
		public Component(string name)
		{
			relationName = name;
		}

		public virtual void Add (Component c)
		{
		}
		public virtual void Remove (Component c)
		{
		}
		public abstract void Display ();
	}

	// Leaf
	class Person : Component
	{
		public Person(string name) : base(name)
		{
		}

		public override void Display ()
		{
			Console.WriteLine (relationName);
		}
	}

	// Composite
	class Relation : Component
	{
		List<Component> family = new List<Component>();

		public Relation(string name) : base(name)
		{
		}

		public override void Add (Component c)
		{
			family.Add (c);
		}

		public override void Remove (Component c)
		{
			family.Remove (c);
		}

		public override void Display ()
		{
			Console.WriteLine ("[ " + relationName + "의 가족 ]");

			for (int i = 0; i < family.Count; i++) {
				family [i].Display ();
			}
		}
	}
}
