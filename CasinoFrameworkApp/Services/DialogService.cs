using CasinoFrameworkApp.Services;
using CasinoFrameworkApp;
using System.Collections.Generic;
using System;

public class DialogService : IDialogService
{
    public bool EditClient(ClientDto dto, out string newName)
    {
        var wnd = new EditClientWindow(dto);
        bool? result = wnd.ShowDialog();
        newName = wnd.NewName;
        return result == true;
    }

    public bool EditGame(GameDto dto, out DateTime playedAt, out string type, out List<int> playerIds)
    {
        var wnd = new EditGameWindow(dto);
        bool? result = wnd.ShowDialog();
        playedAt = wnd.PlayedAt;
        type = wnd.Type;
        playerIds = wnd.PlayerIds;
        return result == true;
    }
}
