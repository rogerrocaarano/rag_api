using Domain.Service;
using Infrastructure.Provider.Deepseek;
using Infrastructure.Provider.TextProcessor;
using Infrastructure.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class ProvidersInjector
{
    public static void InjectProviders(this IServiceCollection services, IConfiguration configuration)
    {
        AddDeepseekApiClient(services, configuration);
        AddTextProcessor(services, configuration);
        services.AddHostedService<QueueSenderService>();
    }

    private static void AddDeepseekApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiKey = configuration["Services:Deepseek:ApiKey"] ?? throw new InvalidOperationException();
        services.AddScoped<ILlmService>(_ => new DeepseekClient(apiKey));
    }

    private static void AddTextProcessor(IServiceCollection services, IConfiguration configuration)
    {
        var baseUrl = configuration["Services:TextProcessor:BaseUrl"] ?? throw new InvalidOperationException();
        services.AddScoped<ITextProcessorService>(_ => new TextProcessorService(baseUrl));
    }
}