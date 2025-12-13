using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Data;
using System.Threading.Tasks;

namespace NT106_BattleshipServer.Hubs
{
    public class XepTauHub : Hub
    {
        private readonly AppDbContext _db;
        public XepTauHub (AppDbContext db)
        {
            _db = db;
        }
        public async Task JoinRoom(int roomId)
        {
            // Kiem tra IdPhong da ton tai hay chua
            /*var roomExists = await _db.TranDau.AnyAsync(td => td.IdPhongCho == roomId);
            if (!roomExists)
            {
                throw new HubException($"Room {roomId} does not exist.");
            } */

            await Groups.AddToGroupAsync(Context.ConnectionId, $"room-{roomId}");
        }
        public async Task StartGame(int roomId)
        {
            Console.WriteLine($"StartGame received for room {roomId}");
            await Clients.Group($"room-{roomId}")
                .SendAsync("GameStarted");
        }
        public async Task UpdateReadyFlag(int roomId, bool BtnReadyFlag)
        {
            await Clients.OthersInGroup($"room-{roomId}")
                .SendAsync("ReceiveReadyFlag", BtnReadyFlag);
        }
    }
}
