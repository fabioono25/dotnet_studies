using CommandsService.Data;
using CommandsService.EventProcessing;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMen"));
builder.Services.AddScoped<ICommandRepo, CommandRepo>();

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

// builder.Services.AddHostedService<MessageBusSubscriber>();

builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// builder.Services.AddScoped<IPlatformDataClient, PlatformDataClient>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
