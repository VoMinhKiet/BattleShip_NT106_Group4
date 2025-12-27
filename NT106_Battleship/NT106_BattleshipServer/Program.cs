using Microsoft.EntityFrameworkCore;
using NT106_BattleshipServer.Data;
using NT106_BattleshipServer.Hubs;
using Microsoft.AspNetCore.SignalR;
using NT106_BattleshipServer.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<LastOnlineFilter>();

builder.Services.AddControllers(options =>
{
    options.Filters.AddService<LastOnlineFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.MapHub<RoomHub>("/roomHub");
app.MapHub<XepTauHub>("/xepTauHub");
app.MapHub<ChatHub>("/chatHub");
app.MapHub<TranDauHub>("/tranDauHub");
app.MapHub<BattleRankingHub>("/battleRankingHub");
app.MapHub<FriendHub>("/friendHub");
app.MapHub<InviteHub>("/inviteHub");

app.Run();
