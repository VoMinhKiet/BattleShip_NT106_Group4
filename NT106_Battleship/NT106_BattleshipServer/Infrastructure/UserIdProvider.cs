using Microsoft.AspNetCore.SignalR;

namespace NT106_BattleshipServer.Infrastructure
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // Lấy userId từ query string
            var httpContext = connection.GetHttpContext();
            return httpContext?.Request.Query["userId"];
        }
    }
}
