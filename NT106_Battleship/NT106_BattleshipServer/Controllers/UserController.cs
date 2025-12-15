using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Data;

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
        [HttpGet("{id}")]
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
    }
}
