namespace MSFSAddonPublisher.Domain.Interfaces;

/// <summary>
/// Repository interface for managing addon persistence operations.
/// </summary>
public interface IAddonRepository
{
    /// <summary>
    /// Retrieves an addon by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the addon.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The addon if found; otherwise, null.</returns>
    Task<Entities.Addon?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all addons from the repository.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>A collection of all addons.</returns>
    Task<IEnumerable<Entities.Addon>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new addon to the repository.
    /// </summary>
    /// <param name="addon">The addon to add.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    Task AddAsync(Entities.Addon addon, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing addon in the repository.
    /// </summary>
    /// <param name="addon">The addon with updated information.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    Task UpdateAsync(Entities.Addon addon, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an addon from the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the addon to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an addon exists in the repository.
    /// </summary>
    /// <param name="id">The unique identifier of the addon.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>True if the addon exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Searches for addons matching the specified criteria.
    /// </summary>
    /// <param name="searchTerm">The search term to match against addon metadata.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>A collection of addons matching the search criteria.</returns>
    Task<IEnumerable<Entities.Addon>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
