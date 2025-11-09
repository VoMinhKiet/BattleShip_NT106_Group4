using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmResetpassword : BaseForm
    {
        private string _email;

        public frmResetpassword(string email)
        {
            InitializeComponent();
            _email = email;
        }
        public frmResetpassword()
        {
            InitializeComponent();
            txtConfirmpassword.PasswordChar = '*';
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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

        private void linkCreateAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmSignup f = new frmSignup();
            f.Show();
            this.Hide();
        }

        private void frmResetpassword_Load(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            string newPassword = txtNewpassword.Text.Trim();
            string confirmPassword = txtConfirmpassword.Text.Trim();

            if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ mật khẩu mới và xác nhận mật khẩu.");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp.");
                return;
            }

            var userRepo = new UserRepository();

            // Hash mật khẩu mới
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // Cập nhật mật khẩu mới vào cơ sở dữ liệu
            if (userRepo.UpdatePassword(_email, hashedPassword))
            {
                MessageBox.Show("Mật khẩu đã được cập nhật thành công.");
            }
            else
            {
                MessageBox.Show("Cập nhật mật khẩu thất bại.");
            }
        }
    }
}
