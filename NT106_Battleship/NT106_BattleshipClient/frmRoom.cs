using NT106_BattleshipClient.Models;
using NT106_BattleshipClient.Services;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.SignalR.Client;

namespace NT106_BattleshipClient
{
    public partial class frmRoom : BaseForm
    {
        private RoomDto _room;                 // Thông tin phòng hiện tại
        private int _currentUserId;            // ID user hiện tại
        private string _currentUsername;       // Username user hiện tại
        private readonly RoomApiService _roomApi = new RoomApiService();
        private bool _isLeaving = false;       // Tránh gọi rời phòng nhiều lần
        private int? _lastKnownGuestId = null; // Lưu ID khách lần trước
        private bool _isHost;          // true nếu người chơi là chủ phòng
        private bool _isGuestReady;    // khách đã bấm nút sẵn sàng

        private bool IsHost => _room.IDChuPhong == _currentUserId; // Kiểm tra có phải Host

        public frmRoom(RoomDto room, int userId, string username)
        {
            InitializeComponent();

            _room = room ?? throw new ArgumentNullException(nameof(room));
            _currentUserId = userId;
            _currentUsername = username;
            this.FormClosing += frmRoom_FormClosing; // Sự kiện đóng form

            _isHost = (_currentUserId == room.IDChuPhong);
            _isGuestReady = false;
        }

