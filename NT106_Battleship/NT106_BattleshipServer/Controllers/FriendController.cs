using Microsoft.AspNetCore.Mvc;
using NT106_BattleshipServer.Data;
using NT106_BattleshipServer.Models;
using System;
using System.Linq;

namespace NT106_BattleshipServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FriendController(AppDbContext context)
        {
            _context = context;
        }

        // ================== FIND ==================
        // GET: api/Friend/find?currentUserId=10&username=abc&id=23
        [HttpGet("find")]
        public IActionResult Find(int currentUserId, string? username, int? id)
        {
            if (string.IsNullOrEmpty(username) && !id.HasValue)
                return BadRequest(new { message = "Cần nhập username hoặc ID." });

            var query = _context.NguoiDungs.AsQueryable();

            if (!string.IsNullOrEmpty(username))
                query = query.Where(u => u.TenDangNhap == username);

            if (id.HasValue)
                query = query.Where(u => u.Id == id.Value);

            var target = query.FirstOrDefault();
            if (target == null)
                return NotFound(new { message = "Không tìm thấy người dùng." });

            if (target.Id == currentUserId)
                return BadRequest(new { message = "Không thể kết bạn với chính mình." });

            int idMin = Math.Min(currentUserId, target.Id);
            int idMax = Math.Max(currentUserId, target.Id);

            var relation = _context.BanBes
                .FirstOrDefault(b => b.IdNguoi1 == idMin && b.IdNguoi2 == idMax);

            string relationStatus = "NOT_FRIEND";
            string direction = "";

            if (relation != null)
            {
                relationStatus = relation.TrangThai; // PENDING / ACCEPTED / BLOCK
                if (relation.TrangThai == "PENDING")
                {
                    if (relation.IdNguoiThucHien == currentUserId)
                        direction = "YOU_SENT_REQUEST";
                    else if (relation.IdNguoiThucHien == target.Id)
                        direction = "THEY_SENT_REQUEST";
                }
            }

            var rankInfo = _context.BangXepHangs
                .FirstOrDefault(b => b.IdNguoiDung == target.Id);
            string rank = rankInfo?.BacRank ?? "ĐỒNG";

            bool isOnline = target.LastOnline != null &&
                            (DateTime.Now - target.LastOnline.Value).TotalSeconds <= 30;

            return Ok(new
            {
                id = target.Id,
                tenDangNhap = target.TenDangNhap,
                rank = rank,
                relation = relationStatus,
                direction = direction,
                online = isOnline,
                lastOnline = target.LastOnline
            });
        }

        // ================== ADD FRIEND ==================
        public class AddFriendRequest
        {
            public int CurrentUserId { get; set; }
            public int TargetUserId { get; set; }
        }

        // Gửi lời mời hoặc chấp nhận nếu bên kia đã gửi trước
        [HttpPost("add")]
        public IActionResult AddFriend([FromBody] AddFriendRequest model)
        {
            if (model.CurrentUserId == model.TargetUserId)
                return BadRequest(new { message = "Không thể kết bạn với chính mình." });

            var current = _context.NguoiDungs.FirstOrDefault(u => u.Id == model.CurrentUserId);
            var target = _context.NguoiDungs.FirstOrDefault(u => u.Id == model.TargetUserId);

            if (current == null || target == null)
                return NotFound(new { message = "Người dùng không tồn tại." });

            int idMin = Math.Min(model.CurrentUserId, model.TargetUserId);
            int idMax = Math.Max(model.CurrentUserId, model.TargetUserId);

            var relation = _context.BanBes
                .FirstOrDefault(b => b.IdNguoi1 == idMin && b.IdNguoi2 == idMax);

            // Chưa có quan hệ -> tạo lời mời mới
            if (relation == null)
            {
                relation = new BanBe
                {
                    IdNguoi1 = idMin,
                    IdNguoi2 = idMax,
                    TrangThai = "PENDING",
                    IdNguoiThucHien = model.CurrentUserId
                };
                _context.BanBes.Add(relation);
                _context.SaveChanges();

                return Ok(new
                {
                    message = "Đã gửi lời mời kết bạn.",
                    relation = "PENDING",
                    direction = "YOU_SENT_REQUEST"
                });
            }

            // Đã là bạn
            if (relation.TrangThai == "ACCEPTED")
            {
                return Ok(new
                {
                    message = "Hai bạn đã là bạn bè.",
                    relation = "ACCEPTED"
                });
            }

            // PENDING – nếu bên kia đã gửi trước, giờ mình bấm Add => CHẤP NHẬN
            if (relation.TrangThai == "PENDING")
            {
                if (relation.IdNguoiThucHien == model.CurrentUserId)
                {
                    return Ok(new
                    {
                        message = "Bạn đã gửi lời mời trước đó, đang chờ phản hồi.",
                        relation = "PENDING",
                        direction = "YOU_SENT_REQUEST"
                    });
                }
                else
                {
                    // Người kia gửi, mình bấm Add => ACCEPT
                    relation.TrangThai = "ACCEPTED";
                    relation.IdNguoiThucHien = model.CurrentUserId;
                    _context.SaveChanges();

                    return Ok(new
                    {
                        message = "Đã chấp nhận lời mời kết bạn.",
                        relation = "ACCEPTED"
                    });
                }
            }

            // BLOCK
            if (relation.TrangThai == "BLOCK")
            {
                return Ok(new
                {
                    message = "Không thể kết bạn vì đang ở trạng thái BLOCK.",
                    relation = "BLOCK"
                });
            }

            return Ok(new { message = "Trạng thái quan hệ không xác định." });
        }

        // ================== DELETE / HỦY ==================
        public class DeleteFriendRequest
        {
            public int CurrentUserId { get; set; }
            public int TargetUserId { get; set; }
        }

        // Xoá bạn hoặc huỷ lời mời
        [HttpPost("delete")]
        public IActionResult DeleteFriend([FromBody] DeleteFriendRequest model)
        {
            if (model.CurrentUserId == model.TargetUserId)
                return BadRequest(new { message = "Không hợp lệ." });

            int idMin = Math.Min(model.CurrentUserId, model.TargetUserId);
            int idMax = Math.Max(model.CurrentUserId, model.TargetUserId);

            var relation = _context.BanBes
                .FirstOrDefault(b => b.IdNguoi1 == idMin && b.IdNguoi2 == idMax);

            if (relation == null)
                return NotFound(new { message = "Không có quan hệ bạn bè/lời mời để xoá." });

            _context.BanBes.Remove(relation);
            _context.SaveChanges();

            return Ok(new { message = "Đã xoá quan hệ bạn bè / huỷ lời mời." });
        }

        [HttpGet("list")]
        public IActionResult GetFriendList(int currentUserId)
        {
            // Kiểm tra user tồn tại
            var user = _context.NguoiDungs.FirstOrDefault(u => u.Id == currentUserId);
            if (user == null)
                return NotFound(new { message = "Người dùng không tồn tại" });

            // Lấy tất cả quan hệ ACCEPTED liên quan tới user
            var friends = _context.BanBes
                .Where(b => b.TrangThai == "ACCEPTED" &&
                           (b.IdNguoi1 == currentUserId || b.IdNguoi2 == currentUserId))
                .ToList();

            var result = new List<object>();

            foreach (var f in friends)
            {
                int friendId = (f.IdNguoi1 == currentUserId) ? f.IdNguoi2 : f.IdNguoi1;

                var friend = _context.NguoiDungs.FirstOrDefault(u => u.Id == friendId);
                if (friend == null) continue;

                var rankInfo = _context.BangXepHangs
                    .FirstOrDefault(b => b.IdNguoiDung == friendId);

                bool online = friend.LastOnline != null &&
                              (DateTime.Now - friend.LastOnline.Value).TotalSeconds <= 30;

                result.Add(new
                {
                    id = friend.Id,
                    tenDangNhap = friend.TenDangNhap,
                    rank = rankInfo?.BacRank ?? "ĐỒNG",
                    online = online,
                    lastOnline = friend.LastOnline
                });
            }

            return Ok(result);
        }

    }
}
