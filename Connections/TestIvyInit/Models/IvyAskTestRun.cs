using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestIvyInit.Connections.TestIvyInit.Models;

[Table("ivy_ask_test_runs")]
[Index("IvyVersion", Name = "ix_test_runs_ivy_version")]
public partial class IvyAskTestRun
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)]
    public string IvyVersion { get; set; } = null!;

    [StringLength(50)]
    public string Environment { get; set; } = null!;

    public int TotalQuestions { get; set; }

    public int SuccessCount { get; set; }

    public int NoAnswerCount { get; set; }

    public int ErrorCount { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    [StringLength(20)]
    public string DifficultyFilter { get; set; } = null!;

    [StringLength(10)]
    public string Concurrency { get; set; } = null!;

    [InverseProperty("TestRun")]
    public virtual ICollection<IvyAskTestResult> IvyAskTestResults { get; set; } = new List<IvyAskTestResult>();
}
