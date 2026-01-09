using Newtonsoft.Json;
using NT106_BattleshipClient.Models;
using NT106_BattleshipClient;
using System;
using System.Net.Http;
using System.Security.Policy;
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
            // Lấy URL động từ ConfigHelper
            string url = ConfigHelper.GetServerUrl();
            if (!url.EndsWith("/")) url += "/";

            _http.BaseAddress = new Uri(url);
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

        public async Task<RoomDto> JoinRoomAndGetRoomAsync(int roomId, int userId)
        {
            // Lấy URL động từ ConfigHelper
            string dynamicUrl = ConfigHelper.GetServerUrl();
            if (!dynamicUrl.EndsWith("/")) dynamicUrl += "/";

            // Thay thế _baseUrl cũ bằng dynamicUrl
            using (var http = new HttpClient { BaseAddress = new Uri(dynamicUrl) })
            {
                var res = await http.PostAsync($"api/Room/join?roomId={roomId}&userId={userId}", null);
                var json = await res.Content.ReadAsStringAsync();

                if (!res.IsSuccessStatusCode)
                    throw new Exception(json);

                var wrapper = JsonConvert.DeserializeObject<JoinRoomResponse>(json);
                if (wrapper?.room == null) throw new Exception("JoinRoomResponse.room is null");

                return wrapper.room;
            }
        }

        private class JoinRoomResponse
        {
            public string message { get; set; }
            public RoomDto room { get; set; }
        }
          
        // Hàm tìm ID user dựa trên tên đăng nhập (Dùng để tìm Bot)
        public async Task<int?> GetUserIdByUsernameAsync(string username)
        {
            try
            {
                var resp = await _http.GetAsync($"api/User/find/{username}");

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserDto>(json);
                    return user?.Id;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }

    // Class model nhận JSON trả về khi Create / Join room
    public class CreateRoomResponse
    {
        public string message { get; set; } // Thông báo từ server
        public RoomDto room { get; set; }   // Object phòng trả về
    }
}
