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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Estos métodos son para evitar borrar en cascada y asi mantener el historial, ya que estas tablas están apuntando varias veces con FKs a una misma tabla

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
        }

    }

}
