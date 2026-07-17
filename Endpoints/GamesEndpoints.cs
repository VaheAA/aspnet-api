using Microsoft.EntityFrameworkCore;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{
    private const string GetGameEndpoint = "GetGame";

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");

        group.MapGet(
            "/",
            async (GameStoreContext dbContext) =>
                await dbContext
                    .Games.Include(g => g.Genre)
                    .Select(g => new GameSummaryDto(
                        g.Id,
                        g.Name,
                        g.Genre!.Name,
                        g.Price,
                        g.ReleaseDate
                    ))
                    .AsNoTracking()
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
            async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
            {
                var existingGame = await dbContext.Games.FindAsync(id);

                if (existingGame is null)
                {
                    return Results.NotFound();
                }

                existingGame.Name = updatedGame.Name;
                existingGame.GenreId = updatedGame.GenreId;
                existingGame.Price = updatedGame.Price;
                existingGame.ReleaseDate = updatedGame.ReleaseDate;

                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }
        );

        group.MapDelete(
            "/{id}",
            async (int id, GameStoreContext dbContext) =>
            {
                var game = await dbContext.Games.FindAsync(id);

                if (game == null)
                {
                    return Results.NotFound();
                }

                dbContext.Games.Remove(game);
                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }
        );
    }
}
