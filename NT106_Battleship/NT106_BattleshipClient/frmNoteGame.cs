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
    public partial class frmNoteGame : Form
    {
        public frmNoteGame()
        {
            InitializeComponent();
        }

        private void frmNoteGame_Load(object sender, EventArgs e)
        {
            rchtxtNote.ReadOnly = true;
            rchtxtNote.ScrollBars = RichTextBoxScrollBars.Vertical;
            rchtxtNote.BorderStyle = BorderStyle.None;

            rchtxtNote.Text =
@"⚓══════════════════════════════════════════════════════════════⚓
                     BATTLESHIP GAME — HƯỚNG DẪN LUẬT CHƠI
⚓══════════════════════════════════════════════════════════════⚓

🎯 MỤC TIÊU:
   ▪ Đánh chìm toàn bộ hạm đội của đối phương trước khi họ đánh chìm tàu của bạn.
   ▪ Ai tiêu diệt hết tàu đối thủ trước sẽ là NGƯỜI CHIẾN THẮNG!

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

🧭 CÁCH CHƠI:
   1️. Mỗi người chơi sở hữu một bản đồ ô vuông (Map) dùng để đặt tàu và tấn công.
       • Có thể chọn kích thước bản đồ:
           - 8×8 : Dành cho người mới, chơi nhanh, ít tàu.
           - 9×9 : Cân bằng giữa tốc độ và chiến thuật.
           - 10×10 : Chuẩn quốc tế, độ khó cao, chiến lược phức tạp.
           
   2️. Trước khi bắt đầu:
       • Đặt các tàu của bạn vào vị trí mong muốn trên bản đồ.
       • Mỗi tàu có chiều dài khác nhau (tàu ngầm, khu trục, tuần dương, thiết giáp...).
       • Tàu không được chồng lên nhau hoặc vượt ra ngoài bản đồ.

   3️. Trong lượt của bạn:
       • Chọn một ô trên bản đồ đối phương để bắn phá.
       • Nếu bắn trúng tàu, ô đó hiển thị 🔥 **HIT!**
       • Nếu trượt, ô đó hiển thị 💦 **MISS!**
       • Sau đó, lượt chơi sẽ chuyển cho đối phương.

   4️. ⏳ **Thời gian cho mỗi lượt bắn: 15 giây**
       • Nếu bạn không chọn ô nào trong vòng 15 giây, lượt của bạn sẽ tự động bị bỏ qua.
       • Hãy ra quyết định thật nhanh và chính xác!

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

🏁 KẾT THÚC TRẬN:
   ▪ Khi toàn bộ tàu của một người bị đánh chìm, trận đấu kết thúc.
   ▪ Người còn lại được tuyên bố chiến thắng ⚔️!

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

💡 MẸO CHIẾN THUẬT:
   🌊 1. Không nên đặt tàu sát nhau — dễ bị đối phương phát hiện theo chuỗi.
   📍 2. Ghi nhớ vị trí đã bắn để tối ưu hóa lượt tấn công.
   🧭 3. Trên map 8×8: chơi nhanh, đánh phủ toàn bản đồ.
   ⚙️ 4. Trên map 9×9 và 10×10: chia bản đồ thành các vùng để tìm tàu hiệu quả.
   🔎 5. Khi bắn trúng, hãy dò theo hướng ngang hoặc dọc để tìm hết thân tàu.
   ⏰ 6. Quản lý thời gian 15s thật tốt — đừng để lượt của bạn trôi qua vô ích!

━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

🏆 LỜI NHẮN TỪ THUYỀN TRƯỞNG:
   “Thời gian là đạn pháo mạnh nhất — nếu bạn không khai hỏa đúng lúc,
    bạn sẽ là người bị đánh chìm trước!”

🎉 Chúc bạn có những trận hải chiến thật gay cấn và vui vẻ!
⚓══════════════════════════════════════════════════════════════⚓";

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rchtxtNote_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
