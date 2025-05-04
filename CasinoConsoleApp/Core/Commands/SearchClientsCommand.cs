namespace CasinoConsoleApp.Core.Commands
{
    public class SearchClientsCommand : ICommand
    {
        public string? NameFilter { get; init; }
        public bool UseRawSql { get; init; } = false;
    }
}
