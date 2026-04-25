using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TestIvyInit.Connections.TestIvyInit;

public class TestIvyInitDesignTimeContextFactory : IDesignTimeDbContextFactory<TestIvyInitContext>
{
    public TestIvyInitContext CreateDbContext(string[] args)
    {
        if (args.Length == 0)
            throw new InvalidOperationException("Connection string argument is required. Usage: -- \"Data Source=<path>.db\"");
        var connectionString = args[0];
        var optionsBuilder = new DbContextOptionsBuilder<TestIvyInitContext>()
            .UseSqlite(connectionString);
        return new TestIvyInitContext(optionsBuilder.Options);
    }
}
