using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net.Http;
using Newtonsoft.Json;

namespace NT106_BattleshipClient
{
    public partial class frmFriendlist : BaseForm
    {
        private int currentUserId;
        public frmFriendlist(int userId)
        {
            InitializeComponent();
            currentUserId = userId;

            // chống nháy form
            EnableFormDoubleBuffering();
            SetUseComposited(true);
        }
        private class FriendSearchResult
        {
            public int id { get; set; }
            public string tenDangNhap { get; set; }
            public string rank { get; set; }
            public string relation { get; set; }
            public string direction { get; set; }
            public bool online { get; set; }
            public DateTime? lastOnline { get; set; }
        }
        private async void frmFriendlist_Load(object sender, EventArgs e)
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add("All");
            cbStatus.Items.Add("Online");
            cbStatus.Items.Add("Offline");
            cbStatus.SelectedIndex = 0;

            // Ẩn thanh tiêu đề nếu cần
            this.FormBorderStyle = FormBorderStyle.None;

            // ===== CẤU HÌNH LẠI LISTVIEW =====
            lvFriendlist.View = View.Details;      // bắt buộc để hiện cột
            lvFriendlist.FullRowSelect = true;
            lvFriendlist.GridLines = true;
            lvFriendlist.HideSelection = false;
            lvFriendlist.MultiSelect = false;

            lvFriendlist.BackColor = Color.FromArgb(255, 255, 224);   // nền vàng nhạt
            lvFriendlist.ForeColor = Color.Black;                     // CHỮ ĐEN

            lvFriendlist.OwnerDraw = false;   // nếu lỡ bật vẽ tay thì tắt đi

            await LoadFriendList();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void btnFind_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string idText = txtID.Text.Trim();

            int? searchId = null;
            if (int.TryParse(idText, out int parsed))
                searchId = parsed;

