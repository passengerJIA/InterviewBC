using System;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try { 
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    // Add health checks
    builder.Services.AddHealthChecks();
    builder.Services.AddControllers();
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    // Add exception handler, to send end-user standard error message, and also write log into files.
    app.ConfigureExceptionHandler(logger);
    //Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecks("/api/health");

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}