using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmMainMenu : BaseForm
    {
       
        public frmMainMenu()
        {
            InitializeComponent();
            
        }

        private void frmMainMenu_Load(object sender, EventArgs e)
        {
            /*if (!Session.IsAuthenticated)
            {
                // Chặn mở thẳng form chính khi chưa login
                new frmLogin().Show();
                this.Close();
                return;
            }*/
            // ví dụ hiển thị tên
            lblXinChao.Text = $"Xin chào, {Session.Username}!";
            // Lấy kích thước màn hình chính
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;

            // Áp dụng kích thước đó cho Form
            this.Size = screen.Size;
            this.Location = new Point(0, 0); // Đặt Form ở góc trên bên trái
                                             // Đường dẫn tương đối từ thư mục bin/Debug đến file

            //Ẩn thanh tiêu đề nếu cần
            this.FormBorderStyle = FormBorderStyle.None;
        }
        
        private void btnHoSo_Click(object sender, EventArgs e)
        {
            frmUserInfo MoForm = new frmUserInfo();
            MoForm.ShowDialog();
        }

        private void btnThoat_Click(object sender, EventArgs e)
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

        private void btnBXH_Click(object sender, EventArgs e)
        {

            frmLeaderBoard formBXH = new frmLeaderBoard();

            formBXH.Show();
  


        }

        private void btnHuongDanChoi_Click(object sender, EventArgs e)
        {
            frmNoteGame MoForm = new frmNoteGame();
            MoForm.ShowDialog();
        }

        private void btnHuongDanChoi_Click_1(object sender, EventArgs e)
        {
            frmNoteGame moForm = new frmNoteGame();
            moForm.ShowDialog();
        }

        private void btnBanBe_Click(object sender, EventArgs e)
        {
            frmFriendlist MoForm = new frmFriendlist();
            MoForm.ShowDialog();
        }

        private void btnCaiDat_Click(object sender, EventArgs e)
        {
            frmSettings MoForm = new frmSettings();
            MoForm.ShowDialog();
        }

        private void btnChoiVoiMay_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmLobby Lobby = new frmLobby();
            Lobby.ShowDialog();
            this.Show();
        }

        private void btnChoiVoiNguoi_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmLobby Lobby = new frmLobby();
            Lobby.ShowDialog();
            this.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var ask = MessageBox.Show(
        "Bạn có chắc chắn muốn đăng xuất?",
        "Xác nhận",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            if (ask == DialogResult.Yes)
            {
                Session.Logout();
                // Mở form Signup
                frmSignup signupForm = new frmSignup();
                signupForm.Show();

                // Ẩn 
                this.Hide();
                //đang để frmMainMenu là form chính nếu để frmLogin nên đổi thành this.Close();
            }
        }

        private void lblXinChao_Click(object sender, EventArgs e)
        {

        }
    }
}
