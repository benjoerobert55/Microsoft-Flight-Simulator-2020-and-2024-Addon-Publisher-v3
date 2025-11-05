using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using MSFSAddonPublisher.Domain.Entities;
using MSFSAddonPublisher.Domain.Interfaces;
using MSFSAddonPublisher.Domain.ValueObjects;

namespace MSFSAddonPublisher.Infrastructure.Platforms;

/// <summary>
/// Discord implementation of <see cref="IPublishingPlatform"/> using an incoming webhook.
/// Secure by default: requires webhook URL via options or environment variable DISCORD_WEBHOOK_URL.
/// </summary>
public sealed class DiscordPublishingPlatform : IPublishingPlatform
{
    private readonly HttpClient _httpClient;
    private readonly string _webhookUrl;
    private readonly string? _username;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscordPublishingPlatform"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to send webhook requests.</param>
    /// <param name="options">Options containing webhook configuration. If null, falls back to environment variables.</param>
    /// <exception cref="ArgumentNullException">Thrown when httpClient is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when webhook URL is not provided in options or environment.</exception>
    public DiscordPublishingPlatform(HttpClient httpClient, IOptions<DiscordPublishingOptions> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        var optionsValue = options?.Value ?? DiscordPublishingOptions.FromEnvironment();
        if (string.IsNullOrWhiteSpace(optionsValue.WebhookUrl))
        {
            throw new InvalidOperationException("Discord webhook URL is not configured. Set it via options or DISCORD_WEBHOOK_URL environment variable.");
        }

        _webhookUrl = optionsValue.WebhookUrl!;
        _username = optionsValue.Username;
    }

    /// <inheritdoc />
    public string PlatformName => "Discord";

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

        var content = BuildDiscordMessage(list);
        try
        {
            using var payload = JsonContent.Create(new
            {
                username = _username,
                content
            });

            using var response = await _httpClient.PostAsync(_webhookUrl, payload, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                return PublishResult.CreateFailure($"Discord webhook returned {(int)response.StatusCode}: {response.ReasonPhrase}. Body: {body}");
            }

            return PublishResult.CreateSuccess(list.Count, $"Published {list.Count} addon(s) to Discord.");
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            return PublishResult.CreateFailure($"Failed to publish to Discord: {ex.Message}");
        }
    }

    /// <inheritdoc />
    public async Task<bool> ValidateCredentialsAsync()
    {
        try
        {
            using var payload = JsonContent.Create(new { content = "MSFS Addon Publisher: validation ping" });
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var response = await _httpClient.PostAsync(_webhookUrl, payload, cts.Token).ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    private static string BuildDiscordMessage(IReadOnlyList<Addon> addons)
    {
        // Simple text content summarizing addons. Future enhancement: rich embeds.
        var lines = new List<string>
        {
            "New MSFS Addons Published:",
        };

        foreach (var a in addons)
        {
            lines.Add($"• {a.Metadata.Title} ({a.Metadata.ContentType}) — v{a.Metadata.Version}");
        }

        return string.Join("\n", lines);
    }
}

/// <summary>
/// Options for configuring the Discord publishing platform.
/// </summary>
public sealed class DiscordPublishingOptions
{
    /// <summary>
    /// Discord webhook URL. Required.
    /// </summary>
    public string? WebhookUrl { get; set; }

    /// <summary>
    /// Optional username override shown in the webhook message.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Creates options from environment variables.
    /// </summary>
    public static DiscordPublishingOptions FromEnvironment()
    {
        return new DiscordPublishingOptions
        {
            WebhookUrl = Environment.GetEnvironmentVariable("DISCORD_WEBHOOK_URL"),
            Username = Environment.GetEnvironmentVariable("DISCORD_USERNAME")
        };
    }
}
