using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using NT106_BattleshipClient.Models;


namespace NT106_BattleshipClient
{
    public partial class ucChatBox : UserControl
    {
        private readonly int _roomId;
        private HubConnection _hub;
        private bool _connected = false;
        public ucChatBox()
        {
            InitializeComponent();
            btnGui.Enabled = false;
        }

        public ucChatBox(int roomId) : this()
        {
            _roomId = roomId;
        }
        public async Task ConnectAsync()
        {
            if (_connected) return;

            try
            {
                _hub = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5074/chatHub")
                    .WithAutomaticReconnect()
                    .Build();

                // Nhận tin nhắn
                _hub.On<TinNhanDto>("NhanTinNhan", tin =>
                {
                    if (IsDisposed) return;

                    BeginInvoke(new Action(() =>
                    {
                        AppendMessage($"[{tin.TenNguoiDung}] : {tin.NoiDung}");
                    }));
                });

                await _hub.StartAsync();

                // Join phòng chat
                await _hub.InvokeAsync("JoinPhong", _roomId);

                _connected = true;
                btnGui.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không kết nối được ChatHub:\n" + ex.Message,
                    "CHAT ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async void btnGui_Click(object sender, EventArgs e)
        {
            if (!_connected || _hub == null) return;
            if (string.IsNullOrWhiteSpace(txtTinNhan.Text)) return;

            var tinNhan = new TinNhanDto
            {
                IdPhongCho = _roomId,
                IdNguoiDung = GlobalData.UserId,
                TenNguoiDung = GlobalData.Username,
                NoiDung = txtTinNhan.Text.Trim()
            };

            await _hub.InvokeAsync("GuiTinNhan", tinNhan);

            txtTinNhan.Clear();
            txtTinNhan.Focus();

        }
        private void AppendMessage(string message)
        {
            if (rtbLichSuTinNhan.TextLength > 0)
                rtbLichSuTinNhan.AppendText(Environment.NewLine);

            rtbLichSuTinNhan.AppendText(message);
            rtbLichSuTinNhan.SelectionStart = rtbLichSuTinNhan.Text.Length;
            rtbLichSuTinNhan.ScrollToCaret();
        }

        public async Task DisconnectAsync()
        {
            if (_hub != null)
            {
                await _hub.StopAsync();
                await _hub.DisposeAsync();
                _hub = null;
            }

            _connected = false;
        }
    }
}
