using System.Threading.Tasks;
using CasinoConsoleApp.Core.Commands;
using CasinoConsoleApp.Core.Entities;
using CasinoConsoleApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CasinoConsoleApp.Core.Handlers
{
    public class CreateClientCommandHandler : ICommandHandler<CreateClientCommand>
    {
        private readonly ApplicationDbContext _db;

        public CreateClientCommandHandler(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task HandleAsync(CreateClientCommand cmd)
        {
            if (cmd.UseRawSql)
            {
                await _db.Database.ExecuteSqlRawAsync(
                    "INSERT INTO Clients (Name) VALUES ({0})",
                    cmd.Name);
            }
            else
            {
                var client = new Client { Name = cmd.Name };
                _db.Clients.Add(client);
                await _db.SaveChangesAsync();
            }
        }
    }
}
