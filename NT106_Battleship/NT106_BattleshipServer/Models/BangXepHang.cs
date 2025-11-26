using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NT106_BattleshipServer.Models
{
    [Table("BangXepHang")]  // map đúng tên bảng SQL
    public class BangXepHang
    {
        [Key]
        public int Id { get; set; }

        public int IdNguoiDung { get; set; }
        public int SoTranThang { get; set; } = 0;
        public int SoTranThua { get; set; } = 0;
        public string BacRank { get; set; } = "ĐỒNG";
        public int CapSao { get; set; } = 0;

        [ForeignKey("IdNguoiDung")]
        public NguoiDung? NguoiDung { get; set; }
    }
}
