using lab3.Data;
using lab3.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FleetDbContext>(options =>
    options.UseSqlite("Data Source=fleet.db"));

builder.Services.AddSingleton<FleetManager>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FleetDbContext>();
    db.Database.EnsureCreated();
    
    var fleetManager = scope.ServiceProvider.GetRequiredService<FleetManager>();
    fleetManager.OnNewOrderCreated += (message) => 
    {
        Console.WriteLine((object?)$"🔔 POWIADOMIENIE: {message}");
    };
}

app.Run();