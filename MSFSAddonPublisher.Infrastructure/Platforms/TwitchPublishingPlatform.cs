using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using MSFSAddonPublisher.Domain.Entities;
using MSFSAddonPublisher.Domain.Interfaces;
using MSFSAddonPublisher.Domain.ValueObjects;

namespace MSFSAddonPublisher.Infrastructure.Platforms;

/// <summary>
/// Twitch implementation of <see cref="IPublishingPlatform"/>. For Phase 2, we support a configurable webhook-style endpoint
/// to keep the implementation testable and credential-safe. In a future phase, this can be upgraded to Helix APIs with OAuth.
/// </summary>
public sealed class TwitchPublishingPlatform : IPublishingPlatform
{
    private readonly HttpClient _httpClient;
    private readonly string _endpointUrl;
    private readonly string? _channel;

    /// <summary>
    /// Initializes a new instance of the <see cref="TwitchPublishingPlatform"/> class.
    /// </summary>
    /// <param name="httpClient">HTTP client used to send publishing requests.</param>
    /// <param name="options">Options or environment-provided configuration.</param>
    /// <exception cref="ArgumentNullException">Thrown when httpClient is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when endpoint URL is not configured.</exception>
    public TwitchPublishingPlatform(HttpClient httpClient, IOptions<TwitchPublishingOptions> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        var optionsValue = options?.Value ?? TwitchPublishingOptions.FromEnvironment();
        if (string.IsNullOrWhiteSpace(optionsValue.EndpointUrl))
        {
            throw new InvalidOperationException("Twitch endpoint URL is not configured. Set TWITCH_ENDPOINT_URL or provide via options.");
        }

        _endpointUrl = optionsValue.EndpointUrl!;
        _channel = optionsValue.Channel;
    }

    /// <inheritdoc />
    public string PlatformName => "Twitch";

    /// <inheritdoc />
    public async Task<PublishResult> PublishAsync(IEnumerable<Addon> addons, CancellationToken cancellationToken)
    {
        if (addons is null)
        {
            throw new ArgumentNullException(nameof(addons));
        }

        var list = addons.ToList();
        if (list.Count == 0)
        {
            return PublishResult.CreateFailure("No addons provided to publish.");
        }

        var content = BuildMessage(list);
        try
        {
            using var payload = JsonContent.Create(new
            {
                channel = _channel,
                message = content
            });

            using var response = await _httpClient.PostAsync(_endpointUrl, payload, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                return PublishResult.CreateFailure($"Twitch endpoint returned {(int)response.StatusCode}: {response.ReasonPhrase}. Body: {body}");
            }

            return PublishResult.CreateSuccess(list.Count, $"Published {list.Count} addon(s) to Twitch.");
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return PublishResult.CreateFailure($"Failed to publish to Twitch: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public async Task<bool> ValidateCredentialsAsync()
    {
        try
        {
            using var payload = JsonContent.Create(new { message = "MSFS Addon Publisher: validation ping" });
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var response = await _httpClient.PostAsync(_endpointUrl, payload, cts.Token).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private static string BuildMessage(IReadOnlyList<Addon> addons)
    {
        var lines = new List<string> { "New MSFS Addons:" };
        foreach (var a in addons)
        {
            lines.Add($"- {a.Metadata.Title} — v{a.Metadata.Version}");
        }

        return string.Join("\n", lines);
    }
}

/// <summary>
/// Options for configuring the Twitch publishing platform.
/// </summary>
public sealed class TwitchPublishingOptions
{
    /// <summary>
    /// Endpoint URL to post messages to (webhook, proxy, or bot gateway). Required for Phase 2.
    /// </summary>
    public string? EndpointUrl { get; set; }

    /// <summary>
    /// Optional channel identifier/slug.
    /// </summary>
    public string? Channel { get; set; }

    /// <summary>
    /// Creates options from environment variables.
    /// </summary>
    public static TwitchPublishingOptions FromEnvironment()
    {
        return new TwitchPublishingOptions
        {
            EndpointUrl = Environment.GetEnvironmentVariable("TWITCH_ENDPOINT_URL"),
            Channel = Environment.GetEnvironmentVariable("TWITCH_CHANNEL")
        };
    }
}
