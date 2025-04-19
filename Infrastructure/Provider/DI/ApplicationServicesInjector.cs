using Application.Service;
using Application.UseCase;
using Domain.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Provider.DI;

public static class ApplicationServicesInjector
{
    public static void InjectApplicationServices(this IServiceCollection services)
    {
        AddConversationService(services);
    }

    private static void AddConversationService(IServiceCollection services)
    {
        services.AddSingleton<ConversationService>(provider =>
        {
            var chats = provider.GetRequiredService<IChatsRepository>();
            var llm = provider.GetRequiredService<AskLlmUseCase>();
            var getContext = provider.GetRequiredService<GetContextUseCase>();
            return new ConversationService(chats, llm, getContext);
        });
    }
}