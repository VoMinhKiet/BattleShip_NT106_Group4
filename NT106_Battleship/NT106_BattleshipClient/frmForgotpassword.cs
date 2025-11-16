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
using System.Net.Http.Headers;
using Newtonsoft.Json; 

namespace NT106_BattleshipClient
{
    public partial class frmForgotpassword : BaseForm
    {
        public frmForgotpassword()
        {
            InitializeComponent();
        }

        private void linkCreateAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmSignup f = new frmSignup();
            f.Show();
            this.Hide();
        }

        private void linkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLogin f = new frmLogin();
            f.Show();
            this.Hide();
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

        private async void btnResetpassword_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (email == "")
            {
                MessageBox.Show("Vui lòng nhập email!", "Thông báo");
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5074/"); // PORT API của bạn

                    var requestData = new
                    {
                        email = email
                    };

                    string json = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("api/Auth/forgot-password", content);
                    string responseText = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Mã OTP đã được gửi đến email của bạn!", "Thông báo");

                        // Mở form nhập OTP
                        frmOTP f = new frmOTP(txtEmail.Text.Trim());
                        f.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi: " + responseText, "Thông báo");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối server: " + ex.Message);
            }
        }

        private void frmForgotpassword_Load(object sender, EventArgs e)
        {
            //Ẩn thanh tiêu đề nếu cần
            this.FormBorderStyle = FormBorderStyle.None;
        }
    }
}
