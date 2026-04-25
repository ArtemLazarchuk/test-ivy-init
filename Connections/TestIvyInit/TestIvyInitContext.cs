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

    public DbSet<SimpleNote> SimpleNotes => Set<SimpleNote>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SimpleNote>(entity =>
        {
            entity.ToTable("simple_notes");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Text).HasMaxLength(2000).IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
