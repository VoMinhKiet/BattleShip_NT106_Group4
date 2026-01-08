using NT106_BattleshipClient.Models;
using NT106_BattleshipClient;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NT106_BattleshipClient.Services
{
    public class CreateTranDauRequest
    {
        public int IdPlayer1 { get; set; }
        public int IdPlayer2 { get; set; }
        public string TenNV1 { get; set; }
        public string TenNV2 { get; set; }
        public int KichThuoc { get; set; }
        public int IdPhongCho { get; set; }
    }

    public class TranDauApiService
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _options;

        public TranDauApiService()
        {
            _http = new HttpClient();

            // 1. Lấy URL động từ ConfigHelper
            string url = ConfigHelper.GetServerUrl();

            // 2. Đảm bảo có dấu / ở cuối
            if (!url.EndsWith("/")) url += "/";

            // 3. Gán vào BaseAddress
            _http.BaseAddress = new Uri(url);

            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // 1. GỌI API TẠO TRẬN ĐẤU
        public async Task<TranDauDto> CreateMatchAsync(CreateTranDauRequest req)
        {
            try
            {
                string json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var resp = await _http.PostAsync("api/trandau/create", content);

                if (!resp.IsSuccessStatusCode)
                {
                    string errorContent = await resp.Content.ReadAsStringAsync();
                    // Ném lỗi ra để frmRoom bắt được
                    throw new Exception($"Server Error {resp.StatusCode}: {errorContent}");
                }

                string respJson = await resp.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TranDauDto>(respJson, _options);
            }
            catch (Exception ex)
            {
                // Ném lỗi lên tầng trên để MessageBox hiển thị
                throw ex;
            }
        }

        // 2. GỌI API KẾT THÚC TRẬN ĐẤU
        public async Task<bool> EndMatchAsync(int id, int winnerId)
        {
            try
            {
                var body = new { WinnerId = winnerId };
                string json = JsonSerializer.Serialize(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var resp = await _http.PostAsync($"api/trandau/end/{id}", content);
                return resp.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // 3. LẤY LỊCH SỬ
        public async Task<List<MatchHistoryDto>> GetHistoryAsync(int userId)
        {
            var resp = await _http.GetAsync($"api/trandau/history/{userId}");
            resp.EnsureSuccessStatusCode();
            string json = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<MatchHistoryDto>>(json, _options);
        }
    }
}