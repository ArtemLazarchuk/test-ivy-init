using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TestIvyInit.Connections.TestIvyInit;

public class TestIvyInitDesignTimeContextFactory : IDesignTimeDbContextFactory<TestIvyInitContext>
{
    public TestIvyInitContext CreateDbContext(string[] args)
    {
        var connectionString = args.Length > 0
            ? args[0]
            : $"Data Source={Path.Combine(Path.GetTempPath(), "TestIvyInit-design.db")}";
        var optionsBuilder = new DbContextOptionsBuilder<TestIvyInitContext>()
            .UseSqlite(connectionString);
        return new TestIvyInitContext(optionsBuilder.Options);
    }
}
