using Ivy;
using Microsoft.EntityFrameworkCore;
using TestIvyInit.Connections.TestIvyInit;
using TestIvyInit.Connections.TestIvyInit.Models;

namespace TestIvyInit.Apps;

[App(title: "DB Check", group: ["Debug"])]
public class DbCheckApp : AppBase
{
    public override object? Build()
    {
        var dbFactory = UseService<TestIvyInitContextFactory>();
        var note = UseState("Hello from deploy");

        var rows = UseQuery(
            "db-check.verification-rows",
            async ct =>
            {
                await using var db = dbFactory.CreateDbContext();
                return await db.DbVerificationEntries
                    .AsNoTracking()
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(50)
                    .Select(x => new RowVm(
                        x.Id,
                        x.Message,
                        x.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss 'UTC'zzz")))
                    .ToListAsync(ct);
            });

        var status = rows.Error is null
            ? $"Connected. Rows in db_verification_entries: {rows.Value?.Count ?? 0}."
            : $"Database error: {rows.Error.Message}";

        return Layout.Vertical(
            new object?[]
            {
                Text.H2("Database check"),
                Text.P(
                    "Table db_verification_entries is created by EF migrations on startup. Add a row here to confirm read/write against the same database your app uses."),
                Text.Block(status),
                Text.P("Note"),
                note.ToTextInput("Text to store"),
                new Button(
                    "Save row",
                    async () =>
                    {
                        var text = string.IsNullOrWhiteSpace(note.Value)
                            ? $"ping {DateTimeOffset.UtcNow:O}"
                            : note.Value.Trim();

                        await using var db = dbFactory.CreateDbContext();
                        db.DbVerificationEntries.Add(new DbVerificationEntry
                        {
                            Message = text,
                            CreatedAt = DateTimeOffset.UtcNow
                        });
                        await db.SaveChangesAsync();
                        rows.Mutator.Revalidate();
                    },
                    ButtonVariant.Primary),
                Text.P("Saved rows (newest first)"),
                Text.Markdown(BuildTable(rows.Value ?? []))
            });
    }

    private static string BuildTable(IReadOnlyList<RowVm> items)
    {
        if (items.Count == 0)
        {
            return "_No rows yet — add one above._";
        }

        var lines = new List<string>
        {
            "| Id | Created | Message |",
            "| --- | --- | --- |"
        };

        foreach (var r in items)
        {
            lines.Add($"| `{r.Id}` | {r.CreatedAt} | {EscapePipes(r.Message)} |");
        }

        return string.Join('\n', lines);
    }

    private static string EscapePipes(string value) => value.Replace("|", "\\|").Replace("\n", " ");

    private sealed record RowVm(Guid Id, string Message, string CreatedAt);
}
