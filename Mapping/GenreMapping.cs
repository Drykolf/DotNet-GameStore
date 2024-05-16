using GameStore.Dtos;
using GameStore.Entities;

namespace GameStore.Mapping;

public static class GenreMapping {
    public static GenreDto ToDto(this Genre genre) {
        return new(genre.id, genre.Name);
    }
}
