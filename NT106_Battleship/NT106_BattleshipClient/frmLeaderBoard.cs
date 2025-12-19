using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Text.Json;

namespace NT106_BattleshipClient
{
    public partial class frmLeaderBoard : BaseForm
    {
        public frmLeaderBoard()
        {
            InitializeComponent();

            // chống nháy form
            EnableFormDoubleBuffering();
        }

        private async void frmLeaderBoard_Load(object sender, EventArgs e)
        {
            //  this.FormBorderStyle = FormBorderStyle.None;

            dataGridView1.DefaultCellStyle.Font =
    new Font("Segoe UI", 18F, FontStyle.Bold);

            dataGridView1.RowTemplate.Height = 45;


            HttpClient http = new HttpClient();
            http.BaseAddress = new Uri("http://localhost:5074/");


            var resp = await http.GetAsync("api/LeaderBoard");
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<LeaderBoardDto>>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            dataGridView1.Rows.Clear();

            int stt = 1;
            foreach (var item in list)
            {
                int tongTran = item.SoTranThang + item.SoTranThua;
                string tiLeThang = tongTran == 0
                    ? "0%"
                    : $"{(item.SoTranThang * 100 / tongTran)}%";

                dataGridView1.Rows.Add(
                    stt,         
                    item.TenNguoiDung,
                    item.CapSao,
                    tiLeThang,            
                    tongTran               
                );

                                stt++;

            }

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {

        }
    }
}
