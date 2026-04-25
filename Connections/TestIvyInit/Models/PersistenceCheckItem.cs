using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestIvyInit.Connections.TestIvyInit.Models;

/// <summary>Minimal table used only to verify that reads/writes hit the real database.</summary>
[Table("persistence_check")]
public class PersistenceCheckItem
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("message")]
    [MaxLength(2000)]
    public string Message { get; set; } = "";

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}
