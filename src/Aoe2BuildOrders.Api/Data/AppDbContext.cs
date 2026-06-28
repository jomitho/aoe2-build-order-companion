using Aoe2BuildOrders.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Aoe2BuildOrders.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<BuildOrder> BuildOrders => Set<BuildOrder>();
    public DbSet<BuildOrderStep> BuildOrderSteps => Set<BuildOrderStep>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BuildOrder>(entity =>
        {
            entity.HasKey(buildOrder => buildOrder.Id);

            entity.Property(buildOrder => buildOrder.Name)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(buildOrder => buildOrder.Civilization)
                .IsRequired()
                .HasMaxLength(80);

            entity.Property(buildOrder => buildOrder.StrategyType)
                .IsRequired()
                .HasMaxLength(80);

            entity.Property(buildOrder => buildOrder.Difficulty)
                .IsRequired()
                .HasMaxLength(40);

            entity.Property(buildOrder => buildOrder.Description)
                .HasMaxLength(500);

            entity.HasMany(buildOrder => buildOrder.Steps)
                .WithOne()
                .HasForeignKey(step => step.BuildOrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BuildOrderStep>(entity =>
        {
            entity.HasKey(step => step.Id);

            entity.Property(step => step.Age)
                .IsRequired()
                .HasMaxLength(40);

            entity.Property(step => step.Instruction)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(step => step.ResourceFocus)
                .HasMaxLength(80);

            entity.Property(step => step.Notes)
                .HasMaxLength(300);
        });
    }
}
