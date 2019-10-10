using System;

namespace Decorator {
    class Program {
        static void Main(string[] args) {
            Coffee coffee = new Americano();
            coffee.Blended();
            coffee.Result();

            CoffeeBlender lattee = new MilkBlend(coffee);
            lattee.Blended();
            lattee.Result();

            CoffeeBlender capuccino = new FoamBlend(lattee);
            capuccino.Blended();
            capuccino.Result();
        }
    }

    // Component
    abstract class Coffee {
        public abstract void Blended();
        public abstract void Result();
    }

    // ConcreteComponent
    class Americano : Coffee {
        public override void Blended() {
            Console.WriteLine("Material : " + GetType().Name);
        }

        public override void Result() {
            Console.WriteLine("Beverage : Espresso\n");
        }
    }

    // Decorator
    abstract class CoffeeBlender : Coffee {
        protected Coffee coffee;

        public CoffeeBlender(Coffee _coffee) {
            coffee = _coffee;
        }

        public override void Blended() {
            coffee.Blended();
        }

        public abstract void AddMaterial();
    }

    // Concrete Decorator1
    class MilkBlend : CoffeeBlender {
        public MilkBlend(Coffee _coffee) : base (_coffee) {
        }

        public override void Blended() {
            base.Blended();
            AddMaterial();
        }

        public override void Result() {
            Console.WriteLine("Beverage : Latte\n");
        }

        public override void AddMaterial() {
            Console.WriteLine("Add Material : " + GetType().Name);
        }
    }

    // Concrete Decorator2
    class FoamBlend : CoffeeBlender {
        public FoamBlend(Coffee _coffee) : base (_coffee) {
        }

        public override void Blended() {
            base.Blended();
            AddMaterial();
        }

        public override void Result() {
            Console.WriteLine("Beverage : Cappuccino\n");
        }

        public override void AddMaterial() {
            Console.WriteLine("Add Material : " + GetType().Name);
        }
    }
}
