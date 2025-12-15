using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace NT106_BattleshipServer.Hubs
{
    public class XepTauHub : Hub
    {
        // store current host-turn flag per room so late-joining clients can query state
        private static ConcurrentDictionary<int, bool> _roomTurn = new ConcurrentDictionary<int, bool>();
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
        public async Task SendShipPos(int roomId, int[] rowIndices, int[] colIndices, int[] orientations)
        {
            await Clients.OthersInGroup($"room-{roomId}")
                .SendAsync("ReceivedShips", rowIndices, colIndices, orientations);
        }
        public async Task Turn(int roomId, bool turnIsHost)
        {
            // persist the turn state for the room
            _roomTurn.AddOrUpdate(roomId, turnIsHost, (k, v) => turnIsHost);

            // broadcast to others in group
            await Clients.Group($"room-{roomId}").SendAsync("Turn", turnIsHost);
        }

        // allow clients to query latest turn state after they subscribe
        public Task<bool> GetTurnStatus(int roomId)
        {
            if (_roomTurn.TryGetValue(roomId, out var value))
                return Task.FromResult(value);

            // default: host's turn = true (or choose whatever default fits your logic)
            return Task.FromResult(true);
        }
        public async Task Hit(int roomId, int row, int col, bool isHit)
        {
            await Clients.OthersInGroup($"room-{roomId}")
                .SendAsync("ReceiveHit", row, col, isHit);
        }
    }
}
