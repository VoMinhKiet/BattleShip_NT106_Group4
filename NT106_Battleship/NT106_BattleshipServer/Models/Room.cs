using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NT106_BattleshipServer.Models
{
    [Table("PhongCho")]
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        // ID của chủ phòng
        [Column("IDChuPhong")]
        public int IDChuPhong { get; set; }

        // ID của khách
        [Column("IDKhach")]
        public int? IDKhach { get; set; }

        // Trạng thái phòng chờ
        [Column("TrangThai")]
        public string TrangThai { get; set; } = "waiting";

        // Thời điểm tạo phòng
        [Column("NgayTao")]
        public DateTime NgayTao { get; set; }
    }
}
