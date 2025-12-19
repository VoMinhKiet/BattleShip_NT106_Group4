using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Data;
using NT106_BattleshipServer.Data.Entities;
using NT106_BattleshipServer.DTOs;

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
        public async Task JoinTranDau(int idTranDau)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                $"trandau_{idTranDau}"
            );
        }

        public async Task GuiTinNhan(TinNhanDto dto)
        {
            try
            {
                if (dto.IdTranDau != null)
                {
                    bool tranDauTonTai = await _db.TranDau
                        .AnyAsync(t => t.Id == dto.IdTranDau.Value);

                    if (!tranDauTonTai)
                    {

                        await Clients.Group($"trandau_{dto.IdTranDau}")
                            .SendAsync("NhanTinNhan", dto);
                        return;
                    }
                }

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

                if (dto.IdPhongCho != null)
                {
                    await Clients.Group($"phong_{dto.IdPhongCho}")
                        .SendAsync("NhanTinNhan", dto);
                }
                else if (dto.IdTranDau != null)
                {
                    await Clients.Group($"trandau_{dto.IdTranDau}")
                        .SendAsync("NhanTinNhan", dto);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

    }
}
