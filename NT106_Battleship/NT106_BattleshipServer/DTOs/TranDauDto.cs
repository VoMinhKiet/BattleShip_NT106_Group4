namespace NT106_BattleshipServer.DTOs
{
    public class CreateTranDauRequest
    {
        public int IdPlayer1 { get; set; }
        public int IdPlayer2 { get; set; }
        public string TenNV1 { get; set; }
        public string TenNV2 { get; set; }
        public int KichThuoc { get; set; }
        public int IdPhongCho { get; set; }
    }

    public class EndMatchRequest
    {
        public int WinnerId { get; set; }
        public DateTime? TimeEnd { get; set; }
    }

    public class TranDauDto
    {
        public int Id { get; set; }
        public int IdPlayer1 { get; set; }
        public int IdPlayer2 { get; set; }
        public string TenNV1 { get; set; }
        public string TenNV2 { get; set; }
        public int KichThuoc { get; set; }
        public int? Winner { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public int IdPhongCho { get; set; }
    }

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