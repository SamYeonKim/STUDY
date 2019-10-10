using System;
using System.Collections.Generic;

namespace Visitor
{
	class MainClass
	{
		// Client
		static void Main()
		{
			Teacher doraemon = new Teacher ();
			doraemon.AddStudents(new Gijung());
			doraemon.AddStudents(new Soonsil());

			doraemon.ShowInfo (new ShowName ());
			doraemon.ShowInfo (new ShowHP ());
		}
	}

	// Visitor
	public abstract class StudentInfo
	{
		public abstract void Display(Student student);
	}

	// ConcreteVisitorA
	class ShowName : StudentInfo
	{
		public override void Display (Student student)
		{
			Console.WriteLine ("이름 : " + student.name);
		}
	}

	// ConcreteVisitorB
	class ShowHP : StudentInfo
	{
		public override void Display (Student student)
		{
			Console.WriteLine ("HP : " + student.hp);
		}
	}

	// ObjectStructure
	public class Teacher
	{
		List<Student> my_class_students = new List<Student>();

		public void AddStudents(Student student)
		{
			my_class_students.Add (student);
		}

		public void ShowInfo(StudentInfo info)
		{
			for (int i = 0; i < my_class_students.Count; i++) 
			{
				my_class_students [i].ShowStudentInfo (info);
			}
		}
	}

	// Element
	public class Student
	{
		public string name;
		public string hp;

		public Student(string name, string hp)
		{
			this.name = name;
			this.hp = hp;
		}

		public void ShowStudentInfo(StudentInfo info)
		{
			info.Display (this);
		}
	}

	// ConcreteElementA
	public class Gijung : Student
	{
		public Gijung () : base ("이기정", "010-5298-0874"){}
	}

	// ConcreteElementB
	public class Soonsil : Student
	{
		public Soonsil () : base ("최순실", "010-8282-82828"){}
	}
}
