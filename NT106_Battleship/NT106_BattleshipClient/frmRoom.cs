//using NT106_BattleshipClient.Models;
//using NT106_BattleshipClient.Services;
//using System;
//using System.Drawing;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace NT106_BattleshipClient
//{
//    public partial class frmRoom : BaseForm
//    {
//        private RoomDto _room;
//        private int _currentUserId;
//        private string _currentUsername;
//        private readonly RoomApiService _roomApi = new RoomApiService();
//        private bool _isClosing = false;
//        private int? _lastKnownGuestId = null;

//        public frmRoom(RoomDto room, int userId, string username)
//        {
//            InitializeComponent();
//            _room = room;
//            _currentUserId = userId;
//            _currentUsername = username;

//            // Gắn sự kiện FormClosing
//            this.FormClosing += frmRoom_FormClosing;
//        }

//        private async void frmRoom_Load(object sender, EventArgs e)
//        {
//            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
//            this.Size = screen.Size;
//            this.Location = new Point(0, 0);

//            // Hiển thị thông tin ngay khi mở
//            await UpdateRoomUIAsync(_room, firstLoad: true);

//            // Bật timer polling (1s)
//            timerCheckRoom.Interval = 1000;
//            timerCheckRoom.Tick += timerCheckRoom_Tick;
//            timerCheckRoom.Start();
//        }

//        // Cập nhật UI (host/guest names and ids)
//        private async Task UpdateRoomUIAsync(RoomDto room, bool firstLoad = false)
//        {
//            if (room == null) return;

//            // Host info: nếu host là chính bạn -> dùng _currentUsername để tránh gọi API thừa
//            if (room.IDChuPhong == _currentUserId)
//            {
//                lblTenChuPhong.Text = $"Tên: {_currentUsername}";
//                lblIDChuPhong.Text = $"ID: {_currentUserId}";
//            }
//            else
//            {
//                try
//                {
//                    var host = await _roomApi.GetUserByIdAsync(room.IDChuPhong);
//                    lblTenChuPhong.Text = $"Tên: {host.TenDangNhap}";
//                    lblIDChuPhong.Text = $"ID: {host.Id}";
//                }
//                catch
//                {
//                    lblTenChuPhong.Text = $"Tên: (Không lấy được)";
//                    lblIDChuPhong.Text = $"ID: {room.IDChuPhong}";
//                }
//            }

//            // Guest info
//            if (!room.IDKhach.HasValue)
//            {
//                lblTenKhach.Text = "Chưa có khách vào";
//                lblIDKhach.Text = "Đang chờ khách vào";
//                _lastKnownGuestId = null;
//            }
//            else
//            {
//                // Nếu guestId thay đổi (mới join) hoặc lần đầu load, cập nhật tên
//                if (_lastKnownGuestId != room.IDKhach || firstLoad)
//                {
//                    try
//                    {
//                        var guest = await _roomApi.GetUserByIdAsync(room.IDKhach.Value);
//                        lblTenKhach.Text = $"Tên: {guest.TenDangNhap}";
//                        lblIDKhach.Text = $"ID: {guest.Id}";
//                    }
//                    catch
//                    {
//                        // Fallback hiển thị ID nếu không lấy được tên
//                        lblTenKhach.Text = "Tên: (Không lấy được)";
//                        lblIDKhach.Text = $"ID: {room.IDKhach}";
//                    }
//                    _lastKnownGuestId = room.IDKhach;
//                }
//            }
//        }

//        // Timer tick: poll server for latest room info
//        private async void timerCheckRoom_Tick(object sender, EventArgs e)
//        {
//            try
//            {
//                var room = await _roomApi.GetRoomByIdAsync(_room.Id);

//                if (room == null)
//                {
//                    timerCheckRoom.Stop();
//                    MessageBox.Show("Phòng đã bị xoá (chủ phòng rời).");
//                    var lobby = new frmLobby();
//                    lobby.Show();
//                    this.FormClosing -= frmRoom_FormClosing;
//                    this.Close();
//                    return;
//                }

//                bool guestChanged = room.IDKhach != _lastKnownGuestId;

//                if (guestChanged || room.TrangThai != _room.TrangThai)
//                {
//                    _room = room;
//                    _lastKnownGuestId = room.IDKhach;

//                    await UpdateRoomUIAsync(room);
//                }
//            }
//            catch
//            {
//                // Bỏ qua lỗi mạng tạm thời
//            }
//        }

//        private async Task UpdateRoomUIAsync(RoomDto room)
//        {
//            // Hiển thị ID chủ phòng
//            lblIDChuPhong.Text = "ID: " + room.IDChuPhong;

//            // Hiển thị ID khách
//            if (room.IDKhach == null)
//                lblIDKhach.Text = "Đang chờ khách...";
//            else
//                lblIDKhach.Text = "ID: " + room.IDKhach.Value;

