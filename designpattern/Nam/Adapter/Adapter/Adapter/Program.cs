using System;

namespace Adapter {
    class Program {
        static void Main(string[] args) {
            USB usb = new USB();
            usb.ReadData();
            usb.WriteData();

            usb = new USBAdapter();
            usb.ReadData();
            usb.WriteData();
        }
    }

    // Taget Class
    class USB {
        public virtual void ReadData() {
            Console.WriteLine("Read Data : " + GetType().Name);
        }

        public virtual void WriteData() {
            Console.WriteLine("Write Data : " + GetType().Name);
        }
    }

    // Adater Class
    class USBAdapter : USB {
        SDCard sdcard = new SDCard();

        public override void ReadData() {
            sdcard.Read();
        }

        public override void WriteData() {
            sdcard.Write();
        }
    }
    
    // Adaptee Class
    class SDCard {
        public void Read() {
            Console.WriteLine("Read Data : " + GetType().Name);
        }

        public void Write() {
            Console.WriteLine("Write Data : " + GetType().Name);
        }
    }
}