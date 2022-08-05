using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using web.Models;

namespace web.Configuration;

public class RunConfiguration : IEntityTypeConfiguration<Run>
{
    public void Configure(EntityTypeBuilder<Run> builder)
    {
        builder.ToTable("Runs");

        builder.HasKey(c => c.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();
    }
}