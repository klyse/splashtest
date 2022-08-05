using Microsoft.EntityFrameworkCore;
using web.Models;

namespace web.Db;

public class SplashContext : DbContext
{
    public SplashContext(DbContextOptions<SplashContext> options) : base(options)
    {
    }

    public DbSet<Test> Tests => Set<Test>();
    public DbSet<Run> Runs => Set<Run>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SplashContext).Assembly);
    }
}