using Microsoft.EntityFrameworkCore;
using GameStore.Api.Data;
using GameStore.Api.Dtos;

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
    }
}
