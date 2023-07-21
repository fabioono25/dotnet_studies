
// Builder config
using AppTestOne;
using Serilog;

// instance of .NET application
var builder = WebApplication.CreateBuilder(args);


// Pipeline config

// Middleware config
//builder.Host.UseSerilog();
builder.AddSerilog(); // a better construction, using extension methods

// Services config
builder.Services.AddControllersWithViews(); // adding the ASP.NET MVC

// App config
var app = builder.Build();

// behavior middleware
//app.UseMiddleware<LogTimeMiddleware>();
app.UseLogTime();

// App's behaviors config
app.MapGet("/", () => "Hello World!");

app.MapGet("/test", () =>
{
    Thread.Sleep(1000);
    return "Test";
});

// Running the app
app.Run();
