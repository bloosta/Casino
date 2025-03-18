using System;
using System.Collections.Generic;
using System.Linq;

namespace CasinoConsoleApp
{
    class Program
    {
        static IEnumerable<Session> CreateData()
        {
            var sessions = new List<Session>
            {
                new Session
                {
                    Id = 1,   DateTime = new DateTime(2025, 3, 18, 15, 25, 4), GameType = "Blackjack", Clients = new List<Client>
                    {
                        new Client {Id = 1, Name = "Egorov Ivan"},
                        new Client {Id = 2, Name = "Kapustin Alexander"},
                        new Client {Id = 3, Name = "Bogdanov Anton"}
                    }
                },
                new Session
                {
                    Id = 2, DateTime = new DateTime(2025, 3, 8, 5, 1, 14), GameType = "Poker", Clients = new List<Client>
                    {
                        new Client {Id = 1, Name = "Egorov Ivan"},
                        new Client {Id = 2, Name = "Kapustin Alexander"},
                        new Client {Id = 4, Name = "Biryukova Diana"}

                    }
                }
            };

            return sessions;
        }

        static void Main()
        {
            //var options = new DbContextOptionsBuilder<BooksContext>()
            //    .UseSqlite("Filename=../../../MyLocalLibrary.db")
            //    .Options;

            //using var db = new BooksContext(options);

            //db.Database.EnsureCreated();

            var sessions = CreateData();

            //db.Authors.AddRange(authors);

            //db.SaveChanges();

            //var recentBooks = from b in db.Books where b.YearOfPublication > 1900 select b;

            foreach (var session in sessions)
            {
                Console.WriteLine($"{session} была сыграна..");

                foreach (var client in session.Clients)
                {
                    Console.WriteLine($"    {client}");
                    Console.WriteLine();
                }
            }
        }
    }
}

