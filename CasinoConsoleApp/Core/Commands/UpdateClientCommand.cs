using CasinoConsoleApp.Core.Commands;

namespace CasinoConsoleApp.Core.Commands
{
    public class UpdateClientCommand : ICommand
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public bool UseRawSql { get; init; } = false;
    }
}
