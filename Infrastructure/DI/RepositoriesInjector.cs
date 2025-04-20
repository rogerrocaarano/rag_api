using Domain.Repository;
using Infrastructure.Persistence.ChatsRepository;
using Infrastructure.Persistence.ContextRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class RepositoriesInjector
{
    public static void InjectRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        AddChromaDbContextRepository(services, configuration);
        AddChatsRepository(services, configuration);
    }
    
    private static void AddChromaDbContextRepository(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IContextRepository, ContextRepository>(provider =>
        {
            var chromaUri = configuration["Services:ContextRepository:ChromaUri"] ?? throw new InvalidOperationException();
            var contextCollectionName = configuration["Services:ContextRepository:ContextCollection"] ??
                                        throw new InvalidOperationException();
            var fragmentCollectionName = configuration["Services:ContextRepository:FragmentCollection"] ??
                                         throw new InvalidOperationException();

            var httpClient = provider.GetService<HttpClient>();
            var repository = new ContextRepository(chromaUri, httpClient);
            repository.InitializeAsync(contextCollectionName, fragmentCollectionName).GetAwaiter().GetResult();
            return repository;
        });
    }
    
    private static void AddChatsRepository(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:ChatsDb"] ?? throw new InvalidOperationException();
        services.AddDbContext<ChatsDb>(options => options.UseSqlite(connectionString));
        services.AddSingleton<IChatsRepository, ChatsRepository>(provider =>
        {
            var dbContext = provider.GetRequiredService<ChatsDb>();
            return new ChatsRepository(dbContext);
        });
    }
}