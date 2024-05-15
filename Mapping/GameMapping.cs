using GameStore.Dtos;
using GameStore.Entities;

namespace GameStore.Mapping;

public static class GameMapping {
    public static GameSummaryDto ToSummaryDto(this Game game) {
        return new(game.id, game.Name, game.Genre!.Name, game.Price, game.ReleaseDate);
    }
    public static GameDetails ToDetailsDto(this Game game) {
        return new(game.id, game.Name, game.GenreId, game.Price, game.ReleaseDate);
    }
    public static Game ToEntity(this CreateGameDto game) {
        return new() {
            Name = game.Name,
            GenreId = game.GenreId,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }
    public static Game ToEntity(this UpdateGameDto game, int id) {
        return new Game() {
            id = id,
            Name = game.Name,
            GenreId = game.GenreId,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate
        };
    }
}
