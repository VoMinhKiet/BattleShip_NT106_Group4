using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace NT106_BattleshipClient
{
    public partial class frmUserInfo : BaseForm
    {

        private int userId;

        private async Task LoadPlayerProfile(int userId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5074/");

                // Gửi yêu cầu GET đến API, truyền userId dưới dạng số nguyên
                HttpResponseMessage response = await client.GetAsync($"api/Player/profile/{userId}");
                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var user = JsonConvert.DeserializeObject<dynamic>(result);

                    txtUserID.Text = user.id.ToString();
                    txtUsername.Text = (string)user.tenDangNhap;
                    txtEmail.Text = (string)user.email;
                    txtRank.Text = (string)user.bacRank;
                    txtStarnums.Text = user.capSao.ToString();
                    txtMatchs.Text = user.tongTran.ToString();
                    txtWinRates.Text = ((double)user.tiLeThang).ToString("0.##") + "%";
                }
                else
                {
                    MessageBox.Show("Không thể lấy thông tin người chơi.");
                }
            }
        }
        public frmUserInfo(int id)
        {
            InitializeComponent();
            userId = id;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void frmUserInfo_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            txtUserID.ReadOnly = true;
            txtUsername.ReadOnly = true;
            txtEmail.ReadOnly = true;
            txtRank.ReadOnly = true;
            txtStarnums.ReadOnly = true;
            txtMatchs.ReadOnly = true;
            txtWinRates.ReadOnly = true;
            await LoadPlayerProfile(userId);
        }

        private void btnLichSuDau_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMatchHistory matchHistoryForm = new frmMatchHistory();
            matchHistoryForm.ShowDialog();
            this.Show();
        }
    }
}
