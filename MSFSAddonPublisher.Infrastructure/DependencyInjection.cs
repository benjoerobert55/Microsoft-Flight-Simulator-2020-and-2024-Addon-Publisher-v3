using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSFSAddonPublisher.Domain.Repositories;
using MSFSAddonPublisher.Infrastructure.Platforms;
using MSFSAddonPublisher.Infrastructure.Repositories;

namespace MSFSAddonPublisher.Infrastructure;

/// <summary>
/// Extension methods for configuring infrastructure services in the dependency injection container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds all infrastructure services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The configuration instance for binding options.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register repository as singleton since it manages file-based persistence with thread safety
        services.AddSingleton<IAddonRepository, FileAddonRepository>();

        // Configure HttpClient for Discord platform
        services.AddHttpClient<DiscordPublishingPlatform>();

        // Configure HttpClient for Twitch platform
        services.AddHttpClient<TwitchPublishingPlatform>();

        // Bind configuration options for Discord platform
        services.Configure<DiscordPublishingOptions>(
            configuration.GetSection("Publishing:Discord"));

        // Bind configuration options for Twitch platform
        services.Configure<TwitchPublishingOptions>(
            configuration.GetSection("Publishing:Twitch"));

        return services;
    }
}
