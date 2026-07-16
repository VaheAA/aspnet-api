using Microsoft.EntityFrameworkCore;
using rest.Data;
using rest.Endpoints;
using rest.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();
builder.AddGameStoreDb();

var app = builder.Build();

app.MapGamesEndpoints();
app.MigrateDb();

app.Run();