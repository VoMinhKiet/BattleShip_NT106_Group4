using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NT106_BattleshipServer.Data.Entities
{
    [Table("TinNhan")]
    public class TinNhan
    {
        [Key]
        public int Id { get; set; }

        public int? IdTranDau { get; set; }

        public int? IdPhongCho { get; set; }

        [Required]
        public int IdNguoiDung { get; set; }

        [Required]
        [MaxLength(100)]
        public string NoiDung { get; set; } = "";

        public DateTime ThoiGian { get; set; }
    }
}
