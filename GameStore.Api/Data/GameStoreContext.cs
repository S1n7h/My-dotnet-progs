using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    //Games is a property which points directly into a set of game
    //This DbSet is an object that can be used for both query and set instances of game in this case
    //and any link queries that you send to this games object, is going to be translated into qeueries 
    //against the database 
    public DbSet<Game> Games => Set<Game>();

    public DbSet<Genre> Genres => Set<Genre>();
    
}
