using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TruequeU.Models;

namespace TruequeU.Persistence
{

    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Listings> Listings { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        public DbSet<ModerationLog> ModerationLogs { get; set; }
        public DbSet<ListingImage> ListingImages { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Estos métodos son para evitar borrar en cascada y asi mantener el historial, ya que estas tablas están apuntando varias veces con FKs a una misma tabla.

            // Si no se colocan, no se pueden hacer las migraciones

            modelBuilder.Entity<Favorites>()
                .HasKey(f => new { f.ClientId, f.ListingId });

            //modelBuilder.Entity<Favorites>()
            //    .HasOne(f => f.Client)
            //    .WithMany()
            //    .HasForeignKey(f => f.ClientId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Favorites>()
            //    .HasOne(f => f.Listing)
            //    .WithMany()
            //    .HasForeignKey(f => f.ListingId)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Buyer)
                .WithMany()
                .HasForeignKey(c => c.BuyerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Seller)
                .WithMany()
                .HasForeignKey(c => c.SellerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Reporter)
                .WithMany()
                .HasForeignKey(r => r.ReportedBy)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.ReportedUser)
                .WithMany()
                .HasForeignKey(r => r.ReportedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.ReportedListing)
                .WithMany()
                .HasForeignKey(r => r.ReportedListingId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Favorites>()
                 .HasOne(f => f.Client)
                 .WithMany()
                 .HasForeignKey(f => f.ClientId)
                 .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Favorites>()
                .HasOne(f => f.Listing)
                .WithMany()
                .HasForeignKey(f => f.ListingId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ListingImage>()
                .HasOne(li => li.Listing)
                .WithMany(l => l.Images) //para navegación bidireccional y "join" con listing
                .HasForeignKey(li => li.ListingID)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }

}
