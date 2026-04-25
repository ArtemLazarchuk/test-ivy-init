using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestIvyInit.Connections.TestIvyInit.Models;

[Table("ivy_ask_test_results")]
[Index("QuestionId", Name = "ix_test_results_question_id")]
[Index("TestRunId", Name = "ix_test_results_run_id")]
public partial class IvyAskTestResult
{
    [Key]
    public Guid Id { get; set; }

    public Guid TestRunId { get; set; }

    public Guid QuestionId { get; set; }

    public string ResponseText { get; set; } = null!;

    public int ResponseTimeMs { get; set; }

    public bool IsSuccess { get; set; }

    public int HttpStatus { get; set; }

    [StringLength(500)]
    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; }

    [ForeignKey("QuestionId")]
    [InverseProperty("IvyAskTestResults")]
    public virtual IvyAskQuestion Question { get; set; } = null!;

    [ForeignKey("TestRunId")]
    [InverseProperty("IvyAskTestResults")]
    public virtual IvyAskTestRun TestRun { get; set; } = null!;
}
