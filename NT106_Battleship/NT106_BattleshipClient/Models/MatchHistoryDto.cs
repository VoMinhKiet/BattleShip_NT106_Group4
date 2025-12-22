using System;

namespace NT106_BattleshipClient.Models
{
    public class MatchHistoryDto
    {
        public int Id1 { get; set; }
        public string NguoiChoi1 { get; set; }
        public string NhanVat1 { get; set; }

        public int Id2 { get; set; }
        public string NguoiChoi2 { get; set; }
        public string NhanVat2 { get; set; }

        public string KetQua { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
    }
}
