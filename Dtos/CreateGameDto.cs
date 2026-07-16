using System.ComponentModel.DataAnnotations;

namespace rest.Dtos;

public record CreateGameDto(
    [Required] [StringLength(50)] string Name,
    [Range(1, 50)] int GenreId,
    [Required] [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
);
