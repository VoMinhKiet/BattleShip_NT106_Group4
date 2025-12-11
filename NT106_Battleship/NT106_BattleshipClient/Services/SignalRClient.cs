using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using NT106_BattleshipClient.Models;

namespace NT106_BattleshipClient.Services
{
    public static class SignalRClient
    {
        public static HubConnection Connection { get; private set; }

        private static bool _started = false;                 // Tránh StartAsync nhiều lần
        private static bool _handlersRegistered = false;      // Tránh đăng ký handler lặp lại
        private static bool _inviteHandlerRegistered = false; // Tránh đăng ký invite nhiều lần

        public static event Action<RoomInviteDto> RoomInviteReceived; // Event toàn cục: bất kỳ form nào cũng có thể subscribe
        public static void Init(string hubUrl)
        {
            if (Connection != null) return;            // Nếu đã tạo → bỏ qua

            // Tạo connection đến Hub
            Connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()              // Tự reconnect khi mất kết nối
                .Build();
        }

        public static async Task StartAsync()
        {
            if (Connection == null) return;
            if (Connection.State == HubConnectionState.Disconnected)
            {
                await Connection.StartAsync();

                if (GlobalData.UserId != 0)
                {
                    await Connection.InvokeAsync("RegisterUser", GlobalData.UserId);
                }
            }
        }

        public static void RegisterHandlers(
            Action<RoomDto> onRoomUpdated,
            Action onRoomListUpdated,
            Action<int> onRoomDeleted)
        {
            if (_handlersRegistered) return;           // Tránh đăng ký trùng

            // Nhận sự kiện cập nhật danh sách phòng
            Connection.On("RoomListUpdated", onRoomListUpdated);

            // Nhận sự kiện phòng bị xoá
            Connection.On<int>("RoomDeleted", onRoomDeleted);

            // Nhận sự kiện phòng thay đổi
            Connection.On<RoomDto>("RoomUpdated", onRoomUpdated);

            _handlersRegistered = true;
        }

        public static void RegisterInviteHandler()
        {
            if (_inviteHandlerRegistered) return;
            if (Connection == null) return;

            Connection.On<RoomInviteDto>("InvitedToRoom", invite =>
            {
                RoomInviteReceived?.Invoke(invite);
            });

            _inviteHandlerRegistered = true;
        }
    }
}
