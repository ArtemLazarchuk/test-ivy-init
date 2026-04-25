using System.Reflection;
using Ivy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TestIvyInit.Connections.TestIvyInit;

public class TestIvyInitContextFactory(ServerArgs args) : IDbContextFactory<TestIvyInitContext>
{
    public TestIvyInitContext CreateDbContext()
    {
        var configuration = new ConfigurationBuilder()
           .AddEnvironmentVariables()
           .AddUserSecrets(Assembly.GetExecutingAssembly())
           .Build();

        var optionsBuilder = new DbContextOptionsBuilder<TestIvyInitContext>();

        var connectionString = configuration.GetConnectionString("TestIvyInit");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Database connection string 'TestIvyInit' is not set.");
        }

        optionsBuilder.UseNpgsql(connectionString, conf =>
        {
            conf.EnableRetryOnFailure(5, TimeSpan.FromSeconds(2), null);
        });

        if (args.Verbose)
        {
            optionsBuilder
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }

        return new TestIvyInitContext(optionsBuilder.Options);
    }
}
