using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using NT106_BattleshipClient.Models;

namespace NT106_BattleshipClient.Services
{
    public static class SignalRClient
    {
        public static HubConnection Connection { get; private set; }

        private static bool _started = false;           // Tránh StartAsync nhiều lần
        private static bool _handlersRegistered = false; // Tránh đăng ký handler lặp lại

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
            if (_started) return;                      // Chỉ Start 1 lần
            await Connection.StartAsync();
            _started = true;
        }

        //NOTE: Hiện tại handlers được đăng ký trực tiếp trong từng Form.
        // RegisterHandlers giữ lại để mở rộng sau.
        //public static void RegisterHandlers(
        //    Action<RoomDto> onRoomUpdated,
        //    Action onRoomListUpdated,
        //    Action<int> onRoomDeleted)
        //{
        //    if (_handlersRegistered) return;           // Tránh đăng ký trùng

        //    // Nhận sự kiện cập nhật danh sách phòng
        //    Connection.On("RoomListUpdated", onRoomListUpdated);

        //    // Nhận sự kiện phòng bị xoá
        //    Connection.On<int>("RoomDeleted", onRoomDeleted);

        //    // Nhận sự kiện phòng thay đổi
        //    Connection.On<RoomDto>("RoomUpdated", onRoomUpdated);

        //    _handlersRegistered = true;
        //}
    }
}
