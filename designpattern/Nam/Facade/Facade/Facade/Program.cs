using System;
using System.Collections.Generic;
using System.Linq;
namespace Facade {
    class Program {
        static void Main(string[] args) {
            Computer computer = new Computer();
            computer.StartComputer();
        }
    }

    // Facade class
    class Computer {
        CPU cpu;
        GPU gpu;
        Memory memory;
        HardDrive hard_drive;

        public Computer() {
            cpu = new CPU();
            gpu = new GPU();
            memory = new Memory();
            hard_drive = new HardDrive();
        }

        public void StartComputer() {
            cpu.Init();
            gpu.UpdateView();
            cpu.Jump();
            memory.Load();
            hard_drive.ReadData();
            cpu.Excute();
        }
    }

    class CPU {
        public void Excute() {
            Console.WriteLine("Excute");
        }

        public void Jump() {
            Console.WriteLine("Jump");
        }

        public void Init() {
            Console.WriteLine("Initialized");
        }
    }

    class GPU {
        public void UpdateView() {
            Console.WriteLine("UpdateView");
        }
    }

    class Memory {
        public void Load() {
            Console.WriteLine("Load");
        }
    }

    class HardDrive {
        public void ReadData() {
            Console.WriteLine("ReadData");
        }

        public void WriteData() {
            Console.WriteLine("WriteData");
        }
    }
}
