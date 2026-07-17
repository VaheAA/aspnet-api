using Microsoft.EntityFrameworkCore;
using rest.Data;
using rest.Dtos;
using rest.Models;

namespace rest.Endpoints;

public static class GamesEndpoints
{
    private const string GetGameEndpoint = "GetGame";

    private static readonly List<GameDto> games =
    [
        new GameDto(1, "Street Fighter", "Fighting", 10.99M, DateOnly.FromDateTime(DateTime.Now)),
        new GameDto(2, "Syphon Filter", "Action", 15.99M, DateOnly.FromDateTime(DateTime.Now)),
        new GameDto(3, "Gran Turismo", "Racing", 39.99M, DateOnly.FromDateTime(DateTime.Now)),
    ];

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        group.MapGet(
            "/",
            async (GameStoreContext dbContext) =>
                await dbContext
                    .Games.Include(g => g.Genre)
                    .Select(g => new GameDto(g.Id, g.Name, g.Genre!.Name, g.Price, g.ReleaseDate))
                    .ToListAsync()
        );

        group
            .MapGet(
                "/{id}",
                async (int id, GameStoreContext dbContext) =>
                {
                    var game = await dbContext.Games.FindAsync(id);

                    return game is null
                        ? Results.NotFound()
                        : Results.Ok(
                            new GameDetailsDto(
                                game.Id,
                                game.Name,
                                game.GenreId,
                                game.Price,
                                game.ReleaseDate
                            )
                        );
                }
            )
            .WithName(GetGameEndpoint);

        group.MapPost(
            "/",
            async (CreateGameDto newGame, GameStoreContext dbContext) =>
            {
                Game game = new()
                {
                    Name = newGame.Name,
                    GenreId = newGame.GenreId,
                    Price = newGame.Price,
                    ReleaseDate = newGame.ReleaseDate,
                };

                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                GameDetailsDto gameDto = new(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                );

                return Results.CreatedAtRoute(GetGameEndpoint, new { id = gameDto.Id }, gameDto);
            }
        );

        group.MapPut(
            "/{id}",
            (int id, UpdateGameDto updatedGame) =>
            {
                var index = games.FindIndex(g => g.Id == id);

                if (index == -1)
                {
                    return Results.NotFound();
                }

                games[index] = new GameDto(
                    id,
                    updatedGame.Name,
                    updatedGame.Genre,
                    updatedGame.Price,
                    updatedGame.ReleaseDate
                );

                return Results.NoContent();
            }
        );

        group.MapDelete(
            "/{id}",
            (int id) =>
            {
                games.RemoveAll(g => g.Id == id);

                return Results.NoContent();
            }
        );
    }
}
