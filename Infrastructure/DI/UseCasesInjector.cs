using Application.UseCase;
using Domain.Repository;
using Domain.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class UseCasesInjector
{
    public static void InjectUseCases(this IServiceCollection services, IConfiguration configuration)
    {
        AddAskLlmUseCase(services, configuration);
        AddGetContextUseCase(services);
    }

    private static void AddAskLlmUseCase(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AskLlmUseCase>(provider =>
        {
            var llm = provider.GetRequiredService<ILlmService>();
            var ruleSet = configuration["Services:Deepseek:Rules"]?.Split(Environment.NewLine)
                ?? throw new InvalidOperationException("Services:Deepseek:Rules not configured");
            return new AskLlmUseCase(llm, ruleSet.ToList());
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