using System;

namespace NT106_BattleshipClient.Models
{
    public class TinNhanDto
    {
        public int? IdTranDau { get; set; }
        public int? IdPhongCho { get; set; }

        public int IdNguoiDung { get; set; }
        public string TenNguoiDung { get; set; } = "";
        public string NoiDung { get; set; } = "";

        public DateTime ThoiGian { get; set; }
    }

}
