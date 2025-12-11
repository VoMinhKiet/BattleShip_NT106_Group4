using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace NT106_BattleshipServer.Hubs
{
    public class RoomHub : Hub
    {
        // client gọi sau khi connect: đăng ký userId vào group riêng
        public async Task RegisterUser(int userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        // Khi client vào phòng → thêm connection vào group theo roomId
        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        // Khi client rời phòng → xoá connection khỏi group
        public async Task LeaveRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }

        // Server gọi để thông báo room thay đổi (host/guest update)
        public async Task UpdateRoom(string roomId)
        {
            await Clients.Group(roomId).SendAsync("roomUpdated", roomId);
        }

        // Server/client gửi lệnh đồng bộ UI trong phòng (start, ready, update name…)
        public async Task SendUISync(string roomId, string command, string value)
        {
            await Clients.Group(roomId)
                         .SendAsync("SynchronizeRoomUI", command, value);
        }

        public async Task SetGuestReady(int roomId, bool state)
        {
            await Clients.Group(roomId.ToString())
                         .SendAsync("GuestReadyStateChanged", state);
        }

    }
}
