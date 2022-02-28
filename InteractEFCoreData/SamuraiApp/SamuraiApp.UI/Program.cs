// See https://aka.ms/new-console-template for more information

using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

SamuraiContext _context = new SamuraiContext();
SamuraiContext _contextNT = new SamuraiContextNoTracking();

await StartProgram();

async Task StartProgram()
{
    #region Add&Read
    //await AddSamurai("Julie", "Sampson");
    //await GetSamurais();
    #endregion


    #region Add bulk operations & Read
    //await AddSamurai("Shimada", "Okamoto", "Kikuchio", "Hayashida");
    //await GetSamurais();
    #endregion

    #region Batching can combine types and operations
    //await AddVariousTypes();
    #endregion

    #region Filtering in Queries
    //await QueryFilters();
    #endregion

    #region Aggregates in Queries
    //await QueryAggregates();
    #endregion

    #region UpdateSimpleObjects
    //await RetrieveAndUpdateSamurai();
    //await RetrieveAndUpdateMultipleSamurais();
    #endregion

    #region  DeletSampleObjects
    //await RetrieveAndDeleteSamurai();
    #endregion

    #region Persisting Data - Disconnected scenario

    //QueryAndUpdateBattlesDisconnected();
    #endregion

    #region NoTracking
    //await QueryFiltersNoTracking();
    await RetrieveAndUpdateMultipleSamuraisMixed();
    #endregion

    Console.Write("Press any key...");
    Console.ReadKey();
}

async Task RetrieveAndUpdateMultipleSamuraisMixed()
{
    //Does not track.
    //var samurais = await _contextNT.Samurais.Skip(1).Take(4).ToListAsync();
    //Tracks
    var samurais = await _contextNT.Samurais.AsTracking().Skip(1).Take(4).ToListAsync();
    samurais.ForEach(s => s.Name += "ReMixed");
    await _contextNT.SaveChangesAsync();
}

async Task QueryFiltersNoTracking()
{
    var samurais = await _contextNT.Samurais
        .Where(s => EF.Functions.Like(s.Name, "J%")).ToListAsync();

    //var name = "Sampson";
    //var samurais = await _context.Samurais.Where(s => s.Name == name).ToListAsync();

    //var samurais = await _context.Samurais.Where(s => s.Name == "Sampson").ToListAsync();
    //Uses parameter in SQL queries
}

async Task RetrieveAndDeleteSamurai()
{
    var samurai = await _context.Samurais.FindAsync(2);
    _context.Samurais.Remove(samurai);
    await _context.SaveChangesAsync();
}

async Task RetrieveAndUpdateMultipleSamurais()
{
    var samurais = await _context.Samurais.Skip(1).Take(4).ToListAsync();
    samurais.ForEach(s => s.Name += "San");
    await _context.SaveChangesAsync();
}

async Task RetrieveAndUpdateSamurai()
{
    //Samurai entity was modified. Name was edited. New Name is saved.
    var samurai = await _context.Samurais.FirstOrDefaultAsync();
    samurai.Name += "san";
    await _context.SaveChangesAsync();
}

async Task QueryAggregates()
{
    var name = "Sampson";
    //var samurai = await _context.Samurais.Where(s => s.Name == name).FirstOrDefaultAsync();

    //Generates same SQL query as above.
    //var samurai = await _context.Samurais.FirstOrDefaultAsync(s => s.Name == name);

    //Retrieve the key.
    //var samurai = await _context.Samurais.FirstOrDefaultAsync(s => s.Id == 2);

    //Not a LINQ method. It is a DbSet method.
    //Executes immediately
    //If a key is found in the change tracker, avoids unneeded database query.
    var samurai = await _context.Samurais.FindAsync(2);

}

async Task QueryFilters()
{
    var samurais = await  _context.Samurais
        .Where(s => EF.Functions.Like(s.Name, "J%")).ToListAsync();
    
    //var name = "Sampson";
    //var samurais = await _context.Samurais.Where(s => s.Name == name).ToListAsync();

    //var samurais = await _context.Samurais.Where(s => s.Name == "Sampson").ToListAsync();
    //Uses parameter in SQL queries
}

async Task AddVariousTypes()
{
    await _context.AddRangeAsync(new Samurai {Name = "Shimada"},
        new Samurai { Name = "Okamoto" },
        new Battle { Name = "Battle of Anegwada" },
        new Battle { Name = "Battle of Nagashino" });

    //await _context.Samurais.AddRangeAsync(
    //    new Samurai {Name = "Shimada"},
    //                 new Samurai {Name = "Okamoto"});

    //await _context.Battles.AddRangeAsync(
    //    new Battle { Name = "Battle of Anegwada" },
    //                 new Battle { Name = "Battle of Nagashino" });

    await _context.SaveChangesAsync();
}

async Task AddSamuraisByName(params string[] names)
{
    foreach (var name in names)
    {
        //DBContext "tracks" or "change tracks" entities.
        //Uses EntityEntry in DBContext under the covers.
        await _context.Samurais.AddAsync(new Samurai { Name = name });
    }

    //Checks the EntityEntry -> State
    //In case of failure, Save changes will rollback if needed.
    await _context.SaveChangesAsync();
}

async Task  AddSamurai(params string[] names)
{
    foreach (var name in names)
    {
        //DBContext "tracks" or "change tracks" entities.
        //Uses EntityEntry in DBContext under the covers.
        await _context.Samurais.AddAsync(new Samurai {Name = name});
    }
    
    //Checks the EntityEntry -> State
    //In case of failure, Save changes will rollback if needed.
    await _context.SaveChangesAsync();
}

async Task GetSamurais()
{
    //Query tags will add a comment to the generated SQL.
    var samurais = await _context.Samurais
        .TagWith("ConsoleAPP.Program.GetSamurais method")
        .ToListAsync();
    

    foreach (var samurai in samurais)
    {
        Console.WriteLine(samurai.Name);
    }
}

void QueryAndUpdateBattlesDisconnected()
{
    List<Battle> disconnectedBattles;

    using (var context1 = new SamuraiContext())
    {
        disconnectedBattles = context1.Battles.ToListAsync().GetAwaiter().GetResult();
    }//context 1 is disposed

    disconnectedBattles.ForEach(b =>
    {
        b.StartDate = new DateTime(1570, 01, 01);
        b.EndDate = new DateTime(1570, 12, 1);
    });
    //The new Samurai Context has no tracking information.
    using (var context2 = new SamuraiContext())
    {
        //Update causes all properties to be updated whether they were edited or not.
        context2.UpdateRange(disconnectedBattles);
        context2.SaveChanges();

    }
}

