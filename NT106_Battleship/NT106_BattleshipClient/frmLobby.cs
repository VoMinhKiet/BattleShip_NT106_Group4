//using NT106_BattleshipClient.Services;
//using System;
//using System.Data;
//using System.Drawing;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace NT106_BattleshipClient
//{
//    public partial class frmLobby : BaseForm
//    {
//        private RoomApiService _roomApi = new RoomApiService();
//        private int _currentUserId;

//        public frmLobby()
//        {
//            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint |
//                          ControlStyles.AllPaintingInWmPaint, true);
//            _currentUserId = GlobalData.UserId;
//            InitializeComponent();
//        }

//        private async void frmLobby_Load(object sender, EventArgs e)
//        {
//            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
//            this.Size = screen.Size;
//            this.Location = new Point(0, 0);

//            await LoadRoomsFromServer();
//            label1.Focus();

//            // Scrollbar đồng bộ
//            guna2VScrollBar1.Scroll += (s, E) =>
//            {
//                int maxIndex = dgvDanhSachPhong.RowCount - 1;
//                int scrollValue = Math.Min(guna2VScrollBar1.Value, maxIndex);
//                dgvDanhSachPhong.FirstDisplayedScrollingRowIndex = scrollValue;
//            };

//            int visibleRows = dgvDanhSachPhong.DisplayedRowCount(true);
//            int totalRows = dgvDanhSachPhong.RowCount;

//            guna2VScrollBar1.Visible = totalRows > visibleRows;
//            guna2VScrollBar1.Maximum = totalRows;

//            dgvDanhSachPhong.ClearSelection();
//            dgvDanhSachPhong.CurrentCell = null;

//            txtTimTaoPhong.Text = "Nhập ID hoặc tên chủ phòng";
//        }

//        private async Task LoadRoomsFromServer()
//        {
//            dgvDanhSachPhong.Rows.Clear();

//            var rooms = await _roomApi.GetRoomsAsync();

//            if (rooms == null)
//            {
//                Console.WriteLine("Không có phòng nào được trả về từ server.");
//                return;
//            }

//            foreach (var r in rooms)
//            {
//                string trangThai = r.TrangThai == "waiting" ? "Trống" : "Đầy";
//                dgvDanhSachPhong.Rows.Add(r.Id, "Phòng của " + r.IDChuPhong, trangThai);
//            }
//        }

//        //
//        private void dgvTimPhong_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
//        {
//            const int HORIZONTAL_PADDING = 30;
//            const int VERTICAL_PADDING = 3;

//            if (e.ColumnIndex == dgvDanhSachPhong.Columns["colThamGia"].Index && e.RowIndex >= 0)
//            {
//                string trangThai = dgvDanhSachPhong.Rows[e.RowIndex]
//                                   .Cells["colTrangThai"].Value?.ToString();

//                if (trangThai == "Trống")
//                {
//                    e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

//                    Color buttonBackColor = Color.LightSkyBlue;
//                    Color buttonForeColor = Color.SteelBlue;

//                    Rectangle buttonRect = new Rectangle(
//                        e.CellBounds.X + HORIZONTAL_PADDING,
//                        e.CellBounds.Y + VERTICAL_PADDING,
//                        e.CellBounds.Width - 2 * HORIZONTAL_PADDING,
//                        e.CellBounds.Height - 2 * VERTICAL_PADDING);

//                    using (SolidBrush backBrush = new SolidBrush(buttonBackColor))
//                    {
//                        e.Graphics.FillRectangle(backBrush, buttonRect);
//                        e.Graphics.DrawRectangle(Pens.DarkBlue, buttonRect);
//                    }

//                    using (StringFormat sf = new StringFormat()
//                    {
//                        Alignment = StringAlignment.Center,
//                        LineAlignment = StringAlignment.Center
//                    })
//                    using (Font boldFont = new Font("Segoe UI", 12, FontStyle.Bold))
//                    {
//                        e.Graphics.DrawString("Vào", boldFont,
//                            new SolidBrush(buttonForeColor), e.CellBounds, sf);
//                    }

//                    e.Handled = true;
//                }
//                else
//                {
//                    e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
//                    e.Handled = true;
//                }
//            }
//        }

//        private async void dgvDanhSachPhong_CellClick(object sender, DataGridViewCellEventArgs e)
//        {
//            if (e.RowIndex < 0) return;

//            int colJoin = dgvDanhSachPhong.Columns["colThamGia"].Index;

//            if (e.ColumnIndex == colJoin)
//            {
//                int roomId = Convert.ToInt32(dgvDanhSachPhong.Rows[e.RowIndex]
//                                             .Cells["colID"].Value);
//                string status = dgvDanhSachPhong.Rows[e.RowIndex]
//                                .Cells["colTrangThai"].Value.ToString();

//                if (status == "Đầy")
//                {
//                    MessageBox.Show("Phòng đã đầy!");
//                    return;
//                }

//                var room = await _roomApi.JoinRoomAsync(roomId, _currentUserId);

//                frmRoom roomForm = new frmRoom(room, _currentUserId, GlobalData.Username);
//                roomForm.Show();
//                this.Hide();
//            }
//        }

//        private void btnThoat_Click(object sender, EventArgs e)
//        {
//            this.Close();
//            frmMainMenu mainMenu = new frmMainMenu();
//            mainMenu.Show();
//        }

//        private async void btnTaoPhong_Click(object sender, EventArgs e)
//        {
//            var room = await _roomApi.CreateRoomAsync(_currentUserId);

//            // Truyền UserId + Username vào frmRoom
//            frmRoom roomForm = new frmRoom(room, _currentUserId, GlobalData.Username);

//            roomForm.Show();
//            this.Hide();
//        }

