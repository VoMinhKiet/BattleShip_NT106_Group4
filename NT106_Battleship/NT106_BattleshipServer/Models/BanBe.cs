using System.ComponentModel.DataAnnotations.Schema;

namespace NT106_BattleshipServer.Models
{
    [Table("BanBe")]
    public class BanBe
    {
        public int IdNguoi1 { get; set; }
        public int IdNguoi2 { get; set; }

        public string TrangThai { get; set; } = "PENDING"; // PENDING / ACCEPTED / BLOCK
        public int IdNguoiThucHien { get; set; }

        [ForeignKey("IdNguoi1")]
        public NguoiDung? Nguoi1 { get; set; }

        [ForeignKey("IdNguoi2")]
        public NguoiDung? Nguoi2 { get; set; }

        [ForeignKey("IdNguoiThucHien")]
        public NguoiDung? NguoiThucHien { get; set; }
    }
}
