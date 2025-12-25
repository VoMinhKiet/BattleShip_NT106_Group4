using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Data;
using NT106_BattleshipServer.DTOs;

[ApiController]
[Route("api/[controller]")]
public class TranDauController : ControllerBase
{
    private readonly AppDbContext _db;

    public TranDauController(AppDbContext db)
    {
        _db = db;
    }

    // 1. TẠO TRẬN ĐẤU (Lưu Tên NV thay vì ID)
    [HttpPost("create")]
    public async Task<ActionResult<TranDauDto>> Create([FromBody] CreateTranDauRequest req)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new TranDau
        {
            IdPlayer1 = req.IdPlayer1,
            IdPlayer2 = req.IdPlayer2,
            TenNV1 = req.TenNV1,
            TenNV2 = req.TenNV2,
            KichThuoc = req.KichThuoc,
            IdPhongCho = req.IdPhongCho,
            TimeStart = DateTime.UtcNow,
            Winner = null
        };

        _db.TranDau.Add(entity);
        await _db.SaveChangesAsync();

        // Trả về DTO
        return Ok(new TranDauDto
        {
            Id = entity.Id,
            IdPlayer1 = entity.IdPlayer1,
            IdPlayer2 = entity.IdPlayer2,
            TenNV1 = entity.TenNV1,
            TenNV2 = entity.TenNV2,
            KichThuoc = entity.KichThuoc,
            Winner = entity.Winner,
            TimeStart = entity.TimeStart,
            TimeEnd = entity.TimeEnd,
            IdPhongCho = entity.IdPhongCho
        });
    }

    // 2. KẾT THÚC TRẬN ĐẤU
    [HttpPost("end/{id:int}")]
    public async Task<ActionResult> EndMatch(int id, [FromBody] EndMatchRequest req)
    {
        var entity = await _db.TranDau.FindAsync(id);
        if (entity == null) return NotFound();

        entity.Winner = req.WinnerId;
        entity.TimeEnd = req.TimeEnd ?? DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok();
    }

    // 3. LẤY LỊCH SỬ
    [HttpGet("history/{userId:int}")]
    public async Task<ActionResult<List<MatchHistoryDto>>> GetHistory(int userId)
    {
        var list = await (
            from t in _db.TranDau
            join p1 in _db.NguoiDungs on t.IdPlayer1 equals p1.Id
            join p2 in _db.NguoiDungs on t.IdPlayer2 equals p2.Id
            // KHÔNG CẦN JOIN NHANVAT NỮA
            where t.IdPlayer1 == userId || t.IdPlayer2 == userId
            orderby t.TimeStart descending
            select new MatchHistoryDto
            {
                Id1 = p1.Id,
                NguoiChoi1 = p1.TenDangNhap,
                NhanVat1 = t.TenNV1,

                Id2 = p2.Id,
                NguoiChoi2 = p2.TenDangNhap,
                NhanVat2 = t.TenNV2, 

                KetQua = t.Winner == null ? "Chưa kết thúc" :
                         t.Winner == userId ? "Thắng" : "Thua",

                TimeStart = t.TimeStart,
                TimeEnd = t.TimeEnd
            }
        ).AsNoTracking().ToListAsync();

        return Ok(list);
    }

}
