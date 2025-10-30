using MSFSAddonPublisher.Domain.Enums;
using MSFSAddonPublisher.Domain.ValueObjects;

namespace MSFSAddonPublisher.Tests.Unit.Domain.ValueObjects;

/// <summary>
/// Unit tests for the AddonMetadata value object.
/// Tests validation, equality, and immutability characteristics.
/// </summary>
public class AddonMetadataTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesInstance()
    {
        // Arrange
        const string title = "Cessna 172";
        const string creator = "Test Author";
        const string version = "1.0.0";
        const ContentType contentType = ContentType.Aircraft;
        const string packageVersion = "1.0.0";
        const string minimumGameVersion = "1.32.0";

        // Act
        var metadata = new AddonMetadata(
            title,
            creator,
            version,
            contentType,
            packageVersion,
            minimumGameVersion);

        // Assert
        Assert.Equal(title, metadata.Title);
        Assert.Equal(creator, metadata.Creator);
        Assert.Equal(version, metadata.Version);
        Assert.Equal(contentType, metadata.ContentType);
        Assert.Equal(packageVersion, metadata.PackageVersion);
        Assert.Equal(minimumGameVersion, metadata.MinimumGameVersion);
        Assert.NotNull(metadata.ReleaseNotes);
        Assert.Empty(metadata.ReleaseNotes);
    }

    [Fact]
    public void Constructor_WithReleaseNotes_StoresReleaseNotes()
    {
        // Arrange
        var releaseNotes = new Dictionary<string, string>
        {
            { "1.0.0", "Initial release" },
            { "1.1.0", "Bug fixes" }
        };

        // Act
        var metadata = new AddonMetadata(
            "Test Addon",
            "Test Creator",
            "1.1.0",
            ContentType.Scenery,
            "1.1.0",
            "1.30.0",
            releaseNotes);

        // Assert
        Assert.Equal(2, metadata.ReleaseNotes.Count);
        Assert.Equal("Initial release", metadata.ReleaseNotes["1.0.0"]);
        Assert.Equal("Bug fixes", metadata.ReleaseNotes["1.1.0"]);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithNullOrWhiteSpaceTitle_ThrowsArgumentException(string? invalidTitle)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new AddonMetadata(
                invalidTitle!,
                "Creator",
                "1.0.0",
                ContentType.Aircraft,
                "1.0.0",
                "1.30.0"));

        Assert.Equal("title", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithNullOrWhiteSpaceCreator_ThrowsArgumentException(string? invalidCreator)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new AddonMetadata(
                "Title",
                invalidCreator!,
                "1.0.0",
                ContentType.Aircraft,
                "1.0.0",
                "1.30.0"));

        Assert.Equal("creator", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithNullOrWhiteSpaceVersion_ThrowsArgumentException(string? invalidVersion)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new AddonMetadata(
                "Title",
                "Creator",
                invalidVersion!,
                ContentType.Aircraft,
                "1.0.0",
                "1.30.0"));

        Assert.Equal("version", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithNullOrWhiteSpacePackageVersion_ThrowsArgumentException(string? invalidPackageVersion)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new AddonMetadata(
                "Title",
                "Creator",
                "1.0.0",
                ContentType.Aircraft,
                invalidPackageVersion!,
                "1.30.0"));

        Assert.Equal("packageVersion", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithNullOrWhiteSpaceMinimumGameVersion_ThrowsArgumentException(string? invalidMinGameVersion)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new AddonMetadata(
                "Title",
                "Creator",
                "1.0.0",
                ContentType.Aircraft,
                "1.0.0",
                invalidMinGameVersion!));

        Assert.Equal("minimumGameVersion", exception.ParamName);
    }

    [Fact]
    public void RecordEquality_WithSameValues_AreEqual()
    {
        // Arrange
        var metadata1 = new AddonMetadata(
            "Cessna 172",
            "Author",
            "1.0.0",
            ContentType.Aircraft,
            "1.0.0",
            "1.30.0");

        var metadata2 = new AddonMetadata(
            "Cessna 172",
            "Author",
            "1.0.0",
            ContentType.Aircraft,
            "1.0.0",
            "1.30.0");

        // Act & Assert
        Assert.Equal(metadata1, metadata2);
        Assert.True(metadata1 == metadata2);
        Assert.False(metadata1 != metadata2);
    }

    [Fact]
    public void RecordEquality_WithDifferentValues_AreNotEqual()
    {
        // Arrange
        var metadata1 = new AddonMetadata(
            "Cessna 172",
            "Author",
            "1.0.0",
            ContentType.Aircraft,
            "1.0.0",
            "1.30.0");

        var metadata2 = new AddonMetadata(
            "Boeing 737",
            "Author",
            "1.0.0",
            ContentType.Aircraft,
            "1.0.0",
            "1.30.0");

        // Act & Assert
        Assert.NotEqual(metadata1, metadata2);
        Assert.False(metadata1 == metadata2);
        Assert.True(metadata1 != metadata2);
    }

    [Fact]
    public void GetHashCode_WithSameValues_ReturnsSameHashCode()
    {
        // Arrange
        var metadata1 = new AddonMetadata(
            "Cessna 172",
            "Author",
            "1.0.0",
            ContentType.Aircraft,
            "1.0.0",
            "1.30.0");

        var metadata2 = new AddonMetadata(
            "Cessna 172",
            "Author",
            "1.0.0",
            ContentType.Aircraft,
            "1.0.0",
            "1.30.0");

        // Act & Assert
        Assert.Equal(metadata1.GetHashCode(), metadata2.GetHashCode());
    }

    [Fact]
    public void ReleaseNotes_IsReadOnly_CannotBeModified()
    {
        // Arrange
        var metadata = new AddonMetadata(
            "Test",
            "Creator",
            "1.0.0",
            ContentType.Aircraft,
            "1.0.0",
            "1.30.0");

        // Act & Assert
        Assert.IsAssignableFrom<IReadOnlyDictionary<string, string>>(metadata.ReleaseNotes);
    }

    [Theory]
    [InlineData(ContentType.Aircraft)]
    [InlineData(ContentType.Scenery)]
    [InlineData(ContentType.SimObject)]
    [InlineData(ContentType.Livery)]
    [InlineData(ContentType.Mission)]
    [InlineData(ContentType.Unknown)]
    public void Constructor_WithAllContentTypes_CreatesInstance(ContentType contentType)
    {
        // Act
        var metadata = new AddonMetadata(
            "Test",
            "Creator",
            "1.0.0",
            contentType,
            "1.0.0",
            "1.30.0");

        // Assert
        Assert.Equal(contentType, metadata.ContentType);
    }

    [Fact]
    public void With_ModifyingProperty_CreatesNewInstance()
    {
        // Arrange
        var original = new AddonMetadata(
            "Cessna 172",
            "Author",
            "1.0.0",
            ContentType.Aircraft,
            "1.0.0",
            "1.30.0");

        // Act
        var modified = original with { Version = "2.0.0" };

        // Assert
        Assert.NotEqual(original, modified);
        Assert.Equal("1.0.0", original.Version);
        Assert.Equal("2.0.0", modified.Version);
        Assert.Equal(original.Title, modified.Title);
    }
}
