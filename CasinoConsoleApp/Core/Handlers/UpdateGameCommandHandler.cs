using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using CasinoConsoleApp.Core.Commands;
using CasinoConsoleApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CasinoConsoleApp.Core.Handlers
{
    public class UpdateGameCommandHandler : ICommandHandler<UpdateGameCommand>
    {
        private readonly ApplicationDbContext _db;

        public UpdateGameCommandHandler(ApplicationDbContext db) => _db = db;

        public async Task HandleAsync(UpdateGameCommand cmd)
        {
            if (cmd.UseRawSql)
            {
                var conn = _db.Database.GetDbConnection();
                await conn.OpenAsync();
                using var tx = await conn.BeginTransactionAsync();
                try
                {
                    // Обновляем игру
                    using var upd = conn.CreateCommand();
                    upd.Transaction = tx;
                    upd.CommandText = "UPDATE Games SET PlayedAt=@p0, Type=@p1 WHERE Id=@id";
                    var p0 = upd.CreateParameter(); p0.ParameterName = "@p0"; p0.Value = cmd.PlayedAt; upd.Parameters.Add(p0);
                    var p1 = upd.CreateParameter(); p1.ParameterName = "@p1"; p1.Value = cmd.Type; upd.Parameters.Add(p1);
                    var id = upd.CreateParameter(); id.ParameterName = "@id"; id.Value = cmd.Id; upd.Parameters.Add(id);
                    await upd.ExecuteNonQueryAsync();

                    // Сброс связей: удаляем и вставляем новые
                    using var del = conn.CreateCommand();
                    del.Transaction = tx;
                    del.CommandText = "DELETE FROM ClientGames WHERE GameId=@gid";
                    var gid = del.CreateParameter(); gid.ParameterName = "@gid"; gid.Value = cmd.Id; del.Parameters.Add(gid);
                    await del.ExecuteNonQueryAsync();

                    if (cmd.ClientIds != null)
                    {
                        foreach (var clientId in cmd.ClientIds)
                        {
                            using var join = conn.CreateCommand();
                            join.Transaction = tx;
                            join.CommandText = "INSERT INTO ClientGames (ClientId, GameId) VALUES (@c, @g)";
                            var c = join.CreateParameter(); c.ParameterName = "@c"; c.Value = clientId; join.Parameters.Add(c);
                            var g = join.CreateParameter(); g.ParameterName = "@g"; g.Value = cmd.Id; join.Parameters.Add(g);
                            await join.ExecuteNonQueryAsync();
                        }
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
                var game = await _db.Games
                    .Include(g => g.Players)
                    .FirstOrDefaultAsync(g => g.Id == cmd.Id);
                if (game == null) return;

                game.PlayedAt = cmd.PlayedAt;
                game.Type = cmd.Type;

                if (cmd.ClientIds != null)
                {
                    var clients = await _db.Clients
                        .Where(c => cmd.ClientIds.Contains(c.Id))
                        .ToListAsync();
                    game.Players.Clear();
                    foreach (var c in clients) game.Players.Add(c);
                }

                await _db.SaveChangesAsync();
            }
            Console.WriteLine("Game updated.");
        }
    }
}
