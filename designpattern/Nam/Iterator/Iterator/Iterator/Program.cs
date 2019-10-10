using System;
using System.Collections.Generic;

namespace Iterator {
    class Program {
        static void Main(string[] args) {
            Library library = new Library();
            for(int n_idx = 0; n_idx < 10; n_idx++) {
                library[n_idx] = new Book("book " + n_idx);
            }

            Librarian librarian = library.EmployLibrarian();
            librarian.Next(2);

            for(Book book = librarian.First(); !librarian.IsDone(); book = librarian.Next()) {
                Console.WriteLine(book.Name);
            }
        }
    }

    // Item
    class Book {
        string name;

        public Book(string _name) {
            name = _name;
        }

        public string Name {
            get { return name; }
        }
    }

    // Aggregate class
    interface ICollection {
        Librarian EmployLibrarian();
    }

    // Concrete Aggregate class
    class Library : ICollection {
        List<Book> l_books = new List<Book>();

        public Librarian EmployLibrarian() {
            return new Librarian(this);
        }

        public int Count {
            get { return l_books.Count; }
        }

        public Book this[int idx] {
            get { return l_books[idx]; }
            set { l_books.Add(value); }
        }
    }

    // Iterator class
    interface IIterator {
        Book First();
        Book Next(int n_next);
        bool IsDone();
        Book Current();
    }

    // Concrete Iterator class
    class Librarian : IIterator {
        Library library;
        int n_current;

        public Librarian(Library _library) {
            n_current = 0;
            library = _library;
        }

        public Book First() {
            return library[0];
        }

        public Book Next(int n_next = 1) {
            n_current += n_next;
            if(!IsDone()) {
                return library[n_current];
            } else {
                return null;
            }
        }

        public bool IsDone() {
            return n_current >= library.Count;
        }

        public Book Current() {
            return library[n_current];
        }
    }
}
