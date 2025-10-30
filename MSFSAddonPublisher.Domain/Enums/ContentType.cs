namespace MSFSAddonPublisher.Domain.Enums;

/// <summary>
/// Represents the type of content for a Microsoft Flight Simulator addon.
/// Based on the content_type field in MSFS manifest.json files.
/// </summary>
public enum ContentType
{
    /// <summary>
    /// Aircraft addon (flyable planes, helicopters, etc.)
    /// </summary>
    Aircraft,

    /// <summary>
    /// Scenery addon (airports, cities, landmarks, etc.)
    /// </summary>
    Scenery,

    /// <summary>
    /// SimObject addon (non-flyable objects in the simulator)
    /// </summary>
    SimObject,

    /// <summary>
    /// Livery addon (paint schemes for aircraft)
    /// </summary>
    Livery,

    /// <summary>
    /// Mission addon (flight challenges, tutorials, etc.)
    /// </summary>
    Mission,

    /// <summary>
    /// Unknown or unrecognized content type
    /// </summary>
    Unknown
}
