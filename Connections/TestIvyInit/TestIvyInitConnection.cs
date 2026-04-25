using Ivy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestIvyInit.Connections.TestIvyInit;

public class TestIvyInitConnection : IConnection, IHaveSecrets
{
    public string GetContext(string connectionPath)
    {
        var sb = new System.Text.StringBuilder();

        sb.AppendLine("## Namespaces");
        sb.AppendLine("```csharp");
        sb.AppendLine("using TestIvyInit.Connections.TestIvyInit;");
        sb.AppendLine("using TestIvyInit.Connections.TestIvyInit.Models;");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("## Usage");
        sb.AppendLine();
        sb.AppendLine("IMPORTANT: Do NOT register or inject `TestIvyInitContext` directly. Always use the factory.");
        sb.AppendLine("```csharp");
        sb.AppendLine("var dbFactory = UseService<TestIvyInitContextFactory>();");
        sb.AppendLine("var data = UseQuery(\"key\", async ct =>");
        sb.AppendLine("{");
        sb.AppendLine("    var db = dbFactory.CreateDbContext();");
        sb.AppendLine("    return await db.SomeEntity.ToListAsync(ct);");
        sb.AppendLine("});");
        sb.AppendLine("```");
        sb.AppendLine();

        // Describe entity types using TypeDescriber
        var entityTypes = typeof(TestIvyInitContext)
            .GetProperties()
            .Where(e => e.PropertyType.IsGenericType && e.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .Where(e => e.PropertyType.GenericTypeArguments[0].Name != "EfmigrationsLock")
            .Select(e => e.PropertyType.GenericTypeArguments[0])
            .ToArray();

        sb.AppendLine(TypeDescriber.Describe(entityTypes, "Models"));

        return sb.ToString();
    }

    public string GetName() => nameof(TestIvyInit);

    public string GetNamespace() => typeof(TestIvyInitConnection).Namespace;

    public string GetConnectionType() => "EntityFramework.Postgres";

    public ConnectionEntity[] GetEntities()
    {
        return typeof(TestIvyInitContext)
            .GetProperties()
            .Where(e => e.PropertyType.IsGenericType && e.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
            .Where(e => e.PropertyType.GenericTypeArguments[0].Name != "EfmigrationsLock")
            .Select(e => new ConnectionEntity(e.PropertyType.GenericTypeArguments[0].Name, e.Name))
            .ToArray();
    }

    public void RegisterServices(Server server)
    {
        server.Services.AddSingleton<TestIvyInitContextFactory>();
        server.Services.AddSingleton<IDbContextFactory<TestIvyInitContext>>(sp => sp.GetRequiredService<TestIvyInitContextFactory>());
    }

   public Secret[] GetSecrets()
   {
       return
       [
           new("ConnectionStrings:TestIvyInit")
       ];
   }

    public async Task<(bool ok, string? message)> TestConnection(IConfiguration config)
    {
        try
        {
            var factory = new TestIvyInitContextFactory(new ServerArgs());
            await using var context = factory.CreateDbContext();
            var canConnect = await context.Database.CanConnectAsync();
            return canConnect
                ? (true, null)
                : (false, "Unable to connect to the database.");
        }
        catch (Exception ex)
        {
            return (false, $"Connection test failed: {ex.Message}");
        }
    }
}
