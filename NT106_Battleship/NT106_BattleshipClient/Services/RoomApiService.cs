//using Newtonsoft.Json;
//using NT106_BattleshipClient.Models;
//using System;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace NT106_BattleshipClient.Services
//{
//    public class RoomApiService
//    {
//        private readonly HttpClient _http;
//        public RoomApiService()
//        {
//            _http = new HttpClient();
//            _http.BaseAddress = new Uri("http://localhost:5074/");
//        }

//        public async Task<RoomDto[]> GetRoomsAsync()
//        {
//            var resp = await _http.GetAsync("api/room/list");
//            resp.EnsureSuccessStatusCode();
//            string json = await resp.Content.ReadAsStringAsync();
//            return JsonConvert.DeserializeObject<RoomDto[]>(json);
//        }

//        public async Task<RoomDto> CreateRoomAsync(int userId)
//        {
//            var resp = await _http.PostAsync($"api/room/create?userId={userId}", null);
//            resp.EnsureSuccessStatusCode();
//            string json = await resp.Content.ReadAsStringAsync();
//            var obj = JsonConvert.DeserializeObject<CreateRoomResponse>(json);
//            return obj.room;
//        }

//        public async Task<RoomDto> JoinRoomAsync(int roomId, int userId)
//        {
//            var resp = await _http.PostAsync($"api/room/join?roomId={roomId}&userId={userId}", null);
//            resp.EnsureSuccessStatusCode();
//            string json = await resp.Content.ReadAsStringAsync();
//            var obj = JsonConvert.DeserializeObject<CreateRoomResponse>(json);
//            return obj.room;
//        }

//        public async Task<bool> LeaveRoomAsync(int roomId, int userId)
//        {
//            var resp = await _http.DeleteAsync($"api/room/leave?roomId={roomId}&userId={userId}");
//            return resp.IsSuccessStatusCode;
//        }

//        public async Task<RoomDto> GetRoomByIdAsync(int roomId)
//        {
//            var resp = await _http.GetAsync($"api/room/get?roomId={roomId}");
//            resp.EnsureSuccessStatusCode();
//            string json = await resp.Content.ReadAsStringAsync();
//            return JsonConvert.DeserializeObject<RoomDto>(json);
//        }

//        // Gọi API user (đảm bảo route server phù hợp)
//        public async Task<UserDto> GetUserByIdAsync(int userId)
//        {
//            var resp = await _http.GetAsync($"api/user/get?id={userId}");
//            resp.EnsureSuccessStatusCode();
//            string json = await resp.Content.ReadAsStringAsync();
//            return JsonConvert.DeserializeObject<UserDto>(json);
//        }
//    }

//    public class CreateRoomResponse
//    {
//        public string message { get; set; }
//        public RoomDto room { get; set; }
//    }
//}

using Newtonsoft.Json;
using NT106_BattleshipClient.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NT106_BattleshipClient.Services
{
    public class RoomApiService
    {
        private readonly HttpClient _http;
        public RoomApiService()
        {
            _http = new HttpClient();
            _http.BaseAddress = new Uri("http://localhost:5074/");
        }

        public async Task<RoomDto[]> GetRoomsAsync()
        {
            var resp = await _http.GetAsync("api/room/list");
            resp.EnsureSuccessStatusCode();
            string json = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RoomDto[]>(json);
        }

        public async Task<RoomDto> CreateRoomAsync(int userId)
        {
            var resp = await _http.PostAsync($"api/room/create?userId={userId}", null);
            resp.EnsureSuccessStatusCode();
            string json = await resp.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<CreateRoomResponse>(json);
            return obj.room;
        }

        public async Task<RoomDto> JoinRoomAsync(int roomId, int userId)
        {
            var resp = await _http.PostAsync($"api/room/join?roomId={roomId}&userId={userId}", null);
            resp.EnsureSuccessStatusCode();
            string json = await resp.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<CreateRoomResponse>(json);
            return obj.room;
        }

        public async Task<bool> LeaveRoomAsync(int roomId, int userId)
        {
            var resp = await _http.DeleteAsync($"api/room/leave?roomId={roomId}&userId={userId}");
            return resp.IsSuccessStatusCode;
        }

        public async Task<RoomDto> GetRoomByIdAsync(int roomId)
        {
            var resp = await _http.GetAsync($"api/room/get?roomId={roomId}");
            resp.EnsureSuccessStatusCode();
            string json = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RoomDto>(json);
        }

        // **Sửa route đúng theo server UserController**
        // Server cung cấp: GET /api/User/{id}
        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var resp = await _http.GetAsync($"api/User/{userId}");
            resp.EnsureSuccessStatusCode();
            string json = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserDto>(json);
        }
    }

    public class CreateRoomResponse
    {
        public string message { get; set; }
        public RoomDto room { get; set; }
    }
}
