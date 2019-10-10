using System;
using System.Collections.Generic;

// 차량에 관한 Builder -> 학생에 관한 Builder

namespace Builder
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			StudentBuilder builder;

			// Create shop with vehicle builders
			Director director = new Director();

			// Construct and display vehicles
			builder = new GijungBuilder();
			director.Construct(builder);
			builder.Student.Show();

			builder = new SoonsilBuilder();
			director.Construct(builder);
			builder.Student.Show();
		}
	}

	/// <summary>
	/// The 'Director' class
	/// </summary>
	class Director
	{
		// Builder uses a complex series of steps
		public void Construct(StudentBuilder studentBuilder)
		{
			studentBuilder.BuildGrade();
			studentBuilder.BuildClass();
		}
	}

	/// <summary>
	/// The 'Builder' abstract class
	/// </summary>
	abstract class StudentBuilder
	{
		protected Student student;

		// Gets vehicle instance
		public Student Student
		{
			get { return student; }
		}

		// Abstract build methods
		public abstract void BuildGrade();
		public abstract void BuildClass();
	}

	/// <summary>
	/// The 'ConcreteBuilder1' class
	/// </summary>
	class GijungBuilder : StudentBuilder
	{
		public GijungBuilder()
		{
			student = new Student("Gijung");
		}

		public override void BuildGrade()
		{
			Student["grade"] = "1학년";
		}

		public override void BuildClass()
		{
			Student["class"] = "5반";
		}
	}


	/// <summary>
	/// The 'ConcreteBuilder2' class
	/// </summary>
	class SoonsilBuilder : StudentBuilder
	{
		public SoonsilBuilder()
		{
			student = new Student("Soonsil");
		}

		public override void BuildGrade()
		{
			Student["grade"] = "3학년";
		}

		public override void BuildClass()
		{
			Student["class"] = "12반";
		}
	}

	/// <summary>
	/// The 'Product' class
	/// </summary>
	class Student
	{
		private string _studentName;
		private Dictionary<string,string> _parts = 
			new Dictionary<string,string>();

		// Constructor
		public Student(string studentName)
		{
			this._studentName = studentName;
		}

		// Indexer
		public string this[string key]
		{
			get { return _parts[key]; }
			set { _parts[key] = value; }
		}

		public void Show()
		{
			Console.WriteLine("\n---------------------------");
			Console.WriteLine("Student Name  : {0}", _studentName);
			Console.WriteLine("Student Grade : {0}", _parts["grade"]);
			Console.WriteLine("Studetn Class : {0}", _parts["class"]);
		}
	}
}
