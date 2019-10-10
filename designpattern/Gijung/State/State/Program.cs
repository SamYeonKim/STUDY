using System;

namespace State
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Character gijung = new Character ();
			gijung.Damage (10);
			gijung.Damage (20);
			gijung.Damage (30);
			gijung.Damage (40);
		}
	}

	// Context
	public class Character
	{
		public State char_state;	

		public Character()
		{
			char_state = new GreenState(this);	// 멤버 변수 할당
		}

		public void Damage(int damage_value)
		{
			char_state.HPState(damage_value);
		}
	}

	// State
	public abstract class State
	{
		public int HP;
		public Character cha;	
	

		public State(){}
		public State(Character cha)
		{
			if (HP == 0)
				HP = 100;

			this.cha = cha;	// 멤버 변수지만 참조
		}

		public void HPState(int damage_value)
		{
			HP -= damage_value;

			cha.char_state.CharState ();
		}

		public abstract void CharState();
		public abstract void CurrentHP();
	}

	// ConcreteState
	class GreenState : State
	{
		public GreenState(){}
		public GreenState(Character cha) : base(cha)
		{
		}

		public override void CharState()
		{
			
			if (HP >= 40 && HP < 80)  //노란피
			{
				cha.char_state = new YellowState (cha);

			} 
			else if (HP < 40) // 빨간피
			{
				cha.char_state = new RedState (cha);
			}
			cha.char_state.CurrentHP ();

		}

		public override void CurrentHP()
		{
			Console.WriteLine ("현재 캐릭터의 피는 [초록피] 입니다.");
		}
	}

	// ConcreteState
	class YellowState : State
	{
		public YellowState(){}
		public YellowState(Character cha) : base(cha)
		{
		}

		public override void CharState()
		{
			if (HP < 40) {
				cha.char_state = new RedState (cha);

			} 
			cha.char_state.CurrentHP ();
		}

		public override void CurrentHP()
		{
			Console.WriteLine ("현재 캐릭터의 피는 [노란피] 입니다.");
		}
	}

	// ConcreteState
	class RedState : State
	{
		public RedState(){}
		public RedState(Character cha) : base(cha)
		{
		}

		public override void CharState()
		{
			
			CurrentHP ();
		}

		public override void CurrentHP()
		{
			Console.WriteLine ("현재 캐릭터의 피는 [빨간피] 입니다.");
		}
	}
}
