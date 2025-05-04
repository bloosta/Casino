using CasinoConsoleApp.Core.Commands;

namespace CasinoConsoleApp.Core.Commands
{
    public class DeleteClientCommand : ICommand
    {
        public int Id { get; init; }
        public bool UseRawSql { get; init; } = false;
    }
}
