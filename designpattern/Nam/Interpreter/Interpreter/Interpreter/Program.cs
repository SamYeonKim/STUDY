using System;
using System.Collections.Generic;

namespace Interpreter {
    class Program {
        static void Main(string[] args) {
            string str = "ABBCCCDDDD";
            Context context = new Context(str);

            List<Expression> l_expr = new List<Expression>();
            l_expr.Add(new ThousandExpression());
            l_expr.Add(new HundredExpression());
            l_expr.Add(new TenExpression());
            l_expr.Add(new OneExpression());

            for(int n_idx = 0; n_idx < l_expr.Count; n_idx++) {
                l_expr[n_idx].Interpret(context);
            }

            Console.WriteLine("{0} = {1}", str, context.Output);
        }
    }

    // Context class
    class Context {
        string input;
        int output;

        public Context(string _input) {
            input = _input;
        }

        public string Input {
            get { return input; }
            set { input = value; }
        }

        public int Output {
            get { return output; }
            set { output = value; }
        }
    }

    // Abstract Expression class
    abstract class Expression {
        public void Interpret(Context context) {
            if (context.Input.Length == 0)
                return;

            while(context.Input.StartsWith(One())) {
                context.Output += (1 * Result());
                context.Input = context.Input.Substring(1);
            }
        }

        public abstract string One();
        public abstract int Result();
    }

    // Terminal class 1
    class ThousandExpression : Expression {
        public override string One() {
            return "A";
        }

        public override int Result() {
            return 1000;
        }
    }

    // Terminal class 2
    class HundredExpression : Expression {
        public override string One() {
            return "B";
        }

        public override int Result() {
            return 100;
        }
    }

    // Terminal class 3
    class TenExpression : Expression {
        public override string One() {
            return "C";
        }

        public override int Result() {
            return 10;
        }
    }

    // Terminal class 4
    class OneExpression : Expression {
        public override string One() {
            return "D";
        }

        public override int Result() {
            return 1;
        }
    }
}
