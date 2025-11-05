using MSFSAddonPublisher.Domain.Entities;

namespace MSFSAddonPublisher.Domain.Repositories;

/// <summary>
/// Repository interface for managing Addon entities with async operations.
/// Follows the Repository pattern for data access abstraction.
/// </summary>
public interface IAddonRepository
{
    /// <summary>
    /// Gets all addons from the repository.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all addons.</returns>
    Task<IEnumerable<Addon>> GetAllAsync();

    /// <summary>
    /// Gets an addon by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the addon.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the addon if found; otherwise, null.</returns>
    Task<Addon?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new addon to the repository.
    /// </summary>
    /// <param name="addon">The addon to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when addon is null.</exception>
    Task AddAsync(Addon addon);

    /// <summary>
    /// Updates an existing addon in the repository.
    /// </summary>
    /// <param name="addon">The addon to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when addon is null.</exception>
    Task UpdateAsync(Addon addon);

    /// <summary>
    /// Deletes an addon from the repository by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the addon to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Gets the total count of addons in the repository.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the total count of addons.</returns>
    Task<int> CountAsync();
}
