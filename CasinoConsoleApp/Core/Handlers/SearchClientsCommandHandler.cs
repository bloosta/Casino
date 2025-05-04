using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CasinoConsoleApp.Core.Commands;
using CasinoConsoleApp.Core.Entities;
using CasinoConsoleApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CasinoConsoleApp.Core.Handlers
{
    public class SearchClientsCommandHandler : ICommandHandler<SearchClientsCommand>
    {
        private readonly ApplicationDbContext _db;

        public SearchClientsCommandHandler(ApplicationDbContext db)
            => _db = db;

        public async Task HandleAsync(SearchClientsCommand cmd)
        {
            List<Client> result;

            if (cmd.UseRawSql)
            {
                string sql = "SELECT * FROM Clients";
                if (!string.IsNullOrWhiteSpace(cmd.NameFilter))
                {
                    sql += " WHERE Name LIKE {0}";
                    result = await _db.Clients
                        .FromSqlRaw(sql, $"%{cmd.NameFilter}%")
                        .ToListAsync();
                }
                else
                {
                    result = await _db.Clients.FromSqlRaw(sql).ToListAsync();
                }
            }
            else
            {
                var query = _db.Clients.AsQueryable();
                if (!string.IsNullOrWhiteSpace(cmd.NameFilter))
                {
                    query = query.Where(c => EF.Functions.Like(c.Name, $"%{cmd.NameFilter}%"));
                }
                result = await query.ToListAsync();
            }

            Console.WriteLine("Результаты поиска клиентов:");
            foreach (var client in result)
            {
                Console.WriteLine($"- [#{client.Id}] {client.Name}");
            }
        }
    }
}
