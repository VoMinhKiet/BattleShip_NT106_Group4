using Microsoft.AspNetCore.SignalR.Client;
using NT106_BattleshipClient.Models;
using NT106_BattleshipClient.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmFriendlist : BaseForm
    {
        private FriendApiService _friendApi;
        private List<FriendDto> _cache = new List<FriendDto>();

        private readonly string _baseUrl = "http://localhost:5074/";
        private bool _isLoaded = false;

        private int? _inviteRoomId;

        public frmFriendlist(int? inviteRoomId = null)
        {
            InitializeComponent();
            _inviteRoomId = inviteRoomId;

            EnableFormDoubleBuffering();
            SetUseComposited(true);
        }
        private void lvFriendlist_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            bool has = lvFriendlist.SelectedItems.Count > 0;
            btnAddfriend.Enabled = has;
            btnDeletefriend.Enabled = has;
        }

        public frmFriendlist()
        {
            InitializeComponent();

            // chống nháy form
            EnableFormDoubleBuffering();
            SetUseComposited(true);
        }

        private async void frmFriendlist_Load(object sender, EventArgs e)
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add("All");
            cbStatus.Items.Add("Online");
            cbStatus.Items.Add("Offline");
            cbStatus.Items.Add("Requests");

            cbStatus.SelectedIndex = 0;

            SetupListViewColumns();

            var http = new HttpClient { BaseAddress = new Uri(_baseUrl) };
            _friendApi = new FriendApiService(http);

            _isLoaded = true;

            await ReloadByModeAsync();
        }

        private void SetupListViewColumns()
        {
            lvFriendlist.View = View.Details;
            lvFriendlist.FullRowSelect = true;
            lvFriendlist.GridLines = true;
            lvFriendlist.HideSelection = false;
            lvFriendlist.MultiSelect = false;

            lvFriendlist.Columns.Clear();

            // đúng thứ tự và đúng độ rộng bạn yêu cầu
            lvFriendlist.Columns.Add("ID", 50);
            lvFriendlist.Columns.Add("Username", 170);
            lvFriendlist.Columns.Add("Stars", 137);
            lvFriendlist.Columns.Add("Status", 110);
            lvFriendlist.Columns.Add("Online", 60);
            lvFriendlist.Columns.Add("LastOnline", 100);
        }

        private void RenderList(List<FriendDto> list)
        {
            // nhớ userId đang chọn (nếu có)
            int? selectedId = null;
            if (lvFriendlist.SelectedItems.Count > 0)
            {
                var dto = lvFriendlist.SelectedItems[0].Tag as FriendDto;
                if (dto != null) selectedId = dto.UserId;
            }

            lvFriendlist.BeginUpdate();
            lvFriendlist.Items.Clear();

            foreach (var f in list)
            {
                var item = new ListViewItem(f.UserId.ToString());         // ID
                item.SubItems.Add(f.Username ?? "");                      // Username
                item.SubItems.Add(f.Stars.ToString());                    // Stars
                item.SubItems.Add(f.RelationStatus ?? "");                // Status
                item.SubItems.Add(f.Online ? "Online" : "Offline");

                item.ForeColor = f.Online
                    ? System.Drawing.Color.LimeGreen
                    : System.Drawing.Color.Gray;

                if (f.Online)
                    item.SubItems.Add("Now");
                else if (f.LastOnline.HasValue)
                    item.SubItems.Add(f.LastOnline.Value.ToString("HH:mm dd/MM"));
                else
                    item.SubItems.Add("-");

                item.Tag = f;
                lvFriendlist.Items.Add(item);

                // chọn lại item nếu trùng selectedId
                if (selectedId.HasValue && f.UserId == selectedId.Value)
                    item.Selected = true;
            }

            lvFriendlist.EndUpdate();

            // đảm bảo nhìn thấy selection + focus
            if (lvFriendlist.SelectedItems.Count > 0)
                lvFriendlist.SelectedItems[0].Focused = true;

            lvFriendlist.Focus();
        }


        private async Task ReloadByModeAsync()
        {
            try
            {
                var mode = cbStatus.SelectedItem?.ToString() ?? "All";

                if (mode == "Requests")
                    _cache = await _friendApi.GetRequestsAsync(GlobalData.UserId);
                else
                    _cache = await _friendApi.GetFriendsAsync(GlobalData.UserId);

                ApplyFiltersAndRender();
                UpdateButtonsByMode();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reload failed: " + ex.Message);
            }
        }

        private void ApplyFiltersAndRender()
        {
            IEnumerable<FriendDto> q = _cache;

            // filter Online/Offline theo cbStatus (trừ Requests)
            var mode = cbStatus.SelectedItem?.ToString() ?? "All";
            if (mode == "Online") q = q.Where(x => x.Online);
            if (mode == "Offline") q = q.Where(x => !x.Online);

            // filter theo Username
            var username = (txtUsername.Text ?? "").Trim();
            if (!string.IsNullOrEmpty(username))
                q = q.Where(x => (x.Username ?? "")
                    .IndexOf(username, StringComparison.OrdinalIgnoreCase) >= 0);

            // filter theo ID
            var idText = (txtID.Text ?? "").Trim();
            if (int.TryParse(idText, out int id))
                q = q.Where(x => x.UserId == id);

            RenderList(q.ToList());
        }

        private void UpdateButtonsByMode()
        {
            var mode = cbStatus.SelectedItem?.ToString() ?? "All";
            if (mode == "Requests")
            {
                btnAddfriend.Text = "Accept";
                btnDeletefriend.Text = "Reject";
            }
            else
            {
                btnAddfriend.Text = "Add friend";
                btnDeletefriend.Text = "Delete friend";
            }
        }

        private FriendDto GetSelectedFriend()
        {
            if (lvFriendlist.SelectedItems.Count == 0) return null;
            return lvFriendlist.SelectedItems[0].Tag as FriendDto;
        }

        private async void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_isLoaded) return;
            await ReloadByModeAsync();
        }


        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await ReloadByModeAsync();
        }

        private async void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                var myId = GlobalData.UserId;

                var idText = (txtID.Text ?? "").Trim();
                int? id = null;
                if (int.TryParse(idText, out int parsed) && parsed > 0)
                    id = parsed;

                var username = (txtUsername.Text ?? "").Trim();

                // nếu không nhập gì -> quay về list friends/requests theo cbStatus
                if (!id.HasValue && string.IsNullOrWhiteSpace(username))
                {
                    ApplyFiltersAndRender();
                    return;
                }

                // Search toàn hệ thống
                var list = await _friendApi.SearchUsersAsync(myId, id, username);

                // Render ngay kết quả search (không dùng _cache)
                RenderList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search failed: " + ex.Message);
            }
        }

        private async void btnAddfriend_Click(object sender, EventArgs e)
        {
            try
            {
                var mode = cbStatus.SelectedItem?.ToString() ?? "All";
                int myId = GlobalData.UserId;

                // ===== Requests: Accept =====
                if (mode == "Requests")
                {
                    var f = GetSelectedFriend();
                    if (f == null)
                    {
                        MessageBox.Show("Chọn 1 lời mời để Accept.");
                        return;
                    }

                    var ok = await _friendApi.AcceptAsync(myId, f.UserId);
                    MessageBox.Show(ok ? "Đã chấp nhận lời mời." : "Accept thất bại.");
                    await ReloadByModeAsync();
                    return;
                }

                // ===== Normal: Add friend (gửi lời mời) =====
                if (!int.TryParse((txtID.Text ?? "").Trim(), out int targetId))
                {
                    MessageBox.Show("Nhập ID hợp lệ ở ô ID.");
                    return;
                }

                if (targetId == myId)
                {
                    MessageBox.Show("Không thể kết bạn với chính mình.");
                    return;
                }

                var okAdd = await _friendApi.AddFriendAsync(myId, targetId);
                MessageBox.Show(okAdd ? "Đã gửi lời mời kết bạn." : "Gửi thất bại / đã tồn tại.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add/Accept error: " + ex.Message);
            }
        }

        private async void btnDeletefriend_Click(object sender, EventArgs e)
        {
            try
            {
                var mode = cbStatus.SelectedItem?.ToString() ?? "All";
                int myId = GlobalData.UserId;

                var f = GetSelectedFriend();
                if (f == null)
                {
                    MessageBox.Show("Chọn 1 dòng trước.");
                    return;
                }

                // ===== Requests: Reject =====
                if (mode == "Requests")
                {
                    var ok = await _friendApi.RejectAsync(myId, f.UserId);
                    MessageBox.Show(ok ? "Đã từ chối lời mời." : "Reject thất bại.");
                    await ReloadByModeAsync();
                    return;
                }

                // ===== Normal: Delete friend =====
                var confirm = MessageBox.Show($"Xóa bạn: {f.Username} ?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm != DialogResult.Yes) return;

                var okDel = await _friendApi.DeleteFriendAsync(myId, f.UserId);
                MessageBox.Show(okDel ? "Đã xóa bạn." : "Xóa thất bại.");
                await ReloadByModeAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete/Reject error: " + ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            var f = GetSelectedFriend();
            if (f == null)
            {
                MessageBox.Show("Chọn 1 người trong danh sách trước.");
                return;
            }

            // mở hồ sơ user được chọn
            frmUserInfo info = new frmUserInfo(f.UserId);
            info.ShowDialog();
        }

        private async void btnInvite_Click(object sender, EventArgs e)
        {
            if (_inviteRoomId == null)
            {
                MessageBox.Show("Không có phòng để mời.");
                return;
            }

            var f = GetSelectedFriend();
            if (f == null)
            {
                MessageBox.Show("Chọn 1 người trong danh sách.");
                return;
            }

            try
            {
                await InviteSignalRClient.Connection.InvokeAsync(
                    "SendRoomInvite",
                    f.UserId,
                    _inviteRoomId.Value,
                    GlobalData.UserId,
                    GlobalData.Username
                );

                MessageBox.Show("Đã gửi lời mời!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invite failed: " + ex.Message);
            }
        }


    }
}
