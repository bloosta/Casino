using CasinoConsoleApp.Core.Commands;

namespace CasinoConsoleApp.Core.Commands
{
    public class AddUserCommand : ICommand
    {
        public string Username { get; init; }
        public string Password { get; init; }    // ещё не захеширована
    }
}
