using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace TruequeU.Models
{
    public class Favorites
    {
        [Key, Column(Order = 0)]
        public Guid ClientId { get; set; }

        [Key, Column(Order = 1)]
        public Guid ListingId { get; set; }

        [ForeignKey("ClientId")]
        public Clients? Client { get; set; }

        [ForeignKey("ListingId")]
        public Listings? Listing { get; set; }
    }
}
