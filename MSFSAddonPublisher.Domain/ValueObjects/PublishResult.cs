namespace MSFSAddonPublisher.Domain.ValueObjects;

/// <summary>
/// Immutable value object representing the result of a publishing operation.
/// Contains success status, message, count, and error details.
/// </summary>
public sealed record PublishResult
{
    /// <summary>
    /// Gets whether the publishing operation was successful.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Gets the message describing the result of the publishing operation.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// Gets the number of addons that were successfully published.
    /// </summary>
    public int PublishedCount { get; init; }

    /// <summary>
    /// Gets the list of errors that occurred during publishing.
    /// </summary>
    public IReadOnlyList<string> Errors { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PublishResult"/> class.
    /// </summary>
    /// <param name="success">Whether the operation was successful.</param>
    /// <param name="message">A message describing the result.</param>
    /// <param name="publishedCount">The number of addons successfully published.</param>
    /// <param name="errors">Optional list of errors that occurred.</param>
    /// <exception cref="ArgumentException">Thrown when message is null or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when publishedCount is negative.</exception>
    public PublishResult(
        bool success,
        string message,
        int publishedCount,
        IReadOnlyList<string>? errors = null)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Message cannot be null or whitespace.", nameof(message));
        }

        if (publishedCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(publishedCount), "Published count cannot be negative.");
        }

        Success = success;
        Message = message;
        PublishedCount = publishedCount;
        Errors = errors ?? Array.Empty<string>();
    }

    /// <summary>
    /// Creates a successful publish result.
    /// </summary>
    /// <param name="publishedCount">The number of addons successfully published.</param>
    /// <param name="message">Optional custom message. If not provided, a default message is used.</param>
    /// <returns>A new PublishResult indicating success.</returns>
    public static PublishResult CreateSuccess(int publishedCount, string? message = null)
    {
        return new PublishResult(
            success: true,
            message: message ?? $"Successfully published {publishedCount} addon(s).",
            publishedCount: publishedCount);
    }

    /// <summary>
    /// Creates a failed publish result.
    /// </summary>
    /// <param name="message">A message describing the failure.</param>
    /// <param name="errors">Optional list of specific errors.</param>
    /// <returns>A new PublishResult indicating failure.</returns>
    public static PublishResult CreateFailure(string message, IReadOnlyList<string>? errors = null)
    {
        return new PublishResult(
            success: false,
            message: message,
            publishedCount: 0,
            errors: errors);
    }

    /// <summary>
    /// Creates a partial success result where some addons were published but others failed.
    /// </summary>
    /// <param name="publishedCount">The number of addons successfully published.</param>
    /// <param name="totalCount">The total number of addons attempted.</param>
    /// <param name="errors">List of errors for failed addons.</param>
    /// <returns>A new PublishResult indicating partial success.</returns>
    public static PublishResult CreatePartialSuccess(int publishedCount, int totalCount, IReadOnlyList<string> errors)
    {
        return new PublishResult(
            success: false,
            message: $"Published {publishedCount} out of {totalCount} addon(s). {errors.Count} error(s) occurred.",
            publishedCount: publishedCount,
            errors: errors);
    }
}
