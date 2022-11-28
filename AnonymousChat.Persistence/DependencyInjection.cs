using AnonymousChat.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnonymousChat.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = "";

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            connectionString = configuration["ProductionDbConnection"];
        else
            connectionString = configuration["DbConnection"];

        services.AddDbContext<AnonChatDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddScoped<IAnonChatDbContext, AnonChatDbContext>();

        return services;
    }
}