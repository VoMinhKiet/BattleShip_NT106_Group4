namespace NT106_BattleshipServer.DTOs
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
