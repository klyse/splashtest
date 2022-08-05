using Microsoft.EntityFrameworkCore;

namespace web.Db;

public class SplashContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SplashContext).Assembly);
    }
}