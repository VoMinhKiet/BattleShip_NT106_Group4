using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Data;
using NT106_BattleshipServer.Models;

namespace NT106_BattleshipServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;  // Inject DbContext
        }

        // =============================
        // GET api/User/{id}
        // → Lấy thông tin 1 user theo ID
        // =============================
        [HttpGet("get/{id:int}")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.NguoiDungs
                .Where(u => u.Id == id)     // tìm theo ID
                .Select(u => new            // trả về DTO đơn giản
                {
                    Id = u.Id,
                    TenDangNhap = u.TenDangNhap,
                    Email = u.Email,
                    NgayTao = u.NgayTao
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(user); // trả về thông tin user
        }
        // ===========================================
        // GET api/User/search?userId=1&id=2&username=abc
        // → Tìm user toàn hệ thống (ưu tiên id nếu có)
        // → Trả về dạng FriendDto-like để client hiển thị + status
        // ===========================================
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] int userId, [FromQuery] int? id, [FromQuery] string? username)
        {
            if (userId <= 0) return BadRequest("userId invalid");

            IQueryable<NguoiDung> q = _context.NguoiDungs.AsNoTracking();

            // ưu tiên tìm theo ID nếu có
            if (id.HasValue && id.Value > 0)
            {
                q = q.Where(u => u.Id == id.Value);
            }
            else
            {
                username = (username ?? "").Trim();
                if (string.IsNullOrEmpty(username))
                    return Ok(new List<object>());

                q = q.Where(u => u.TenDangNhap.Contains(username));
            }

            var users = await q.Take(30).ToListAsync();
            if (users.Count == 0) return Ok(new List<object>());

            var userIds = users.Select(u => u.Id).ToList();

            // lấy status quan hệ bạn bè giữa userId và các user tìm được
            var pairs = userIds
                .Select(x => new { A = Math.Min(userId, x), B = Math.Max(userId, x), Other = x })
                .Where(p => p.A != p.B)
                .ToList();

            var aSet = pairs.Select(p => p.A).Distinct().ToList();
            var bSet = pairs.Select(p => p.B).Distinct().ToList();

            var relations = await _context.BanBes.AsNoTracking()
                .Where(bb => aSet.Contains(bb.IdNguoi1) && bSet.Contains(bb.IdNguoi2))
                .ToListAsync();

            var relMap = relations.ToDictionary(r => $"{r.IdNguoi1}-{r.IdNguoi2}", r => r.TrangThai);

            // Stars từ BangXepHang nếu có
            var starsMap = await _context.BangXepHang.AsNoTracking()
                .Where(b => userIds.Contains(b.IdNguoiDung))
                .ToDictionaryAsync(x => x.IdNguoiDung, x => x.CapSao);

            var result = users.Select(u =>
            {
                string status = "NONE";
                if (u.Id != userId)
                {
                    var a = Math.Min(userId, u.Id);
                    var b = Math.Max(userId, u.Id);
                    if (relMap.TryGetValue($"{a}-{b}", out var st))
                        status = st; // PENDING/ACCEPTED/BLOCK
                }

                return new
                {
                    userId = u.Id,
                    username = u.TenDangNhap,
                    stars = starsMap.TryGetValue(u.Id, out var s) ? s : 0,
                    relationStatus = status,
                    online = false,          // realtime thì lấy từ SignalR
                    lastOnline = u.LastOnline
                };
            });

            return Ok(result);
        }
        // =============================
        // GET api/User/find/{username}
        // → Tìm user theo Tên đăng nhập (String)
        // =============================
        [HttpGet("find/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _context.NguoiDungs
                .Where(u => u.TenDangNhap == username) // So sánh tên
                .Select(u => new
                {
                    Id = u.Id,
                    TenDangNhap = u.TenDangNhap,
                    Email = u.Email
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Không tìm thấy user có tên này" });

            return Ok(user);
        }
    }
}
