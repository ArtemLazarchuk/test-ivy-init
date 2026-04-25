using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TestIvyInit.Connections.TestIvyInit.Models;

/// <summary>Smoke-test table: created by migrations, rows added from the DB Check app.</summary>
[Table("db_verification_entries")]
public partial class DbVerificationEntry
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("message")]
    [MaxLength(1000)]
    public string Message { get; set; } = "";

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}
