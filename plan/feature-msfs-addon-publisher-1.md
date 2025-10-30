---
goal: Build MSFS Addon Publisher Windows Forms Application
version: 1.0
date_created: 2025-10-30
last_updated: 2025-10-30
owner: SimulationBuff
status: Planned
tags: [feature, architecture, windows-forms, dotnet, msfs]
---

# Introduction

![Status: Planned](https://img.shields.io/badge/status-Planned-blue)

This implementation plan defines the complete architecture and development roadmap for the Microsoft Flight Simulator Addon Publisher application. The application is a .NET/C# Windows Forms desktop tool that scans, catalogs, and publishes locally installed MSFS 2020/2024 addons to multiple platforms including Twitch, Discord, and other extensible targets. The solution prioritizes security, performance, maintainability, and follows industry best practices including DDD, SOLID principles, and secure coding standards.

## 1. Requirements & Constraints

### Functional Requirements

- **REQ-001**: Application must scan and detect Microsoft Flight Simulator 2020 and 2024 addon installations from standard installation directories
- **REQ-002**: Application must parse addon manifest files (manifest.json) and extract metadata including title, creator, version, type, and description
- **REQ-003**: Application must display discovered addons in a sortable, filterable DataGridView with columns for Name, Version, Type, Creator, and Installation Path
- **REQ-004**: Application must support publishing addon lists to Discord via webhooks with formatted embed messages
- **REQ-005**: Application must support publishing addon lists to Twitch chat via IRC or API integration
- **REQ-006**: Application must provide an extensible plugin architecture for adding new publishing platforms without modifying core code
- **REQ-007**: Application must cache addon discovery results and provide manual refresh capability
- **REQ-008**: Application must allow users to select/deselect individual addons before publishing
- **REQ-009**: Application must save user preferences including platform credentials, selected MSFS version, and UI state
- **REQ-010**: Application must provide real-time status updates during scanning and publishing operations

### Security Requirements

- **SEC-001**: All API keys, tokens, and credentials must be encrypted at rest using Windows Data Protection API (DPAPI)
- **SEC-002**: Application must validate all file paths to prevent directory traversal attacks
- **SEC-003**: Application must sanitize user input before constructing URLs or API requests to prevent injection attacks
- **SEC-004**: Application must use HTTPS for all external API communications
- **SEC-005**: Application must implement rate limiting for API calls to prevent abuse
- **SEC-006**: Application must validate SSL certificates for all external connections
- **SEC-007**: Application must log security-relevant events (failed authentication, invalid paths) without exposing sensitive data
- **SEC-008**: Application must implement principle of least privilege for file system access

### Performance Requirements

- **PER-001**: Addon scanning must complete within 5 seconds for up to 500 addons
- **PER-002**: UI must remain responsive during background operations using async/await patterns
- **PER-003**: Memory usage must not exceed 200MB under normal operation
- **PER-004**: Application startup time must be under 2 seconds
- **PER-005**: Publishing operations must be cancellable by user without blocking UI

### Technical Constraints

- **CON-001**: Application must target .NET 8.0 or later
- **CON-002**: Application must use Windows Forms for UI (no WPF or other frameworks)
- **CON-003**: Application must support Windows 10 version 1809 or later
- **CON-004**: Application must be deployable as a single-file executable with ClickOnce or MSIX installer
- **CON-005**: Application must not require administrator privileges for normal operation
- **CON-006**: Application must handle missing or malformed manifest.json files gracefully
- **CON-007**: Application must support both Microsoft Store and Steam installation paths for MSFS

### Architecture Guidelines

- **GUD-001**: Follow Domain-Driven Design (DDD) principles with clear separation of Domain, Application, and Infrastructure layers
- **GUD-002**: Implement SOLID principles throughout the codebase
- **GUD-003**: Use Repository pattern for addon data access
- **GUD-004**: Use Strategy pattern for platform-specific publishing implementations
- **GUD-005**: Use Observer pattern for progress updates and event notifications
- **GUD-006**: Implement comprehensive logging using Serilog with structured logging
- **GUD-007**: Follow C# 13 coding standards and conventions
- **GUD-008**: Implement async/await for all I/O-bound operations
- **GUD-009**: Use dependency injection for all service dependencies
- **GUD-010**: Write unit tests with minimum 80% code coverage for domain and application layers

### MSFS Addon Structure Patterns

- **PAT-001**: MSFS addons are packaged as folders containing a manifest.json file at the root
- **PAT-002**: manifest.json structure includes: `{ "content_type": "AIRCRAFT|SCENERY|SIMOBJECT", "title": "string", "manufacturer": "string", "creator": "string", "package_version": "string", "minimum_game_version": "string", "release_notes": {} }`
- **PAT-003**: Default MSFS 2020 installation paths: `C:\Users\{username}\AppData\Local\Packages\Microsoft.FlightSimulator_*\LocalCache\Packages\` (Store), `C:\Users\{username}\AppData\Roaming\Microsoft Flight Simulator\Packages\` (Steam)
- **PAT-004**: Default MSFS 2024 installation paths: `C:\Users\{username}\AppData\Local\Packages\Microsoft.Limitless_*\LocalCache\Packages\` (Store), `C:\Users\{username}\AppData\Roaming\Microsoft Flight Simulator 2024\Packages\` (Steam)
- **PAT-005**: Community addons typically located in `Community\` subfolder within Packages directory
- **PAT-006**: Official marketplace addons located in `Official\` subfolder
- **PAT-007**: Addon folder names often contain GUIDs or publisher-specific identifiers

## 2. Implementation Steps

### Implementation Phase 1: Project Infrastructure and Domain Layer

**GOAL-001**: Establish project structure, configure build pipeline, implement core domain models and business logic following DDD principles

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-001 | Create solution file `MSFSAddonPublisher.sln` with project structure: `MSFSAddonPublisher.Domain`, `MSFSAddonPublisher.Application`, `MSFSAddonPublisher.Infrastructure`, `MSFSAddonPublisher.UI`, `MSFSAddonPublisher.Tests.Unit`, `MSFSAddonPublisher.Tests.Integration` | | |
| TASK-002 | Configure .editorconfig with C# 13 formatting rules, nullable reference types enabled, and code analysis ruleset | | |
| TASK-003 | Create Directory.Build.props with common properties: TargetFramework=net8.0-windows, LangVersion=13, Nullable=enable, TreatWarningsAsErrors=true | | |
| TASK-004 | Implement Domain layer value object `AddonMetadata` in file `src/MSFSAddonPublisher.Domain/ValueObjects/AddonMetadata.cs` with properties: Title (string), Creator (string), Version (string), ContentType (enum), PackageVersion (string), MinimumGameVersion (string), ReleaseNotes (Dictionary<string,string>) | | |
| TASK-005 | Implement Domain layer enum `ContentType` in file `src/MSFSAddonPublisher.Domain/Enums/ContentType.cs` with values: Aircraft, Scenery, SimObject, Livery, Mission, Unknown | | |
| TASK-006 | Implement Domain layer entity `Addon` in file `src/MSFSAddonPublisher.Domain/Entities/Addon.cs` with properties: Id (Guid), Metadata (AddonMetadata), InstallPath (string), IsSelected (bool), DiscoveredAt (DateTime), CreatedAt (DateTime), UpdatedAt (DateTime) | | |
| TASK-007 | Implement Domain layer aggregate root `AddonCatalog` in file `src/MSFSAddonPublisher.Domain/Aggregates/AddonCatalog.cs` with methods: AddAddon(Addon), RemoveAddon(Guid), GetSelectedAddons(), ClearSelection(), SelectAll(), GetAddonsByType(ContentType) | | |
| TASK-008 | Implement Domain layer interface `IAddonRepository` in file `src/MSFSAddonPublisher.Domain/Repositories/IAddonRepository.cs` with methods: Task<IEnumerable<Addon>> GetAllAsync(), Task<Addon?> GetByIdAsync(Guid), Task AddAsync(Addon), Task UpdateAsync(Addon), Task DeleteAsync(Guid), Task<int> CountAsync() | | |
| TASK-009 | Implement Domain layer interface `IPublishingPlatform` in file `src/MSFSAddonPublisher.Domain/Interfaces/IPublishingPlatform.cs` with methods: Task<PublishResult> PublishAsync(IEnumerable<Addon>, CancellationToken), Task<bool> ValidateCredentialsAsync(), string PlatformName { get; } | | |
| TASK-010 | Create Domain layer value object `PublishResult` in file `src/MSFSAddonPublisher.Domain/ValueObjects/PublishResult.cs` with properties: Success (bool), Message (string), PublishedCount (int), Errors (List<string>) | | |
| TASK-011 | Set up Serilog in file `src/MSFSAddonPublisher.UI/Program.cs` with sinks to file (`logs/msfs-addon-publisher-.txt`) and debug output, minimum level Information | | |
| TASK-012 | Configure NuGet packages: Serilog.Sinks.File (latest), Serilog.Sinks.Debug (latest), Newtonsoft.Json (latest), System.Security.Cryptography.ProtectedData (latest) | | |

### Implementation Phase 2: Infrastructure Layer - File System and Addon Discovery

**GOAL-002**: Implement addon scanning, manifest parsing, and file system access with security validation

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-013 | Implement `PathValidator` static class in file `src/MSFSAddonPublisher.Infrastructure/Security/PathValidator.cs` with method `bool IsValidPath(string path)` that validates against directory traversal patterns (.., absolute paths outside allowed directories) | | |
| TASK-014 | Implement `MSFSPathResolver` class in file `src/MSFSAddonPublisher.Infrastructure/FileSystem/MSFSPathResolver.cs` with methods: `List<string> GetMSFS2020Paths()`, `List<string> GetMSFS2024Paths()` that detect Store and Steam installation directories | | |
| TASK-015 | Implement `ManifestParser` class in file `src/MSFSAddonPublisher.Infrastructure/FileSystem/ManifestParser.cs` with method `async Task<AddonMetadata?> ParseManifestAsync(string manifestPath)` using Newtonsoft.Json with error handling for malformed JSON | | |
| TASK-016 | Implement `AddonScanner` class in file `src/MSFSAddonPublisher.Infrastructure/FileSystem/AddonScanner.cs` with method `async Task<List<Addon>> ScanDirectoryAsync(string basePath, IProgress<ScanProgress> progress, CancellationToken ct)` that recursively searches for manifest.json files | | |
| TASK-017 | Create `ScanProgress` class in file `src/MSFSAddonPublisher.Infrastructure/FileSystem/ScanProgress.cs` with properties: CurrentFile (string), ProcessedCount (int), TotalCount (int), PercentComplete (int) | | |
| TASK-018 | Implement in-memory `AddonRepository` in file `src/MSFSAddonPublisher.Infrastructure/Repositories/InMemoryAddonRepository.cs` implementing IAddonRepository using ConcurrentDictionary<Guid, Addon> for thread-safe storage | | |
| TASK-019 | Implement `AddonCache` class in file `src/MSFSAddonPublisher.Infrastructure/Caching/AddonCache.cs` with methods: `void Set(List<Addon> addons, TimeSpan expiration)`, `List<Addon>? Get()`, `void Clear()` using MemoryCache | | |
| TASK-020 | Add unit tests in file `tests/MSFSAddonPublisher.Tests.Unit/Infrastructure/PathValidatorTests.cs` with test methods: `IsValidPath_WithDirectoryTraversal_ReturnsFalse()`, `IsValidPath_WithValidPath_ReturnsTrue()`, `IsValidPath_WithAbsolutePathOutsideAllowed_ReturnsFalse()` | | |
| TASK-021 | Add unit tests in file `tests/MSFSAddonPublisher.Tests.Unit/Infrastructure/ManifestParserTests.cs` with test methods: `ParseManifestAsync_WithValidJson_ReturnsMetadata()`, `ParseManifestAsync_WithMalformedJson_ReturnsNull()`, `ParseManifestAsync_WithMissingFile_ReturnsNull()` | | |

### Implementation Phase 3: Infrastructure Layer - Platform Publishers

**GOAL-003**: Implement Discord and Twitch publishing services with secure credential management and extensible plugin architecture

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-022 | Implement `SecureCredentialStore` class in file `src/MSFSAddonPublisher.Infrastructure/Security/SecureCredentialStore.cs` using Windows DPAPI (ProtectedData.Protect/Unprotect) with methods: `void SaveCredential(string key, string value)`, `string? GetCredential(string key)`, `void DeleteCredential(string key)` storing in `%APPDATA%\MSFSAddonPublisher\credentials.dat` | | |
| TASK-023 | Implement `DiscordPublisher` class in file `src/MSFSAddonPublisher.Infrastructure/Publishers/DiscordPublisher.cs` implementing IPublishingPlatform with webhook URL support, embed message formatting (title, description, fields for each addon), and HttpClient with retry policy | | |
| TASK-024 | Implement `TwitchPublisher` class in file `src/MSFSAddonPublisher.Infrastructure/Publishers/TwitchPublisher.cs` implementing IPublishingPlatform with IRC connection (irc.chat.twitch.tv:6697, SSL) and OAuth token authentication following Twitch IRC format | | |
| TASK-025 | Create `DiscordEmbedBuilder` helper class in file `src/MSFSAddonPublisher.Infrastructure/Publishers/DiscordEmbedBuilder.cs` with method `object BuildEmbed(IEnumerable<Addon> addons)` that formats addon list as Discord embed with color coding by content type | | |
| TASK-026 | Create `TwitchMessageFormatter` helper class in file `src/MSFSAddonPublisher.Infrastructure/Publishers/TwitchMessageFormatter.cs` with method `string FormatMessage(IEnumerable<Addon> addons, int maxLength = 500)` that truncates and formats for Twitch chat limits | | |
| TASK-027 | Implement `PublishingPlatformFactory` class in file `src/MSFSAddonPublisher.Infrastructure/Publishers/PublishingPlatformFactory.cs` with method `IPublishingPlatform CreatePlatform(string platformName)` using reflection to discover and instantiate platform implementations | | |
| TASK-028 | Create `HttpClientFactory` singleton in file `src/MSFSAddonPublisher.Infrastructure/Http/HttpClientFactory.cs` with configured HttpClient instances including timeout (30s), SSL validation, and user agent header | | |
| TASK-029 | Implement rate limiter `RateLimitedPublisher` decorator in file `src/MSFSAddonPublisher.Infrastructure/Publishers/RateLimitedPublisher.cs` wrapping IPublishingPlatform with configurable rate limits (default: 5 requests per minute) | | |
| TASK-030 | Add unit tests in file `tests/MSFSAddonPublisher.Tests.Unit/Infrastructure/SecureCredentialStoreTests.cs` with test methods: `SaveCredential_EncryptsData()`, `GetCredential_DecryptsData()`, `DeleteCredential_RemovesData()` | | |
| TASK-031 | Add unit tests in file `tests/MSFSAddonPublisher.Tests.Unit/Infrastructure/DiscordPublisherTests.cs` with test methods: `PublishAsync_WithValidWebhook_ReturnsSuccess()`, `PublishAsync_WithInvalidWebhook_ReturnsFailure()`, `ValidateCredentialsAsync_WithValidWebhook_ReturnsTrue()` | | |

### Implementation Phase 4: Application Layer - Services and Use Cases

**GOAL-004**: Implement application services coordinating domain logic and infrastructure with async operations

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-032 | Implement `AddonDiscoveryService` class in file `src/MSFSAddonPublisher.Application/Services/AddonDiscoveryService.cs` with method `async Task<DiscoveryResult> DiscoverAddonsAsync(SimulatorVersion version, IProgress<ScanProgress> progress, CancellationToken ct)` orchestrating MSFSPathResolver, AddonScanner, and AddonRepository | | |
| TASK-033 | Implement `PublishingService` class in file `src/MSFSAddonPublisher.Application/Services/PublishingService.cs` with method `async Task<PublishResult> PublishToPatformAsync(string platformName, IEnumerable<Addon> addons, CancellationToken ct)` using PublishingPlatformFactory and RateLimitedPublisher | | |
| TASK-034 | Implement `ConfigurationService` class in file `src/MSFSAddonPublisher.Application/Services/ConfigurationService.cs` with methods: `void SavePreferences(UserPreferences prefs)`, `UserPreferences LoadPreferences()`, `void ResetToDefaults()` using JSON serialization to `%APPDATA%\MSFSAddonPublisher\config.json` | | |
| TASK-035 | Create `UserPreferences` DTO in file `src/MSFSAddonPublisher.Application/DTOs/UserPreferences.cs` with properties: LastSelectedSimulator (SimulatorVersion enum), EnableAutoRefresh (bool), RefreshIntervalMinutes (int), DefaultPublishingPlatform (string), WindowWidth (int), WindowHeight (int) | | |
| TASK-036 | Create `DiscoveryResult` DTO in file `src/MSFSAddonPublisher.Application/DTOs/DiscoveryResult.cs` with properties: Addons (List<Addon>), TotalFound (int), ScanDuration (TimeSpan), Errors (List<string>) | | |
| TASK-037 | Implement `AddonExportService` class in file `src/MSFSAddonPublisher.Application/Services/AddonExportService.cs` with methods: `async Task ExportToCsvAsync(IEnumerable<Addon> addons, string filePath)`, `async Task ExportToJsonAsync(IEnumerable<Addon> addons, string filePath)` for local backup functionality | | |
| TASK-038 | Create enum `SimulatorVersion` in file `src/MSFSAddonPublisher.Application/Enums/SimulatorVersion.cs` with values: MSFS2020, MSFS2024, Both | | |
| TASK-039 | Implement dependency injection container setup in file `src/MSFSAddonPublisher.Application/DependencyInjection.cs` with extension method `IServiceCollection AddApplicationServices(this IServiceCollection services)` registering all services with appropriate lifetimes | | |
| TASK-040 | Add unit tests in file `tests/MSFSAddonPublisher.Tests.Unit/Application/AddonDiscoveryServiceTests.cs` with test methods: `DiscoverAddonsAsync_WithValidPath_ReturnsAddons()`, `DiscoverAddonsAsync_WithCancellation_ThrowsOperationCanceled()`, `DiscoverAddonsAsync_ReportsProgress()` | | |
| TASK-041 | Add unit tests in file `tests/MSFSAddonPublisher.Tests.Unit/Application/PublishingServiceTests.cs` with test methods: `PublishToPlatformAsync_WithValidPlatform_ReturnsSuccess()`, `PublishToPlatformAsync_WithInvalidPlatform_ReturnsFailure()` | | |

### Implementation Phase 5: UI Layer - Main Form and Addon Grid

**GOAL-005**: Build Windows Forms UI with main form, addon grid, and responsive async operations

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-042 | Create `MainForm.cs` Windows Form in file `src/MSFSAddonPublisher.UI/Forms/MainForm.cs` with dimensions 1200x800, title "MSFS Addon Publisher", icon, and menu strip | | |
| TASK-043 | Add MenuStrip to MainForm with menus: File (Exit), Tools (Refresh Addons, Settings, Export), Platforms (Configure Discord, Configure Twitch, Manage Platforms), Help (About, Documentation) | | |
| TASK-044 | Add ToolStrip to MainForm with buttons: Refresh (icon: refresh arrow), Publish (icon: upload), Select All (icon: checkbox), Deselect All (icon: empty checkbox), Export (icon: save) | | |
| TASK-045 | Add Panel to top of MainForm with controls: Label "Simulator Version:", ComboBox for SimulatorVersion selection, Label "Status:", Label for status text with auto-ellipsis | | |
| TASK-046 | Add DataGridView to MainForm with columns: CheckBox (Selected), TextBox (Name), TextBox (Type), TextBox (Version), TextBox (Creator), TextBox (Path), configured with AllowUserToAddRows=false, ReadOnly=true (except checkbox), AutoSizeColumnsMode=Fill | | |
| TASK-047 | Add ProgressBar to bottom of MainForm for scan/publish progress, initially hidden | | |
| TASK-048 | Add StatusStrip to MainForm with labels: Addon count, Selected count, Last scan time | | |
| TASK-049 | Implement `MainFormViewModel` class in file `src/MSFSAddonPublisher.UI/ViewModels/MainFormViewModel.cs` with properties: ObservableCollection<AddonViewModel>, SimulatorVersion, Status, IsScanning, IsPublishing, ScanProgress | | |
| TASK-050 | Implement `AddonViewModel` class in file `src/MSFSAddonPublisher.UI/ViewModels/AddonViewModel.cs` wrapping Addon entity with INotifyPropertyChanged for IsSelected property binding | | |
| TASK-051 | Implement async event handler `RefreshButton_Click` in MainForm.cs calling `await addonDiscoveryService.DiscoverAddonsAsync()` with progress reporting to ProgressBar and status updates, using CancellationTokenSource | | |
| TASK-052 | Implement `UpdateGridData` method in MainForm.cs that populates DataGridView from ObservableCollection<AddonViewModel> on UI thread using Invoke if necessary | | |
| TASK-053 | Implement CheckBox cell click handler in DataGridView updating AddonViewModel.IsSelected property | | |

### Implementation Phase 6: UI Layer - Platform Configuration and Publishing

**GOAL-006**: Implement platform configuration dialogs and publishing workflow with progress feedback

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-054 | Create `DiscordConfigForm.cs` Windows Form in file `src/MSFSAddonPublisher.UI/Forms/DiscordConfigForm.cs` with dimensions 500x300, modal dialog with controls: Label "Webhook URL:", TextBox for URL, Button "Test Connection", Button "Save", Button "Cancel" | | |
| TASK-055 | Implement validation in DiscordConfigForm ensuring webhook URL matches pattern `https://discord.com/api/webhooks/` or `https://discordapp.com/api/webhooks/` before saving | | |
| TASK-056 | Implement "Test Connection" button in DiscordConfigForm calling `await discordPublisher.ValidateCredentialsAsync()` and displaying MessageBox with result | | |
| TASK-057 | Implement save functionality in DiscordConfigForm calling `secureCredentialStore.SaveCredential("discord_webhook", webhookUrl)` | | |
| TASK-058 | Create `TwitchConfigForm.cs` Windows Form in file `src/MSFSAddonPublisher.UI/Forms/TwitchConfigForm.cs` with dimensions 500x350, modal dialog with controls: Label "Channel Name:", TextBox, Label "OAuth Token:", TextBox with PasswordChar, LinkLabel "Get Token", Button "Test Connection", Button "Save", Button "Cancel" | | |
| TASK-059 | Implement validation in TwitchConfigForm ensuring OAuth token starts with "oauth:" prefix and channel name is alphanumeric | | |
| TASK-060 | Implement "Get Token" LinkLabel in TwitchConfigForm opening browser to `https://twitchapps.com/tmi/` | | |
| TASK-061 | Implement save functionality in TwitchConfigForm calling `secureCredentialStore.SaveCredential("twitch_channel")` and `secureCredentialStore.SaveCredential("twitch_oauth")` | | |
| TASK-062 | Create `PublishDialog.cs` Windows Form in file `src/MSFSAddonPublisher.UI/Forms/PublishDialog.cs` with dimensions 600x400, modal dialog with controls: Label "Select Platform:", ComboBox with available platforms, Label "Selected Addons:", ListBox (read-only), ProgressBar, Label for status, Button "Publish", Button "Cancel" | | |
| TASK-063 | Implement PublishDialog load logic populating platform ComboBox from PublishingPlatformFactory and ListBox from selected addons | | |
| TASK-064 | Implement async "Publish" button handler in PublishDialog calling `await publishingService.PublishToPlatformAsync()` with progress reporting and displaying final PublishResult in MessageBox | | |
| TASK-065 | Add menu handler in MainForm for "Configure Discord" opening DiscordConfigForm as modal dialog | | |
| TASK-066 | Add menu handler in MainForm for "Configure Twitch" opening TwitchConfigForm as modal dialog | | |
| TASK-067 | Add toolbar button handler in MainForm for "Publish" opening PublishDialog with currently selected addons | | |

### Implementation Phase 7: UI Layer - Settings, Export, and Polish

**GOAL-007**: Implement settings management, export functionality, and UI polish features

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-068 | Create `SettingsForm.cs` Windows Form in file `src/MSFSAddonPublisher.UI/Forms/SettingsForm.cs` with dimensions 600x500, modal dialog with TabControl containing tabs: General, Advanced, About | | |
| TASK-069 | Implement General tab in SettingsForm with controls: CheckBox "Enable auto-refresh", NumericUpDown "Refresh interval (minutes)" (range: 1-60), ComboBox "Default platform", CheckBox "Minimize to system tray", CheckBox "Start with Windows" | | |
| TASK-070 | Implement Advanced tab in SettingsForm with controls: TextBox "Custom MSFS path" with Browse button, CheckBox "Include official addons", CheckBox "Scan subdirectories", NumericUpDown "Scan timeout (seconds)" (range: 10-300), Button "Clear cache", Button "Reset settings" | | |
| TASK-071 | Implement About tab in SettingsForm displaying: Application name, version (from assembly), copyright, license (MIT), GitHub link as LinkLabel, Button "Check for updates" | | |
| TASK-072 | Implement save functionality in SettingsForm calling `configurationService.SavePreferences()` with all form values | | |
| TASK-073 | Implement load functionality in SettingsForm calling `configurationService.LoadPreferences()` and populating form controls | | |
| TASK-074 | Add menu handler in MainForm for "Settings" opening SettingsForm as modal dialog | | |
| TASK-075 | Implement "Export to CSV" functionality in MainForm calling `await addonExportService.ExportToCsvAsync()` with SaveFileDialog | | |
| TASK-076 | Implement "Export to JSON" functionality in MainForm calling `await addonExportService.ExportToJsonAsync()` with SaveFileDialog | | |
| TASK-077 | Add menu handler in MainForm for "Export" showing submenu with CSV and JSON options | | |
| TASK-078 | Implement "About" dialog in file `src/MSFSAddonPublisher.UI/Forms/AboutForm.cs` displaying application info, version, and license with dimensions 400x300 | | |
| TASK-079 | Implement "Select All" and "Deselect All" toolbar button handlers in MainForm updating all AddonViewModel.IsSelected properties and refreshing grid | | |
| TASK-080 | Implement form state persistence in MainForm saving and restoring window position, size, and column widths using UserPreferences | | |
| TASK-081 | Add keyboard shortcuts in MainForm: F5 (Refresh), Ctrl+A (Select All), Ctrl+D (Deselect All), Ctrl+E (Export), Ctrl+P (Publish) | | |

### Implementation Phase 8: Testing and Quality Assurance

**GOAL-008**: Implement comprehensive test suites and ensure code quality standards

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-082 | Add integration test in file `tests/MSFSAddonPublisher.Tests.Integration/AddonDiscoveryIntegrationTests.cs` with method `DiscoverAddons_WithTestDirectory_FindsManifests()` using test fixtures with sample manifest.json files | | |
| TASK-083 | Add integration test for Discord publishing in file `tests/MSFSAddonPublisher.Tests.Integration/DiscordPublishingIntegrationTests.cs` with method `PublishToDiscord_WithMockWebhook_SendsRequest()` using WireMock for HTTP mocking | | |
| TASK-084 | Add integration test for credential storage in file `tests/MSFSAddonPublisher.Tests.Integration/SecureCredentialStoreIntegrationTests.cs` verifying encryption/decryption roundtrip | | |
| TASK-085 | Configure test project with NuGet packages: xUnit (latest), Moq (latest), FluentAssertions (latest), WireMock.Net (latest), Coverlet (latest for code coverage) | | |
| TASK-086 | Create test fixture helper `TestManifestGenerator` in file `tests/MSFSAddonPublisher.Tests.Unit/Helpers/TestManifestGenerator.cs` with method `string GenerateValidManifest(string title, string creator, ContentType type)` | | |
| TASK-087 | Add unit tests for AddonCatalog aggregate in file `tests/MSFSAddonPublisher.Tests.Unit/Domain/AddonCatalogTests.cs` with test methods: `AddAddon_IncreasesCount()`, `GetSelectedAddons_ReturnsOnlySelected()`, `SelectAll_SelectsAllAddons()` | | |
| TASK-088 | Add unit tests for AddonDiscoveryService in file `tests/MSFSAddonPublisher.Tests.Unit/Application/AddonDiscoveryServiceTests.cs` using Moq to mock dependencies | | |
| TASK-089 | Configure code coverage thresholds in file `tests/coverlet.runsettings` with minimum line coverage 80% for Domain and Application projects | | |
| TASK-090 | Add GitHub Actions workflow file `.github/workflows/build-and-test.yml` running dotnet build, dotnet test with code coverage, and publishing coverage reports | | |
| TASK-091 | Implement performance test in file `tests/MSFSAddonPublisher.Tests.Performance/AddonScanPerformanceTests.cs` verifying scan of 500 addons completes within 5 seconds | | |

### Implementation Phase 9: Documentation and Deployment

**GOAL-009**: Create comprehensive documentation and setup deployment infrastructure

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-092 | Create README.md in repository root with sections: Overview, Features, Installation, Usage, Configuration, Platform Setup (Discord/Twitch), Building from Source, Contributing, License | | |
| TASK-093 | Create CONTRIBUTING.md with guidelines: Code style, pull request process, testing requirements, commit message conventions | | |
| TASK-094 | Create docs/USER_GUIDE.md with detailed usage instructions, screenshots (placeholders), and troubleshooting section | | |
| TASK-095 | Create docs/DEVELOPER_GUIDE.md with architecture overview, project structure explanation, development setup, and extending platform support instructions | | |
| TASK-096 | Create docs/SECURITY.md with security policy, responsible disclosure process, and supported versions | | |
| TASK-097 | Add XML documentation comments to all public APIs in Domain, Application, and Infrastructure projects | | |
| TASK-098 | Create ClickOnce deployment manifest in file `src/MSFSAddonPublisher.UI/Properties/app.manifest` with required Windows compatibility and DPI awareness settings | | |
| TASK-099 | Configure publish profile in file `src/MSFSAddonPublisher.UI/Properties/PublishProfiles/ClickOnce.pubxml` for single-file deployment with target runtime win-x64 | | |
| TASK-100 | Create installer project using WiX Toolset or Advanced Installer for MSI generation with prerequisites (.NET 8 Runtime) | | |
| TASK-101 | Add GitHub Actions workflow file `.github/workflows/release.yml` for automated release builds triggered on version tags, generating MSI and ClickOnce packages | | |
| TASK-102 | Create CHANGELOG.md following Keep a Changelog format with initial v1.0.0 entry listing all features | | |

### Implementation Phase 10: Plugin Architecture and Extensibility

**GOAL-010**: Implement plugin system for custom platform publishers

| Task | Description | Completed | Date |
|------|-------------|-----------|------|
| TASK-103 | Create plugin interface `IPublisherPlugin` in file `src/MSFSAddonPublisher.Domain/Plugins/IPublisherPlugin.cs` with properties: Name, Description, Version, Author, Icon (byte[]) and methods: Initialize(IServiceProvider), CreatePublisher() returning IPublishingPlatform | | |
| TASK-104 | Implement `PluginLoader` class in file `src/MSFSAddonPublisher.Infrastructure/Plugins/PluginLoader.cs` with method `List<IPublisherPlugin> LoadPlugins(string pluginDirectory)` using Assembly.LoadFrom and reflection to discover IPublisherPlugin implementations | | |
| TASK-105 | Create plugin directory structure `%APPDATA%\MSFSAddonPublisher\Plugins\` with subdirectories for each plugin | | |
| TASK-106 | Implement plugin manifest schema `plugin.json` with fields: name, version, author, description, entryAssembly, entryClass | | |
| TASK-107 | Add "Manage Platforms" dialog in file `src/MSFSAddonPublisher.UI/Forms/ManagePlatformsForm.cs` displaying installed plugins in ListBox with buttons: Install, Remove, Reload | | |
| TASK-108 | Implement plugin installation in ManagePlatformsForm copying plugin DLL and dependencies to plugin directory and validating plugin.json | | |
| TASK-109 | Update PublishingPlatformFactory to query PluginLoader for available platforms in addition to built-in ones | | |
| TASK-110 | Create sample plugin project `MSFSAddonPublisher.Plugins.Sample` demonstrating custom platform implementation with README | | |
| TASK-111 | Add plugin development guide to docs/PLUGIN_DEVELOPMENT.md with API reference, sample code, and deployment instructions | | |

## 3. Alternatives

### Alternative Approaches Considered

- **ALT-001**: WPF instead of Windows Forms - Not chosen because requirement CON-002 explicitly specifies Windows Forms, and WPF would add unnecessary complexity for this use case
- **ALT-002**: SQLite database instead of in-memory repository - Not chosen for v1.0 due to added complexity; addon data is ephemeral and scan times are acceptable. Can be added in future version if performance becomes issue
- **ALT-003**: REST API architecture with separate frontend - Not chosen because requirement specifies desktop application, not web-based. Desktop provides better file system access and user trust for credentials
- **ALT-004**: Electron/JavaScript instead of .NET - Not chosen because requirements specify .NET/C#, and native .NET provides better performance and Windows integration
- **ALT-005**: Direct database queries instead of Repository pattern - Not chosen because Repository pattern provides better testability, abstraction, and follows DDD principles per GUD-003
- **ALT-006**: Hardcoded platform list instead of plugin architecture - Not chosen because REQ-006 requires extensibility, and plugin system provides better maintainability and community contributions
- **ALT-007**: Plaintext credential storage - Not chosen due to SEC-001 requirement for encryption; DPAPI provides OS-level security without external dependencies
- **ALT-008**: Synchronous file I/O - Not chosen because PER-002 requires responsive UI and PER-001 requires acceptable performance with large addon counts

## 4. Dependencies

### NuGet Package Dependencies

- **DEP-001**: Serilog.Sinks.File ^6.0.0 - Structured logging to file system
- **DEP-002**: Serilog.Sinks.Debug ^3.0.0 - Logging to debug output for development
- **DEP-003**: Newtonsoft.Json ^13.0.3 - JSON parsing for manifest.json files
- **DEP-004**: System.Security.Cryptography.ProtectedData ^8.0.0 - DPAPI encryption for credentials
- **DEP-005**: xUnit ^2.6.0 - Unit testing framework
- **DEP-006**: Moq ^4.20.0 - Mocking framework for unit tests
- **DEP-007**: FluentAssertions ^6.12.0 - Fluent assertion library for readable tests
- **DEP-008**: WireMock.Net ^1.5.0 - HTTP mocking for integration tests
- **DEP-009**: Coverlet.Collector ^6.0.0 - Code coverage collection

### External Service Dependencies

- **DEP-010**: Discord Webhook API - External HTTP endpoint for Discord publishing, requires webhook URL from user
- **DEP-011**: Twitch IRC - External IRC server (irc.chat.twitch.tv:6697) for Twitch chat publishing, requires OAuth token
- **DEP-012**: .NET 8.0 Runtime - Must be installed on target system for application execution
- **DEP-013**: Windows 10 1809+ - Minimum OS version for DPAPI and Windows Forms features

### Development Tool Dependencies

- **DEP-014**: Visual Studio 2022 17.8+ or JetBrains Rider 2024.1+ - IDE with C# 13 support
- **DEP-015**: .NET 8.0 SDK - Build tools and compiler
- **DEP-016**: Git - Version control
- **DEP-017**: WiX Toolset 4.0+ or Advanced Installer - MSI installer creation (optional)

## 5. Files

### Domain Layer Files

- **FILE-001**: `src/MSFSAddonPublisher.Domain/Entities/Addon.cs` - Core addon entity with identity and metadata
- **FILE-002**: `src/MSFSAddonPublisher.Domain/ValueObjects/AddonMetadata.cs` - Immutable value object for addon metadata
- **FILE-003**: `src/MSFSAddonPublisher.Domain/ValueObjects/PublishResult.cs` - Publishing operation result
- **FILE-004**: `src/MSFSAddonPublisher.Domain/Aggregates/AddonCatalog.cs` - Aggregate root managing addon collection
- **FILE-005**: `src/MSFSAddonPublisher.Domain/Enums/ContentType.cs` - Addon content type enumeration
- **FILE-006**: `src/MSFSAddonPublisher.Domain/Repositories/IAddonRepository.cs` - Repository interface
- **FILE-007**: `src/MSFSAddonPublisher.Domain/Interfaces/IPublishingPlatform.cs` - Platform publisher interface
- **FILE-008**: `src/MSFSAddonPublisher.Domain/Plugins/IPublisherPlugin.cs` - Plugin interface for extensibility

### Application Layer Files

- **FILE-009**: `src/MSFSAddonPublisher.Application/Services/AddonDiscoveryService.cs` - Orchestrates addon scanning
- **FILE-010**: `src/MSFSAddonPublisher.Application/Services/PublishingService.cs` - Orchestrates publishing operations
- **FILE-011**: `src/MSFSAddonPublisher.Application/Services/ConfigurationService.cs` - User preference management
- **FILE-012**: `src/MSFSAddonPublisher.Application/Services/AddonExportService.cs` - Export functionality
- **FILE-013**: `src/MSFSAddonPublisher.Application/DTOs/UserPreferences.cs` - User settings DTO
- **FILE-014**: `src/MSFSAddonPublisher.Application/DTOs/DiscoveryResult.cs` - Scan result DTO
- **FILE-015**: `src/MSFSAddonPublisher.Application/Enums/SimulatorVersion.cs` - MSFS version enumeration
- **FILE-016**: `src/MSFSAddonPublisher.Application/DependencyInjection.cs` - DI configuration

### Infrastructure Layer Files

- **FILE-017**: `src/MSFSAddonPublisher.Infrastructure/FileSystem/AddonScanner.cs` - File system scanning logic
- **FILE-018**: `src/MSFSAddonPublisher.Infrastructure/FileSystem/ManifestParser.cs` - JSON manifest parsing
- **FILE-019**: `src/MSFSAddonPublisher.Infrastructure/FileSystem/MSFSPathResolver.cs` - Installation path detection
- **FILE-020**: `src/MSFSAddonPublisher.Infrastructure/FileSystem/ScanProgress.cs` - Progress reporting model
- **FILE-021**: `src/MSFSAddonPublisher.Infrastructure/Repositories/InMemoryAddonRepository.cs` - In-memory storage
- **FILE-022**: `src/MSFSAddonPublisher.Infrastructure/Caching/AddonCache.cs` - Memory cache wrapper
- **FILE-023**: `src/MSFSAddonPublisher.Infrastructure/Publishers/DiscordPublisher.cs` - Discord webhook implementation
- **FILE-024**: `src/MSFSAddonPublisher.Infrastructure/Publishers/TwitchPublisher.cs` - Twitch IRC implementation
- **FILE-025**: `src/MSFSAddonPublisher.Infrastructure/Publishers/DiscordEmbedBuilder.cs` - Discord message formatting
- **FILE-026**: `src/MSFSAddonPublisher.Infrastructure/Publishers/TwitchMessageFormatter.cs` - Twitch message formatting
- **FILE-027**: `src/MSFSAddonPublisher.Infrastructure/Publishers/PublishingPlatformFactory.cs` - Platform factory
- **FILE-028**: `src/MSFSAddonPublisher.Infrastructure/Publishers/RateLimitedPublisher.cs` - Rate limiting decorator
- **FILE-029**: `src/MSFSAddonPublisher.Infrastructure/Security/SecureCredentialStore.cs` - DPAPI credential storage
- **FILE-030**: `src/MSFSAddonPublisher.Infrastructure/Security/PathValidator.cs` - Path validation security
- **FILE-031**: `src/MSFSAddonPublisher.Infrastructure/Http/HttpClientFactory.cs` - HTTP client management
- **FILE-032**: `src/MSFSAddonPublisher.Infrastructure/Plugins/PluginLoader.cs` - Dynamic plugin loading

### UI Layer Files

- **FILE-033**: `src/MSFSAddonPublisher.UI/Forms/MainForm.cs` - Main application window
- **FILE-034**: `src/MSFSAddonPublisher.UI/Forms/DiscordConfigForm.cs` - Discord configuration dialog
- **FILE-035**: `src/MSFSAddonPublisher.UI/Forms/TwitchConfigForm.cs` - Twitch configuration dialog
- **FILE-036**: `src/MSFSAddonPublisher.UI/Forms/PublishDialog.cs` - Publishing progress dialog
- **FILE-037**: `src/MSFSAddonPublisher.UI/Forms/SettingsForm.cs` - Application settings dialog
- **FILE-038**: `src/MSFSAddonPublisher.UI/Forms/AboutForm.cs` - About information dialog
- **FILE-039**: `src/MSFSAddonPublisher.UI/Forms/ManagePlatformsForm.cs` - Plugin management dialog
- **FILE-040**: `src/MSFSAddonPublisher.UI/ViewModels/MainFormViewModel.cs` - Main form view model
- **FILE-041**: `src/MSFSAddonPublisher.UI/ViewModels/AddonViewModel.cs` - Addon display wrapper
- **FILE-042**: `src/MSFSAddonPublisher.UI/Program.cs` - Application entry point

### Configuration Files

- **FILE-043**: `.editorconfig` - Code formatting rules
- **FILE-044**: `Directory.Build.props` - Common MSBuild properties
- **FILE-045**: `MSFSAddonPublisher.sln` - Solution file
- **FILE-046**: `.github/workflows/build-and-test.yml` - CI workflow
- **FILE-047**: `.github/workflows/release.yml` - Release workflow

### Documentation Files

- **FILE-048**: `README.md` - Project overview and quick start
- **FILE-049**: `CONTRIBUTING.md` - Contribution guidelines
- **FILE-050**: `CHANGELOG.md` - Version history
- **FILE-051**: `LICENSE` - MIT license (already exists)
- **FILE-052**: `docs/USER_GUIDE.md` - End user documentation
- **FILE-053**: `docs/DEVELOPER_GUIDE.md` - Developer documentation
- **FILE-054**: `docs/PLUGIN_DEVELOPMENT.md` - Plugin development guide
- **FILE-055**: `docs/SECURITY.md` - Security policy

## 6. Testing

### Unit Tests

- **TEST-001**: PathValidator tests verifying directory traversal prevention and path validation logic
- **TEST-002**: ManifestParser tests verifying JSON parsing with valid, malformed, and missing files
- **TEST-003**: AddonCatalog aggregate tests verifying business rules for addon management
- **TEST-004**: SecureCredentialStore tests verifying DPAPI encryption/decryption
- **TEST-005**: DiscordPublisher tests verifying webhook formatting and error handling
- **TEST-006**: AddonDiscoveryService tests verifying orchestration with mocked dependencies
- **TEST-007**: PublishingService tests verifying platform selection and error handling
- **TEST-008**: ConfigurationService tests verifying serialization and file I/O

### Integration Tests

- **TEST-009**: AddonDiscovery integration test with real file system and test manifest files
- **TEST-010**: Discord publishing integration test with WireMock simulating webhook endpoint
- **TEST-011**: SecureCredentialStore integration test verifying roundtrip encryption on Windows
- **TEST-012**: Plugin loading integration test verifying assembly loading and reflection

### Performance Tests

- **TEST-013**: Addon scanning performance test verifying 500 addons scanned within 5 seconds (PER-001)
- **TEST-014**: UI responsiveness test verifying UI remains responsive during background scanning (PER-002)
- **TEST-015**: Memory usage test verifying application stays under 200MB during normal operation (PER-003)
- **TEST-016**: Startup time test verifying application launches within 2 seconds (PER-004)

### Manual Testing Checklist

- **TEST-017**: Manual test: Install on clean Windows 10 machine and verify .NET runtime installation prompt
- **TEST-018**: Manual test: Configure Discord webhook and publish 10 addons, verify Discord message format
- **TEST-019**: Manual test: Configure Twitch IRC and publish addon list, verify chat message
- **TEST-020**: Manual test: Scan large addon collection (100+ addons) and verify UI responsiveness
- **TEST-021**: Manual test: Test all keyboard shortcuts (F5, Ctrl+A, Ctrl+P, etc.)
- **TEST-022**: Manual test: Export addon list to CSV and JSON, verify file contents
- **TEST-023**: Manual test: Restart application and verify window position and preferences persist
- **TEST-024**: Manual test: Install sample plugin and verify it appears in platform list

## 7. Risks & Assumptions

### Risks

- **RISK-001**: Microsoft may change MSFS addon directory structure or manifest format in future updates, breaking scanner logic. Mitigation: Implement version detection and format abstraction, maintain update monitoring
- **RISK-002**: Discord or Twitch may modify API/webhook formats, breaking publishing functionality. Mitigation: Implement comprehensive error handling, version checking, and provide user error messages with troubleshooting
- **RISK-003**: Antivirus software may flag DPAPI credential file as suspicious. Mitigation: Document credential storage in user guide, consider code signing certificate for executable
- **RISK-004**: Large addon collections (1000+) may cause performance degradation. Mitigation: Implement pagination or virtualization in DataGridView, optimize scanning algorithm
- **RISK-005**: Plugin system may introduce security vulnerabilities from untrusted code. Mitigation: Document plugin security risks, implement assembly validation, consider sandboxing in future version
- **RISK-006**: Windows Forms is legacy technology with limited modern UI capabilities. Mitigation: Accept constraint per CON-002, ensure clean professional design within Windows Forms limitations
- **RISK-007**: Rate limiting on Discord/Twitch may block legitimate publishing attempts. Mitigation: Implement exponential backoff retry logic, inform users of rate limits in UI

### Assumptions

- **ASSUMPTION-001**: Users have Microsoft Flight Simulator 2020 or 2024 installed in standard locations (Microsoft Store or Steam)
- **ASSUMPTION-002**: Users have administrative access to install .NET 8.0 runtime if not already present
- **ASSUMPTION-003**: Users understand how to obtain Discord webhook URLs and Twitch OAuth tokens (guidance provided in documentation)
- **ASSUMPTION-004**: Addon manifest.json files follow standard MSFS SDK format with required fields (title, creator, content_type)
- **ASSUMPTION-005**: Users have stable internet connection for publishing operations
- **ASSUMPTION-006**: Windows Data Protection API is available and functional on target systems
- **ASSUMPTION-007**: Target users are comfortable with Windows desktop applications and basic configuration
- **ASSUMPTION-008**: Community addon developers maintain manifest.json files according to MSFS standards
- **ASSUMPTION-009**: Publishing frequency is moderate (not continuous streaming), rate limits are sufficient for typical use
- **ASSUMPTION-010**: Single-user desktop application model is sufficient; multi-user or server deployment not required in v1.0

## 8. Related Specifications / Further Reading

### Microsoft Flight Simulator Documentation

- [MSFS SDK Documentation](https://docs.flightsimulator.com/) - Official SDK and addon development guide
- [MSFS Manifest Reference](https://docs.flightsimulator.com/html/Content_Configuration/Packages/Package_Definitions.htm) - Package manifest format specification
- [MSFS Installation Paths](https://flightsimulator.zendesk.com/hc/en-us/articles/4405893759378-PC-versions-User-Data-locations) - Official documentation on file locations

### Platform API Documentation

- [Discord Webhooks Guide](https://discord.com/developers/docs/resources/webhook) - Discord webhook API reference
- [Discord Embed Reference](https://discord.com/developers/docs/resources/channel#embed-object) - Embed message formatting
- [Twitch IRC Guide](https://dev.twitch.tv/docs/irc/) - Twitch chat IRC protocol documentation
- [Twitch Authentication](https://dev.twitch.tv/docs/authentication/) - OAuth token generation guide

### .NET and Architecture Resources

- [.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8) - .NET 8 features and APIs
- [Windows Forms Documentation](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/) - Windows Forms programming guide
- [Domain-Driven Design Reference](https://www.domainlanguage.com/ddd/reference/) - DDD patterns and principles
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID) - Object-oriented design principles
- [OWASP Top 10](https://owasp.org/www-project-top-ten/) - Web application security risks

### Security Standards

- [Windows Data Protection API](https://learn.microsoft.com/en-us/windows/win32/api/dpapi/) - DPAPI reference
- [OWASP Secure Coding Practices](https://owasp.org/www-project-secure-coding-practices-quick-reference-guide/) - Security guidelines
- [CWE-22: Path Traversal](https://cwe.mitre.org/data/definitions/22.html) - Path traversal vulnerability reference

### Testing and Quality

- [xUnit Documentation](https://xunit.net/) - Unit testing framework
- [Moq Quickstart](https://github.com/moq/moq4/wiki/Quickstart) - Mocking framework guide
- [FluentAssertions Documentation](https://fluentassertions.com/introduction) - Assertion library
- [.NET Code Coverage](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage) - Code coverage tools