using MSFSAddonPublisher.Domain.Entities;
using MSFSAddonPublisher.Domain.Enums;
using MSFSAddonPublisher.Domain.ValueObjects;

namespace MSFSAddonPublisher.Tests.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the Addon entity.
/// Tests entity identity, business rules, state mutations, and validation.
/// </summary>
public class AddonTests
{
    private static AddonMetadata CreateValidMetadata()
    {
        return new AddonMetadata(
            "Test Addon",
            "Test Creator",
            "1.0.0",
            ContentType.Aircraft,
            "1.0.0",
            "1.30.0");
    }

    [Fact]
    public void Constructor_WithValidParameters_CreatesInstance()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        const string installPath = "C:\\MSFS\\Community\\TestAddon";
        var discoveredAt = DateTime.UtcNow;

        // Act
        var addon = new Addon(metadata, installPath, discoveredAt);

        // Assert
        Assert.NotEqual(Guid.Empty, addon.Id);
        Assert.Equal(metadata, addon.Metadata);
        Assert.Equal(installPath, addon.InstallPath);
        Assert.False(addon.IsSelected);
        Assert.Equal(discoveredAt, addon.DiscoveredAt);
        Assert.True((DateTime.UtcNow - addon.CreatedAt).TotalSeconds < 1);
        Assert.True((DateTime.UtcNow - addon.UpdatedAt).TotalSeconds < 1);
    }

    [Fact]
    public void Constructor_WithFullParameters_CreatesInstanceWithSpecifiedValues()
    {
        // Arrange
        var id = Guid.NewGuid();
        var metadata = CreateValidMetadata();
        const string installPath = "C:\\MSFS\\Community\\TestAddon";
        const bool isSelected = true;
        var discoveredAt = DateTime.UtcNow.AddHours(-1);
        var createdAt = DateTime.UtcNow.AddDays(-1);
        var updatedAt = DateTime.UtcNow.AddMinutes(-5);

        // Act
        var addon = new Addon(id, metadata, installPath, isSelected, discoveredAt, createdAt, updatedAt);

        // Assert
        Assert.Equal(id, addon.Id);
        Assert.Equal(metadata, addon.Metadata);
        Assert.Equal(installPath, addon.InstallPath);
        Assert.True(addon.IsSelected);
        Assert.Equal(discoveredAt, addon.DiscoveredAt);
        Assert.Equal(createdAt, addon.CreatedAt);
        Assert.Equal(updatedAt, addon.UpdatedAt);
    }

    [Fact]
    public void Constructor_WithNullMetadata_ThrowsArgumentNullException()
    {
        // Arrange
        AddonMetadata? nullMetadata = null;
        const string installPath = "C:\\MSFS\\Community\\TestAddon";
        var discoveredAt = DateTime.UtcNow;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new Addon(nullMetadata!, installPath, discoveredAt));

        Assert.Equal("metadata", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithNullOrWhiteSpaceInstallPath_ThrowsArgumentException(string? invalidPath)
    {
        // Arrange
        var metadata = CreateValidMetadata();
        var discoveredAt = DateTime.UtcNow;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Addon(metadata, invalidPath!, discoveredAt));

        Assert.Equal("installPath", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithEmptyGuid_ThrowsArgumentException()
    {
        // Arrange
        var emptyId = Guid.Empty;
        var metadata = CreateValidMetadata();
        const string installPath = "C:\\MSFS\\Community\\TestAddon";
        var discoveredAt = DateTime.UtcNow;
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new Addon(emptyId, metadata, installPath, false, discoveredAt, createdAt, updatedAt));

        Assert.Equal("id", exception.ParamName);
    }

    [Fact]
    public void Select_WhenNotSelected_SetsIsSelectedToTrueAndUpdatesTimestamp()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        var addon = new Addon(metadata, "C:\\Test", DateTime.UtcNow);
        var originalUpdatedAt = addon.UpdatedAt;

        // Small delay to ensure timestamp changes
        Thread.Sleep(10);

        // Act
        addon.Select();

        // Assert
        Assert.True(addon.IsSelected);
        Assert.True(addon.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void Select_WhenAlreadySelected_DoesNotChangeTimestamp()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        var addon = new Addon(metadata, "C:\\Test", DateTime.UtcNow);
        addon.Select();
        var updatedAtAfterFirstSelect = addon.UpdatedAt;

        // Small delay to ensure we could detect a timestamp change
        Thread.Sleep(10);

        // Act
        addon.Select();

        // Assert
        Assert.True(addon.IsSelected);
        Assert.Equal(updatedAtAfterFirstSelect, addon.UpdatedAt);
    }

    [Fact]
    public void Deselect_WhenSelected_SetsIsSelectedToFalseAndUpdatesTimestamp()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        var addon = new Addon(metadata, "C:\\Test", DateTime.UtcNow);
        addon.Select();
        var originalUpdatedAt = addon.UpdatedAt;

        // Small delay to ensure timestamp changes
        Thread.Sleep(10);

        // Act
        addon.Deselect();

        // Assert
        Assert.False(addon.IsSelected);
        Assert.True(addon.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void Deselect_WhenNotSelected_DoesNotChangeTimestamp()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        var addon = new Addon(metadata, "C:\\Test", DateTime.UtcNow);
        var originalUpdatedAt = addon.UpdatedAt;

        // Small delay to ensure we could detect a timestamp change
        Thread.Sleep(10);

        // Act
        addon.Deselect();

        // Assert
        Assert.False(addon.IsSelected);
        Assert.Equal(originalUpdatedAt, addon.UpdatedAt);
    }

    [Fact]
    public void ToggleSelection_WhenNotSelected_SetsToSelected()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        var addon = new Addon(metadata, "C:\\Test", DateTime.UtcNow);
        var originalUpdatedAt = addon.UpdatedAt;

        // Small delay to ensure timestamp changes
        Thread.Sleep(10);

        // Act
        addon.ToggleSelection();

        // Assert
        Assert.True(addon.IsSelected);
        Assert.True(addon.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void ToggleSelection_WhenSelected_SetsToNotSelected()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        var addon = new Addon(metadata, "C:\\Test", DateTime.UtcNow);
        addon.Select();
        var updatedAtAfterSelect = addon.UpdatedAt;

        // Small delay to ensure timestamp changes
        Thread.Sleep(10);

        // Act
        addon.ToggleSelection();

        // Assert
        Assert.False(addon.IsSelected);
        Assert.True(addon.UpdatedAt > updatedAtAfterSelect);
    }

    [Fact]
    public void ToggleSelection_CalledTwice_ReturnsToOriginalState()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        var addon = new Addon(metadata, "C:\\Test", DateTime.UtcNow);
        var originalState = addon.IsSelected;

        // Act
        addon.ToggleSelection();
        addon.ToggleSelection();

        // Assert
        Assert.Equal(originalState, addon.IsSelected);
    }

    [Fact]
    public void Equals_WithSameId_ReturnsTrue()
    {
        // Arrange
        var id = Guid.NewGuid();
        var metadata1 = CreateValidMetadata();
        var metadata2 = new AddonMetadata(
            "Different Title",
            "Different Creator",
            "2.0.0",
            ContentType.Scenery,
            "2.0.0",
            "1.35.0");

        var addon1 = new Addon(id, metadata1, "C:\\Path1", false, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);
        var addon2 = new Addon(id, metadata2, "C:\\Path2", true, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);

        // Act & Assert
        Assert.True(addon1.Equals(addon2));
        Assert.Equal(addon1, addon2);
    }

    [Fact]
    public void Equals_WithDifferentIds_ReturnsFalse()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        var addon1 = new Addon(metadata, "C:\\Test", DateTime.UtcNow);
        var addon2 = new Addon(metadata, "C:\\Test", DateTime.UtcNow);

        // Act & Assert
        Assert.False(addon1.Equals(addon2));
        Assert.NotEqual(addon1, addon2);
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        var addon = new Addon(metadata, "C:\\Test", DateTime.UtcNow);

        // Act & Assert
        Assert.False(addon.Equals(null));
    }

    [Fact]
    public void Equals_WithSameReference_ReturnsTrue()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        var addon = new Addon(metadata, "C:\\Test", DateTime.UtcNow);

        // Act & Assert
        Assert.True(addon.Equals(addon));
    }

    [Fact]
    public void GetHashCode_WithSameId_ReturnsSameHashCode()
    {
        // Arrange
        var id = Guid.NewGuid();
        var metadata = CreateValidMetadata();
        var addon1 = new Addon(id, metadata, "C:\\Path1", false, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);
        var addon2 = new Addon(id, metadata, "C:\\Path2", true, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);

        // Act & Assert
        Assert.Equal(addon1.GetHashCode(), addon2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_WithDifferentIds_ReturnsDifferentHashCodes()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        var addon1 = new Addon(metadata, "C:\\Test", DateTime.UtcNow);
        var addon2 = new Addon(metadata, "C:\\Test", DateTime.UtcNow);

        // Act & Assert
        Assert.NotEqual(addon1.GetHashCode(), addon2.GetHashCode());
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        const string installPath = "C:\\MSFS\\Community\\TestAddon";
        var addon = new Addon(metadata, installPath, DateTime.UtcNow);

        // Act
        var result = addon.ToString();

        // Assert
        Assert.Contains("Test Addon", result);
        Assert.Contains("1.0.0", result);
        Assert.Contains("Aircraft", result);
        Assert.Contains(installPath, result);
    }

    [Fact]
    public void Id_GeneratedByConstructor_IsNotEmpty()
    {
        // Arrange
        var metadata = CreateValidMetadata();

        // Act
        var addon = new Addon(metadata, "C:\\Test", DateTime.UtcNow);

        // Assert
        Assert.NotEqual(Guid.Empty, addon.Id);
    }

    [Fact]
    public void CreatedAt_SetByConstructor_IsUtcNow()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        var before = DateTime.UtcNow;

        // Act
        var addon = new Addon(metadata, "C:\\Test", DateTime.UtcNow);

        // Assert
        var after = DateTime.UtcNow;
        Assert.InRange(addon.CreatedAt, before, after);
    }

    [Fact]
    public void UpdatedAt_SetByConstructor_IsUtcNow()
    {
        // Arrange
        var metadata = CreateValidMetadata();
        var before = DateTime.UtcNow;

        // Act
        var addon = new Addon(metadata, "C:\\Test", DateTime.UtcNow);

        // Assert
        var after = DateTime.UtcNow;
        Assert.InRange(addon.UpdatedAt, before, after);
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
        // Arrange
        var metadata = new AddonMetadata(
            "Test",
            "Creator",
            "1.0.0",
            contentType,
            "1.0.0",
            "1.30.0");

        // Act
        var addon = new Addon(metadata, "C:\\Test", DateTime.UtcNow);

        // Assert
        Assert.Equal(contentType, addon.Metadata.ContentType);
    }
}
