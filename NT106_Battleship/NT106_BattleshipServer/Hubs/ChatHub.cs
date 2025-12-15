using Microsoft.AspNetCore.SignalR;
using NT106_BattleshipServer.DTOs;

namespace NT106_BattleshipServer.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinPhong(int roomId)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                roomId.ToString()
            );
        }

        public async Task GuiTinNhan(TinNhanDto tinNhan)
        {
            await Clients.Group(tinNhan.IdPhongCho.ToString())
                .SendAsync("NhanTinNhan", tinNhan);
        }
    }
}
