using System;
using System.Drawing;
using System.Windows.Forms;
using NT106_BattleshipClient.Services;

namespace NT106_BattleshipClient
{
    public partial class frmMainMenu : BaseForm
    {
        private readonly RoomApiService _roomApi = new RoomApiService();

        public frmMainMenu()
        {
            this.SetStyle(ControlStyles.DoubleBuffer |
            ControlStyles.UserPaint |
            ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
            FormManager.frmMainMenu = this;

            EnableFormDoubleBuffering();//test

            SetUseComposited(true);
        }

        private void frmMainMenu_Load(object sender, EventArgs e)
        {
            lblXinChao.Text = $"Xin chào {GlobalData.Username}";

            // Lấy kích thước màn hình chính
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;

            // Áp dụng kích thước đó cho Form
            this.Size = screen.Size;
            this.Location = new Point(0, 0); // Đặt Form ở góc trên bên trái
                                             // Đường dẫn tương đối từ thư mục bin/Debug đến file

            //Ẩn thanh tiêu đề nếu cần
            this.FormBorderStyle = FormBorderStyle.None;

            //MusicManager.PlayMenuMusic();
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

        private async void btnChoiVoiMay_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Tìm ID của Bot nếu chưa có
                if (GlobalData.BotId == null)
                {
                    var botId = await _roomApi.GetUserIdByUsernameAsync(GlobalData.BOT_NAME);
                    if (botId == null)
                    {
                        MessageBox.Show($"Không tìm thấy tài khoản của máy ({GlobalData.BOT_NAME}) trong server");
                        return;
                    }
                    GlobalData.BotId = botId;
                }

                // 2. Tạo phòng
                var room = await _roomApi.CreateRoomAsync(GlobalData.UserId);

                if (room != null)
                {
                    // 3. Add Bot vào phòng
                    // IDKhach = BotId, TrangThai = Full
                    var roomWithBot = await _roomApi.JoinRoomAsync(room.Id, GlobalData.BotId.Value);

                    // 4. Mở form Room
                    frmRoom roomForm = new frmRoom(roomWithBot, GlobalData.UserId, GlobalData.Username);

                    roomForm.RoomReadyToShow += () => this.Hide();
                    roomForm.FormClosed += (s, args) => this.Show();
                    roomForm.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tạo phòng chơi với máy: " + ex.Message);
            }
        }

        private void btnChoiVoiNguoi_Click(object sender, EventArgs e)
        {
            frmLobby lobby = new frmLobby();

            lobby.LobbyReadyToShow += () =>
            {
                this.Hide();
            };

            lobby.FormClosed += (s, e1) =>
            {
                this.Show();
            };

            lobby.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var ask = MessageBox.Show(
        "Bạn có chắc chắn muốn đăng xuất?",
        "Xác nhận",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            this.Close();
        }

        private void frmMainMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            MusicManager.Stop();

            // Nếu muốn tắt hẳn chương trình luôn (không quay lại Login)
            // Thì bắt buộc phải có dòng này để giết form Login đang ẩn

            //Application.Exit();
        }
    }
}
