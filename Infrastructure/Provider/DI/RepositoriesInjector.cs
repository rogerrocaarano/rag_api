using Domain.Repository;
using Infrastructure.Persistence.ContextRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Provider.DI;

public static class RepositoriesInjector
{
    public static IServiceCollection InjectRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        AddChromaDbContextRepository(services, configuration);
        return services;
    }
    
    private static void AddChromaDbContextRepository(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IContextRepository, ContextRepository>(_ =>
        {
            var chromaUri = configuration["Repository:Context:ChromaUri"] ?? throw new InvalidOperationException();
            var contextCollectionName = configuration["Repository:Context:ContextCollection"] ??
                                        throw new InvalidOperationException();
            var fragmentCollectionName = configuration["Repository:Context:FragmentCollection"] ??
                                         throw new InvalidOperationException();

            return new ContextRepository(chromaUri, contextCollectionName, fragmentCollectionName);
        });
    }
}