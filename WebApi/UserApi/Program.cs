using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using UserApi.DatabaseServices;
using UserApi.Services;

// Early init of NLog to allow startup and exception logging, before host is built
var _logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
_logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // NLog: Setups NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    // Adds services to the container.
    builder.Services.AddControllers();
    // Learns more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddLogging(
        builder =>
        {
            builder.AddFilter("Microsoft", Microsoft.Extensions.Logging.LogLevel.Warning)
                    .AddFilter("System", Microsoft.Extensions.Logging.LogLevel.Warning)
                    .AddFilter("NToastNotify", Microsoft.Extensions.Logging.LogLevel.Warning)
                    .AddConsole();
        });

    // Add DB context
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IUserService, UserService>();

    var app = builder.Build();

    // Gets environment name
    _logger.Info($"Environment name: {app.Environment.EnvironmentName}");

    // Configures the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    
}
catch (Exception exception)
{
    //NLog: catchs setup errors
    _logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensures to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}

public partial class Program { }
