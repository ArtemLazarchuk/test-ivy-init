using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestIvyInit.Connections.TestIvyInit.Models;

[PrimaryKey("RepoName", "UserLogin")]
[Table("github_stargazers")]
public partial class GithubStargazer
{
    [Key]
    [Column("repo_name")]
    public string RepoName { get; set; } = null!;

    [Key]
    [Column("user_login")]
    public string UserLogin { get; set; } = null!;

    [Column("unstarred_at")]
    public DateTime? UnstarredAt { get; set; }

    [Column("starred_at")]
    public DateTime? StarredAt { get; set; }
}
