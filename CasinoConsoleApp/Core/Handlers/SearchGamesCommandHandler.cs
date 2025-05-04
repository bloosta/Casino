using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CasinoConsoleApp.Core.Commands;
using CasinoConsoleApp.Core.Entities;
using CasinoConsoleApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CasinoConsoleApp.Core.Handlers
{
    public class SearchGamesCommandHandler : ICommandHandler<SearchGamesCommand>
    {
        private readonly ApplicationDbContext _db;

        public SearchGamesCommandHandler(ApplicationDbContext db) => _db = db;

        public async Task HandleAsync(SearchGamesCommand cmd)
        {
            List<Game> games;

            if (cmd.UseRawSql)
            {
                var sql = @"
                    SELECT g.* 
                    FROM Games g
                    LEFT JOIN ClientGames cg ON g.Id = cg.GameId
                    WHERE 1 = 1";
                var parameters = new List<object>();

                if (cmd.From.HasValue)
                {
                    sql += " AND g.PlayedAt >= {0}";
                    parameters.Add(cmd.From.Value);
                }
                if (cmd.To.HasValue)
                {
                    sql += " AND g.PlayedAt <= {1}";
                    parameters.Add(cmd.To.Value);
                }
                if (!string.IsNullOrWhiteSpace(cmd.TypeFilter))
                {
                    sql += " AND g.Type LIKE {2}";
                    parameters.Add($"%{cmd.TypeFilter}%");
                }
                if (cmd.ClientId.HasValue)
                {
                    sql += " AND cg.ClientId = {3}";
                    parameters.Add(cmd.ClientId.Value);
                }

                games = await _db.Games.FromSqlRaw(sql, parameters.ToArray())
                    .Include(g => g.Players)
                    .ToListAsync();
            }
            else
            {
                var q = _db.Games.Include(g => g.Players).AsQueryable();

                if (cmd.From.HasValue)
                    q = q.Where(g => g.PlayedAt >= cmd.From.Value);
                if (cmd.To.HasValue)
                    q = q.Where(g => g.PlayedAt <= cmd.To.Value);
                if (!string.IsNullOrWhiteSpace(cmd.TypeFilter))
                    q = q.Where(g => EF.Functions.Like(g.Type, $"%{cmd.TypeFilter}%"));
                if (cmd.ClientId.HasValue)
                    q = q.Where(g => g.Players.Any(c => c.Id == cmd.ClientId.Value));

                games = await q.ToListAsync();
            }

            Console.WriteLine("Found games:");
            foreach (var g in games)
            {
                var players = string.Join(", ", g.Players.Select(c => c.Id));
                Console.WriteLine(
                    $"- [#{g.Id}] {g.Type} at {g.PlayedAt} | Players: {players}");
            }
        }
    }
}
