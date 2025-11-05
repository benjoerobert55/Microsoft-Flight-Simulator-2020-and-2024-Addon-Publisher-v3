using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MSFSAddonPublisher.Domain.Entities;
using MSFSAddonPublisher.Domain.Enums;
using MSFSAddonPublisher.Domain.ValueObjects;
using MSFSAddonPublisher.Infrastructure.Platforms;

namespace MSFSAddonPublisher.Tests.Unit.Infrastructure.Platforms;

public sealed class TwitchPublishingPlatformTests
{
    [Fact(DisplayName = "ValidateCredentialsAsync returns true on 2xx (Twitch)")]
    public async Task ValidateCredentialsAsync_Success_ReturnsTrue()
    {
        var handler = new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.NoContent));
        var client = new HttpClient(handler);
        var platform = new TwitchPublishingPlatform(client, Options.Create(new TwitchPublishingOptions { EndpointUrl = "https://twitch.example/endpoint" }));

        var ok = await platform.ValidateCredentialsAsync();
        Assert.True(ok);
    }

    [Fact(DisplayName = "PublishAsync returns success and counts addons on 2xx (Twitch)")]
    public async Task PublishAsync_TwoAddons_Success()
    {
        var handler = new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.OK));
        var client = new HttpClient(handler);
        var platform = new TwitchPublishingPlatform(client, Options.Create(new TwitchPublishingOptions { EndpointUrl = "https://twitch.example/endpoint", Channel = "channel" }));

        var addons = new[] { CreateAddon("A1"), CreateAddon("A2") };
        var result = await platform.PublishAsync(addons, CancellationToken.None);

        Assert.True(result.Success);
        Assert.Equal(2, result.PublishedCount);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "PublishAsync returns failure on non-2xx status (Twitch)")]
    public async Task PublishAsync_NonSuccessStatus_Failure()
    {
        var handler = new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Bad" });
        var client = new HttpClient(handler);
        var platform = new TwitchPublishingPlatform(client, Options.Create(new TwitchPublishingOptions { EndpointUrl = "https://twitch.example/endpoint" }));

        var result = await platform.PublishAsync(new[] { CreateAddon("A1") }, CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal(0, result.PublishedCount);
    }

    [Fact(DisplayName = "Constructor throws when endpoint missing (Twitch)")]
    public void Ctor_NoEndpoint_Throws()
    {
        var client = new HttpClient(new StubHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)));
        Assert.Throws<InvalidOperationException>(() => new TwitchPublishingPlatform(client, Options.Create(new TwitchPublishingOptions { EndpointUrl = "" })));
    }

    private static Addon CreateAddon(string title)
    {
        var metadata = new AddonMetadata(
            title: title,
            creator: "Author",
            version: "1.0.0",
            contentType: ContentType.Aircraft,
            packageVersion: "1.0.0",
            minimumGameVersion: "1.0.0",
            releaseNotes: new Dictionary<string,string>());

        return new Addon(
            metadata,
            installPath: "/tmp/mock",
            discoveredAt: DateTime.UtcNow);
    }

    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;
        public StubHandler(Func<HttpRequestMessage, HttpResponseMessage> responder) => _responder = responder;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(_responder(request));
    }
}
