using Microsoft.AspNetCore.Mvc;
using NT106_BattleshipServer.Data;
using NT106_BattleshipServer.Models;
using System.Linq;

namespace NT106_BattleshipServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlayerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/player/profile/{id} - Lấy hồ sơ người chơi
        [HttpGet("profile/{id}")]
        public IActionResult GetPlayerProfile(int id)
        {
            // Lấy thông tin tài khoản người chơi từ bảng NguoiDung
            var user = _context.NguoiDungs.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound(new { message = "Không tìm thấy người chơi!" });

            // Lấy thông tin rank từ bảng BangXepHang
            var rankInfo = _context.BangXepHangs.FirstOrDefault(b => b.IdNguoiDung == id);

            // Tính tổng số trận và tỉ lệ thắng
            int soThang = rankInfo?.SoTranThang ?? 0;
            int soThua = rankInfo?.SoTranThua ?? 0;
            int tongTran = soThang + soThua;
            double tiLeThang = tongTran > 0 ? (soThang * 100.0 / tongTran) : 0;

            return Ok(new
            {
                id = user.Id,
                tenDangNhap = user.TenDangNhap,
                email = user.Email,
                bacRank = rankInfo?.BacRank ?? "ĐỒNG",
                capSao = rankInfo?.CapSao ?? 0,
                tongTran = tongTran,
                tiLeThang = tiLeThang
            });
        }

        // POST: api/player/finish-match - Cập nhật kết quả trận đấu (sau mỗi trận)
        [HttpPost("finish-match")]
        public IActionResult FinishMatch([FromBody] FinishMatchRequest model)
        {
            var player = _context.NguoiDungs.FirstOrDefault(u => u.Id == model.PlayerId);
            if (player == null)
                return NotFound(new { message = "Người chơi không tồn tại!" });

            var rankInfo = _context.BangXepHangs.FirstOrDefault(b => b.IdNguoiDung == model.PlayerId);

            if (rankInfo == null)
            {
                rankInfo = new BangXepHang
                {
                    IdNguoiDung = model.PlayerId,
                    SoTranThang = 0,
                    SoTranThua = 0,
                    BacRank = "ĐỒNG",
                    CapSao = 0
                };
                _context.BangXepHangs.Add(rankInfo);
            }

            if (model.IsWin)
            {
                rankInfo.SoTranThang++;
            }
            else
            {
                rankInfo.SoTranThua++;
            }

            _context.SaveChanges();
            return Ok(new { message = "Cập nhật kết quả trận đấu thành công!" });
        }
    }
}

public class FinishMatchRequest
{
    public int PlayerId { get; set; }
    public bool IsWin { get; set; }
}
