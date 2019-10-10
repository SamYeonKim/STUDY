using System;

namespace State {
    class Program {
        static void Main(string[] args) {
            User user = new User("Noname");

            user.Win();
            user.Win();
            user.Win();

            user.Lose();
            user.Lose();
            user.Lose();
        }
    }

    // State class
    abstract class Rank {
        protected User user;
        protected int score;

        public User User {
            get { return user; }
            set { user = value; }
        }

        public int Score {
            get { return score; }
            set { score = value; }
        }

        public abstract void Win();
        public abstract void Lose();
    }

    // Concrete State class
    class Bronze : Rank {
        public Bronze (int _score, User _user) {
            score = _score;
            user = _user;
        }

        public override void Win() {
            score += 3;
            CheckRankChange();
        }

        public override void Lose() {
            score -= 3;
            CheckRankChange();
        }

        public void CheckRankChange() {
            if(score >= 3) {
                user.Rank = new Silver(score, user);
            }
        }
    }

    // Concrete State class
    class Silver : Rank {
        public Silver(int _score, User _user) {
            score = _score;
            user = _user;
        }

        public override void Win() {
            score += 3;
            CheckRankChange();
        }

        public override void Lose() {
            score -= 3;
            CheckRankChange();
        }

        public void CheckRankChange() {
            if(score < 3) {
                user.Rank = new Bronze(score, user);
            } else if (score >= 6) {
                user.Rank = new Gold(score, user);
            }
        }
    }

    // Concrete State class
    class Gold : Rank {
        public Gold(int _score, User _user) {
            score = _score;
            user = _user;
        }

        public override void Win() {
            score += 3;
            CheckRankChange();
        }

        public override void Lose() {
            score -= 3;
            CheckRankChange();
        }

        public void CheckRankChange() {
            if(score < 6) {
                user.Rank = new Silver(score, user);
            }
        }
    }

    // Context class
    class User {
        string name;
        Rank rank;

        public User(string _name) {
            name = _name;
            rank = new Bronze(0, this);
        }

        public Rank Rank {
            get { return rank; }
            set { rank = value; }
        }

        public void Win() {
            rank.Win();

            Console.WriteLine("User Win!");
            Console.WriteLine("Rank : {0}", rank.GetType().Name);
            Console.WriteLine("Score : {0}\n", rank.Score);
        }

        public void Lose() {
            rank.Lose();

            Console.WriteLine("User Lose!");
            Console.WriteLine("Rank : {0}", rank.GetType().Name);
            Console.WriteLine("Score : {0}\n", rank.Score);
        }
    }
}
