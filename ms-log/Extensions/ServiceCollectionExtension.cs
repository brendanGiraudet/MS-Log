using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using ms_log.Data;
using ms_log.Models;
using ms_log.Services.ConfigurationService;
using ms_log.Services.LoggerService;
using ms_log.Services.RabbitMq;
using ms_log.Settings;
using System.Text.Json.Serialization;


namespace ms_log.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlite(connectionString));
    }

    public static void AddODataContext(this IServiceCollection services)
    {
        var modelBuilder = new ODataConventionModelBuilder();
        modelBuilder.EntitySet<LogModel>("Logs");

        services.AddControllers()
            .AddOData(
                options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(100).AddRouteComponents(
                    "odata",
                    modelBuilder.GetEdmModel())
            )
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
    }


    public static void AddCustomHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<RabbitMqSubscriberService>();
    }

    public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MSConfigurationSettings>(configuration.GetSection("MSConfigurationSettings"));
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddTransient<ILogger, LoggerService>();
        services.AddTransient<IConfigurationService, ConfigurationService>();
    }
}