//        private void txtTimTaoPhong_Enter(object sender, EventArgs e)
//        {
//            if (txtTimTaoPhong.Text == "Nhập ID hoặc tên chủ phòng")
//                txtTimTaoPhong.Text = "";
//        }

//        private void txtTimTaoPhong_Leave(object sender, EventArgs e)
//        {
//            if (string.IsNullOrWhiteSpace(txtTimTaoPhong.Text))
//                txtTimTaoPhong.Text = "Nhập ID hoặc tên chủ phòng";
//        }
//    }
//}

using NT106_BattleshipClient.Services;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmLobby : BaseForm
    {
        private RoomApiService _roomApi = new RoomApiService();
        private int _currentUserId;

        public frmLobby()
        {
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint, true);
            _currentUserId = GlobalData.UserId;
            InitializeComponent();
        }

        private async void frmLobby_Load(object sender, EventArgs e)
        {
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            this.Size = screen.Size;
            this.Location = new Point(0, 0);
            await LoadRoomsFromServer();
            label1.Focus();

            // Scrollbar đồng bộ
            guna2VScrollBar1.Scroll += (s, E) =>
            {
                int maxIndex = dgvDanhSachPhong.RowCount - 1;
                int scrollValue = Math.Min(guna2VScrollBar1.Value, maxIndex);
                dgvDanhSachPhong.FirstDisplayedScrollingRowIndex = scrollValue;
            };

            dgvDanhSachPhong.ClearSelection();
            dgvDanhSachPhong.CurrentCell = null;
            txtTimTaoPhong.Text = "Nhập ID hoặc tên chủ phòng";
        }

        private async Task LoadRoomsFromServer()
        {
            dgvDanhSachPhong.Rows.Clear();
            var rooms = await _roomApi.GetRoomsAsync();
            if (rooms == null)
            {
                Console.WriteLine("Không có phòng nào được trả về từ server.");
                return;
            }

            foreach (var r in rooms)
            {
                string trangThai = r.TrangThai == "waiting" ? "Trống" : "Đầy";

                // HIỂN THỊ TÊN CHỦ PHÒNG (nếu server trả)
                string tenChu = !string.IsNullOrWhiteSpace(r.TenChuPhong)
                    ? r.TenChuPhong
                    : ("ID: " + r.IDChuPhong);

                dgvDanhSachPhong.Rows.Add(r.Id, tenChu, trangThai);
            }
        }

        private async void dgvDanhSachPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int colJoin = dgvDanhSachPhong.Columns["colThamGia"].Index;
            if (e.ColumnIndex == colJoin)
            {
                int roomId = Convert.ToInt32(dgvDanhSachPhong.Rows[e.RowIndex]
                                             .Cells["colID"].Value);
                string status = dgvDanhSachPhong.Rows[e.RowIndex]
                                .Cells["colTrangThai"].Value.ToString();
                if (status == "Đầy")
                {
                    MessageBox.Show("Phòng đã đầy!");
                    return;
                }
                var room = await _roomApi.JoinRoomAsync(roomId, _currentUserId);
                // **Quan trọng**: constructor frmRoom dùng RoomDto, đảm bảo _room.Id được lưu trong frmRoom
                frmRoom roomForm = new frmRoom(room, _currentUserId, GlobalData.Username);
                roomForm.Show();
                this.Hide();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
            frmMainMenu mainMenu = new frmMainMenu();
            mainMenu.Show();
        }

        private async void btnTaoPhong_Click(object sender, EventArgs e)
        {
            var room = await _roomApi.CreateRoomAsync(_currentUserId);
            frmRoom roomForm = new frmRoom(room, _currentUserId, GlobalData.Username);
            roomForm.Show();
            this.Hide();
        }

        private void txtTimTaoPhong_Enter(object sender, EventArgs e)
        {
            if (txtTimTaoPhong.Text == "Nhập ID hoặc tên chủ phòng")
                txtTimTaoPhong.Text = "";
        }

        private void txtTimTaoPhong_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTimTaoPhong.Text))
                txtTimTaoPhong.Text = "Nhập ID hoặc tên chủ phòng";
        }

        private void dgvTimPhong_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            const int HORIZONTAL_PADDING = 30;
            const int VERTICAL_PADDING = 3;

            if (e.ColumnIndex == dgvDanhSachPhong.Columns["colThamGia"].Index && e.RowIndex >= 0)
            {
                string trangThai = dgvDanhSachPhong.Rows[e.RowIndex]
                                   .Cells["colTrangThai"].Value?.ToString();

                if (trangThai == "Trống")
                {
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

                    Color buttonBackColor = Color.LightSkyBlue;
                    Color buttonForeColor = Color.SteelBlue;

                    Rectangle buttonRect = new Rectangle(
                        e.CellBounds.X + HORIZONTAL_PADDING,
                        e.CellBounds.Y + VERTICAL_PADDING,
                        e.CellBounds.Width - 2 * HORIZONTAL_PADDING,
                        e.CellBounds.Height - 2 * VERTICAL_PADDING);

                    using (SolidBrush backBrush = new SolidBrush(buttonBackColor))
                    {
                        e.Graphics.FillRectangle(backBrush, buttonRect);
                        e.Graphics.DrawRectangle(Pens.DarkBlue, buttonRect);
                    }

                    using (StringFormat sf = new StringFormat()
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    })
                    using (Font boldFont = new Font("Segoe UI", 12, FontStyle.Bold))
                    {
                        e.Graphics.DrawString("Vào", boldFont,
                            new SolidBrush(buttonForeColor), e.CellBounds, sf);
                    }

                    e.Handled = true;
                }
                else
                {
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                    e.Handled = true;
                }
            }
        }
    }
}

