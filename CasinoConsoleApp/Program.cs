using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CasinoConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new casinoContext())
            {

                var clients = context.Clients.ToList();

                var sessions = context.Sessions
                                      .Include(s => s.Clientsession)
                                      .ThenInclude(cs => cs.Client)
                                      .ToList();

                Console.WriteLine("Clients:");
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine("| Id | Name                           |");
                Console.WriteLine("-----------------------------------------------");
                foreach (var client in clients)
                {
                    Console.WriteLine($"| {client.Id,3} | {client.Name,-30} |");
                }
                Console.WriteLine("-----------------------------------------------");

                Console.WriteLine();

                Console.WriteLine("Sessions:");
                Console.WriteLine("--------------------------------------------------------------------------");
                Console.WriteLine("| Id | DateTime           | GameType        | Clients                    |");
                Console.WriteLine("--------------------------------------------------------------------------");
                foreach (var session in sessions)
                {
                    var clientNames = string.Join(", ", session.Clientsession.Select(cs => cs.Client.Name));
                    Console.WriteLine($"| {session.Id,3} | {session.Datetime,-17} | {session.Gametype,-15} | {clientNames,-25} |");
                }
                Console.WriteLine("--------------------------------------------------------------------------");
                Console.ReadLine();
            }
        }
    }
}
