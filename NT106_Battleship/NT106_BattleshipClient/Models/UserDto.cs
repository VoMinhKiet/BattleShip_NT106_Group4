using System;
namespace NT106_BattleshipClient.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string TenDangNhap { get; set; }
        public string Email { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
