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

        private int? _idPhongCho;
        private int? _idTranDau;

        private bool _connected = false;
        public ucChatBox()
        {
            InitializeComponent();
        }

        public ucChatBox(int idPhongCho) : this()
        {
            _idPhongCho = idPhongCho;
            btnGui.Enabled = false;
        }


        public ucChatBox(int idPhongCho, int idTranDau) : this()
        {
            _idPhongCho = idPhongCho;
            _idTranDau = idTranDau;
        }
        public async Task ConnectAsync()
        {
            if (_connected) return;

            btnGui.Enabled = false;

            _hub = new HubConnectionBuilder()
                .WithUrl("http://localhost:5074/chatHub")
                .WithAutomaticReconnect()
                .Build();

            _hub.On<TinNhanDto>("NhanTinNhan", tin =>
            {
                if (IsDisposed) return;

                BeginInvoke(new Action(() =>
                {
                    AppendMessage($"[{tin.TenNguoiDung}] : {tin.NoiDung}");
                }));
            });

            await _hub.StartAsync();

            // Join đúng group
            if (_idTranDau != null)
            {
                await _hub.InvokeAsync("JoinTranDau", _idTranDau.Value);
            }
            else if (_idPhongCho != null)
            {
                await _hub.InvokeAsync("JoinPhong", _idPhongCho.Value);
            }

            _connected = true;
            btnGui.Enabled = true;
        }

        private async void btnGui_Click(object sender, EventArgs e)
        {
            if (!_connected || string.IsNullOrWhiteSpace(txtTinNhan.Text))
                return;

            var dto = new TinNhanDto
            {
                IdPhongCho = _idPhongCho,
                IdTranDau = _idTranDau,
                IdNguoiDung = GlobalData.UserId,
                TenNguoiDung = GlobalData.Username,
                NoiDung = txtTinNhan.Text.Trim()
            };

            try
            {
                await _hub.InvokeAsync("GuiTinNhan", dto);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không gửi được tin nhắn. Vui lòng thử lại!",
                    "Chat Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            txtTinNhan.Clear();
            txtTinNhan.Focus();

        }
        private void AppendMessage(string msg)
        {
            if (rtbLichSuTinNhan.TextLength > 0)
                rtbLichSuTinNhan.AppendText(Environment.NewLine);

            rtbLichSuTinNhan.AppendText(msg);
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
            btnGui.Enabled = false;
        }
    }
}
