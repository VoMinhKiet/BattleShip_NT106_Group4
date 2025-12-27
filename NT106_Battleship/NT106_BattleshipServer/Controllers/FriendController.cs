using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Data;
using NT106_BattleshipServer.Models;

namespace NT106_BattleshipServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendController : ControllerBase
    {
        private readonly AppDbContext _db;
        public FriendController(AppDbContext db) => _db = db;

        private static (int a, int b) NormalizePair(int u1, int u2)
            => u1 < u2 ? (u1, u2) : (u2, u1);

        private static bool IsOnline(DateTime? lastOnline)
            => lastOnline.HasValue && lastOnline.Value >= DateTime.Now.AddMinutes(-3);

        // =========================
        // 1) LIST FRIENDS (ACCEPTED)
        // GET: /api/friend/list?userId=1
        // =========================
        [HttpGet("list")]
        public async Task<IActionResult> List([FromQuery] int userId)
        {
            if (userId <= 0) return BadRequest("userId invalid");

            var friendIds = await _db.BanBes.AsNoTracking()
                .Where(bb => bb.TrangThai == "ACCEPTED"
                          && (bb.IdNguoi1 == userId || bb.IdNguoi2 == userId))
                .Select(bb => bb.IdNguoi1 == userId ? bb.IdNguoi2 : bb.IdNguoi1)
                .Distinct()
                .ToListAsync();

            if (friendIds.Count == 0) return Ok(new List<object>());

            var friends = await (
                from u in _db.NguoiDungs.AsNoTracking()
                where friendIds.Contains(u.Id)
                join bxh in _db.BangXepHang.AsNoTracking()
                    on u.Id equals bxh.IdNguoiDung into bxhJoin
                from bxh in bxhJoin.DefaultIfEmpty()
                select new
                {
                    userId = u.Id,
                    username = u.TenDangNhap,
                    stars = bxh != null ? bxh.CapSao : 0,
                    relationStatus = "ACCEPTED",
                    online = IsOnline(u.LastOnline),
                    lastOnline = u.LastOnline
                }
            ).ToListAsync();

            return Ok(friends);
        }

        // =========================
        // 2) SEND FRIEND REQUEST
        // POST: /api/friend/add?userId=1&targetUserId=2
        // =========================
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromQuery] int userId, [FromQuery] int targetUserId)
        {
            if (userId <= 0 || targetUserId <= 0) return BadRequest("invalid id");
            if (userId == targetUserId) return BadRequest("cannot add yourself");

            var ok1 = await _db.NguoiDungs.AnyAsync(x => x.Id == userId);
            var ok2 = await _db.NguoiDungs.AnyAsync(x => x.Id == targetUserId);
            if (!ok1 || !ok2) return NotFound("user not found");

            var (a, b) = NormalizePair(userId, targetUserId);

            var existed = await _db.BanBes.FindAsync(a, b);
            if (existed != null) return Conflict($"relationship exists: {existed.TrangThai}");

            var row = new BanBe
            {
                IdNguoi1 = a,
                IdNguoi2 = b,
                TrangThai = "PENDING",
                IdNguoiThucHien = userId
            };

            _db.BanBes.Add(row);
            await _db.SaveChangesAsync();

            return Ok(new { message = "request_sent" });
        }

        // =========================
        // 3) LIST INCOMING REQUESTS
        // GET: /api/friend/requests?userId=1
        // =========================
        [HttpGet("requests")]
        public async Task<IActionResult> Requests([FromQuery] int userId)
        {
            if (userId <= 0) return BadRequest("userId invalid");

            var pendingFromIds = await _db.BanBes.AsNoTracking()
                .Where(bb => bb.TrangThai == "PENDING"
                          && (bb.IdNguoi1 == userId || bb.IdNguoi2 == userId)
                          && bb.IdNguoiThucHien != userId)
                .Select(bb => bb.IdNguoi1 == userId ? bb.IdNguoi2 : bb.IdNguoi1)
                .Distinct()
                .ToListAsync();

            if (pendingFromIds.Count == 0) return Ok(new List<object>());

            var reqs = await (
                from u in _db.NguoiDungs.AsNoTracking()
                where pendingFromIds.Contains(u.Id)
                join bxh in _db.BangXepHang.AsNoTracking()
                    on u.Id equals bxh.IdNguoiDung into bxhJoin
                from bxh in bxhJoin.DefaultIfEmpty()
                select new
                {
                    userId = u.Id,
                    username = u.TenDangNhap,
                    stars = bxh != null ? bxh.CapSao : 0,
                    relationStatus = "PENDING",
                    online = IsOnline(u.LastOnline),
                    lastOnline = u.LastOnline
                }
            ).ToListAsync();

            return Ok(reqs);
        }

        // =========================
        // 4) ACCEPT REQUEST
        // POST: /api/friend/accept?userId=1&targetUserId=2
        // =========================
        [HttpPost("accept")]
        public async Task<IActionResult> Accept([FromQuery] int userId, [FromQuery] int targetUserId)
        {
            if (userId <= 0 || targetUserId <= 0) return BadRequest("invalid id");
            if (userId == targetUserId) return BadRequest("invalid");

            var (a, b) = NormalizePair(userId, targetUserId);

            var row = await _db.BanBes.FindAsync(a, b);
            if (row == null) return NotFound("request not found");
            if (row.TrangThai != "PENDING") return BadRequest("not pending");

            // người gửi không được accept
            if (row.IdNguoiThucHien == userId) return BadRequest("sender cannot accept");

            row.TrangThai = "ACCEPTED";
            row.IdNguoiThucHien = userId;
            await _db.SaveChangesAsync();

            return Ok(new { message = "accepted" });
        }

        // =========================
        // 5) REJECT REQUEST (DELETE)
        // POST: /api/friend/reject?userId=1&targetUserId=2
        // =========================
        [HttpPost("reject")]
        public async Task<IActionResult> Reject([FromQuery] int userId, [FromQuery] int targetUserId)
        {
            if (userId <= 0 || targetUserId <= 0) return BadRequest("invalid id");
            if (userId == targetUserId) return BadRequest("invalid");

            var (a, b) = NormalizePair(userId, targetUserId);

            var row = await _db.BanBes.FindAsync(a, b);
            if (row == null) return NotFound("request not found");
            if (row.TrangThai != "PENDING") return BadRequest("not pending");

            // người gửi không được reject
            if (row.IdNguoiThucHien == userId) return BadRequest("sender cannot reject");

            _db.BanBes.Remove(row);
            await _db.SaveChangesAsync();

            return Ok(new { message = "rejected" });
        }

        // =========================
        // 6) DELETE FRIEND (UNFRIEND)
        // DELETE: /api/friend/delete?userId=1&targetUserId=2
        // =========================
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int userId, [FromQuery] int targetUserId)
        {
            if (userId <= 0 || targetUserId <= 0) return BadRequest("invalid id");
            if (userId == targetUserId) return BadRequest("invalid");

            var (a, b) = NormalizePair(userId, targetUserId);

            var row = await _db.BanBes.FindAsync(a, b);
            if (row == null) return NotFound("relationship not found");

            _db.BanBes.Remove(row);
            await _db.SaveChangesAsync();

            return Ok(new { message = "deleted" });
        }
    }
}
