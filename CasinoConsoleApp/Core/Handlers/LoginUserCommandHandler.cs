using System.Threading.Tasks;
using CasinoConsoleApp.Core.Commands;
using CasinoConsoleApp.Core.Entities;
using CasinoConsoleApp.Core.Security;
using CasinoConsoleApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CasinoConsoleApp.Core.Handlers
{
    public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand>
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher _hasher;
        private readonly ICurrentUserContext _userCtx;

        public LoginUserCommandHandler(
            ApplicationDbContext db,
            PasswordHasher hasher,
            ICurrentUserContext userCtx)
        {
            _db = db;
            _hasher = hasher;
            _userCtx = userCtx;
        }

        public async Task HandleAsync(LoginUserCommand cmd)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Username == cmd.Username);

            if (user == null || !_hasher.Verify(cmd.Password, user.PasswordHash))
            {
                Console.WriteLine("Ошибка: неверный логин или пароль.");
                return;
            }

            _userCtx.UserId = user.Id;
            Console.WriteLine($"Пользователь '{user.Username}' успешно авторизован (Id={user.Id}).");
        }
    }
}
