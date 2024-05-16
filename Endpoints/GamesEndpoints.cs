using GameStore.Data;
using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints;
public static class GamesEndpoints {

    const string GetGameRouteName = "GetGame";
    public static RouteGroupBuilder RegisterGamesEndpoints(this WebApplication app) {
        var group = app.MapGroup("/games").WithParameterValidation();

        // GET /games   
        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Games
                .Include(game => game.Genre)
                .Select(game=>game.ToSummaryDto())
                .AsNoTracking()
                .ToListAsync()
        );

        // GET /games/1
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) => {
            Game? game = await dbContext.Games.FindAsync(id);
            if (game is null) return Results.NotFound();
            return Results.Ok(game.ToDetailsDto());
        }).WithName(GetGameRouteName);

        // POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) => {
            Game game = newGame.ToEntity();
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            return Results.CreatedAtRoute(GetGameRouteName, new { game.id }, game.ToDetailsDto());
        });

        // PUT /games/1
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) => {
            var existingGame = await dbContext.Games.FindAsync(id);
            if (existingGame is null) {
                return Results.NotFound();
            }
            dbContext.Entry(existingGame).CurrentValues.SetValues(updatedGame.ToEntity(id));
            await dbContext.SaveChangesAsync();
            return Results.Ok(updatedGame);
        });

        // DELETE /games/1
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) => {
            await dbContext.Games.Where(game => game.id == id).ExecuteDeleteAsync();
            return Results.NoContent();
        });

        return group;
    }
}
