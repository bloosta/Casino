using System.Data.Common;
using System.Threading.Tasks;
using CasinoConsoleApp.Core.Commands;
using CasinoConsoleApp.Core.Entities;
using CasinoConsoleApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CasinoConsoleApp.Core.Handlers
{
    public class AddGameCommandHandler : ICommandHandler<AddGameCommand>
    {
        private readonly ApplicationDbContext _db;

        public AddGameCommandHandler(ApplicationDbContext db) => _db = db;

        public async Task HandleAsync(AddGameCommand cmd)
        {
            if (cmd.UseRawSql)
            {
                var conn = _db.Database.GetDbConnection();
                await conn.OpenAsync();
                using var tx = await conn.BeginTransactionAsync();
                try
                {
                    // Вставляем игру и получаем её Id
                    using var insertGame = conn.CreateCommand();
                    insertGame.Transaction = tx;
                    insertGame.CommandText =
                        "INSERT INTO Games (PlayedAt, Type) OUTPUT INSERTED.Id VALUES (@p0, @p1)";
                    var p0 = insertGame.CreateParameter(); p0.ParameterName = "@p0"; p0.Value = cmd.PlayedAt; insertGame.Parameters.Add(p0);
                    var p1 = insertGame.CreateParameter(); p1.ParameterName = "@p1"; p1.Value = cmd.Type; insertGame.Parameters.Add(p1);
                    var scalar = await insertGame.ExecuteScalarAsync();
                    var gameId = Convert.ToInt32(scalar);

                    // Вставляем записи в таблицу связей
                    foreach (var clientId in cmd.ClientIds)
                    {
                        using var joinCmd = conn.CreateCommand();
                        joinCmd.Transaction = tx;
                        joinCmd.CommandText =
                            "INSERT INTO ClientGames (ClientId, GameId) VALUES (@c, @g)";
                        var c = joinCmd.CreateParameter(); c.ParameterName = "@c"; c.Value = clientId; joinCmd.Parameters.Add(c);
                        var g = joinCmd.CreateParameter(); g.ParameterName = "@g"; g.Value = gameId; joinCmd.Parameters.Add(g);
                        await joinCmd.ExecuteNonQueryAsync();
                    }

                    await tx.CommitAsync();
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
            else
            {
                var game = new Game
                {
                    PlayedAt = cmd.PlayedAt,
                    Type = cmd.Type
                };
                var clients = await _db.Clients
                    .Where(c => cmd.ClientIds.Contains(c.Id))
                    .ToListAsync();
                game.Players = clients;
                _db.Games.Add(game);
                await _db.SaveChangesAsync();
            }
            Console.WriteLine("Game added.");
        }
    }
}
