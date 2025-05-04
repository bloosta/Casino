using CasinoConsoleApp.Core.Commands;

namespace CasinoConsoleApp.Core.Commands
{
    public class LoginUserCommand : ICommand
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
    }
}
