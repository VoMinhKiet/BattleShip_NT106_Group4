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
        private const int MAX_STARS = 5;

        public PlayerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/player/profile/{id} - Lấy hồ sơ người chơi
        [HttpGet("profile/{id}")]
        public IActionResult GetPlayerProfile(int id)
        {
            var user = _context.NguoiDungs.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound(new { message = "Không tìm thấy người chơi!" });

            var rankInfo = _context.BangXepHangs.FirstOrDefault(b => b.IdNguoiDung == id);

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

                // +1 sao nếu chưa đủ 5
                if (rankInfo.CapSao < MAX_STARS)
                {
                    rankInfo.CapSao++;
                }
                else
                {
                    // Đang 5 sao và thắng thêm
                    var next = NextRank(rankInfo.BacRank);

                    if (next == rankInfo.BacRank)
                    {
                        // Đã KIM CƯƠNG, giữ nguyên rank và sao max
                        rankInfo.CapSao = MAX_STARS;
                    }
                    else
                    {
                        // Lên rank mới, sao = 1
                        rankInfo.BacRank = next;
                        rankInfo.CapSao = 1;
                    }
                }
            }
            else
            {
                rankInfo.SoTranThua++;

                // Thua: trừ 1 sao, không tụt rank, không nhỏ hơn 0
                if (rankInfo.CapSao > 0)
                    rankInfo.CapSao--;
            }

            _context.SaveChanges();

            return Ok(new
            {
                message = "Cập nhật kết quả trận đấu thành công!",
                bacRank = rankInfo.BacRank,
                capSao = rankInfo.CapSao,
                soTranThang = rankInfo.SoTranThang,
                soTranThua = rankInfo.SoTranThua
            });
        }

        // Hàm xác định rank tiếp theo
        private string NextRank(string current)
        {
            switch (current)
            {
                case "ĐỒNG": return "BẠC";
                case "BẠC": return "VÀNG";
                case "VÀNG": return "BẠCH KIM";
                case "BẠCH KIM": return "KIM CƯƠNG";
                case "KIM CƯƠNG": return "CAO THỦ";
                case "CAO THỦ":
                default: return "KIM CƯƠNG"; // cao nhất, không lên nữa
            }
        }
    }

    public class FinishMatchRequest
    {
        public int PlayerId { get; set; }
        public bool IsWin { get; set; }
    }
}
