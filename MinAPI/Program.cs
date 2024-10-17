using System.Net;
using System.Threading.RateLimiting;
using AutoMapper;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MinAPI;
using MinAPI.data;
using MinAPI.Dtos;
using MinAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConnBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("SqlDbConnection"));
sqlConnBuilder.UserID = builder.Configuration["UserId"];
sqlConnBuilder.Password = builder.Configuration["Password"];

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(sqlConnBuilder.ConnectionString));
builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Adding a rate limiting middleware. Advantages:
// 1. Prevents abuse of the API
// 2. Prevents DDoS attacks
// 3. Prevents brute force attacks
// 4. Prevents web scraping
builder.Services.AddRateLimiter(_ =>
{
    _.AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 5;
        options.Window = TimeSpan.FromSeconds(15);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 2;
    });
});

// adding request timeout middleware. Advantages:
// 1. Prevents slow client connections from holding up server resources
builder.Services.AddRequestTimeouts(option =>
 {
     option.DefaultPolicy = new RequestTimeoutPolicy { Timeout = TimeSpan.FromSeconds(5) };
     option.AddPolicy("ShortTimeoutPolicy", TimeSpan.FromSeconds(2));
     option.AddPolicy("LongTimeoutPolicy", TimeSpan.FromSeconds(10));
 });

var app = builder.Build();

// This example is to enable the short-circuit middleware.
// The short-circuit middleware should be placed before other middleware components.
// advantage: Prevents the request from reaching the next middleware component.
//app.MapGet("robots.txt", () => Results.Content("User-agent: *\nDisallow: /", "text/plain")).ShortCircuit();
app.MapShortCircuit((int)HttpStatusCode.NotFound, "robots.txt", "favicon.ico");

// This is an example of custom middleware.
app.UseCorrelationId();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiter();
app.UseRequestTimeouts();

app.MapGet("api/v1/test", (HttpContext context) =>
{
    // get the X-Correlation-Id header value
    var correlationId = context.Request.Headers["X-Correlation-Id"];

    return Results.Ok("Hello World. CorrelationId: " + correlationId);
}).RequireRateLimiting("fixed");

app.MapGet("api/v1/request-timeout", async (HttpContext context, ILogger<Program> logger) =>
{
    var random = new Random();
    var delay = random.Next(1, 10);
    logger.LogInformation($"Delaying for {delay} seconds");
    try
    {
        await Task.Delay(TimeSpan.FromSeconds(delay), context.RequestAborted);
    }
    catch
    {
        logger.LogWarning("The request timed out");
        return Results.Content("The request timed out", "text/plain");
    }

    return Results.Content($"Hello! The task is complete in {delay} seconds", "text/plain");
}).WithRequestTimeout(TimeSpan.FromSeconds(3));

app.MapGet("api/v1/commands", async (ICommandRepo repo, IMapper mapper) =>
{
    var commands = await repo.GetAllCommands();

    return Results.Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
});

app.MapGet("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, int id) =>
{
    var command = await repo.GetCommandById(id);

    return Results.Ok(mapper.Map<CommandReadDto>(command));
});

app.MapPost("api/v1/commands", async (ICommandRepo repo, IMapper mapper, CommandCreateDto cmdCreateDto) =>
{
    var commandModel = mapper.Map<Command>(cmdCreateDto);

    await repo.CreateCommand(commandModel);
    await repo.SaveChanges();

    var cmdReadDto = mapper.Map<CommandReadDto>(commandModel);

    return Results.Created($"api/v1/commands/{cmdReadDto.Id}", cmdReadDto);

});

app.MapPut("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, int id, CommandUpdateDto cmdUpdateDto) =>
{
    var command = await repo.GetCommandById(id);
    if (command == null)
    {
        return Results.NotFound();
    }

    mapper.Map(cmdUpdateDto, command);

    await repo.SaveChanges();

    return Results.NoContent();
});

app.MapDelete("api/v1/commands/{id}", async (ICommandRepo repo, IMapper mapper, int id) =>
{
    var command = await repo.GetCommandById(id);
    if (command == null)
    {
        return Results.NotFound();
    }

    repo.DeleteCommand(command);

    await repo.SaveChanges();

    return Results.NoContent();

});

app.Run();
