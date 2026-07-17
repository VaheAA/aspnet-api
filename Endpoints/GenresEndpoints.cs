using Microsoft.EntityFrameworkCore;
using rest.Data;
using rest.Dtos;

namespace rest.Endpoints;

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
    }
}
