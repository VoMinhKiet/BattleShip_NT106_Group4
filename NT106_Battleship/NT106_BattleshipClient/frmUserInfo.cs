using System;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace NT106_BattleshipClient
{
    public partial class frmUserInfo : BaseForm
    {
        public frmUserInfo()
        {
            InitializeComponent();

            // chống nháy form
            EnableFormDoubleBuffering();


        }

        private async Task LoadUserRankingAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"http://localhost:5074/api/battle-ranking/user/{GlobalData.UserId}";
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


            //Ẩn thanh tiêu đề nếu cần
            //    this.FormBorderStyle = FormBorderStyle.None;

            lblID.Text = $"ID : {GlobalData.UserId}";
            lblTen.Text = $"Tên : {GlobalData.Username}";
            lblEmail.Text = $"Email : {GlobalData.Email}";

            await LoadUserRankingAsync();
        }



        private void btnLichSuDau_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMatchHistory matchHistoryForm = new frmMatchHistory();
            matchHistoryForm.ShowDialog();
            this.Show();
        }

        private void frmUserInfo_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void panelUserInfo_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }
    }
}
