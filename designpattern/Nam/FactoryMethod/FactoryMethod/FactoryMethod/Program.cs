using System;
using System.Collections.Generic;

namespace FactoryMethod {
    class Program {
        static void Main(string[] args) {
            Animal[] animals = new Animal[2];

            animals[0] = new Herbivore();
            animals[1] = new Carnivore();

            for(int n_idx = 0; n_idx < animals.Length; n_idx++) {
                Animal animal = animals[n_idx];
                Console.WriteLine("\n" + animal.GetType().Name + "---");
                for(int n_s_idx = 0; n_s_idx < animal.Beasts.Count; n_s_idx++) {
                    Beast beast = animal.Beasts[n_s_idx];
                    Console.WriteLine(" " + beast.GetType().Name);
                }
            }
        }
    }

    // Creator class
    abstract class Animal {
        List<Beast> l_beast = new List<Beast>();

        public Animal() {
            CreateBeast();
        }

        public List<Beast> Beasts {
            get { return l_beast; }
        }

        public abstract void CreateBeast();
    }

    class Herbivore : Animal {
        public override void CreateBeast() {
            Beasts.Add(new Rabbit());
            Beasts.Add(new Pig());
        }
    }

    class Carnivore : Animal {
        public override void CreateBeast() {
            Beasts.Add(new Tiger());
            Beasts.Add(new Leopard());
        }
    }

    // Product class
    abstract class Beast {

    }

    class Rabbit : Beast {

    }

    class Pig : Beast {

    }

    class Tiger : Beast {

    }

    class Leopard : Beast {

    }
}
