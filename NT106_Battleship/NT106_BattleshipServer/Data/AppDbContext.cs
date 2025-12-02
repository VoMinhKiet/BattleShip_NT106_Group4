using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Models;

namespace NT106_BattleshipServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // ánh xạ bảng NguoiDung trong database
        public DbSet<NguoiDung> NguoiDungs { get; set; }

        // (sau này bạn có thể thêm các bảng khác như PhongCho, TranDau,...)
        public DbSet<BangXepHang> BangXepHangs { get; set; }
        public DbSet<BanBe> BanBes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BanBe>()
                .HasKey(b => new { b.IdNguoi1, b.IdNguoi2 });
        }
    }
}
