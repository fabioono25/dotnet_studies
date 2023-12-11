using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MinAPI.data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConnBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("SqlDbConnection"));
sqlConnBuilder.UserID = builder.Configuration["UserId"];
sqlConnBuilder.Password = builder.Configuration["Password"];

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(sqlConnBuilder.ConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
