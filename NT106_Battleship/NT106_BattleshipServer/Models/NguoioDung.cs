using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NT106_BattleshipServer.Models
{
    [Table("NguoiDung")]
    public class NguoiDung
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("TenDangNhap")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required]
        [Column("MatKhau")]
        public string MatKhau { get; set; } = string.Empty;

        [Column("Email")]
        public string? Email { get; set; }

        [Column("NgayTao")]
        public DateTime NgayTao { get; set; }

        // Mã OTP reset mật khẩu
        [Column("ResetCode")]
        public string? ResetCode { get; set; }

        // Thời điểm hết hạn OTP
        [Column("ResetCodeExpire")]
        public DateTime? ResetCodeExpire { get; set; }
    }
}
