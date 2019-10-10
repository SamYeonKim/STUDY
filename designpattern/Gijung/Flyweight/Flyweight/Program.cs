using System;
using System.Collections.Generic;

namespace Flyweight
{
	public enum Wheel
	{
		one,
		two,
		three
	}

	class MainClass
	{
		public static void Main (string[] args)
		{
			BicycleRental samchuly = new BicycleRental ();

			samchuly.Rental (Wheel.one);	// 현재 한대 밖에 없는 한발 자전거를 빌립니다.
			samchuly.Return (Wheel.one);	// 한발 자전거를 반납합니다.
			samchuly.Rental (Wheel.one);	// 다시 한발 자전거를 빌립니다.

			Console.WriteLine ("-----");

			samchuly.Rental (Wheel.two);	// 현재 한대 밖에 없는 두발 자전거를 빌립니다.
			samchuly.Rental (Wheel.two);	// 현재 한대도 없는 두발 자전거를 빌립니다.
			samchuly.Rental (Wheel.two);	// 현재 한대도 없는 두발 자전거를 빌립니다.
		}
	}

	// FlyweightFactory
	public class BicycleRental
	{
		List<Bicycle> rental_bicycle = new List<Bicycle> ();	// 빌려줄 수 있는 자전거
		List<Bicycle> return_bicycle = new List<Bicycle> ();	// 빌려간 자전거

		public BicycleRental()
		{
			rental_bicycle.Add (new OneWheel ());
			rental_bicycle.Add (new TwoWheels ());
			rental_bicycle.Add (new ThreeWheels ());
		}

		public void Rental(Wheel name)
		{
			for (int i = 0; i < rental_bicycle.Count; i++) 
			{
				if (rental_bicycle [i].bicycle_name == name) {
					return_bicycle.Add (rental_bicycle [i]);
					rental_bicycle.RemoveAt (i);

					Console.WriteLine ("빌리려고 하는 [ 자전거 ] 여기 한대 남아있네.");
					return;
				} 
			}

			switch (name) 
			{
			case Wheel.one:
				return_bicycle.Add (new OneWheel ());
				break;

			case Wheel.two:
				return_bicycle.Add (new TwoWheels ());
				break;

			case Wheel.three:
				return_bicycle.Add (new ThreeWheels ());
				break;
			}

			Console.WriteLine ("빌리려고 하는 [ 자전거 ] 가 없어져서 하나 사왔어.");
		}

		public void Return(Wheel name)
		{
			for (int i = 0; i < return_bicycle.Count; i++) 
			{
				if (return_bicycle [i].bicycle_name == name) {
					rental_bicycle.Add (return_bicycle [i]);
					return_bicycle.RemoveAt (i);

					Console.WriteLine ("[ 자전거 ] 돌려줘서 고마워");
					break;
				} 
			}
		}
	}

	// Flyweight
	public class Bicycle
	{
		public Wheel bicycle_name;
	}

	// ConcreteFlyweight A
	public class OneWheel : Bicycle
	{
		public OneWheel()
		{
			bicycle_name = Wheel.one;
		}
	}

	// ConcreteFlyweight B
	public class TwoWheels : Bicycle
	{
		public TwoWheels()
		{
			bicycle_name = Wheel.two;
		}
	}

	// ConcreteFlyweight C
	public class ThreeWheels : Bicycle
	{
		public ThreeWheels()
		{
			bicycle_name = Wheel.three;
		}
	}
}
