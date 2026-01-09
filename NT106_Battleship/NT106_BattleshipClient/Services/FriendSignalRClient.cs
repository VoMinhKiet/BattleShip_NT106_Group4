using Microsoft.AspNetCore.SignalR.Client;
using NT106_BattleshipClient;
using System;
using System.Threading.Tasks;

namespace NT106_BattleshipClient.Services
{
    public class FriendSignalRClient
    {
        private HubConnection _connection;

        public event Action<int, string> OnInviteReceived;

        public async Task StartAsync(int userId)
        {
            string baseUrl = ConfigHelper.GetServerUrl();

            _connection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl.TrimEnd('/')}/friendHub?userId={userId}")
                .WithAutomaticReconnect()
                .Build();

            _connection.On<dynamic>("OnInviteToPlay", data =>
            {
                int fromId = (int)data.fromUserId;
                string message = (string)data.message;

                // Cách gọi event ngắn gọn an toàn hơn (Null check)
                OnInviteReceived?.Invoke(fromId, message);
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
