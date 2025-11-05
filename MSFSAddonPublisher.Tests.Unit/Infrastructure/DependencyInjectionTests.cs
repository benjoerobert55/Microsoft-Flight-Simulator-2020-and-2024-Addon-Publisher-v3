using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MSFSAddonPublisher.Domain.Repositories;
using MSFSAddonPublisher.Infrastructure;
using MSFSAddonPublisher.Infrastructure.Platforms;
using MSFSAddonPublisher.Infrastructure.Repositories;

namespace MSFSAddonPublisher.Tests.Unit.Infrastructure;

public class DependencyInjectionTests
{
    private readonly IConfiguration _configuration;

    public DependencyInjectionTests()
    {
        // Create minimal configuration for testing with valid URLs
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Publishing:Discord:WebhookUrl"] = "https://discord.com/api/webhooks/123/test",
            ["Publishing:Twitch:EndpointUrl"] = "https://api.twitch.tv/helix/eventsub/subscriptions/test"
        });
        _configuration = configBuilder.Build();
    }

    [Fact]
    public void AddInfrastructure_RegistersAddonRepository()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddInfrastructure(_configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        var repository = provider.GetService<IAddonRepository>();
        Assert.NotNull(repository);
        Assert.IsType<FileAddonRepository>(repository);
    }

    [Fact]
    public void AddInfrastructure_RegistersAddonRepositoryAsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddInfrastructure(_configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        var repository1 = provider.GetService<IAddonRepository>();
        var repository2 = provider.GetService<IAddonRepository>();
        Assert.Same(repository1, repository2);
    }

    [Fact]
    public void AddInfrastructure_RegistersDiscordPublishingPlatform()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddInfrastructure(_configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        var platform = provider.GetService<DiscordPublishingPlatform>();
        Assert.NotNull(platform);
    }

    [Fact]
    public void AddInfrastructure_RegistersDiscordPublishingPlatformAsTransient()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddInfrastructure(_configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        var platform1 = provider.GetService<DiscordPublishingPlatform>();
        var platform2 = provider.GetService<DiscordPublishingPlatform>();
        Assert.NotSame(platform1, platform2);
    }

    [Fact]
    public void AddInfrastructure_RegistersTwitchPublishingPlatform()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddInfrastructure(_configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        var platform = provider.GetService<TwitchPublishingPlatform>();
        Assert.NotNull(platform);
    }

    [Fact]
    public void AddInfrastructure_RegistersTwitchPublishingPlatformAsTransient()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddInfrastructure(_configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        var platform1 = provider.GetService<TwitchPublishingPlatform>();
        var platform2 = provider.GetService<TwitchPublishingPlatform>();
        Assert.NotSame(platform1, platform2);
    }

    [Fact]
    public void AddInfrastructure_ConfiguresHttpClientForDiscord()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddInfrastructure(_configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        var platform = provider.GetService<DiscordPublishingPlatform>();
        Assert.NotNull(platform);
        // HttpClient is injected via constructor, so if platform is created, HttpClient was configured
    }

    [Fact]
    public void AddInfrastructure_ConfiguresHttpClientForTwitch()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddInfrastructure(_configuration);
        var provider = services.BuildServiceProvider();

        // Assert
        var platform = provider.GetService<TwitchPublishingPlatform>();
        Assert.NotNull(platform);
        // HttpClient is injected via constructor, so if platform is created, HttpClient was configured
    }

    [Fact]
    public void AddInfrastructure_ReturnsServiceCollection()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var result = services.AddInfrastructure(_configuration);

        // Assert
        Assert.Same(services, result);
    }
}
