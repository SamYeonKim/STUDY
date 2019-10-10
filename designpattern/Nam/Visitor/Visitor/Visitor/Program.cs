using System;
using System.Collections.Generic;

namespace Visitor {
    class Program {
        static void Main(string[] args) {
            Shopper shopper = new Shopper();
            shopper.Add(new Meat(1000));
            shopper.Add(new Vegetable(500));

            shopper.Accept(new DiscountVisitor());
        }
    }

    // Abstract Visitor class
    public interface IVisitor {
        void Visit(Products product);
    }

    // Concrete Visitor class
    public class DiscountVisitor : IVisitor {
        public void Visit(Products product) {
            int n_discount_factor = 10;
            int n_org_price = product.Price;
            int n_discount_price = n_org_price / n_discount_factor;

            product.Price -= n_discount_price;

            Console.WriteLine("Visit Discount !!!");
            Console.WriteLine("{0} : {1} -> {2}\n", product.GetType().Name, n_org_price, product.Price);
        }
    }

    // Abstract Element class
    public abstract class Products {
        int price;

        public Products(int _price) {
            price = _price;
        }

        public int Price {
            get { return price; }
            set { price = value; }
        }

        public abstract void Accept(IVisitor visitor);
    }

    // Concrete Element class
    class Meat : Products {
        public Meat(int price) : base(price) {
        }

        public override void Accept(IVisitor visitor) {
            visitor.Visit(this);
        }
    }

    class Vegetable : Products {
        public Vegetable(int price) : base(price) {
        }

        public override void Accept(IVisitor visitor) {
            visitor.Visit(this);
        }
    }

    // ObjectStructure class
    public class Shopper {
        List<Products> l_products = new List<Products>();

        public void Add(Products product) {
            l_products.Add(product);
        }

        public void Remove(Products product) {
            l_products.Remove(product);
        }

        public void Accept(IVisitor visitor) {
            foreach(Products product in l_products) {
                product.Accept(visitor);
            }
        }
    }
}
