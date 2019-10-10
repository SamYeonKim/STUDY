using System;
using System.Collections.Generic;

namespace Decorator
{
	class MainClass
	{
		static void Main (string[] args)
		{
			Join kt = new Join(new KT());
			kt.join("이기정");
			kt.join("김복자");

			Join skt = new Join(new SKT());
			skt.join("김길정");
			skt.join("옥동자");

			kt.display();
			skt.display();
		}

		// Component
		abstract class mobileCarrier
		{
			public abstract void display();

		}

		// ConcreteComponent
		class KT : mobileCarrier
		{
			public override void display()
			{
				Console.WriteLine("---KT---");
			}
		}

		// ConcreteComponent
		class SKT : mobileCarrier
		{
			public override void display()
			{
				Console.WriteLine("---SKT---");
			}
		}

		// Decorator
		abstract class Decorator : mobileCarrier
		{
			mobileCarrier carrier;

			public Decorator(mobileCarrier carrier)
			{
				this.carrier = carrier;
			}

			public override void display()
			{
				carrier.display();
			}
		}

		// ConcreteDecorator
		class Join : Decorator
		{
			List<string> joinPeople = new List<string>();

			public Join(mobileCarrier carrier) : base(carrier)
			{

			}

			public void join(string name)
			{
				joinPeople.Add(name);
			}

			public override void display()
			{
				base.display();

				for(int i=0; i<joinPeople.Count; i++)
				{
					Console.WriteLine(i+1 + "번째 가입자 : " + joinPeople[i]);
				}

			}
		}
	}
}
