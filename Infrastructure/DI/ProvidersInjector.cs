using Domain.Service;
using Infrastructure.Provider.Deepseek;
using Infrastructure.Provider.TextProcessor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class ProvidersInjector
{
    public static void InjectProviders(this IServiceCollection services, IConfiguration configuration)
    {
        AddDeepseekApiClient(services, configuration);
        AddTextProcessor(services, configuration);
    }

    private static void AddDeepseekApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiKey = configuration["Services:Deepseek:ApiKey"] ?? throw new InvalidOperationException();
        services.AddSingleton<ILlmService>(_ => new DeepseekClient(apiKey));
    }

    private static void AddTextProcessor(IServiceCollection services, IConfiguration configuration)
    {
        var baseUrl = configuration["Services:TextProcessor:BaseUrl"] ?? throw new InvalidOperationException();
        services.AddSingleton<ITextProcessorService>(_ => new TextProcessorService(baseUrl));
    }
}