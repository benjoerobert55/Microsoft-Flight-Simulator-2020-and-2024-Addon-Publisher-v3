using MSFSAddonPublisher.Domain.Entities;
using MSFSAddonPublisher.Domain.Enums;
using MSFSAddonPublisher.Domain.ValueObjects;
using MSFSAddonPublisher.Infrastructure.Repositories;

namespace MSFSAddonPublisher.Tests.Unit.Infrastructure.Repositories;

public sealed class FileAddonRepositoryTests : IDisposable
{
    private readonly string _testFilePath;
    private readonly FileAddonRepository _repository;

    public FileAddonRepositoryTests()
    {
        _testFilePath = Path.Combine(Path.GetTempPath(), $"test_addons_{Guid.NewGuid()}.json");
        _repository = new FileAddonRepository(_testFilePath);
    }

    public void Dispose()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [Fact(DisplayName = "GetAllAsync returns empty collection when no addons exist")]
    public async Task GetAllAsync_NoAddons_ReturnsEmptyCollection()
    {
        var result = await _repository.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact(DisplayName = "AddAsync adds addon successfully")]
    public async Task AddAsync_ValidAddon_AddsSuccessfully()
    {
        var addon = CreateTestAddon();

        await _repository.AddAsync(addon);

        var allAddons = await _repository.GetAllAsync();
        Assert.Single(allAddons);
        Assert.Contains(allAddons, a => a.Id == addon.Id);
    }

    [Fact(DisplayName = "AddAsync throws ArgumentNullException when addon is null")]
    public async Task AddAsync_NullAddon_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _repository.AddAsync(null!));
    }

    [Fact(DisplayName = "AddAsync throws InvalidOperationException when addon already exists")]
    public async Task AddAsync_DuplicateAddon_ThrowsInvalidOperationException()
    {
        var addon = CreateTestAddon();
        await _repository.AddAsync(addon);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _repository.AddAsync(addon));
    }

    [Fact(DisplayName = "GetByIdAsync returns addon when it exists")]
    public async Task GetByIdAsync_ExistingAddon_ReturnsAddon()
    {
        var addon = CreateTestAddon();
        await _repository.AddAsync(addon);

        var result = await _repository.GetByIdAsync(addon.Id);

        Assert.NotNull(result);
        Assert.Equal(addon.Id, result.Id);
        Assert.Equal(addon.Metadata.Title, result.Metadata.Title);
    }

    [Fact(DisplayName = "GetByIdAsync returns null when addon does not exist")]
    public async Task GetByIdAsync_NonExistentAddon_ReturnsNull()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact(DisplayName = "UpdateAsync updates addon successfully")]
    public async Task UpdateAsync_ExistingAddon_UpdatesSuccessfully()
    {
        var addon = CreateTestAddon();
        await _repository.AddAsync(addon);

        addon.Select();
        await _repository.UpdateAsync(addon);

        var updated = await _repository.GetByIdAsync(addon.Id);
        Assert.NotNull(updated);
        Assert.True(updated.IsSelected);
    }

    [Fact(DisplayName = "UpdateAsync throws ArgumentNullException when addon is null")]
    public async Task UpdateAsync_NullAddon_ThrowsArgumentNullException()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => _repository.UpdateAsync(null!));
    }

    [Fact(DisplayName = "UpdateAsync throws InvalidOperationException when addon does not exist")]
    public async Task UpdateAsync_NonExistentAddon_ThrowsInvalidOperationException()
    {
        var addon = CreateTestAddon();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _repository.UpdateAsync(addon));
    }

    [Fact(DisplayName = "DeleteAsync removes addon successfully")]
    public async Task DeleteAsync_ExistingAddon_RemovesSuccessfully()
    {
        var addon = CreateTestAddon();
        await _repository.AddAsync(addon);

        await _repository.DeleteAsync(addon.Id);

        var allAddons = await _repository.GetAllAsync();
        Assert.Empty(allAddons);
    }

    [Fact(DisplayName = "DeleteAsync throws InvalidOperationException when addon does not exist")]
    public async Task DeleteAsync_NonExistentAddon_ThrowsInvalidOperationException()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _repository.DeleteAsync(Guid.NewGuid()));
    }

    [Fact(DisplayName = "CountAsync returns correct count")]
    public async Task CountAsync_WithAddons_ReturnsCorrectCount()
    {
        await _repository.AddAsync(CreateTestAddon());
        await _repository.AddAsync(CreateTestAddon());
        await _repository.AddAsync(CreateTestAddon());

        var count = await _repository.CountAsync();

        Assert.Equal(3, count);
    }

    [Fact(DisplayName = "CountAsync returns zero when no addons exist")]
    public async Task CountAsync_NoAddons_ReturnsZero()
    {
        var count = await _repository.CountAsync();

        Assert.Equal(0, count);
    }

    [Fact(DisplayName = "Repository persists data across multiple instances")]
    public async Task Repository_MultipleInstances_PersistsDataCorrectly()
    {
        var addon = CreateTestAddon();
        await _repository.AddAsync(addon);

        var newRepository = new FileAddonRepository(_testFilePath);
        var result = await newRepository.GetByIdAsync(addon.Id);

        Assert.NotNull(result);
        Assert.Equal(addon.Id, result.Id);
        Assert.Equal(addon.Metadata.Title, result.Metadata.Title);
    }

    [Fact(DisplayName = "Repository handles concurrent operations safely")]
    public async Task Repository_ConcurrentOperations_HandledSafely()
    {
        var tasks = Enumerable.Range(0, 10)
            .Select(_ => _repository.AddAsync(CreateTestAddon()))
            .ToList();

        await Task.WhenAll(tasks);

        var count = await _repository.CountAsync();
        Assert.Equal(10, count);
    }

    [Fact(DisplayName = "Repository creates directory if it does not exist")]
    public async Task Repository_DirectoryNotExists_CreatesDirectory()
    {
        var testDir = Path.Combine(Path.GetTempPath(), $"test_dir_{Guid.NewGuid()}");
        var testFile = Path.Combine(testDir, "addons.json");

        try
        {
            var repository = new FileAddonRepository(testFile);
            await repository.AddAsync(CreateTestAddon());

            Assert.True(Directory.Exists(testDir));
            Assert.True(File.Exists(testFile));
        }
        finally
        {
            if (Directory.Exists(testDir))
            {
                Directory.Delete(testDir, true);
            }
        }
    }

    private static Addon CreateTestAddon()
    {
        var metadata = new AddonMetadata(
            title: $"Test Addon {Guid.NewGuid()}",
            creator: "Test Creator",
            version: "1.0.0",
            contentType: ContentType.Aircraft,
            packageVersion: "1.0.0",
            minimumGameVersion: "1.0.0",
            releaseNotes: new Dictionary<string, string> { { "en-US", "Test release" } });

        return new Addon(
            metadata,
            $"C:\\Test\\Path\\{Guid.NewGuid()}",
            DateTime.UtcNow);
    }
}
