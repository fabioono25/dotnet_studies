using EfCoreDemo.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// MAKING YOUR HTTP CLIENT RESILIENT
// builder.Services.AddHttpClient<InvoiceClient>(client =>
// {
//     client.BaseAddress = new Uri("https://localhost:5001/");
//     client.DefaultRequestHeaders.Add("Accept", "application/json");
// }).AddResilienceHandler("custom", pipeline =>
// {
//     // total request timeout
//     pipeline.AddTimeout(TimeSpan.FromSeconds(5)); // Timeout after 5 seconds

//     // retry
//     pipeline.AddRetry( // Retry 3 times with an exponential backoff strategy
//         new HttpRetryStrategyOptions
//         {
//             MaxRetryAttempts = 3,
//             BackoffType = DelayBackoffType.Exponential,
//             UseJitter = true, // Add jitter to the delay. What is it: it is a random value that is added to the delay to prevent the thundering herd problem.
//             Delay = TimeSpan.FromSeconds(2),
//         });

//     // circuit breaker
//     pipeline.AddCircuitBreaker( // Break the circuit if 50% of the requests fail
//         new HttpCircuitBreakerStrategyOptions
//         {
//             SamplingDuration = TimeSpan.FromSeconds(30), // 30 seconds
//             FailureRatio = 0.5, // 50% of the requests should fail
//             MinimumThroughput = 10, // 10 requests
//             BreakDuration = TimeSpan.FromSeconds(30) // 30 seconds
//         });

//     // per request timeout
//     pipeline.AddTimeout(TimeSpan.FromSeconds(1)); // Timeout after 5 seconds
// });

// adding the resilience pipeline method
builder.Services.AddHttpClient<InvoiceClient>().AddStandardResilienceHandler(); // Add the standard resilience handler


// Add services to the container.
var useDbContextPooling = true;
if (useDbContextPooling)
{
    builder.Services.AddDbContextPool<InvoiceDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}
else
{
    builder.Services.AddDbContext<InvoiceDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
