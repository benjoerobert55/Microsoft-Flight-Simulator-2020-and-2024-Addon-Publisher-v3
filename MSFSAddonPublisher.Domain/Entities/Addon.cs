using MSFSAddonPublisher.Domain.ValueObjects;

namespace MSFSAddonPublisher.Domain.Entities;

/// <summary>
/// Represents a Microsoft Flight Simulator addon entity with identity and business rules.
/// This is a domain entity that tracks addon state and lifecycle information.
/// </summary>
public sealed class Addon
{
    /// <summary>
    /// Gets the unique identifier for this addon instance.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the metadata extracted from the addon manifest.
    /// </summary>
    public AddonMetadata Metadata { get; }

    /// <summary>
    /// Gets the installation path where the addon is located on the file system.
    /// </summary>
    public string InstallPath { get; }

    /// <summary>
    /// Gets or sets whether this addon is currently selected for publishing.
    /// </summary>
    public bool IsSelected { get; private set; }

    /// <summary>
    /// Gets the timestamp when this addon was discovered during a scan.
    /// </summary>
    public DateTime DiscoveredAt { get; }

    /// <summary>
    /// Gets the timestamp when this entity was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the timestamp when this entity was last updated.
    /// </summary>
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Addon"/> class.
    /// </summary>
    /// <param name="metadata">The addon metadata extracted from manifest.json.</param>
    /// <param name="installPath">The installation path of the addon.</param>
    /// <param name="discoveredAt">The timestamp when the addon was discovered.</param>
    /// <exception cref="ArgumentNullException">Thrown when metadata is null.</exception>
    /// <exception cref="ArgumentException">Thrown when installPath is null or whitespace.</exception>
    public Addon(
        AddonMetadata metadata,
        string installPath,
        DateTime discoveredAt)
        : this(Guid.NewGuid(), metadata, installPath, false, discoveredAt, DateTime.UtcNow, DateTime.UtcNow)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Addon"/> class with full control over all properties.
    /// This constructor is primarily used for reconstitution from storage.
    /// </summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="metadata">The addon metadata.</param>
    /// <param name="installPath">The installation path.</param>
    /// <param name="isSelected">Whether the addon is selected.</param>
    /// <param name="discoveredAt">The discovery timestamp.</param>
    /// <param name="createdAt">The creation timestamp.</param>
    /// <param name="updatedAt">The last update timestamp.</param>
    /// <exception cref="ArgumentNullException">Thrown when metadata is null.</exception>
    /// <exception cref="ArgumentException">Thrown when installPath is null or whitespace, or id is empty.</exception>
    public Addon(
        Guid id,
        AddonMetadata metadata,
        string installPath,
        bool isSelected,
        DateTime discoveredAt,
        DateTime createdAt,
        DateTime updatedAt)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", nameof(id));
        }

        ArgumentNullException.ThrowIfNull(metadata);

        if (string.IsNullOrWhiteSpace(installPath))
        {
            throw new ArgumentException("Install path cannot be null or whitespace.", nameof(installPath));
        }

        Id = id;
        Metadata = metadata;
        InstallPath = installPath;
        IsSelected = isSelected;
        DiscoveredAt = discoveredAt;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Marks this addon as selected for publishing.
    /// </summary>
    public void Select()
    {
        if (!IsSelected)
        {
            IsSelected = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Marks this addon as not selected for publishing.
    /// </summary>
    public void Deselect()
    {
        if (IsSelected)
        {
            IsSelected = false;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Toggles the selection state of this addon.
    /// </summary>
    public void ToggleSelection()
    {
        IsSelected = !IsSelected;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Determines whether two Addon instances are equal based on their Id.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is Addon other && Id.Equals(other.Id);
    }

    /// <summary>
    /// Returns a hash code based on the entity's Id.
    /// </summary>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Returns a string representation of this addon.
    /// </summary>
    public override string ToString()
    {
        return $"Addon: {Metadata.Title} v{Metadata.Version} ({Metadata.ContentType}) at {InstallPath}";
    }
}
