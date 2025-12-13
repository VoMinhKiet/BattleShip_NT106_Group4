// Dtos/TranDauDto.cs
public class TranDauDto
{
    public int Id { get; set; }
    public int IdPlayer1 { get; set; }
    public int IdPlayer2 { get; set; }
    public int IdNhanVat1 { get; set; }
    public int IdNhanVat2 { get; set; }
    public int KichThuoc { get; set; }
    public int? Winner { get; set; }
    public DateTime TimeStart { get; set; }
    public DateTime? TimeEnd { get; set; }
    public int IdPhongCho { get; set; }
}

// Dtos/CreateTranDauRequest.cs
public class CreateTranDauRequest
{
    public int IdPlayer1 { get; set; }
    public int IdPlayer2 { get; set; }
    public int IdNhanVat1 { get; set; }
    public int IdNhanVat2 { get; set; }
    public int KichThuoc { get; set; }
    public int IdPhongCho { get; set; }
}

// Dtos/UpdateWinnerRequest.cs
public class UpdateWinnerRequest
{
    public int WinnerId { get; set; }
}

// Dtos/EndMatchRequest.cs
public class EndMatchRequest
{
    public int WinnerId { get; set; }
    // optional: TimeEnd can be provided; otherwise server will set now
    public DateTime? TimeEnd { get; set; }
}