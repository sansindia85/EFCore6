// See https://aka.ms/new-console-template for more information

using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using SamuraiApp.UI;

SamuraiContext _context = new SamuraiContext();
SamuraiContext _contextNT = new SamuraiContextNoTracking();

await StartProgram();

async Task StartProgram()
{
    #region Type1 : Just interacting with EF Core data

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
    //await RetrieveAndUpdateMultipleSamuraisMixed();
    #endregion

    #endregion

    #region Type2 : Interacting with related data

    #region Insert Related Data
    //await InsertNewSamuraiWithAQuote();
    //await InsertNewSamuraiWithManyQuotes();
    //await AddQuoteToExistingSamuraiWhileTracked();
    //await AddQuoteToExistingSamuraiNotTracked(1);
    //await AddQuoteToExistingSamuraiNotTrackedImproved(5);
    //await SimplerAddQuoteToExistingSamuraiNotTracked(4);
    //await SimplerAddQuoteToExistingSamuraiNotTracked(6);
    #endregion

    #region Eager Loading related data.
    //await EagerLoadingWithSamuraiQuotes();
    #endregion

    #region Project related data

    //await ProjectSomeProperties();
    //await ProjectSamuraisWithQuotes();

    #endregion

    #region Explicit loading
    //await ExplicitLoadQuotes();
    //await ExplicitLoadingFilter();
    #endregion

    #region Filtering related data
    //await FilteringRelatedData();
    //await ModifyingRelatedDataWhenTracked();
    await ModifyingRelatedDataWhenNotTracked();
    #endregion

    #region Working with many to many relationships

    //await AddingNewSamuraiToAnExistingBattle();
    //await ReturnBattleWithSamurais();
    //await ReturnAllBattleWithSamurais();
    //await AddAllSamuraisToAllBattles();
    #endregion

    #region Altering or removing many to many relationships
    //await RemoveSamuraiFromABattle();
    //await WillNotRemoveSamuraiFromABattle();
    #endregion

    #region  Removing Many to Many - BetterApproach
    //await RemoveSamuraiFromABattleExplict();
    #endregion

    #region Data in one-to one relationship
    //await AddNewSamuraiWithHorse();
    //await AddNewHorseToSamuraiUsingId();
    //await AddNewHorseToSamuraiObject();
    //await ReplaceAHorse();
    #endregion

    #region Query one to one relationship

    await GetHorsesWithSamurai();

    #endregion

    #endregion

    Console.Write("Press any key...");
    Console.ReadKey();
}

async Task GetHorsesWithSamurai()
{
    var horseOnly = _context.Set<Horse>().FindAsync(3);

    var horseWithSamurai = _context.Samurais.Include(s => s.Horse)
        .FirstOrDefaultAsync(s => s.Horse.Id == 3);

    var horseSamuraiWithParis = _context.Samurais
        .Where((s => s.Horse != null))
        .Select(s => new {Horse = s.Horse, Samurai = s})
        .ToListAsync();
}

async Task ReplaceAHorse()
{
    var samurai = await _context.Samurais.Include(s => s.Horse)
        .FirstOrDefaultAsync(s => s.Id == 5);

    samurai.Horse = new Horse {Name = "Trigger"};
    await _context.SaveChangesAsync();
}

async Task AddNewHorseToSamuraiObject()
{
    var samurai = await _context.Samurais.FindAsync(12);
    samurai.Horse = new Horse {Name = "Black Beauty"};
    await _context.SaveChangesAsync();
}
async Task AddNewHorseToSamuraiUsingId()
{
    var horse = new Horse() {Name = "Scout", SamuraiId = 3};
    await _context.AddAsync(horse);
    await _context.SaveChangesAsync();
}

async Task AddNewSamuraiWithHorse()
{
    var samurai = new Samurai {Name = "Jina Ujicjika"};
    samurai.Horse = new Horse {Name = "Silver"};
    await _context.Samurais.AddAsync(samurai);
    await _context.SaveChangesAsync();
}

async Task RemoveSamuraiFromABattleExplict()
{
    //Just removes the data from Join table only
    var battleSamurai = await  _context.Set<BattleSamurai>()
        .SingleOrDefaultAsync(bs => bs.BattleId == 1 && bs.SamuraiId == 63);

    if (battleSamurai != null)
    {
        _context.RemoveRange(battleSamurai); //_context.Set<BattleSamurai>(). Remove works, too.
        await _context.SaveChangesAsync();
    }
}

