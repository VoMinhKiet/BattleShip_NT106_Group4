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

namespace NT106_BattleshipClient
{
    public partial class frmResetpassword : BaseForm
    {
        private string userEmail;
        private string otpCode;

        public frmResetpassword(string email, string otp)
        {
            InitializeComponent();

            userEmail = email;
            otpCode = otp;

            // Ẩn mật khẩu khi nhập
            txtNewpassword.PasswordChar = '*';
            txtConfirmpassword.PasswordChar = '*';
        }

        private void bntExit_Click(object sender, EventArgs e)
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
            char c = chkShowpassword.Checked ? '\0' : '*';

            txtNewpassword.PasswordChar = c;
            txtConfirmpassword.PasswordChar = c;
        }

        private void linkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLogin f = new frmLogin();
            f.Show();
            this.Hide();
        }

        private void linkCreateAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmSignup f = new frmSignup();
            f.Show();
            this.Hide();
        }

        private void frmResetpassword_Load(object sender, EventArgs e)
        {
            //Ẩn thanh tiêu đề nếu cần
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private async void btnReset_Click(object sender, EventArgs e)
        {
            string newPass = txtNewpassword.Text.Trim();
            string confirmPass = txtConfirmpassword.Text.Trim();

            // Kiểm tra rỗng
            if (newPass == "" || confirmPass == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi");
                return;
            }

            // Kiểm tra trùng khớp
            if (newPass != confirmPass)
            {
                MessageBox.Show("Mật khẩu xác nhận không trùng!", "Lỗi");
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = new
                    {
                        email = userEmail,
                        code = otpCode,
                        newPassword = newPass
                    };

                    string json = JsonConvert.SerializeObject(request);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("http://localhost:5074/api/Auth/reset-password", content);
                    string result = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Đặt lại mật khẩu thành công!", "Thông báo");

                        frmLogin f = new frmLogin();
                        f.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Không thể đổi mật khẩu!\n" + result, "Lỗi");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối API: " + ex.Message);
            }
        }
    }
}
