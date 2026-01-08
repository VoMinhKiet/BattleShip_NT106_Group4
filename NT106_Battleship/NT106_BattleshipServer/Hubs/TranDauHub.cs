using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace NT106_BattleshipServer.Hubs
{
    public class TranDauHub : Hub
    {
        // Client vào trận -> join group theo roomId (hoặc matchId)
        public async Task JoinBattle(int roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        }

        public async Task LeaveBattle(int roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }

        // Đồng bộ lượt: host phát lên group, guest nhận
        public async Task Turn(int roomId, bool isHostTurn)
        {
            await Clients.Group(roomId.ToString())
                         .SendAsync("Turn", isHostTurn);
        }

        // Bắn 1 ô: gửi qua đối thủ
        public async Task Hit(int roomId, int row, int col, bool isHit)
        {
            // chỉ gửi cho đối thủ trong cùng room
            await Clients.OthersInGroup(roomId.ToString())
                         .SendAsync("ReceiveHit", row, col, isHit);
        }

        // BATCH cho skill multi-shot: bật/tắt “đang bắn skill”
        public async Task SkillBatch(int roomId, bool started)
        {
            await Clients.Group(roomId.ToString())
                         .SendAsync("SkillBatch", started);
        }

        public async Task Surrender(int roomId)
        {
            await Clients.OthersInGroup(roomId.ToString()).SendAsync("OpponentSurrender");
        }
    }
}
