namespace CasinoConsoleApp.Core.Security
{
    public interface ICurrentUserContext
    {
        int? UserId { get; set; }
        bool IsAuthenticated => UserId.HasValue;
    }
}
