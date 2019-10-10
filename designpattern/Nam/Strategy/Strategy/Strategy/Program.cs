using System;
using System.Collections.Generic;

namespace Strategy {
    class Program {
        static void Main(string[] args) {
            ItemList items = new ItemList();
            items.Add(1);
            items.Add(5);
            items.Add(6);
            items.Add(10);
            items.Add(3);
            items.Add(7);

            items.SetArrange(new Forward());
            items.Arrangement();

            items.SetArrange(new Reverse());
            items.Arrangement();
        }
    }

    interface IArrange {
         void Arrange(List<int> list);
    }

    // ConcreteStrategy Class
    class Forward : IArrange {
        public void Arrange(List<int> list) {
            Console.WriteLine("Forward");
        }
    }

    // ConcreteStrategy Class
    class Reverse : IArrange {
        public void Arrange(List<int> list) {
            int n_last_idx = list.Count - 1;
            for(int n_idx = 0; n_idx < list.Count / 2; n_idx++) {
                int n_tmp = list[n_idx];
                list[n_idx] = list[n_last_idx];
                list[n_last_idx] = n_tmp;
                n_last_idx--;
            }

            Console.WriteLine("Reverse");
        }
    }

    // Context Class
    class ItemList {
        private List<int> items = new List<int>();
        private IArrange arrange_strategy;

        public void SetArrange(IArrange arrange) {
            arrange_strategy = arrange;
        }

        public void Add(int value) {
            items.Add(value);
        }

        public void Arrangement() {
            items.Sort();
            arrange_strategy.Arrange(items);

            foreach (int val in items) {
                Console.WriteLine(val + " ");
            }
            Console.WriteLine();
        }
    }
}
