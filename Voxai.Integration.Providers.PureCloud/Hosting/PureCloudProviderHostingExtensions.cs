using Microsoft.Extensions.DependencyInjection;
using PureCloudPlatform.Client.V2.Client;
using System.Reflection;

namespace Voxai.Integration.Providers.PureCloud;

/// <summary>
/// Contains extension methods for registering types in a DI container.
/// </summary>
public static class PureCloudProviderHostingExtensions
{
    /// <summary>
    /// Registers providers and their dependencies in the DI container.
    /// </summary>
    /// <param name="services">The DI container, provided at startup.</param>
    /// <returns>The provided <paramref name="services"/> instance.</returns>
    public static IServiceCollection AddPureCloudProviders(this IServiceCollection services)
        => services.AddPureCloudApis()
                   .AddPureCloudProviderOptions()
                   .AddScoped<IUsersProvider, UsersProvider>()
                   .AddScoped<IGroupsProvider, GroupProvider>()
                   .AddScoped<IConversationProvider, ConversationProvider>()
                   .AddScoped<IQueueProvider, QueueProvider>()
                   .AddScoped<INotificationProvider, NotificationProvider>();

    /// <summary>
    /// Registers the client in the DI container, along with its dependencies.
    /// </summary>
    /// <param name="services">The DI container, provided at startup.</param>
    /// <returns>The provided <paramref name="services"/> instance.</returns>
    private static IServiceCollection AddPureCloudClient(this IServiceCollection services)
        => services.AddPureCloudClientOptions()
                   .AddScoped<IPureCloudClient, PureCloudClient>()
                   .AddScoped(serviceProvider => serviceProvider.GetRequiredService<IPureCloudClient>().ClientConfig);

    /// <summary>
    /// Registers the client options in the DI container.
    /// </summary>
    /// <param name="services">The DI container, provided at startup.</param>
    /// <returns>The provided <paramref name="services"/> instance.</returns>
    private static IServiceCollection AddPureCloudClientOptions(this IServiceCollection services)
    {
        services.AddOptions<PureCloudClientOptions>()
            .BindConfiguration(PureCloudClientOptions.ConfigSection)
            .ValidateDataAnnotations();
        return services;
    }

    private static IServiceCollection AddPureCloudProviderOptions(this IServiceCollection services)
    {
        services.AddOptions<ProviderOptions>()
            .BindConfiguration(ProviderOptions.ConfigSection);
        return services;
    }

    /// <summary>
    /// Registers all PureCloud API classes in the DI container.
    /// </summary>
    /// <param name="services">The DI container, provided at startup.</param>
    /// <returns>The provided <paramref name="services"/> instance.</returns>
    private static IServiceCollection AddPureCloudApis(this IServiceCollection services)
    {
        services.AddPureCloudClient();

        foreach (var (implementation, contracts) in PureCloudReflectionExtensions.ApiContractMap)
        {
            var ctor = implementation.GetConstructor(new[] { typeof(Configuration) });
            if (ctor is ConstructorInfo info)
            {
                foreach (var contract in contracts)
                    services.AddScoped(contract, provider => info.Invoke(new[] { provider.GetRequiredService<Configuration>() }));
            }
        }

        return services;
    }
}
