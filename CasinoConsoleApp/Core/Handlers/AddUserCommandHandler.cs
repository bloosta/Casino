using System.Threading.Tasks;
using CasinoConsoleApp.Core.Commands;
using CasinoConsoleApp.Core.Entities;
using CasinoConsoleApp.Core.Security;
using CasinoConsoleApp.Data;
using Microsoft.EntityFrameworkCore;


namespace CasinoConsoleApp.Core.Handlers
{
    public class AddUserCommandHandler : ICommandHandler<AddUserCommand>
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher _hasher;

        public AddUserCommandHandler(ApplicationDbContext db, PasswordHasher hasher)
        {
            _db = db;
            _hasher = hasher;
        }

        public async Task HandleAsync(AddUserCommand cmd)
        {
            var hash = _hasher.Hash(cmd.Password);
            var user = new User { Username = cmd.Username, PasswordHash = hash };

            // через EF Core
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }
    }
}