async Task WillNotRemoveSamuraiFromABattle()
{
    var battle = await _context.Battles.FindAsync(1);
    var samurai = await _context.Samurais.FindAsync(63);
    battle.Samurais.Remove(samurai);
    await _context.SaveChangesAsync(); //The relationship is not being tracked.
}

async Task RemoveSamuraiFromABattle()
{
    var battleWithSamurai = await _context.Battles
        .Include(b => b.Samurais.Where(s => s.Id == 63))
        .SingleAsync(s => s.BattleId == 1);

    var samurai = battleWithSamurai.Samurais[0];
    battleWithSamurai.Samurais.Remove(samurai);
    await _context.SaveChangesAsync();
}

async Task AddAllSamuraisToAllBattles()
{
    var allBattles = await _context.Battles.Include(b=>b.Samurais).ToListAsync();
    //Workaround as it is already in database
    //var allSamurais = await _context.Samurais.Where(s => s.Id!=63).ToListAsync();
    //If there is a lot of data in eager load, this could hurt performance. Seek another solution.
    var allSamurais = await _context.Samurais.ToListAsync();

    foreach (var battle in allBattles)
    {
        battle.Samurais.AddRange(allSamurais);
    }

    await _context.SaveChangesAsync();
}

async Task ReturnAllBattleWithSamurais()
{
    var battles = await _context.Battles.Include(b => b.Samurais).ToListAsync();
}

async Task ReturnBattleWithSamurais()
{
    var battle = await _context.Battles.Include(b => b.Samurais).FirstOrDefaultAsync();
}
async Task AddingNewSamuraiToAnExistingBattle()
{
    var battle = await _context.Battles.FirstOrDefaultAsync();
    battle.Samurais.Add(new Samurai { Name = "Takeda Shingen" });
    await _context.SaveChangesAsync();
}

async Task ModifyingRelatedDataWhenNotTracked()
{
    //DBCOntext has no clue about history of objects before they are attached.
    var samurai = await _context.Samurais.Include(s => s.Quotes)
                              .FirstOrDefaultAsync(s => s.Id == 62);

    var quote = samurai.Quotes[0];
    quote.Text += "Did you hear that again?";

    using var newContext = new SamuraiContext();
    //If there are two entries in the database then two updates will happened. The attach will also behave same.
    //newContext.Quotes.Update(quote); 
    //To overcome the above commented challenge
    newContext.Entry(quote).State = EntityState.Modified;

    await newContext.SaveChangesAsync();
}

async Task ModifyingRelatedDataWhenTracked()
{
    //DbContext is aware of all changes made to objects that is it tracking.
    var samurai = await _context.Samurais.Include(s => s.Quotes)
        .FirstOrDefaultAsync(s => s.Id == 3);

    samurai.Quotes[0].Text = "Did you hear that?";
    
    _context.Quotes.Remove(samurai.Quotes[0]);
    
    await _context.SaveChangesAsync();
}

async Task FilteringRelatedData()
{
    var samurais = await _context.Samurais.Where(s => s.Quotes.Any(q => q.Text.Contains("happy"))).ToListAsync();
}

async Task ExplicitLoadingFilter()
{
    var samurai = await _context.Samurais.FindAsync(3);
    var happyQuotes = await _context.Entry(samurai)
        .Collection(b => b.Quotes)
        .Query()
        .Where(q => q.Text.Contains("happy"))
        .ToListAsync();
}
async Task ExplicitLoadQuotes()
{
    //Make sure there's a horse in the DB, then clear the context's change tracker.
    _context.Set<Horse>().Add(new Horse { SamuraiId = 1, Name = "Mr.Ed" });
    await _context.SaveChangesAsync();
    _context.ChangeTracker.Clear();

    //---------------------
    var samurai = await _context.Samurais.FindAsync(1);
    await _context.Entry(samurai).Collection(s => s.Quotes).LoadAsync();
    await _context.Entry(samurai).Reference(s => s.Horse).LoadAsync();

}

