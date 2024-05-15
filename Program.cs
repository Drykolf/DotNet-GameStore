using GameStore.Data;
using GameStore.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connectionString);

var app = builder.Build();

app.RegisterGamesEndpoints();
app.MigrateDb();
app.Run();
