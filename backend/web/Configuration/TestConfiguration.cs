using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using web.Models;

namespace web.Configuration;

public class TestConfiguration : IEntityTypeConfiguration<Test>
{
    public void Configure(EntityTypeBuilder<Test> builder)
    {
        builder.ToTable("Tests");

        builder.HasKey(c => c.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.TestSteps)
            .HasColumnType("jsonb");

        builder.HasMany(c => c.Runs)
            .WithOne(c => c.Test)
            .HasForeignKey(c => c.TestId);
    }
}