using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Truhome.Business.Interfaces;
using Truhome.Business.Managers;
using Truhome.Domain.Contexts;
using Truhome.Domain.Services;

namespace Truhome.Api.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services, string dbConnectionString)
    {
        services.AddDatabase(dbConnectionString);
        services.AddManagers();
        services.AddIntegrations();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, string dbConnectionString)
    {
        services.AddSingleton<DatabaseSetupService>();

        services.AddDbContext<TruhomeDbContext>(options =>
            options.UseNpgsql(dbConnectionString));

        return services;
    }

    private static IServiceCollection AddManagers(this IServiceCollection services)
    {
        services.AddScoped<ICustomerManager, CustomerManager>();

        return services;
    }

    private static IServiceCollection AddIntegrations(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}
