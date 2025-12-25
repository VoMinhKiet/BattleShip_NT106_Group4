using Newtonsoft.Json;
using NT106_BattleshipClient.Models;

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NT106_BattleshipClient.Services
{
    // Service dùng để gọi API liên quan đến phòng (Room)
    public class RoomApiService
    {
        // HttpClient thực hiện các request HTTP
        private readonly HttpClient _http;

        public RoomApiService()
        {
            _http = new HttpClient();

            // Thiết lập URL gốc cho toàn bộ request
            _http.BaseAddress = new Uri("http://localhost:5074/");
        }

        // LẤY DANH SÁCH PHÒNG
        // GET api/room/list
        public async Task<RoomDto[]> GetRoomsAsync()
        {
            var resp = await _http.GetAsync("api/room/list");

            // Ném lỗi nếu mã status không phải 200 OK
            resp.EnsureSuccessStatusCode();

            string json = await resp.Content.ReadAsStringAsync();

            // Chuyển JSON thành mảng RoomDto
            return JsonConvert.DeserializeObject<RoomDto[]>(json);
        }

        // TẠO PHÒNG
        // POST api/room/create?userId={userId}
        public async Task<RoomDto> CreateRoomAsync(int userId)
        {
            var resp = await _http.PostAsync($"api/room/create?userId={userId}", null);

            resp.EnsureSuccessStatusCode();

            string json = await resp.Content.ReadAsStringAsync();

            // Server trả về CreateRoomResponse { message, room }
            var obj = JsonConvert.DeserializeObject<CreateRoomResponse>(json);

            return obj.room; // Trả về object RoomDto
        }

        // THAM GIA PHÒNG
        // POST api/room/join?roomId={roomId}&userId={userId}
        public async Task<RoomDto> JoinRoomAsync(int roomId, int userId)
        {
            var resp = await _http.PostAsync($"api/room/join?roomId={roomId}&userId={userId}", null);

            resp.EnsureSuccessStatusCode();

            string json = await resp.Content.ReadAsStringAsync();

            var obj = JsonConvert.DeserializeObject<CreateRoomResponse>(json);

            return obj.room;
        }

        // RỜI PHÒNG
        // DELETE api/room/leave?roomId={roomId}&userId={userId}
        public async Task<bool> LeaveRoomAsync(int roomId, int userId)
        {
            var resp = await _http.DeleteAsync($"api/room/leave?roomId={roomId}&userId={userId}");

            // Trả về TRUE nếu status là 2xx
            return resp.IsSuccessStatusCode;
        }

        // LẤY THÔNG TIN PHÒNG THEO ID
        // GET api/room/get?roomId={roomId}
        public async Task<RoomDto> GetRoomByIdAsync(int roomId)
        {
            var resp = await _http.GetAsync($"api/room/get?roomId={roomId}");

            resp.EnsureSuccessStatusCode();

            string json = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<RoomDto>(json);
        }

        // LẤY USER THEO ID
        // GET /api/User/{id}
        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var resp = await _http.GetAsync($"api/User/{userId}");

            resp.EnsureSuccessStatusCode();

            string json = await resp.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<UserDto>(json);
        }

        // BẮT ĐẦU TRẬN ĐẤU TỪ PHÒNG
        // POST api/room/start?roomId={roomId}
        public async Task<bool> StartGameAsync(int roomId)
        {
            var response = await _http.PostAsync($"api/room/start?roomId={roomId}", null);
            return response.IsSuccessStatusCode;
        }
    }

    // Class model nhận JSON trả về khi Create / Join room
    public class CreateRoomResponse
    {
        public string message { get; set; } // Thông báo từ server
        public RoomDto room { get; set; }   // Object phòng trả về
    }
}
