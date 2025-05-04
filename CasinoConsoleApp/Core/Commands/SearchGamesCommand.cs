using System;

namespace CasinoConsoleApp.Core.Commands
{
    public class SearchGamesCommand : ICommand
    {
        public DateTime? From { get; init; }
        public DateTime? To { get; init; }
        public string? TypeFilter { get; init; }
        public int? ClientId { get; init; }
        public bool UseRawSql { get; init; } = false;
    }
}
