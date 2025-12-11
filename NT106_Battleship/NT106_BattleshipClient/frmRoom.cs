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
        private RoomDto _room;
        private int _currentUserId;
        private string _currentUsername;

        private readonly RoomApiService _roomApi = new RoomApiService();
        private bool _isLeaving = false;
        private int? _lastKnownGuestId = null;

        private frmLobby _lobby;

        private bool _isHost;
        private bool _isGuestReady;

        private bool IsHost => _room.IDChuPhong == _currentUserId;

        public frmRoom(RoomDto room, int userId, string username, frmLobby lobby)
        {
            InitializeComponent();
            _room = room ?? throw new ArgumentNullException(nameof(room));
            _currentUserId = userId;
            _currentUsername = username;
            _lobby = lobby;

            GlobalData.IsInRoom = true;   // Đánh dấu đang ở trong phòng

            this.FormClosing += frmRoom_FormClosing;

            _isHost = (_currentUserId == room.IDChuPhong);
            _isGuestReady = false;
        }
        private void ReturnToLobby()
        {
            // rời phòng rồi thì cho cờ IsInRoom = false
            GlobalData.IsInRoom = false;

            // Ưu tiên quay về lobby nếu có
            if (_lobby != null && !_lobby.IsDisposed)
            {
                _lobby.Show();
            }
            else
            {
                // Nếu không có lobby (vd: vào phòng bằng invite từ MainMenu)
                // thì quay về MainMenu
                var main = Application.OpenForms["frmMainMenu"] as frmMainMenu;
                if (main != null)
                {
                    main.Show();
                }
                else
                {
                    // fallback cuối: tạo MainMenu mới
                    var newMain = new frmMainMenu(GlobalData.UserId, GlobalData.Username);
                    newMain.Show();
                }
            }

            this.FormClosing -= frmRoom_FormClosing;
            this.Close();
        }
        private async void frmRoom_Load(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated)
                this.CreateControl();

            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            this.Size = screen.Size;
            this.Location = new Point(0, 0);

            await UpdateRoomUIAsync(_room, firstLoad: true);

            try
            {
                SignalRClient.Init("http://localhost:5074/roomHub");
                await SignalRClient.StartAsync();

                SetupSignalREvents();

                try
                {
                    await SignalRClient.Connection.InvokeAsync("JoinRoom", _room.Id.ToString());
                }
                catch { }
            }
            catch
            {
                
            }

            SetupUIControls();
        }
        private void SetupSignalREvents()
        {
            // ================================
            // FIX 2: Mẫu an toàn cho mọi BeginInvoke
            // ================================
            void SafeInvoke(Action act)
            {
                if (!this.IsHandleCreated) return;
                if (this.IsDisposed) return;

                try
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        if (!this.IsDisposed) act();
                    }));
                }
                catch { }
            }

            // ROOM UPDATED
            SignalRClient.Connection.On<RoomDto>("RoomUpdated", (roomDto) =>
            {
                if (roomDto.Id != _room.Id) return;
                _room = roomDto;

                SafeInvoke(() =>
                {
                    _ = UpdateRoomUIAsync(roomDto);
                });
            });

            // ROOM DELETED
            SignalRClient.Connection.On<int>("RoomDeleted", (deletedRoomId) =>
            {
                if (deletedRoomId != _room.Id) return;

                SafeInvoke(() =>
                {
                    _isLeaving = true; // thêm dòng này
                    MessageBox.Show("Phòng đã bị xoá (chủ phòng rời).");
                    ReturnToLobby();
                });
            });


            // UI SYNC
            SignalRClient.Connection.On<string, string>("SynchronizeRoomUI", (command, value) =>
            {
                SafeInvoke(() =>
                {
                    ProcessIncomingData(command, value);
                });
            });

            // GUEST READY
            SignalRClient.Connection.On<bool>("GuestReadyStateChanged", (state) =>
            {
                SafeInvoke(() =>
                {
                    _isGuestReady = state;
                    pnlTieuDeKhach.Text = state ? "KHÁCH đã sẵn sàng!" : "KHÁCH";
                });
            });
        }
        private void SetupUIControls()
        {
            cbKichThuoc.Enabled = IsHost;

            if (cbKichThuoc.Items.Count == 0)
            {
                cbKichThuoc.Items.AddRange(new object[] { "8x8", "9x9", "10x10" });
                cbKichThuoc.SelectedIndex = 0;
            }
        }

        private async Task SendUISyncCommand(string command, string value)
        {
            try
            {
                if (SignalRClient.Connection.State == HubConnectionState.Connected)
                {
                    await SignalRClient.Connection.InvokeAsync("SendUISync",
                        _room.Id.ToString(), command, value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi gửi đồng bộ UI: {ex.Message}");
            }
        }

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
                try
                {
                    var host = await _roomApi.GetUserByIdAsync(room.IDChuPhong);
                    lblTenChuPhong.Text = $"Tên: {host?.TenDangNhap ?? "(Không lấy được)"}";
                }
                catch { lblTenChuPhong.Text = "Tên: (Không lấy được)"; }
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
                    try
                    {
                        var guest = await _roomApi.GetUserByIdAsync(room.IDKhach.Value);
                        lblTenKhach.Text = $"Tên: {guest?.TenDangNhap ?? "(Không lấy được)"}";
                    }
                    catch { lblTenKhach.Text = "Tên: (Không lấy được)"; }

                    _lastKnownGuestId = room.IDKhach;
                }
            }
        }
        private async Task LeaveRoomAsync()
        {
            if (_isLeaving) return;
            _isLeaving = true;

            try
            {
                if (SignalRClient.Connection.State == HubConnectionState.Connected)
                {
                    try
                    {
                        await SignalRClient.Connection.InvokeAsync("LeaveRoom", _room.Id.ToString());
                    }
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

            e.Cancel = true;
            await LeaveRoomAsync();
            ReturnToLobby();
        }


        // ============================
        // Chat + chọn nhân vật
        // ============================

        private void btnTinNhan_Click(object sender, EventArgs e)
        {
            ucChatBox1.Visible = !ucChatBox1.Visible;
            if (ucChatBox1.Visible) ucChatBox1.BringToFront();
        }

        private async void btnNVChuPhong_Click(object sender, EventArgs e)
        {
            if (!IsHost)
            {
                MessageBox.Show("Chỉ chủ phòng được chọn nhân vật.");
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
                MessageBox.Show("Chỉ khách được chọn nhân vật.");
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
                MessageBox.Show("Chỉ chủ phòng được đổi kích thước.");
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
            if (IsHost)
            {
                MessageBox.Show("Chỉ khách mới được bấm nút này.");
                return;
            }

            if (lblNhanVatKhach.Text == "Nhân vật:")
            {
                MessageBox.Show("Hãy chọn nhân vật trước khi sẵn sàng!");
                return;
            }

            _isGuestReady = !_isGuestReady;

            pnlTieuDeKhach.Text = _isGuestReady ?
                "KHÁCH đã sẵn sàng!" :
                "KHÁCH";

            await SignalRClient.Connection.InvokeAsync("SetGuestReady",
                _room.Id, _isGuestReady);
        }

        private void btnBatDau_Click(object sender, EventArgs e)
        {
            if (!IsHost)
            {
                MessageBox.Show("Chỉ chủ phòng mới được bắt đầu trận!");
                return;
            }

            if (!_isGuestReady)
            {
                MessageBox.Show("Khách chưa sẵn sàng!");
                return;
            }

            if (lblNhanVatChuPhong.Text == "Nhân vật:")
            {
                MessageBox.Show("Hãy chọn nhân vật trước khi bắt đầu!");
                return;
            }

            if (cbKichThuoc.SelectedItem == null)
            {
                MessageBox.Show("Hãy chọn kích thước trận đấu!");
                return;
            }

            SignalRClient.Connection.InvokeAsync("StartGame", _room.Id);

            MessageBox.Show("Xếp tàu");
        }

        private void btnMoiBan_Click(object sender, EventArgs e)
        {
            if (!_isHost)
            {
                MessageBox.Show("Chỉ chủ phòng được mời bạn vào phòng.");
                return;
            }

            using (var f = new frmFriendlist(_currentUserId, _room.Id))
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowInTaskbar = false;   

                f.ShowDialog(this);
            }
        }
    }
}
