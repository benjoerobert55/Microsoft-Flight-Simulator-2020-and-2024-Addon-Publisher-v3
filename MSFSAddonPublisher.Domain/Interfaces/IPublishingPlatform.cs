using MSFSAddonPublisher.Domain.Entities;
using MSFSAddonPublisher.Domain.ValueObjects;

namespace MSFSAddonPublisher.Domain.Interfaces;

/// <summary>
/// Interface for publishing platform implementations.
/// Enables the Strategy pattern for platform-specific publishing logic.
/// </summary>
public interface IPublishingPlatform
{
    /// <summary>
    /// Gets the name of the publishing platform (e.g., "Discord", "Twitch").
    /// </summary>
    string PlatformName { get; }

    /// <summary>
    /// Publishes a collection of addons to the platform asynchronously.
    /// </summary>
    /// <param name="addons">The collection of addons to publish.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the publish result with success status and details.</returns>
    /// <exception cref="ArgumentNullException">Thrown when addons collection is null.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled via the cancellation token.</exception>
    Task<PublishResult> PublishAsync(IEnumerable<Addon> addons, CancellationToken cancellationToken);

    /// <summary>
    /// Validates that the platform credentials are configured correctly and can connect to the service.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if credentials are valid; otherwise, false.</returns>
    Task<bool> ValidateCredentialsAsync();
}
