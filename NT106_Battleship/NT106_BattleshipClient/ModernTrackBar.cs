using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel; // Cần thiết cho Designer

namespace NT106_BattleshipClient // <<< ĐÃ KHẮC PHỤC LỖI THIẾU NAMESPACE
{
    // Giúp Designer hiển thị control một cách chính xác
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design")]
    public class ModernTrackBar : TrackBar
    {
        // Biến để theo dõi trạng thái kéo chuột
        private bool isDragging = false;

        // Ghi đè CreateParams để ẩn thanh trượt gốc của Windows
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // 0x01 là cờ (flag) để loại bỏ THUMB mặc định.
                cp.Style |= 0x01;
                return cp;
            }
        }

        // --- THUỘC TÍNH MỚI ---
        [Category("Appearance")]
        public Color MauChinh { get; set; } = Color.FromArgb(51, 153, 255); // Màu xanh dương hiện đại

        [Category("Appearance")]
        public Color MauNen { get; set; } = Color.LightGray; // Màu nền track

        public ModernTrackBar()
        {
            // SetStyle quan trọng nhất:
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor,
                     true);

            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent;
            this.TickStyle = TickStyle.None;
        }

        // <<< KHẮC PHỤC TRIỆT ĐỂ BẰNG CÁCH GHI ĐÈ SỰ KIỆN CHUỘT >>>

        // 1. Khi nhấn chuột xuống, bắt đầu trạng thái kéo
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            if (mevent.Button == MouseButtons.Left)
            {
                isDragging = true;
                // Vẫn gọi Invalidate() để đảm bảo vẽ lại ngay lập tức
                this.Invalidate();
            }
        }

        // 2. Khi di chuyển chuột (chỉ cần Invalidate() để vẽ lại, OnValueChanged sẽ xử lý logic)
        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);
            if (isDragging)
            {
                // Gọi Invalidate() để buộc vẽ lại khi đang kéo
                this.Invalidate();
            }
        }

        // 3. Khi nhả chuột, kết thúc trạng thái kéo
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            isDragging = false;
            // Buộc vẽ lại lần cuối, đảm bảo trạng thái cuối cùng chính xác
            this.Invalidate();
        }

        // Vẫn giữ OnValueChanged để xử lý thay đổi giá trị và Invalidate
        protected override void OnValueChanged(EventArgs e)
        {
            base.OnValueChanged(e);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int trackHeight = 8;
            int thumbSize = 14;
            int yCenter = Height / 2;

            float percent = (float)(Value - Minimum) / (Maximum - Minimum);

            // Chiều rộng không gian vẽ thanh trượt (từ mép trái đến mép phải của thanh nền)
            int trackWidth = Width - thumbSize;

            // Vị trí TÂM của nút kéo (tính từ mép trái của control)
            int thumbCenter = thumbSize / 2 + (int)(trackWidth * percent);

            // Vị trí góc trên bên trái của hình chữ nhật bao quanh nút kéo
            int thumbX = thumbCenter - thumbSize / 2;

            // 1. Thanh nền (bo tròn 4 góc)
            // Thanh nền bắt đầu từ thumbSize/2 (để tạo khoảng trống cho nút kéo ở Value=0)
            RectangleF trackRect = new RectangleF(thumbSize / 2f, yCenter - trackHeight / 2f, Width - thumbSize, trackHeight);
            using (GraphicsPath path = BoTron(trackRect, trackHeight / 2f))
            using (SolidBrush brush = new SolidBrush(MauNen))
                g.FillPath(brush, path);

            // 2. Thanh tiến trình (ĐÃ SỬA LỖI LÕI ĐỨT ĐOẠN)
            if (percent > 0)
            {
                // Độ rộng của thanh tiến trình: từ mép trái của thanh nền đến TÂM của nút kéo.
                float progressWidth = thumbCenter - (thumbSize / 2f);

                // Vùng tiến trình: Bắt đầu từ X ban đầu (thumbSize/2), chiều rộng là progressWidth.
                RectangleF progressRect = new RectangleF(thumbSize / 2f, yCenter - trackHeight / 2f, progressWidth, trackHeight);

                using (GraphicsPath path = (percent >= 1f) ? BoTron(progressRect, trackHeight / 2f) : BoTronLeft(progressRect, trackHeight / 2f))
                using (SolidBrush brush = new SolidBrush(MauChinh))
                {
                    g.FillPath(brush, path);
                }
            }

            // 3. Nút kéo tròn (Thumb)
            Rectangle thumbRect = new Rectangle(thumbX, yCenter - thumbSize / 2, thumbSize, thumbSize);
            using (SolidBrush brush = new SolidBrush(MauChinh))
                g.FillEllipse(brush, thumbRect);

            // 4. Viền cho nút kéo 
            using (Pen outline = new Pen(Color.FromArgb(100, Color.Black), 1))
                g.DrawEllipse(outline, thumbRect);
        }

        // --- HÀM HỖ TRỢ VẼ BO TRÒN (Bo tròn 4 góc - Dùng cho thanh nền) ---
        private static GraphicsPath BoTron(RectangleF rect, float radius)
        {
            float d = radius * 2;
            GraphicsPath path = new GraphicsPath();

            if (rect.Width <= 0 || rect.Height <= 0) return path;

            // Giới hạn d không vượt quá kích thước cạnh nhỏ nhất
            float maxD = Math.Min(rect.Width, rect.Height);
            if (d > maxD) d = maxD;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);

            path.CloseFigure();
            return path;
        }

        // --- HÀM HỖ TRỢ VẼ BO TRÒN BÊN TRÁI (Dùng cho thanh tiến trình < 100%) ---
        private static GraphicsPath BoTronLeft(RectangleF rect, float radius)
        {
            float d = radius * 2;
            GraphicsPath path = new GraphicsPath();

            if (rect.Width <= 0 || rect.Height <= 0) return path;

            // Giới hạn d không vượt quá chiều cao
            if (d > rect.Height) d = rect.Height;

            // Góc trên bên trái (bo tròn)
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);

            // Đường thẳng trên (ĐÃ SỬA LỖI TÍNH TOÁN: bắt đầu sau cung tròn)
            path.AddLine(rect.X + d, rect.Y, rect.Right, rect.Y);

            // Đường thẳng bên phải (vuông góc)
            path.AddLine(rect.Right, rect.Y, rect.Right, rect.Bottom);

            // Đường thẳng dưới (ĐÃ SỬA LỖI TÍNH TOÁN: kết thúc trước cung tròn)
            path.AddLine(rect.Right, rect.Bottom, rect.X + d, rect.Bottom);

            // Góc dưới bên trái (bo tròn)
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);

            path.CloseFigure();
            return path;
        }
    }
}
