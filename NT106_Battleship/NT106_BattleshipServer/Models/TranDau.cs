using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TranDau")]
public class TranDau
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int IdPlayer1 { get; set; }

    [Required]
    public int IdPlayer2 { get; set; }

    [Required]
    public string TenNV1 { get; set; }

    [Required]
    public string TenNV2 { get; set; }

    [Required]
    [Range(8, 10)]
    public int KichThuoc { get; set; }

    public int? Winner { get; set; }

    [Required]
    public DateTime TimeStart { get; set; } = DateTime.UtcNow;

    public DateTime? TimeEnd { get; set; }

    [Required]
    public int IdPhongCho { get; set; }
}
