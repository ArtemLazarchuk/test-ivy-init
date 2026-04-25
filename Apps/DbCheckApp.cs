using Ivy;
using Microsoft.EntityFrameworkCore;
using TestIvyInit.Connections.TestIvyInit;

namespace TestIvyInit.Apps;

[App(title: "DB Check", group: ["Debug"])]
public class DbCheckApp : AppBase
{
    public override object? Build()
    {
        var dbFactory = UseService<TestIvyInitContextFactory>();
        var sampleRows = UseQuery(
            "db-check.nuget-history",
            async ct =>
            {
                await using var db = dbFactory.CreateDbContext();
                return await db.NugetHistories
                    .AsNoTracking()
                    .OrderByDescending(x => x.Date)
                    .Take(20)
                    .Select(x => new NugetRow(
                        x.Id,
                        x.Date.HasValue ? x.Date.Value.ToString("yyyy-MM-dd") : "-",
                        x.PackageName ?? "-",
                        x.Downloads))
                    .ToListAsync(ct);
            });

        var statusLine = sampleRows.Error is null
            ? $"DB status: connected. Loaded {sampleRows.Value?.Count ?? 0} rows."
            : $"DB status: failed. {sampleRows.Error.Message}";

        var markdown = BuildTable(sampleRows.Value ?? []);

        return Layout.Vertical(
            new object?[]
            {
                Text.H2("Database check"),
                Text.P("Simple runtime check for TestIvyInit PostgreSQL connection."),
                Text.Block(statusLine),
                Text.Markdown(markdown)
            });
    }

    private static string BuildTable(IReadOnlyList<NugetRow> rows)
    {
        if (rows.Count == 0)
        {
            return "No rows returned from `nuget_history`.";
        }

        var lines = new List<string>
        {
            "| Id | Date | Package | Downloads |",
            "| --- | --- | --- | ---: |"
        };

        lines.AddRange(rows.Select(row =>
            $"| {row.Id} | {row.Date} | {EscapePipes(row.PackageName)} | {row.Downloads?.ToString() ?? "-"} |"));

        return string.Join('\n', lines);
    }

    private static string EscapePipes(string value) => value.Replace("|", "\\|");

    private sealed record NugetRow(int Id, string Date, string PackageName, long? Downloads);
}
