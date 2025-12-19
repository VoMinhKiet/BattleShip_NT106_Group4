using System;

public class TranDauDto
{
    public int Id { get; set; }
    public int IdPlayer1 { get; set; }
    public int? IdPlayer2 { get; set; }
    public string TenNV1 { get; set; }
    public string TenNV2 { get; set; }
    public int KichThuoc { get; set; }
    public int? Winner { get; set; }
    public DateTime TimeStart { get; set; }
    public DateTime? TimeEnd { get; set; }
    public int IdPhongCho { get; set; }
}
