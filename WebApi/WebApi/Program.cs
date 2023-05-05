using NLog;
using NLog.Web;
using WebApi.Dapper;
using WebApi.Models;
using WebApi.Options;
using WebApi.Services;
using WebApi.Services.UserApi;

// Early init of NLog to allow startup and exception logging, before host is built
var _logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
_logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.Configure<UserUrlApi>(builder.Configuration.GetSection("UserUrlApi"));

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

    builder.Services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
    builder.Services.AddScoped<IGenericRepository<Product>, DapperRepository<Product>>();
    builder.Services.AddScoped<IReadProductRepository, ReadProductRepository>();
    builder.Services.AddScoped<IWriteProductRepository, WriteProductRepository>();
    builder.Services.AddScoped<IUserApiService, UserApiService>();
    builder.Services.AddScoped<IProductService, ProductService>();

    builder.Services.AddHttpClient<IUserApiService, UserApiService>();

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
