using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media; // 👈 Cần thiết cho SoundPlayer
using System.IO; // 👈 Cần thiết để xử lý lỗi (ví dụ: FileNotFoundException)

namespace NT106_BattleshipClient
{
    // Đảm bảo BaseForm kế thừa từ System.Windows.Forms.Form
    public partial class BaseForm : Form
    {
        // 1. KHAI BÁO BIẾN CHO CON TRỎ VÀ ÂM THANH
        protected Cursor cursorDefault = null;
        protected Cursor cursorClick = null;

        // ✅ ĐÃ BẬT LẠI (UNCOMMENTED)
        protected System.Media.SoundPlayer clickSoundPlayer = null;

        public BaseForm()
        {
            InitializeComponent();

            // Chỉ chạy code tải tài nguyên khi ứng dụng chạy (không chạy trong Designer)
            if (!DesignMode)
            {
                LoadCustomCursorsAndSound();
                
            }
        }

        // 2. HÀM TẢI CON TRỎ VÀ ÂM THANH
        private void LoadCustomCursorsAndSound()
        {
            // ✅ CHỈ CẦN DÙNG MỘT ĐƯỜNG DẪN NÀY
            string resourcesPath = "Resources/Cursor/";

            // (Xóa dòng 'string soundPath = "Resources/Cursor";' nếu còn)

            try
            {
                // Tải Con trỏ
                cursorDefault = new Cursor(resourcesPath + "CursorImage.cur");
                cursorClick = new Cursor(resourcesPath + "CursorAnimation.cur");

                // Đặt con trỏ mặc định
                this.Cursor = cursorDefault;

                // ✅ TẢI ÂM THANH (Sử dụng 'resourcesPath' đã đúng)
                clickSoundPlayer = new System.Media.SoundPlayer(resourcesPath + "ClickSound.wav");
                clickSoundPlayer.Load();
            }
            catch (Exception ex)
            {
                // Báo lỗi nếu không tìm thấy file
                MessageBox.Show("Lỗi tải tài nguyên con trỏ/âm thanh: " + ex.Message, "Lỗi Tải Tài Nguyên", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 3. HÀM GẮN SỰ KIỆN CHUỘT (Không thay đổi)
        protected void AttachMouseEventsRecursive(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                // Gắn sự kiện cho Control hiện tại
                control.MouseEnter += OnControlMouseEnter;
                control.MouseLeave += OnControlMouseLeave;
                control.MouseDown += OnControlMouseDown;
                control.MouseUp += OnControlMouseUp;

                // Gọi đệ quy cho các Control con
                if (control.HasChildren)
                {
                    AttachMouseEventsRecursive(control);
                }
            }
        }

        // 4. CÁC HÀM XỬ LÝ SỰ KIỆN MOUSE

        protected virtual void OnControlMouseEnter(object sender, EventArgs e)
        {
            this.Cursor = cursorDefault;
        }

        protected virtual void OnControlMouseLeave(object sender, EventArgs e)
        {
            this.Cursor = cursorDefault;
        }

        protected virtual void OnControlMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && cursorClick != null)
            {
                this.Cursor = cursorClick; // Đổi con trỏ

                // ✅ PHÁT ÂM THANH (Đã bật lại)
                if (clickSoundPlayer != null)
                {
                    clickSoundPlayer.Stop();
                    clickSoundPlayer.Play();
                }
            }
        }

        protected virtual void OnControlMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && cursorDefault != null)
            {
                this.Cursor = cursorDefault; // Trả về con trỏ mặc định
            }
        }

        // Hàm này để trống, dùng để sửa lỗi CS1061 trong Designer
        private void BaseForm_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                AttachMouseEventsRecursive(this);
            }
        }
    }
}