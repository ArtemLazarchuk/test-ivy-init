using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TestIvyInit.Connections.TestIvyInit;

public partial class TestIvyInitContext : DbContext
{
    public TestIvyInitContext(DbContextOptions<TestIvyInitContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
