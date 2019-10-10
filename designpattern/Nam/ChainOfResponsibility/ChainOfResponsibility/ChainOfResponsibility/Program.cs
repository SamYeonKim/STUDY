using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainOfResponsibility {
    class Program {
        static void Main(string[] args) {
            // Build the chain of responsibility
            Logger logger1 = new StdoutLogger(Logger.DEBUG);
            Logger logger2 = new StdinLogger(Logger.NOTICE);
            Logger logger3 = new StderrLogger(Logger.ERR);

            logger1.SetSuccessor(logger2);
            logger2.SetSuccessor(logger3);

            logger1.Message("Logger Type : Debug", Logger.DEBUG);
            
            logger1.Message("Logger Type : NOTICE", Logger.NOTICE);
            
            logger1.Message("Logger Type : ERR", Logger.ERR);
        }
    }

    // Handler class
    abstract class Logger {
        public static int ERR = 1;
        public static int NOTICE = 2;
        public static int DEBUG = 3;
        protected int mask;
        
        protected Logger next_logger;

        public Logger SetSuccessor(Logger log) {
            next_logger = log;
            return log;
        }

        public void Message(String msg, int priority) {
            if (priority <= mask) {
                WriteMessage(msg);
            }
            if (next_logger != null) {
                next_logger.Message(msg, priority);
            }
        }

        abstract protected void WriteMessage(String msg);
    }

    // Concrete Handler1 class
    class StdoutLogger : Logger {
        public StdoutLogger(int mask) {
            this.mask = mask;
        }

        protected override void WriteMessage(String msg) {
            Console.WriteLine("Writing to stdout: " + msg);
        }
    }

    // Concrete Handler2 class
    class StdinLogger : Logger {
        public StdinLogger(int mask) {
            this.mask = mask;
        }

        protected override void WriteMessage(String msg) {
            Console.WriteLine("Writing to stdin: " + msg);
        }
    }

    // Concrete Handler3 class
    class StderrLogger : Logger {
        public StderrLogger(int mask) {
            this.mask = mask;
        }

        protected override void WriteMessage(String msg) {
            Console.WriteLine("Writing to stderr: " + msg);
        }
    }
}
