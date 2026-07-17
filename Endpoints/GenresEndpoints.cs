using GameStore.Api.Data;
using GameStore.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    public static void MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");

        group.MapGet(
            "/",
            async (GameStoreContext dbContext) =>
                await dbContext
                    .Genres.Select(g => new GenreDto(g.Id, g.Name))
                    .AsNoTracking()
                    .ToListAsync()
        );

        group.MapPost(
            "/",
            async (GenreDto genreDto, GameStoreContext dbContext) =>
            {
                Genre genre = new() { Name = genreDto.Name };
                dbContext.Genres.Add(genre);
                await dbContext.SaveChangesAsync();

                return Results.Created($"/genres/{genre.Id}", genreDto);
            }
        );
    }
}
