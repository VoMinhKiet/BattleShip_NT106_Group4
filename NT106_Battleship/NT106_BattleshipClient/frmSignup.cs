using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace NT106_BattleshipClient
{
    public partial class frmSignup : BaseForm
    {
        private async Task DangKyTaiKhoanAsync(string tenDangNhap, string matKhau, string email)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5074/");

                var body = new
                {
                    tenDangNhap = tenDangNhap,
                    matKhau = matKhau,
                    email = email
                };

                string json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/Auth/register", content);
                string result = await response.Content.ReadAsStringAsync();

                // Thành công
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Đăng ký thành công!", "Thông báo");

                    // Quay về form Login
                    frmLogin loginForm = new frmLogin();
                    loginForm.Show();
                    this.Hide();  // Ẩn form signup

                    return;
                }

                // Lấy lỗi từ API
                dynamic error = JsonConvert.DeserializeObject(result);
                string msg = error?.message ?? "Có lỗi xảy ra";

                // Kiểm tra thông báo lỗi để báo đẹp hơn
                if (msg.Contains("tồn tại"))
                {
                    MessageBox.Show(msg, "Tên đăng nhập hoặc email bị trùng",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Lỗi đăng ký: " + msg);
                }
            }
        }


        public frmSignup()
        {
            InitializeComponent();
            txtPassword.PasswordChar = '*';
            txtConfirmpassword.PasswordChar = '*';
        }
        private async void btnSignup_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirm = txtConfirmpassword.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (username == "" || password == "" || confirm == "" || email == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            if (username.Length < 4)
            {
                MessageBox.Show("Tên đăng nhập phải ít nhất 4 ký tự!");
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải ít nhất 6 ký tự!");
                return;
            }

            if (password != confirm)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!");
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Email không hợp lệ!");
                return;
            }

            await DangKyTaiKhoanAsync(username, password, email);
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

            if (chkShowpassword.Checked)
                txtConfirmpassword.PasswordChar = '\0';
            else
                txtConfirmpassword.PasswordChar = '*';
        }

        private void linkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLogin f = new frmLogin();
            f.Show();
            this.Hide();
        }

        private void linkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmForgotpassword f = new frmForgotpassword();
            f.Show();
            this.Hide();
        }

        private void frmSignup_Load(object sender, EventArgs e)
        {
            //Ẩn thanh tiêu đề nếu cần
            this.FormBorderStyle = FormBorderStyle.None;
        }
    }
}
