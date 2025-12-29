using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace NT106_BattleshipClient.Services
{
    public class BattleHubService
    {
        public HubConnection Connection { get; }

        public BattleHubService(string baseUrl)
        {
            Connection = new HubConnectionBuilder()
                .WithUrl($"{baseUrl}/tranDauHub")
                .WithAutomaticReconnect()
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .Build();
        }

        public async Task StartAsync()
        {
            if (Connection.State == HubConnectionState.Disconnected)
                await Connection.StartAsync();
        }

        public async Task StopAsync()
        {
            if (Connection.State != HubConnectionState.Disconnected)
                await Connection.StopAsync();
        }
    }
}
