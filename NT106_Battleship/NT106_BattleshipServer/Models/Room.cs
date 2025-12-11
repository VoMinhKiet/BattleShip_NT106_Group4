using NT106_BattleshipServer.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("PhongCho")]
public class Room
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Column("IDChuPhong")]
    public int IDChuPhong { get; set; }

    [Column("IDKhach")]
    public int? IDKhach { get; set; }

    [Column("TrangThai")]
    public string TrangThai { get; set; } = "waiting";

    [Column("NgayTao")]
    public DateTime NgayTao { get; set; }

    // Optional: Navigation Properties
    [ForeignKey("IDChuPhong")]
    public NguoiDung? ChuPhong { get; set; }

    [ForeignKey("IDKhach")]
    public NguoiDung? Khach { get; set; }
}
