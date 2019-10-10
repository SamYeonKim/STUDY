using System;
using System.Collections.Generic;

// 기존의 문서(보고서, 이력서)내의 Page내용들에서 -> 동물원(사자, 토끼)의 행동으로 변경.


namespace FactoryMethod
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Zoo[] Animals = new Zoo[2];

			Animals[0] = new Lion();
			Animals[1] = new Rabbit();

			foreach(Zoo animals in Animals)
			{
				Console.WriteLine("\n" + animals.GetType().Name + "--");
				foreach (Behavior _behavior in animals.behavior)
				{
					Console.WriteLine(" " + _behavior.GetType().Name);
				}
			}
		}
	}

	/// <summary>
	/// The 'Product' abstract class
	/// </summary>
	abstract class Behavior
	{
	}

	/// <summary>
	/// A 'ConcreteProduct' class
	/// </summary>
	class Hunting : Behavior		// 사냥
	{
	}

	/// <summary>
	/// A 'ConcreteProduct' class
	/// </summary>
	class Pluck : Behavior			// 풀 뜯는~
	{
	}

	/// <summary>
	/// The 'Creator' abstract class
	/// </summary>
	abstract class Zoo
	{
		private List<Behavior> _behavior = new List<Behavior>();

		// Constructor calls abstract Factory method
		public Zoo()
		{
			this.CreateBehavior();
		}

		public List<Behavior> behavior
		{
			get { return _behavior; }
		}

		// Factory Method
		public abstract void CreateBehavior();
	}

	/// <summary>
	/// A 'ConcreteCreator' class
	/// </summary>
	class Lion : Zoo
	{
		// Factory Method implementation
		public override void CreateBehavior()
		{
			behavior.Add(new Hunting());
		}
	}

	/// <summary>
	/// A 'ConcreteCreator' class
	/// </summary>
	class Rabbit : Zoo
	{
		// Factory Method implementation
		public override void CreateBehavior()
		{
			behavior.Add(new Pluck());
		}
	}
}