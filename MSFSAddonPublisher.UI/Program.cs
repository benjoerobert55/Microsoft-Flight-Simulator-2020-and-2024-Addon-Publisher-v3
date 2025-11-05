using Serilog;

namespace MSFSAddonPublisher.UI;

/// <summary>
/// Main entry point for the MSFS Addon Publisher application.
/// </summary>
internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        // Configure Serilog
        ConfigureLogging();

        try
        {
            Log.Information("Starting MSFS Addon Publisher application");

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // TODO: Create and run main form once UI is implemented
            // Application.Run(new MainForm());

            Log.Information("Application started successfully");
            
            // Temporary message for Phase 1
            MessageBox.Show(
                "MSFS Addon Publisher - Phase 1 Infrastructure Complete!\n\n" +
                "Domain layer, configuration, and logging are now set up.\n" +
                "UI forms will be implemented in Phase 5.",
                "MSFS Addon Publisher",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
            MessageBox.Show(
                $"A fatal error occurred:\n\n{ex.Message}\n\nCheck logs for details.",
                "Fatal Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    /// <summary>
    /// Configures Serilog logging with file and debug sinks.
    /// </summary>
    private static void ConfigureLogging()
    {
        var logsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MSFSAddonPublisher",
            "logs");

        Directory.CreateDirectory(logsDirectory);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Debug()
            .WriteTo.File(
                path: Path.Combine(logsDirectory, "msfs-addon-publisher-.txt"),
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        Log.Information("Logging configured. Log files location: {LogsDirectory}", logsDirectory);
    }
}
