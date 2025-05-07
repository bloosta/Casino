using CasinoFrameworkApp.Services;
using System.Collections.Generic;
using System;

public interface IDialogService
{
    bool EditClient(ClientDto dto, out string newName);
    bool EditGame(GameDto dto, out DateTime playedAt, out string type, out List<int> playerIds);
}
