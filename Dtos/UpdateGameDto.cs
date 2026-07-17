using System.ComponentModel.DataAnnotations;

namespace rest.Dtos;

public record UpdateGameDto(
    [Required] [StringLength(50)] string Name,
    [Range(1, 50)] int GenreId,
    [Required] [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
);
