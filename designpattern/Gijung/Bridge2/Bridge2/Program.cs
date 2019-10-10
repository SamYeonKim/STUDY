using System;

// Bridge pattern은 추상과 구현을 분리해놓음으로써 서로 독립적으로 concrete를 만들며 다양해질 수 있다.

namespace Bridge2
{
	class MainClass
	{
		public static void Main()
		{
			GyeonggiMachine machine = new GyeonggiMachine("경기도 자판기");
			machine.Data = new Coke();

			machine.Add();
			machine.Display();
		}

		// Abstract
		class VendingMachine
		{
			public string vendingM;	// 자판기 이름
			public Beverage beverage;

			public VendingMachine(string machineName)
			{
				vendingM = machineName;
			}

			public Beverage Data
			{
				set{beverage = value;}
			}

			public virtual void Add()
			{
				beverage.AddBeverage();
			}


			public virtual void Display()
			{
				Console.WriteLine("[" + vendingM + "]의 음료수 현황\n");

				beverage.BeverageCount();
			}
		}

		// Concrete Abstract
		class GyeonggiMachine : VendingMachine
		{
			public GyeonggiMachine(string name) : base(name)
			{
			}

			public override void Display()
			{
				Console.WriteLine("---------------------------------------");
				base.Display ();
				Console.WriteLine("---------------------------------------");
			}

		}

		// Implementor
		abstract class Beverage
		{
			protected int beverageCount;

			public abstract void AddBeverage();
			public abstract void BeverageCount();

		}

		class Coke : Beverage
		{
			public override void AddBeverage()
			{
				beverageCount++;
			}

			public override void BeverageCount()
			{
				Console.WriteLine("이 자판기에 콜라가 " + beverageCount + "개 들어있습니다.");
			}
		}

		class Fanta : Beverage
		{
			public override void AddBeverage()
			{
				beverageCount++;
			}

			public override void BeverageCount()
			{
				Console.WriteLine("이 자판기에 환타가 " + beverageCount + "개 들어있습니다.");
			}
		}
	}
}
