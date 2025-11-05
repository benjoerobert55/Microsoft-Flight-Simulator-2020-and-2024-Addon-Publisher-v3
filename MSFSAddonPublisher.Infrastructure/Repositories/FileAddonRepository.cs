using System.Text.Json;
using MSFSAddonPublisher.Domain.Entities;
using MSFSAddonPublisher.Domain.Enums;
using MSFSAddonPublisher.Domain.Repositories;
using MSFSAddonPublisher.Domain.ValueObjects;

namespace MSFSAddonPublisher.Infrastructure.Repositories;

/// <summary>
/// File-based implementation of IAddonRepository using JSON for persistence.
/// Stores addons in a JSON file in the local application data directory.
/// </summary>
public sealed class FileAddonRepository : IAddonRepository
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _fileLock = new(1, 1);
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileAddonRepository"/> class.
    /// </summary>
    /// <param name="filePath">The path to the JSON file for storing addons. If null, uses default location.</param>
    public FileAddonRepository(string? filePath = null)
    {
        _filePath = filePath ?? GetDefaultFilePath();
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        EnsureDirectoryExists();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Addon>> GetAllAsync()
    {
        await _fileLock.WaitAsync();
        try
        {
            if (!File.Exists(_filePath))
            {
                return Enumerable.Empty<Addon>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            var dtos = JsonSerializer.Deserialize<List<AddonDto>>(json, _jsonOptions);
            
            return dtos?.Select(MapToEntity) ?? Enumerable.Empty<Addon>();
        }
        finally
        {
            _fileLock.Release();
        }
    }

    /// <inheritdoc/>
    public async Task<Addon?> GetByIdAsync(Guid id)
    {
        var addons = await GetAllAsync();
        return addons.FirstOrDefault(a => a.Id == id);
    }

    /// <inheritdoc/>
    public async Task AddAsync(Addon addon)
    {
        ArgumentNullException.ThrowIfNull(addon);

        await _fileLock.WaitAsync();
        try
        {
            var addons = (await GetAllInternalAsync()).ToList();
            
            if (addons.Any(a => a.Id == addon.Id))
            {
                throw new InvalidOperationException($"Addon with ID {addon.Id} already exists.");
            }

            addons.Add(addon);
            await SaveAllInternalAsync(addons);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Addon addon)
    {
        ArgumentNullException.ThrowIfNull(addon);

        await _fileLock.WaitAsync();
        try
        {
            var addons = (await GetAllInternalAsync()).ToList();
            var index = addons.FindIndex(a => a.Id == addon.Id);

            if (index == -1)
            {
                throw new InvalidOperationException($"Addon with ID {addon.Id} not found.");
            }

            addons[index] = addon;
            await SaveAllInternalAsync(addons);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Guid id)
    {
        await _fileLock.WaitAsync();
        try
        {
            var addons = (await GetAllInternalAsync()).ToList();
            var removed = addons.RemoveAll(a => a.Id == id);

            if (removed == 0)
            {
                throw new InvalidOperationException($"Addon with ID {id} not found.");
            }

            await SaveAllInternalAsync(addons);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    /// <inheritdoc/>
    public async Task<int> CountAsync()
    {
        var addons = await GetAllAsync();
        return addons.Count();
    }

    private async Task<IEnumerable<Addon>> GetAllInternalAsync()
    {
        if (!File.Exists(_filePath))
        {
            return Enumerable.Empty<Addon>();
        }

        var json = await File.ReadAllTextAsync(_filePath);
        var dtos = JsonSerializer.Deserialize<List<AddonDto>>(json, _jsonOptions);
        
        return dtos?.Select(MapToEntity) ?? Enumerable.Empty<Addon>();
    }

    private async Task SaveAllInternalAsync(IEnumerable<Addon> addons)
    {
        var dtos = addons.Select(MapToDto).ToList();
        var json = JsonSerializer.Serialize(dtos, _jsonOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }

    private void EnsureDirectoryExists()
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    private static string GetDefaultFilePath()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolder = Path.Combine(appDataPath, "MSFSAddonPublisher");
        return Path.Combine(appFolder, "addons.json");
    }

    private static Addon MapToEntity(AddonDto dto)
    {
        var releaseNotes = dto.ReleaseNotes ?? new Dictionary<string, string>();
        var metadata = new AddonMetadata(
            dto.Title,
            dto.Creator,
            dto.Version,
            Enum.Parse<ContentType>(dto.ContentType),
            dto.PackageVersion,
            dto.MinimumGameVersion,
            releaseNotes);

        return new Addon(
            dto.Id,
            metadata,
            dto.InstallPath,
            dto.IsSelected,
            dto.DiscoveredAt,
            dto.CreatedAt,
            dto.UpdatedAt);
    }

    private static AddonDto MapToDto(Addon addon)
    {
        return new AddonDto
        {
            Id = addon.Id,
            Title = addon.Metadata.Title,
            Creator = addon.Metadata.Creator,
            Version = addon.Metadata.Version,
            ContentType = addon.Metadata.ContentType.ToString(),
            PackageVersion = addon.Metadata.PackageVersion,
            MinimumGameVersion = addon.Metadata.MinimumGameVersion,
            ReleaseNotes = addon.Metadata.ReleaseNotes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            InstallPath = addon.InstallPath,
            IsSelected = addon.IsSelected,
            DiscoveredAt = addon.DiscoveredAt,
            CreatedAt = addon.CreatedAt,
            UpdatedAt = addon.UpdatedAt
        };
    }

    /// <summary>
    /// Data transfer object for JSON serialization of Addon entities.
    /// </summary>
    private sealed class AddonDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Creator { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public string PackageVersion { get; set; } = string.Empty;
        public string MinimumGameVersion { get; set; } = string.Empty;
        public Dictionary<string, string>? ReleaseNotes { get; set; }
        public string InstallPath { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
        public DateTime DiscoveredAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
