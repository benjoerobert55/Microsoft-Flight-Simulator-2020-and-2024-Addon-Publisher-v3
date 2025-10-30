using MSFSAddonPublisher.Domain.Aggregates;
using MSFSAddonPublisher.Domain.Entities;
using MSFSAddonPublisher.Domain.Enums;
using MSFSAddonPublisher.Domain.ValueObjects;

namespace MSFSAddonPublisher.Tests.Unit.Domain.Aggregates;

/// <summary>
/// Unit tests for the AddonCatalog aggregate root.
/// Tests aggregate behavior, business rules, state management, and validation.
/// </summary>
public class AddonCatalogTests
{
    private static AddonMetadata CreateValidMetadata(string title = "Test Addon", ContentType contentType = ContentType.Aircraft)
    {
        return new AddonMetadata(
            title,
            "Test Creator",
            "1.0.0",
            contentType,
            "1.0.0",
            "1.30.0");
    }

    private static Addon CreateTestAddon(string title = "Test Addon", ContentType contentType = ContentType.Aircraft)
    {
        var metadata = CreateValidMetadata(title, contentType);
        return new Addon(metadata, $"C:\\MSFS\\Community\\{title}", DateTime.UtcNow);
    }

    [Fact]
    public void Constructor_WithoutParameters_CreatesEmptyCatalog()
    {
        // Act
        var catalog = new AddonCatalog();

        // Assert
        Assert.NotEqual(Guid.Empty, catalog.Id);
        Assert.Equal(0, catalog.Count);
        Assert.True((DateTime.UtcNow - catalog.CreatedAt).TotalSeconds < 1);
        Assert.True((DateTime.UtcNow - catalog.UpdatedAt).TotalSeconds < 1);
    }

    [Fact]
    public void Constructor_WithFullParameters_CreatesInstanceWithSpecifiedValues()
    {
        // Arrange
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow.AddDays(-1);
        var updatedAt = DateTime.UtcNow.AddMinutes(-5);
        var addon = CreateTestAddon();
        var addons = new Dictionary<Guid, Addon> { { addon.Id, addon } };

        // Act
        var catalog = new AddonCatalog(id, createdAt, updatedAt, addons);

        // Assert
        Assert.Equal(id, catalog.Id);
        Assert.Equal(createdAt, catalog.CreatedAt);
        Assert.Equal(updatedAt, catalog.UpdatedAt);
        Assert.Equal(1, catalog.Count);
    }

    [Fact]
    public void Constructor_WithEmptyGuid_ThrowsArgumentException()
    {
        // Arrange
        var emptyId = Guid.Empty;
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow;
        var addons = new Dictionary<Guid, Addon>();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            new AddonCatalog(emptyId, createdAt, updatedAt, addons));

        Assert.Equal("id", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullAddonsCollection_ThrowsArgumentNullException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;
        var updatedAt = DateTime.UtcNow;
        Dictionary<Guid, Addon>? nullAddons = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            new AddonCatalog(id, createdAt, updatedAt, nullAddons!));

