using Ivy;
using Microsoft.EntityFrameworkCore.Design;

namespace TestIvyInit.Connections.TestIvyInit;

public class TestIvyInitDesignTimeContextFactory : IDesignTimeDbContextFactory<TestIvyInitContext>
{
    public TestIvyInitContext CreateDbContext(string[] args)
    {
        var serverArgs = new ServerArgs { Verbose = false };
        var contextFactory = new TestIvyInitContextFactory(serverArgs);
        return contextFactory.CreateDbContext();
    }
}
