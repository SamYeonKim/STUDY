# Design Pattern

## AbstractFactory

```
using System;

namespace AbstractFactory {
    class Program {
        static void Main(string[] args) {
            
        }
    }

    // AbstractFactory
    abstract class ComputerFactory {
        public abstract Notebook CreateNotebook();
        public abstract Desktop CreateDesktop();
    }

    // ConcreteFactory
    class Samsung : ComputerFactory {
        public override Notebook CreateNotebook() {
            return new Samsung9();
        }

        public override Desktop CreateDesktop() {
            return new AllInOne();
        }
    }
    
    // ConcreteFactory
    class LG : ComputerFactory {
        public override Notebook CreateNotebook() {
            return new Gram();
        }

        public override Desktop CreateDesktop() {
            return new Slim();
        }
    }

    // AbstractProductA
    abstract class Notebook {

    }

    // AbstractProductB
    abstract class Desktop {
        public abstract void Interact(Notebook notebook);
    }

    // ConcreteProductA1
    class Samsung9 : Notebook {

    }

    // ConcreteProductB1
    class AllInOne : Desktop {
        public override void Interact(Notebook notebook) {
            Console.WriteLine(GetType().Name + " interacts with " + notebook.GetType().Name);
        }
    }

    // ConcreteProductA2
    class Gram : Notebook {

    }

    // ConcreteProductB2
    class Slim : Desktop {
        public override void Interact(Notebook notebook) {
            Console.WriteLine(GetType().Name + " interacts with " + notebook.GetType().Name);
        }
    }

    // Client
    class ComputerStore {
        Notebook m_notebook;
        Desktop m_desktop;

        public ComputerStore(ComputerFactory factory) {
            m_notebook = factory.CreateNotebook(); 
            m_desktop = factory.CreateDesktop();
        }

        public void Sell() {
            m_desktop.Interact(m_notebook);
        }
    }
}
```