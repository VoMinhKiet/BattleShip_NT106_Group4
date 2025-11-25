using Newtonsoft.Json;
using NT106_BattleshipClient.Models;
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
            _client.BaseAddress = new Uri("http://localhost:5074/");   // URL API server
        }

        // Lấy thông tin User theo ID từ API
        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var resp = await _client.GetAsync($"api/user/{userId}");   // Gửi GET
            resp.EnsureSuccessStatusCode();                            // Báo lỗi nếu thất bại

            string json = await resp.Content.ReadAsStringAsync();      // Đọc JSON
            return JsonConvert.DeserializeObject<UserDto>(json);       // Convert → UserDto
        }
    }
}
