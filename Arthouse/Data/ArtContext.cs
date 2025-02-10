using ArtHouse.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtHouse.Data
{
    public class ArtContext : DbContext
    {
        public ArtContext(DbContextOptions<ArtContext> options)
            : base(options)
        {

        }
        public DbSet<ArtType> ArtTypes { get; set; }
        public DbSet<Artwork> Artworks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Prevent Cascade Delete from Doctor to Patient
            //so we are prevented from deleting a Doctor with
            //Patients assigned
            modelBuilder.Entity<ArtType>()
                .HasMany(p => p.Artworks)
                .WithOne(d => d.ArtType)
                .OnDelete(DeleteBehavior.Restrict);

            //Add a unique index to Artwork
            modelBuilder.Entity<Artwork>()
            .HasIndex(p => new { p.Name, p.Completed, p.ArtTypeID })
            .IsUnique();

        }

    }
}