            if (string.IsNullOrEmpty(username) && !searchId.HasValue)
            {
                MessageBox.Show("Vui lòng nhập Username hoặc ID trước khi tìm.");
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5074/");

                string url = $"api/Friend/find?currentUserId={currentUserId}";
                if (!string.IsNullOrEmpty(username))
                    url += "&username=" + Uri.EscapeDataString(username);
                if (searchId.HasValue)
                    url += $"&id={searchId.Value}";

                HttpResponseMessage resp;
                try
                {
                    resp = await client.GetAsync(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không kết nối được server: " + ex.Message);
                    return;
                }

                string json = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    MessageBox.Show("Lỗi tìm kiếm: " + json);
                    return;
                }

                var data = JsonConvert.DeserializeObject<FriendSearchResult>(json);

                // Đổ 1 dòng vào ListView
                lvFriendlist.Items.Clear();

                var item = new ListViewItem(data.id.ToString()); // ID
                item.SubItems.Add(data.tenDangNhap);                           // Username
                item.SubItems.Add(data.rank ?? "");                            // Rank
                item.SubItems.Add(BuildStatusText(data.relation, data.direction)); // Status
                item.SubItems.Add(data.online ? "Online" : "Offline");         // Online
                item.SubItems.Add(FormatLastOnline(data.lastOnline));          // LastOnline

                item.UseItemStyleForSubItems = true;   // dùng chung ForeColor = Black
                item.ForeColor = Color.Black;

                lvFriendlist.Items.Add(item);

                MessageBox.Show("Số dòng trong ListView: " + lvFriendlist.Items.Count);

            }
        }
        private string FormatLastOnline(DateTime? last)
        {
            if (last == null)
                return "";

            var diff = DateTime.Now - last.Value;
            if (diff.TotalMinutes < 1) return "Just now";
            if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} phút trước";
            if (diff.TotalHours < 24) return $"{(int)diff.TotalHours} giờ trước";
            return last.Value.ToString("dd/MM HH:mm");
        }
        private string BuildStatusText(string relation, string direction)
        {
            switch (relation)
            {
                case "ACCEPTED":
                    return "Friend";
                case "PENDING":
                    if (direction == "YOU_SENT_REQUEST") return "Waiting for them";
                    if (direction == "THEY_SENT_REQUEST") return "Request received";
                    return "Pending";
                case "BLOCK":
                    return "Blocked";
                case "NOT_FRIEND":
                default:
                    return "Not friend";
            }
        }

        private int? GetSelectedFriendId()
        {
            if (lvFriendlist.SelectedItems.Count == 0)
                return null;

            var item = lvFriendlist.SelectedItems[0];
            if (int.TryParse(item.SubItems[0].Text, out int id))
                return id;

            return null;
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            int? friendId = GetSelectedFriendId();

            if (!friendId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn 1 người trong danh sách!");
                return;
            }

            // Mở form xem hồ sơ (friendId)
            frmUserInfo f = new frmUserInfo(friendId.Value);
            f.ShowDialog();
        }

        //test chống nháy
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Vẫn chống nháy cho panel / form
            SetControlDoubleBuffered(pnlFriendlist);

            // KHÔNG đụng vào ListView
            // SetControlDoubleBuffered(lvFriendlist);

            // Nếu hàm này áp dụng cho tất cả child (trong đó có ListView)
            // thì cũng tạm thời bỏ để test:
            // SetDoubleBufferedForAllChildren(this);
        }


        private async void btnAddfriend_Click(object sender, EventArgs e)
        {
            // Ưu tiên: nếu user đã chọn 1 dòng trong ListView
            int? targetId = GetSelectedFriendId();

            if (!targetId.HasValue)
            {
                // Nếu chưa chọn dòng, fallback về textbox ID / Username
                string idText = txtID.Text.Trim();
                string username = txtUsername.Text.Trim();

                if (int.TryParse(idText, out int parsed))
                {
                    targetId = parsed;
                }
                else if (!string.IsNullOrEmpty(username))
                {
                    // Gọi Find để lấy ID từ username
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:5074/");
                        string url = $"api/Friend/find?currentUserId={currentUserId}&username={Uri.EscapeDataString(username)}";

                        var resp = await client.GetAsync(url);
                        string json = await resp.Content.ReadAsStringAsync();

                        if (!resp.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Không tìm thấy người dùng để kết bạn: " + json);
                            return;
                        }

                        var data = JsonConvert.DeserializeObject<FriendSearchResult>(json);
                        targetId = data.id;
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn 1 dòng hoặc nhập ID/Username để kết bạn.");
                    return;
                }
            }

            // Gửi AddFriend
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5074/");

                var body = new
                {
                    CurrentUserId = currentUserId,
                    TargetUserId = targetId.Value
                };

                string jsonBody = JsonConvert.SerializeObject(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var resp = await client.PostAsync("api/Friend/add", content);
                var json = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    MessageBox.Show("Lỗi kết bạn: " + json);
                    return;
                }

                MessageBox.Show("Kết quả: " + json);

                // refresh lại kết quả Find (nếu trước đó dùng Find)
                btnFind_Click(sender, e);
            }
        }

        private void btnInvite_Click(object sender, EventArgs e)
        {

        }

        private async void btnDeletefriend_Click(object sender, EventArgs e)
        {
            int? targetId = GetSelectedFriendId();
            if (!targetId.HasValue)
            {
                MessageBox.Show("Vui lòng chọn một dòng trong danh sách để xoá.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xoá quan hệ với ID {targetId.Value}?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5074/");

                var body = new
                {
                    CurrentUserId = currentUserId,
                    TargetUserId = targetId.Value
                };

                string jsonBody = JsonConvert.SerializeObject(body);
                var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var resp = await client.PostAsync("api/Friend/delete", content);
                var json = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    MessageBox.Show("Lỗi xoá bạn / huỷ lời mời: " + json);
                    return;
                }

                MessageBox.Show("Kết quả: " + json);

                // Xoá khỏi ListView
                lvFriendlist.Items.Remove(lvFriendlist.SelectedItems[0]);
            }
        }

        private List<FriendSearchResult> friendListRaw = new List<FriendSearchResult>();
        private async Task LoadFriendList()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5074/");

                HttpResponseMessage resp = await client.GetAsync(
                    $"api/Friend/list?currentUserId={currentUserId}"
                );

                string json = await resp.Content.ReadAsStringAsync();

                if (!resp.IsSuccessStatusCode)
                {
                    MessageBox.Show("Không thể tải danh sách bạn bè: " + json);
                    return;
                }

                friendListRaw = JsonConvert.DeserializeObject<List<FriendSearchResult>>(json);

                // HIỂN THỊ theo filter hiện tại
                ApplyFilter();
            }
        }

        private void ApplyFilter()
        {
            if (friendListRaw == null) return;

            string filter = cbStatus.SelectedItem.ToString();

            List<FriendSearchResult> filtered = friendListRaw;

            if (filter == "Online")
                filtered = friendListRaw.Where(f => f.online).ToList();

            else if (filter == "Offline")
                filtered = friendListRaw.Where(f => !f.online).ToList();

            DisplayFriendList(filtered);
        }
        private void DisplayFriendList(List<FriendSearchResult> list)
        {
            lvFriendlist.Items.Clear();

            foreach (var u in list)
            {
                var item = new ListViewItem(u.id.ToString());
                item.SubItems.Add(u.tenDangNhap);
                item.SubItems.Add(u.rank);
                item.SubItems.Add("Friend");
                item.SubItems.Add(u.online ? "Online" : "Offline");
                item.SubItems.Add(FormatLastOnline(u.lastOnline));

                item.ForeColor = Color.Black;

                lvFriendlist.Items.Add(item);
            }
        }


        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadFriendList();
            MessageBox.Show("Đã cập nhật danh sách bạn bè!");
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }
    }
}