async Task ProjectSamuraisWithQuotes()
{
    //Select 2 scalars and List<Quote> from Samurai type.
    //var somePropertiesWithQuotes = await _context.Samurais.Select(s => new {s.Id, s.Name, s.Quotes}).ToListAsync();

    //Selecting an aggregate of related data
    //var somePropertiesWithQuotes = await _context.Samurais.Select( s=> new{ s.Id, s.Name, NumberOfQuotes = s.Quotes.Count}).ToListAsync();

    var samuraisAndQuotes = await _context.Samurais
        .Select(s => new
        {
            s.Id, s.Name,
            HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
        }).ToListAsync();

}
async Task ProjectSomeProperties()
{
    //Projecting an undefined anonymous type. And return two types.
    //var someProperties = await _context.Samurais.Select(s => new {s.Id, s.Name}).ToListAsync();

    //Creating a list of defined types
    var idAndNames = await _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToListAsync();
}


async Task EagerLoadingWithSamuraiQuotes()
{

    //By default : Left join.
    //var samuraiQuotes = await _context.Samurais.Include(s => s.Quotes).ToListAsync();

    //SplitQuery : Query is broken up into multiple queries sent in a single command.
    //In some cases, multiple commands with separate queries may be faster.
    //var splitQuery = await _context.Samurais.AsSplitQuery().Include(s => s.Quotes).ToListAsync();

    //Does not work in NPGSQL.
    //var filteredInclude = await _context.Samurais.Include(s => s.Quotes.Where(q => q.Text.Contains("Thanks"))).ToListAsync();

    //Does not work in NPGSQL.
    //var filteredInclude = await _context.Samurais.Include(s => s.Quotes.Where(q => EF.Functions.Like(q.Text, "%Thanks%"))).ToListAsync();

    //This won't compile.Does not work in NPGSQL._context.Samurais.Find(10).Include does not work.
    var filterPrimaryEntityWithInclude = await _context.Samurais.Where(s => s.Name.Contains("Sampson"))
                                         .Include(s => s.Quotes).FirstOrDefaultAsync();
}

async Task SimplerAddQuoteToExistingSamuraiNotTracked(int samuraiId)
{
    var quote = new Quote() {Text = "Thanks for dinner!", SamuraiId = samuraiId};
    //C# 8 using declaration gets disposed when variable goes out of scope.
    using var newContext = new SamuraiContext();
    await newContext.Quotes.AddAsync(quote);
    await newContext.SaveChangesAsync();
}
async  Task AddQuoteToExistingSamuraiNotTrackedImproved(int samuraiId)
{
    var samurai = await _context.Samurais.FindAsync(samuraiId);

    samurai.Quotes.Add(new Quote
    {
        Text = "I bet you're happy that I've saved you!"
    });

    //As child's key value is not set, state will automatically be "Added".
    //Child's FK value to parent (eg. Quote.SamuraiId is set to parent's key.
    //Attach performs better than update.
    using (var newContext = new SamuraiContext())
    {
        newContext.Samurais.Attach(samurai);
        await newContext.SaveChangesAsync();
    }
}
async Task AddQuoteToExistingSamuraiNotTracked(int samuraiId)
{
    var samurai = await _context.Samurais.FindAsync(samuraiId);

    samurai.Quotes.Add(new Quote
    {
        Text = "I bet you're happy that I've saved you!"
    });

    //As child's key value is not set, state will automatically be "Added".
    //Child's FK value to parent (eg. Quote.SamuraiId is set to parent's key.
    using (var newContext = new SamuraiContext())
    {
        newContext.Samurais.Update(samurai);
        await newContext.SaveChangesAsync();
    }
}

async Task AddQuoteToExistingSamuraiWhileTracked()
{
    var samurai = _context.Samurais.FirstOrDefault();
    //The context is tracking the Samurai at the time the quote got added.
    samurai.Quotes.Add(new Quote
    {
        Text = "I bet you're happy that I've saved you!"
    });

    await _context.SaveChangesAsync();
}

async Task InsertNewSamuraiWithManyQuotes()
{
    var samurai = new Samurai()
    {
        Name = "Kyuzo",
        Quotes = new List<Quote>
        {
            new Quote() {Text = "Watch out for my sharp sword!"},
            new Quote() {Text = "I told you to watch out for the sharp sword! Oh well!"}
        }
    };

    await _context.Samurais.AddAsync(samurai);
    await _context.SaveChangesAsync();
}

async Task InsertNewSamuraiWithAQuote()
{
    var samurai = new Samurai()
    {
        Name = "Kambei Shimada",
        Quotes = new List<Quote>
        {
            new Quote() {Text = "I've come to save you."}
        }
    };

    await _context.Samurais.AddAsync(samurai);
    await _context.SaveChangesAsync();
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



