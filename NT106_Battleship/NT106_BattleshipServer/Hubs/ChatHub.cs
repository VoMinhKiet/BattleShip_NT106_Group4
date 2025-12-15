using Microsoft.AspNetCore.SignalR;
using NT106_BattleshipServer.DTOs;
using NT106_BattleshipServer.Data;
using NT106_BattleshipServer.Data.Entities;

namespace NT106_BattleshipServer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _db;

        public ChatHub(AppDbContext db)
        {
            _db = db;
        }

        public async Task JoinPhong(int idPhongCho)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                $"phong_{idPhongCho}"
            );
        }

        public async Task GuiTinNhan(TinNhanDto dto)
        {

            var tinNhan = new TinNhan
            {
                IdTranDau = dto.IdTranDau,
                IdPhongCho = dto.IdPhongCho,
                IdNguoiDung = dto.IdNguoiDung,
                NoiDung = dto.NoiDung,
                ThoiGian = DateTime.Now
            };

            _db.TinNhans.Add(tinNhan);
            await _db.SaveChangesAsync();

            dto.ThoiGian = tinNhan.ThoiGian;

            if (dto.IdPhongCho.HasValue)
            {
                await Clients.Group($"phong_{dto.IdPhongCho}")
                    .SendAsync("NhanTinNhan", dto);
            }
        }
    }
}
