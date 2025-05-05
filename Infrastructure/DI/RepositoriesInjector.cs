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
        AddChatsDbContext(services, configuration);
        AddMessagesRepository(services);
        AddConversationsRepository(services);
        AddFirebaseUsersRepository(services);
    }

    private static void AddChromaDbContextRepository(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IContextRepository, ContextRepository>(provider =>
        {
            var chromaUri = configuration["Services:ContextRepository:ChromaUri"] ??
                            throw new InvalidOperationException();
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

    private static void AddChatsDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:ChatsDb"] ?? throw new InvalidOperationException();
        services.AddDbContext<ChatsDb>(options => options.UseSqlite(connectionString));

        using var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<ChatsDb>();
        dbContext.Database.Migrate();
    }

    private static void AddMessagesRepository(IServiceCollection services)
    {
        services.AddScoped<IMessagesRepository, MessagesRepository>();
    }

    private static void AddConversationsRepository(IServiceCollection services)
    {
        services.AddScoped<IConversationsRepository, ConversationsRepository>();
    }
    
    private static void AddFirebaseUsersRepository(IServiceCollection services)
    {
        services.AddScoped<IFirebaseUsersRepository, FirebaseUsersRepository>();
    }
}