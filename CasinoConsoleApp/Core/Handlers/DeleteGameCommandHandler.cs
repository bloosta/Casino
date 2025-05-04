using System.Data.Common;
using System.Threading.Tasks;
using CasinoConsoleApp.Core.Commands;
using CasinoConsoleApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CasinoConsoleApp.Core.Handlers
{
    public class DeleteGameCommandHandler : ICommandHandler<DeleteGameCommand>
    {
        private readonly ApplicationDbContext _db;

        public DeleteGameCommandHandler(ApplicationDbContext db) => _db = db;

        public async Task HandleAsync(DeleteGameCommand cmd)
        {
            if (cmd.UseRawSql)
            {
                var conn = _db.Database.GetDbConnection();
                await conn.OpenAsync();
                using var tx = await conn.BeginTransactionAsync();
                try
                {
                    using var delRel = conn.CreateCommand();
                    delRel.Transaction = tx;
                    delRel.CommandText = "DELETE FROM ClientGames WHERE GameId=@g";
                    var g = delRel.CreateParameter(); g.ParameterName = "@g"; g.Value = cmd.Id; delRel.Parameters.Add(g);
                    await delRel.ExecuteNonQueryAsync();

                    using var delGame = conn.CreateCommand();
                    delGame.Transaction = tx;
                    delGame.CommandText = "DELETE FROM Games WHERE Id=@g";
                    delGame.Parameters.Add(g);
                    await delGame.ExecuteNonQueryAsync();

                    await tx.CommitAsync();
                }
                finally
                {
                    await conn.CloseAsync();
                }
            }
            else
            {
                var game = await _db.Games.FindAsync(cmd.Id);
                if (game != null)
                {
                    _db.Games.Remove(game);
                    await _db.SaveChangesAsync();
                }
            }
            Console.WriteLine("Game deleted.");
        }
    }
}
