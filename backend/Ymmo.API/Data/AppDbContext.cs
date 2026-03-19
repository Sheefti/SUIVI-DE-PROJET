using Microsoft.EntityFrameworkCore;
using Ymmo.API.Models;

namespace Ymmo.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Utilisateur> Utilisateurs { get; set; }
    public DbSet<Agence> Agences { get; set; }
    public DbSet<Bien> Biens { get; set; }
    public DbSet<Photo> Photos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Agence
        modelBuilder.Entity<Agence>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Nom).IsRequired().HasMaxLength(100);
            entity.Property(a => a.Ville).IsRequired().HasMaxLength(100);
            entity.Property(a => a.CodePostal).HasMaxLength(10);
        });

        // Utilisateur
        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Role).HasConversion<string>();

            entity.HasOne(u => u.Agence)
                  .WithMany(a => a.Utilisateurs)
                  .HasForeignKey(u => u.AgenceId);
        });

        // Bien
        modelBuilder.Entity<Bien>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Titre).IsRequired().HasMaxLength(200);
            entity.Property(b => b.Prix).HasColumnType("decimal(12,2)");
            entity.Property(b => b.Type).HasConversion<string>();
            entity.Property(b => b.Statut).HasConversion<string>();

            entity.HasOne(b => b.Agence)
                  .WithMany(a => a.Biens)
                  .HasForeignKey(b => b.AgenceId);
        });

        // Photo
        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Url).IsRequired();

            entity.HasOne(p => p.Bien)
                  .WithMany(b => b.Photos)
                  .HasForeignKey(p => p.BienId);
        });
    }
}