using System;
using System.Collections.Generic;

namespace Mediator {
    class Program {
        static void Main(string[] args) {
            Chatroom chatroom = new Chatroom();

            User user1 = new PersonalUser("user 1");
            User user2 = new PersonalUser("user 2");
            User user3 = new PersonalUser("user 3");

            chatroom.Register(user1);
            chatroom.Register(user2);
            chatroom.Register(user3);

            user1.Send("user 2", "Hello!");
            user2.Send("user 1", "World!");
            user3.Send("user 1", "Hello World!");
            user3.Send("user 2", "Hello World!");
        }
    }

    // Mediator class
    interface IChatroom {
        void Register(User user);
        void Send(string from, string to, string msg);
    }

    // Concrete Mediator class
    class Chatroom : IChatroom {
        List<User> l_users = new List<User>();

        public void Register(User user) {
            l_users.Add(user);
            user.SetChatroom(this);
        }

        public void Send(string from, string to, string msg) {
            User chat_user = l_users.Find((user) => user.Name == to);
            if(chat_user != null) {
                chat_user.Receive(from, msg);
            }
        }
    }

    // Collegue class
    class User {
        Chatroom chatroom;
        string user_name;

        public User(string name) {
            user_name = name;
        }

        public string Name {
            get { return user_name; }
        }

        public void SetChatroom(Chatroom _chatroom) {
            chatroom = _chatroom;
        }

        public virtual void Send(string to, string msg) {
            chatroom.Send(user_name, to, msg);
        }

        public virtual void Receive(string from, string msg) {
            Console.WriteLine("{0} to {1} : {2}", from, Name, msg);
        }
    }

    // Concrete Collegue class
    class PersonalUser : User {

        public PersonalUser(string name) : base(name) {

        }

        public override void Receive(string from, string msg) {
            Console.WriteLine("To a " + Name);
            base.Receive(from, msg);
        }
    }
}
