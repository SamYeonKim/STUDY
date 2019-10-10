using System;
using System.Collections.Generic;

namespace Builder {
    class Program {
        static void Main(string[] args) {
            ComputerBuilder comp_builder;

            Shop shop = new Shop();

            comp_builder = new NoteBookBuilder();
            shop.Construct(comp_builder);
            comp_builder.Computer.Show();

            comp_builder = new DesktopBuilder();
            shop.Construct(comp_builder);
            comp_builder.Computer.Show();
        }
    }

    // Builder class
    abstract class ComputerBuilder {
        protected Computer computer;

        public Computer Computer {
            get { return computer; }
        }

        public abstract void BuildOS();
        public abstract void BuildCPU();
        public abstract void BuildGPU();
    }

    // Director class
    class Shop {
        public void Construct(ComputerBuilder computer_builder) {
            computer_builder.BuildOS();
            computer_builder.BuildCPU();
            computer_builder.BuildGPU();
        }
    }

    // ConcreteBuilder1 class
    class NoteBookBuilder : ComputerBuilder {
        public NoteBookBuilder() {
            computer = new Computer("NoteBook");
        }

        public override void BuildOS() {
            computer["os"] = "Windows7";
        }

        public override void BuildCPU() {
            computer["cpu"] = "i5";
        }

        public override void BuildGPU() {
            computer["gpu"] = "gt1050";
        }
    }

    class DesktopBuilder : ComputerBuilder {
        public DesktopBuilder() {
            computer = new Computer("Desktop");
        }

        public override void BuildOS() {
            computer["os"] = "Windows10";
        }

        public override void BuildCPU() {
            computer["cpu"] = "i7";
        }

        public override void BuildGPU() {
            computer["gpu"] = "gtx1060";
        }
    }


    // Product Class
    class Computer {
        string computer_type;
        Dictionary<string, string> parts = new Dictionary<string, string>();

        public Computer(string type) {
            computer_type = type;
        }

        public string this[string key] {
            get { return parts[key]; }
            set { parts[key] = value; }
        }

        public void Show() {
            Console.WriteLine("Construct Parts ---------");
            Console.WriteLine("Computer Type : {0}", computer_type);
            Console.WriteLine("OS : {0}", parts["os"]);
            Console.WriteLine("CPU : {0}", parts["cpu"]);
            Console.WriteLine("GPU : {0}", parts["gpu"]);
        }
    }
}
