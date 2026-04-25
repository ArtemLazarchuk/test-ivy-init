using Ivy;
using Microsoft.EntityFrameworkCore;
using TestIvyInit.Connections.TestIvyInit;
using TestIvyInit.Connections.TestIvyInit.Models;

namespace TestIvyInit.Apps;

[App(title: "Simple notes", group: ["Examples"])]
public class SimpleNotesApp : AppBase
{
    public override object? Build()
    {
        var dbFactory = UseService<TestIvyInitContextFactory>();

        var notes = UseQuery(
            "simple-notes.all",
            async ct =>
            {
                await using var db = dbFactory.CreateDbContext();
                return await db.SimpleNotes
                    .AsNoTracking()
                    .OrderByDescending(n => n.UpdatedAt)
                    .Select(n => new NoteRow(n.Id, n.Text, n.UpdatedAt.ToString("yyyy-MM-dd HH:mm")))
                    .ToListAsync(ct);
            });

        var newText = UseState("");
        var editingId = UseState<int?>(null);
        var editText = UseState("");

        var children = new List<object?>
        {
            Text.H2("Нотатки"),
            Text.P("Додай рядок або натисни «Редагувати», зміни текст і «Зберегти зміни» — список оновлюється одразу."),
            Text.H4("Нова нотатка"),
            Layout.Horizontal(newText.ToTextInput("Текст…"), new Button("Додати", async () =>
            {
                var t = (newText.Value ?? "").Trim();
                if (string.IsNullOrEmpty(t)) return;

                await using var db = dbFactory.CreateDbContext();
                db.SimpleNotes.Add(new SimpleNote { Text = t, UpdatedAt = DateTimeOffset.UtcNow });
                await db.SaveChangesAsync();
                newText.Set("");
                notes.Mutator.Revalidate();
            })),
            Text.H4("Список"),
        };

        if (notes.Error != null)
            children.Add(Text.Danger(notes.Error.Message));
        else
            children.Add(Text.Markdown(BuildTable(notes.Value ?? [])));

        children.Add(Text.H4("Редагування"));
        children.Add(editText.ToTextInput("Текст для вибраного рядка…"));
        children.Add(Layout.Horizontal(
            new Button("Зберегти зміни", async () =>
            {
                if (!editingId.Value.HasValue) return;
                var t = (editText.Value ?? "").Trim();
                if (string.IsNullOrEmpty(t)) return;

                await using var db = dbFactory.CreateDbContext();
                var row = await db.SimpleNotes.FirstOrDefaultAsync(n => n.Id == editingId.Value.Value);
                if (row == null) return;

                row.Text = t;
                row.UpdatedAt = DateTimeOffset.UtcNow;
                await db.SaveChangesAsync();
                editingId.Set(null);
                editText.Set("");
                notes.Mutator.Revalidate();
            }, ButtonVariant.Primary),
            new Button("Скасувати", () =>
            {
                editingId.Set(null);
                editText.Set("");
            })));

        if (notes.Value is { Count: > 0 } list)
        {
            children.Add(Text.H4("Обрати рядок"));
            foreach (var row in list)
            {
                var r = row;
                children.Add(Layout.Horizontal(
                    Text.Inline($"#{r.Id} — {r.UpdatedAtLabel}"),
                    new Button("Редагувати", () =>
                    {
                        editingId.Set(r.Id);
                        editText.Set(r.Text);
                    })));
            }
        }

        return Layout.Vertical(children);
    }

    private static string BuildTable(IReadOnlyList<NoteRow> rows)
    {
        if (rows.Count == 0)
            return "_Поки порожньо — додай перший рядок вище._";

        var lines = new List<string>
        {
            "| # | Оновлено | Текст |",
            "| --- | --- | --- |"
        };
        foreach (var r in rows)
        {
            var safe = r.Text.Replace("|", "\\|").Replace("\n", " ");
            lines.Add($"| {r.Id} | {r.UpdatedAtLabel} | {safe} |");
        }

        return string.Join('\n', lines);
    }

    private sealed record NoteRow(int Id, string Text, string UpdatedAtLabel);
}
