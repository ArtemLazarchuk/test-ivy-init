using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestIvyInit.Connections.TestIvyInit.Models;

[PrimaryKey("RepoName", "Date")]
[Table("github_stargazers_daily")]
public partial class GithubStargazersDaily
{
    [Key]
    [Column("repo_name")]
    public string RepoName { get; set; } = null!;

    [Key]
    [Column("date")]
    public DateOnly Date { get; set; }

    [Column("new_count")]
    public int? NewCount { get; set; }

    [Column("unstar_count")]
    public int? UnstarCount { get; set; }

    [Column("reactivated_count")]
    public int? ReactivatedCount { get; set; }
}
