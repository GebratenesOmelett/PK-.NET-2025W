using Microsoft.Extensions.DependencyInjection;
using TextAnalytics.Core;
using TextAnalytics.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTextAnalyticsServices(this IServiceCollection services)
    {
        services
            .AddSingleton<ILoggerService, ConsoleLogger>()
            .AddSingleton<IInputProvider, ConsoleInputProvider>()
            .AddSingleton<TextAnalyzer>();

        return services;
    }

    public static IServiceCollection AddFileInputProvider(this IServiceCollection services, string filePath)
    {
        services.AddSingleton<IInputProvider>(new FileInputProvider(filePath));
        return services;
    }
}