using Microsoft.AspNetCore.SignalR.Client;
using NT106_BattleshipClient.Services;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmLobby : BaseForm
    {
        public event Action LobbyReadyToShow;

        private readonly RoomApiService _roomApi = new RoomApiService();
        private int _currentUserId;
        private bool _signalRInitialized = false;


        public frmLobby()
        {
            // Tối ưu vẽ form
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint, true);

            _currentUserId = GlobalData.UserId;
            InitializeComponent();
            FormManager.frmLobby = this;
            //// chống nháy form
            //EnableFormDoubleBuffering();
            //SetUseComposited(true);

            this.Opacity = 0; // ẨN FORM ĐI
        }

        private async void frmLobby_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            // Kết nối SignalR
            if (!_signalRInitialized)
            {
                try
                {
                    SignalRClient.Init("http://localhost:5074/roomHub");
                    await SignalRClient.StartAsync();

                    RegisterLobbySignalREvents();

                    _signalRInitialized = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            await LoadRoomsFromServer();

            this.ResumeLayout(true);

            label1.Focus();

            // Cuộn scrollbar → cuộn dgv
            guna2VScrollBar1.Scroll += (s, E) =>
            {
                if (dgvDanhSachPhong.RowCount == 0) return;

                int maxIndex = dgvDanhSachPhong.RowCount - dgvDanhSachPhong.DisplayedRowCount(false);
                if (maxIndex < 0) maxIndex = 0;

                int v = guna2VScrollBar1.Value;
                if (v < 0) v = 0;
                if (v > maxIndex) v = maxIndex;

                dgvDanhSachPhong.FirstDisplayedScrollingRowIndex = v;
            };


            // Bỏ chọn ô
            dgvDanhSachPhong.ClearSelection();
            dgvDanhSachPhong.CurrentCell = null;
        }

        private void RegisterLobbySignalREvents()
        {
            var conn = SignalRClient.Connection;

            // Gỡ bỏ các handler cũ nếu có
            conn.Remove("RoomListUpdated");
            conn.Remove("RoomDeleted");

            // Đăng ký các handler mới
            conn.On("RoomListUpdated", () =>
            {
                if (IsDisposed || !IsHandleCreated) return;

                BeginInvoke(new Action(async () =>
                {
                    await LoadRoomsFromServer();
                }));
            });

            conn.On<int>("RoomDeleted", (_) =>
            {
                if (IsDisposed || !IsHandleCreated) return;

                BeginInvoke(new Action(async () =>
                {
                    await LoadRoomsFromServer();
                }));
            });
        }


        private void UpdateScrollBar()
        {
            int total = dgvDanhSachPhong.RowCount;
            int visible = dgvDanhSachPhong.DisplayedRowCount(false);

            if (visible >= total)
            {
                guna2VScrollBar1.Visible = false;
            }
            else
            {
                guna2VScrollBar1.Visible = true;
                guna2VScrollBar1.Minimum = 0;
                guna2VScrollBar1.Maximum = total - visible;
                guna2VScrollBar1.LargeChange = 1;
            }
        }

        private async Task LoadRoomsFromServer()
        {
            dgvDanhSachPhong.SuspendLayout();

            dgvDanhSachPhong.Rows.Clear();

            var rooms = await _roomApi.GetRoomsAsync();
            if (rooms == null)
            {
                dgvDanhSachPhong.ResumeLayout();
                return;
            }
            foreach (var r in rooms)
            {
                string trangThaiHienThi = "";

                // Map trạng thái từ Server sang Tiếng Việt hiển thị
                switch (r.TrangThai)
                {
                    case "waiting": trangThaiHienThi = "Đang chờ"; break;
                    case "full": trangThaiHienThi = "Đầy"; break;
                    case "playing": trangThaiHienThi = "Đang chơi"; break;
                    default: trangThaiHienThi = r.TrangThai; break;
                }

                string tenChu = !string.IsNullOrWhiteSpace(r.TenChuPhong)
                                ? r.TenChuPhong
                                : ("ID: " + r.IDChuPhong);

                dgvDanhSachPhong.Rows.Add(r.Id, tenChu, trangThaiHienThi);

                dgvDanhSachPhong.Rows[dgvDanhSachPhong.Rows.Count - 1].Tag = r.TrangThai;
            }

            UpdateScrollBar();

            dgvDanhSachPhong.ResumeLayout();
        }

        private async void dgvDanhSachPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int colJoin = dgvDanhSachPhong.Columns["colThamGia"].Index;

            // Nhấn nút "Vào"
            if (e.ColumnIndex == colJoin)
            {
                string trangThai = dgvDanhSachPhong.Rows[e.RowIndex].Cells["colTrangThai"].Value.ToString();

                if (trangThai != "Đang chờ")
                {
                    // Nếu click vào phòng Đầy hoặc Đang chơi -> Không làm gì cả
                    return;
                }

                int roomId = Convert.ToInt32(dgvDanhSachPhong.Rows[e.RowIndex]
                                             .Cells["colID"].Value);

                // Gọi API join
                var room = await _roomApi.JoinRoomAsync(roomId, _currentUserId);

                // Mở form phòng
                frmRoom roomForm = new frmRoom(room, _currentUserId, GlobalData.Username);

                roomForm.RoomReadyToShow += () =>
                {
                    this.Hide();
                };

                roomForm.FormClosed += (s, args) =>
                {
                    this.Show();
                };

                roomForm.Show();
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnTaoPhong_Click(object sender, EventArgs e)
        {
            // Tạo phòng mới → user là host
            var room = await _roomApi.CreateRoomAsync(_currentUserId);
            // Mở form phòng
            frmRoom roomForm = new frmRoom(room, _currentUserId, GlobalData.Username);

            roomForm.RoomReadyToShow += () =>
            {
                this.Hide();
            };

            roomForm.FormClosed += (s, args) =>
            {
                this.Show();
            };

            roomForm.Show();
        }

        private void frmLobby_Shown(object sender, EventArgs e)
        {
            SetPlaceholder();

            // Cho WinForms vẽ xong hết rồi mới hiện
            this.BeginInvoke(new Action(() =>
            {
                LobbyReadyToShow?.Invoke(); // Báo hiệu cho bên gọi biết là Lobby đã sẵn sàng hiển thị
                this.Opacity = 1; //HIỆN FORM
            }));
        }

        private void SetPlaceholder()
        {
            txtTimTaoPhong.Text = "Nhập ID hoặc tên chủ phòng";
            txtTimTaoPhong.ForeColor = Color.SteelBlue;
        }

        private void txtTimTaoPhong_Enter(object sender, EventArgs e)
        {
            // Placeholder
            if (txtTimTaoPhong.Text == "Nhập ID hoặc tên chủ phòng")
                txtTimTaoPhong.Text = "";
        }

        private void txtTimTaoPhong_Leave(object sender, EventArgs e)
        {
            // Khôi phục placeholder
            if (string.IsNullOrWhiteSpace(txtTimTaoPhong.Text))
            {
                txtTimTaoPhong.Text = "Nhập ID hoặc tên chủ phòng";
                txtTimTaoPhong.ForeColor = Color.SteelBlue;
            }
        }

        private void dgvDanhSachPhong_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            const int HORIZONTAL_PADDING = 30;
            const int VERTICAL_PADDING = 3;

            // Vẽ nút "Vào" cho cột tham gia
            if (e.ColumnIndex == dgvDanhSachPhong.Columns["colThamGia"].Index && e.RowIndex >= 0)
            {
                string trangThai = dgvDanhSachPhong.Rows[e.RowIndex]
                                   .Cells["colTrangThai"].Value?.ToString();

                if (trangThai == "Đang chờ")
                {
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

                    Rectangle rect = new Rectangle(
                        e.CellBounds.X + HORIZONTAL_PADDING,
                        e.CellBounds.Y + VERTICAL_PADDING,
                        e.CellBounds.Width - 2 * HORIZONTAL_PADDING,
                        e.CellBounds.Height - 2 * VERTICAL_PADDING
                    );

                    using (SolidBrush b = new SolidBrush(Color.LightSkyBlue))
                        e.Graphics.FillRectangle(b, rect);

                    using (Font boldFont = new Font("Segoe UI", 12, FontStyle.Bold))
                        e.Graphics.DrawString("Vào", boldFont, new SolidBrush(Color.SteelBlue), e.CellBounds,
                            new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    e.Handled = true;
                }
                else
                {
                    // Phòng đầy → chỉ vẽ nền
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                    e.Handled = true;
                }
            }
        }


        //test hàm chống nháy
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetControlDoubleBuffered(dgvDanhSachPhong);
            SetControlDoubleBuffered(pnlTimTaoPhong);
            SetControlDoubleBuffered(panel2);
            SetControlDoubleBuffered(panel3);
            SetControlDoubleBuffered(panel1);

            //SetControlDoubleBuffered(ucChatBox1);
            SetDoubleBufferedForAllChildrenExceptTextBox(this);
        }

        private void SetDoubleBufferedForAllChildrenExceptTextBox(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                // BỎ QUA TextBox
                if (c is TextBox) continue;

                SetControlDoubleBuffered(c);

                if (c.HasChildren)
                    SetDoubleBufferedForAllChildrenExceptTextBox(c);
            }
        }

        private void btnTimPhong_Click(object sender, EventArgs e)
        {
            string keyword = txtTimTaoPhong.Text.Trim().ToLower();

            // Nếu chưa nhập gì hoặc còn placeholder → hiện lại tất cả
            if (string.IsNullOrWhiteSpace(keyword) || keyword == "nhập id hoặc tên chủ phòng")
            {
                foreach (DataGridViewRow row in dgvDanhSachPhong.Rows)
                    row.Visible = true;

                UpdateScrollBar();
                return;
            }

            foreach (DataGridViewRow row in dgvDanhSachPhong.Rows)
            {
                if (row.IsNewRow) continue;

                string roomId = row.Cells["colID"].Value?.ToString().ToLower() ?? "";
                string hostName = row.Cells["colTenChuPhong"].Value?.ToString().ToLower() ?? "";

                // Match nếu chứa keyword
                bool match =
                    roomId.Contains(keyword) ||
                    hostName.Contains(keyword);

                row.Visible = match;
            }

            UpdateScrollBar();
        }
    }
}
