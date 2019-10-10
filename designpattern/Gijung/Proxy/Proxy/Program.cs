using System;

namespace Proxy
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			InternetBanking sinhan_internet_banking = new InternetBanking (01052980874);

			sinhan_internet_banking.Deposit (100000);
			sinhan_internet_banking.Transfer (123456789, 100000);
		}
	}

	// Subject
	public abstract class Bank
	{
		public abstract void Deposit(int money);								// 예금
		public abstract void Transfer(int send_account_number, int money);		// 이체
	}

	// Real Subject
	class SinhanBank : Bank
	{
		int account_number;	// 계좌번호

		public SinhanBank(int account_number)
		{
			this.account_number = account_number;
		}

		public override void Deposit (int money)
		{
			Console.WriteLine ("계좌번호 [" + account_number + "]에 " + money + "이 예금되었습니다.");
		}

		public override void Transfer (int send_account_number, int money)
		{
			Console.WriteLine ("계좌번호 [" + account_number + "]에서 [" + send_account_number + "]로 " + money + "만큼 이체되었습니다.");
		}
	}

	// Proxy
	public class InternetBanking : Bank
	{
		SinhanBank my_sinhan;

		public InternetBanking(int account_number)
		{
			my_sinhan = new SinhanBank (account_number);
		}

		public override void Deposit (int money)
		{
			my_sinhan.Deposit (money);
		}

		public override void Transfer (int send_account_number, int money)
		{
			my_sinhan.Transfer (send_account_number, money);
		}
	}

}
