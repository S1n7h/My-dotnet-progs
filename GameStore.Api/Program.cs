using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Endpoints;
using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddValidation();
builder.AddGameStoreDb();

var app = builder.Build();
app.UseCors("AllowFrontend");

app.MapGamesEndpoints();
app.MapGenresEndpoints();

app.MigrateDb();

app.Run();
