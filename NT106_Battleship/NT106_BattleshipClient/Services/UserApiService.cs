using Newtonsoft.Json;
using NT106_BattleshipClient.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace NT106_BattleshipClient
{
    public class UserApiService
    {
        private readonly HttpClient _client;

        public UserApiService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:5074/");
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var resp = await _client.GetAsync($"api/user/{userId}");
            resp.EnsureSuccessStatusCode();

            string json = await resp.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserDto>(json);
        }


        //// API: GET api/User/{id}
        //public async Task<UserDto> GetUserByIdAsync(int id)
        //{
        //    var res = await _client.GetAsync($"api/User/{id}");
        //    string json = await res.Content.ReadAsStringAsync();
        //    return JsonConvert.DeserializeObject<UserDto>(json);
        //}
    }
}
