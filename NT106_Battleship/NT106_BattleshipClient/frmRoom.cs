using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
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
        public event Action RoomReadyToShow;

        private RoomDto _room;
        private TranDauDto _currentMatch;
        private int _currentUserId;
        private string _currentUsername;
        private string _myCharacterName = "";

        private readonly RoomApiService _roomApi = new RoomApiService();
        private bool _isLeaving = false;
        private int? _lastKnownGuestId = null;

        private bool _isHost;
        private bool _isGuestReady;

        // Biến này để đánh dấu khi nào Code đang tự cập nhật UI
        private bool _isUpdatingUI = false;

        private bool _isGoingToGame = false; // Cờ đánh dấu đang vào game

        private HubConnection _hub;
        public int mapsize;
        public int roomId;
        public frmRoom(RoomDto room, int userId, string username)
        {
            // Tối ưu vẽ form
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint |
                          ControlStyles.AllPaintingInWmPaint, true);

            _currentUserId = GlobalData.UserId;
            InitializeComponent();

            // chống nháy form
            EnableFormDoubleBuffering();
            SetUseComposited(true);


            _room = room ?? throw new ArgumentNullException(nameof(room));
            _currentUserId = userId;
            _currentUsername = username;
            roomId = _room.Id;
            _currentMatch = new TranDauDto();
            this.FormClosing += frmRoom_FormClosing;

            _isHost = (_currentUserId == room.IDChuPhong);
            _isGuestReady = false;

            this.Opacity = 0; // ẨN FORM ĐI
        }

        private async void frmRoom_Load(object sender, EventArgs e)
        {
            // Đảm bảo handle đã được tạo
            if (!this.IsHandleCreated)
                this.CreateControl();

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;


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

                SetupUIControls();

                if (!_isHost)
                {
                    // Gửi lệnh "REQUEST_INFO" (value để trống cũng được)
                    await SendUISyncCommand("REQUEST_INFO", "");
                }
            }
            catch { }
            await XepTauConnector();
            GameStartedHandler();
        }

        private void frmRoom_Shown(object sender, EventArgs e)
        {
            // Cho WinForms vẽ xong hết rồi mới hiện
            this.BeginInvoke(new Action(() =>
            {
                RoomReadyToShow?.Invoke(); // Báo hiệu cho bên gọi biết là Room đã sẵn sàng hiển thị
                this.Opacity = 1; //HIỆN FORM
            }));
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
                    MessageBox.Show("Chủ phòng đã rời, phòng đã đóng!");

                    // Đặt cờ này để không gọi API LeaveRoom
                    _isLeaving = true;

                    this.Close();
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
            cbKichThuoc.Enabled = _isHost;

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

        private async void ProcessIncomingData(string command, string value)
        {
            switch (command)
            {
                case "SET_HOST_CHAR": lblNhanVatChuPhong.Text = "Nhân vật: " + value; break;
                case "SET_GUEST_CHAR": lblNhanVatKhach.Text = "Nhân vật: " + value; break;
                case "SET_SIZE":
                    _isUpdatingUI = true;
                    if (cbKichThuoc.Items.Contains(value))
                    {
                        cbKichThuoc.SelectedItem = value;

                        if (value == "10x10") mapsize = 10;
                        else if (value == "9x9") mapsize = 9;
                        else if (value == "8x8") mapsize = 8;
                    }
                    _isUpdatingUI = false;
                    break;
                case "REQUEST_INFO":
                    // Chỉ có Host mới cần trả lời câu hỏi này
                    if (_isHost)
                    {
                        // 1. Gửi lại kích thước bàn cờ hiện tại
                        if (cbKichThuoc.SelectedItem != null)
                        {
                            await SendUISyncCommand("SET_SIZE", cbKichThuoc.SelectedItem.ToString());
                        }

                        // 2. Gửi lại nhân vật của Host (nếu đã chọn)
                        if (!string.IsNullOrEmpty(_myCharacterName))
                        {
                            await SendUISyncCommand("SET_HOST_CHAR", _myCharacterName);
                        }
                    }
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

        private void btnThoatPhongCho_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void frmRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isGoingToGame) return;
            if (_isLeaving) return;

            e.Cancel = true;

            if (_hub != null)
            {
                try { await _hub.StopAsync(); await _hub.DisposeAsync(); } catch { }
            }

            await LeaveRoomAsync();

            _isLeaving = true;

            this.Close();
        }

        // ============================
        // Chat + chọn nhân vật
        // ============================
        private ucChatBox _chatBox;
        private async void btnTinNhan_Click(object sender, EventArgs e)
        {
            if (_chatBox == null)
            {
                _chatBox = new ucChatBox(_room.Id);
                this.Controls.Add(_chatBox);
                await _chatBox.ConnectAsync();
            }

            _chatBox.BringToFront();
            _chatBox.Visible = !_chatBox.Visible;
        }

        private async void btnNVChuPhong_Click(object sender, EventArgs e)
        {
            if (!_isHost)
            {
                MessageBox.Show("Chỉ chủ phòng được chọn nhân vật.");
                return;
            }

            frmSelectcharacter f = new frmSelectcharacter();
            if (f.ShowDialog() == DialogResult.OK)
            {
                string ten = f.TenNhanVatDaChon;
                _currentMatch.TenNV1 = ten;
                _myCharacterName = ten;
                lblNhanVatChuPhong.Text = "Nhân vật: " + ten;
                await SendUISyncCommand("SET_HOST_CHAR", ten);
            }
        }

        private async void btnNVKhach_Click(object sender, EventArgs e)
        {
            if (_isHost)
            {
                MessageBox.Show("Chỉ khách được chọn nhân vật.");
                return;
            }

            frmSelectcharacter f = new frmSelectcharacter();
            if (f.ShowDialog() == DialogResult.OK)
            {
                string ten = f.TenNhanVatDaChon;
                _currentMatch.TenNV2 = ten;
                lblNhanVatKhach.Text = "Nhân vật: " + ten;
                await SendUISyncCommand("SET_GUEST_CHAR", ten);
            }
        }

        private async void cbKichThuoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUI) return;
            if (!_isHost)
            {
                MessageBox.Show("Chỉ chủ phòng được đổi kích thước.");
                return;
            }

            if (cbKichThuoc.SelectedItem != null)
            {
                string size = cbKichThuoc.SelectedItem.ToString();
                await SendUISyncCommand("SET_SIZE", size);
                if (size == "10x10")
                {
                    mapsize = 10;
                }
                if (size == "9x9")
                {
                    mapsize = 9;
                }
                if (size == "8x8")
                {
                    mapsize = 8;
                }
            }
        }

        private async void btnSanSang_Click(object sender, EventArgs e)
        {
            if (_isHost)
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

        private async void btnBatDau_Click(object sender, EventArgs e)
        {
            if (!_isHost)
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

            await _hub.InvokeAsync("StartGame", _room.Id);
        }
        private async Task XepTauConnector()
        {
            if (_hub != null)
                return;

            _hub = new HubConnectionBuilder()
                .WithUrl("http://localhost:5074/xepTauHub")
                .WithAutomaticReconnect()
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Debug);
                    //logging.AddDebug();
                })
                .Build();

            await _hub.StartAsync();
            await _hub.InvokeAsync("JoinRoom", _room.Id);
        }

        private void GameStartedHandler()
        {
            _hub.Remove("GameStarted");

            _hub.On("GameStarted", () =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    _isGoingToGame = true;

                    this.Hide();
                    frmShip_Sorting frmShip_Sorting = new frmShip_Sorting(_room, _currentMatch, mapsize, _hub);
                    frmShip_Sorting.ShowDialog();
                }));
            });
        }


        private void pnlNenTinNhan_Paint(object sender, PaintEventArgs e)
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetControlDoubleBuffered(pnlChuPhong);
            SetControlDoubleBuffered(pnlKhach);
            SetControlDoubleBuffered(pnlNenChuPhong);
            SetControlDoubleBuffered(pnlNenKhach);
            SetControlDoubleBuffered(pnlPhongCho);
            SetControlDoubleBuffered(pnlTieuDeChuPhong);
            SetControlDoubleBuffered(pnlTieuDeKhach);

        }
    }

}
