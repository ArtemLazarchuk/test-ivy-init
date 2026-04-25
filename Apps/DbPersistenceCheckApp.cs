using Ivy;
using Microsoft.EntityFrameworkCore;
using TestIvyInit.Connections.TestIvyInit;
using TestIvyInit.Connections.TestIvyInit.Models;

namespace TestIvyInit.Apps;

[App(title: "DB persistence check", group: ["Debug"])]
public class DbPersistenceCheckApp : AppBase
{
    public override object? Build()
    {
        var dbFactory = UseService<TestIvyInitContextFactory>();

        var rows = UseQuery(
            "persistence-check.rows",
            async ct =>
            {
                await using var db = dbFactory.CreateDbContext();
                return await db.PersistenceCheckItems
                    .AsNoTracking()
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(30)
                    .Select(x => new RowVm(
                        x.Id,
                        x.Message,
                        x.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss 'UTC'zzz")))
                    .ToListAsync(ct);
            });

        var message = UseState("Hello from the app");

        var children = new List<object?>
        {
            Text.H2("Database persistence check"),
            Text.P(
                "This writes to table persistence_check. After you click Save, refresh the page or reopen the app: if the row is still there, data is stored in PostgreSQL."),
            Text.H4("Add a row"),
            Layout.Horizontal(
                message.ToTextInput("Message…"),
                new Button("Save to database", async () =>
                {
                    var text = (message.Value ?? "").Trim();
                    if (string.IsNullOrEmpty(text)) text = $"ping {DateTimeOffset.UtcNow:O}";

                    await using var db = dbFactory.CreateDbContext();
                    db.PersistenceCheckItems.Add(new PersistenceCheckItem
                    {
                        Message = text,
                        CreatedAt = DateTimeOffset.UtcNow
                    });
                    await db.SaveChangesAsync();
                    message.Set("");
                    rows.Mutator.Revalidate();
                }, ButtonVariant.Primary))
        };

        if (rows.Error != null)
            children.Add(Text.Danger(rows.Error.Message));
        else
            children.AddRange(new object?[] { Text.H4("Rows (newest first)"), Text.Markdown(BuildTable(rows.Value ?? [])) });

        return Layout.Vertical(children);
    }

    private static string BuildTable(IReadOnlyList<RowVm> items)
    {
        if (items.Count == 0)
            return "_No rows yet. Save one above._";

        var lines = new List<string>
        {
            "| Id (first 8 chars) | Created | Message |",
            "| --- | --- | --- |"
        };
        foreach (var r in items)
        {
            var idShort = r.Id.ToString("N")[..8] + "…";
            var safe = r.Message.Replace("|", "\\|").Replace("\n", " ");
            lines.Add($"| `{idShort}` | {r.CreatedLabel} | {safe} |");
        }

        return string.Join('\n', lines);
    }

    private sealed record RowVm(Guid Id, string Message, string CreatedLabel);
}
