using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Support.Services.WebAPI.Middlewares;
using System.Net;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithEnvironmentName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.File(@"D:\Logs\log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Starting Web host");
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging
        .ClearProviders()
        .AddSimpleConsole()
        .AddDebug();
   
    builder.Host.UseSerilog();

    // Add Services to Global Exception
    builder.Services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();

    // Add Services to the container
    builder.Services.AddControllers();

    // Add services to the container.
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.UseExceptionHandler(_ => { });

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseSwagger();
    app.UseSwaggerUI();
    // Global OPTIONS Handler
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        context.Response.Headers.Append("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept, additional-header, Authorization");
        context.Response.Headers.Append("Access-Control-Allow-Methods", "HEAD, GET, POST, PUT, DELETE, OPTIONS");
        context.Response.Headers.Append("Access-Control-Allow-Credentials", "true");

        if (context.Request.Method == HttpMethods.Options)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            return;
        }

        await next.Invoke();
    });

    app.MapGet("/cod/test",
        [EnableCors("AnyOrigin")]
    [ResponseCache(NoStore = true)] () =>
        Results.Text("<script>" +
            "window.alert('Your client supports JavaScript!" +
            "\\r\\n\\r\\n" +
            $"Server time (UTC): {DateTime.UtcNow.ToString("o")}" +
            "\\r\\n" +
            "Client time (UTC): ' + new Date().toISOString());" +
            "</script>" +
            "<noscript>Your client does not support JavaScript</noscript>",
            "text/html"));

    app.MapControllers();

    app.Run();

}
catch(Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}