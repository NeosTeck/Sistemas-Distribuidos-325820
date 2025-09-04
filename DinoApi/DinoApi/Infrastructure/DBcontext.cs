using Microsoft.EntityFrameworkCore;
using DinosaurApi.Infrastructure.Entities;
namespace DinoApi.Infrastructure;

public class DinoDbContext : DbContext
{
    public DbSet<DinoEntity> Dinos { get; set; } = null!;

    public DinoDbContext(DbContextOptions<DinoDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DinoEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Nombre).IsUnique();
            entity.Property(e => e.Orden).HasMaxLength(50);
            entity.Property(e => e.Postura).HasMaxLength(50);
            entity.Property(e => e.PeriodoPpl).HasMaxLength(50);
            entity.Property(e => e.Dieta).HasMaxLength(50);
            entity.Property(e => e.Continente).HasMaxLength(50);
        });
    }
}
