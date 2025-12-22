using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Data;
using NT106_BattleshipServer.DTOs;
using NT106_BattleshipServer.Models;
using System.Threading.Tasks;

namespace NT106_BattleshipServer.Hubs
{
    public class BattleRankingHub : Hub
    {
        private readonly AppDbContext _context;

        public BattleRankingHub(AppDbContext context)
        {
            _context = context;
        }

        // Thắng +1 sao | Thua -1 sao (>= 0)
        public async Task FinishBattle(BattleMatchResultDto dto)
        {
            var bxh = await _context.BangXepHang
                .FirstOrDefaultAsync(x => x.IdNguoiDung == dto.IdNguoiDung);

            if (bxh == null)
            {
                bxh = new BangXepHang
                {
                    IdNguoiDung = dto.IdNguoiDung,
                    SoTranThang = 0,
                    SoTranThua = 0,
                    CapSao = 0
                };
                _context.BangXepHang.Add(bxh);
            }

            if (dto.IsWin)
            {
                bxh.SoTranThang++;
                bxh.CapSao++;
            }
            else
            {
                bxh.SoTranThua++;
                if (bxh.CapSao > 0)
                    bxh.CapSao--;
            }

            await _context.SaveChangesAsync();



        }

    }
}
