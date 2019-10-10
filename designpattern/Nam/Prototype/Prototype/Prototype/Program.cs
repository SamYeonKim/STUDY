using System;

namespace Prototype {
    class Program {
        static void Main(string[] args) {
            Character char1 = new Character1("noname1");
            Character c_char1 = (Character1)char1.Clone();
            Console.WriteLine("Cloned : {0}, {1}", char1.GetName, char1.GetType().Name);

            Character char2 = new Character1("noname2");
            Character c_char2 = (Character1)char2.Clone();
            Console.WriteLine("Cloned : {0}, {1}", char2.GetName, char2.GetType().Name);
        }
    }

    // Protytype class
    abstract class Character {
        string name;

        public Character(string char_name) {
            name = char_name;
        }

        public string GetName {
            get { return name; }
        }

        public abstract Character Clone();
    }

    class Character1 : Character {
        public Character1(string name) : base(name) { }

        public override Character Clone() {
            return (Character)MemberwiseClone();
        }
    }
}
