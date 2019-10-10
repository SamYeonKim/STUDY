using System;
using System.Collections.Generic;

namespace Observer {
    class Program {
        static void Main(string[] args) {
            Windows windows = new Windows("Windows", "Click");
            windows.AddHandler(new HandlerA());
            windows.AddHandler(new HandlerB());

            windows.Type = "Hold";
            windows.Type = "Pressed";
        }
    }

    // Subject class
    abstract class GUI {
        string name;
        string type;
        List<IHandler> l_buttons = new List<IHandler>();

        public GUI(string _name, string _type) {
            name = _name;
            type = _type;
        }

        public void AddHandler(IHandler handler) {
            l_buttons.Add(handler);
        }
        
        public void RemoveHandler(IHandler handler) {
            l_buttons.Remove(handler);
        }

        public void Notify() {
            for(int n_idx = 0; n_idx < l_buttons.Count; n_idx++) {
                IHandler handler = l_buttons[n_idx];
                handler.Update(this);
            }
        }
        
        public string Type {
            get { return type;}
            set {
                if (type != value) {
                    type = value;
                    Notify();
                }
            }
        }
        
        public string Name {
            get { return name; }
        }
    }

    // Concrete Subject class
    class Windows : GUI {
        public Windows(string _name, string _type) : base (_name, _type) {
        
        }
    }

    // Obsever class
    interface IHandler {
        void Update(GUI gui);
    }

    // Concrete Observer class
    class HandlerA : IHandler {
        public void Update(GUI gui) {
            Console.WriteLine("Notify : {0} of {1} change to {2}", GetType().Name, gui.Name, gui.Type);
        }
    }

    class HandlerB : IHandler {
        public void Update(GUI gui) {
            Console.WriteLine("Notify : {0} of {1} change to {2}", GetType().Name, gui.Name, gui.Type);
        }
    }
}
