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
    public partial class frmSelectcharacter : BaseForm
    {
        public string TenNhanVatDaChon { get; private set; }

        public frmSelectcharacter()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExitdescribe_Click(object sender, EventArgs e)
        {
            pnlDescribe.Visible = false;
        }

        private void ShowDescription(string name, string description, string skillName, string skillDetail)
        {
            pnlDescribe.Visible = true;
            pnlDescribe.BringToFront();

            rchtxtDescribe.Text =
                $"⚔ {name}\n\n" +
                $"{description}\n\n" +
                $"✨ Kỹ năng đặc biệt: {skillName}\n" +
                $"{skillDetail}";

            pnlDescribe.Location = new Point(
                (this.ClientSize.Width - pnlDescribe.Width) / 2,
                (this.ClientSize.Height - pnlDescribe.Height) / 2
            );
        }

        private void btnDescribeES_Click(object sender, EventArgs e)
        {
            ShowDescription(
                "Elizabeth Swann",
                "Một chiến binh nhanh nhẹn và táo bạo.\n" +
                "Có khả năng né tránh cao và dùng song kiếm cực kỳ thành thạo để phản công.",
                "Không kích (Airstrike)",
                "Tấn công toàn bộ các ô trong **một hàng hoặc một cột** bạn chọn.\n" +
                "Rất hiệu quả khi đối thủ tập trung tàu theo hướng thẳng hàng."
            );
        }

        private void btnDescribeHB_Click(object sender, EventArgs e)
        {
            ShowDescription(
                "Hector Barbossa",
                "Một thuyền trưởng lão luyện, có khả năng ra đòn chí mạng.\n" +
                "Sức mạnh vật lý cao, nhưng tốc độ di chuyển hơi chậm.",
                "Phi tiễn (Projectiles)",
                "Bắn ra **6 đạn pháo ngẫu nhiên** trên bản đồ.\n" +
                "Thích hợp khi muốn dò vị trí tàu địch ở giai đoạn đầu trận."
            );
        }

        private void btnDescribeWT_Click(object sender, EventArgs e)
        {
            ShowDescription(
                "Will Turner",
                "Một kiếm sĩ dũng cảm với khả năng phòng thủ tốt.\n" +
                "Sở hữu kỹ năng phản công mạnh mẽ khi bị tấn công.",
                "Pháo kích (Bombardment)",
                "Bắn phá một vùng **3x3 ô** trên sa bàn.\n" +
                "Hiệu quả cao khi muốn tiêu diệt nhanh một khu vực nghi có nhiều tàu địch."
            );
        }

        private void btnDescribeJS_Click(object sender, EventArgs e)
        {
            ShowDescription(
                "Jack Sparrow",
                "Một tay súng cừ khôi, chuyên tấn công từ xa.\n" +
                "Sử dụng vũ khí tầm xa và bom nổ để kiểm soát chiến trường.",
                "Đạn chùm (Salvo)",
                "Chọn **5 ô bất kỳ** để bắn đồng thời.\n" +
                "Phù hợp khi đã xác định được vùng tàu địch ẩn nấp."
            );
        }

        private void btnSelectES_Click(object sender, EventArgs e)
        {
            // 1. Gán tên nhân vật
            this.TenNhanVatDaChon = "Elizabeth Swann";

            // 2. Thiết lập kết quả và đóng Form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSelectHB_Click(object sender, EventArgs e)
        {
            // 1. Gán tên nhân vật
            this.TenNhanVatDaChon = "Hector Barbossa";

            // 2. Thiết lập kết quả và đóng Form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSelectWT_Click(object sender, EventArgs e)
        {
            // 1. Gán tên nhân vật
            this.TenNhanVatDaChon = "Will Turner";

            // 2. Thiết lập kết quả và đóng Form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSelectJS_Click(object sender, EventArgs e)
        {
            // 1. Gán tên nhân vật
            this.TenNhanVatDaChon = "Jack Sparrow";

            // 2. Thiết lập kết quả và đóng Form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void pnlCharacter_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmSelectcharacter_Load(object sender, EventArgs e)
        {
            //Ẩn thanh tiêu đề nếu cần
            this.FormBorderStyle = FormBorderStyle.None;
        }
    }
}
