using System;

namespace Command {

    // Client Class
    class Program {
        static void Main(string[] args) {
            Light light = new Light();
            LightOn light_on = new LightOn(light);
            LightOff light_off = new LightOff(light);

            RemoteControl remote = new RemoteControl();
            remote.SetCommand(light_on, light_off);

            remote.OnPressLightOn();
            remote.OnPressLightOff();
        }
    }

    // Command Class
    abstract class Command {
        protected Light light;

        public Command(Light _light) {
            light = _light;
        } 

        public abstract void Excute();
    }

    // Concrete Command class 1
    class LightOn : Command {
        public LightOn(Light light) : base(light) {

        }

        public override void Excute() {
            light.LightOn();
        }
    }

    // Concrete Command class 2
    class LightOff : Command {
        public LightOff(Light light) : base(light) {

        }

        public override void Excute() {
            light.LightOff();
        }
    }

    // Receiver Class
    class Light {
        public void LightOn() {
            Console.WriteLine("-- Light On !!! --");
        }

        public void LightOff() {
            Console.WriteLine("-- Light Off !!! --");
        }
    }

    // Invoker Class
    class RemoteControl {
        Command on_command;
        Command off_command;

        public void SetCommand(Command _on_command, Command _off_command) {
            on_command = _on_command;
            off_command = _off_command;
        }

        public void OnPressLightOn() {
            on_command.Excute();
        }

        public void OnPressLightOff() {
            off_command.Excute();
        }
    }
}
