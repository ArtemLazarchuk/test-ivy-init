using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestIvyInit.Connections.TestIvyInit.Models;

[Table("nuget_history")]
[Index("PackageName", "Date", Name = "nuget_history_package_name_date_key", IsUnique = true)]
public partial class NugetHistory
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("package_name")]
    public string? PackageName { get; set; }

    [Column("downloads")]
    public long? Downloads { get; set; }

    [Column("date")]
    public DateOnly? Date { get; set; }
}
