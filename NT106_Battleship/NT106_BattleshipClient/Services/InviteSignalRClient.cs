using Microsoft.AspNetCore.SignalR.Client;
using NT106_BattleshipClient;
using System;
using System.Threading.Tasks;

namespace NT106_BattleshipClient.Services
{
    public static class InviteSignalRClient
    {
        public static HubConnection Connection { get; private set; }

        public static void Init(int userId)
        {
            // 1. Lấy URL động từ ConfigHelper
            string baseUrl = ConfigHelper.GetServerUrl();

            // 2. Đảm bảo có dấu gạch chéo ở cuối để nối chuỗi cho đẹp
            if (!baseUrl.EndsWith("/")) baseUrl += "/";

            // 3. Tạo kết nối
            Connection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl}inviteHub?userId={userId}")
                .WithAutomaticReconnect()
                .Build();
        }

        public static async Task StartAsync()
        {
            if (Connection == null) throw new Exception("InviteSignalRClient not initialized");
            if (Connection.State == HubConnectionState.Connected) return;
            await Connection.StartAsync();
        }
    }
}
