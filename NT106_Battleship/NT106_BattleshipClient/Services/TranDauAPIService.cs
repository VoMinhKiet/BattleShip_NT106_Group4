using NT106_BattleshipClient.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NT106_BattleshipClient.Services
{
    public class TranDauApiService
    {
        private readonly HttpClient _http;

        public TranDauApiService()
        {
            _http = new HttpClient();
            _http.BaseAddress = new Uri("http://localhost:5074/");
        }

        // GET api/trandau/get/{id}
        public async Task<TranDauDto> GetByIdAsync(int id)
        {
            var resp = await _http.GetAsync($"api/trandau/get/{id}");
            resp.EnsureSuccessStatusCode();

            string json = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TranDauDto>(json);
        }

        // GET api/trandau/list
        public async Task<TranDauDto[]> GetAllAsync()
        {
            var resp = await _http.GetAsync("api/trandau/list");
            resp.EnsureSuccessStatusCode();

            string json = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TranDauDto[]>(json);
        }

        // POST api/trandau/create
        public async Task<TranDauDto> CreateMatchAsync(TaoTranDauRequest req)
        {
            string json = JsonSerializer.Serialize(req);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resp = await _http.PostAsync("api/trandau/create", content);
            resp.EnsureSuccessStatusCode();

            string respJson = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TranDauDto>(respJson);
        }

        // PATCH api/trandau/winner/{id}
        public async Task<bool> UpdateWinnerAsync(int id, int winnerId)
        {
            string json = JsonSerializer.Serialize(new { WinnerId = winnerId });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var resp = await _http.PutAsync($"api/trandau/winner/{id}", content);
            return resp.IsSuccessStatusCode;
        }

        // POST api/trandau/end/{id}
        public async Task<bool> EndMatchAsync(int id)
        {
            var resp = await _http.PostAsync($"api/trandau/end/{id}", null);
            return resp.IsSuccessStatusCode;
        }

        // GET api/trandau/history/{userId}
        public async Task<List<MatchHistoryDto>> GetHistoryAsync(int userId)
        {
            var resp = await _http.GetAsync($"api/trandau/history/{userId}");
            resp.EnsureSuccessStatusCode();

            string json = await resp.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<List<MatchHistoryDto>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

    }
}
