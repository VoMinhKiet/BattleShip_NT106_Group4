﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmLobby : Form
    {
        public frmLobby()
        {
            InitializeComponent();
        }

        private void frmLobby_Load(object sender, EventArgs e)
        {
            // Lấy kích thước màn hình chính
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;

            // Áp dụng kích thước đó cho Form
            this.Size = screen.Size;
            this.Location = new Point(0, 0); // Đặt Form ở góc trên bên trái

            label1.Focus();
            // Đặt trạng thái mặc định khi Form tải
            pnlTimTaoPhong.Visible = true; // (1) Hiện Panel Tìm/Tạo phòng
            pnlPhongCho.Visible = false;  // (2) Ẩn Panel Phòng Chờ

            //
            dgvTimPhong.Rows.Add("100", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("101", "Phong A", "Trống");
            dgvTimPhong.Rows.Add("102", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("103", "Phong A", "Trống");
            dgvTimPhong.Rows.Add("104", "Phong A", "Trống");
            dgvTimPhong.Rows.Add("105", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("106", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("107", "Phong A", "Trống");
            dgvTimPhong.Rows.Add("108", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("109", "Phong A", "Trống");
            dgvTimPhong.Rows.Add("110", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("111", "Phong A", "Trống");
            dgvTimPhong.Rows.Add("112", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("113", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("114", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("115", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("116", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("117", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("118", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("119", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("120", "Phong A", "Đầy");
            dgvTimPhong.Rows.Add("121", "Phong A", "Đầy");
            // Đồng bộ scrollbar
            guna2VScrollBar1.Scroll += (s, E) =>
            {
                int maxIndex = dgvTimPhong.RowCount - 1;
                int scrollValue = Math.Min(guna2VScrollBar1.Value, maxIndex);
                dgvTimPhong.FirstDisplayedScrollingRowIndex = scrollValue;
            };

            // Tính số dòng hiển thị
            int visibleRows = dgvTimPhong.DisplayedRowCount(true);
            int totalRows = dgvTimPhong.RowCount;

            // Chỉ hiện scrollbar nếu cần
            guna2VScrollBar1.Visible = totalRows > visibleRows;
            guna2VScrollBar1.Maximum = totalRows;

            // Bỏ chọn dòng đầu tiên khi load
            dgvTimPhong.ClearSelection();
            dgvTimPhong.CurrentCell = null;
        }
    
        private void dgvTimPhong_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Đặt padding mong muốn là 25 pixel mỗi bên
            const int HORIZONTAL_PADDING = 30;
            const int VERTICAL_PADDING = 3; // Thêm padding dọc để nút không chạm viền trên/dưới

            // 1. Kiểm tra điều kiện: Đúng cột và Đúng hàng dữ liệu
            if (e.ColumnIndex == dgvTimPhong.Columns["colThamGia"].Index && e.RowIndex >= 0)
            {
                // 2. Lấy Trạng Thái của hàng hiện tại
                string trangThai = dgvTimPhong.Rows[e.RowIndex].Cells["colTrangThai"].Value?.ToString();

                // 3. Xử lý logic ẩn/hiện nút và vẽ
                if (trangThai == "Trống")
                {
                    // Vẽ nền và viền ô mặc định trước
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

                    // 4. Định nghĩa màu sắc và vẽ button tùy chỉnh
                    Color buttonBackColor = Color.LightSkyBlue; // Nền nút
                    Color buttonForeColor = Color.SteelBlue;    // Chữ nút

                    // Tính toán vị trí và kích thước nút có padding 25px mỗi bên
                    Rectangle buttonRect = new Rectangle(
                        e.CellBounds.X + HORIZONTAL_PADDING,
                        e.CellBounds.Y + VERTICAL_PADDING,
                        e.CellBounds.Width - 2 * HORIZONTAL_PADDING, // Giảm 50px (25 trái + 25 phải)
                        e.CellBounds.Height - 2 * VERTICAL_PADDING
                    );

                    // 5. Vẽ hình chữ nhật (Nút)
                    using (SolidBrush backBrush = new SolidBrush(buttonBackColor))
                    {
                        e.Graphics.FillRectangle(backBrush, buttonRect);
                        // Vẽ viền nút màu đậm hơn (tùy chọn)
                        e.Graphics.DrawRectangle(Pens.DarkBlue, buttonRect);
                    }

                    // 6. Vẽ văn bản "Vào" ở giữa
                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        // Dùng Font đậm 12pt như bạn đã thiết lập trước đó
                        using (Font boldFont = new Font("Segoe UI", 12, FontStyle.Bold))
                        {
                            e.Graphics.DrawString("Vào", boldFont, new SolidBrush(buttonForeColor), e.CellBounds, sf);
                        }
                    }

                    e.Handled = true; // Báo hiệu đã tự vẽ xong
                }
                else // Trạng thái là "Đầy" hoặc không phải "Trống"
                {
                    // Chỉ vẽ nền và chữ mặc định (để ô trông trống rỗng)
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                    e.Handled = true;
                }
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTaoPhong_Click(object sender, EventArgs e)
        {
            // Ẩn Panel Tìm/Tạo phòng (1)
            pnlTimTaoPhong.Visible = false;

            // Hiện Panel Phòng Chờ (2)
            pnlPhongCho.Visible = true;

            // Đảm bảo (2) được đưa lên trên (nếu có vấn đề về z-order)
            pnlPhongCho.BringToFront();

            // (Tùy chọn: Gọi hàm logic để thực sự tạo phòng ở đây)
            // TaoPhongMoi(); 
        }

        private void btnThoatPhongCho_Click(object sender, EventArgs e)
        {
            // Ẩn Panel Phòng Chờ (2)
            pnlPhongCho.Visible = false;

            // Hiện Panel Tìm/Tạo phòng (1)
            pnlTimTaoPhong.Visible = true;

            // Đảm bảo (1) được đưa lên trên
            pnlTimTaoPhong.BringToFront();

            // (Tùy chọn: Gọi hàm logic để thoát khỏi phòng hiện tại)
            // HuyKetNoiPhong();
        }
    }
}
