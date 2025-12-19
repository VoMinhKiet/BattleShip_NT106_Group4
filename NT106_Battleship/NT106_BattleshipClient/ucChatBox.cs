using Microsoft.AspNetCore.SignalR.Client;
using NT106_BattleshipClient.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NT106_BattleshipClient
{
    public partial class ucChatBox : UserControl
    {
        private HubConnection _hub;
        private int _roomId;
        private bool _connected;
        public ucChatBox()
        {
            InitializeComponent();
        }

        public ucChatBox(int roomId) : this()
        {
            _roomId = roomId;
            btnGui.Enabled = false;
        }
        public async Task ConnectAsync()
        {
            if (_connected) return;

            _hub = new HubConnectionBuilder()
                .WithUrl("http://localhost:5074/chatHub")
                .WithAutomaticReconnect()
                .Build();

            _hub.On<TinNhanDto>("NhanTinNhan", tin =>
            {
                BeginInvoke(new Action(() =>
                {
                    AppendMessage($"[{tin.TenNguoiDung}] : {tin.NoiDung}");
                }));
            });

            await _hub.StartAsync();
            await _hub.InvokeAsync("JoinPhong", _roomId);

            _connected = true;
            btnGui.Enabled = true;
        }

        private async void btnGui_Click(object sender, EventArgs e)
        {
            if (!_connected) return;
            if (string.IsNullOrWhiteSpace(txtTinNhan.Text)) return;

            var dto = new TinNhanDto
            {
                IdPhongCho = _roomId,
                IdNguoiDung = GlobalData.UserId,
                TenNguoiDung = GlobalData.Username,
                NoiDung = txtTinNhan.Text.Trim()
            };

            await _hub.InvokeAsync("GuiTinNhan", dto);

            txtTinNhan.Clear();

        }
        private void AppendMessage(string msg)
        {
            if (rtbLichSuTinNhan.TextLength > 0)
                rtbLichSuTinNhan.AppendText(Environment.NewLine);

            rtbLichSuTinNhan.AppendText(msg);
            rtbLichSuTinNhan.ScrollToCaret();
        }

    }
}
