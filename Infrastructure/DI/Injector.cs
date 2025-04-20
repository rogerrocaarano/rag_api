using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class Injector
{
    public static void InjectDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.InjectRepositories(configuration);
        services.InjectProviders(configuration);
        services.InjectUseCases();
        services.InjectApplicationServices();
    }
}