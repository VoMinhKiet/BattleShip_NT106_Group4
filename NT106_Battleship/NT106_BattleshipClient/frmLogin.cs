using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace NT106_BattleshipClient
{
    public partial class frmLogin : BaseForm
    {
        private async Task DangNhapTaiKhoanAsync(string tenDangNhap, string matKhau)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5074/");  // Port server của bạn

                var body = new
                {
                    tenDangNhap = tenDangNhap,
                    matKhau = matKhau
                };

                string json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/Auth/login", content);
                string result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đăng nhập thành công!");

                    // Ở đây bạn có thể chuyển sang form Main Menu
                    frmMainMenu f = new frmMainMenu();
                    f.Show();
                    this.Hide();
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
            this.FormBorderStyle = FormBorderStyle.None;
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
