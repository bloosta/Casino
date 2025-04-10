using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CasinoConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new CasinoContext())
            {

                var clients = context.Clients.ToList();

                var sessions = context.Sessions
                                      .Include(s => s.ClientSessions)
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
                    var clientNames = string.Join(", ", session.ClientSessions.Select(cs => cs.Client.Name));
                    Console.WriteLine($"| {session.Id,3} | {session.DateTime,-17} | {session.GameType,-15} | {clientNames,-25} |");
                }
                Console.WriteLine("--------------------------------------------------------------------------");
            }
        }
    }
}
