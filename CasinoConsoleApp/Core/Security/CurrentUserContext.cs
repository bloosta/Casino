namespace CasinoConsoleApp.Core.Security
{
    public class CurrentUserContext : ICurrentUserContext
    {
        public int? UserId { get; set; }
        public bool IsAuthenticated => UserId.HasValue;
    }
}
