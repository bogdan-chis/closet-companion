using ClosetCompanionApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace ClosetCompanionApp.Repository
{
    public class AppDbContext : DbContext
    {
        public DbSet<Garment> Garments { get; set; }
        public DbSet<PosePhoto> PosePhoto { get; set; }
        public DbSet<GeneratedOutfit> GeneratedOutfits { get; set; }
        public DbSet<OutfitGarment> OutfitGarments { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Garment>().HasKey(g => g.Id);
            modelBuilder.Entity<PosePhoto>().HasKey(p => p.Id);

            modelBuilder.Entity<GeneratedOutfit>().HasKey(o => o.Id);
            modelBuilder.Entity<GeneratedOutfit>().Ignore(o => o.SelectedGarmentIds);

            // Pose → Outfit: real FK, DB-enforced cascade.
            modelBuilder.Entity<GeneratedOutfit>()
                .HasOne<PosePhoto>()
                .WithMany()
                .HasForeignKey(o => o.BasePhotoId)
                .OnDelete(DeleteBehavior.Cascade);

            // The actual join table — atomic rows, real FKs both ways.
            modelBuilder.Entity<OutfitGarment>(entity =>
            {
                entity.HasKey(og => new { og.GeneratedOutfitId, og.GarmentId });

                entity.HasOne<GeneratedOutfit>()
                    .WithMany()
                    .HasForeignKey(og => og.GeneratedOutfitId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<Garment>()
                    .WithMany()
                    .HasForeignKey(og => og.GarmentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}