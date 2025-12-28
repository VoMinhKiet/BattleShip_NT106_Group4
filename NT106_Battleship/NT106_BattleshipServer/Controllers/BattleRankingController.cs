using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Data;
using System.Linq;

namespace NT106_BattleshipServer.Controllers
{
    [ApiController]
    [Route("api/battle-ranking")]
    public class BattleRankingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BattleRankingController(AppDbContext context)
        {
            _context = context;
        }

        // Lấy bảng xếp hạng theo số sao
        [HttpGet]
        public IActionResult GetBattleRanking()
        {
            var data = _context.BangXepHang
                .OrderByDescending(x => x.CapSao)
                .Select(x => new
                {
                    x.IdNguoiDung,
                    x.CapSao,
                    x.SoTranThang,
                    x.SoTranThua
                })
                .ToList();

            return Ok(data);
        }
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetUserRanking(int userId)
        {
            var bxh = await _context.BangXepHang
                .Where(x => x.IdNguoiDung == userId)
                .Select(x => new
                {
                    CapSao = x.CapSao,
                    TongSoTran = x.SoTranThang + x.SoTranThua,
                    TiLeThang = (x.SoTranThang + x.SoTranThua) == 0
                        ? 0
                        : Math.Round(
                            x.SoTranThang * 100.0 /
                            (x.SoTranThang + x.SoTranThua), 2)
                })
                .FirstOrDefaultAsync();

            if (bxh == null)
            {
                return Ok(new
                {
                    CapSao = 0,
                    TongSoTran = 0,
                    TiLeThang = 0
                });
            }

            return Ok(bxh);
        }
    }
}
