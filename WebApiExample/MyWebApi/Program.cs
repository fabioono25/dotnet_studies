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

// Working with the middleware
app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation($"Request Host: {context.Request.Host}");
    logger.LogInformation("My Middleware - Before");
    await next(context);
    logger.LogInformation("My Middleware - After");
    logger.LogInformation($"Response StatusCode: {context.Response.StatusCode}");
}); // middleware

// This is an example showing how to combine multiple middleware components into a single middleware component.
// app.Map("/lottery", app =>
// {
//     var random = new Random();
//     var luckyNumber = random.Next(1, 6);
//     app.UseWhen(context => context.Request.QueryString.Value == $"?{luckyNumber.ToString()}", app =>
//     {
//         app.Run(async context =>
//         {
//             await context.Response.WriteAsync($"You win! You got the lucky number {luckyNumber}!");
//         });
//     });
//     app.UseWhen(context => string.IsNullOrWhiteSpace(context.Request.QueryString.Value), app =>
//     {
//         app.Use(async (context, next) =>
//         {
//             var number = random.Next(1, 6);
//             context.Request.Headers.TryAdd("number", number.ToString());
//             await next(context);
//         });
//         app.MapWhen(context => context.Request.Headers["number"] == luckyNumber.ToString(), app =>
//         {
//             app.Run(async context =>
//             {
//                 await context.Response.WriteAsync($"You win! You got the lucky number {luckyNumber}!");
//             });
//         });
//     });
//     app.Run(async context =>
//     {
//         var number = "";
//         if (context.Request.QueryString.HasValue)
//         {
//             number = context.Request.QueryString.Value?.Replace("?", "");
//         }
//         else
//         {
//             number = context.Request.Headers["number"];
//         }
//         await context.Response.WriteAsync($"Your number is {number}. Try again!");
//     });
// });
// app.Run(async context =>
// {
//     await context.Response.WriteAsync($"Use the /lottery URL to play. You can choose your number with the format /lottery?1.");
// });

// one more example of UseWhen
app.UseWhen(context => context.Request.Path.Equals("/weatherforecast"), app =>
{
    app.Use(async (context, next) =>
    {
        var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
        logger.LogInformation($"From MapWhen(): Branch used = {context.Request.Query["branch"]}");
        await next();
    });
    // app.Run(async context =>
    // {
    //     await context.Response.WriteAsync("Hello from the weather forecast! Middleware intercepted the request.");
    // });
});

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
