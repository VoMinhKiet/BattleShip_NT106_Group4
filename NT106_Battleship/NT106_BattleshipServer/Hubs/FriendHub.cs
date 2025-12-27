using Microsoft.AspNetCore.SignalR;

namespace NT106_BattleshipServer.Hubs
{
    public class FriendHub : Hub
    {
        public async Task InviteToPlay(int fromUserId, int toUserId)
        {
            await Clients.User(toUserId.ToString())
                .SendAsync("OnInviteToPlay", new
                {
                    fromUserId,
                    message = "Bạn nhận được lời mời chơi!"
                });
        }
    }
}
