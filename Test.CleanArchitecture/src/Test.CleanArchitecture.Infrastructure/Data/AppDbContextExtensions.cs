﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Test.CleanArchitecture.Infrastructure.Data;

public static class AppDbContextExtensions
{
  public static void AddApplicationDbContext(this IServiceCollection services, string connectionString) =>
    services.AddDbContext<AppDbContext>(options =>
         options.UseSqlite(connectionString));

}
