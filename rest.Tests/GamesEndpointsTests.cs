using System.Net;
using System.Net.Http.Json;
using rest.Dtos;

namespace rest.Tests;

public class GamesEndpointsTests(GamesApiFactory factory) : IClassFixture<GamesApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetGames_ReturnsOk()
    {
        var response = await _client.GetAsync("/games");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetGame_UnknownId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/games/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateGame_ThenGetById_ReturnsSameGame()
    {
        var newGame = new CreateGameDto(
            "Chrono Trigger",
            GenreId: 4, // seeded as "RPG" in DataExtensions.AddGameStoreDb
            Price: 19.99M,
            ReleaseDate: new DateOnly(1995, 3, 11)
        );

        var createResponse = await _client.PostAsJsonAsync("/games", newGame);
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);

        var created = await createResponse.Content.ReadFromJsonAsync<GameDetailsDto>();
        Assert.NotNull(created);
        Assert.Equal(newGame.Name, created.Name);

        var getResponse = await _client.GetAsync(createResponse.Headers.Location);
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetched = await getResponse.Content.ReadFromJsonAsync<GameDetailsDto>();
        Assert.Equal(created.Id, fetched?.Id);
        Assert.Equal(newGame.Name, fetched?.Name);
    }
}
