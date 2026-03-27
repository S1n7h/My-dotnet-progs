using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GamesEndpoints
{

    const string GetGameEndPointName = "GetGame";

    public static void MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/games");
        //GET GAMES
        // app.MapGet("/games", () => "Hello World! hoohaa meowmeow");
        // group.MapGet("/", () => games);
        group.MapGet("/", async (GameStoreContext dbContext)
            => await dbContext.Games
                                .Include(game => game.Genre)
                                .Select(game => new GameSummaryDto(
                                    game.Id,
                                    game.Name,
                                    //the exclamation mark tells the compiler to ignore
                                    //the ? modifier which Genre has because we know for sure that
                                    //when we are requesting entity framework to send us
                                    //the info for this game, we are using .Include() to ensure that
                                    //we get Genre along with it
                                    game.Genre!.Name,
                                    game.Price,
                                    game.ReleaseDate
                                ))
                                //entity core framework usually keeps a track of the results that
                                //are coming back so that, if you want to change anything present
                                //in the database, you can use those results, but in this case, we are
                                //only using it to know what is stored, and we are not manipulating the
                                //data in any way, so, we don't need entity core framework to track it 
                                //hence, we use the following command to turn off the ef core tracker 
                                .AsNoTracking()
                                //we convert all the info above into a task<List<GameDto>> in this case
                                .ToListAsync());

        //to make sure that whatever we are trying to find exists in the rest api, cus if not, the
        //instead of returning null, return 404 not found
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);

            if (game is null) return Results.NotFound();

            return Results.Ok(new GameDetailsDto(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            ));
        })
        .WithName(GetGameEndPointName);

        //POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };

            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            //the async part in the group.MapPost says that the current block of code
            //is supposed to run asynchronously except for the savechanges part which
            //has to be executed before the rest of the code present in the block
            //is executed
            
            //  dbContext.SaveChangesAsync();
            //because of the async in play, it's possible that the code ends up return results
            //without having saved using dbContext.SaveChangesAsync();, so,
            //return Results.CreatedAtRoute is done before the save occurs.

            // dbContext.SaveChangesAsync().ContinueWith(task =>
            // {
            //     //continue with logic
            // });
            //in this scenario, you can wait for the specific task you want to be completed,
            //if you want it to be completed AFTER the save changes occurs, by adding whatever logic
            //you want to add into the block right after save changes 

            
            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndPointName, new {id = gameDto.Id}, gameDto);
        });
        
        //PUT /games/1
        group.MapPut("/{id}", async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            //var index = games.FindIndex(game => game.Id == id);
            var existingGame = await dbContext.Games.FindAsync(id);

            if (existingGame is null)    return Results.NotFound();

            // games[existingGame] = new GameSummaryDto(
            //     id,
            //     updatedGame.Name,
            //     updatedGame.Genre,
            //     updatedGame.Price,
            //     updatedGame.ReleaseDate
            // );

            existingGame.Name = updatedGame.Name;
            existingGame.GenreId = updatedGame.GenreId;
            existingGame.ReleaseDate = updatedGame.ReleaseDate;
            existingGame.Price = updatedGame.Price;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            //games.RemoveAll(game => game.Id == id);

            await dbContext.Games
                            .Where(game => game.Id == id)
                            .ExecuteDeleteAsync();

            return Results.NoContent();
        });
    }
}