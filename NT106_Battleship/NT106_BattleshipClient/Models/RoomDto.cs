using System;

namespace NT106_BattleshipClient.Models
{
    public class RoomDto
    {
        public int Id { get; set; }
        public int IDChuPhong { get; set; }
        public string TenChuPhong { get; set; }
        public int? IDKhach { get; set; }
        public string TenKhach { get; set; }
        public string TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
    }
}
