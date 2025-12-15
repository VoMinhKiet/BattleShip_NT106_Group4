using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Data;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly AppDbContext _db;

    public ChatController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("phong/{idPhongCho}")]
    public async Task<IActionResult> GetChatPhong(int idPhongCho)
    {
        var list = await _db.TinNhans
            .Where(t => t.IdPhongCho == idPhongCho)
            .OrderBy(t => t.ThoiGian)
            .Select(t => new
            {
                t.IdNguoiDung,
                t.NoiDung,
                t.ThoiGian
            })
            .ToListAsync();

        return Ok(list);
    }
}
