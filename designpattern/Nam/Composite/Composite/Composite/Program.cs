using System;
using System.Collections.Generic;

namespace Composite {
    class Program {
        static void Main(string[] args) {
            Tree root = new Branch("root");
            root.Add(new Leaf("leaf a"));
            root.Add(new Leaf("leaf b"));

            Tree branch = new Branch("branch 1");
            branch.Add(new Leaf("leaf 1a"));
            branch.Add(new Leaf("leaf 1b"));

            root.Add(branch);

            root.Display(1);
        }
    }

    // Component
    abstract class Tree {
        protected string name;

        public Tree(string name) {
            this.name = name;
        }

        public abstract void Add(Tree tree);
        public abstract void Remove(Tree tree);
        public abstract void Display(int n_count);
    }

    // Composite
    class Branch : Tree {
        List<Tree> l_trees = new List<Tree>();

        public Branch(string name) : base(name) { }

        public override void Add(Tree tree) {
            l_trees.Add(tree);
        }

        public override void Remove(Tree tree) {
            l_trees.Remove(tree);
        }

        public override void Display(int n_count) {
            Console.WriteLine(new string('-', n_count) + name);

            for(int n_i = 0; n_i < l_trees.Count; n_i++) {
                Tree tree = l_trees[n_i];
                tree.Display(n_count + 1);
            }
        }
    }

    // Leaf
    class Leaf : Tree {
        public Leaf(string name) : base(name) { }

        public override void Add(Tree tree) {
            throw new NotImplementedException();
        }

        public override void Remove(Tree tree) {
            throw new NotImplementedException();
        }

        public override void Display(int n_count) {
            Console.WriteLine(new string('-', n_count) + name);
        }
    }
}
