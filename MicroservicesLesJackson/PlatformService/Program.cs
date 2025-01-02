using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;
using PlatformService.AsyncDataServices;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Console.WriteLine("--> Using InMem Db");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMem"));

if (builder.Environment.IsProduction())
{
    Console.WriteLine("--> Using SqlServer Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn")));
}
else
{
    Console.WriteLine("--> Using InMem Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseInMemoryDatabase("InMem"));
}


builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

Console.WriteLine($"--> CommandService Endpoint {builder.Configuration["CommandService"]}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Listen on all network interfaces
// without this, the app will only listen on localhost
// app.Urls.Add("http://*:5555");

PrepDb.PrepPopulation(app, app.Environment.IsProduction());

app.Run();
