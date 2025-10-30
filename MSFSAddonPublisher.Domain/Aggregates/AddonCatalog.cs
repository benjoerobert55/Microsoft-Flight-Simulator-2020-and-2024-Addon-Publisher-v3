using MSFSAddonPublisher.Domain.Entities;
using MSFSAddonPublisher.Domain.Enums;

namespace MSFSAddonPublisher.Domain.Aggregates;

/// <summary>
/// Aggregate root for managing a collection of Microsoft Flight Simulator addons.
/// Enforces business rules and consistency boundaries for addon operations.
/// </summary>
public sealed class AddonCatalog
{
    private readonly Dictionary<Guid, Addon> _addons;

    /// <summary>
    /// Gets the unique identifier for this catalog instance.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the timestamp when this catalog was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the timestamp when this catalog was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the total count of addons in the catalog.
    /// </summary>
    public int Count => _addons.Count;

    /// <summary>
    /// Initializes a new instance of the <see cref="AddonCatalog"/> class.
    /// </summary>
    public AddonCatalog()
        : this(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, new Dictionary<Guid, Addon>())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddonCatalog"/> class with full control over all properties.
    /// This constructor is primarily used for reconstitution from storage.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="createdAt">The creation timestamp.</param>
    /// <param name="updatedAt">The last update timestamp.</param>
    /// <param name="addons">The collection of addons.</param>
    /// <exception cref="ArgumentException">Thrown when id is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when addons collection is null.</exception>
    public AddonCatalog(
        Guid id,
        DateTime createdAt,
        DateTime updatedAt,
        Dictionary<Guid, Addon> addons)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }

        ArgumentNullException.ThrowIfNull(addons);

        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        _addons = new Dictionary<Guid, Addon>(addons);
    }

    /// <summary>
    /// Adds an addon to the catalog. If an addon with the same ID already exists, it will be replaced.
    /// </summary>
    /// <param name="addon">The addon to add.</param>
    /// <exception cref="ArgumentNullException">Thrown when addon is null.</exception>
    public void AddAddon(Addon addon)
    {
        ArgumentNullException.ThrowIfNull(addon);

        _addons[addon.Id] = addon;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Removes an addon from the catalog by its ID.
    /// </summary>
    /// <param name="addonId">The ID of the addon to remove.</param>
    /// <returns>True if the addon was removed; false if it was not found.</returns>
    public bool RemoveAddon(Guid addonId)
    {
        if (_addons.Remove(addonId))
        {
            UpdatedAt = DateTime.UtcNow;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets all addons that are currently selected for publishing.
    /// </summary>
    /// <returns>A read-only collection of selected addons.</returns>
    public IReadOnlyCollection<Addon> GetSelectedAddons()
    {
        return _addons.Values
            .Where(addon => addon.IsSelected)
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// Clears the selection state of all addons in the catalog.
    /// </summary>
    public void ClearSelection()
    {
        var anyDeselected = false;

        foreach (var addon in _addons.Values)
        {
            if (addon.IsSelected)
            {
                addon.Deselect();
                anyDeselected = true;
            }
        }

        if (anyDeselected)
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Selects all addons in the catalog for publishing.
    /// </summary>
    public void SelectAll()
    {
        var anySelected = false;

        foreach (var addon in _addons.Values)
        {
            if (!addon.IsSelected)
            {
                addon.Select();
                anySelected = true;
            }
        }

        if (anySelected)
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Gets all addons of a specific content type.
    /// </summary>
    /// <param name="contentType">The content type to filter by.</param>
    /// <returns>A read-only collection of addons matching the specified content type.</returns>
    public IReadOnlyCollection<Addon> GetAddonsByType(ContentType contentType)
    {
        return _addons.Values
            .Where(addon => addon.Metadata.ContentType == contentType)
            .ToList()
            .AsReadOnly();
    }

    /// <summary>
    /// Gets all addons in the catalog.
    /// </summary>
    /// <returns>A read-only collection of all addons.</returns>
    public IReadOnlyCollection<Addon> GetAllAddons()
    {
        return _addons.Values.ToList().AsReadOnly();
    }

    /// <summary>
    /// Gets an addon by its ID.
    /// </summary>
    /// <param name="addonId">The ID of the addon to retrieve.</param>
    /// <returns>The addon if found; otherwise, null.</returns>
    public Addon? GetAddonById(Guid addonId)
    {
        return _addons.TryGetValue(addonId, out var addon) ? addon : null;
    }

    /// <summary>
    /// Checks whether an addon with the specified ID exists in the catalog.
    /// </summary>
    /// <param name="addonId">The ID of the addon to check.</param>
    /// <returns>True if the addon exists; otherwise, false.</returns>
    public bool ContainsAddon(Guid addonId)
    {
        return _addons.ContainsKey(addonId);
    }

    /// <summary>
    /// Clears all addons from the catalog.
    /// </summary>
    public void Clear()
    {
        if (_addons.Count > 0)
        {
            _addons.Clear();
            UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Returns a string representation of this catalog.
    /// </summary>
    public override string ToString()
    {
        var selectedCount = _addons.Values.Count(a => a.IsSelected);
        return $"AddonCatalog: {Count} addons ({selectedCount} selected)";
    }
}
