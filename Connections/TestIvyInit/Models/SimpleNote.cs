namespace TestIvyInit.Connections.TestIvyInit.Models;

public class SimpleNote
{
    public int Id { get; set; }

    /// <summary>Max length enforced in EF configuration.</summary>
    public string Text { get; set; } = "";

    public DateTimeOffset UpdatedAt { get; set; }
}
