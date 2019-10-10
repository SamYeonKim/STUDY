using System;

// ------------ 변경 전 ------------
// 대륙 - 아프리카 - 초식, 육식동물
//      - 아메리카 - 초식, 육식동물
//
// ------------ 변경 후 ------------
// 회사 - 클라이언트팀 - 대리, 사원
//      - 서버팀       - 대리, 사원


namespace AbstractFactory
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// Create and run the African animal world
			Company ClientTeam = new ClientTeam();
			Department department = new Department(ClientTeam);
			department.RunOrderChain();

			// Create and run the American animal world
			Company ServerTeam = new ServerTeam();
			department = new Department(ServerTeam);
			department.RunOrderChain();
		}
				
	}

	/// <summary>
	/// The 'AbstractFactory' abstract class
	/// </summary>
	abstract class Company
	{
		public abstract Staff CreateStaff();
		public abstract AssistantManager CreateAssistantManager();
	}

	/// <summary>
	/// The 'ConcreteFactory1' class
	/// </summary>
	class ServerTeam : Company
	{
		public override Staff CreateStaff()
		{
			return new Geunhye();
		}
		public override AssistantManager CreateAssistantManager()
		{
			return new Soonsil();
		}
	}

	/// <summary>
	/// The 'ConcreteFactory2' class
	/// </summary>
	class ClientTeam : Company
	{
		public override Staff CreateStaff()
		{
			return new Gijung();
		}
		public override AssistantManager CreateAssistantManager()
		{
			return new Samyeon();
		}
	}

	/// <summary>
	/// The 'AbstractProductA' abstract class
	/// </summary>
	abstract class Staff
	{
	}

	/// <summary>
	/// The 'AbstractProductB' abstract class
	/// </summary>
	abstract class AssistantManager
	{
		public abstract void Order(Staff s);
	}

	/// <summary>
	/// The 'ProductA1' class
	/// </summary>
	class Gijung : Staff
	{
	}

	/// <summary>
	/// The 'ProductB1' class
	/// </summary>
	class Samyeon : AssistantManager
	{
		public override void Order(Staff s)
		{
			// Eat Wildebeest
			Console.WriteLine(this.GetType().Name +
				" order " + s.GetType().Name);
		}
	}

	/// <summary>
	/// The 'ProductA2' class
	/// </summary>
	class Geunhye : Staff
	{
	}

	/// <summary>
	/// The 'ProductB2' class
	/// </summary>
	class Soonsil : AssistantManager
	{
		public override void Order(Staff s)
		{
			// Eat Bison
			Console.WriteLine(this.GetType().Name +
				" order " + s.GetType().Name);
		}
	}

	/// <summary>
	/// The 'Client' class 
	/// </summary>
	class Department
	{
		private Staff _staff;
		private AssistantManager _assistManager;

		// Constructor
		public Department(Company company)
		{
			_staff = company.CreateStaff();
			_assistManager = company.CreateAssistantManager();
		}

		public void RunOrderChain()
		{
			_assistManager.Order(_staff);
		}
	}
}
