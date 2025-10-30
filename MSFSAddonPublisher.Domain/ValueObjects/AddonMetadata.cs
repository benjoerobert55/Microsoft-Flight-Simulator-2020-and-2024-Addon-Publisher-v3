using System.Collections.ObjectModel;
using MSFSAddonPublisher.Domain.Enums;

namespace MSFSAddonPublisher.Domain.ValueObjects;

/// <summary>
/// Immutable value object representing metadata extracted from a Microsoft Flight Simulator addon manifest.
/// Based on the MSFS manifest.json file structure.
/// </summary>
public sealed record AddonMetadata
{
    /// <summary>
    /// Gets the title of the addon.
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Gets the creator/author of the addon.
    /// </summary>
    public string Creator { get; init; }

    /// <summary>
    /// Gets the version string of the addon.
    /// </summary>
    public string Version { get; init; }

    /// <summary>
    /// Gets the type of content (Aircraft, Scenery, etc.).
    /// </summary>
    public ContentType ContentType { get; init; }

    /// <summary>
    /// Gets the package version from the manifest.
    /// </summary>
    public string PackageVersion { get; init; }

    /// <summary>
    /// Gets the minimum game version required for this addon.
    /// </summary>
    public string MinimumGameVersion { get; init; }

    /// <summary>
    /// Gets the release notes as a read-only dictionary.
    /// Key represents version or locale, value contains the note text.
    /// </summary>
    public IReadOnlyDictionary<string, string> ReleaseNotes { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AddonMetadata"/> class.
    /// </summary>
    /// <param name="title">The title of the addon.</param>
    /// <param name="creator">The creator/author of the addon.</param>
    /// <param name="version">The version string of the addon.</param>
    /// <param name="contentType">The type of content.</param>
    /// <param name="packageVersion">The package version from the manifest.</param>
    /// <param name="minimumGameVersion">The minimum game version required.</param>
    /// <param name="releaseNotes">Optional release notes dictionary.</param>
    /// <exception cref="ArgumentException">Thrown when required parameters are null or whitespace.</exception>
    public AddonMetadata(
        string title,
        string creator,
        string version,
        ContentType contentType,
        string packageVersion,
        string minimumGameVersion,
        IReadOnlyDictionary<string, string>? releaseNotes = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null or whitespace.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(creator))
        {
            throw new ArgumentException("Creator cannot be null or whitespace.", nameof(creator));
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            throw new ArgumentException("Version cannot be null or whitespace.", nameof(version));
        }

        if (string.IsNullOrWhiteSpace(packageVersion))
        {
            throw new ArgumentException("Package version cannot be null or whitespace.", nameof(packageVersion));
        }

        if (string.IsNullOrWhiteSpace(minimumGameVersion))
        {
            throw new ArgumentException("Minimum game version cannot be null or whitespace.", nameof(minimumGameVersion));
        }

        Title = title;
        Creator = creator;
        Version = version;
        ContentType = contentType;
        PackageVersion = packageVersion;
        MinimumGameVersion = minimumGameVersion;
        ReleaseNotes = releaseNotes ?? new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
    }

    /// <summary>
    /// Determines whether two AddonMetadata instances are equal by comparing all properties including ReleaseNotes content.
    /// </summary>
    public bool Equals(AddonMetadata? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Title == other.Title
            && Creator == other.Creator
            && Version == other.Version
            && ContentType == other.ContentType
            && PackageVersion == other.PackageVersion
            && MinimumGameVersion == other.MinimumGameVersion
            && DictionariesEqual(ReleaseNotes, other.ReleaseNotes);
    }

    /// <summary>
    /// Returns a hash code for this instance based on all properties including ReleaseNotes content.
    /// </summary>
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(Title);
        hash.Add(Creator);
        hash.Add(Version);
        hash.Add(ContentType);
        hash.Add(PackageVersion);
        hash.Add(MinimumGameVersion);

        foreach (var kvp in ReleaseNotes.OrderBy(x => x.Key))
        {
            hash.Add(kvp.Key);
            hash.Add(kvp.Value);
        }

        return hash.ToHashCode();
    }

    private static bool DictionariesEqual(IReadOnlyDictionary<string, string> dict1, IReadOnlyDictionary<string, string> dict2)
    {
        if (dict1.Count != dict2.Count)
        {
            return false;
        }

        foreach (var kvp in dict1)
        {
            if (!dict2.TryGetValue(kvp.Key, out var value) || value != kvp.Value)
            {
                return false;
            }
        }

        return true;
    }
}