        Assert.Equal("addons", exception.ParamName);
    }

    [Fact]
    public void AddAddon_WithValidAddon_IncreasesCount()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon = CreateTestAddon();

        // Act
        catalog.AddAddon(addon);

        // Assert
        Assert.Equal(1, catalog.Count);
        Assert.True(catalog.ContainsAddon(addon.Id));
    }

    [Fact]
    public void AddAddon_WithNullAddon_ThrowsArgumentNullException()
    {
        // Arrange
        var catalog = new AddonCatalog();
        Addon? nullAddon = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() =>
            catalog.AddAddon(nullAddon!));

        Assert.Equal("addon", exception.ParamName);
    }

    [Fact]
    public void AddAddon_WithDuplicateId_ReplacesExistingAddon()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon1 = CreateTestAddon("First Addon");
        var metadata2 = CreateValidMetadata("Second Addon");
        var addon2 = new Addon(addon1.Id, metadata2, "C:\\Different\\Path", false, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow);

        // Act
        catalog.AddAddon(addon1);
        catalog.AddAddon(addon2);

        // Assert
        Assert.Equal(1, catalog.Count);
        var retrieved = catalog.GetAddonById(addon1.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("Second Addon", retrieved.Metadata.Title);
    }

    [Fact]
    public void AddAddon_UpdatesTimestamp()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var originalUpdatedAt = catalog.UpdatedAt;
        var addon = CreateTestAddon();

        // Small delay to ensure timestamp changes
        Thread.Sleep(10);

        // Act
        catalog.AddAddon(addon);

        // Assert
        Assert.True(catalog.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void RemoveAddon_WithExistingId_RemovesAddonAndReturnsTrue()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon = CreateTestAddon();
        catalog.AddAddon(addon);

        // Act
        var result = catalog.RemoveAddon(addon.Id);

        // Assert
        Assert.True(result);
        Assert.Equal(0, catalog.Count);
        Assert.False(catalog.ContainsAddon(addon.Id));
    }

    [Fact]
    public void RemoveAddon_WithNonExistentId_ReturnsFalse()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = catalog.RemoveAddon(nonExistentId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void RemoveAddon_UpdatesTimestampWhenSuccessful()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon = CreateTestAddon();
        catalog.AddAddon(addon);
        var originalUpdatedAt = catalog.UpdatedAt;

        // Small delay to ensure timestamp changes
        Thread.Sleep(10);

        // Act
        catalog.RemoveAddon(addon.Id);

        // Assert
        Assert.True(catalog.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void RemoveAddon_DoesNotUpdateTimestampWhenNotFound()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var originalUpdatedAt = catalog.UpdatedAt;

        // Small delay to ensure we could detect a timestamp change
        Thread.Sleep(10);

        // Act
        catalog.RemoveAddon(Guid.NewGuid());

        // Assert
        Assert.Equal(originalUpdatedAt, catalog.UpdatedAt);
    }

    [Fact]
    public void GetSelectedAddons_ReturnsOnlySelectedAddons()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon1 = CreateTestAddon("Addon 1");
        var addon2 = CreateTestAddon("Addon 2");
        var addon3 = CreateTestAddon("Addon 3");

        catalog.AddAddon(addon1);
        catalog.AddAddon(addon2);
        catalog.AddAddon(addon3);

        addon1.Select();
        addon3.Select();

        // Act
        var selectedAddons = catalog.GetSelectedAddons();

        // Assert
        Assert.Equal(2, selectedAddons.Count);
        Assert.Contains(selectedAddons, a => a.Id == addon1.Id);
        Assert.Contains(selectedAddons, a => a.Id == addon3.Id);
        Assert.DoesNotContain(selectedAddons, a => a.Id == addon2.Id);
    }

    [Fact]
    public void GetSelectedAddons_WithNoSelection_ReturnsEmptyCollection()
    {
        // Arrange
        var catalog = new AddonCatalog();
        catalog.AddAddon(CreateTestAddon("Addon 1"));
        catalog.AddAddon(CreateTestAddon("Addon 2"));

        // Act
        var selectedAddons = catalog.GetSelectedAddons();

        // Assert
        Assert.Empty(selectedAddons);
    }

    [Fact]
    public void ClearSelection_DeselectsAllAddons()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon1 = CreateTestAddon("Addon 1");
        var addon2 = CreateTestAddon("Addon 2");
        var addon3 = CreateTestAddon("Addon 3");

        catalog.AddAddon(addon1);
        catalog.AddAddon(addon2);
        catalog.AddAddon(addon3);

        addon1.Select();
        addon2.Select();
        addon3.Select();

        // Act
        catalog.ClearSelection();

        // Assert
        Assert.False(addon1.IsSelected);
        Assert.False(addon2.IsSelected);
        Assert.False(addon3.IsSelected);
        Assert.Empty(catalog.GetSelectedAddons());
    }

    [Fact]
    public void ClearSelection_UpdatesTimestampWhenAddonsAreDeselected()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon = CreateTestAddon();
        catalog.AddAddon(addon);
        addon.Select();
        var originalUpdatedAt = catalog.UpdatedAt;

        // Small delay to ensure timestamp changes
        Thread.Sleep(10);

        // Act
        catalog.ClearSelection();

        // Assert
        Assert.True(catalog.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void ClearSelection_DoesNotUpdateTimestampWhenNoAddonsAreSelected()
    {
        // Arrange
        var catalog = new AddonCatalog();
        catalog.AddAddon(CreateTestAddon());
        var originalUpdatedAt = catalog.UpdatedAt;

        // Small delay to ensure we could detect a timestamp change
        Thread.Sleep(10);

        // Act
        catalog.ClearSelection();

        // Assert
        Assert.Equal(originalUpdatedAt, catalog.UpdatedAt);
    }

    [Fact]
    public void SelectAll_SelectsAllAddons()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon1 = CreateTestAddon("Addon 1");
        var addon2 = CreateTestAddon("Addon 2");
        var addon3 = CreateTestAddon("Addon 3");

        catalog.AddAddon(addon1);
        catalog.AddAddon(addon2);
        catalog.AddAddon(addon3);

        // Act
        catalog.SelectAll();

        // Assert
        Assert.True(addon1.IsSelected);
        Assert.True(addon2.IsSelected);
        Assert.True(addon3.IsSelected);
        Assert.Equal(3, catalog.GetSelectedAddons().Count);
    }

    [Fact]
    public void SelectAll_UpdatesTimestampWhenAddonsAreSelected()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon = CreateTestAddon();
        catalog.AddAddon(addon);
        var originalUpdatedAt = catalog.UpdatedAt;

        // Small delay to ensure timestamp changes
        Thread.Sleep(10);

        // Act
        catalog.SelectAll();

        // Assert
        Assert.True(catalog.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void SelectAll_DoesNotUpdateTimestampWhenAllAddonsAlreadySelected()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon = CreateTestAddon();
        catalog.AddAddon(addon);
        addon.Select();
        var originalUpdatedAt = catalog.UpdatedAt;

        // Small delay to ensure we could detect a timestamp change
        Thread.Sleep(10);

        // Act
        catalog.SelectAll();

        // Assert
        Assert.Equal(originalUpdatedAt, catalog.UpdatedAt);
    }

    [Fact]
    public void GetAddonsByType_ReturnsOnlyMatchingContentType()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var aircraft1 = CreateTestAddon("Aircraft 1", ContentType.Aircraft);
        var aircraft2 = CreateTestAddon("Aircraft 2", ContentType.Aircraft);
        var scenery = CreateTestAddon("Scenery 1", ContentType.Scenery);
        var livery = CreateTestAddon("Livery 1", ContentType.Livery);

        catalog.AddAddon(aircraft1);
        catalog.AddAddon(aircraft2);
        catalog.AddAddon(scenery);
        catalog.AddAddon(livery);

        // Act
        var aircraftAddons = catalog.GetAddonsByType(ContentType.Aircraft);

        // Assert
        Assert.Equal(2, aircraftAddons.Count);
        Assert.All(aircraftAddons, addon => Assert.Equal(ContentType.Aircraft, addon.Metadata.ContentType));
    }

    [Fact]
    public void GetAddonsByType_WithNoMatchingType_ReturnsEmptyCollection()
    {
        // Arrange
        var catalog = new AddonCatalog();
        catalog.AddAddon(CreateTestAddon("Aircraft 1", ContentType.Aircraft));
        catalog.AddAddon(CreateTestAddon("Scenery 1", ContentType.Scenery));

        // Act
        var missions = catalog.GetAddonsByType(ContentType.Mission);

        // Assert
        Assert.Empty(missions);
    }

    [Theory]
    [InlineData(ContentType.Aircraft)]
    [InlineData(ContentType.Scenery)]
    [InlineData(ContentType.SimObject)]
    [InlineData(ContentType.Livery)]
    [InlineData(ContentType.Mission)]
    [InlineData(ContentType.Unknown)]
    public void GetAddonsByType_WithAllContentTypes_FiltersCorrectly(ContentType contentType)
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon = CreateTestAddon("Test", contentType);
        catalog.AddAddon(addon);

        // Act
        var addons = catalog.GetAddonsByType(contentType);

        // Assert
        Assert.Single(addons);
        Assert.Equal(contentType, addons.First().Metadata.ContentType);
    }

    [Fact]
    public void GetAllAddons_ReturnsAllAddonsInCatalog()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon1 = CreateTestAddon("Addon 1");
        var addon2 = CreateTestAddon("Addon 2");
        var addon3 = CreateTestAddon("Addon 3");

        catalog.AddAddon(addon1);
        catalog.AddAddon(addon2);
        catalog.AddAddon(addon3);

        // Act
        var allAddons = catalog.GetAllAddons();

        // Assert
        Assert.Equal(3, allAddons.Count);
        Assert.Contains(allAddons, a => a.Id == addon1.Id);
        Assert.Contains(allAddons, a => a.Id == addon2.Id);
        Assert.Contains(allAddons, a => a.Id == addon3.Id);
    }

    [Fact]
    public void GetAllAddons_WithEmptyCatalog_ReturnsEmptyCollection()
    {
        // Arrange
        var catalog = new AddonCatalog();

        // Act
        var allAddons = catalog.GetAllAddons();

        // Assert
        Assert.Empty(allAddons);
    }

    [Fact]
    public void GetAddonById_WithExistingId_ReturnsAddon()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon = CreateTestAddon("Test Addon");
        catalog.AddAddon(addon);

        // Act
        var retrieved = catalog.GetAddonById(addon.Id);

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal(addon.Id, retrieved.Id);
        Assert.Equal(addon.Metadata.Title, retrieved.Metadata.Title);
    }

    [Fact]
    public void GetAddonById_WithNonExistentId_ReturnsNull()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var nonExistentId = Guid.NewGuid();

        // Act
        var retrieved = catalog.GetAddonById(nonExistentId);

        // Assert
        Assert.Null(retrieved);
    }

    [Fact]
    public void ContainsAddon_WithExistingId_ReturnsTrue()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon = CreateTestAddon();
        catalog.AddAddon(addon);

        // Act
        var contains = catalog.ContainsAddon(addon.Id);

        // Assert
        Assert.True(contains);
    }

    [Fact]
    public void ContainsAddon_WithNonExistentId_ReturnsFalse()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var nonExistentId = Guid.NewGuid();

        // Act
        var contains = catalog.ContainsAddon(nonExistentId);

        // Assert
        Assert.False(contains);
    }

    [Fact]
    public void Clear_RemovesAllAddons()
    {
        // Arrange
        var catalog = new AddonCatalog();
        catalog.AddAddon(CreateTestAddon("Addon 1"));
        catalog.AddAddon(CreateTestAddon("Addon 2"));
        catalog.AddAddon(CreateTestAddon("Addon 3"));

        // Act
        catalog.Clear();

        // Assert
        Assert.Equal(0, catalog.Count);
        Assert.Empty(catalog.GetAllAddons());
    }

    [Fact]
    public void Clear_UpdatesTimestampWhenNotEmpty()
    {
        // Arrange
        var catalog = new AddonCatalog();
        catalog.AddAddon(CreateTestAddon());
        var originalUpdatedAt = catalog.UpdatedAt;

        // Small delay to ensure timestamp changes
        Thread.Sleep(10);

        // Act
        catalog.Clear();

        // Assert
        Assert.True(catalog.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void Clear_DoesNotUpdateTimestampWhenAlreadyEmpty()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var originalUpdatedAt = catalog.UpdatedAt;

        // Small delay to ensure we could detect a timestamp change
        Thread.Sleep(10);

        // Act
        catalog.Clear();

        // Assert
        Assert.Equal(originalUpdatedAt, catalog.UpdatedAt);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon1 = CreateTestAddon("Addon 1");
        var addon2 = CreateTestAddon("Addon 2");
        var addon3 = CreateTestAddon("Addon 3");

        catalog.AddAddon(addon1);
        catalog.AddAddon(addon2);
        catalog.AddAddon(addon3);

        addon1.Select();

        // Act
        var result = catalog.ToString();

        // Assert
        Assert.Contains("3 addons", result);
        Assert.Contains("1 selected", result);
    }

    [Fact]
    public void Count_ReflectsCurrentAddonCount()
    {
        // Arrange
        var catalog = new AddonCatalog();

        // Act & Assert
        Assert.Equal(0, catalog.Count);

        catalog.AddAddon(CreateTestAddon("Addon 1"));
        Assert.Equal(1, catalog.Count);

        catalog.AddAddon(CreateTestAddon("Addon 2"));
        Assert.Equal(2, catalog.Count);

        catalog.RemoveAddon(catalog.GetAllAddons().First().Id);
        Assert.Equal(1, catalog.Count);

        catalog.Clear();
        Assert.Equal(0, catalog.Count);
    }

    [Fact]
    public void Constructor_CreatesIndependentCopyOfAddonsCollection()
    {
        // Arrange
        var addon = CreateTestAddon();
        var originalAddons = new Dictionary<Guid, Addon> { { addon.Id, addon } };

        // Act
        var catalog = new AddonCatalog(Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow, originalAddons);
        originalAddons.Clear();

        // Assert
        Assert.Equal(1, catalog.Count);
    }

    [Fact]
    public void GetAllAddons_ReturnsReadOnlyCollection()
    {
        // Arrange
        var catalog = new AddonCatalog();
        catalog.AddAddon(CreateTestAddon());

        // Act
        var allAddons = catalog.GetAllAddons();

        // Assert
        Assert.IsAssignableFrom<IReadOnlyCollection<Addon>>(allAddons);
    }

    [Fact]
    public void GetSelectedAddons_ReturnsReadOnlyCollection()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon = CreateTestAddon();
        catalog.AddAddon(addon);
        addon.Select();

        // Act
        var selectedAddons = catalog.GetSelectedAddons();

        // Assert
        Assert.IsAssignableFrom<IReadOnlyCollection<Addon>>(selectedAddons);
    }

    [Fact]
    public void GetAddonsByType_ReturnsReadOnlyCollection()
    {
        // Arrange
        var catalog = new AddonCatalog();
        catalog.AddAddon(CreateTestAddon("Test", ContentType.Aircraft));

        // Act
        var addons = catalog.GetAddonsByType(ContentType.Aircraft);

        // Assert
        Assert.IsAssignableFrom<IReadOnlyCollection<Addon>>(addons);
    }

    [Fact]
    public void MultipleOperations_MaintainConsistency()
    {
        // Arrange
        var catalog = new AddonCatalog();
        var addon1 = CreateTestAddon("Addon 1", ContentType.Aircraft);
        var addon2 = CreateTestAddon("Addon 2", ContentType.Scenery);
        var addon3 = CreateTestAddon("Addon 3", ContentType.Aircraft);

        // Act
        catalog.AddAddon(addon1);
        catalog.AddAddon(addon2);
        catalog.AddAddon(addon3);

        catalog.SelectAll();
        Assert.Equal(3, catalog.GetSelectedAddons().Count);

        catalog.ClearSelection();
        Assert.Empty(catalog.GetSelectedAddons());

        addon1.Select();
        addon3.Select();
        Assert.Equal(2, catalog.GetSelectedAddons().Count);

        var aircraftAddons = catalog.GetAddonsByType(ContentType.Aircraft);
        Assert.Equal(2, aircraftAddons.Count);

        catalog.RemoveAddon(addon1.Id);
        Assert.Equal(2, catalog.Count);
        Assert.Equal(1, catalog.GetAddonsByType(ContentType.Aircraft).Count);

        // Assert
        Assert.Equal(2, catalog.Count);
        Assert.Single(catalog.GetSelectedAddons());
    }
}
