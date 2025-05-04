using System.Threading.Tasks;
using CasinoConsoleApp.Core.Commands;
using CasinoConsoleApp.Data;
using CasinoConsoleApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CasinoConsoleApp.Core.Handlers
{
    public class UpdateClientCommandHandler : ICommandHandler<UpdateClientCommand>
    {
        private readonly ApplicationDbContext _db;

        public UpdateClientCommandHandler(ApplicationDbContext db)
            => _db = db;

        public async Task HandleAsync(UpdateClientCommand cmd)
        {
            if (cmd.UseRawSql)
            {
                await _db.Database.ExecuteSqlRawAsync(
                    "UPDATE Clients SET Name = {0} WHERE Id = {1}",
                    cmd.Name, cmd.Id);
            }
            else
            {
                var client = await _db.Clients.FindAsync(cmd.Id);
                if (client != null)
                {
                    client.Name = cmd.Name;
                    await _db.SaveChangesAsync();
                }
            }
        }
    }
}
