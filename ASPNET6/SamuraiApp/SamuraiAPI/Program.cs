using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//This tells the app to instantiate a SamuraiContext as needed.
builder.Services.AddDbContext<SamuraiContext>(optionsBuilder =>
//The controller has no reliance on tracking and this setting means the context won't bother.
optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("SamuraiConnex"))
.EnableSensitiveDataLogging()
.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)); 


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
