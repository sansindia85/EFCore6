// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;

SamuraiContext _context = new SamuraiContext();

await StartProgram();

async Task StartProgram()
{
    _context.Database.EnsureCreated();
    await GetSamurais("Before Add:");
    await AddSamurai();
    await GetSamurais("After Add:");
    Console.WriteLine("After Add:");
    Console.Write("Press any key...");
    Console.ReadKey();
}

async Task  AddSamurai()
{
    var samurai = new Samurai {Name = "Julie"};
    await _context.Samurais.AddAsync(samurai);
    await _context.SaveChangesAsync();
}

async Task GetSamurais(string text)
{
    var samurais = await _context.Samurais.ToListAsync();
    Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
    foreach (var samurai in samurais)
    {
        Console.WriteLine(samurai.Name);
    }
}