using System.Threading;
using System.Reflection;
using Ivy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestIvyInit.Connections.TestIvyInit;

public sealed class TestIvyInitContextFactory : IDbContextFactory<TestIvyInitContext>
{
    private static readonly SemaphoreSlim InitLock = new(1, 1);

    private readonly ServerArgs _args;
    private readonly string _absolutePath;
    private readonly string _relativePath = "TestIvyInit.db";
    private readonly string _uniqueId = "fd4d3abf";
    private readonly ILogger? _logger;

    public TestIvyInitContextFactory(
        ServerArgs args,
        IVolume? volume = null,
        ILogger? logger = null
    )
    {
        _args = args;
        var fileName = _uniqueId + "." + _relativePath;
        if (volume != null)
        {
            _absolutePath = volume.GetAbsolutePath(fileName);
        }
        else
        {
            var volumePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Ivy-Data", "TestIvyInit");
            System.IO.Directory.CreateDirectory(volumePath);
            _absolutePath = System.IO.Path.Combine(volumePath, fileName);
        }
        _logger = logger;
    }

    public TestIvyInitContext CreateDbContext()
    {
        EnsureDatabasePresentOnce();

        var optionsBuilder = new DbContextOptionsBuilder<TestIvyInitContext>()
            .UseSqlite($@"Data Source=""{_absolutePath}""");

        if (_args.Verbose)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .LogTo(s => _logger?.LogInformation(s), LogLevel.Information);
        }

        return new TestIvyInitContext(optionsBuilder.Options);
    }

    private void EnsureDatabasePresentOnce()
    {
        if (System.IO.File.Exists(_absolutePath)) return;

        InitLock.Wait();
        try
        {
            if (System.IO.File.Exists(_absolutePath)) return;

            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(_absolutePath)!);

            var appDir = System.AppContext.BaseDirectory;
            var templatePath = System.IO.Path.Combine(appDir, _relativePath);

            if (!System.IO.File.Exists(templatePath))
            {
                throw new FileNotFoundException(
                    "Database template not found at '" + templatePath
                    + "'. Make sure '" + _relativePath + "' is copied to the output folder.");
            }

            var tmp = _absolutePath + ".tmp";
            System.IO.File.Copy(templatePath, tmp, overwrite: true);
            System.IO.File.Move(tmp, _absolutePath);
            _logger?.LogInformation("Initialized persistent database at '{Path}'.", _absolutePath);
        }
        catch (IOException)
        {
            if (!System.IO.File.Exists(_absolutePath)) throw;
        }
        finally
        {
            InitLock.Release();
        }
    }
}
