
// Builder config
var builder = WebApplication.CreateBuilder(args);


// Pipeline config

// Middleware config

// Services config


// App config
var app = builder.Build();


// App's behaviors config
app.MapGet("/", () => "Hello World!");


// Running the app
app.Run();
