using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TestIvyInit.Connections.TestIvyInit.Models;

namespace TestIvyInit.Connections.TestIvyInit;

public partial class TestIvyInitContext : DbContext
{
    public TestIvyInitContext(DbContextOptions<TestIvyInitContext> options)
        : base(options)
    {
    }

    public virtual DbSet<GithubStargazer> GithubStargazers { get; set; }

    public virtual DbSet<GithubStargazersDaily> GithubStargazersDailies { get; set; }

    public virtual DbSet<GithubStarsHistory> GithubStarsHistories { get; set; }

    public virtual DbSet<IvyAskQuestion> IvyAskQuestions { get; set; }

    public virtual DbSet<IvyAskTestResult> IvyAskTestResults { get; set; }

    public virtual DbSet<IvyAskTestRun> IvyAskTestRuns { get; set; }

    public virtual DbSet<NugetHistory> NugetHistories { get; set; }

    public virtual DbSet<PersistenceCheckItem> PersistenceCheckItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GithubStargazer>(entity =>
        {
            entity.HasKey(e => new { e.RepoName, e.UserLogin }).HasName("github_stargazers_pkey");
        });

        modelBuilder.Entity<GithubStargazersDaily>(entity =>
        {
            entity.HasKey(e => new { e.RepoName, e.Date }).HasName("github_stargazers_daily_pkey");
        });

        modelBuilder.Entity<GithubStarsHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("github_stars_history_pkey");

            entity.Property(e => e.Date).HasDefaultValueSql("CURRENT_DATE");
        });

        modelBuilder.Entity<IvyAskQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ivy_ask_questions_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Category).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Source).HasDefaultValueSql("'manual'::character varying");
        });

        modelBuilder.Entity<IvyAskTestResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ivy_ask_test_results_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.ResponseText).HasDefaultValueSql("''::text");

            entity.HasOne(d => d.Question).WithMany(p => p.IvyAskTestResults).HasConstraintName("ivy_ask_test_results_QuestionId_fkey");

            entity.HasOne(d => d.TestRun).WithMany(p => p.IvyAskTestResults).HasConstraintName("ivy_ask_test_results_TestRunId_fkey");
        });

        modelBuilder.Entity<IvyAskTestRun>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ivy_ask_test_runs_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Concurrency).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.DifficultyFilter).HasDefaultValueSql("'all'::character varying");
            entity.Property(e => e.Environment).HasDefaultValueSql("'production'::character varying");
            entity.Property(e => e.IvyVersion).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.StartedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<NugetHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("nuget_history_pkey");

            entity.Property(e => e.Date).HasDefaultValueSql("CURRENT_DATE");
        });

        modelBuilder.Entity<PersistenceCheckItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("persistence_check_pkey");
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
