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
    public partial class frmOTP : Form
    {
        private string userEmail;

        public frmOTP(string email)
        {
            InitializeComponent();
            userEmail = email;
        }

        private async void btnVeriOTP_Click(object sender, EventArgs e)
        {
            string otp = txtOTP.Text.Trim();

            if (otp.Length != 6)
            {
                MessageBox.Show("OTP phải gồm 6 số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var body = new
                    {
                        email = userEmail,
                        code = otp
                    };

                    string json = JsonConvert.SerializeObject(body);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Đổi sang port API của bạn
                    var response = await client.PostAsync("http://localhost:5074/api/Auth/verify-otp", content);

                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("OTP hợp lệ! Hãy đặt mật khẩu mới.", "Thành công");

                        // Mở form Reset Password
                        frmResetpassword f = new frmResetpassword(userEmail, txtOTP.Text.Trim());
                        f.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("OTP không đúng hoặc đã hết hạn!\n" + apiResponse,
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối API: " + ex.Message);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            frmForgotpassword f = new frmForgotpassword();
            f.Show();
            this.Hide();
        }
    }
}
