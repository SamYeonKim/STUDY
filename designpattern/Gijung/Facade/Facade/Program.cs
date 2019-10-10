using System;

namespace Facade
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			ChickenRestaurant kyochon = new ChickenRestaurant ("Kyochon");

			Console.WriteLine ("손님 : 아저씨 제가 여기서 16000원으로 뭘 먹을 수 있죠?");
			kyochon.FindChicken (8000);
		}
	}

	// Facade
	class ChickenRestaurant
	{
		string restaurant_name;

		FriedChicken fried;
		SeasoningChicken seasoning;
		SpicyChicken spicy;

		public ChickenRestaurant(string name)
		{
			restaurant_name = name;

			fried = new FriedChicken ();
			seasoning = new SeasoningChicken ();
			spicy = new SpicyChicken ();
		}

		public void FindChicken(int money)
		{
			fried.isFriedEat (restaurant_name, money);
			seasoning.isSeasoningEat (restaurant_name, money);
			spicy.isSpicygEat (restaurant_name, money);
		}
	}

	// Subsystem ClassA
	class FriedChicken
	{
		int fried_price = 14500;

		public void isFriedEat(string restaurant_name, int money)
		{
			if (fried_price <= money) 
			{
				Console.WriteLine (restaurant_name + "사장 : [후라이드 치킨] 먹을 수 있겠네요.");
			} 
			else 
			{
				Console.WriteLine (restaurant_name + "사장 : 이 돈으론 [후라이드 치킨]은 안되겠네요.");
			}
		}
	}

	// Subsystem ClassB
	class SeasoningChicken
	{
		int seasoning_price = 16000;

		public void isSeasoningEat(string restaurant_name, int money)
		{
			if (seasoning_price <= money) 
			{
				Console.WriteLine (restaurant_name + "사장 : [양념 치킨] 먹을 수 있겠네요.");
			} 
			else 
			{
				Console.WriteLine (restaurant_name + "사장 : 이 돈으론 [양념 치킨]은 안되겠네요.");
			}
		}
	}

	// Subsystem ClassC
	class SpicyChicken
	{
		int spicy_price = 20000;

		public void isSpicygEat(string restaurant_name, int money)
		{
			if (spicy_price <= money) 
			{
				Console.WriteLine (restaurant_name + "사장 : [매운 치킨] 먹을 수 있겠네요.");
			} 
			else 
			{
				Console.WriteLine (restaurant_name + "사장 : 이 돈으론 [매운 치킨]은 안되겠네요.");
			}
		}
	}
}
