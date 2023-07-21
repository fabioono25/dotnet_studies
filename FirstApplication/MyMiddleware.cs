using System.Diagnostics;
using System.Diagnostics.Contracts;
using Serilog;

namespace AppTestOne
{
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;

        public MyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // do some action before

            // call next middleware
            await _next(httpContext);

            // do something after
        }
    }

    public class LogTimeMiddleware
    {
        private readonly RequestDelegate _next;

        public LogTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            // do some action before
            var sw = Stopwatch.StartNew();

            // call next middleware
            await _next(httpContext);

            // do something after
            sw.Stop();

            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            Log.Information($"The execution last {sw.Elapsed.TotalMilliseconds} ms ({sw.Elapsed.TotalSeconds} seconds)");   
        }
    }

    public static class SerilogExtensions
    {
        public static void AddSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog();
        }
    }

    public static class LogTimeExtensions
    {
        public static void UseLogTime(this WebApplication app)
        {
            app.UseMiddleware<LogTimeMiddleware>();
        }
    }
}
