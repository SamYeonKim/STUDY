using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento {
    class Program {
        static void Main(string[] args) {
            Game game = new Game();
            game.Name = "Zelda";
            game.Time = 12;
            game.Progress = 20;

            GameMemory game_memory = new GameMemory();
            game_memory.SetSavePoint(game.SaveGame());

            game.Name = "PocketMon";
            game.Time = 6;
            game.Progress = 50;

            game.LoadGame(game_memory.LoadSavePoint());
        }
    }

    // Originator class
    public class Game {
        string name;
        int m_n_time;
        int m_n_progress;

        public string Name {
            get { return name; }
            set {
                name = value;
                Console.WriteLine("Name : " + name);
            }
        }

        public int Time {
            get { return Time; }
            set {
                m_n_time = value;
                Console.WriteLine("Play Time : " + m_n_time);
            }
        }

        public int Progress {
            get { return m_n_progress; }
            set {
                m_n_progress = value;
                Console.WriteLine("Progress : " + m_n_progress + "%");
            }
        }

        public SavePoint SaveGame() {
            Console.WriteLine("\nSaving...\n");
            return new SavePoint(name, m_n_time, m_n_progress);
        }

        public void LoadGame(SavePoint save_point) {
            Console.WriteLine("\nLoading...\n");
            Name = save_point.Name;
            Time = save_point.Time;
            Progress = save_point.Progress;
        }
    }

    // Memento class
    public class SavePoint {
        string name;
        int m_n_time;
        int m_n_progress;

        public SavePoint(string _name, int n_time, int n_progress) {
            name = _name;
            m_n_time = n_time;
            m_n_progress = n_progress;
        }

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public int Time {
            get { return m_n_time; }
            set { m_n_time = value; }
        }

        public int Progress {
            get { return m_n_progress; }
            set { m_n_progress = value; }
        }
    }

    // Caretaker class
    public class GameMemory {
        SavePoint save_point;

        public void SetSavePoint(SavePoint _save_point) {
            save_point = _save_point;
        }

        public SavePoint LoadSavePoint() {
            return save_point;
        }
    }
}
