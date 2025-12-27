using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmUserInfo : BaseForm
    {

        private readonly int _viewUserId;

        public frmUserInfo() : this(GlobalData.UserId) { }  

        public frmUserInfo(int userId)
        {
            MessageBox.Show("View userId = " + userId);


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
                    string url = $"http://localhost:5074/api/User/get/{_viewUserId}";
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
            using (HttpClient client = new HttpClient())
            {
                string url = $"http://localhost:5074/api/battle-ranking/user/{_viewUserId}";

                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                dynamic data = JsonConvert.DeserializeObject(json);

                lblSao.Text = $"Số Sao : {data.CapSao}";
                lblTongSoTran.Text = $"Tổng số trận : {data.TongSoTran}";
                lblTiLeThang.Text = $"Tỉ lệ thắng : {data.TiLeThang}%";
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
            frmMatchHistory matchHistoryForm = new frmMatchHistory();
            matchHistoryForm.ShowDialog();
            this.Show();
        }
    }
}
