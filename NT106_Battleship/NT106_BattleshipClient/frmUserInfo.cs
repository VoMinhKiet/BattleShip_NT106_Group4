using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;
using NT106_BattleshipClient.Models;

namespace NT106_BattleshipClient
{
    public partial class frmUserInfo : BaseForm
    {

        private readonly int _viewUserId;

        public frmUserInfo() : this(GlobalData.UserId) { }  

        public frmUserInfo(int userId)
        {
            InitializeComponent();
            EnableFormDoubleBuffering();
            _viewUserId = userId;
        }

        private async Task LoadUserInfoByIdAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Lấy IP động + ghép chuỗi
                    string baseUrl = ConfigHelper.GetServerUrl();
                    string url = $"{baseUrl}/api/User/get/{_viewUserId}";

                    var res = await client.GetAsync(url);
                    var json = await res.Content.ReadAsStringAsync();


                    if (!res.IsSuccessStatusCode) return;

                    dynamic u = JsonConvert.DeserializeObject(json);

                    lblID.Text = $"ID : {u.id}";
                    lblTen.Text = $"Tên : {u.tenDangNhap}";
                    lblEmail.Text = $"Email : {u.email}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadUserInfoByIdAsync error: " + ex.Message);
            }
        }

        private async Task LoadUserRankingAsync()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Lấy IP động + ghép chuỗi
                    string baseUrl = ConfigHelper.GetServerUrl();
                    string url = $"{baseUrl}/api/battle-ranking/user/{_viewUserId}";

                    var res = await client.GetAsync(url);
                    var json = await res.Content.ReadAsStringAsync();

                    if (!res.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Ranking API failed: {(int)res.StatusCode}\n{json}");
                        return;
                    }

                    var data = JsonConvert.DeserializeObject<UserRankingDto>(json);
                    if (data == null)
                    {
                        MessageBox.Show("Ranking parse failed: data=null");
                        return;
                    }

                    lblSao.Text = $"Số Sao : {data.capSao}";
                    lblTongSoTran.Text = $"Tổng số trận : {data.tongSoTran}";
                    lblTiLeThang.Text = $"Tỉ lệ thắng : {data.tiLeThang}%";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("LoadUserRankingAsync error: " + ex.Message);
            }
        }

        private void LoadUserInfo()
        {
            lblID.Text = $"ID : {GlobalData.UserId}";
            lblTen.Text = $"Tên : {GlobalData.Username}";
            lblEmail.Text = $"Email : {GlobalData.Email}";
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void frmUserInfo_Load(object sender, EventArgs e)
        {

            await LoadUserInfoByIdAsync();
            await LoadUserRankingAsync();
        }

        private void btnLichSuDau_Click(object sender, EventArgs e)
        {
            this.Hide();

            frmMatchHistory matchHistoryForm = new frmMatchHistory(_viewUserId);
            matchHistoryForm.ShowDialog();

            this.Show();
        }
    }
}
