using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestIvyInit.Connections.TestIvyInit.Models;

[Table("github_stars_history")]
[Index("RepoName", "Date", Name = "github_stars_history_repo_name_date_key", IsUnique = true)]
public partial class GithubStarsHistory
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("repo_name")]
    public string? RepoName { get; set; }

    [Column("stars")]
    public long? Stars { get; set; }

    [Column("date")]
    public DateOnly? Date { get; set; }
}
