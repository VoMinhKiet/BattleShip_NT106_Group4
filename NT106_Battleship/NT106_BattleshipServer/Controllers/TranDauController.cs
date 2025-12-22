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

    // GET api/trandau/list
    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<TranDauDto>>> GetAll()
    {
        var list = await _db.TranDau
            .AsNoTracking()
            .Select(t => new TranDauDto
            {
                Id = t.Id,
                IdPlayer1 = t.IdPlayer1,
                IdPlayer2 = t.IdPlayer2,
                IdNhanVat1 = t.IdNhanVat1,
                IdNhanVat2 = t.IdNhanVat2,
                KichThuoc = t.KichThuoc,
                Winner = t.Winner,
                TimeStart = t.TimeStart,
                TimeEnd = t.TimeEnd,
                IdPhongCho = t.IdPhongCho
            })
            .ToListAsync();

        return Ok(list);
    }

    // GET api/trandau/get/5
    [HttpGet("get/{id:int}")]
    public async Task<ActionResult<TranDauDto>> GetById(int id)
    {
        var t = await _db.TranDau
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new TranDauDto
            {
                Id = x.Id,
                IdPlayer1 = x.IdPlayer1,
                IdPlayer2 = x.IdPlayer2,
                IdNhanVat1 = x.IdNhanVat1,
                IdNhanVat2 = x.IdNhanVat2,
                KichThuoc = x.KichThuoc,
                Winner = x.Winner,
                TimeStart = x.TimeStart,
                TimeEnd = x.TimeEnd,
                IdPhongCho = x.IdPhongCho
            })
            .FirstOrDefaultAsync();

        if (t == null) return NotFound();
        return Ok(t);
    }

    // POST api/trandau/create
    [HttpPost("create")]
    public async Task<ActionResult<TranDauDto>> Create([FromBody] CreateTranDauRequest req)
    {
        // Model validation
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (req.KichThuoc < 8 || req.KichThuoc > 10)
            return BadRequest("KichThuoc must be 8, 9, or 10.");

        var entity = new TranDau
        {
            IdPlayer1 = req.IdPlayer1,
            IdPlayer2 = req.IdPlayer2,
            IdNhanVat1 = req.IdNhanVat1,
            IdNhanVat2 = req.IdNhanVat2,
            KichThuoc = req.KichThuoc,
            IdPhongCho = req.IdPhongCho,
            TimeStart = DateTime.UtcNow
        };

        _db.TranDau.Add(entity);
        await _db.SaveChangesAsync();

        var dto = new TranDauDto
        {
            Id = entity.Id,
            IdPlayer1 = entity.IdPlayer1,
            IdPlayer2 = entity.IdPlayer2,
            IdNhanVat1 = entity.IdNhanVat1,
            IdNhanVat2 = entity.IdNhanVat2,
            KichThuoc = entity.KichThuoc,
            Winner = entity.Winner,
            TimeStart = entity.TimeStart,
            TimeEnd = entity.TimeEnd,
            IdPhongCho = entity.IdPhongCho
        };

        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    // PUT api/trandau/update/5
    [HttpPut("update/{id:int}")]
    public async Task<ActionResult<TranDauDto>> Update(int id, [FromBody] CreateTranDauRequest req)
    {
        var entity = await _db.TranDau.FindAsync(id);
        if (entity == null) return NotFound();

        // update fields (you can make a separate Update DTO if you want)
        entity.IdPlayer1 = req.IdPlayer1;
        entity.IdPlayer2 = req.IdPlayer2;
        entity.IdNhanVat1 = req.IdNhanVat1;
        entity.IdNhanVat2 = req.IdNhanVat2;
        entity.KichThuoc = req.KichThuoc;
        entity.IdPhongCho = req.IdPhongCho;

        // validate KichThuoc
        if (entity.KichThuoc < 8 || entity.KichThuoc > 10)
            return BadRequest("KichThuoc must be 8, 9, or 10.");

        await _db.SaveChangesAsync();

        return Ok(new TranDauDto
        {
            Id = entity.Id,
            IdPlayer1 = entity.IdPlayer1,
            IdPlayer2 = entity.IdPlayer2,
            IdNhanVat1 = entity.IdNhanVat1,
            IdNhanVat2 = entity.IdNhanVat2,
            KichThuoc = entity.KichThuoc,
            Winner = entity.Winner,
            TimeStart = entity.TimeStart,
            TimeEnd = entity.TimeEnd,
            IdPhongCho = entity.IdPhongCho
        });
    }

    // POST api/trandau/end/5
    [HttpPost("end/{id:int}")]
    public async Task<ActionResult> EndMatch(int id, [FromBody] EndMatchRequest req)
    {
        var entity = await _db.TranDau.FindAsync(id);
        if (entity == null) return NotFound();

        // set winner and end time
        entity.Winner = req.WinnerId;
        entity.TimeEnd = req.TimeEnd ?? DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok();
    }

    // PATCH api/trandau/winner/5
    [HttpPut("winner/{id}")]
    public async Task<IActionResult> UpdateWinner(int id, UpdateWinnerRequest req)
    {
        var match = await _db.TranDau.FindAsync(id);
        if (match == null) return NotFound();

        match.Winner = req.WinnerId;

        await _db.SaveChangesAsync();
        return Ok();
    }

    // DELETE api/trandau/delete/5
    [HttpDelete("delete/{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        var entity = await _db.TranDau.FindAsync(id);
        if (entity == null) return NotFound();

        _db.TranDau.Remove(entity);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("history/{userId:int}")]
    public async Task<ActionResult<List<MatchHistoryDto>>> GetHistory(int userId)
    {
        var list = await (
            from t in _db.TranDau
            join p1 in _db.NguoiDungs on t.IdPlayer1 equals p1.Id
            join p2 in _db.NguoiDungs on t.IdPlayer2 equals p2.Id
            join nv1 in _db.NhanVat on t.IdNhanVat1 equals nv1.Id
            join nv2 in _db.NhanVat on t.IdNhanVat2 equals nv2.Id
            where t.IdPlayer1 == userId || t.IdPlayer2 == userId
            orderby t.TimeStart descending
            select new MatchHistoryDto
            {
                Id1 = p1.Id,
                NguoiChoi1 = p1.TenDangNhap,
                NhanVat1 = nv1.TenNhanVat,

                Id2 = p2.Id,
                NguoiChoi2 = p2.TenDangNhap,
                NhanVat2 = nv2.TenNhanVat,

                KetQua =
                    t.Winner == null ? "Chưa kết thúc" :
                    t.Winner == userId ? "Thắng" : "Thua",

                TimeStart = t.TimeStart,
                TimeEnd = t.TimeEnd
            }
        ).AsNoTracking().ToListAsync();

        return Ok(list);
    }

}
