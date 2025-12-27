using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NT106_BattleshipServer.Models
{
    [Table("BanBe")]
    public class BanBe
    {
        [Column("IdNguoi1")]
        public int IdNguoi1 { get; set; }

        [Column("IdNguoi2")]
        public int IdNguoi2 { get; set; }

        [Required]
        [Column("TrangThai")]
        public string TrangThai { get; set; } = "PENDING";

        [Column("IdNguoiThucHien")]
        public int IdNguoiThucHien { get; set; }
        public NguoiDung Nguoi1 { get; set; }
        public NguoiDung Nguoi2 { get; set; }
        public NguoiDung NguoiThucHien { get; set; }
    }
}
