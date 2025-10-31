using System;
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
    public partial class frmMatchHistory : Form
    {
        public frmMatchHistory()
        {
            InitializeComponent();
        }

        private void frmMatchHistory_Load(object sender, EventArgs e)
        {
            // Lấy kích thước màn hình chính
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;

            // Áp dụng kích thước đó cho Form
            this.Size = screen.Size;
            this.Location = new Point(0, 0);

            // Dữ liệu mẫu để hiển thị trong dgvBangXepHang
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Elizabeth Swan", "2", "PlayerB", "Captain Kid", "Thắng", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Hector Barbossa", "2", "PlayerB", "Will Turner", "Thua", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Elizabeth Swan", "2", "PlayerB", "Captain Kid", "Thắng", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Hector Barbossa", "2", "PlayerB", "Will Turner", "Thua", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Elizabeth Swan", "2", "PlayerB", "Captain Kid", "Thắng", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Hector Barbossa", "2", "PlayerB", "Will Turner", "Thua", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Elizabeth Swan", "2", "PlayerB", "Captain Kid", "Thắng", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Hector Barbossa", "2", "PlayerB", "Will Turner", "Thua", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Elizabeth Swan", "2", "PlayerB", "Captain Kid", "Thắng", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Hector Barbossa", "2", "PlayerB", "Will Turner", "Thua", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Elizabeth Swan", "2", "PlayerB", "Captain Kid", "Thắng", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Hector Barbossa", "2", "PlayerB", "Will Turner", "Thua", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Elizabeth Swan", "2", "PlayerB", "Captain Kid", "Thắng", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Hector Barbossa", "2", "PlayerB", "Will Turner", "Thua", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Elizabeth Swan", "2", "PlayerB", "Captain Kid", "Thắng", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Hector Barbossa", "2", "PlayerB", "Will Turner", "Thua", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Elizabeth Swan", "2", "PlayerB", "Captain Kid", "Thắng", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Hector Barbossa", "2", "PlayerB", "Will Turner", "Thua", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Elizabeth Swan", "2", "PlayerB", "Captain Kid", "Thắng", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Hector Barbossa", "2", "PlayerB", "Will Turner", "Thua", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Elizabeth Swan", "2", "PlayerB", "Captain Kid", "Thắng", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Hector Barbossa", "2", "PlayerB", "Will Turner", "Thua", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Elizabeth Swan", "2", "PlayerB", "Captain Kid", "Thắng", "00:00:00 01/01/2222", "00:00:00 01/01/3333");
            dgvLichSuDau.Rows.Add("1", "PlayerA", "Hector Barbossa", "2", "PlayerB", "Will Turner", "Thua", "00:00:00 01/01/2222", "00:00:00 01/01/3333");

            // Đồng bộ scrollbar
            guna2VScrollBar1.Scroll += (s, E) =>
            {
                int maxIndex = dgvLichSuDau.RowCount - 1;
                int scrollValue = Math.Min(guna2VScrollBar1.Value, maxIndex);
                dgvLichSuDau.FirstDisplayedScrollingRowIndex = scrollValue;
            };

            // Tính số dòng hiển thị
            int visibleRows = dgvLichSuDau.DisplayedRowCount(true);
            int totalRows = dgvLichSuDau.RowCount;

            // Chỉ hiện scrollbar nếu cần
            guna2VScrollBar1.Visible = totalRows >= visibleRows;
            guna2VScrollBar1.Maximum = totalRows;
        }

        private void dgvLichSuDau_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Chỉ xử lý hàng tiêu đề cột
            if (e.RowIndex == -1)
            {
                // Tắt style mặc định (nếu chưa tắt trong Form_Load)
                dgvLichSuDau.EnableHeadersVisualStyles = false;

                // Lấy màu nền và màu chữ của header
                Color backColor = dgvLichSuDau.ColumnHeadersDefaultCellStyle.BackColor;
                Color foreColor = dgvLichSuDau.ColumnHeadersDefaultCellStyle.ForeColor;

                // Lấy màu viền của bảng
                Color borderColor = dgvLichSuDau.GridColor;

                // Tô nền header
                using (SolidBrush backBrush = new SolidBrush(backColor))
                {
                    e.Graphics.FillRectangle(backBrush, e.CellBounds);
                }

                // Vẽ nội dung (text)
                TextRenderer.DrawText(e.Graphics,
                                      Convert.ToString(e.FormattedValue),
                                      e.CellStyle.Font,
                                      e.CellBounds,
                                      foreColor,
                                      TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);

                // Vẽ viền khung (từng ô header)
                using (Pen p = new Pen(borderColor, 1))
                {
                    // Viền trái (chỉ cột đầu)
                    if (e.ColumnIndex == 0)
                        e.Graphics.DrawLine(p, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom - 1);

                    // Viền phải (cho tất cả cột)
                    e.Graphics.DrawLine(p, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

                    // Viền dưới
                    e.Graphics.DrawLine(p, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);

                    // Viền trên (vẽ cho tất cả header để khung liền)
                    e.Graphics.DrawLine(p, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Top);
                }

                e.Handled = true;
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