//            // Gọi service
//            var userApi = new UserApiService();

//            // ===== Lấy tên Chủ Phòng =====
//            var host = await userApi.GetUserByIdAsync(room.IDChuPhong);
//            lblTenChuPhong.Text = "Tên: " + (host?.TenDangNhap ?? "Unknown");

//            // ===== Lấy tên Khách =====
//            if (room.IDKhach != null)
//            {
//                var guest = await userApi.GetUserByIdAsync(room.IDKhach.Value);
//                lblTenKhach.Text = "Tên: " + (guest?.TenDangNhap ?? "Waiting…");
//            }
//            else
//            {
//                lblTenKhach.Text = "Chưa có khách vào!";
//            }
//        }


//        // Nút thoát phòng (host/guest)
//        private async void btnThoatPhongCho_Click(object sender, EventArgs e)
//        {
//            // Disable timer while closing
//            timerCheckRoom.Stop();
//            try
//            {
//                await _roomApi.LeaveRoomAsync(_room.Id, _currentUserId);
//            }
//            catch
//            {
//                // ignore
//            }
//            var lobby = new frmLobby();
//            lobby.Show();
//            this.FormClosing -= frmRoom_FormClosing;
//            this.Close();
//        }

//        // FormClosing: đảm bảo rời phòng khi người đóng cửa sổ (bấm X)
//        private async void frmRoom_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            if (_isClosing) return;

//            // Nếu timer đang chạy, tạm dừng và thực hiện leave async
//            e.Cancel = true;
//            _isClosing = true;
//            timerCheckRoom.Stop();

//            try
//            {
//                await _roomApi.LeaveRoomAsync(_room.Id, _currentUserId);
//            }
//            catch
//            {
//                // ignore
//            }

//            // Mở lại lobby
//            var lobby = new frmLobby();
//            lobby.Show();

//            // Tháo handler để không lặp
//            this.FormClosing -= frmRoom_FormClosing;

//            // Đóng form lần nữa (không gọi FormClosing handler)
//            this.Close();
//        }

//        private void btnTinNhan_Click(object sender, EventArgs e)
//        {
//            ucChatBox1.Visible = !ucChatBox1.Visible;
//            if (ucChatBox1.Visible) ucChatBox1.BringToFront();

//        }

//        private void btnNVChuPhong_Click(object sender, EventArgs e)
//        {
//            frmSelectcharacter selectForm = new frmSelectcharacter();

//            if (selectForm.ShowDialog() == DialogResult.OK)
//            {
//                string tenNV = selectForm.TenNhanVatDaChon;
//                lblNhanVatChuPhong.Text = "Nhân vật: " + tenNV;
//            }
//        }

//        private void btnNVKhach_Click(object sender, EventArgs e)
//        {
//            frmSelectcharacter selectForm = new frmSelectcharacter();

//            if (selectForm.ShowDialog() == DialogResult.OK)
//            {
//                string tenNV = selectForm.TenNhanVatDaChon;
//                lblNhanVatKhach.Text = "Nhân vật: " + tenNV;
//            }
//        }
//    }
//}

