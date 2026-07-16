using System.ComponentModel.DataAnnotations;

namespace rest.Dtos;

public record UpdateGameDto(
    [Required] [StringLength(50)] string Name,
    [Required] [StringLength(20)] string Genre,
    [Required] [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
);
