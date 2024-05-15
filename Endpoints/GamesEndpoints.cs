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
        group.MapGet("/", (GameStoreContext dbContext) =>
            dbContext.Games
                .Include(game => game.Genre)
                .Select(game=>game.ToSummaryDto())
                .AsNoTracking()
        );

        // GET /games/1
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) => {
            Game? game = dbContext.Games.Find(id);
            if (game is null) return Results.NotFound();
            return Results.Ok(game.ToDetailsDto());
        }).WithName(GetGameRouteName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) => {
            Game game = newGame.ToEntity();
            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            return Results.CreatedAtRoute(GetGameRouteName, new { id = game.id }, game.ToSummaryDto());
        });

        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) => {
            var existingGame = dbContext.Games.Find(id);
            if (existingGame is null) {
                return Results.NotFound();
            }
            dbContext.Entry(existingGame).CurrentValues.SetValues(updatedGame.ToEntity(id));
            dbContext.SaveChanges();
            return Results.Ok(updatedGame);
        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id, GameStoreContext dbContext) => {
            dbContext.Games.Where(game => game.id == id).ExecuteDelete();
            return Results.NoContent();
        });

        return group;
    }
}
