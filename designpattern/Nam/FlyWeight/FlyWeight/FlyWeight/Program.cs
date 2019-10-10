using System;
using System.Collections.Generic;

namespace FlyWeight {
    class Program {
        static void Main(string[] args) {
            Restaurant restaurant = new Restaurant();

            Dish dish = restaurant.Order("SideDish");
            dish.ShowDishState();

            dish = restaurant.Order("MainDish");
            dish.ShowDishState();

            dish = restaurant.Order("SideDish");
            dish.ShowDishState();
        }
    }

    // Flyweight Factory class
    class Restaurant {
        List<Dish> l_dish_table = new List<Dish>();

        public Restaurant() {
            l_dish_table.Add(new SideDish());
        }

        public Dish Order(string name) {
            Dish order_dish = null;

            if(l_dish_table.Exists(dish => dish.GetType().Name == name)) {
                order_dish = l_dish_table.Find(dish => dish.GetType().Name == name);
                Console.WriteLine("Existing Dish");
            } else {
                Console.WriteLine("New Dish");

                //only side dishes are reuse, the main dish is made.
                if (name == "SideDish") {
                    order_dish = new SideDish();
                    l_dish_table.Add(order_dish);
                } else if (name == "MainDish") {
                    order_dish = new MainDish();
                } else {
                    Console.WriteLine("There is no Dish");
                }
            }
            return order_dish;
        }
    }

    // Flyweight class
    abstract class Dish {
        public abstract void ShowDishState();
    }

    // Concrete Flyweight class
    class SideDish : Dish {
        public override void ShowDishState() {
            Console.WriteLine("This dish is reuse\n");
        }
    }

    // Concrete Flyweight class
    class MainDish : Dish {
        public override void ShowDishState() {
            Console.WriteLine("This dish is new\n");
        }
    }
}
