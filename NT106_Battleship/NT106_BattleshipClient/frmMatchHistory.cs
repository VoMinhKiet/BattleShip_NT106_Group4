using NT106_BattleshipClient.Services;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT106_BattleshipClient
{
    public partial class frmMatchHistory : BaseForm
    {
        public frmMatchHistory()
        {
            InitializeComponent();

            //test chống nháy
            EnableFormDoubleBuffering();
            //test chống nháy cực mạnh
            SetUseComposited(true);

        }

        private TranDauApiService _tranDauService = new TranDauApiService();


        private async void frmMatchHistory_Load(object sender, EventArgs e)
        {

            this.FormBorderStyle = FormBorderStyle.Sizable; // ← QUAN TRỌNG
            this.ControlBox = true;
            this.MinimizeBox = true;
            this.MaximizeBox = true;

            this.WindowState = FormWindowState.Normal;

            // Lấy kích thước màn hình chính
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;

            // Áp dụng kích thước đó cho Form
            this.Size = screen.Size;
            this.Location = new Point(0, 0);


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
                    m.Id1,
                    m.NguoiChoi1,
                    m.NhanVat1,

                    m.Id2,
                    m.NguoiChoi2,
                    m.NhanVat2,

                    m.KetQua,
                    m.TimeStart.ToString("dd/MM/yyyy HH:mm"),
                    m.TimeEnd?.ToString("dd/MM/yyyy HH:mm") ?? ""
                );
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

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