using NT106_BattleshipClient.Models;
using NT106_BattleshipClient.Services;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmRoom : BaseForm
    {
        private RoomDto _room;
        private int _currentUserId;
        private string _currentUsername;
        private readonly RoomApiService _roomApi = new RoomApiService();
        private bool _isClosing = false;
        private int? _lastKnownGuestId = null;

        public frmRoom(RoomDto room, int userId, string username)
        {
            InitializeComponent();
            _room = room ?? throw new ArgumentNullException(nameof(room));
            _currentUserId = userId;
            _currentUsername = username;
            this.FormClosing += frmRoom_FormClosing;

            // **Quan trọng**: đảm bảo _room.Id được lưu
            // (room.Id có giá trị bởi server trả về khi Create/Join)
        }

        private async void frmRoom_Load(object sender, EventArgs e)
        {
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            this.Size = screen.Size;
            this.Location = new Point(0, 0);

            // Hiển thị thông tin ngay khi mở
            await UpdateRoomUIAsync(_room, firstLoad: true);

            // Bật timer polling (1s)
            timerCheckRoom.Interval = 1000;
            timerCheckRoom.Tick += timerCheckRoom_Tick;
            timerCheckRoom.Start();
        }

        /// <summary>
        /// Duy nhất 1 hàm UpdateRoomUIAsync (không có overload trùng)
        /// cập nhật tên/ID host + guest; firstLoad dùng để buộc update lần đầu
        /// </summary>
        private async Task UpdateRoomUIAsync(RoomDto room, bool firstLoad = false)
        {
            if (room == null) return;

            // HOST
            if (room.IDChuPhong == _currentUserId)
            {
                // bạn là host
                lblTenChuPhong.Text = $"Tên: {_currentUsername}";
                lblIDChuPhong.Text = $"ID: {_currentUserId}";
            }
            else
            {
                // Lấy tên host từ RoomDto (server đã trả TenChuPhong), fallback gọi API nếu NULL
                if (!string.IsNullOrWhiteSpace(room.TenChuPhong))
                {
                    lblTenChuPhong.Text = $"Tên: {room.TenChuPhong}";
                    lblIDChuPhong.Text = $"ID: {room.IDChuPhong}";
                }
                else
                {
                    try
                    {
                        var hostObj = await _roomApi.GetUserByIdAsync(room.IDChuPhong);
                        lblTenChuPhong.Text = $"Tên: {hostObj?.TenDangNhap ?? "(Không lấy được)"}";
                        lblIDChuPhong.Text = $"ID: {hostObj?.Id ?? room.IDChuPhong}";
                    }
                    catch
                    {
                        lblTenChuPhong.Text = $"Tên: (Không lấy được)";
                        lblIDChuPhong.Text = $"ID: {room.IDChuPhong}";
                    }
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
                // Nếu lần đầu load hoặc guestId thay đổi -> cập nhật
                if (_lastKnownGuestId != room.IDKhach || firstLoad)
                {
                    lblIDKhach.Text = $"ID: {room.IDKhach.Value}";

                    if (!string.IsNullOrWhiteSpace(room.TenKhach))
                    {
                        lblTenKhach.Text = $"Tên: {room.TenKhach}";
                    }
                    else
                    {
                        try
                        {
                            var guestObj = await _roomApi.GetUserByIdAsync(room.IDKhach.Value);
                            lblTenKhach.Text = $"Tên: {guestObj?.TenDangNhap ?? "(Không lấy được)"}";
                        }
                        catch
                        {
                            lblTenKhach.Text = "Tên: (Không lấy được)";
                        }
                    }

                    _lastKnownGuestId = room.IDKhach;
                }
            }
        }

        private async void timerCheckRoom_Tick(object sender, EventArgs e)
        {
            try
            {
                // Gọi server lấy trạng thái mới (dùng _room.Id — đã được set khi join/create)
                var room = await _roomApi.GetRoomByIdAsync(_room.Id);
                if (room == null)
                {
                    timerCheckRoom.Stop();
                    MessageBox.Show("Phòng đã bị xoá (chủ phòng rời).");
                    var lobby = new frmLobby();
                    lobby.Show();
                    this.FormClosing -= frmRoom_FormClosing;
                    this.Close();
                    return;
                }

                bool guestChanged = room.IDKhach != _lastKnownGuestId;
                if (guestChanged || room.TrangThai != _room.TrangThai)
                {
                    _room = room;
                    _lastKnownGuestId = room.IDKhach;
                    await UpdateRoomUIAsync(room);
                }
            }
            catch
            {
                // ignore transient errors
            }
        }

        private async void btnThoatPhongCho_Click(object sender, EventArgs e)
        {
            timerCheckRoom.Stop();
            try
            {
                await _roomApi.LeaveRoomAsync(_room.Id, _currentUserId);
            }
            catch
            {
                // ignore
            }
            var lobby = new frmLobby();
            lobby.Show();
            this.FormClosing -= frmRoom_FormClosing;
            this.Close();
        }

        private async void frmRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isClosing) return;
            e.Cancel = true;
            _isClosing = true;
            timerCheckRoom.Stop();
            try
            {
                await _roomApi.LeaveRoomAsync(_room.Id, _currentUserId);
            }
            catch { }
            var lobby = new frmLobby();
            lobby.Show();
            this.FormClosing -= frmRoom_FormClosing;
            this.Close();
        }

        private void btnTinNhan_Click(object sender, EventArgs e)
        {
            ucChatBox1.Visible = !ucChatBox1.Visible;
            if (ucChatBox1.Visible) ucChatBox1.BringToFront();
        }

        private void btnNVChuPhong_Click(object sender, EventArgs e)
        {
            frmSelectcharacter selectForm = new frmSelectcharacter();
            if (selectForm.ShowDialog() == DialogResult.OK)
            {
                string tenNV = selectForm.TenNhanVatDaChon;
                lblNhanVatChuPhong.Text = "Nhân vật: " + tenNV;
            }
        }

        private void btnNVKhach_Click(object sender, EventArgs e)
        {
            frmSelectcharacter selectForm = new frmSelectcharacter();
            if (selectForm.ShowDialog() == DialogResult.OK)
            {
                string tenNV = selectForm.TenNhanVatDaChon;
                lblNhanVatKhach.Text = "Nhân vật: " + tenNV;
            }
        }
    }
}
