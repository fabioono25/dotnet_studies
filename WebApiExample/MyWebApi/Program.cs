using MyWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IPostService, PostsService>();
builder.Services.AddSingleton<IDemoService, DemoService>();

// builder.Services.AddScoped<IScopedService, ScopedService>();
// builder.Services.AddTransient<ITransientService, TransientService>();
// builder.Services.AddSingleton<ISingletonService, SingletonService>();

builder.Services.AddKeyedScoped<IDataService, SqlDatabaseService>("sqlDatabaseService");
builder.Services.AddKeyedScoped<IDataService, CosmosDatabaseService>("cosmosDatabaseService");

// Group registration
builder.Services.AddLifetimeServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Resolving a service inside app (when it starts)
using var serviceScope = app.Services.CreateScope();
var services = serviceScope.ServiceProvider;
var demoService = services.GetRequiredService<IDemoService>();
var message = demoService.SayHello();
Console.WriteLine(message);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
