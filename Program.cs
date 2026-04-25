using Ivy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TestIvyInit.Connections.TestIvyInit;

var server = new Server();
server.UseCulture("en-US");
#if DEBUG
server.UseHotReload();
#endif

if (!server.Args.IsCliCommand)
{
    var factory = new TestIvyInitContextFactory(server.Args);
    await using var db = factory.CreateDbContext();
    await db.Database.MigrateAsync();
}

server.AddAppsFromAssembly();
server.AddConnectionsFromAssembly();
server.UseAppShell(new AppShellSettings().UseTabs(preventDuplicates: true));
await server.RunAsync();