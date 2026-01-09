using Newtonsoft.Json;
using NT106_BattleshipClient.Models;
using NT106_BattleshipClient;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NT106_BattleshipClient
{
    public class UserApiService
    {
        private readonly HttpClient _client;

        public UserApiService()
        {
            _client = new HttpClient();

            // 1. Lấy URL động từ ConfigHelper
            string url = ConfigHelper.GetServerUrl();

            // 2. Đảm bảo có dấu / ở cuối
            if (!url.EndsWith("/")) url += "/";

            // 3. Gán vào BaseAddress
            _client.BaseAddress = new Uri(url);
        }

        // Lấy thông tin User theo ID từ API
        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var resp = await _client.GetAsync($"api/user/{userId}");   // Gửi GET
            resp.EnsureSuccessStatusCode();                            // Báo lỗi nếu thất bại

            string json = await resp.Content.ReadAsStringAsync();      // Đọc JSON
            return JsonConvert.DeserializeObject<UserDto>(json);       // Convert -> UserDto
        }
    }
}
