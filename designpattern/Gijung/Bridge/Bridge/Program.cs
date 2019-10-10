using System;

namespace Bridge
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Student gijung = new Student ("이기정");

			gijung.DATA = new Gijung ();

			Console.WriteLine ("학생 이름 : " + gijung.objName);
			gijung.ShowName ();
			gijung.ShowNumber ();

			Student soonsil = new Student ("최순실");

			soonsil.DATA = new Soonsil ();

			Console.WriteLine ("학생 이름 : " + soonsil.objName);
			soonsil.ShowName ();
			soonsil.ShowNumber ();
		}
	}

	// Abstarction
	class Student
	{
		private DataObject obj;
		public string objName;

		public Student(){}
		public Student(string name)
		{
			objName = name;
		}

		public DataObject DATA
		{
			get {
				return obj;
			}
			set{
				obj = value;
			}
		}

		public virtual void ShowName()
		{
			obj.ShowClassName ();
		}

		public virtual void ShowNumber()
		{
			obj.ShowClassNumber ();
		}
	}

	// Implementor
	abstract class DataObject
	{
		public abstract void ShowClassNumber();
		public abstract void ShowClassName();

	}

	// ConcreteImplementorA
	class Gijung : DataObject
	{
		public override void ShowClassNumber()
		{
			Console.WriteLine ("반 번호   : 1반\n");
		}

		public override void ShowClassName()
		{
			Console.WriteLine ("반 이름   : 햇님반");
		}
	}

	// ConcreteImplementorB
	class Soonsil : DataObject
	{
		public override void ShowClassNumber()
		{
			Console.WriteLine ("반 번호   : 3반");
		}

		public override void ShowClassName()
		{
			Console.WriteLine ("반 이름   : 별님반");
		}
	}
}
