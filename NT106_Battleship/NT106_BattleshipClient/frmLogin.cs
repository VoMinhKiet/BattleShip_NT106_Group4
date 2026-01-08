using Newtonsoft.Json;
using NT106_BattleshipClient.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmLogin : BaseForm
    {
        private async Task DangNhapTaiKhoanAsync(string tenDangNhap, string matKhau)
        {
            using (HttpClient client = new HttpClient())
            {
                // 1. Lấy URL động từ ConfigHelper
                string url = ConfigHelper.GetServerUrl();

                // 2. Đảm bảo có dấu / ở cuối để BaseAddress hoạt động đúng với api/Auth/login
                if (!url.EndsWith("/")) url += "/";

                client.BaseAddress = new Uri(url);

                MessageBox.Show("Đang kết nối tới: " + url);

                var body = new
                {
                    tenDangNhap = tenDangNhap,
                    matKhau = matKhau
                };

                string json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/Auth/login", content);
                string result = await response.Content.ReadAsStringAsync();

                var obj = JsonConvert.DeserializeObject<LoginResponse>(result);

                if (response.IsSuccessStatusCode)
                {
                    GlobalData.UserId = obj.user.Id;
                    GlobalData.Username = obj.user.TenDangNhap;
                    GlobalData.Email = obj.user.Email;

                    MessageBox.Show($"Đăng nhập thành công!");

                    //frmMainMenu f = new frmMainMenu();
                    //this.Hide();
                    //f.ShowDialog();
                    //this.Show();

                    frmMainMenu f = new frmMainMenu();
                    f.FormClosed += (s, e) => this.Show();
                    this.Hide();
                    f.Show();
                }

                else
                {
                    MessageBox.Show("Đăng nhập thất bại!\n" + result, "Lỗi");
                }
            }
        }
        public frmLogin()
        {
            InitializeComponent();
            txtPassword.PasswordChar = '*';

            // chống nháy form
            EnableFormDoubleBuffering();
        }
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "" || password == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            await DangNhapTaiKhoanAsync(username, password);
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            //Ẩn thanh tiêu đề nếu cần
            // this.FormBorderStyle = FormBorderStyle.None;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát?",
                "Xác nhận thoát",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void chkShowpassword_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowpassword.Checked)
                txtPassword.PasswordChar = '\0';
            else
                txtPassword.PasswordChar = '*';
        }

        private void linkCreateAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmSignup f = new frmSignup();
            f.Show();
            this.Hide();
        }

        private void linkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmForgotpassword f = new frmForgotpassword();
            f.Show();
            this.Hide();
        }

    }
}
