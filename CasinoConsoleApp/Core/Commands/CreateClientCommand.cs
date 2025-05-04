using CasinoConsoleApp.Core.Commands;

namespace CasinoConsoleApp.Core.Commands
{
    public class CreateClientCommand : ICommand
    {
        public string Name { get; init; }
        public bool UseRawSql { get; init; } = false;
    }
}
