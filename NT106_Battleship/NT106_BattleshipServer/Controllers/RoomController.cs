using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Data;
using NT106_BattleshipServer.Models;

namespace NT106_BattleshipServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly AppDbContext _context;
        public RoomController(AppDbContext context)
        {
            _context = context;
        }

        // DTO trả về cho client (có tên người dùng)
        public class RoomDto
        {
            public int Id { get; set; }
            public int IDChuPhong { get; set; }
            public string? TenChuPhong { get; set; }
            public int? IDKhach { get; set; }
            public string? TenKhach { get; set; }
            public string TrangThai { get; set; } = "waiting";
            public DateTime NgayTao { get; set; }
        }

        private async Task<RoomDto> BuildRoomDto(Room room)
        {
            var dto = new RoomDto
            {
                Id = room.Id,
                IDChuPhong = room.IDChuPhong,
                IDKhach = room.IDKhach,
                TrangThai = room.TrangThai,
                NgayTao = room.NgayTao
            };

            var host = await _context.NguoiDungs.FindAsync(room.IDChuPhong);
            dto.TenChuPhong = host?.TenDangNhap;

            if (room.IDKhach.HasValue)
            {
                var guest = await _context.NguoiDungs.FindAsync(room.IDKhach.Value);
                dto.TenKhach = guest?.TenDangNhap;
            }
            return dto;
        }

        // POST: api/room/create?userId=5
        [HttpPost("create")]
        public async Task<IActionResult> CreateRoom(int userId)
        {
            var room = new Room
            {
                IDChuPhong = userId,
                IDKhach = null,
                TrangThai = "waiting",
                NgayTao = DateTime.Now
            };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            var dto = await BuildRoomDto(room);
            return Ok(new { message = "Tạo phòng thành công", room = dto });
        }

        // GET: api/room/list
        [HttpGet("list")]
        public async Task<IActionResult> GetAvailableRooms()
        {
            var rooms = await _context.Rooms
                .Where(r => r.TrangThai == "waiting")
                .ToListAsync();

            var list = new List<RoomDto>();
            foreach (var r in rooms)
                list.Add(await BuildRoomDto(r));

            return Ok(list);
        }

        // POST: api/room/join?roomId=3&userId=10
        [HttpPost("join")]
        public async Task<IActionResult> JoinRoom(int roomId, int userId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
                return NotFound(new { message = "Phòng không tồn tại" });
            if (room.TrangThai != "waiting")
                return BadRequest(new { message = "Phòng không còn chỗ" });

            room.IDKhach = userId;
            room.TrangThai = "full";
            await _context.SaveChangesAsync();

            var dto = await BuildRoomDto(room);
            return Ok(new { message = "Tham gia phòng thành công", room = dto });
        }

        // GET: api/room/get?roomId=3
        [HttpGet("get")]
        public async Task<IActionResult> GetRoom(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
                return NotFound(new { message = "Phòng không tồn tại" });

            var dto = await BuildRoomDto(room);
            return Ok(dto);
        }

        // DELETE: api/room/leave?roomId=3&userId=10
        [HttpDelete("leave")]
        public async Task<IActionResult> LeaveRoom(int roomId, int userId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
                return NotFound(new { message = "Phòng không tồn tại" });

            if (room.IDChuPhong == userId)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Chủ phòng rời — phòng đã bị xoá" });
            }

            if (room.IDKhach == userId)
            {
                room.IDKhach = null;
                room.TrangThai = "waiting";
                await _context.SaveChangesAsync();

                var dto = await BuildRoomDto(room);
                return Ok(new { message = "Khách rời phòng", room = dto });
            }

            return BadRequest(new { message = "Người này không nằm trong phòng" });
        }

        // POST: api/room/start?roomId=3
        [HttpPost("start")]
        public async Task<IActionResult> StartGame(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
                return NotFound(new { message = "Phòng không tồn tại" });
            if (room.TrangThai != "full")
                return BadRequest(new { message = "Phòng chưa đủ người để chơi" });

            room.TrangThai = "playing";
            await _context.SaveChangesAsync();

            var dto = await BuildRoomDto(room);
            return Ok(new { message = "Trận đấu bắt đầu", room = dto });
        }

        // POST: api/room/finish?roomId=3
        [HttpPost("finish")]
        public async Task<IActionResult> FinishGame(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
                return NotFound(new { message = "Phòng không tồn tại" });
            if (room.TrangThai != "playing")
                return BadRequest(new { message = "Phòng không trong trạng thái playing" });

            room.TrangThai = "full";
            await _context.SaveChangesAsync();

            var dto = await BuildRoomDto(room);
            return Ok(new { message = "Trận đấu kết thúc — quay lại full", room = dto });
        }
    }
}
