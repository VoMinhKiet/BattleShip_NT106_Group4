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
        private readonly UserRepository _repo = new UserRepository();
        public frmSignup()
        {
            InitializeComponent();
            txtPassword.PasswordChar = '*';
            txtConfirmpassword.PasswordChar = '*';
        }
        private void btnSignup_Click(object sender, EventArgs e)
        {
            string u = txtUsername.Text.Trim();
            string eMail = txtEmail.Text.Trim();
            string p = txtPassword.Text;
            string c = txtConfirmpassword.Text;

            // Validate cơ bản
            if (_repo.UsernameExists(u))
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.");
                return;
            }
            if (string.IsNullOrWhiteSpace(eMail))
            {
                MessageBox.Show("Vui lòng nhập email.");
                return;
            }
            if (!string.IsNullOrWhiteSpace(eMail) && _repo.EmailExists(eMail))
            {
                MessageBox.Show("Email đã tồn tại. Vui lòng dùng email khác.");
                return;
            }
            if (string.IsNullOrWhiteSpace(u) || string.IsNullOrWhiteSpace(p))
            {
                MessageBox.Show("Vui lòng nhập tên người dùng và mật khẩu.");
                return;
            }
            if (p.Length < 6)
            {
                MessageBox.Show("Mật khẩu tối thiểu 6 ký tự.");
                return;
            }
            if (p != c)
            {
                MessageBox.Show("Xác nhận mật khẩu không khớp.");
                return;
            }
            try
            {
                _repo.CreateUser(u, p, eMail);
                MessageBox.Show("Đăng ký thành công! Hãy đăng nhập.");
                // quay lại form login
                var f = new frmLogin();
                f.Show();
                this.Close();
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại (ràng buộc UNIQUE).");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
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

        }
    }
}
