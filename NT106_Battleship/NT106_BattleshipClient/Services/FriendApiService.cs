using Newtonsoft.Json;
using NT106_BattleshipClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient.Services
{
    public class FriendApiService
    {
        private readonly HttpClient _http;
        public FriendApiService(HttpClient http) => _http = http;

        public async Task<List<FriendDto>> GetFriendsAsync(int userId)
        { 
            var res = await _http.GetAsync($"api/Friend/list?userId={userId}");
            var json = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
                throw new Exception(json);

            var data = JsonConvert.DeserializeObject<List<FriendDto>>(json);
            return data ?? new List<FriendDto>();
        }

        public async Task<bool> AddFriendAsync(int userId, int targetUserId)
        {
            var res = await _http.PostAsync($"api/Friend/add?userId={userId}&targetUserId={targetUserId}", null);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteFriendAsync(int userId, int targetUserId)
        {
            var res = await _http.DeleteAsync($"api/Friend/delete?userId={userId}&targetUserId={targetUserId}");
            return res.IsSuccessStatusCode;
        }

        public async Task<List<FriendDto>> GetRequestsAsync(int userId)
        {
            var res = await _http.GetAsync($"api/Friend/requests?userId={userId}");
            var json = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
                throw new Exception(json);

            var data = JsonConvert.DeserializeObject<List<FriendDto>>(json);
            return data ?? new List<FriendDto>();
        }

        public async Task<bool> AcceptAsync(int userId, int targetUserId)
        {
            var res = await _http.PostAsync($"api/Friend/accept?userId={userId}&targetUserId={targetUserId}", null);
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> RejectAsync(int userId, int targetUserId)
        {
            var res = await _http.PostAsync($"api/Friend/reject?userId={userId}&targetUserId={targetUserId}", null);
            return res.IsSuccessStatusCode;
        }

        public async Task<List<FriendDto>> SearchUsersAsync(int myUserId, int? id, string username)
        {
            var idPart = id.HasValue ? $"&id={id.Value}" : "";
            var namePart = !string.IsNullOrWhiteSpace(username) ? $"&username={Uri.EscapeDataString(username.Trim())}" : "";

            var url = $"api/User/search?userId={myUserId}{idPart}{namePart}";

            var res = await _http.GetAsync(url);
            var json = await res.Content.ReadAsStringAsync();

            if (!res.IsSuccessStatusCode)
                throw new Exception(json);

            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FriendDto>>(json);
            return data ?? new List<FriendDto>();
        }

    }
}
