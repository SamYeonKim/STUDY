using System;

namespace Adapter
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Student gijung = new Adapter ("이기정");
			gijung.Information ();

			Student mail = new Adapter ("우편함");
			mail.Information ();

			Student lineage = new Adapter ("리니지");
			lineage.Information ();

			// Adapter를 사용하지 않았더라면?? => 이기정이라는 학생의 정보를 찾으려고 이런 코드를....
//			StudentData data = new StudentData();
//			string studentName = "이기정";
//			string studentClassName = data.FindStudentClassName (studentName);
//			int studentClassNumber = data.FindStudentClassNumber (studentName);
//			string studentHPNumber = data.FindStudentHPNumber (studentName);
//
//			Console.WriteLine ("[" + studentName + "] 학생의 정보");
//			Console.WriteLine ("반  이름 : " + studentClassName);
//			Console.WriteLine ("반  번호 : " + studentClassNumber);
//			Console.WriteLine ("전화번호 : " + studentHPNumber + "\n");
		}
	}


	// Target
	class Student
	{
		public string studentName;
		
		public Student(){}
		public Student(string name)
		{
			studentName = name;
		}

		public virtual void Information()
		{
			Console.WriteLine ("["+studentName + "] 학생의 정보");
		}
	}

	// Adapter
	class Adapter : Student
	{
		string className;		// 반 이름
		int classNumber;		// 반 번호
		string studentHPNumber;	// 전화번호

		StudentData sd = new StudentData();

		public Adapter(string name) : base(name)
		{}
		
		public override void Information()
		{
			className = sd.FindStudentClassName (studentName);
			classNumber = sd.FindStudentClassNumber (studentName);
			studentHPNumber = sd.FindStudentHPNumber (studentName);

			base.Information ();
			Console.WriteLine ("반  이름 : " + className);
			Console.WriteLine ("반  번호 : " + classNumber);
			Console.WriteLine ("전화번호 : " + studentHPNumber + "\n");
		}
	}

	// Adaptee
	class StudentData
	{
		public string FindStudentClassName(string name)
		{
			string className = null;

			switch (name) 
			{
			case "이기정":
				className = "햇님반";
				break;

			case "우편함":
				className = "달님반";
				break;

			case "리니지":
				className = "레볼루션반";
				break;
			}
			return className;
		}

		public int FindStudentClassNumber(string name)
		{
			int classNumber = 0;

			switch (name) 
			{
			case "이기정":
				classNumber = 1;
				break;

			case "우편함":
				classNumber = 2;
				break;

			case "리니지":
				classNumber = 3;
				break;
			}
			return classNumber;
		}

		public string FindStudentHPNumber(string name)
		{
			string studentHPNumber = null;

			switch (name) 
			{
			case "이기정":
				studentHPNumber = "010-5298-0874";
				break;

			case "우편함":
				studentHPNumber = "010-8282-8282";
				break;

			case "리니지":
				studentHPNumber = "010-1688-5588";
				break;
			}
			return studentHPNumber;
		}
	}
}
