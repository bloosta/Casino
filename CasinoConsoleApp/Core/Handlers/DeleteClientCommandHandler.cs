using System.Threading.Tasks;
using CasinoConsoleApp.Core.Commands;
using CasinoConsoleApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CasinoConsoleApp.Core.Handlers
{
    public class DeleteClientCommandHandler : ICommandHandler<DeleteClientCommand>
    {
        private readonly ApplicationDbContext _db;

        public DeleteClientCommandHandler(ApplicationDbContext db)
            => _db = db;

        public async Task HandleAsync(DeleteClientCommand cmd)
        {
            if (cmd.UseRawSql)
            {
                await _db.Database.ExecuteSqlRawAsync(
                    "DELETE FROM Clients WHERE Id = {0}",
                    cmd.Id);
            }
            else
            {
                var client = await _db.Clients.FindAsync(cmd.Id);
                if (client != null)
                {
                    _db.Clients.Remove(client);
                    await _db.SaveChangesAsync();
                }
            }
        }
    }
}
