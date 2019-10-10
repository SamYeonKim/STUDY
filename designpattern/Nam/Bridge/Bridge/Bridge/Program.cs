using System;

namespace Bridge {
    class Program {
        static void Main(string[] args) {
            Device device = new SmartPhone(new Unit1());
            device.ReadData();
            device.ShowData();

            device = new SmartPhone(new Unit2());
            device.ReadData();
            device.ShowData();
        }
    }

    // Implementor
    abstract class ReadUnit {
        public abstract string ReadData();
    }

    // ConcreteImplementor
    class Unit1 : ReadUnit {
        public override string ReadData() {
            return "Read Data : " + GetType().Name;
        }
    }

    class Unit2 : ReadUnit {
        public override string ReadData() {
            return "Read Data : " + GetType().Name;
        }
    }

    // Abstraction
    abstract class Device {
        public ReadUnit read_unit
        public abstract void ReadData();
        public abstract void ShowData();
    }

    // RefinedAbstraction
    class SmartPhone : Device {
        string data;

        public SmartPhone(ReadUnit unit) {
            read_unit = unit;
        }

        public override void ReadData() {
            data = read_unit.ReadData();
        }

        public override void ShowData() {
            Console.WriteLine(data);
        }
    }
}
