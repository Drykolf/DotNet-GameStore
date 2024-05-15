using Microsoft.EntityFrameworkCore;

namespace GameStore.Data;

public static class DataExtensions {
    public static void MigrateDb(this WebApplication app) {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<GameStoreContext>();
        context.Database.Migrate();
    }
}
