using Application.Service;
using Application.UseCase;
using Domain.Repository;
using Domain.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class ApplicationServicesInjector
{
    public static void InjectApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        AddConversationService(services);
        AddContextSeederService(services, configuration);
    }

    private static void AddConversationService(IServiceCollection services)
    {
        services.AddScoped<ConversationService>(provider =>
        {
            var chats = provider.GetRequiredService<IChatsRepository>();
            var llm = provider.GetRequiredService<AskLlmUseCase>();
            var getContext = provider.GetRequiredService<GetContextUseCase>();
            return new ConversationService(chats, llm, getContext);
        });
    }

    private static void AddContextSeederService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ContextSeeder>(provider =>
        {
            var contextDb = provider.GetRequiredService<IContextRepository>();
            var textProcessor = provider.GetRequiredService<ITextProcessorService>();
            var seedDirectory = configuration["Services:ContextSeeder:SeedDirectory"] ??
                                throw new InvalidOperationException();
            return new ContextSeeder(contextDb, textProcessor, seedDirectory);
        });
    }
}