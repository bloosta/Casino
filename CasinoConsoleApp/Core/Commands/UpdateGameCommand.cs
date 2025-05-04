using System;
using System.Collections.Generic;
using CasinoConsoleApp.Core.Commands;

namespace CasinoConsoleApp.Core.Commands
{
    public class UpdateGameCommand : ICommand
    {
        public int Id { get; init; }
        public DateTime PlayedAt { get; init; }
        public string Type { get; init; } = string.Empty;
        public List<int>? ClientIds { get; init; }
        public bool UseRawSql { get; init; } = false;
    }
}
