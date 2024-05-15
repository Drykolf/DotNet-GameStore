using GameStore.Data;
using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Mapping;

namespace GameStore.Endpoints;
public static class GamesEndpoints {

    const string GetGameRouteName = "GetGame";
    private static readonly List<GameDto> games = new() {
    new GameDto(1, "The Witcher 3", "RPG", 29.99m, new DateOnly(2015, 5, 19)),
    new GameDto(2, "Cyberpunk 2077", "RPG", 59.99m, new DateOnly(2020, 12, 10)),
    new GameDto(3, "Doom Eternal", "FPS", 49.99m, new DateOnly(2020, 3, 20))
    };

    public static RouteGroupBuilder RegisterGamesEndpoints(this WebApplication app) {
        var group = app.MapGroup("/games").WithParameterValidation();

        // GET /games   
        group.MapGet("/", () => games);

        // GET /games/1
        group.MapGet("/{id}", (int id) => games.FirstOrDefault(g => g.Id == id) is GameDto game ? Results.Ok(game) : Results.NotFound())
            .WithName(GetGameRouteName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) => {
            Game game = newGame.ToEntity();
            game.Genre = dbContext.Genres.Find(newGame.GenreId);
            dbContext.Games.Add(game);
            dbContext.SaveChanges();
            return Results.CreatedAtRoute(GetGameRouteName, new { id = game.id }, game.ToDto());
        });

        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto game) => {
            var existingGame = games.FirstOrDefault(g => g.Id == id);
            if (existingGame is null) {
                return Results.NotFound();
            }

            var updatedGame = existingGame with {
                Name = game.Name,
                Genre = game.Genre,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate
            };

            games[games.IndexOf(existingGame)] = updatedGame;
            return Results.Ok(updatedGame);
        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id) => {
            var existingGame = games.FirstOrDefault(g => g.Id == id);
            if (existingGame is null) {
                return Results.NotFound();
            }

            games.Remove(existingGame);
            return Results.NoContent();
        });

        return group;
    }
}
