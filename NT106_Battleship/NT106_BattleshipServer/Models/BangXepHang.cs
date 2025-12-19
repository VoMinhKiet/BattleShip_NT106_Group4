using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NT106_BattleshipServer.Models

{
    [Table("BangXepHang")]
    public class BangXepHang
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdNguoiDung { get; set; }

        public int SoTranThang { get; set; }
        public int SoTranThua { get; set; }
        public int CapSao { get; set; }
    }
}
