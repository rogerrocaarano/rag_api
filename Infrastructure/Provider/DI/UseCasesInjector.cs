using Application.UseCase;
using Domain.Repository;
using Domain.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Provider.DI;

public static class UseCasesInjector
{
    public static void InjectUseCases(this IServiceCollection services)
    {
        AddAskLlmUseCase(services);
        AddGetContextUseCase(services);
    }

    private static void AddAskLlmUseCase(IServiceCollection services)
    {
        services.AddScoped<AskLlmUseCase>(provider =>
        {
            var llm = provider.GetRequiredService<ILlmService>();
            return new AskLlmUseCase(llm);
        });
    }

    private static void AddGetContextUseCase(IServiceCollection services)
    {
        services.AddScoped<GetContextUseCase>(provider =>
        {
            var contextDb = provider.GetRequiredService<IContextRepository>();
            var textProcessor = provider.GetRequiredService<ITextProcessorService>();
            return new GetContextUseCase(contextDb, textProcessor);
        });
    }
}