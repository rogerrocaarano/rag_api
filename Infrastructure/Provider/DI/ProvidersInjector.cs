using Domain.Service;
using Infrastructure.Provider.Deepseek;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Provider.DI;

public static class ProvidersInjector
{
    public static void InjectProviders(this IServiceCollection services, IConfiguration configuration)
    {
        AddDeepseekApiClient(services, configuration);
    }

    private static void AddDeepseekApiClient(IServiceCollection services, IConfiguration configuration)
    {
        var apiKey = configuration["Provider:Deepseek:ApiKey"] ?? throw new InvalidOperationException();
        services.AddSingleton<ILlmService>(_ => new DeepseekClient(apiKey));
    }
}