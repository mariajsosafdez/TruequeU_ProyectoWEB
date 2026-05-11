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
        //public DbSet<Report> Reports { get; set; }

        public DbSet<Clients> Clients { get; set; }
        public DbSet<Listings> Listings { get; set; }

        //TODO Mio, borrar posiblemente 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Buyer)
                .WithMany()
                .HasForeignKey(c => c.BuyerId)
                .OnDelete(DeleteBehavior.NoAction);  // Evitar borrar en cascada 

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Seller)
                .WithMany()
                .HasForeignKey(c => c.SellerId)
                .OnDelete(DeleteBehavior.NoAction);  //Evitar borrar en cascada
        }

    }

}
