using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TestIvyInit.Connections.TestIvyInit.Models;

[Table("ivy_ask_questions")]
public partial class IvyAskQuestion
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string Widget { get; set; } = null!;

    [StringLength(100)]
    public string Category { get; set; } = null!;

    [StringLength(10)]
    public string Difficulty { get; set; } = null!;

    public string QuestionText { get; set; } = null!;

    [StringLength(20)]
    public string Source { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }

    [InverseProperty("Question")]
    public virtual ICollection<IvyAskTestResult> IvyAskTestResults { get; set; } = new List<IvyAskTestResult>();
}