        private async void frmRoom_Load(object sender, EventArgs e)
        {
            // Full màn hình
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            this.Size = screen.Size;
            this.Location = new Point(0, 0);

            await UpdateRoomUIAsync(_room, firstLoad: true); // Hiển thị UI ban đầu

            try
            {
                SignalRClient.Init("http://localhost:5074/roomHub");
                await SignalRClient.StartAsync();

                // Nhận cập nhật phòng
                SignalRClient.Connection.On<RoomDto>("RoomUpdated", (roomDto) =>
                {
                    if (roomDto.Id != _room.Id) return;
                    _room = roomDto;
                    this.BeginInvoke(new Action(() =>
                    {
                        _ = UpdateRoomUIAsync(roomDto);
                    }));
                });

                // Nhận thông báo phòng bị xoá
                SignalRClient.Connection.On<int>("RoomDeleted", (deletedRoomId) =>
                {
                    if (deletedRoomId != _room.Id) return;
                    this.BeginInvoke(new Action(() =>
                    {
                        MessageBox.Show("Phòng đã bị xoá (chủ phòng rời).");
                        ReturnToLobby();
                    }));
                });

                // Nhận lệnh đồng bộ UI
                SignalRClient.Connection.On<string, string>("SynchronizeRoomUI", (command, value) =>
                {
                    if (!this.IsDisposed)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            ProcessIncomingData(command, value);
                        }));
                    }
                });

                // Nhận sự kiện khách đổi trạng thái sẵn sàng
                SignalRClient.Connection.On<bool>("GuestReadyStateChanged", (state) =>
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        _isGuestReady = state;
                        pnlTieuDeKhach.Text = state ? "KHÁCH đã sẵn sàng!" : "KHÁCH";
                    }));
                });



                // Tham gia group phòng
                try
                {
                    await SignalRClient.Connection.InvokeAsync("JoinRoom", _room.Id.ToString());
                }
                catch { }

                SetupUIControls(); // Thiết lập quyền điều khiển UI
            }
            catch { }
        }

        private void SetupUIControls()
        {
            cbKichThuoc.Enabled = IsHost; // Chỉ host chỉnh được size

            if (cbKichThuoc.Items.Count == 0)
            {
                cbKichThuoc.Items.AddRange(new object[] { "8x8", "9x9", "10x10" });
                cbKichThuoc.SelectedIndex = 0;
            }
        }

        // Gửi lệnh đồng bộ UI qua SignalR
        private async Task SendUISyncCommand(string command, string value)
        {
            try
            {
                if (SignalRClient.Connection.State == HubConnectionState.Connected)
                {
                    await SignalRClient.Connection.InvokeAsync("SendUISync", _room.Id.ToString(), command, value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi đồng bộ UI: {ex.Message}");
            }
        }

        // Xử lý lệnh Sync nhận từ server
        private void ProcessIncomingData(string command, string value)
        {
            switch (command)
            {
                case "SET_HOST_CHAR": lblNhanVatChuPhong.Text = "Nhân vật: " + value; break;
                case "SET_GUEST_CHAR": lblNhanVatKhach.Text = "Nhân vật: " + value; break;
                case "SET_SIZE":
                    if (cbKichThuoc.Items.Contains(value))
                        cbKichThuoc.SelectedItem = value;
                    break;
            }
        }

        // Cập nhật UI host/guest
        private async Task UpdateRoomUIAsync(RoomDto room, bool firstLoad = false)
        {
            if (room == null) return;

            // HOST
            if (room.IDChuPhong == _currentUserId)
            {
                lblTenChuPhong.Text = $"Tên: {_currentUsername}";
                lblIDChuPhong.Text = $"ID: {_currentUserId}";
            }
            else
            {
                lblIDChuPhong.Text = $"ID: {room.IDChuPhong}";

                if (!string.IsNullOrWhiteSpace(room.TenChuPhong))
                    lblTenChuPhong.Text = $"Tên: {room.TenChuPhong}";
                else
                {
                    try
                    {
                        var host = await _roomApi.GetUserByIdAsync(room.IDChuPhong);
                        lblTenChuPhong.Text = $"Tên: {host?.TenDangNhap ?? "(Không lấy được)"}";
                    }
                    catch { lblTenChuPhong.Text = "Tên: (Không lấy được)"; }
                }
            }

            // GUEST
            if (!room.IDKhach.HasValue)
            {
                lblTenKhach.Text = "Chưa có khách vào";
                lblIDKhach.Text = "Đang chờ khách vào";
                _lastKnownGuestId = null;
            }
            else
            {
                lblIDKhach.Text = $"ID: {room.IDKhach}";

                if (_lastKnownGuestId != room.IDKhach || firstLoad)
                {
                    if (!string.IsNullOrWhiteSpace(room.TenKhach))
                        lblTenKhach.Text = $"Tên: {room.TenKhach}";
                    else
                    {
                        try
                        {
                            var guest = await _roomApi.GetUserByIdAsync(room.IDKhach.Value);
                            lblTenKhach.Text = $"Tên: {guest?.TenDangNhap ?? "(Không lấy được)"}";
                        }
                        catch { lblTenKhach.Text = "Tên: (Không lấy được)"; }
                    }

                    _lastKnownGuestId = room.IDKhach;
                }
            }
        }

        // ==============================
        // Rời phòng (chỉ gọi 1 lần)
        // ==============================
        private async Task LeaveRoomAsync()
        {
            if (_isLeaving) return;
            _isLeaving = true;

            try
            {
                if (SignalRClient.Connection.State == HubConnectionState.Connected)
                {
                    try { await SignalRClient.Connection.InvokeAsync("LeaveRoom", _room.Id.ToString()); }
                    catch { }
                }

                await _roomApi.LeaveRoomAsync(_room.Id, _currentUserId);
            }
            catch { }
        }

        private async void btnThoatPhongCho_Click(object sender, EventArgs e)
        {
            await LeaveRoomAsync();
            ReturnToLobby();
        }

        private async void frmRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isLeaving) return;

            e.Cancel = true;      // Chặn đóng form trực tiếp
            await LeaveRoomAsync();
            ReturnToLobby();
        }

        private void ReturnToLobby()
        {
            if (!_isLeaving) return;

            var lobby = new frmLobby();
            lobby.Show();

            this.FormClosing -= frmRoom_FormClosing;
            this.Hide();
            this.Dispose();
        }

        // ==============================
        // Chat + chọn nhân vật
        // ==============================
        private void btnTinNhan_Click(object sender, EventArgs e)
        {
            ucChatBox1.Visible = !ucChatBox1.Visible; // Bật/tắt khung chat
            if (ucChatBox1.Visible) ucChatBox1.BringToFront();
        }

        private async void btnNVChuPhong_Click(object sender, EventArgs e)
        {
            if (!IsHost)
            {
                MessageBox.Show("Chỉ chủ phòng được chọn nhân vật cho chủ phòng.", "Lỗi");
                return;
            }

            frmSelectcharacter f = new frmSelectcharacter();
            if (f.ShowDialog() == DialogResult.OK)
            {
                string ten = f.TenNhanVatDaChon;
                lblNhanVatChuPhong.Text = "Nhân vật: " + ten;
                await SendUISyncCommand("SET_HOST_CHAR", ten);
            }
        }

        private async void btnNVKhach_Click(object sender, EventArgs e)
        {
            if (IsHost)
            {
                MessageBox.Show("Chỉ khách được chọn nhân vật cho khách.", "Lỗi");
                return;
            }

            frmSelectcharacter f = new frmSelectcharacter();
            if (f.ShowDialog() == DialogResult.OK)
            {
                string ten = f.TenNhanVatDaChon;
                lblNhanVatKhach.Text = "Nhân vật: " + ten;
                await SendUISyncCommand("SET_GUEST_CHAR", ten);
            }
        }

        private async void cbKichThuoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsHost)
            {
                MessageBox.Show("Chỉ chủ phòng được đổi kích thước.", "Lỗi");
                return;
            }

            if (cbKichThuoc.SelectedItem != null)
            {
                string size = cbKichThuoc.SelectedItem.ToString();
                await SendUISyncCommand("SET_SIZE", size);
            }
        }

        private async void btnSanSang_Click(object sender, EventArgs e)
        {
            if (_isHost)
            {
                MessageBox.Show("Chỉ khách mới được bấm nút này!");
                return;
            }

            if (lblNhanVatKhach.Text == "Nhân vật:")
            {
                MessageBox.Show("Hãy chọn nhân vật trước khi sẵn sàng!");
                return;
            }

            // Toggle trạng thái sẵn sàng
            _isGuestReady = !_isGuestReady;

            if (_isGuestReady)
                pnlTieuDeKhach.Text = "KHÁCH đã sẵn sàng!";
            else
                pnlTieuDeKhach.Text = "KHÁCH";

            // Báo cho host qua signalR (nếu bạn có)
            await SignalRClient.Connection.InvokeAsync("SetGuestReady", _room.Id, _isGuestReady);
        }

        private void btnBatDau_Click(object sender, EventArgs e) {
            if (!_isHost)
            {
                MessageBox.Show("Chỉ chủ phòng mới được bắt đầu trận!");
                return;
            }

            if (!_isGuestReady)
            {
                MessageBox.Show("Khách chưa sẵn sàng – không thể bắt đầu trận!");
                return;
            }

            // Kiểm tra nhân vật chủ phòng
            if (lblNhanVatChuPhong.Text == "Nhân vật:")
            {
                MessageBox.Show("Hãy chọn nhân vật trước khi bắt đầu trận!");
                return;
            }

            // Kiểm tra kích thước trận đấu
            if (cbKichThuoc.SelectedItem == null)
            {
                MessageBox.Show("Hãy chọn kích thước trận đấu trước khi bắt đầu!");
                return;
            }

            // Báo cho khách chuẩn bị vào game
            SignalRClient.Connection.InvokeAsync("StartGame", _room.Id);

            // Chủ phòng vào xếp tàu
            MessageBox.Show("Xếp tàu");
        }
    }
}
