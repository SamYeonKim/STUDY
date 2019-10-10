using System;

namespace Proxy {
    class Program {
        static void Main(string[] args) {
            Data image = new ProxyImage("Image1", ProxyImage.Authority.NORMAL);
            image.LoadData();
            image.LoadData();

            image = new ProxyImage("Image2", ProxyImage.Authority.MANAGE);
            image.LoadData();
            image.LoadData();

            image = new ProxyImage("Image3", ProxyImage.Authority.MANAGE);
            image.LoadData();
        }
    }

    // Subject class
    abstract class Data {
        public abstract void LoadData();
    }

    // Real Subject class
    class ImageData : Data {
        public string name { get; private set; }

        public ImageData(string file_name) {
            name = file_name;
            ReadFromFile();
        }

        void ReadFromFile() {
            Console.WriteLine("Read... " + name);
        }        

        public override void LoadData() {
            Console.WriteLine("Load " + name);
        }
    }

    // Proxy class
    class ProxyImage : Data {
        public enum Authority {
            NONE,
            NORMAL,
            MANAGE
        }

        Data data;
        Authority authority;

        public string fileName { get; private set; }

        public ProxyImage(string name, Authority auth) {
            fileName = name;
            authority = auth;
        }        

        public override void LoadData() {
            if(authority != Authority.MANAGE) {
                Console.WriteLine("Do not have permission.");
                return;
            }

            if (data == null)
                data = new ImageData(fileName);
            data.LoadData();
        }
    }
}
