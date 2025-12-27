using Microsoft.AspNetCore.SignalR;

namespace NT106_BattleshipServer.Hubs
{
    public class InviteHub : Hub
    {
        // Host gửi lời mời cho user khác
        public async Task SendRoomInvite(int toUserId, int roomId, int fromUserId, string fromUsername)
        {
            // Gửi đúng 1 người theo UserIdProvider (Clients.User)
            await Clients.User(toUserId.ToString())
                .SendAsync("ReceiveRoomInvite", new
                {
                    roomId,
                    fromUserId,
                    fromUsername
                });
        }

        // Optional: host được báo kết quả accept/reject (cho đẹp)
        public async Task ReplyInvite(int toHostId, int roomId, int fromUserId, bool accepted)
        {
            await Clients.User(toHostId.ToString())
                .SendAsync("InviteReplied", new
                {
                    roomId,
                    fromUserId,
                    accepted
                });
        }
    }
}
