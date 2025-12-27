using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace NT106_BattleshipClient.Services
{
    public static class InviteSignalRClient
    {
        public static HubConnection Connection { get; private set; }

        public static void Init(string baseUrl, int userId)
        {
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
