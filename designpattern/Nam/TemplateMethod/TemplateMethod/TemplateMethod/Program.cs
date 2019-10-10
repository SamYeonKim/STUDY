using System;

namespace TemplateMethod {
    class Program {
        static void Main(string[] args) {
            Beverage beverage = new Coffee();
            beverage.Complete();

            beverage = new BlackTea();
            beverage.Complete();
        }
    }

    // Abstract Class
    abstract class Beverage {
        void SetWater() {
            Console.WriteLine("--- Ready Water ---");
        }

        public abstract void SetIngredients();
        public abstract void SetAdditive();

        void Blending() {
            Console.WriteLine("--- Blending... ---");
        }

        // Templete Method
        public void Complete() {
            SetWater();
            SetIngredients();
            SetAdditive();
            Blending();

            Console.WriteLine("--- Complete : {0} ---\n", GetType().Name);
        } 
    }

    // Concrete Class
    class Coffee : Beverage {
        public override void SetIngredients() {
            Console.WriteLine("Ingredient : Coffee Bean");
        }

        public override void SetAdditive() {
            Console.WriteLine("Additive : None");
        }
    }

    // Concrete Class
    class BlackTea : Beverage {
        public override void SetIngredients() {
            Console.WriteLine("Ingredient : Black Tea Leaf");
        }

        public override void SetAdditive() {
            Console.WriteLine("Additive : Lemon");
        }
    }
}
