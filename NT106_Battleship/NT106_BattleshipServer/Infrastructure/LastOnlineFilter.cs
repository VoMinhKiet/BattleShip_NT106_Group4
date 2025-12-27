using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Data;

namespace NT106_BattleshipServer.Infrastructure
{
    public class LastOnlineFilter : IAsyncActionFilter
    {
        private readonly AppDbContext _db;

        public LastOnlineFilter(AppDbContext db)
        {
            _db = db;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Query.TryGetValue("userId", out var q)
                && int.TryParse(q.ToString(), out int userId))
            {
                await _db.NguoiDungs
                    .Where(u => u.Id == userId)
                    .ExecuteUpdateAsync(s =>
                        s.SetProperty(x => x.LastOnline, DateTime.Now)
                    );
            }

            await next();
        }
    }
}
