namespace GameStore.Dtos {
    public record class GameDetails(
        int Id,
        string Name,
        int GenreId,
        decimal Price,
        DateOnly ReleaseDate);
}
