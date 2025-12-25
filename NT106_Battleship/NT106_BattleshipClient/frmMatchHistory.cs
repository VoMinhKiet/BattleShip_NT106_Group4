using NT106_BattleshipClient.Services;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmMatchHistory : BaseForm
    {
        private TranDauApiService _tranDauService = new TranDauApiService();

        public frmMatchHistory()
        {
            InitializeComponent();

            //test chống nháy
            EnableFormDoubleBuffering();
            //test chống nháy cực mạnh
            SetUseComposited(true);

        }

        private async void frmMatchHistory_Load(object sender, EventArgs e)
        {

            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            // Tắt thanh cuộn mặc định của DataGridView để dùng Guna Scrollbar
            dgvLichSuDau.ScrollBars = ScrollBars.None;

            // Đồng bộ scrollbar
            guna2VScrollBar1.Scroll += (s, E) =>
            {
                if (dgvLichSuDau.RowCount > 0)
                {
                    int maxIndex = dgvLichSuDau.RowCount - 1;
                    int scrollValue = Math.Min(guna2VScrollBar1.Value, maxIndex);
                    // Kiểm tra an toàn để tránh crash
                    if (scrollValue >= 0 && scrollValue < dgvLichSuDau.RowCount)
                        dgvLichSuDau.FirstDisplayedScrollingRowIndex = scrollValue;
                }
            };

            //// Tính số dòng hiển thị
            //int visibleRows = dgvLichSuDau.DisplayedRowCount(true);
            //int totalRows = dgvLichSuDau.RowCount;

            //// Chỉ hiện scrollbar nếu cần
            //guna2VScrollBar1.Visible = totalRows >= visibleRows;
            //guna2VScrollBar1.Maximum = totalRows;

            // Tạm thời ẩn scrollbar lúc mới vào
            guna2VScrollBar1.Visible = false;

            await LoadMatchHistoryAsync();
        }

        private async Task LoadMatchHistoryAsync()
        {
            int userId = GlobalData.UserId;
            var list = await _tranDauService.GetHistoryAsync(userId);

            dgvLichSuDau.Rows.Clear();

            foreach (var m in list)
            {
                dgvLichSuDau.Rows.Add(
                    m.Id1, m.NguoiChoi1, m.NhanVat1,
                    m.Id2, m.NguoiChoi2, m.NhanVat2,
                    m.KetQua,
                    m.TimeStart.ToString("dd/MM/yyyy HH:mm"),
                    m.TimeEnd?.ToString("dd/MM/yyyy HH:mm") ?? ""
                );
            }

            // Cập nhật scrollbar sau khi đã có dữ liệu
            UpdateScrollbarState();
        }

        // Hàm xử lý logic ẩn hiện thanh cuộn
        private void UpdateScrollbarState()
        {
            // 1. Tính tổng chiều cao thực tế của nội dung (Header + Tất cả các dòng)
            int totalContentHeight = dgvLichSuDau.ColumnHeadersHeight +
                                     dgvLichSuDau.Rows.GetRowsHeight(DataGridViewElementStates.Visible);

            // 2. Lấy chiều cao vùng hiển thị của bảng
            int visibleHeight = dgvLichSuDau.ClientSize.Height;

            // 3. So sánh: Nếu nội dung dài hơn vùng hiển thị -> Hiện Scrollbar
            if (totalContentHeight > visibleHeight)
            {
                guna2VScrollBar1.Visible = true;

                // Cập nhật lại Maximum cho Guna Scrollbar khớp với số dòng
                guna2VScrollBar1.Maximum = dgvLichSuDau.RowCount;

                // (Tùy chọn) Tính toán ThumbSize để thanh cuộn nhìn chuẩn hơn
                // guna2VScrollBar1.ThumbSize = ... 
            }
            else
            {
                guna2VScrollBar1.Visible = false;
            }
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

        //test chống nháy
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SetControlDoubleBuffered(panel1);
            SetControlDoubleBuffered(tableLayoutPanel1);
            SetControlDoubleBuffered(pnlDong);
            SetControlDoubleBuffered(panel2);
            SetControlDoubleBuffered(panel3);

            SetControlDoubleBuffered(dgvLichSuDau);
        }

    }
}
