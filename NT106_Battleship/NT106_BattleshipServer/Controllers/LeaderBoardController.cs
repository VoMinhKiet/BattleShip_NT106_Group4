using Microsoft.AspNetCore.Mvc;
using NT106_BattleshipServer.Data;
using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.DTOs;

[ApiController]
[Route("api/[controller]")]
public class LeaderBoardController : ControllerBase
{
    private readonly AppDbContext _db;

    public LeaderBoardController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var list = await (
            from u in _db.NguoiDungs
            join bxh in _db.BangXepHang
                on u.Id equals bxh.IdNguoiDung into g
            from bxh in g.DefaultIfEmpty()   // LEFT JOIN
            select new LeaderBoardDto
            {
                IdNguoiDung = u.Id,
                TenNguoiDung = u.TenDangNhap,

                SoTranThang = bxh != null ? bxh.SoTranThang : 0,
                SoTranThua = bxh != null ? bxh.SoTranThua : 0,
                CapSao = bxh != null ? bxh.CapSao : 0
            }
        )
        .OrderByDescending(x => x.CapSao)
        .ThenByDescending(x => x.SoTranThang)
        .ToListAsync();

        return Ok(list);
    }
}
