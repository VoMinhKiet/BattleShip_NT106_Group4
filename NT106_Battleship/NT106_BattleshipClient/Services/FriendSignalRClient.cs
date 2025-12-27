using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace NT106_BattleshipClient.Services
{
    public class FriendSignalRClient
    {
        private HubConnection _connection;

        public event Action<int, string> OnInviteReceived;

        public async Task StartAsync(string baseUrl, int userId)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(string.Format("{0}/friendHub?userId={1}", baseUrl.TrimEnd('/'), userId))
                .WithAutomaticReconnect()
                .Build();

            _connection.On<dynamic>("OnInviteToPlay", data =>
            {
                int fromId = (int)data.fromUserId;
                string message = (string)data.message;

                var handler = OnInviteReceived;
                if (handler != null) handler(fromId, message);
            });

            await _connection.StartAsync();
        }

        public async Task InviteAsync(int fromUserId, int toUserId)
        {
            if (_connection == null)
                throw new InvalidOperationException("SignalR chưa được khởi tạo.");

            await _connection.InvokeAsync("InviteToPlay", fromUserId, toUserId);
        }
    }
}
