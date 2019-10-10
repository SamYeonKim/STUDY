using System;

namespace ChainOfResponsibility
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			DetectiveAgency agencyA = new DetectiveAgencyA ();
			DetectiveAgency agencyB = new DetectiveAgencyB ();

			agencyA.ChainAgency (agencyB);

			agencyA.FindPerson ("이기정");
			agencyA.FindPerson ("김봉남");
			agencyA.FindPerson ("김정일");
		}
	}

	// Handler
	class DetectiveAgency
	{
		public DetectiveAgency agency;

		public void ChainAgency(DetectiveAgency chain_agency)
		{
			agency = chain_agency;
		}

		public virtual void FindPerson(string find_person_name){}
	}

	// ConcreteHandlerA
	class DetectiveAgencyA : DetectiveAgency
	{
		public override void FindPerson (string find_person_name)
		{
			if (find_person_name == "이기정") 
			{
				Console.WriteLine ("우리 A 흥신소에서 [" + find_person_name + "] 를 찾았어.");
			} 
			else 
			{
				agency.FindPerson (find_person_name);
			}
		}
	}

	// ConcreteHandlerB
	class DetectiveAgencyB : DetectiveAgency
	{
		public override void FindPerson (string find_person_name)
		{
			if (find_person_name == "김봉남") 
			{
				Console.WriteLine ("우리 B 흥신소에서 [" + find_person_name + "] 를 찾았어.");
			} 
			else 
			{
				Console.WriteLine ("["+find_person_name + "] 이 사람 한국을 떴구만.");
			}
		}
	}
}
