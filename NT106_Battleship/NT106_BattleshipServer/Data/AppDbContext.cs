using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Data.Entities;
using NT106_BattleshipServer.Models;


namespace NT106_BattleshipServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // ánh xạ bảng NguoiDung trong database
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<Room> Rooms { get; set; }
        // (sau này bạn có thể thêm các bảng khác như PhongCho, TranDau,...)
        public DbSet<TinNhan> TinNhans { get; set; }
        public DbSet<TranDau> TranDau { get; set; }
        public DbSet<NhanVat> NhanVat { get; set; }

        public DbSet<BangXepHang> BangXepHang { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
