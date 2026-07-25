using ClosetCompanionApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace ClosetCompanionApp.Repository
{
    public class AppDbContext : DbContext
    {
        public DbSet<Garment> Garments { get; set; }
        public DbSet<PosePhoto> PosePhoto { get; set; }
        public DbSet<GeneratedOutfit> GeneratedOutfits { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Garment>().HasKey(g => g.Id);
            modelBuilder.Entity<PosePhoto>().HasKey(p => p.Id);
            modelBuilder.Entity<GeneratedOutfit>().HasKey(o => o.Id);
        }
    }
}
