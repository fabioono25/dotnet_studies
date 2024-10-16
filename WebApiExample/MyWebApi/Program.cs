using System.Text.RegularExpressions;
using MyWebApi;
using MyWebApi.Services;
using Serilog;
using Serilog.Formatting.Json;

var builder = WebApplication.CreateBuilder(args);

// Working with logs
builder.Logging.ClearProviders();

// builder.Logging.AddConsole();
// builder.Logging.AddDebug();
// builder.Logging.AddEventLog(); // Window event log

// logging to a file, using Serilog
var logger = new LoggerConfiguration()
    .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs/log.txt"), rollingInterval: RollingInterval.Day, retainedFileCountLimit: 90)
    .WriteTo.Console(new JsonFormatter())
    .CreateLogger();
builder.Logging.AddSerilog(logger);

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

builder.Services.AddRouting(options => options.LowercaseUrls = true); // only affects URL generation, not routing
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the DatabaseOption class as a configuration object.
// This line must be added before the `builder.Build()` method.
// builder.Services.Configure<DatabaseOption>(builder.Configuration.GetSection(DatabaseOption.SectionName));

// Register the DatabaseOptions class as a configuration object.
// builder.Services.Configure<DatabaseOptions>(DatabaseOptions.SystemDatabaseSectionName, builder.Configuration.GetSection($"{DatabaseOptions.SectionName}:{DatabaseOptions.SystemDatabaseSectionName}"));
// builder.Services.Configure<DatabaseOptions>(DatabaseOptions.BusinessDatabaseSectionName, builder.Configuration.GetSection($"{DatabaseOptions.SectionName}:{DatabaseOptions.BusinessDatabaseSectionName}"));

// group registration
builder.Services.AddConfig(builder.Configuration);

var app = builder.Build();

// read the environment variable ASPNETCORE_ENVIRONMENT
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
Console.WriteLine($"ASPNETCORE_ENVIRONMENT: {env}");

// Resolving a service inside app (when it starts)
using var serviceScope = app.Services.CreateScope();
var services = serviceScope.ServiceProvider;
var demoService = services.GetRequiredService<IDemoService>();
var message = demoService.SayHello();
Console.WriteLine(message);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
